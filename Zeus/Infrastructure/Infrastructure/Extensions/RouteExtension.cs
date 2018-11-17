using System;
using System.IO;
using System.Web;
using System.Web.Routing;
using Employment.Web.Mvc.Infrastructure.Http;

namespace Employment.Web.Mvc.Infrastructure.Extensions
{
    /// <summary>
    /// Extensions for <see cref="Route"/>.
    /// </summary>
    public static class RouteExtension
    {
        /// <summary>
        /// Get the name of a route.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns>The route name.</returns>
        public static string GetRouteName(this Route route)
        {
            if (route == null)
            {
                return null;
            }

            return route.DataTokens.GetRouteName();
        }

        /// <summary>
        /// Get the name of a route.
        /// </summary>
        /// <param name="routeData">The route data.</param>
        /// <returns>The route name.</returns>
        public static string GetRouteName(this RouteData routeData)
        {
            if (routeData == null)
            {
                return null;
            }

            return routeData.DataTokens.GetRouteName();
        }

        /// <summary>
        /// Get the name of a route.
        /// </summary>
        /// <param name="routeValues">The route values.</param>
        /// <returns>The route name.</returns>
        public static string GetRouteName(this RouteValueDictionary routeValues)
        {
            if (routeValues == null)
            {
                return null;
            }

            object routeName = null;

            routeValues.TryGetValue("RouteName", out routeName);

            return routeName as string;
        }

        /// <summary>
        /// Set the name of a route.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="routeName">The route name.</param>
        /// <returns>The route.</returns>
        public static Route SetRouteName(this Route route, string routeName)
        {
            if (route == null)
            {
                throw new ArgumentNullException("route");
            }

            if (route.DataTokens == null)
            {
                route.DataTokens = new RouteValueDictionary();
            }

            route.DataTokens["RouteName"] = routeName;

            return route;
        }

        /// <summary>
        /// Return the action name from a route.
        /// </summary>
        /// <param name="routeData">The route data instance.</param>
        /// <returns>The action name.</returns>
        public static string GetAction(this RouteData routeData)
        {
            return routeData.Values["action"] != null ? routeData.Values["action"].ToString() : string.Empty;
        }

        /// <summary>
        /// Return the controller name from a route.
        /// </summary>
        /// <param name="routeData">The route data instance.</param>
        /// <returns>The controller name.</returns>
        public static string GetController(this RouteData routeData)
        {
            return routeData.Values["controller"] != null ? routeData.Values["controller"].ToString() : string.Empty;
        }

        /// <summary>
        /// Return the actual area name from a route (supports fake areas).
        /// </summary>
        /// <param name="routeData">The route data instance.</param>
        /// <returns>The actual area name.</returns>
        public static string GetArea(this RouteData routeData)
        {
            var actualArea = routeData.DataTokens["area"] != null ? routeData.DataTokens["area"].ToString() : string.Empty;
            var suppliedArea = routeData.Values["area"] != null ? routeData.Values["area"].ToString() : string.Empty;

            if (!suppliedArea.Equals(actualArea, StringComparison.OrdinalIgnoreCase))
            {
                actualArea = suppliedArea;
            }

            return actualArea;
        }

        /// <summary>
        /// Returns the <see cref="RouteData" /> of a <see cref="Uri" />.
        /// </summary>
        /// <remarks>
        /// This way of generating Route Data cannot determine the RouteName so cannot be used directly for faked Area's.
        /// </remarks>
        /// <param name="uri">The Uri instance.</param>
        /// <returns>The Route Data for the Uri.</returns>
        public static RouteData ToRouteData(this Uri uri)
        {
            if (uri == null)
            {
                return null;
            }
            
            var url = uri.AbsoluteUri;
            var queryString = string.Empty;

            if (uri.AbsoluteUri.IndexOf('?')>=0)
            {
                url = uri.AbsoluteUri.Split('?')[0];
                queryString = uri.AbsoluteUri.Split('?')[1];
            }

            var httpRequest = new HttpRequest(null, url, queryString);

            return RouteTable.Routes.GetRouteData(new HttpContextWrapper(new HttpContext(httpRequest, new HttpResponse(null))));
        }
    }
}
