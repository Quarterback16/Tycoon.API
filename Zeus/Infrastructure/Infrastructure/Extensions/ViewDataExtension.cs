using System.Web.Mvc;

namespace Employment.Web.Mvc.Infrastructure.Extensions
{
    /// <summary>
    /// Extensions for strongly typed usage of <see cref="ViewDataDictionary"/>.
    /// </summary>
    public static class ViewDataExtension
    {
        /// <summary>
        /// Set a value.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="viewData">The ViewData instance.</param>
        /// <param name="value">The value to set.</param>
        public static void Set<T>(this ViewDataDictionary viewData, T value)
        {
            viewData[typeof(T).FullName] = value;
        }

        /// <summary>
        /// Set a value with a key.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="viewData">The ViewData instance.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value to set.</param>
        public static void Set<T>(this ViewDataDictionary viewData, string key, T value) 
        {
            viewData[string.Format("{0}.{1}", typeof(T).FullName, key)] = value;
        }

        /// <summary>
        /// Get a value.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="viewData">The ViewData instance.</param>
        /// <returns>The value as the specified type.</returns>
        public static T Get<T>(this ViewDataDictionary viewData)
        {
            object o;
            viewData.TryGetValue(typeof(T).FullName, out o);

            return o == null ? default(T) : (T)o;
        }

        /// <summary>
        /// Get a value for a key.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="viewData">The ViewData instance.</param>
        /// <param name="key">The key.</param>
        /// <returns>The value as the specified type.</returns>
        public static T Get<T>(this ViewDataDictionary viewData, string key)
        {
            object o;
            viewData.TryGetValue(string.Format("{0}.{1}", typeof(T).FullName, key), out o);

            return o == null ? default(T) : (T)o;
        }
    }
}
