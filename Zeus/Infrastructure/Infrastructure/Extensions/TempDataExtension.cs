using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using System.ComponentModel;
using Employment.Web.Mvc.Infrastructure.Helpers;

namespace Employment.Web.Mvc.Infrastructure.Extensions
{
    /// <summary>
    /// Extensions for strongly typed usage of <see cref="TempDataDictionary"/> which is commonly accessed via <see cref="System.Web.Mvc.ControllerBase.TempData"/>.
    /// </summary>
    public static class TempDataExtension
    {
        /// <summary>
        /// Set a value.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="tempData">The TempData instance.</param>
        /// <param name="value">The value to set.</param>
        public static void Set<T>(this TempDataDictionary tempData, T value)
        {
            tempData[typeof(T).FullName] = value;
        }

        /// <summary>
        /// Set a value with a key.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="tempData">The TempData instance.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value to set.</param>
        public static void Set<T>(this TempDataDictionary tempData, string key, T value) 
        {
            tempData[string.Format("{0}.{1}", typeof(T).FullName, key)] = value;
        }

        /// <summary>
        /// Get a value.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="tempData">The TempData instance.</param>
        /// <returns>The value as the specified type.</returns>
        public static T Get<T>(this TempDataDictionary tempData)
        {
            object o;
            tempData.TryGetValue(typeof(T).FullName, out o);

            return o == null ? default(T) : (T)o;
        }

        /// <summary>
        /// Get a value for a key.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="tempData">The TempData instance.</param>
        /// <param name="key">The key.</param>
        /// <returns>The value as the specified type.</returns>
        public static T Get<T>(this TempDataDictionary tempData, string key)
        {
            object o;
            tempData.TryGetValue(string.Format("{0}.{1}", typeof(T).FullName, key), out o);

            return o == null ? default(T) : (T)o;
        }

        /// <summary>
        /// Keep a value for an additional request.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="tempData">The TempData instance.</param>
        public static void Keep<T>(this TempDataDictionary tempData)
        {
            T o = tempData.Get<T>();
            tempData.Set<T>(o);
        }

        /// <summary>
        /// Keep a value with a key for an additional request.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="tempData">The TempData instance.</param>
        /// <param name="key">The key.</param>
        public static void Keep<T>(this TempDataDictionary tempData, string key)
        {
            T o = tempData.Get<T>(key);
            tempData.Set<T>(key, o);
        }
    }
}
