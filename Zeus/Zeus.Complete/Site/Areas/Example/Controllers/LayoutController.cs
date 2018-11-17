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
using Employment.Web.Mvc.Infrastructure.ViewModels;
using Employment.Web.Mvc.Infrastructure.ViewModels.Dynamic;
using Employment.Web.Mvc.Area.Example.Service.Interfaces;
using Employment.Web.Mvc.Area.Example.ViewModels.Layout;

namespace Employment.Web.Mvc.Area.Example.Controllers
{
    [Security(AllowAny = true)]
    public class LayoutController : InfrastructureController
    {
        /// </summary>
        /// <param name="userService">User service for retrieving user data.</param>
        /// <param name="adwService">Adw service for retrieving ADW data.</param>
        public LayoutController(IUserService userService, IAdwService adwService)
            : base(userService, adwService)
        { }

        [Menu("Layout", Order = 30)]
        public ActionResult Index()
        {
            var model = new ContentViewModel()
                .AddTitle("Layout Examples")
                .AddParagraph("This area contains examples on how to layout elements within a view model.");

            return View(model);
        }

        #region Groups
        /// <summary>
        /// Examples of grouping structures
        /// </summary>
        [Menu("Groups", Order = 10, ParentAction = "Index")]
        public ActionResult Groups()
        {
            var model = new GroupsViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Groups(GroupsViewModel model)
        {
            return View(model);
        }
        #endregion

        #region Rows
        /// <summary>
        /// Examples of using rows to make layouts
        /// </summary>
        [Menu("Rows", Order = 20, ParentAction = "Index")]
        public ActionResult Rows()
        {
            var model = new RowsViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Rows(RowsViewModel model)
        {
            return View(model);
        }
        #endregion

        #region Dynamic display
        /// <summary>
        /// Examples of using rows to make layouts
        /// </summary>
        [Menu("Dynamic display", Order = 30, ParentAction = "Index")]
        public ActionResult DynamicDisplay()
        {
            var model = new DynamicDisplayViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult DynamicDisplay(DynamicDisplayViewModel model)
        {
            return View(model);
        }
        #endregion

        #region Dynamic display with role checking
        /// <summary>
        /// Examples of using rows to make layouts
        /// </summary>
        [Menu("Dynamic display with role checking", Order = 40, ParentAction = "Index")]
        public ActionResult RoleDynamicDisplay()
        {
            var model = new RoleDynamicDisplayViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult RoleDynamicDisplay(RoleDynamicDisplayViewModel model)
        {
            return View(model);
        }
        #endregion

        #region Triggering dynamic change

        private DynamicViewModel DynamicFoo()
        {
            var model = new DynamicViewModel();

            // Display text
            model.Add(new LabelViewModel { Value = "I agree to participate in" });

            // Hidden label for Hours input (for Accessibility)
            model.Add(new LabelViewModel { ForProperty = "Hours", Hidden = true, Value = "The number of hours of voluntary work per fortnight I agree to participate in" });

            // Hours input
            model.Add(new IntViewModel { Name = "Hours" });

            // Display text
            model.Add(new LabelViewModel { Value = "hours of voluntary work per fortnight with" });

            // Hidden label for Provider input (for Accessibility)
            model.Add(new LabelViewModel { ForProperty = "Provider", Hidden = true, Value = "The service provider the work will be done with" });

            // Providers (would be retrieved from a RHEA service)
            var options = new Dictionary<string, string> { { "1", "Provider 1" }, { "2", "Provider 2" }, { "3", "Provider 3" } };

            // Provider input
            model.Add(new SelectListViewModel { Name = "Provider", Value = options.ToSelectListItem(m => m.Key, m => m.Value).ToList() });

            // Display text
            model.Add(new LabelViewModel { Value = "from" });

            // Hidden label for Hours input (for Accessibility)
            model.Add(new LabelViewModel { ForProperty = "FromDate", Hidden = true, Value = "The date I will work from" });

            // FromDate input
            model.Add(new DateViewModel { Name = "FromDate" });

            // Display text
            model.Add(new LabelViewModel { Value = "to" });

            // Hidden label for Hours input (for Accessibility)
            model.Add(new LabelViewModel { ForProperty = "ToDate", Hidden = true, Value = "The date I will work to" });

            // ToDate input
            model.Add(new DateViewModel { Name = "ToDate" });

            // Display text
            model.Add(new LabelViewModel { Value = "." });

            return model;
        }

        private DynamicViewModel DynamicBar()
        {
            var model = new DynamicViewModel();

            // Display text
            model.Add(new LabelViewModel { Value = "I have participated in" });

            // Hidden label for Hours input (for Accessibility)
            model.Add(new LabelViewModel { ForProperty = "Hours", Hidden = true, Value = "The number of hours of voluntary work I have participated in" });

            // Hours input
            model.Add(new IntViewModel { Name = "Hours" });

            // Display text
            model.Add(new LabelViewModel { Value = "hours of voluntary work with" });

            // Hidden label for Provider input (for Accessibility)
            model.Add(new LabelViewModel { ForProperty = "Provider", Hidden = true, Value = "The service provider the work was done with" });

            // Providers (would be retrieved from a RHEA service)
            var options = new Dictionary<string, string> { { "1", "Provider 1" }, { "2", "Provider 2" }, { "3", "Provider 3" } };

            // Provider input
            model.Add(new SelectListViewModel { Name = "Provider", Value = options.ToSelectListItem(m => m.Key, m => m.Value).ToList() });

            // Display text
            model.Add(new LabelViewModel { Value = "." });

            return model;
        }

        [Menu("Triggering dynamic change", Order = 50, ParentAction = "Index")]
        public ActionResult TriggeringDynamicChange()
        {
            var model = new TriggeringDynamicChangeViewModel();

            model.MyDynamicProperty = DynamicFoo();

            return View(model);
        }

        [HttpPost]
        public ActionResult TriggeringDynamicChange(TriggeringDynamicChangeViewModel model, string submitType)
        {
            if (submitType == "Change")
            {
                // Ignore model errors for this submit type
                ModelState.Clear();

                // Change which dynamic form to use based on UseAlternative
                model.MyDynamicProperty = model.FormToShow == "Bar" ? DynamicBar() : DynamicFoo();
            }
            else if (ModelState.IsValid)
            {
                model.Content.AddTitle("You submitted the following");

                var hours = model.MyDynamicProperty.Get<int>("Hours");
                var provider = model.MyDynamicProperty.Get<IEnumerable<SelectListItem>>("Provider");
                var selectedProvider = provider.FirstOrDefault(p => p.Selected);

                model.Content.AddParagraph("You agreed to work {0} voluntary hours", hours);
                model.Content.AddParagraph("With {0}", selectedProvider != null ? selectedProvider.Text : "no one");

                if (model.FormToShow == "Foo")
                {
                    var fromDate = model.MyDynamicProperty.Get<DateTime>("FromDate");
                    var toDate = model.MyDynamicProperty.Get<DateTime>("ToDate");

                    model.Content.AddParagraph("From {0} to {1}", fromDate, toDate);
                }
            }

            return View(model);
        }
        #endregion
    }
}