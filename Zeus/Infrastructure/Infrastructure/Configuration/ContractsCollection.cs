using System.Configuration;

namespace Employment.Web.Mvc.Infrastructure.Configuration
{
    /// <summary>
    /// Defines 'Contracts' for Security section <see cref="SecuritySection"/>.
    /// </summary>
    public class ContractsCollection : ConfigurationElementCollection
    {

        /// <summary>
        /// Creates a new configuration element.
        /// </summary>
        /// <returns>The config element instance.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new Contract();
        }

        /// <summary>
        /// Gets config element key.
        /// </summary>
        /// <param name="element">Config element.</param>
        /// <returns>The key.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as Contract).Name;
        }


        /// <summary>
        /// Access Contracts by index.
        /// </summary>
        /// <param name="index">index.</param>
        /// <returns>the Contract at specified index.</returns>
        public Contract this[int index]
        {
            get
            {
                return (Contract)BaseGet(index);
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
        /// Get Contract by key.
        /// </summary>
        /// <param name="key">key.</param>
        /// <returns>Contract for specified key.</returns>
        public Contract Get(string key)
        {
            return (Contract)BaseGet(key);
        }

        /// <summary>
        /// Adds a new Contract.
        /// </summary>
        /// <param name="newContract">New Contract to add.</param>
        public void Add(Contract newContract)
        {
            BaseAdd(newContract);
        }

        /// <summary>
        /// Removes a Contract.
        /// </summary>
        /// <param name="ContractToRemove">Contract to remove.</param>
        public void Remove(Contract contractToRemove)
        {
            BaseRemove(contractToRemove.Name);
        }

        /// <summary>
        /// Remove Contract at specified index.
        /// </summary>
        /// <param name="index">index.</param>
        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        /// <summary>
        /// Remove Contract by key.
        /// </summary>
        /// <param name="key">key.</param>
        public void Remove(string key)
        {
            BaseRemove(key);
        }

        /// <summary>
        /// Clear all Contracts.
        /// </summary>
        public void Clear()
        {
            BaseClear();
        }

    }
}

