using System.Configuration;

namespace Employment.Web.Mvc.Infrastructure.Configuration
{
    /// <summary>
    /// Defines the configuration element for a menu tile used by <see cref="MenuTiles" />.
    /// </summary>
    public class MenuTile : ConfigurationElement
    {
        /// <summary>
        /// Area name.
        /// </summary>
        [ConfigurationProperty("areaName", IsKey = true, IsRequired = true)]
        public string AreaName
        {
            get
            {
                return (string)this["areaName"];
            }
            set
            {
                this["areaName"] = value;
            }
        }

        /// <summary>
        /// Display name.
        /// </summary>
        [ConfigurationProperty("displayName", IsRequired = true)]
        public string DisplayName
        {
            get
            {
                return (string)this["displayName"];
            }
            set
            {
                this["displayName"] = value;
            }
        }

        /// <summary>
        /// Hidden.
        /// </summary>
        [ConfigurationProperty("hidden", DefaultValue = "false")]
        public bool Hidden
        {
            get
            {
                return (bool)this["hidden"];
            }
            set
            {
                this["hidden"] = value;
            }
        }

        /// <summary>
        /// Order.
        /// </summary>
        [ConfigurationProperty("order", DefaultValue = int.MaxValue)]
        public int Order
        {
            get
            {
                return (int)this["order"];
            }
            set
            {
                this["order"] = value;
            }
        }

        /// <summary>
        /// Route name.
        /// </summary>
        [ConfigurationProperty("routeName")]
        public string RouteName
        {
            get
            {
                return (string)this["routeName"];
            }
            set
            {
                this["routeName"] = value;
            }
        }
    }
}
