using System.Configuration;

namespace Employment.Web.Mvc.Infrastructure.Configuration
{
    /// <summary>
    /// Defines the configuration section for the IoC code.
    /// </summary>
    public class IoCSection : ConfigurationSection
    {
        /// <summary>
        /// Tiles for each menu item.
        /// </summary>
        [ConfigurationProperty("types", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(IoCTypes), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public IoCTypes Types
        {
            get
            {
                return (IoCTypes)this["types"];
            }
            set
            {
                this["types"] = value;
            }
        }
    }
}