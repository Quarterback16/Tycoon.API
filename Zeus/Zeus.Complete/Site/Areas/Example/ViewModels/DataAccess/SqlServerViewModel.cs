using System;
using System.ComponentModel.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.ViewModels;

namespace Employment.Web.Mvc.Area.Example.ViewModels.DataAccess
{
    [Group("Overview")]
    public class SqlServerViewModel
    {
        [Display(GroupName = "Overview")]
        public ContentViewModel Overview {
            get{
                return new ContentViewModel()
                    .AddParagraph("Access to SQL databases should be performed through your Area's services. It should provide an interface definition in the 'Interfaces' namespace")
                    .AddPreformatted(@"
    namespace Employment.Web.Mvc.Area.Example.Service.Interfaces {
        DataAccessSqlServerModel GetSomeSqlServerData();

        public class DataAccessSqlServerModel {
            public string BulletinTitle { get; set; }
            public int BulletinId { get; set; }
            public DateTime BulletinLiveDate { get; set; }
        }
    }
")
                    .AddParagraph("It should provide an implementation such as the following in the 'Implementation' namespace")
                    .AddPreformatted(@"
    namespace Employment.Web.Mvc.Area.Example.Service.Implementation
    {
        // Some SQL calls will benefit from using the ICacheService - you need to evaluate this for each call.
        public DataAccessSqlServerModel GetSomeSqlServerData() {
            // The name of the connection string to your database in the web.config
            string connectionName = ""DbConn_AjsCms"";

            // ---------------------------------------------------------------------------------------------------------
            // Example of executing a stored procedure - Retrieves the value of the Site ID for use in the next example.
            // ---------------------------------------------------------------------------------------------------------
            string SiteCode = ""BULLETINS"";
            short siteId = 0;

            SqlParameter siteCodeParameter = new SqlParameter() { ParameterName = ""@SiteCode"", SqlDbType = SqlDbType.VarChar, Value = SiteCode };
            SqlParameter siteIdParameter = new SqlParameter() { ParameterName = ""@SiteId"", Direction = ParameterDirection.Output, Value = 0 };

            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(siteCodeParameter); // Input parameter
            sqlParameters.Add(siteIdParameter); // Output parameter

            SqlService.ExecuteNonQuery(connectionName, ""AjsCmsSiteGet"", sqlParameters);
            if (siteIdParameter.Value == null)
            {
                throw new Exception(string.Format(""Could not find Site ID for {0}"", SiteCode));
            }
            short.TryParse(siteIdParameter.Value.ToString(), out siteId);

            // ---------------------------------------------------------------------------------------------------------
            // Example of executing a query - Returns a simple set of data
            // ---------------------------------------------------------------------------------------------------------
            var sqlParameters2 = new List<SqlParameter>{new SqlParameter{ParameterName = ""@SiteId"",SqlDbType = SqlDbType.VarChar,Value = siteId}};

            var bulletinList = SqlService.Execute<DataAccessSqlServerModel>(connectionName, ""AjsCmsPageGetAll"", sqlParameters2,
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
")
                .AddParagraph("Adding the content to your view model is then simply translating the returned data into an appropriate view model. e.g.")
                .AddPreformatted(@"
        public ActionResult SqlServer()
        {
            SqlServerViewModel model = new SqlServerViewModel();
            DataAccessSqlServerModel tempmodel = exampleCaGenService.GetSomeSqlServerData();
            model.BulletinId = tempmodel.BulletinId;
            model.BulletinLiveDate = tempmodel.BulletinLiveDate.ToLongDateString();
            model.BulletinTitle = tempmodel.BulletinTitle;

            return View(model);
        }
")
                ;
            }
        }

        [Bindable, Editable(false)]
        [Display(GroupName="Overview")]
        public string BulletinTitle { get; set; }

        [Bindable, Editable(false)]
        [Display(GroupName = "Overview")]
        public int BulletinId { get; set; }

        [Bindable, Editable(false)]
        [Display(GroupName = "Overview")]
        public string BulletinLiveDate { get; set; }
    }
}