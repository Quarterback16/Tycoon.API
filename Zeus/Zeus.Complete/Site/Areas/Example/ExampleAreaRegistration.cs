using System;
using System.Text;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;

namespace Employment.Web.Mvc.Area.Example
{
	/// <summary>
	/// Defines the Area Registration for Example.
	/// </summary>
	public class ExampleAreaRegistration : AreaRegistration
	{
        // It is good practice to namespace your keys so they don't collide with other peoples
        public const string TextComponentKey = "Example.TextComponentKey"; 
        public const string GraphComponentKey = "Example.GraphComponentKey"; 

		/// <summary>
		/// The name of the area.
		/// </summary>
		public override string AreaName
		{
			get { return "Example"; }
		}

		/// <summary>
		/// Register the area routes.
		/// </summary>
		public override void RegisterArea(AreaRegistrationContext context)
		{
            IRouteRegistrationService routeRegistration = DependencyResolver.Current.GetService<IRouteRegistrationService>();
            

            // Routes for the history example
            routeRegistration.MapRoute(context, "Example_history_route_path",
                "Example/{controller}/History/{jobseekerId}/{contractId}",
                new { jobseekerId = UrlParameter.Optional, contractId = UrlParameter.Optional, action = "History", area = "Example" })
                ;//.SetRouteName("Example_history_route_path");

            // Routes for the dynamic link example
            routeRegistration.MapRoute(context, "Example_parameter_route_path",
                "Example/{controller}/{action}/{FirstParameter}/{SecondParameter}",
                new { first = UrlParameter.Optional, second = UrlParameter.Optional, area = "Example" });
            routeRegistration.MapRoute(context, "Example_parameter_route_argument",
                "Example/{controller}/{action}",
                new { area = "Example" });

            // TODO: comment this
            routeRegistration.MapRoute(context,
                "Example_inline_button_edit",
                "Example/Grid/{action}/{HashKey}",
                new { area = "Example", controller = "Grid", action = "Index", HashKey = UrlParameter.Optional }
                );

            // Default route
            routeRegistration.MapRoute(context,
				"Example_default",
				"Example/{controller}/{action}/{id}",
				new { area = "Example", controller = "Default", action = "Index", id = UrlParameter.Optional }
				);
            
            // Default route for a fake area
            routeRegistration.MapRoute(context,
                "Fake_default",
                "Fake/{controller}/{action}/{id}",
                new { area = "Fake", controller = "Default", action = "FakeArea", id = UrlParameter.Optional }
                );//.SetRouteName("Fake_default");


            // Widget registration
            string[] exampleContexts = new string[] { "Example", "Single widget example" };
            string[] dashboardOnly = new string[] { "Example" };

            WidgetViewModel.RegisterWidget(new WidgetViewModel("Unique title for the widget", "PowerComponents", "WidgetContentOne"), exampleContexts);
            WidgetViewModel.RegisterWidget(new WidgetViewModel("Writing implement preferences", "PowerComponents", "WidgetContentTwo"), exampleContexts);
            WidgetViewModel.RegisterWidget(new WidgetViewModel("Wide widget", "Example", "PowerComponents", "WidgetContentThree", GroupRowType.Full), exampleContexts);
            WidgetViewModel.RegisterWidget(new WidgetViewModel("Labour Market", "Example", "PowerComponents", "WidgetContentFour"), exampleContexts);
            WidgetViewModel.RegisterWidget(new WidgetViewModel("Duplicate widget", "Example", "PowerComponents", "WidgetContentOne"), dashboardOnly);
            WidgetViewModel.RegisterWidget(new WidgetViewModel("Data context example", "Example", "PowerComponents", "WidgetContentFive"), exampleContexts);

            // Context component registration
            ContextComponentAttribute.RegisteredRenderers.Add(TextComponentKey, (html, model) => { return TextContextComponent(html, model); });
            ContextComponentAttribute.RegisteredRenderers.Add(GraphComponentKey, (html, model) => { return GraphContextComponent(html, model); });
        }

        private MvcHtmlString TextContextComponent(HtmlHelper html, object model)
        {
            TagBuilder surrounds = new TagBuilder("div");
            surrounds.AddCssClass("body-content");
            TagBuilder header = new TagBuilder("h3");
            header.InnerHtml = "A text context component";
            TagBuilder content = new TagBuilder("p");
            content.InnerHtml = "A sentence describing some important things. You may wish to read another sentence as well. Have you considered putting a table in here? That might be a good way to display contextual information";

            surrounds.InnerHtml = header.ToString() + content.ToString();
            return new MvcHtmlString(surrounds.ToString());
        }

        private MvcHtmlString GraphContextComponent(HtmlHelper html, object model)
        {
            TagBuilder surrounds = new TagBuilder("div");
            surrounds.AddCssClass("body-content");
            GraphViewModel graph = new GraphViewModel(GraphViewModel.LINE);
            graph.Title = "Important looking graph";
            graph.SingleSeries.Add(0, 15);
            graph.SingleSeries.Add(10, 27);
            graph.SingleSeries.Add(20, 36);
            graph.SingleSeries.Add(30, 62);

            surrounds.InnerHtml = html.RenderPartialViewToString("EditorTemplates/GraphViewModel", graph, null).ToString();
            return new MvcHtmlString(surrounds.ToString());
        }
	}
}