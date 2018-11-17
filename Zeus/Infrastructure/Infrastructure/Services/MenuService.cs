using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Configuration;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Mappers;
using Employment.Web.Mvc.Infrastructure.Models;
using System.IdentityModel.Claims;
using System.Web.Routing;
#if DEBUG
using StackExchange.Profiling;
#endif

namespace Employment.Web.Mvc.Infrastructure.Services
{
    /// <summary>
    /// Defines a service for accessing Menu data.
    /// </summary>
    public class MenuService : Service, IMenuService
    {
        /// <summary>
        /// Configuration manager for interacting with the Web configuration.
        /// </summary>
        protected readonly IConfigurationManager ConfigurationManager;

        /// <summary>
        /// Menu Items
        /// </summary>
        public ReadOnlyCollection<MenuModel> MenuItems { get; private set; }

        /// <summary>
        /// Area Tiles
        /// </summary>
        public ReadOnlyCollection<MenuTile> AreaTiles { get; private set; }

        /// <summary>
        /// Area Names
        /// </summary>
        public ReadOnlyDictionary<string, string> AreaNames { get; private set; }

        /// <summary>
        /// Visible area tiles.
        /// </summary>
        /// <remarks>
        /// Cannot store in static as IsAuthorized needs to run for each individual user, storing would apply the first users IsAuthorized result to all other users.
        /// </remarks>
        public virtual ReadOnlyCollection<MenuTile> VisibleAreaTiles
        {
            get
            {
#if DEBUG
                var step = MiniProfiler.Current.Step("MenuService.VisibleAreaTiles");

                try
                {
#endif
                    //System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                    //stopwatch.Start();
                    bool isInPreprodorProd = (ConfigurationManager.AppSettings.Get("Environment").Equals("PROD", StringComparison.OrdinalIgnoreCase) || ConfigurationManager.AppSettings.Get("Environment").Equals("PREPROD", StringComparison.OrdinalIgnoreCase));
                    var identity = UserService.Identity;
                    var res = AreaTiles.Where(area => MenuItems.Any(m => string.Equals(m.Area, area.AreaName, System.StringComparison.OrdinalIgnoreCase) && m.IsAuthorized(identity, isInPreprodorProd))).ToList().AsReadOnly();
                    //stopwatch.Stop();
                    //System.Diagnostics.Debug.WriteLine(stopwatch.ElapsedMilliseconds + "ms VisibleAreaTiles");
                    return res;
#if DEBUG
                }
                finally
                {
                    if (step != null)
                    {
                        step.Dispose();
                    }
                }
#endif

            }
        }

        #region Constructor methods

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuService" /> class.
        /// </summary>
        /// <param name="configurationManager">Configuration manager for interacting with the Web configuration.</param>
        /// <param name="client">Client for interacting with WCF services.</param>
        /// <param name="cacheService">Cache service for interacting with cached data.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="configurationManager" /> is <c>null</c>.</exception>
        public MenuService(IConfigurationManager configurationManager, IClient client, ICacheService cacheService)
            : base(client, cacheService)
        {
            if (configurationManager == null)
            {
                throw new ArgumentNullException("configurationManager");
            }
#if DEBUG
            var step = MiniProfiler.Current.Step("MenuService.Ctor");

            try
            {
#endif

                ConfigurationManager = configurationManager;

                MenuItems = InitMenu();
                AreaTiles = InitAreaTiles();
                AreaNames = new ReadOnlyDictionary<string, string>(AreaTiles.OrderBy(a => a.Order).ThenBy(a => a.DisplayName, StringComparer.OrdinalIgnoreCase).ToDictionary(a => a.AreaName, a => a.DisplayName, StringComparer.OrdinalIgnoreCase));
#if DEBUG
            }
            finally
            {
                if (step != null)
                {
                    step.Dispose();
                }
            }
#endif

        }

        /// <summary>
        /// Builds the menu based on the MenuItem attribute on controller actions.
        /// </summary>
        private static ReadOnlyCollection<MenuModel> InitMenu()
        {
            List<MenuModel> menuItems = new List<MenuModel>();
            IEnumerable<Type> controllers = typeof(Controller).GetConcreteTypesImplementing();

            foreach (var controller in controllers)
            {
                var controllerSecurity = TypeDescriptor.GetAttributes(controller).OfType<SecurityAttribute>().SingleOrDefault();

                foreach (MethodInfo method in controller.GetMethods())
                {
                    var item = method.GetCustomAttributes(typeof(MenuAttribute), false).SingleOrDefault() as MenuAttribute;

                    if (item == null)
                    {
                        continue;
                    }

                    var actionSecurity = method.GetCustomAttributes(typeof(SecurityAttribute), false).SingleOrDefault() as SecurityAttribute;

                    if (string.IsNullOrEmpty(item.Area))
                    {
                        item.Area = !string.IsNullOrEmpty(controller.Namespace) && controller.Namespace.StartsWith("Employment.Web.Mvc.Area.", StringComparison.Ordinal) ? controller.Namespace.Substring("Employment.Web.Mvc.Area.".Length, controller.Namespace.LastIndexOf('.') - "Employment.Web.Mvc.Area.".Length) : string.Empty;
                    }

                    if (string.IsNullOrEmpty(item.Controller))
                    {
                        item.Controller = controller.Name.Substring(0, controller.Name.Length - "Controller".Length);
                    }

                    if (string.IsNullOrEmpty(item.Action))
                    {
                        item.Action = method.Name;
                    }

                    if (string.IsNullOrEmpty(item.Name))
                    {
                        item.Name = method.Name;
                    }

                    if (!string.IsNullOrEmpty(item.ParentAction))
                    {
                        item.ParentController = !string.IsNullOrEmpty(item.ParentController) ? item.ParentController : item.Controller;
                        item.ParentArea = !string.IsNullOrEmpty(item.ParentArea) ? item.ParentArea : item.Area;
                    }

                    item.ControllerSecurity = controllerSecurity;
                    item.ActionSecurity = actionSecurity;

                    menuItems.Add((item).ToMenuModel());
                }
            }

            menuItems = menuItems.OrderBy(i => i.Order).ToList();
            short menuItemId = 1;
            menuItems.ForEach(i =>
            {
                i.Parent = null;
                i.MenuItemId = menuItemId;
                menuItemId++;
                if (!string.IsNullOrEmpty(i.ParentAction))
                {
                    foreach (MenuModel p in menuItems)
                    {
                        if (string.Equals(p.Action, i.ParentAction, StringComparison.OrdinalIgnoreCase)
                            && string.Equals(p.Controller, i.ParentController, StringComparison.OrdinalIgnoreCase)
                            && string.Equals(p.Area, i.ParentArea, StringComparison.OrdinalIgnoreCase))
                        {
                            i.Parent = p;
                            p.HasChildItems = true;
                            break;
                        }
                    }
                }
            });
            menuItems.ForEach(i =>
            {
                i.StaticUrl = i.Url;
                if (!string.IsNullOrEmpty(i.ParentAction))
                {
                    foreach (MenuModel p in menuItems)
                    {
                        if (string.Equals(p.Action, i.ParentAction, StringComparison.OrdinalIgnoreCase)
                            && string.Equals(p.Controller, i.ParentController, StringComparison.OrdinalIgnoreCase)
                            && string.Equals(p.Area, i.ParentArea, StringComparison.OrdinalIgnoreCase))
                        {
                            i.ParentMenuItemId = p.MenuItemId;
                            break;
                        }
                    }
                }
            });

            return menuItems.AsReadOnly();
        }

        private ReadOnlyCollection<MenuTile> InitAreaTiles()
        {
            var result = new List<MenuTile>();
            var areas = MenuItems.Select(m => m.Area).Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(m => m, StringComparer.OrdinalIgnoreCase).ToList();
            areas.Remove(string.Empty);

            var section = ConfigurationManager.GetSection<MenuSection>("menu");

            foreach (var area in areas)
            {
                if (section != null)
                {
                    var tile = section.Tiles.Get(area);

                    if (tile == null)
                    {
                        result.Add(new MenuTile { AreaName = area, DisplayName = area, Order = int.MaxValue });
                    }
                    else if (!tile.Hidden)
                    {
                        result.Add(tile);
                    }
                }
                else
                {
                    result.Add(new MenuTile { AreaName = area, DisplayName = area, Order = int.MaxValue });
                }
            }

            return result.OrderBy(r => r.Order).ThenBy(r => r.DisplayName, StringComparer.OrdinalIgnoreCase).ToList().AsReadOnly();
        }

        #endregion

        /// <summary>
        /// Shows the menu items.
        /// </summary>
        /// <param name="html">An instance of the HTML helper.</param>
        public virtual MvcHtmlString ShowMenu(HtmlHelper html)
        {
            string area = html.ViewContext.RouteData.GetArea();
            bool isInPreprodorProd = (ConfigurationManager.AppSettings.Get("Environment").Equals("PROD", StringComparison.OrdinalIgnoreCase) || ConfigurationManager.AppSettings.Get("Environment").Equals("PREPROD", StringComparison.OrdinalIgnoreCase));
            ClaimsIdentity identity = UserService.Identity;
            ClaimSubset identitySubset = identity.ToClaimSubset();
            List<MenuModel> menuItems = MenuItems.Where(i => { return (string.Equals(i.Area, area, StringComparison.OrdinalIgnoreCase) && i.IsAuthorized(identitySubset, isInPreprodorProd)); }).OrderBy(m => m.Area, StringComparer.OrdinalIgnoreCase).ThenBy(m => m.Order).ThenBy(m => m.Name, StringComparer.OrdinalIgnoreCase).ToList();


            IGrouping<string, MenuModel> areaMenuItems = null;
            foreach (IGrouping<string, MenuModel> m1 in menuItems.GroupBy(m => m.Area, StringComparer.OrdinalIgnoreCase).OrderBy(m => m.Key, StringComparer.OrdinalIgnoreCase))
            {
                if (string.Equals(m1.Key, area, StringComparison.OrdinalIgnoreCase))
                {
                    areaMenuItems = m1;
                    break;
                }
            }
            string menu = string.Empty;
            if (areaMenuItems != null && areaMenuItems.Count() > 1)
            {
                StringBuilder str = new StringBuilder(500);
                string controller = html.ViewContext.RouteData.GetController();
                string action = html.ViewContext.RouteData.GetAction();
                MenuModel selectedMenuItem = null;
                for (int index = 0; index < MenuItems.Count; index++)
                {
                    MenuModel m = MenuItems[index];
                    if ((System.String.Equals(m.Area, area, System.StringComparison.OrdinalIgnoreCase)
                         && System.String.Equals(m.Controller, controller, System.StringComparison.OrdinalIgnoreCase)
                         && System.String.Equals(m.Action, action, System.StringComparison.OrdinalIgnoreCase)))
                    {
                        selectedMenuItem = m;
                        break;
                    }
                }
#if DEBUG
                var step = MiniProfiler.Current.Step("MenuService.RenderHierarchy");

                try
                {
#endif

                    RenderHierarchy(html, str, areaMenuItems.ToArray(), null, selectedMenuItem);
                    menu = str.ToString();


                    const string SidebarMinifyBtn = "<li><a href=\"javascript:;\" class=\"sidebar-minify-btn\" data-click=\"sidebar-minify\"><i class=\"fa fa-angle-double-left\"></i><span class=\"readers\">Minimize menu</span></a></li>";
                    menu = menu.Substring(0, menu.LastIndexOf("</ul>")) + SidebarMinifyBtn + "</ul>";
#if DEBUG
                }
                finally
                {
                    if (step != null)
                    {
                        step.Dispose();
                    }
                }
#endif


            }

            return MvcHtmlString.Create(menu);
        }

        private void RenderHierarchy(HtmlHelper html, StringBuilder sb, MenuModel[] hierarchy, MenuModel root, MenuModel selectedMenuItem)
        {
            Random rand;
            // if there are no children under current root.
            if (root != null && !root.HasChildItems)
            {
                return;
            }
            rand = new Random();
            MenuModel[] parentIsRoot = null;
            if (root == null)
            {
                parentIsRoot = hierarchy.Where(i => i.Parent == null).ToArray();
            }
            else
            {
                parentIsRoot = hierarchy.Where(i => i.ParentMenuItemId == root.MenuItemId).ToArray();
            }

            if (parentIsRoot.Length == 0)
            {
                return;
            }
            //Debug.WriteLine("render hierarchy called");

            if (root != null && root.Parent == null)
            {
                // Add sub-menu class on UL that is sub menu.
                sb.Append("<ul class=\"sub-menu\">");
            }
            else if (root == null)
            {
                sb.Append("<ul class=\"nav\"><li class=\"nav-header\">Navigation</li>");
            }
            else
            {
                sb.Append("<ul>");
            }

            for (int index = 0; index < parentIsRoot.Length; index++)
            {
                MenuModel current = parentIsRoot[index];
                List<MenuModel> children = new List<MenuModel>();
                bool hasChildrenItems = false;
                if (current.HasChildItems)
                {
                    for (int i = 0; i < hierarchy.Length; i++)
                    {
                        var x = hierarchy[i];
                        if (x.ParentMenuItemId == current.MenuItemId)
                        {
                            children.Add(x);
                        }
                    }
                    //hierarchy.Where(i => i.ParentMenuItemId == current.MenuItemId);
                    hasChildrenItems = children.Count > 0;
                }

                bool currentSelected = selectedMenuItem != null && selectedMenuItem.ParentMenuItemId == current.MenuItemId;
                bool selectedChild = !currentSelected && hasChildrenItems && selectedMenuItem != null && selectedMenuItem.ParentMenuItemId == current.MenuItemId;

                sb.Append("<li");
                string cssClass = string.Empty;
                // Set active if current item is selected
                if (selectedChild || currentSelected)
                {
                    if (hasChildrenItems)
                    {
                        //sb.Append(" class=\"\"");
                        cssClass = "active has-sub ";
                    }
                }
                else if (hasChildrenItems)
                {
                    //sb.Append(" class=\"has-sub\"");
                    cssClass = " has-sub";
                }
                if (selectedMenuItem != null && selectedMenuItem.MenuItemId == current.MenuItemId)
                {
                    //sb.Append(" class=\"active\"");
                    cssClass += " active";
                }
                if (!string.IsNullOrEmpty(cssClass))
                {
                    sb.Append(string.Format(" class =\"{0}\"", cssClass));
                }
                sb.Append(">");

                // Has children items
                if (hasChildrenItems)
                {
                    if (selectedChild)
                    {
                        sb.Append("<a href=\"javascript:;\" title=\"");
                        sb.Append(current.Name);
                        sb.Append("\"><b class=\"caret pull-right\"></b><span>");
                        sb.Append(current.Name);
                        sb.Append("</span> <span class=\"readers\">(Closes a list below)</span>");
                        //Make sure the span tag is the last element in this anchor tag as jquery code depends on it.
                    }
                    else
                    {
                        //collapse
                        sb.Append("<a href=\"javascript:;\" title=\"");
                        sb.Append(current.Name);
                        sb.Append("\"><b class=\"caret pull-right\"></b><span>");
                        sb.Append(current.Name);
                        sb.Append("</span> <span class=\"readers\">(Opens a list below)</span>");
                        //Make sure the span tag is the last element in this anchor tag as jquery code depends on it.
                    }
                    if (current.Parent == null || current.Parent != root)
                    {
                        sb.Append(string.Format("<i class=\"fa {0}\"></i>", GenRandomClass(rand.Next())));
                    }
                    sb.Append("</a>");
                    // links  
                }
                else
                {
                    var route = new RouteValueDictionary();

                    // Check if we have parameters and if the current route has matching ones
                    if (current.Parameters != null && current.Parameters.Length > 0)
                    {
                        foreach (var parameter in current.Parameters)
                        {
                            if (html.ViewContext.RouteData.Values.ContainsKey(parameter))
                            {
                                route.Add(parameter, html.ViewContext.RouteData.Values[parameter]);
                            }
                        }
                    }

                    sb.Append("<a href=\"");
                    sb.Append(route.Count() > 0 ? string.Format("{0}?{1}", current.StaticUrl, route.ToQueryString()) : current.StaticUrl);
                    sb.Append("\" title=\"");
                    sb.Append(current.Name);
                    sb.Append("\">");
                    sb.Append("<span>" + HttpUtility.HtmlEncode(current.Name) + "</span>");
                    if (current.Parent == null || current.Parent != root)
                    {
                        sb.Append(string.Format("<i class=\"fa {0}\"></i>", GenRandomClass(rand.Next())));
                    }

                    sb.Append("</a>");
                    // link  
                }
                RenderHierarchy(html, sb, hierarchy, current, selectedMenuItem);

                sb.Append("</li>");
                // li        
            }
            sb.Append("</ul>");
        }


        private string GenAreaIconClass(string areaName)
        {
            string cssClass = string.Empty;

            switch (areaName.ToLower())
            {
                case "admin":
                    cssClass = "fa-cogs";
                    break;
                case "example":
                    cssClass = "fa-suitcase";
                    break;
                default:
                    cssClass = "fa-inbox";
                    break;
            }
            return cssClass;
        }


        private string GenRandomClass(int randomNumber)
        {
            string cssClass = "fa-suitcase";
             

            if (randomNumber % 10 == 0)
            {
                cssClass = "fa-file-o";
            }
            else if (randomNumber % 9 == 0)
            {
                cssClass = "fa-th";
            }
            else if (randomNumber % 8 == 0)
            {
                cssClass = "fa-envelope";
            }
            else if (randomNumber % 7 == 0)
            {
                cssClass = "fa-signal";
            }
            else if (randomNumber % 6 == 0)
            {
                cssClass = "fa-calendar";
            }
            else if (randomNumber % 5 == 0)
            {
                cssClass = "fa-map-marker";
            }
            else if (randomNumber % 3 == 0)
            {
                cssClass = "fa-camera";
            }
            else if (randomNumber % 11 == 0)
            {
                cssClass = "fa-align-left";
            }

            return cssClass;
        }
    }
}
