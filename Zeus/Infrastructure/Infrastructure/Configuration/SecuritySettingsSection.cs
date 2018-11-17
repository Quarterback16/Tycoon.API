using System.Configuration;

namespace Employment.Web.Mvc.Infrastructure.Configuration
{
    /// <summary>
    /// Defines the configuration element collection for security settings <see cref="SecuritySection"/>.
    /// </summary>
    public class SecuritySettingsSection : ConfigurationSection  
    {
         

        private const string SecuritytypesKey = "securityTypes";

        /// <summary>
        /// Security Section.
        /// </summary>
        [ConfigurationProperty(SecuritytypesKey, IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(SecurityTypes), AddItemName = "security")]
        public SecurityTypes SecurityTypes
        {
            get
            {
                return (SecurityTypes)this[SecuritytypesKey];
            }
            set
            {
                this[SecuritytypesKey] = value;
            }
        }
        
        
    }
}
