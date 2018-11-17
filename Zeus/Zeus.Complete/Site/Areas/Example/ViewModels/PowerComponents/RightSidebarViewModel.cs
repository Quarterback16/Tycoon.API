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
    [ContextComponent(ExampleAreaRegistration.GraphComponentKey)]
    [ContextComponent(ExampleAreaRegistration.TextComponentKey)]
    public class RightSidebarViewModel : ILayoutOverride
    {
        public IEnumerable<LayoutType> Hidden { get; set; }

        public RightSidebarViewModel()
        {
            Hidden = new List<LayoutType>() { LayoutType.RequiredFieldsMessage };
        }

        [Display(GroupName="Overview")]
        public ContentViewModel Overview
        {
            get
            {
                return new ContentViewModel()
                    .AddParagraph("The right hand sidebar provides context information for your page. There is a set of reusable components that you can place in the right hand sidebar to make the business task your page enables easier for the user.")
                    .AddParagraph("To use a component in a page, decorate your view model with the 'ContextComponent' attribute using that component's unique key")
                    .AddPreformatted(@"
    [ContextComponent(ExampleAreaRegistration.GraphComponentKey)]
    [ContextComponent(ExampleAreaRegistration.TextComponentKey)]
    public class RightSidebarViewModel : ILayoutOverride
    {
        ...
    } 
")
                    .AddParagraph("Components should be registered using your Area's registration function. Each component is required to define a function that returns HTML content to place in the sidebar.")
                    .AddPreformatted(@"
    public class ExampleAreaRegistration : AreaRegistration
    {
        // It is good practice to namespace your keys so they don't collide with other peoples
        public const string TextComponentKey = ""Example.TextComponentKey""; 
        public const string GraphComponentKey = ""Example.GraphComponentKey"";
        ...
        public override void RegisterArea(AreaRegistrationContext context)
        {
            ....
            // Context registration
            ContextComponentAttribute.RegisteredRenderers.Add(TextComponentKey, (html, model) => { return TextContextComponent(html, model); });
            ContextComponentAttribute.RegisteredRenderers.Add(GraphComponentKey, (html, model) => { return GraphContextComponent(html, model); });
        }

        private MvcHtmlString TextContextComponent(HtmlHelper html, object model)
        {
            TagBuilder surrounds = new TagBuilder(""div"");
            surrounds.AddCssClass(""body-content"");
            TagBuilder header = new TagBuilder(""h3"");
            header.InnerHtml = ""A text context component"";
            TagBuilder content = new TagBuilder(""p"");
            content.InnerHtml = ""A sentence describing some important things. You may wish to read another sentence as well. Have you considered putting a table in here? That might be a good way to display contextual information"";

            surrounds.InnerHtml = header.ToString() + content.ToString();
            return new MvcHtmlString(surrounds.ToString());
        }

        private MvcHtmlString GraphContextComponent(HtmlHelper html, object model)
        {
            TagBuilder surrounds = new TagBuilder(""div"");
            surrounds.AddCssClass(""body-content"");
            GraphViewModel graph = new GraphViewModel(GraphViewModel.LINE);
            graph.Title = ""Important looking graph"";
            graph.SingleSeries.Add(0, 15);
            graph.SingleSeries.Add(10, 27);
            graph.SingleSeries.Add(20, 36);
            graph.SingleSeries.Add(30, 62);

            surrounds.InnerHtml = html.RenderPartialViewToString(""EditorTemplates/GraphViewModel"", graph, null).ToString();
            return new MvcHtmlString(surrounds.ToString());
        }

")
                ;
            }
        }
    }
}