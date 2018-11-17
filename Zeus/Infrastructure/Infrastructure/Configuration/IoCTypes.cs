using System.Configuration;

namespace Employment.Web.Mvc.Infrastructure.Configuration
{
    /// <summary>
    /// Defines the configuration element collection for menu tiles used by <see cref="MenuSection" />.
    /// </summary>
    public class IoCTypes : ConfigurationElementCollection
    {
        /// <summary>
        /// Create a new configuration element.
        /// </summary>
        /// <returns>The new configuration element instance.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new IoCType();
        }

        /// <summary>
        /// Get configuration element key.
        /// </summary>
        /// <param name="element">The configuration element.</param>
        /// <returns>The key of the configuration element.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((IoCType)element).Name;
        }

        /// <summary>
        /// Access history routes by index.
        /// </summary>
        /// <param name="index">The history route index value.</param>
        /// <returns>The history route at the specified index.</returns>
        public IoCType this[int index]
        {
            get
            {
                return (IoCType)BaseGet(index);
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
        /// Get a history route by key.
        /// </summary>
        /// <param name="key">The history type key value.</param>
        /// <returns>The history route with the specified key.</returns>
        public IoCType Get(string key)
        {
            return (IoCType)BaseGet(key);
        }

        /// <summary>
        /// Add a history route.
        /// </summary>
        /// <param name="historyTile">The history route to add.</param>
        public void Add(IoCType historyTile)
        {
            BaseAdd(historyTile);
        }

        /// <summary>
        /// Clear all history routes.
        /// </summary>
        public void Clear()
        {
            BaseClear();
        }

        /// <summary>
        /// Remove history route.
        /// </summary>
        /// <param name="historyTile">The history route to remove.</param>
        public void Remove(IoCType historyTile)
        {
            BaseRemove(historyTile.Name);
        }

        /// <summary>
        /// Remove history route at index.
        /// </summary>
        /// <param name="index">The history route index value.</param>
        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        /// <summary>
        /// Remove history route by key.
        /// </summary>
        /// <param name="key">The history route key value.</param>
        public void Remove(string key)
        {
            BaseRemove(key);
        }
    }
}