using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Models;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.Wrappers;
using Microsoft.ApplicationServer.Caching;

namespace Employment.Web.Mvc.Infrastructure.Services
{
    /// <summary>
    /// Defines a service for interacting with App Fabric cache.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AppFabricCacheService : ICacheService, IRuntimeCacheService
    {
        //private readonly KeyModel keysInUseKey;
        private readonly DataCacheFactory cacheFactory = new DataCacheFactory();
        private readonly IConfigurationManager ConfigurationManager;

        //private readonly HashSet<string> excludeFromKeysInUse = new HashSet<string>(StringComparer.Ordinal) { 
        //    "Employment.Web.Mvc.Infrastructure.Services.HistoryService"
        //    ,"Employment.Web.Mvc.Infrastructure.Services.Profiled.HistoryService"
        //    ,"Employment.Web.Mvc.Infrastructure.Services.AdwService"
        //    ,"Employment.Web.Mvc.Infrastructure.Services.Profiled.AdwService"
        //    ,"Employment.Web.Mvc.Infrastructure.Services.BulletinService"
        //    ,"Employment.Web.Mvc.Infrastructure.Services.Profiled.BulletinService"
        //};
        private DataCache Cache { get
        {
            var environment = ConfigurationManager.AppSettings.Get("Environment");
            var cache = cacheFactory.GetCache(environment);

            if (cache == null)
            {
                throw new InvalidOperationException(string.Format("Could not get App Fabric cache {0}.", environment));
            }

            return cache;
        }}

        /// <summary>
        /// Initializes a new instance of the <see cref="AppFabricCacheService" /> class.
        /// </summary>
        /// <param name="configurationManager">Configuration manager for interacting with Application Settings.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="configurationManager"/> is <c>null</c>.</exception>
        public AppFabricCacheService(IConfigurationManager configurationManager)
        {
            if (configurationManager == null)
            {
                throw new ArgumentNullException("configurationManager");
            }

            ConfigurationManager = configurationManager;

            //keysInUseKey = new KeyModel("KeysInUse") { Namespace = GetType().Namespace };
        }

        /// <summary>
        /// Set an item in the cache under a unique key.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="model">Key model.</param>
        /// <param name="o">Item to be stored in cache.</param>
        public void Set<T>(KeyModel model, T o)
        {
            var key = model.GetKey();
            
            try
            {
                Cache.Put(key, o);

                //if (!excludeFromKeysInUse.Contains(model.Namespace))
                //{
                //    // Add key to list of keys in use
                //    AddKeyInUse(key);
                //}
            }
            catch(DataCacheException e)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(e);
            }
        }

        /// <summary>
        /// Set an item in the cache under a unique key for a set amount of time.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="model">Key model.</param>
        /// <param name="o">Item to be stored in cache.</param>
        /// <param name="timeToLive">Time for item to live in cache.</param>
        public void Set<T>(KeyModel model, T o, TimeSpan timeToLive)
        {
            var key = model.GetKey();

            try
            {
                Cache.Put(key, o, timeToLive);

                //if (!excludeFromKeysInUse.Contains(model.Namespace))
                //{
                //    // Add key to list of keys in use
                //    AddKeyInUse(key);
                //}
            }
            catch(DataCacheException e)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(e);
            }
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

	        try
            {
                Cache.Remove(key);
            }
            catch(DataCacheException e)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(e);
            }
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
        ///// <param name="cacheType">Cache type to use.</param>
        ///// <param name="key">Key used in key model.</param>
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
        //        try
        //        {
        //            Cache.Remove(k);
        //        }
        //        catch (DataCacheException e)
        //        {
        //            Elmah.ErrorSignal.FromCurrentContext().Raise(e);
        //        }
        //    }

        //    // Remove variations of key from list of keys in use
        //    RemoveKeyInUseVariations(key);
        //}

        /// <summary>
        /// Checks if the item is stored in the cache.
        /// </summary>
        /// <param name="model">Key model.</param>
        /// <returns><c>true</c> if the item is stored in the cache; otherwise, <c>false</c>.</returns>
        public bool Contains(KeyModel model)
        {
            try
            {
                var item = Cache.Get(model.GetKey());

                return (item != null);
            }
            catch(DataCacheException e)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(e);
            }

            return false;
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
                value = (T) Cache.Get(model.GetKey());
            }
            catch(DataCacheException e)
            {
                value = default(T);

                Elmah.ErrorSignal.FromCurrentContext().Raise(e);
            }

            return !Equals(value, default(T));
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
        //    value = Enumerable.Empty<T>();
        //    var values = new List<T>();

        //    var keyInUseNamespace = string.Empty;
        //    if (!string.IsNullOrEmpty(key) && key.IndexOf(']') >= 0)
        //    {
        //        keyInUseNamespace=  key.Split(new char[] {'[', ']'},StringSplitOptions.RemoveEmptyEntries)[0];
        //    }

        //    var keysInUse = GetKeysInUse(keyInUseNamespace);

        //    // Get from cache each variation of the key name in use
        //    foreach (var k in keysInUse.Where(m => m.StartsWith(key,StringComparison.Ordinal)))
        //    {
        //        try
        //        {
        //            var obj = Cache.Get(k);

        //            if (obj != null)
        //            {
        //                try
        //                {
        //                    values.Add((T)obj);
        //                }
        //                catch
        //                {
        //                    return false;
        //                }
        //            }
        //        }
        //        catch(DataCacheException e)
        //        {
        //            Elmah.ErrorSignal.FromCurrentContext().Raise(e);

        //            return false;
        //        }
        //    }

        //    value = values.AsEnumerable();

        //    return true;
        //}

        //#region Keys in use management

        //private void AddKeyInUse(string key)
        //{
        //    var keyInUseNamespace = string.Empty;//actually keysinuseclass
        //    if (!string.IsNullOrEmpty(key) && key.IndexOf(']') >= 0)
        //    {
        //        keyInUseNamespace=  key.Split(new char[] {'[', ']'},StringSplitOptions.RemoveEmptyEntries)[0];
        //    }

        //    var keysInUse = GetKeysInUse(keyInUseNamespace);

        //    keysInUse.Add(key);

        //    try
        //    {
        //        Cache.Put(string.Format("{0}_{1}", keyInUseNamespace, keysInUseKey.GetKey()), keysInUse, new TimeSpan(12, 0, 0));
        //    }
        //    catch (DataCacheException e)
        //    {
        //        Elmah.ErrorSignal.FromCurrentContext().Raise(e);
        //    }
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

        //    try
        //    {
        //        Cache.Put(string.Format("{0}_{1}", keyInUseNamespace, keysInUseKey.GetKey()), keysInUse, new TimeSpan(12, 0, 0));
        //    }
        //    catch(DataCacheException e)
        //    {
        //        Elmah.ErrorSignal.FromCurrentContext().Raise(e);
        //    }

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

        //    try
        //    {
        //        Cache.Put(string.Format("{0}_{1}", keyInUseNamespace, keysInUseKey.GetKey()), keysInUse, new TimeSpan(12, 0, 0));
        //    }
        //    catch (DataCacheException e)
        //    {
        //        Elmah.ErrorSignal.FromCurrentContext().Raise(e);
        //    }

        //}

        //private HashSet<string> GetKeysInUse(string keyInUseNamespace)
        //{
        //    HashSet<string> keys;

        //    tryGetKeys(keysInUseKey, keyInUseNamespace, out keys);

        //    return keys ?? new HashSet<string>();
        //}


        // private bool tryGetKeys<T>(KeyModel model,string keyInUseNamespace, out T value)
        //{
        //    try
        //    {
        //        value = (T) Cache.Get(string.Format("{0}_{1}", keyInUseNamespace, model.GetKey()));
        //    }
        //    catch(DataCacheException e)
        //    {
        //        value = default(T);

        //        Elmah.ErrorSignal.FromCurrentContext().Raise(e);
        //    }

        //    return !Equals(value, default(T));
        //}

        //#endregion
    }
}