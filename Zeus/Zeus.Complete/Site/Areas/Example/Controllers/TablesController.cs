using Employment.Web.Mvc.Area.Example.ViewModels;
using Employment.Web.Mvc.Area.Example.ViewModels.Tables;
using Employment.Web.Mvc.Infrastructure.Controllers;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Employment.Web.Mvc.Area.Example.Controllers
{
    [Security(AllowAny = true)]
    public class TablesController : InfrastructureController
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="TablesControllerEx" /> class.
        /// </summary>
        /// <param name="mappingEngine">AutoMapper mapping engine for mapping between types.</param>
        /// <param name="userService">User service for retrieving user data.</param>
        /// <param name="adwService">Adw service for retrieving ADW data.</param>
        public TablesController(IUserService userService, IAdwService adwService) : base(userService, adwService) { }

        //
        // GET: /Foo/
        [Menu("Grids")]
        public ActionResult Index()
        {
            var model = new ContentViewModel()
                .AddTitle("Grid Examples")
                .AddParagraph("This area contains examples demonstrating how to use the various Grid types.");

            return View(model);
        }

      
        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [Menu("Footable", ParentAction = "Index")]
        public ActionResult Footable(int page = 1)
        {
            var ctx = new FooData();
            var model = new FootableViewModelDoc();
            model.FootableRows = new List<FootableDemoRowDoc>
            {
                new FootableDemoRowDoc { JobseekerId = "111", Firstname = "Wanda", Surname = "Maximoff", EPPStatus = "Approved 11/07/2014", PostCode = "5607" },
                new FootableDemoRowDoc { JobseekerId = "222", Firstname = "Dane", Surname = "Whitman", EPPStatus = "Approved 15/04/2014", PostCode = "5605" },
                new FootableDemoRowDoc { JobseekerId = "333", Firstname = "Natalia", Surname = "Romanova", EPPStatus = "Approved 4/06/2014", PostCode = "5641" }
            };


            return View(model);
        }

        [HttpPost]
        public ActionResult Footable(FootableViewModelDoc vm)
        {
            
            
            return View();
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [Menu("Footable (Editable)", ParentAction = "Index")]
        public ActionResult FootableEditable(int page = 1)
        {
            var ctx = new FooData();
            var model = new FootableEditableViewModelDoc();
            model.FootableRows = new List<FootableEditableDemoRow>
            {
                new FootableEditableDemoRow { JobseekerId = "111", Firstname = "Wanda", Surname = "Maximoff", EPPStatus = "Approved 11/07/2014", PostCode = "5607" },
                new FootableEditableDemoRow { JobseekerId = "222", Firstname = "Dane", Surname = "Whitman", EPPStatus = "Approved 15/04/2014", PostCode = "5605" },
                new FootableEditableDemoRow { JobseekerId = "333", Firstname = "Natalia", Surname = "Romanova", EPPStatus = "Approved 4/06/2014", PostCode = "5641" }
            };


            return View(model);
        }

        [HttpPost]
        public ActionResult FootableEditable(FootableEditableViewModelDoc vm)
        {
            

            return View();
        }

        [HttpPost]
        public ActionResult SaveGrid1Data(IEnumerable<FootableEditableDemoRow> rowData)
        {


            return Json("Success");
        }

        [HttpPost]
        public ActionResult SaveGrid2(IEnumerable<PersonRowEditable> rowData)
        {


            return Json("Boo");
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [Menu("Footable (Editable Exp)", ParentAction = "Index")]
        public ActionResult FootableEditableExp(int page = 1)
        {
            var ctx = new FooData();
            var model = new FootableEditableViewModelExp();
            model.FootableRows = new List<FootableEditableDemoRowExp>
            {
                new FootableEditableDemoRowExp { JobseekerId = "111", Firstname = "Wanda", Surname = "Maximoff", Country = "0001", EPPStatus = "Approved 11/07/2014", PostCode = "5607" },
                new FootableEditableDemoRowExp { JobseekerId = "222", Firstname = "Dane", Surname = "Whitman", Country = "0002", EPPStatus = "Approved 15/04/2014", PostCode = "5605" },
                new FootableEditableDemoRowExp { JobseekerId = "333", Firstname = "Natalia", Surname = "Romanova", Country = "0001", EPPStatus = "Approved 4/06/2014", PostCode = "5641" }
            };

            // Reference list of Countries for drop down list
            model.FootableRows_Countries = new List<Country>
            {
                new Country { Id = "0001", Name = "Australia" },
                new Country { Id = "0002", Name = "New Zealand" },
                new Country { Id = "0003", Name = "USA" }
            };


            return View(model);
        }

        [HttpPost]
        public ActionResult FootableEditableExp(FootableEditableViewModelDoc vm)
        {


            return View();
        }

        [HttpPost]
        public ActionResult SaveGridExpData(IEnumerable<FootableEditableDemoRow> rowData)
        {


            return Json("Success");
        }


	}
}