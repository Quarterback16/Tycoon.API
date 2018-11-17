using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Employment.Web.Mvc.Infrastructure.Controllers;
using Employment.Web.Mvc.Infrastructure.Csv;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using Employment.Web.Mvc.Area.Example.Service.Interfaces;
using Employment.Web.Mvc.Area.Example.ViewModels.BasicFormElements;

namespace Employment.Web.Mvc.Area.Example.Controllers
{
    //[Security(AllowAny=true)]
    [Security(securityConfigKey: "Foo")]
    public class BasicFormElementsController : InfrastructureController
    {
        protected readonly IDummyService DummyService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dummyService">Dummy service for retrieving example data</param>
        /// <param name="userService">User service for retrieving user data.</param>
        /// <param name="adwService">Adw service for retrieving ADW data.</param>
        public BasicFormElementsController(IDummyService dummyService,  IUserService userService, IAdwService adwService) : base( userService, adwService)
		{
            if (dummyService == null)
            {
                throw new ArgumentNullException("dummyService");
            }

		    DummyService = dummyService;
		}

        [Menu("Basic form controls", Order = 10)]
        public ActionResult Index()
        {
            AddAlert("Test", new ContentViewModel().AddText("Testing ").AddLink("Workflow", "Workflow", "link to workflow").AddText("."));
            AddAlert("Test", new ContentViewModel().AddText("Testing ").AddLink("Workflow", "Workflow", "link to workflow").AddText("."), IconType.Bolt, ColourType.Blue);

            var model = new ContentViewModel()
                .AddTitle("Basic form controls examples")
                .AddParagraph("This area contains examples demonstrating the use of basic form elements within the Infrastructure ViewModel system.");

            return View(model);
        }

        #region Content

        [Menu("Basic HTML content", Order=5, ParentAction="Index")]
        public ActionResult ExampleContent()
        {
            var model = new ExampleContentViewModel();

            return View(model);
        }

        #endregion

        #region Text boxes / hiddens
        /// <summary>
        /// Examples of text boxes and multiline text areas
        /// </summary>
        /// <returns></returns>
        [Menu("Text boxes and hiddens", Order = 10, ParentAction = "Index")]
        public ActionResult TextBoxes()
        {
            var model = new TextBoxesViewModel();
            model.Hidden = "Surprise! Betcha weren't expecting this value.";
            return View(model);
        }

        [HttpPost]
        public ActionResult TextBoxes(TextBoxesViewModel model)
        {
            return View(model);
        }
        #endregion

        
        #region Booleans
        /// <summary>
        /// Examples of Switchers and checkboxes for selecting single boolean values
        /// </summary>
        /// <returns></returns>
        [Menu("Booleans", Order = 20, ParentAction = "Index")]
        public ActionResult Booleans()
        {
            var model = new BooleansViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Booleans(BooleansViewModel model)
        {
            return View(model);
        }
        #endregion 
        
        #region Date/Times
        /// <summary>
        /// Examples of Date and time pickers
        /// </summary>
        /// <returns></returns>
        [Menu("Date / Times", Order = 30, ParentAction = "Index")]
        public ActionResult DateTimes()
        {
            var model = new DateTimesViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult DateTimes(DateTimesViewModel model)
        {
            return View(model);
        }
        #endregion 

        
        #region Single selects
        /// <summary>
        /// Examples of single selections, including drop downs and radio buttons
        /// </summary>
        /// <returns></returns>
        [Menu("Single selects", Order = 40, ParentAction = "Index")]
        public ActionResult SingleSelects()
        {
            var model = new SingleSelectsViewModel();
            // SelectList
            model.Age = AdwService.GetListCodes("AGE").ToSelectList(m => m.Code, m => m.Description);

            // Enumerable<SelectListItem>
            model.State = AdwService.GetListCodes("STT").ToSelectListItem(m => m.Code, m => m.Description);

            return View(model);
        }

        [HttpPost]
        public ActionResult SingleSelects(SingleSelectsViewModel model)
        {
            return View(model);
        }
        #endregion 
        
        
        #region Multi selects
        /// <summary>
        /// Examples of multi selects, including multi select boxes and checkbox lists
        /// </summary>
        /// <returns></returns>
        [Menu("Multi selects", Order = 50, ParentAction = "Index")]
        public ActionResult MultiSelects()
        {
            var model = new MultiSelectsViewModel();
            model.Ages = AdwService.GetListCodes("AGE").ToMultiSelectList(m => m.Code, m => m.Description);
            model.Checkboxes = AdwService.GetListCodes("AGE").ToMultiSelectList(m => m.Code, m => m.Description);
            model.States = AdwService.GetListCodes("STT").ToSelectListItem(m => m.Code, m => m.Description);

            return View(model);
        }

        [HttpPost]
        public ActionResult MultiSelects(MultiSelectsViewModel model)
        {
            return View(model);
        }
        #endregion 
        
        #region Links
        /// <summary>
        /// Examples of links, including external and internal links
        /// </summary>
        /// <returns></returns>
        [Menu("Links", Order = 60, ParentAction = "Index")]
        public ActionResult Links()
        {
            var model = new LinksViewModel();
            model.FirstParameter = "pizza";
            model.SecondParameter = "pie";
            return View(model);
        }

        [HttpPost]
        public ActionResult Links(LinksViewModel model)
        {
            return View(model);
        }
        #endregion 

        
        #region Submit buttons
        /// <summary>
        /// Examples of submit buttons and custom Action handling
        /// </summary>
        /// <returns></returns>
        [Menu("Submit buttons", Order = 70, ParentAction = "Index")]
        public ActionResult SubmitButtons()
        {
            var model = new SubmitButtonsViewModel();
            model.Result = new ContentViewModel("Submit result");
            model.Result.AddParagraph("Nothing has been submitted");
            return View(model);
        }

        [HttpPost]
        public ActionResult SubmitButtons(SubmitButtonsViewModel model, string submitType)
        {
            model.Result = new ContentViewModel("Submit result");
            model.Result.AddParagraph(submitType + " has been submitted");
            return View(model);
        }
        
        #endregion 
        
        #region Split buttons
        /// <summary>
        /// Examples of Split buttons, including split buttons and drop down menus only
        /// </summary>
        /// <returns></returns>
        [Menu("Split buttons", Order = 80, ParentAction = "Index")]
        public ActionResult SplitButtons()
        {
            var model = new SplitButtonsViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult SplitButtons(SplitButtonsViewModel model)
        {
            return View(model);
        }
        #endregion 
       
    }
}