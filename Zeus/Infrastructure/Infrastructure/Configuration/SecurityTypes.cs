using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Employment.Web.Mvc.Infrastructure.Configuration
{
    
    /// <summary>
    /// Represents 'Security Types' section in web config.
    /// </summary>
    public class SecurityTypes : ConfigurationElementCollection
    {

        /// <summary>
        /// Create a new configuration element.
        /// </summary>
        /// <returns>The new configuration element instance.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new SecuritySection();
        }

        /// <summary>
        /// Get configuration element key.
        /// </summary>
        /// <param name="element">The configuration element.</param>
        /// <returns>The key of the configuration element.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SecuritySection)element).Name;
        }

        /// <summary>
        /// Get Security section by index.
        /// </summary>
        /// <param name="index">index value.</param>
        /// <returns>The Security section at the specified index.</returns>
        public SecuritySection this[int index]
        {
            get
            {
                return (SecuritySection)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }

                BaseAdd(index, value);
            }
        }

        /// <summary>
        /// Get a Security SubSection by key.
        /// </summary>
        /// <param name="key">The SecuritySection key value.</param>
        /// <returns>The Security SubSection with the specified key.</returns>
        public SecuritySection Get(string key)
        {
            return (SecuritySection)BaseGet(key);
        }

        /// <summary>
        /// Add a Security SubSection route.
        /// </summary>
        /// <param name="SecuritySection">The SecuritySection to add.</param>
        public void Add(SecuritySection SecuritySection)
        {
            BaseAdd(SecuritySection);
        }

        /// <summary>
        /// Clear all security SubSections.
        /// </summary>
        public void Clear()
        {
            BaseClear();
        }

        /// <summary>
        /// Remove SecuritySection.
        /// </summary>
        /// <param name="SecuritySection">The SecuritySection to remove.</param>
        public void Remove(SecuritySection SecuritySection)
        {
            BaseRemove(SecuritySection.Name);
        }

        /// <summary>
        /// Remove security SubSection at index.
        /// </summary>
        /// <param name="index">The SecuritySection index value.</param>
        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        /// <summary>
        /// Remove security SubSection by key.
        /// </summary>
        /// <param name="key">The security SubSection key value.</param>
        public void Remove(string key)
        {
            BaseRemove(key);
        }
    }
    
}
