using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using Employment.Web.Mvc.Infrastructure.Configuration;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Models;
using Employment.Web.Mvc.Infrastructure.Types;
using StackExchange.Profiling;

namespace Employment.Web.Mvc.Infrastructure.Services.Profiled
{
    /// <summary>
    /// Defines a service for accessing Menu data.
    /// </summary>
    /// <remarks>
    /// Profiled version of <see cref="Services.MenuService" />.
    /// </remarks>
    public class MenuService : Services.MenuService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MenuService" /> class.
        /// </summary>
        /// <param name="configurationManager">Configuration manager for interacting with the Web configuration.</param>
        /// <param name="client">Client for interacting with WCF services.</param>
        /// <param name="cacheService">Cache service for interacting with cached data.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="configurationManager" /> is <c>null</c>.</exception>
        public MenuService(IConfigurationManager configurationManager, IClient client,  ICacheService cacheService) : base(configurationManager, client,  cacheService) { }

        /// <summary>
        /// Visible area tiles.
        /// </summary>
        /// <remarks>
        /// Cannot store in static as IsAuthorized needs to run for each individual user, storing would apply the first users IsAuthorized result to all other users.
        /// </remarks>
        public override ReadOnlyCollection<MenuTile> VisibleAreaTiles
        {
            get
            {
                using (MiniProfiler.Current.Step("MenuService.VisibleAreaTiles"))
                {
                    return base.VisibleAreaTiles;
                }
            }
        }

        /// <summary>
        /// Shows the menu items.
        /// </summary>
        /// <param name="html">An instance of the HTML helper.</param>
        public override MvcHtmlString ShowMenu(HtmlHelper html)
        {
            using (MiniProfiler.Current.Step("MenuService.ShowMenu"))
            {
                return base.ShowMenu(html);
            }
        }
    }
}
