using System.Web.Mvc;
using System.Web.Routing;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Zeus.Registrations
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
            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            RouteTable.Routes.IgnoreRoute("elmah.axd");

            RouteTable.Routes.MapRoute(
                "Default_Bulletin", // Route name
                "Bulletin/{id}", // URL
                new { controller = "Default", action = "Bulletin", id = UrlParameter.Optional }, // Parameter defaults
                null,
                new string[] { "Employment.Web.Mvc.Zeus.Controllers" } // Namespaced so routes from Area Registrations do not conflict
            );

            RouteTable.Routes.MapRoute(
                "Default_Bulletins", // Route name
                "Bulletins", // URL
                new { controller = "Default", action = "Bulletins" }, // Parameter defaults
                null,
                new string[] { "Employment.Web.Mvc.Zeus.Controllers" } // Namespaced so routes from Area Registrations do not conflict
            );

            RouteTable.Routes.MapRoute(
                "Default_Accessibility", // Route name
                "Accessibility", // URL
                new { controller = "Default", action = "Accessibility" }, // Parameter defaults
                null,
                new string[] { "Employment.Web.Mvc.Zeus.Controllers" } // Namespaced so routes from Area Registrations do not conflict
            );

            RouteTable.Routes.MapRoute(
                "Default_Privacy", // Route name
                "Privacy", // URL
                new { controller = "Default", action = "Privacy" }, // Parameter defaults
                null,
                new string[] { "Employment.Web.Mvc.Zeus.Controllers" } // Namespaced so routes from Area Registrations do not conflict
            );

            RouteTable.Routes.MapRoute(
                "Default_Support", // Route name
                "Support", // URL
                new { controller = "Default", action = "Support" }, // Parameter defaults
                null,
                new string[] { "Employment.Web.Mvc.Zeus.Controllers" } // Namespaced so routes from Area Registrations do not conflict
            );

            RouteTable.Routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Default", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                null,
                new string[] { "Employment.Web.Mvc.Zeus.Controllers" } // Namespaced so routes from Area Registrations do not conflict
            );
        }
    }
}