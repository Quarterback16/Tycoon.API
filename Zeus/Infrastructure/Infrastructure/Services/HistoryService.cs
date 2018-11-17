using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.Models;

namespace Employment.Web.Mvc.Infrastructure.Services
{
    /// <summary>
    /// Defines a service for accessing history data which is stored in SQL.
    /// </summary>
    public class HistoryService : Service, IHistoryService
    {
        /// <summary>
        /// Sql service for interacting with a Sql database.
        /// </summary>
        protected readonly ISqlService SqlService;

        /// <summary>
        /// Configuration manager for interacting with the Web configuration.
        /// </summary>
        protected readonly IConfigurationManager ConfigurationManager;

        /// <summary>
        /// Connection string name
        /// </summary>
        private const string connectionName = "DbConn_ZEUS";

        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryService"/> class.
        /// </summary>
        /// <param name="configurationManager">The configuration manager.</param>
        /// <param name="sqlService">Sql service for interacting with a Sql database.</param>
        /// <param name="client">Client for interacting with WCF services.</param>
        /// <param name="cacheService">Cache service for interacting with cached data.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="configurationManager"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sqlService"/> is <c>null</c>.</exception>
        public HistoryService(IConfigurationManager configurationManager, ISqlService sqlService, IClient client,  ICacheService cacheService) : base(client,  cacheService)
        {
            if (configurationManager == null)
            {
                throw new ArgumentNullException("configurationManager");
            }

            ConfigurationManager = configurationManager;

            if (sqlService == null)
            {
                throw new ArgumentNullException("sqlService");
            }

            SqlService = sqlService;
        }

        /// <summary>
        /// Set an activity into recent history storage.
        /// </summary>
        /// <param name="activityID">The activity ID.</param>
        /// <param name="displayName">The activity display name.</param>
        public void SetActivity(long activityID, string displayName)
        {
            Set(HistoryType.Activity, new Dictionary<string, object> { { "activityId", activityID } }, displayName);
        }

        /// <summary>
        /// Set a contract into recent history storage.
        /// </summary>
        /// <param name="contractID">The contract ID.</param>
        /// <param name="displayName">The contract display name.</param>
        public void SetContract(string contractID, string displayName)
        {
            Set(HistoryType.Contract, new Dictionary<string, object> { { "contractId", contractID } }, displayName);
        }

        /// <summary>
        /// Set a employer into recent history storage.
        /// </summary>
        /// <param name="employerID">The employer ID.</param>
        /// <param name="displayName">The employer display name.</param>
        public void SetEmployer(long employerID, string displayName)
        {
            Set(HistoryType.Employer, new Dictionary<string, object> { { "employerID", employerID } }, displayName);
        }

        /// <summary>
        /// Set a job seeker into recent history storage.
        /// </summary>
        /// <param name="jobSeekerID">The job seeker ID.</param>
        /// <param name="displayName">The job seeker display name.</param>
        public void SetJobSeeker(long jobSeekerID, string displayName)
        {
            Set(HistoryType.JobSeeker, new Dictionary<string, object> { { "jobseekerId", jobSeekerID } }, displayName);
        }
        
        /// <summary>
        /// Set a vacancy into recent history storage.
        /// </summary>
        /// <param name="vacancyID">The vacancy ID.</param>
        /// <param name="displayName">The vacancy display name.</param>
        public void SetVacancy(long vacancyID, string displayName)
        {
            Set(HistoryType.Vacancy, new Dictionary<string, object> { { "vacancyID", vacancyID } }, displayName);
        }

        /// <summary>
        /// Set an item into recent history storage.
        /// </summary>
        /// <param name="historyType">The type of history.</param>
        /// <param name="values">The values necessary for loading the object as a dictionary of key value pairs.</param>
        /// <param name="displayName">The object's display name.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is <c>null</c> or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="displayName"/> is <c>null</c> or empty.</exception>
        public void Set(HistoryType historyType, IDictionary<string, object> values, string displayName)
        {
            if (values == null || !values.Any())
            {
                throw new ArgumentNullException("values");
            }

            if (string.IsNullOrEmpty(displayName))
            {
                throw new ArgumentNullException("displayName");
            }

            var queryStringValues = values.ToQueryString(true);
            var sqlParameters = new List<SqlParameter>();

            sqlParameters.Add(new SqlParameter { ParameterName = "@return_value", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.ReturnValue });
            sqlParameters.Add(new SqlParameter { ParameterName = "@HistoryType_RHC", SqlDbType = SqlDbType.VarChar, Value = historyType.ToString() });
            sqlParameters.Add(new SqlParameter { ParameterName = "@ObjectValues", SqlDbType = SqlDbType.VarChar, Value = queryStringValues });
            sqlParameters.Add(new SqlParameter { ParameterName = "@DisplayName", SqlDbType = SqlDbType.VarChar, Value = displayName });
            sqlParameters.Add(new SqlParameter { ParameterName = "@IsPinned", SqlDbType = SqlDbType.Bit, Value = false });
            sqlParameters.Add(new SqlParameter { ParameterName = "@UserID", SqlDbType = SqlDbType.VarChar, Value = UserService.Username });

            SqlService.ExecuteNonQuery(connectionName, "RecentHistoryInsert", sqlParameters);

            // Data changed so remove all variations of history
            // Note: Changed to explicitly set key models instead of going by key name while non-cluster safe keysInUse process is still in place.
            CacheService.Remove(new KeyModel(CacheType.User, "History").Add("Single").Add(historyType).Add(queryStringValues));
            CacheService.Remove(new KeyModel(CacheType.User, "History").Add("All").Add(historyType));
        }
        
        /// <summary>
        /// Removes the specified recent history item.
        /// </summary>
        /// <param name="historyType">The history type.</param>
        /// <param name="values">The values necessary for loading the object as a dictionary of key value pairs.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is <c>null</c> or empty.</exception>
        public void Remove(HistoryType historyType, IDictionary<string, object> values)
        {
            if (values == null || !values.Any())
            {
                throw new ArgumentNullException("values");
            }

            var queryStringValues = values.ToQueryString(true);
            var sqlParameters = new List<SqlParameter>();

            sqlParameters.Add(new SqlParameter { ParameterName = "@return_value", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.ReturnValue });
            sqlParameters.Add(new SqlParameter { ParameterName = "@HistoryType_RHC", SqlDbType = SqlDbType.VarChar, Value = historyType.ToString() });
            sqlParameters.Add(new SqlParameter { ParameterName = "@ObjectValues", SqlDbType = SqlDbType.VarChar, Value = queryStringValues });
            sqlParameters.Add(new SqlParameter { ParameterName = "@UserID", SqlDbType = SqlDbType.VarChar, Value = UserService.Username });

            SqlService.ExecuteNonQuery(connectionName, "RecentHistoryDelete", sqlParameters);

            // Data changed so remove all variations of history
            // Note: Changed to explicitly set key models instead of going by key name while non-cluster safe keysInUse process is still in place.
            CacheService.Remove(new KeyModel(CacheType.User, "History").Add("Single").Add(historyType).Add(queryStringValues));
            CacheService.Remove(new KeyModel(CacheType.User, "History").Add("All").Add(historyType));
        }

        /// <summary>
        /// Pins the specified recent history record.
        /// </summary>
        /// <param name="historyType">The history type.</param>
        /// <param name="values">The values necessary for loading the object as a dictionary of key value pairs.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="values" /> is <c>null</c> or empty.</exception>
        public void Pin(HistoryType historyType, IDictionary<string, object> values)
        {
            if (values == null || !values.Any())
            {
                throw new ArgumentNullException("values");
            }

            var queryStringValues = values.ToQueryString(true);
            var sqlParameters = new List<SqlParameter>();

            sqlParameters.Add(new SqlParameter { ParameterName = "@return_value", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.ReturnValue });
            sqlParameters.Add(new SqlParameter { ParameterName = "@HistoryType_RHC", SqlDbType = SqlDbType.VarChar, Value = historyType.ToString() });
            sqlParameters.Add(new SqlParameter { ParameterName = "@ObjectValues", SqlDbType = SqlDbType.VarChar, Value = queryStringValues });
            sqlParameters.Add(new SqlParameter { ParameterName = "@IsPinned", SqlDbType = SqlDbType.Bit, Value = true });
            sqlParameters.Add(new SqlParameter { ParameterName = "@UserID", SqlDbType = SqlDbType.VarChar, Value = UserService.Username });

            SqlService.ExecuteNonQuery(connectionName, "RecentHistoryUpdate", sqlParameters);

            // Data changed so remove all variations of history
            // Note: Changed to explicitly set key models instead of going by key name while non-cluster safe keysInUse process is still in place.
            CacheService.Remove(new KeyModel(CacheType.User, "History").Add("Single").Add(historyType).Add(queryStringValues));
            CacheService.Remove(new KeyModel(CacheType.User, "History").Add("All").Add(historyType));
        }

        /// <summary>
        /// Unpins the specified recent history record.
        /// </summary>
        /// <param name="historyType">The history type.</param>
        /// <param name="values">The values necessary for loading the object as a dictionary of key value pairs.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="values" /> is <c>null</c> or empty.</exception>
        public void Unpin(HistoryType historyType, IDictionary<string, object> values)
        {
            if (values == null || !values.Any())
            {
                throw new ArgumentNullException("values");
            }

            var queryStringValues = values.ToQueryString(true);
            var sqlParameters = new List<SqlParameter>();

            sqlParameters.Add(new SqlParameter { ParameterName = "@return_value", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.ReturnValue });
            sqlParameters.Add(new SqlParameter { ParameterName = "@HistoryType_RHC", SqlDbType = SqlDbType.VarChar, Value = historyType.ToString() });
            sqlParameters.Add(new SqlParameter { ParameterName = "@ObjectValues", SqlDbType = SqlDbType.VarChar, Value = queryStringValues });
            sqlParameters.Add(new SqlParameter { ParameterName = "@IsPinned", SqlDbType = SqlDbType.Bit, Value = false });
            sqlParameters.Add(new SqlParameter { ParameterName = "@UserID", SqlDbType = SqlDbType.VarChar, Value = UserService.Username });

            SqlService.ExecuteNonQuery(connectionName, "RecentHistoryUpdate", sqlParameters);

            // Data changed so remove all variations of history
            // Note: Changed to explicitly set key models instead of going by key name while non-cluster safe keysInUse process is still in place.
            CacheService.Remove(new KeyModel(CacheType.User, "History").Add("Single").Add(historyType).Add(queryStringValues));
            CacheService.Remove(new KeyModel(CacheType.User, "History").Add("All").Add(historyType));
        }

        /// <summary>
        /// Gets the recent history item for a specified history type and object ID
        /// </summary>
        /// <param name="historyType">The history type.</param>
        /// <param name="values">The values necessary for loading the object as a dictionary of key value pairs.</param>
        /// <returns>Returns <see cref="IEnumerable{HistoryModel}"/> for a single matching recent history record. If no matching record found returns null.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is <c>null</c> or empty.</exception>
        public HistoryModel Get(HistoryType historyType, IDictionary<string, object> values)
        {
            if (values == null || !values.Any())
            {
                throw new ArgumentNullException("values");
            }

            var queryStringValues = values.ToQueryString(true);
            var key = new KeyModel(CacheType.User, "History").Add("Single").Add(historyType).Add(queryStringValues);
            HistoryModel data;

            if (!CacheService.TryGet(key, out data))
            {
                var sqlParameters = new List<SqlParameter>();

                sqlParameters.Add(new SqlParameter { ParameterName = "@return_value", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.ReturnValue });
                sqlParameters.Add(new SqlParameter { ParameterName = "@HistoryType_RHC", SqlDbType = SqlDbType.VarChar, Value = historyType.ToString() });
                sqlParameters.Add(new SqlParameter { ParameterName = "@ObjectValues", SqlDbType = SqlDbType.VarChar, Value = queryStringValues });
                sqlParameters.Add(new SqlParameter { ParameterName = "@UserID", SqlDbType = SqlDbType.VarChar, Value = UserService.Username });

                data = SqlService.Execute<HistoryModel>(connectionName, "RecentHistoryGetSingle", sqlParameters,
                    reader =>
                    {
                        var model = new HistoryModel();
                        model.HistoryType =  historyType;
                        string vals = (reader[2] as string);
                        if (!string.IsNullOrEmpty(vals) && vals.IndexOf('=') >= 0)
                        {
                            model.Values = new Dictionary<string, object>();
                            var split = vals.Split(new char[] {'='},StringSplitOptions.None);
                            model.Values.Add(split[0],split[1]);
                        }
                        model.DisplayName = reader[3] as string;
                        model.IsPinned = reader.GetBoolean(4);
                        model.DateAccessed = reader.GetDateTime(5);
                        model.Username = reader[6] as string;
                        return model;
                    }
                    ).FirstOrDefault();

                if (data != null)
                {
                    // Successful so store in cache
                    CacheService.Set(key, data);
                }
            }

            return data;
        }

        /// <summary>
        /// Gets the list of recent history records for the current user and a specified record type.
        /// </summary>
        /// <param name="historyType">The history type.</param>
        /// <returns>Returns <see cref="IEnumerable{HistoryModel}"/> for matching recent history records.</returns>
        public IEnumerable<HistoryModel> Get(HistoryType historyType)
        {
            var key = new KeyModel(CacheType.User, "History").Add("All").Add(historyType);
            List<HistoryModel> data;

            if (!CacheService.TryGet(key, out data))
            {
                var sqlParameters = new List<SqlParameter>();

                sqlParameters.Add(new SqlParameter { ParameterName = "@return_value", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.ReturnValue });
                sqlParameters.Add(new SqlParameter { ParameterName = "@HistoryType_RHC", SqlDbType = SqlDbType.VarChar, Value = historyType.ToString() });
                sqlParameters.Add(new SqlParameter { ParameterName = "@UserID", SqlDbType = SqlDbType.VarChar, Value = UserService.Username });

                data = SqlService.Execute<HistoryModel>(connectionName, "RecentHistoryGet", sqlParameters,
  reader =>
  {
      var model = new HistoryModel();
      model.HistoryType = historyType;
      string vals = (reader[2] as string);
      if (!string.IsNullOrEmpty(vals) && vals.IndexOf('=') >= 0)
      {
          model.Values = new Dictionary<string, object>();
          var split = vals.Split(new char[] { '=' }, StringSplitOptions.None);
          model.Values.Add(split[0], split[1]);
      }
      model.DisplayName = reader[3] as string;
      model.IsPinned = reader.GetBoolean(4);
      model.DateAccessed = reader.GetDateTime(5);
      model.Username = reader[6] as string;
      return model;
  }                  
                    ).ToList();

                if (data.Any())
                {
                    // Successful so store in cache
                    CacheService.Set(key, data);
                }
            }

            return data;
        }
    }
}
