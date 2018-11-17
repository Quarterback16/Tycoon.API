using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Models;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.Wrappers;

namespace Employment.Web.Mvc.Infrastructure.Services
{
    /// <summary>
    /// Defines a service for interacting with data stored in the cache.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class HttpCacheService : ICacheService, IRuntimeCacheService
    {
        //private readonly HashSet<string> excludeFromKeysInUse = new HashSet<string>(StringComparer.Ordinal) { 
        //    "Employment.Web.Mvc.Infrastructure.Services.HistoryService"
        //    ,"Employment.Web.Mvc.Infrastructure.Services.Profiled.HistoryService"
        //    ,"Employment.Web.Mvc.Infrastructure.Services.AdwService"
        //    ,"Employment.Web.Mvc.Infrastructure.Services.Profiled.AdwService"
        //    ,"Employment.Web.Mvc.Infrastructure.Services.BulletinService"
        //    ,"Employment.Web.Mvc.Infrastructure.Services.Profiled.BulletinService"
        //};
        /// <summary>
        /// User service <see cref="IUserService" />
        /// </summary>
        public IUserService UserService
        {
            get
            {
                var containerProvider = DependencyResolver.Current as IContainerProvider;

                return (containerProvider != null) ? containerProvider.GetService<IUserService>() : null;
            }
        }

        private readonly int timeToLiveInMinutes = 10;
       // private readonly KeyModel keysInUseKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpCacheService" /> class.
        /// </summary>
        /// <param name="configurationManager">Configuration manager for interacting with Application Settings.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="configurationManager"/> is <c>null</c>.</exception>
        public HttpCacheService(IConfigurationManager configurationManager)
        {
            if (configurationManager == null)
            {
                throw new ArgumentNullException("configurationManager");
            }

            int i;

            if (int.TryParse(configurationManager.AppSettings.Get("CacheTimeInMinutes"), out i))
            {
                timeToLiveInMinutes = i;
            }

           // keysInUseKey = new KeyModel("KeysInUse") { Namespace = GetType().Namespace };
        }

        /// <summary>
        /// Set an item in the cache under a unique key.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="o">Item to be stored in cache.</param>
        /// <param name="model">Key model.</param>
        /// <exception cref="TypeMismatchException">Thrown if the cache already has an object set with the key and its actual type does not match the expected type.</exception>
        public void Set<T>(KeyModel model, T o)
        {
            Set<T>(model, o, timeToLiveInMinutes);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="o"></param>
        /// <param name="timeToLive"></param>
        public void Set<T>(KeyModel model, T o, TimeSpan timeToLive)
        {
            Set<T>(model, o, timeToLive.Minutes);
        }

        /// <summary>
        /// Set an item in the cache under a unique key.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="o">Item to be stored in cache.</param>
        /// <param name="model">Key model.</param>
        /// <param name="minutes">Number of minutes to store.</param>
        /// <exception cref="TypeMismatchException">Thrown if the cache already has an object set with the key and its actual type does not match the expected type.</exception>
        internal void Set<T>(KeyModel model, T o, int minutes)
        {
            Set<T>(model, o, minutes, CacheItemPriority.Default);
        }

        /// <summary>
        /// Set an item in the cache under a unique key.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="o">Item to be stored in cache.</param>
        /// <param name="model">Key model.</param>
        /// <param name="minutes">Number of minutes to store.</param>
        /// <param name="priority">Priority of the item. Note that <see cref="CacheItemPriority.NotRemovable" /> is only to be used by Infrastructure.</param>
        /// <exception cref="TypeMismatchException">Thrown if the cache already has an object set with the key and its actual type does not match the expected type.</exception>
        internal void Set<T>(KeyModel model, T o, int minutes, CacheItemPriority priority)
        {
            T data;

            if (TryGet(model, out data))
            {
                if (typeof(T) != data.GetType())
                {
                    throw new TypeMismatchException(typeof(T), data.GetType());
                }
            }
            
            var key = model.GetKey();

            //if (!excludeFromKeysInUse.Contains(model.Namespace))
            //{
            //    // Add key to list of keys in use
            //    AddKeyInUse(key);
            //}
	        HttpRuntime.Cache.Insert(key, o, null, UserService.DateTime.AddMinutes(minutes), Cache.NoSlidingExpiration, priority, null);
        }

        /// <summary>
        /// Remove an item from the cache.
        /// </summary>
        /// <param name="model">Key model.</param>
        public void Remove(KeyModel model)
        {
            var key = model.GetKey();

            //if (!excludeFromKeysInUse.Contains(model.Namespace))
            //{
            //    // Remove key from list of keys in use
            //    RemoveKeyInUse(key);
            //}

	        HttpRuntime.Cache.Remove(key);
        }

        ///// <summary>
        ///// Remove all variations of an item from the cache based on a key name.
        ///// </summary>
        ///// <remarks>
        ///// Uses <see cref="CacheType.Default" />.
        ///// </remarks>
        ///// <param name="key">Key used in key model.</param>
        //public void Remove(string key)
        //{
        //    Remove(CacheType.Default, key);
        //}

        ///// <summary>
        ///// Remove all variations of an item from the cache based on a key name.
        ///// </summary>
        ///// <remarks>
        ///// Note that <see cref="CacheServiceWrapper" /> has already included the namespace and cache type in the key value.
        ///// </remarks>
        ///// <param name="key">Key used in key model.</param>
        ///// <param name="cacheType">Cache type to use.</param>
        //public void Remove(CacheType cacheType, string key)
        //{
        //    var keyInUseNamespace = string.Empty;
        //    if (!string.IsNullOrEmpty(key) && key.IndexOf(']') >= 0)
        //    {
        //        keyInUseNamespace=  key.Split(new char[] {'[', ']'},StringSplitOptions.RemoveEmptyEntries)[0];
        //    }
        //   var keysInUse = GetKeysInUse(keyInUseNamespace);

        //    // Remove from cache for each variation of the key name in use
        //    foreach (var k in keysInUse.Where(m => m.StartsWith(key,StringComparison.Ordinal)))
        //    {
        //        HttpRuntime.Cache.Remove(k);
        //    }

        //    // Remove variations of key from list of keys in use
        //    RemoveKeyInUseVariations(key);
        //}

        /// <summary>
        /// Checks if the item is stored in the cache.
        /// </summary>
        /// <param name="model">Key model.</param>
        /// <returns>true if the item is stored in the cache; otherwise, false.</returns>
        public bool Contains(KeyModel model)
        {
            return HttpRuntime.Cache[model.GetKey()] != null;
        }

        /// <summary>
        /// Try to get a stored object.
        /// </summary>
        /// <typeparam name="T">Type of stored item.</typeparam>
        /// <param name="model">Key model.</param>
        /// <param name="value">Stored value.</param>
        /// <returns>Stored item as type.</returns>
        public bool TryGet<T>(KeyModel model, out T value)
        {
            try
            {
                if (!Contains(model))
                {
                    value = default(T);

                    return false;
                }

                value = (T)HttpRuntime.Cache[model.GetKey()];
            }
            catch
            {
                value = default(T);

                return false;
            }

            return true;
        }

        ///// <summary>
        ///// Try to get all stored objects that belong to they specified key.
        ///// </summary>
        ///// <remarks>
        ///// Uses <see cref="CacheType.Default" />.
        ///// </remarks>
        ///// <typeparam name="T">Type of stored item.</typeparam>
        ///// <param name="key">Key used in key model.</param>
        ///// <param name="value">Stored values.</param>
        ///// <returns>Stored items as type.</returns>
        //public bool TryGet<T>(string key, out IEnumerable<T> value)
        //{
        //    return TryGet(CacheType.Default, key, out value);
        //}

        ///// <summary>
        ///// Try to get all stored objects that belong to they specified key.
        ///// </summary>
        ///// <remarks>
        ///// Note that <see cref="CacheServiceWrapper" /> has already included the namespace and cache type in the key value.
        ///// </remarks>
        ///// <param name="cacheType">Cache type to use.</param>
        ///// <typeparam name="T">Type of stored item.</typeparam>
        ///// <param name="key">Key used in key model.</param>
        ///// <param name="value">Stored values.</param>
        ///// <returns>Stored items as type.</returns>
        //public bool TryGet<T>(CacheType cacheType, string key, out IEnumerable<T> value)
        //{
        //    var keyInUseNamespace = string.Empty;
        //    if (!string.IsNullOrEmpty(key) && key.IndexOf(']') >= 0)
        //    {
        //        keyInUseNamespace=  key.Split(new char[] {'[', ']'},StringSplitOptions.RemoveEmptyEntries)[0];
        //    }
        //    value = Enumerable.Empty<T>();
        //    var values = new List<T>();
        //    var keysInUse = GetKeysInUse(keyInUseNamespace);

        //    // Get from cache each variation of the key name in use
        //    foreach (var k in keysInUse.Where(m => m.StartsWith(key,StringComparison.Ordinal)))
        //    {
        //        if (HttpRuntime.Cache[k] != null)
        //        {
        //            try
        //            {
        //                values.Add((T)HttpRuntime.Cache[k]);
        //            }
        //            catch
        //            {
        //                return false;
        //            }
        //        }
        //    }

        //    value = values.AsEnumerable();

        //    return true;
        //}

        //#region Keys in use management

        //private void AddKeyInUse(string key)
        //{
        //    var keyInUseNamespace = string.Empty;
        //    if (!string.IsNullOrEmpty(key) && key.IndexOf(']') >= 0)
        //    {
        //        keyInUseNamespace=  key.Split(new char[] {'[', ']'},StringSplitOptions.RemoveEmptyEntries)[0];
        //    }

        //    var keysInUse = GetKeysInUse(keyInUseNamespace);

        //    keysInUse.Add(key);

        //    HttpRuntime.Cache.Insert(string.Format("{0}_{1}", keyInUseNamespace, keysInUseKey.GetKey()), keysInUse, null, DateTime.MaxValue, new TimeSpan(12, 0, 0), CacheItemPriority.Default, null);
        //}

        //private void RemoveKeyInUse(string key)
        //{
        //    var keyInUseNamespace = string.Empty;
        //    if (!string.IsNullOrEmpty(key) && key.IndexOf(']') >= 0)
        //    {
        //        keyInUseNamespace=  key.Split(new char[] {'[', ']'},StringSplitOptions.RemoveEmptyEntries)[0];
        //    }

        //    var keysInUse = GetKeysInUse(keyInUseNamespace);

        //    keysInUse.Remove(key);

        //    HttpRuntime.Cache.Insert(string.Format("{0}_{1}", keyInUseNamespace, keysInUseKey.GetKey()), keysInUse, null, DateTime.MaxValue, new TimeSpan(12, 0, 0), CacheItemPriority.Default, null);
        //}

        //private void RemoveKeyInUseVariations(string key)
        //{
        //    var keyInUseNamespace = string.Empty;
        //    if (!string.IsNullOrEmpty(key) && key.IndexOf(']') >= 0)
        //    {
        //        keyInUseNamespace=  key.Split(new char[] {'[', ']'},StringSplitOptions.RemoveEmptyEntries)[0];
        //    }

        //   var keysInUse = GetKeysInUse(keyInUseNamespace);

        //    keysInUse.RemoveWhere(m => m.StartsWith(key,StringComparison.Ordinal));

        //    HttpRuntime.Cache.Insert(string.Format("{0}_{1}", keyInUseNamespace, keysInUseKey.GetKey()), keysInUse, null, DateTime.MaxValue, new TimeSpan(12, 0, 0), CacheItemPriority.Default, null);
        //}

        //private HashSet<string> GetKeysInUse(string keyInUseNamespace)
        //{
        //    HashSet<string> keys;

        //    tryGetKeysInUse(keysInUseKey,keyInUseNamespace, out keys);

        //    return keys ?? new HashSet<string>();
        //}

        //private bool tryGetKeysInUse<T>(KeyModel model,string keyInUseNamespace, out T value)
        //{
        //    try
        //    {
        //        if (!Contains(model))
        //        {
        //            value = default(T);

        //            return false;
        //        }

        //        value = (T)HttpRuntime.Cache[string.Format("{0}_{1}", keyInUseNamespace, model.GetKey())];
        //    }
        //    catch
        //    {
        //        value = default(T);

        //        return false;
        //    }

        //    return true;
        //}

        //#endregion
    }
}
