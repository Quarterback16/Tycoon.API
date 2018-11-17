using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using ProgramAssuranceTool.Infrastructure.Interfaces;

namespace ProgramAssuranceTool.Registrations
{
	/// <summary>
	/// Represents a registration that is used to register routes.
	/// </summary>
	public class RouteRegistration : IRegistration
	{
		/// <summary>
		/// Register routes.
		/// </summary>
		public void Register()
		{
			RouteTable.Routes.IgnoreRoute( "{resource}.axd/{*pathInfo}" );

			RouteTable.Routes.MapHttpRoute(
				 name: "DefaultApi",
				 routeTemplate: "api/{controller}/{id}",
				 defaults: new { id = RouteParameter.Optional }
				);

			RouteTable.Routes.MapRoute(
				name: "Default", // Route name
				url: "{controller}/{action}/{id}", // URL with parameters
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);



		}
	}
}