using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace Employment.Web.Mvc.Infrastructure.Interfaces
{
    public interface IRouteRegistrationService
    {
        Route MapRoute(AreaRegistrationContext context, string name, string url);

        Route MapRoute(AreaRegistrationContext context, string name, string url, object defaults);

        Route MapRoute(AreaRegistrationContext context, string name, string url, string[] namespaces);

        Route MapRoute(AreaRegistrationContext context, string name, string url, object defaults, object constraints);

        Route MapRoute(AreaRegistrationContext context, string name, string url, object defaults, string[] namespaces);

        Route MapRoute(AreaRegistrationContext context, string name, string url, object defaults, object constraints, string[] namespaces);

        ReadOnlyCollection<Route> GetRoutes();
    }
}
