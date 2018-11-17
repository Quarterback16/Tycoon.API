using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Models;
using Employment.Web.Mvc.Infrastructure.Types;
using System.Data.SqlClient;
using System.Data;

// TODO : Make this actually use a database instead of a session. The session usage is just a quick hack
namespace Employment.Web.Mvc.Infrastructure.Services
{
    public class DashboardService : Service, IDashboardService
    {
        /// <summary>
        /// Connection string name
        /// </summary>
        private const string connectionName = "DbConn_ZEUS";

        /// <summary>
        /// Sql service for interacting with a Sql database.
        /// </summary>
        internal readonly ISqlService SqlService;


        public DashboardService(ISqlService sqlService, IClient client, ICacheService cacheService)
            : base(client, cacheService)
        {
            if (sqlService == null)
            {
                throw new ArgumentNullException("sqlService");
            }

            SqlService = sqlService;
        }

        public IEnumerable<string> GetOpenWidgetNames(string widgetContext)
        {
            LoadFromContext(widgetContext);
            return OpenWidgetNames;
        }


        /// <summary>
        /// Provides the currently set data context for displaying widget information.
        /// For example this might be "User" or "Office" depending on the scope of the information to be displayed in widgets
        /// </summary>
        public string GetDataContext(string widgetContext)
        {
            LoadFromContext(widgetContext);
            return DataContext;
        }

        /// <summary>
        /// Sets the current data context in use within the given widget context
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="widgetContext"></param>
        public void SetDataContext(string dataContext, string widgetContext)
        {
            LoadFromContext(widgetContext);
            DataContext = dataContext;
            SaveContext(widgetContext);
        }

        /// <summary>
        /// Adds the given widget name to the given context. The specified widget is now 'open'.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="widgetContext"></param>
        public void AddWidgetName(string name, string widgetContext)
        {
            LoadFromContext(widgetContext);
            if (!OpenWidgetNames.Contains(name))
            {
                OpenWidgetNames.Add(name);
                SaveContext(widgetContext);
            }
        }

        /// <summary>
        /// Removes the given widget name from the given context. The specified widget is now 'closed'.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="widgetContext"></param>
        public void RemoveWidgetName(string name, string widgetContext)
        {
            LoadFromContext(widgetContext);
            if (OpenWidgetNames.Contains(name))
            {
                OpenWidgetNames.Remove(name);
                SaveContext(widgetContext);
            }
        }


        /// <summary>
        /// Sets the entire layout of widgets for the given context. Only the widgets in the given string will be open, all others will be closed.
        /// </summary>
        /// <param name="layout">A comma separated list of widget names</param>
        /// <param name="widgetContext"></param>
        public void SetWidgetLayout(string layout, string widgetContext)
        {
            LoadFromContext(widgetContext);
            OpenWidgetNames = layout.Split(',').ToList();
            SaveContext(widgetContext);
        }

        #region internal section

        internal string DataContext { get; set; }
        internal List<string> OpenWidgetNames { get; set; }

        public void LoadFromContext(string widgetContext)
        {
            string context;
            var key = new KeyModel(CacheType.User, widgetContext);
            if (!CacheService.TryGet<string>(key, out context))
            {
                // load from db
                var sqlParameters = new List<SqlParameter>();
                sqlParameters.Add(new SqlParameter { ParameterName = "@return_value", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.ReturnValue });
                sqlParameters.Add(new SqlParameter { ParameterName = "@UserID", SqlDbType = SqlDbType.VarChar, Value = UserService.Username });
                sqlParameters.Add(new SqlParameter { ParameterName = "@WidgetContext", SqlDbType = SqlDbType.VarChar, Value = widgetContext });
                IEnumerable<string> data = SqlService.Execute<string>(connectionName, "DashboardGet", sqlParameters,
                      reader =>
                      {
                          return reader[2] as string;
                      }
                );

                context = data.Any() ? data.First() : ",";

                // Set cache
                CacheService.Set<string>(key, context);
            }

            OpenWidgetNames = context.Split(',').ToList();
            DataContext = OpenWidgetNames[0];
            OpenWidgetNames.RemoveAt(0);
        }

        public void SaveContext(string widgetContext)
        {
            var key = new KeyModel(CacheType.User, widgetContext);
            string layout = DataContext + "," + string.Join(",", OpenWidgetNames);

            // save to db
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter { ParameterName = "@return_value", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.ReturnValue });
            sqlParameters.Add(new SqlParameter { ParameterName = "@UserID", SqlDbType = SqlDbType.VarChar, Value = UserService.Username });
            sqlParameters.Add(new SqlParameter { ParameterName = "@WidgetContext", SqlDbType = SqlDbType.VarChar, Value = widgetContext });
            sqlParameters.Add(new SqlParameter { ParameterName = "@WidgetLayout", SqlDbType = SqlDbType.VarChar, Value = layout });
            SqlService.ExecuteNonQuery(connectionName, "DashboardSetLayout", sqlParameters);

            // Set cache
            CacheService.Set<string>(key, layout);
        }

        #endregion
    }
}
