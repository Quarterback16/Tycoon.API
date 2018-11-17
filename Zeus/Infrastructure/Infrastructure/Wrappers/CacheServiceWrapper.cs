using System;
using System.Collections.Generic;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Models;
using Employment.Web.Mvc.Infrastructure.Services;
using Employment.Web.Mvc.Infrastructure.Types;
#if DEBUG
using StackExchange.Profiling;
#endif

namespace Employment.Web.Mvc.Infrastructure.Wrappers
{
    /// <summary>
    /// Defines a wrapper of <see cref="ICacheService" /> which is used by a <see cref="Service" />. This wrapper will set the namespace of the <see cref="KeyModel" /> to that of the current <see cref="Service" /> namespace.
    /// </summary>
    /// <remarks>
    /// Design rule is that each Service must inherit from <see cref="Service" /> and each service is only able to access the cache of that service via the service namespace.
    /// This wrapper is the only way to properly use caching as it is the only way the <see cref="KeyModel.Namespace" /> can be set (both that property and this class are internal) which is by design to enforce the namespacing and use of caching only within services.
    /// </remarks>
    internal class CacheServiceWrapper : ICacheService
    {
        private readonly ICacheService CacheService;
        private readonly string Namespace;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheServiceWrapper" /> class.
        /// </summary>
        /// <param name="cacheService">Cache service for interacting with cached data.</param>
        /// <param name="namespace">Namespace of the <see cref="Service" /> using the current <see cref="CacheServiceWrapper"/>instance.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="cacheService"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="namespace"/> is <c>null</c>.</exception>
        public CacheServiceWrapper(ICacheService cacheService, string @namespace)
        {
            if (cacheService == null)
            {
                throw new ArgumentNullException("cacheService");
            }

            if (string.IsNullOrEmpty(@namespace))
            {
                throw new ArgumentNullException("namespace");
            }

            CacheService = cacheService;
            Namespace = @namespace;
        }

        /// <summary>
        /// Set an item in the cache under a unique key.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="model">Key model.</param>
        /// <param name="o">Item to be stored in cache.</param>
        /// <exception cref="TypeMismatchException">Thrown if the cache already has an object set with the key and its actual type does not match the expected type.</exception>
        public void Set<T>(KeyModel model, T o)
        {
            // Set namespace of model
            model.Namespace = Namespace;

            // Call actual Cache Service
#if DEBUG
            var step = MiniProfiler.Current.Step(string.Format("CacheService: Set {0} {1} {2}", model.CacheType, model.Key, model.GetValuesKey()));

            try
            {
#endif
                CacheService.Set(model, o);
#if DEBUG
            }
            finally
            {
                if (step != null)
                {
                    step.Dispose();
                }
            }
#endif
        }


        /// <summary>
        /// Set an item in the cache under a unique key.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="model">Key model.</param>
        /// <param name="o">Item to be stored in cache.</param>
        /// <param name="timeToLive"></param>
        public void Set<T>(KeyModel model, T o, TimeSpan timeToLive)
        {
            // Set namespace of model
            model.Namespace = Namespace;

            // Call actual Cache Service
#if DEBUG
            var step = MiniProfiler.Current.Step(string.Format("CacheService: Set {0} {1} {2}", model.CacheType, model.Key, model.GetValuesKey()));

            try
            {
#endif
                CacheService.Set(model, o, timeToLive);
#if DEBUG
            }
            finally
            {
                if (step != null)
                {
                    step.Dispose();
                }
            }
#endif
        }

        /// <summary>
        /// Remove an item from the cache.
        /// </summary>
        /// <param name="model">Key model.</param>
        public void Remove(KeyModel model)
        {
            // Set namespace of model
            model.Namespace = Namespace;

            // Call actual Cache Service
#if DEBUG
            var step = MiniProfiler.Current.Step(string.Format("CacheService: Remove {0} {1} {2}", model.CacheType, model.Key, model.GetValuesKey()));

            try
            {
#endif
                CacheService.Remove(model);
#if DEBUG
            }
            finally
            {
                if (step != null)
                {
                    step.Dispose();
                }
            }
#endif
        }

//        /// <summary>
//        /// Remove all variations of an item from the cache based on a key name.
//        /// </summary>
//        /// <remarks>
//        /// Uses <see cref="CacheType.Default" />.
//        /// </remarks>
//        /// <param name="key">Key used in key model.</param>
//        public void Remove(string key)
//        {
//            Remove(CacheType.Default, key);
//        }

//        /// <summary>
//        /// Remove all variations of an item from the cache based on a key name.
//        /// </summary>
//        /// <param name="cacheType">Cache type to use.</param>
//        /// <param name="key">Key used in key model.</param>
//        public void Remove(CacheType cacheType, string key)
//        {
//            var model = new KeyModel(cacheType, key);

//            // Set namespace of model
//            model.Namespace = Namespace;

//            // Call actual Cache Service
//#if DEBUG
//            var step = MiniProfiler.Current.Step(string.Format("CacheService: Remove {0} {1}", model.CacheType, model.Key));

//            try
//            {
//#endif
//                CacheService.Remove(cacheType, model.GetKey());
//#if DEBUG
//            }
//            finally
//            {
//                if (step != null)
//                {
//                    step.Dispose();
//                }
//            }
//#endif
//        }

        /// <summary>
        /// Checks if the item is stored in the cache.
        /// </summary>
        /// <param name="model">Key model.</param>
        /// <returns><c>true</c> if the item is stored in the cache; otherwise, <c>false</c>.</returns>
        public bool Contains(KeyModel model)
        {
            // Set namespace of model
            model.Namespace = Namespace;

            // Call actual Cache Service
#if DEBUG
            var step = MiniProfiler.Current.Step(string.Format("CacheService: Contains {0} {1} {2}", model.CacheType, model.Key, model.GetValuesKey()));

            try
            {
#endif
                return CacheService.Contains(model);
#if DEBUG
            }
            finally
            {
                if (step != null)
                {
                    step.Dispose();
                }
            }
#endif
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
            // Set namespace of model
            model.Namespace = Namespace;

            // Call actual Cache Service
#if DEBUG
            var step = MiniProfiler.Current.Step(string.Format("CacheService: TryGet {0} {1} {2}", model.CacheType, model.Key, model.GetValuesKey()));
            
            try
            {
#endif
                return CacheService.TryGet(model, out value);
#if DEBUG
            }
            finally
            {
                if (step != null)
                {
                    step.Dispose();
                }
            }
#endif
        }

//        /// <summary>
//        /// Try to get all stored objects that belong to they specified key.
//        /// </summary>
//        /// <remarks>
//        /// Uses <see cref="CacheType.Default" />.
//        /// </remarks>
//        /// <typeparam name="T">Type of stored item.</typeparam>
//        /// <param name="key">Key used in key model.</param>
//        /// <param name="value">Stored values.</param>
//        /// <returns>Stored item as type.</returns>
//        public bool TryGet<T>(string key, out IEnumerable<T> value)
//        {
//            return TryGet(CacheType.Default, key, out value);
//        }

//        /// <summary>
//        /// Try to get all stored objects that belong to they specified key.
//        /// </summary>
//        /// <param name="cacheType">Cache type to use.</param>
//        /// <typeparam name="T">Type of stored item.</typeparam>
//        /// <param name="key">Key used in key model.</param>
//        /// <param name="value">Stored values.</param>
//        /// <returns>Stored item as type.</returns>
//        public bool TryGet<T>(CacheType cacheType, string key, out IEnumerable<T> value)
//        {
//            var model = new KeyModel(cacheType, key);

//            // Set namespace of model
//            model.Namespace = Namespace;

//            // Call actual Cache Service
//#if DEBUG
//            var step = MiniProfiler.Current.Step(string.Format("CacheService: TryGet {0} {1} {2}", cacheType, model.Key, model.GetValuesKey()));

//            try
//            {
//#endif
//                return CacheService.TryGet(cacheType, model.GetKey(), out value);
//#if DEBUG
//            }
//            finally
//            {
//                if (step != null)
//                {
//                    step.Dispose();
//                }
//            }
//#endif
//        }
    }
}
