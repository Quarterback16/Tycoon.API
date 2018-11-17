using System.Configuration;

namespace Employment.Web.Mvc.Infrastructure.Configuration
{
    /// <summary>
    /// Defines the configuration section for the landing page menu.
    /// </summary>
    public class MenuSection : ConfigurationSection
    {
        /// <summary>
        /// Tiles for each menu item.
        /// </summary>
        [ConfigurationProperty("tiles", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(MenuTiles), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public MenuTiles Tiles
        {
            get
            {
                return (MenuTiles)this["tiles"];
            }
            set
            {
                this["tiles"] = value;
            }
        }
    }
}
