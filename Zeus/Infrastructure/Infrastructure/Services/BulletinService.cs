using System;
using System.Collections.Generic;
using System.Linq;

using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.Models;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.IO;
using System.Xml;

namespace Employment.Web.Mvc.Infrastructure.Services
{
    /// <summary>
    /// Defines a service for accessing context information data
    /// </summary>
    public class BulletinService : Service, IBulletinService
    {
        /// <summary>
        /// Sql service for interacting with a Sql database.
        /// </summary>
        protected readonly ISqlService SqlService;

        private readonly TimeSpan defaultTimeSpan;

        /// <summary>
        /// Configuration manager for interacting with the Web configuration.
        /// </summary>
        protected readonly IConfigurationManager ConfigurationManager;

        private readonly string connectionName = "DbConn_AjsCms";

        // Context
        private const string SiteCode = "BULLETINS";

        private readonly short SiteID;

        /// <summary>
        /// Initializes a new instance of the <see cref="BulletinService" /> class.
        /// </summary>
        /// <param name="sqlService">Sql service for interacting with a Sql database.</param>
        /// <param name="configurationManager">Configuration manager for interacting with the Web configuration.</param>
        /// <param name="client">Client for interacting with WCF services.</param>
        /// <param name="cacheService">Cache service for interacting with cached data.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sqlService" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="configurationManager" /> is <c>null</c>.</exception>
        public BulletinService(IClient client, IConfigurationManager configurationManager,  ICacheService cacheService, ISqlService sqlService)
            : base(client,  cacheService)
        {
            if (sqlService == null)
            {
                throw new ArgumentNullException("sqlService");
            }

            SqlService = sqlService;

            if (configurationManager == null)
            {
                throw new ArgumentNullException("configurationManager");
            }

            ConfigurationManager = configurationManager;


            String timeoutInMinutesStringValue = configurationManager.AppSettings["BulletinsCacheTimeoutInMinutes"];

            Int32 timeoutInMinutes;
            const Int32 defaultTimeoutInMinutes = 5;
            
            defaultTimeSpan = !String.IsNullOrEmpty(timeoutInMinutesStringValue) &&
                              Int32.TryParse(timeoutInMinutesStringValue, out timeoutInMinutes)
                                  ? new TimeSpan(0, timeoutInMinutes, 0)
                                  : new TimeSpan(0, defaultTimeoutInMinutes, 0);

            SiteID = GetSiteID();
        }

        /// <summary>
        /// Get bulletin details for specified ID.
        /// </summary>
        /// <param name="bulletinID">The bulletin ID.</param>
        /// <returns>The bulletin details.</returns>
        public BulletinModel Get(int bulletinID)
        {
            var key = new KeyModel(CacheType.Global, "BulletinItem").Add(bulletinID);
            BulletinModel data = null;
            if (!CacheService.TryGet(key, out data))
            {
                var sqlParameters = new List<SqlParameter>();

                sqlParameters.Add(new SqlParameter {ParameterName = "@SiteId", SqlDbType = SqlDbType.VarChar, Value = SiteID});
                sqlParameters.Add(new SqlParameter {ParameterName = "@PageId", SqlDbType = SqlDbType.Int, Value = bulletinID});

                data = SqlService.Execute<BulletinModel>(connectionName, "AjsCmsPageGet", sqlParameters,
                    reader =>
                    {
                        var contentPage = new BulletinModel();
                        contentPage.Url = reader.GetString(0).ToLower();
                        contentPage.Title = reader.GetString(1);
                        contentPage.PageId = reader.GetInt32(2);
                        contentPage.Html = reader[10] as string;
                        contentPage.HtmlExtended = reader[11] as string;
                        contentPage.ExtensionData = reader[12] as string;
                        contentPage.ExtensionType = reader[13] as string;
                        contentPage.LiveDate = reader.GetDateTime(15);
                        if (!string.IsNullOrEmpty(contentPage.ExtensionData) && contentPage.ExtensionType == "BULLETIN")
                        {
                            contentPage.BulletinContracts = Deserialize<BulletinExtensionData>(contentPage.ExtensionData).Contracts;
                        }
                        else
                        {
                            return null;
                        }
                        return contentPage;
                    }
                    ).FirstOrDefault();

                if (data == null)
                {
                    throw new DataException(String.Format("Bulletin data is not available for the bulletin code - {0}", bulletinID));
                }

                data.Html = ReplaceContent(data.Html);
                data.HtmlExtended = ReplaceContent(data.HtmlExtended);

                CacheService.Set(key, data, defaultTimeSpan);
            }
            if (data == null)
            {
                throw new DataException(String.Format("Bulletin data is not available for the bulletin code - {0}", bulletinID));
            }

            return data;
        }

        /// <summary>
        /// Get all bulletins of the specified type.
        /// </summary>
        /// <param name="bulletinType">The bulletin type.</param>
        /// <param name="limit">The maximum number of bulletins to get.</param>
        /// <returns>Bulletins of the specified type.</returns>
        public IList<BulletinModel> List(BulletinType bulletinType, int limit)
        {
            var key = new KeyModel(CacheType.Global, "BulletinList");

            IList<BulletinModel> data;

            if (!CacheService.TryGet(key, out data))
            {
                var sqlParameters = new List<SqlParameter>
                                        {
                                            new SqlParameter
                                                {
                                                    ParameterName = "@SiteId",
                                                    SqlDbType = SqlDbType.VarChar,
                                                    Value = SiteID
                                                }
                                        };

                var bulletinList = SqlService.Execute<BulletinModel>(connectionName, "AjsCmsPageGetAll", sqlParameters,
                    reader =>
                    {
                        var contentPage = new BulletinModel();
                        contentPage.Url = reader.GetString(0).ToLower();
                        contentPage.Title = reader.GetString(1);
                        contentPage.PageId = reader.GetInt32(2);
                        contentPage.ExtensionType = reader[10] as string;
                        contentPage.LiveDate = reader.GetDateTime(12);
                        contentPage.ExtensionData = reader[14] as string;
                        return contentPage;
                    }                    
                    ).ToList();

                foreach (var bulletin in bulletinList)
                {
                    if (bulletin.ExtensionType == "BULLETIN" && !string.IsNullOrEmpty(bulletin.ExtensionData))
                    {
                        var contractData = Deserialize<BulletinExtensionData>(bulletin.ExtensionData);

                        bulletin.BulletinContracts = contractData != null ? contractData.Contracts : null;
                    }
                }

                data = bulletinList.OrderByDescending(b => b.LiveDate).ToList();

                if (data.Any())
                {
                    CacheService.Set(key, data, defaultTimeSpan);
                }
            }

            // Filter by contract
            data = data.AsParallel().Where(b => b.BulletinContracts != null && b.BulletinContracts.Any(c => c.Equals(bulletinType.ToString(), StringComparison.OrdinalIgnoreCase))).ToList();

            // Then apply limit
            if (limit > 0)
            {
                data = data.Take(limit).ToList();
            }

            return data;
        }

        private short GetSiteID()
        {
            KeyModel key = new KeyModel(CacheType.Global, "BulletinSiteId");
            short siteID = 0;
            if (!CacheService.TryGet(key, out siteID))
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();

                SqlParameter siteIdParameter = new SqlParameter {ParameterName = "@SiteId", Direction = ParameterDirection.Output, Value = 0};

                sqlParameters.Add(siteIdParameter);
                sqlParameters.Add(new SqlParameter {ParameterName = "@SiteCode", SqlDbType = SqlDbType.VarChar, Value = SiteCode});

                SqlService.ExecuteNonQuery(connectionName, "AjsCmsSiteGet", sqlParameters);

                if (siteIdParameter.Value == null)
                {
                    throw new Exception(string.Format("Could not find Site ID for {0}", SiteCode));
                }

                short.TryParse(siteIdParameter.Value.ToString(), out siteID);

                if (siteID>0)
                {
                    CacheService.Set(key, siteID, defaultTimeSpan);
                }
            }
            return siteID;
        }

        /// <summary>
        /// This is used in lower environments to replace prod URLs in the content with ones from the current environment.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        internal string ReplaceContent(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return content;
            }

            if (ConfigurationManager.AppSettings.Get("CmsBulletinsUrlReplacement") == "true")
            {
                if (ConfigurationManager.AppSettings.Get("CmsBulletinsUrlProd") != ConfigurationManager.AppSettings.Get("CmsBulletinsUrlCurrent"))
                {
                    content = ReplaceEx(content, ConfigurationManager.AppSettings.Get("CmsBulletinsUrlProd"), ConfigurationManager.AppSettings.Get("CmsBulletinsUrlCurrent"), StringComparison.OrdinalIgnoreCase);
                }
            }

            return content;
        }

        /// <summary>
        /// Replaces the string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="comparison">The comparison.</param>
        /// <returns></returns>
        internal string ReplaceEx(string str, string oldValue, string newValue, StringComparison comparison)
        {
            if (string.IsNullOrEmpty(oldValue))
            {
                return str;
            }

            var sb = new StringBuilder();

            int previousIndex = 0;
            int index = str.IndexOf(oldValue, comparison);

            while (index != -1)
            {
                sb.Append(str.Substring(previousIndex, index - previousIndex));
                sb.Append(newValue);
                index += oldValue.Length;

                previousIndex = index;
                index = str.IndexOf(oldValue, index, comparison);
            }

            sb.Append(str.Substring(previousIndex));

            return sb.ToString();
        }

        /// <summary>
        /// DeSerialize an object
        /// </summary>
        internal T Deserialize<T>(string xmlOfAnObject)
        {
            using (var read = new StringReader(xmlOfAnObject))
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));

                using (XmlReader reader = new XmlTextReader(read))
                {
                    T myObject = (T)serializer.Deserialize(reader);

                    return myObject;
                }
            }
        }
    }
}
