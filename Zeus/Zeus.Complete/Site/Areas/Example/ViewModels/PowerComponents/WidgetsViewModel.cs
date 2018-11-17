using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Employment.Web.Mvc.Area.Example.Controllers;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using BindableAttribute = Employment.Web.Mvc.Infrastructure.DataAnnotations.BindableAttribute;

namespace Employment.Web.Mvc.Area.Example.ViewModels.PowerComponents
{
    [Widgets("Single widget example")]
    [Group("Overview")]
    public class WidgetsViewModel : ILayoutOverride
    {
        public WidgetsViewModel()
        {
            Hidden = new List<LayoutType>() { LayoutType.RequiredFieldsMessage };
        }

        public IEnumerable<LayoutType> Hidden { get; set; }


        [Display(GroupName="Overview")]
        public ContentViewModel Overview
        {
            get
            {
                return new ContentViewModel()
                    .AddParagraph("Widgets are self contained information displays that can be added/removed to pages by the user. To define a widget, you must provide an Action that returns the content of the widget (using a PartialView), as well as register the Widget definition. This registration would typically occur in your Area's registration function.")
                    .AddParagraph("Your registration must inlcude at least an Action and a Controller, and optionally an Area and GroupRowType (i.e. width of the widget). Additionally, you must indicate which specific widget contexts this widget will be available in. The special context WidgetViewModel.ALL_CONTEXTS is available to use if this widget should be available in all dashboards.")
                    .AddPreformatted(@"
    public class ExampleAreaRegistration : AreaRegistration
    {
        ...
        public override void RegisterArea(AreaRegistrationContext context)
        {
            ...
            // Widget registration
            string[] exampleContexts = new string[] { ""Example"", ""Single widget example"" };
            WidgetViewModel.RegisterWidget(new WidgetViewModel(""Unique title for the widget"", ""PowerComponents"", ""WidgetContentOne""), exampleContexts);

            // Widget registration using all options and all contexts
            string[] allContexts = new string[] { WidgetViewModel.ALL_CONTEXTS };
            WidgetViewModel.RegisterWidget(new WidgetViewModel(""All options widget"", ""Example"", ""PowerComponents"", ""WidgetContentThree"", GroupRowType.Full), allContexts);
        }
    }

    public class PowerComponentsController : InfrastructureController
    {
        ...

        [AjaxOnly]
        public ActionResult WidgetContentOne()
        {
            System.Threading.Thread.Sleep(1000); // Pause to simulate loading time
            var model = new ContentViewModel();
            model.AddSubTitle(""A content based block"");
            model.AddParagraph(""A content based widget with lots of information."");
            model.AddListItem(""A list with items"");
            model.AddListItem(""Another list item"");
            model.AddParagraph(""Some further text after the list contains even more words, and punctuation marks as well."");
            return PartialView(""EditorTemplates/ContentViewModel"", model);
        }
    }
")
                    .AddParagraph("The set of widgets displayed on any particular page is defined by the Widget 'Context' specified in your view model. Each context will be unique for that user, and include only the widgets that that user has set up. In the example widget below, this page sets the user context to only show one widget. This would not normally be done. See the 'Dashboard' page to experiment with a user customisable dashboard.")
                    .AddPreformatted(@"
    [Widgets(""Single widget example"")]
    [Group(""Overview"")]
    public class WidgetsViewModel
    {
        ...
        [Display(GroupName=""Overview"")]
        public ContentViewModel Overview { get; set; }
    }
")
                    ;
            }
        }


    }
}