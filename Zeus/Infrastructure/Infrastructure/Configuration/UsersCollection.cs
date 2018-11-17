using System.Configuration;

namespace Employment.Web.Mvc.Infrastructure.Configuration
{
    /// <summary>
    /// Defines 'Users' for Security section <see cref="SecuritySection"/>.
    /// </summary>
    public class UsersCollection : ConfigurationElementCollection
    {

        /// <summary>
        /// Creates a new configuration element.
        /// </summary>
        /// <returns>The config element instance.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new User();
        }

        /// <summary>
        /// Gets config element key.
        /// </summary>
        /// <param name="element">Config element.</param>
        /// <returns>The key.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as User).Name;
        }


        /// <summary>
        /// Access Users by index.
        /// </summary>
        /// <param name="index">index.</param>
        /// <returns>the User at specified index.</returns>
        public User this[int index]
        {
            get
            {
                return (User)BaseGet(index);
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
        /// Get User by key.
        /// </summary>
        /// <param name="key">key.</param>
        /// <returns>User for specified key.</returns>
        public User Get(string key)
        {
            return (User)BaseGet(key);
        }

        /// <summary>
        /// Adds a new User.
        /// </summary>
        /// <param name="newUser">New User to add.</param>
        public void Add(User newUser)
        {
            BaseAdd(newUser);
        }

        /// <summary>
        /// Removes a User.
        /// </summary>
        /// <param name="UserToRemove">User to remove.</param>
        public void Remove(User userToRemove)
        {
            BaseRemove(userToRemove.Name);
        }

        /// <summary>
        /// Remove User at specified index.
        /// </summary>
        /// <param name="index">index.</param>
        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        /// <summary>
        /// Remove User by key.
        /// </summary>
        /// <param name="key">key.</param>
        public void Remove(string key)
        {
            BaseRemove(key);
        }

        /// <summary>
        /// Clear all Users.
        /// </summary>
        public void Clear()
        {
            BaseClear();
        }

    }
}
