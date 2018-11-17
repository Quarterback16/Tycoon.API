using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Configuration;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Models;

namespace Employment.Web.Mvc.Infrastructure.Interfaces
{
    /// <summary>
    /// Defines the methods and properties that are required for a Menu Service.
    /// </summary>
    public interface IMenuService
    {
        ReadOnlyCollection<MenuModel> MenuItems { get; }
        ReadOnlyCollection<MenuTile> AreaTiles { get; }
        ReadOnlyCollection<MenuTile> VisibleAreaTiles { get; }
        ReadOnlyDictionary<string, string> AreaNames { get; }

        MvcHtmlString ShowMenu(HtmlHelper html);

        ///// <summary>
        ///// Menu Items
        ///// </summary>
        //ReadOnlyDictionary<string, MenuModel> MenuItemsDictionary { get; }

    }
}
