using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Web.Mvc;

using Department.AddressValidation;
using Employment.Web.Mvc.Area.Example.Service.Interfaces;
//using Employment.Web.Mvc.Area.Example.ViewModels.Address;
using Employment.Web.Mvc.Area.Example.ViewModels.DataAccess;
using Employment.Web.Mvc.Infrastructure.Controllers;
using Employment.Web.Mvc.Infrastructure.Csv;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;

namespace Employment.Web.Mvc.Area.Example.Controllers
{
    /// <summary>
    /// Defines the data access controller.
    /// </summary>
    [Security(AllowAny = true)]
    public class DataAccessController : InfrastructureController
    {
        private readonly IDataAccessService exampleCaGenService;
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultController" /> class.
        /// </summary>
        /// <param name="userService">User service for retrieving user data.</param>
        /// <param name="adwService">Adw service for retrieving ADW data.</param>
        /// <param name="exampleCaGenService">The example ca gen service.</param>
        ///// <param name="yourService">Your service for interacting with your data.</param>
        public DataAccessController(IUserService userService, IAdwService adwService, IDataAccessService exampleCaGenService)
            : base(userService, adwService)
        {
            this.exampleCaGenService = exampleCaGenService;
        }

        /// <summary>
        /// Index action.
        /// </summary>
        [Menu("Data access/mapping")]
        public ActionResult Index()
        {
            var model = new ContentViewModel()
            .AddTitle("Data access examples")
            .AddParagraph("This area contains examples demonstrating how to call SQL Server from Service.Implementation project.")
           // .AddAreaLink(AreaLinkIconType.Default, "CaGen", "DataAccess", "CA Gen call")
            .AddAreaLink(AreaLinkIconType.Default, "SqlServer", "DataAccess", "SQL Server call");

            return View(model);
        }

        //[Menu("CA Gen Call", ParentAction = "Index")]
        //public ActionResult CaGen()
        //{
        //    CaGenViewModel model = new CaGenViewModel();
        //    var tempmodel = exampleCaGenService.GetSomeCaGenData("NANZ");
        //    model.AddressLine1 = tempmodel.AddressLine1;
        //    model.AddressLine2 = tempmodel.AddressLine2;
        //    model.AddressLine3 = tempmodel.AddressLine3;
        //    model.Contracts = tempmodel.Contracts;
        //    model.SiteName = tempmodel.SiteName;

        //    return View(model);
        //}


        [Menu("SQL Server Call", ParentAction = "Index")]
        public ActionResult SqlServer()
        {
            SqlServerViewModel model = new SqlServerViewModel();
            DataAccessSqlServerModel tempmodel = exampleCaGenService.GetSomeSqlServerData();
            model.BulletinId = tempmodel.BulletinId;
            model.BulletinLiveDate = tempmodel.BulletinLiveDate.ToLongDateString();
            model.BulletinTitle = tempmodel.BulletinTitle;

            return View(model);
        }

        [Menu("Static Mapping", ParentAction = "Index")]
        public ActionResult StaticMapping()
        {
            var model = new StaticMappingViewModel();

            return View(model);
        }


    }
}