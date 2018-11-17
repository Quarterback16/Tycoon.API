using System.Configuration; 

namespace Employment.Web.Mvc.Infrastructure.Configuration
{
    /// <summary>
    /// Defines 'Roles' for Security section <see cref="SecuritySection"/>.
    /// </summary>
    public class RolesCollection : ConfigurationElementCollection
    {

        /// <summary>
        /// Creates a new configuration element.
        /// </summary>
        /// <returns>The config element instance.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new Role();
        }

        /// <summary>
        /// Gets config element key.
        /// </summary>
        /// <param name="element">Config element.</param>
        /// <returns>The key.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as Role).Name;
        }


        /// <summary>
        /// Access Roles by index.
        /// </summary>
        /// <param name="index">index.</param>
        /// <returns>the role at specified index.</returns>
        public Role this[int index]
        {
            get
            {
                return (Role)BaseGet(index);
            }
            set
            {
                if(BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }

                BaseAdd(index, value);
            }
        }


        /// <summary>
        /// Get role by key.
        /// </summary>
        /// <param name="key">key.</param>
        /// <returns>role for specified key.</returns>
        public Role Get(string key)
        {
            return (Role)BaseGet(key);
        }

        /// <summary>
        /// Adds a new Role.
        /// </summary>
        /// <param name="newRole">New Role to add.</param>
        public void Add(Role newRole)
        {
            BaseAdd(newRole);
        }

        /// <summary>
        /// Removes a Role.
        /// </summary>
        /// <param name="roleToRemove">Role to remove.</param>
        public void Remove(Role roleToRemove)
        {
            BaseRemove(roleToRemove.Name);
        }

        /// <summary>
        /// Remove role at specified index.
        /// </summary>
        /// <param name="index">index.</param>
        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        /// <summary>
        /// Remove Role by key.
        /// </summary>
        /// <param name="key">key.</param>
        public void Remove(string key)
        {
            BaseRemove(key);
        }

        /// <summary>
        /// Clear all roles.
        /// </summary>
        public void Clear()
        {
            BaseClear();
        }

    }
}
