using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Employment.Web.Mvc.Area.Example.Service.Interfaces;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Area.Example.Service.Implementation
{
    /// <summary>
    /// A service to demonstrate using ICaGenService
    /// </summary>
    /// <remarks>
    /// This is for example purposes only. A service should only ever exist in the Service projects.
    /// </remarks>
    public class DataAccessService : Infrastructure.Services.Service, IDataAccessService
    {
        private readonly ISqlService SqlService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessService" /> class.
        /// </summary>
        /// <param name="client">Client for interacting with WCF services.</param>
        /// <param name="cacheService">Cache service for interacting with cached data.</param>
        /// <param name="caGenService">The ca gen service.</param>
        /// <param name="sqlService">The SQL service.</param>
        public DataAccessService(IClient client, ICacheService cacheService,  ISqlService sqlService)
            : base(client, cacheService)
        {
            this.SqlService = sqlService;
        }


        /// <summary>
        /// Gets data from SQL server.
        /// </summary>
        /// <returns></returns>
        public DataAccessSqlServerModel GetSomeSqlServerData()
        {
            // Some SQL calls will benefit from using the ICacheService - you need to evaluate this for each call.

            // The name of the connection string to your database in the web.config
            string connectionName = "DbConn_AjsCms";

            // ---------------------------------------------------------------------------------------------------------
            // Example of executing a stored procedure - Retrieves the value of the Site ID for use in the next example.
            // ---------------------------------------------------------------------------------------------------------
            string SiteCode = "BULLETINS";
            short siteId = 0;

            SqlParameter siteCodeParameter = new SqlParameter() { ParameterName = "@SiteCode", SqlDbType = SqlDbType.VarChar, Value = SiteCode };
            SqlParameter siteIdParameter = new SqlParameter() { ParameterName = "@SiteId", Direction = ParameterDirection.Output, Value = 0 };

            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(siteCodeParameter); // Input parameter
            sqlParameters.Add(siteIdParameter); // Output parameter

            SqlService.ExecuteNonQuery(connectionName, "AjsCmsSiteGet", sqlParameters);
            if (siteIdParameter.Value == null)
            {
                throw new Exception(string.Format("Could not find Site ID for {0}", SiteCode));
            }
            short.TryParse(siteIdParameter.Value.ToString(), out siteId);

            // ---------------------------------------------------------------------------------------------------------
            // Example of executing a query - Returns a simple set of data
            // ---------------------------------------------------------------------------------------------------------
            var sqlParameters2 = new List<SqlParameter>{new SqlParameter{ParameterName = "@SiteId",SqlDbType = SqlDbType.VarChar,Value = siteId}};

            var bulletinList = SqlService.Execute<DataAccessSqlServerModel>(connectionName, "AjsCmsPageGetAll", sqlParameters2,
                reader =>
                {
                    var contentPage = new DataAccessSqlServerModel();
                    contentPage.BulletinTitle = reader.GetString(1);
                    contentPage.BulletinId = reader.GetInt32(2);
                    contentPage.BulletinLiveDate = reader.GetDateTime(12);
                    return contentPage;
                }
                ).ToList();

            // Just return the first bulletin we find for a demo.
            return bulletinList.FirstOrDefault();
        }

    }
}
