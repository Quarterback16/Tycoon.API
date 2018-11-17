using System.Configuration;

namespace Employment.Web.Mvc.Infrastructure.Configuration
{
    /// <summary>
    /// Defines 'OrgCodes' for Security section <see cref="SecuritySection"/>.
    /// </summary>
    public class OrgCodesCollection : ConfigurationElementCollection
    {

        /// <summary>
        /// Creates a new configuration element.
        /// </summary>
        /// <returns>The config element instance.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new OrgCode();
        }

        /// <summary>
        /// Gets config element key.
        /// </summary>
        /// <param name="element">Config element.</param>
        /// <returns>The key.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as OrgCode).Name;
        }


        /// <summary>
        /// Access OrgCodes by index.
        /// </summary>
        /// <param name="index">index.</param>
        /// <returns>the OrgCode at specified index.</returns>
        public OrgCode this[int index]
        {
            get
            {
                return (OrgCode)BaseGet(index);
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
        /// Get OrgCode by key.
        /// </summary>
        /// <param name="key">key.</param>
        /// <returns>OrgCode for specified key.</returns>
        public OrgCode Get(string key)
        {
            return (OrgCode)BaseGet(key);
        }

        /// <summary>
        /// Adds a new OrgCode.
        /// </summary>
        /// <param name="newOrgCode">New OrgCode to add.</param>
        public void Add(OrgCode newOrgCode)
        {
            BaseAdd(newOrgCode);
        }

        /// <summary>
        /// Removes a OrgCode.
        /// </summary>
        /// <param name="OrgCodeToRemove">OrgCode to remove.</param>
        public void Remove(OrgCode OrgCodeToRemove)
        {
            BaseRemove(OrgCodeToRemove.Name);
        }

        /// <summary>
        /// Remove OrgCode at specified index.
        /// </summary>
        /// <param name="index">index.</param>
        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        /// <summary>
        /// Remove OrgCode by key.
        /// </summary>
        /// <param name="key">key.</param>
        public void Remove(string key)
        {
            BaseRemove(key);
        }

        /// <summary>
        /// Clear all OrgCodes.
        /// </summary>
        public void Clear()
        {
            BaseClear();
        }

    }
}
