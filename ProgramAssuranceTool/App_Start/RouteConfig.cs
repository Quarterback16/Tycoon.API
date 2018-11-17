using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace ProgramAssuranceTool
{
	public class RouteConfig
	{
		/// <summary>
		///   This is the standard MVC code for registering routes, however, as we are using 
		///   RJCP style, go to Registrations / RouteRegistrations to register
		/// </summary>
		/// <param name="routes"></param>
		public static void RegisterRoutes( RouteCollection routes )
		{
			routes.IgnoreRoute( "{resource}.axd/{*pathInfo}" );

			routes.MapHttpRoute(
				 name: "DefaultApi",
				 routeTemplate: "api/{controller}/{id}",
				 defaults: new { id = RouteParameter.Optional } 
				 );

			routes.MapRoute(
				 name: "Default",
				 url: "{controller}/{action}/{id}",
				 defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}