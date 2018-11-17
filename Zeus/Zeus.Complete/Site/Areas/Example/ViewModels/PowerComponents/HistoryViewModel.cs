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
    public class HistoryViewModel : ILayoutOverride
    {
        public HistoryViewModel(string jobseekerId, string contractId)
        {
            Hidden = new List<LayoutType>() { LayoutType.RequiredFieldsMessage };

            Overview = new ContentViewModel()
                .AddParagraph("The Last accessed list is a list that will appear in a pull out tab on the right hand side of the screen. Inside this tab will be references to recently accessed objects relevant to your area.")
                .AddParagraph("Clicking on the links in the history list will reload the current page, but with the context of the selected object. In essence this simply replaces the appropriate context parameter in your URL. (e.g. jobseekerId for Jobseekers, contractId for Contracts etc.)")
                .AddParagraph("In this case you have gone to a page with ")
                .AddSubTitle("jobseekerId: " + (jobseekerId ?? "(not set)") + " and contractId: " + (contractId ?? "(not set)"))
                .AddParagraph("To set which history objects will be available on your page, use the 'History' attribute on your controller or action. You can specifiy multiple History attributes if your page has multiple contexts.")
                .AddPreformatted(@"
    [Menu(""Last accessed"", Order = 40, ParentAction = ""Index"")]
    [History(Infrastructure.Types.HistoryType.JobSeeker)]
    [History(Infrastructure.Types.HistoryType.Contract)]
    public ActionResult History(string jobseekerId, string contractId)
    {
        ...
    }
")
                .AddParagraph("In order to support context in your pages, you will need to create a specific route in your area registration")
                .AddPreformatted(@"
    public override void RegisterArea(AreaRegistrationContext context)
    {
        ...
        // Routes for the history example
        context.MapRoute(""Example_history_route_path""
            ,""Example/{controller}/History/{jobseekerId}/{contractId}""
            , new { jobseekerId = UrlParameter.Optional, contractId = UrlParameter.Optional, action = ""History"", area = ""Example"" })
                .SetRouteName(""Example_history_route_path"");
        ...
    }
")
                .AddParagraph("In order to create entries in the history, your actions must set elements using the History Service. For example: ")
                .AddPreformatted(@"
    public ActionResult History(string jobseekerId, string contractId)
    {
        ...
        UserService.History.SetContract(""CB222222"", ""A big contract"");
        ...
    }
")
                ;
        }

        public IEnumerable<LayoutType> Hidden { get; set; }

        [Display(GroupName="Overview")]
        public ContentViewModel Overview { get; private set; }
    }
}