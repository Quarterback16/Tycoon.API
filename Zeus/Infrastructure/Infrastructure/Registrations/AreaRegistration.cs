using System;
using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Routing;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Infrastructure.Registrations
{
    /// <summary>
    /// Represents a registration that is used to register areas. Should be run first in sequence.
    /// </summary>
    [Order(FirstInSequence = true)]
    public class AreaRegistration : IRegistration
    {
        /// <summary>
        /// Register areas.
        /// </summary>
        public void Register()
        {
            System.Web.Mvc.AreaRegistration.RegisterAllAreas();

            IRouteRegistrationService routeRegistration = DependencyResolver.Current.GetService<IRouteRegistrationService>();

            var validRegisteredRoutes = routeRegistration.GetRoutes();
            foreach (var route in RouteTable.Routes)
            {
                var rt = route as Route;
                if (rt != null)
                {
                    if (!validRegisteredRoutes.Contains(rt))
                    {
                        throw new Exception("Route '" + rt.Url + "' is not registered with IRouteRegistrationService.");
                    }
                }
            }
        }
    }
}
