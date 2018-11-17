using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.Mvc;
using System.Web.Routing;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Infrastructure.Services
{
    /// <summary>
    /// Stops stupidly long URLs.
    /// </summary>
   internal class RouteRegistrationService : IRouteRegistrationService
    {
        internal readonly IConfigurationManager ConfigurationManager;
        internal readonly bool ShortRoutes = false;
       internal readonly Dictionary<string, string> shortenedNames;
       internal readonly HashSet<string> usedShortValues;
       internal readonly List<Route> routes = new List<Route>();

       public RouteRegistrationService(IConfigurationManager configurationManager)
       {
           if (configurationManager == null)
           {
               throw new ArgumentNullException("configurationManager");
           }

           ConfigurationManager = configurationManager;

           bool b;
           ShortRoutes = (ConfigurationManager.AppSettings["ShortRoutes"] != null && bool.TryParse(ConfigurationManager.AppSettings.Get("ShortRoutes"), out b) && b);

           shortenedNames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
           shortenedNames.Add("Contract", "Con");
           shortenedNames.Add("Example", "Example");

           usedShortValues=new HashSet<string>(StringComparer.OrdinalIgnoreCase);
           foreach (var val in shortenedNames.Values)
               usedShortValues.Add(val);
       }

       internal string shorten(string urlPart)
       {
           if (string.IsNullOrEmpty(urlPart))
               throw new ArgumentNullException("urlPart");

           if (shortenedNames.ContainsKey(urlPart))
               return shortenedNames[urlPart];

           if (urlPart.Length < 4)
           {
               shortenedNames.Add(urlPart,urlPart);
               usedShortValues.Add(urlPart);
               return urlPart;
           }

           string renamed = urlPart.Replace("a", "").Replace("e", "").Replace("i", "").Replace("o", "").Replace("u", "");
           if (renamed.Length < 4)
           {
               //too small to rename
               shortenedNames.Add(urlPart, urlPart);
               usedShortValues.Add(urlPart);
               return urlPart;
           }
           string partial = renamed.Substring(0, 3);
           if (!usedShortValues.Contains(partial))
           {
               shortenedNames.Add(urlPart, partial);
               usedShortValues.Add(partial);
               return partial;
           }
           else
           {
               for (int i = 0; i < 100; i++)
               {
                   var tmp = partial + i;
                   if (!usedShortValues.Contains(tmp))
                   {
                       shortenedNames.Add(urlPart, tmp);
                       usedShortValues.Add(tmp);
                       return tmp;
                   }
               }
           }

           return urlPart;
       }

       /// <summary>
       /// Map a route.
       /// </summary>
       /// <param name="context">The context.</param>
       /// <param name="name">The name.</param>
       /// <param name="url">The URL.</param>
       /// <returns>The route.</returns>
       public Route MapRoute(AreaRegistrationContext context, string name, string url)
       {
           return MapRoute(context, name, url, null, null, null);
       }

       /// <summary>
       /// Map a route.
       /// </summary>
       /// <param name="context">The context.</param>
       /// <param name="name">The name.</param>
       /// <param name="url">The URL.</param>
       /// <param name="defaults">The defaults.</param>
       /// <returns>The route.</returns>
       public Route MapRoute(AreaRegistrationContext context, string name, string url, object defaults)
       {
           return MapRoute(context, name, url, defaults, null, null);
       }

       /// <summary>
       /// Map a route.
       /// </summary>
       /// <param name="context">The context.</param>
       /// <param name="name">The name.</param>
       /// <param name="url">The URL.</param>
       /// <param name="namespaces">The namespaces.</param>
       /// <returns>The route.</returns>
       public Route MapRoute(AreaRegistrationContext context, string name, string url, string[] namespaces)
       {
           return MapRoute(context, name, url, null, null, namespaces);
       }

       /// <summary>
       /// Map a route.
       /// </summary>
       /// <param name="context">The context.</param>
       /// <param name="name">The name.</param>
       /// <param name="url">The URL.</param>
       /// <param name="defaults">The defaults.</param>
       /// <param name="constraints">The constraints.</param>
       /// <returns>The route.</returns>
       public Route MapRoute(AreaRegistrationContext context, string name, string url, object defaults, object constraints)
       {
           return MapRoute(context, name, url, defaults, constraints, null);
       }

       /// <summary>
       /// Map a route.
       /// </summary>
       /// <param name="context">The context.</param>
       /// <param name="name">The name.</param>
       /// <param name="url">The URL.</param>
       /// <param name="defaults">The defaults.</param>
       /// <param name="namespaces">The namespaces.</param>
       /// <returns>The route.</returns>
       public Route MapRoute(AreaRegistrationContext context, string name, string url, object defaults, string[] namespaces)
       {
           return MapRoute(context, name, url, defaults, null, namespaces);
       }

       /// <summary>
       /// Map a route.
       /// </summary>
       /// <param name="context">The context.</param>
       /// <param name="name">The name.</param>
       /// <param name="url">The URL.</param>
       /// <param name="defaults">The defaults.</param>
       /// <param name="constraints">The constraints.</param>
       /// <param name="namespaces">The namespaces.</param>
       /// <returns>The route.</returns>
       public Route MapRoute(AreaRegistrationContext context, string name, string url, object defaults, object constraints, string[] namespaces)
       {
           if (ShortRoutes)
           {
               if (url.IndexOf('/') >= 0)
               {
                   string[] urlChunks = url.Split(new char[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
                   if (urlChunks.Length > 1)
                   {
                       for (int i = 0; i < urlChunks.Length; i++)
                       {
                           if (urlChunks[i].IndexOf('{') >= 0)
                           {
                               break;
                           }
                           urlChunks[i] = shorten(urlChunks[i]);
                       }
                   }
                   url = string.Join("/", urlChunks);
               }
           }

           //replaces extension method
           var route = context.MapRoute(name, url, defaults, constraints, namespaces);

           if (route.DataTokens == null)
           {
               route.DataTokens = new RouteValueDictionary();
           }

           route.DataTokens["RouteName"] = name;

           //add to the list we're maintaining
           routes.Add(route);

           return route;
       }

       /// <summary>
       /// Gets the routes.
       /// </summary>
       /// <returns></returns>
       public ReadOnlyCollection<Route> GetRoutes()
       {
           return new ReadOnlyCollection<Route>( routes);
       }
    }
}
