using System.Collections.Generic;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Models;
using StackExchange.Profiling;

namespace Employment.Web.Mvc.Infrastructure.Services.Profiled
{
    /// <summary>
    /// Defines a service for interacting with data stored in the session.
    /// </summary>
    /// <remarks>
    /// Profiled version of <see cref="SessionService" />.
    /// </remarks>
    public class SessionService : Services.SessionService
    {
        /// <summary>
        /// Set an item in the session under a unique key.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="model">Key model.</param>
        /// <param name="o">Item to be stored in session.</param>
        /// <exception cref="TypeMismatchException">Thrown if the session already has an object set with the key and its actual type does not match the expected type.</exception>
        public override void Set<T>(KeyModel model, T o)
        {
            using (MiniProfiler.Current.Step(string.Format("SessionService: Set {0}", model.Key)))
            {
                base.Set(model, o);
            }
        }

        /// <summary>
        /// Remove an item from the session.
        /// </summary>
        /// <param name="model">Key model.</param>
        public override void Remove(KeyModel model)
        {
            using (MiniProfiler.Current.Step(string.Format("SessionService: Remove {0}", model.Key)))
            {
                base.Remove(model);
            }
        }

        /// <summary>
        /// Remove all variations of an item from the session based on a key name.
        /// </summary>
        /// <param name="key">Key used in key model.</param>
        public override void Remove(string key)
        {
            using (MiniProfiler.Current.Step(string.Format("SessionService: Remove {0}", key)))
            {
                base.Remove(key);
            }
        }

        /// <summary>
        /// Checks if the item is stored in the session.
        /// </summary>
        /// <param name="model">Key model.</param>
        /// <returns><c>true</c> if the item is stored in the session; otherwise, <c>false</c>.</returns>
        public override bool Contains(KeyModel model)
        {
            using (MiniProfiler.Current.Step(string.Format("SessionService: Contains {0}", model.Key)))
            {
                return base.Contains(model);
            }
        }

        /// <summary>
        /// Try to get a stored object.
        /// </summary>
        /// <typeparam name="T">Type of stored item.</typeparam>
        /// <param name="model">Key model.</param>
        /// <param name="value">Stored value.</param>
        /// <returns>Stored item as type.</returns>
        public override bool TryGet<T>(KeyModel model, out T value)
        {
            using (MiniProfiler.Current.Step(string.Format("SessionService: TryGet {0}", model.Key)))
            {
                return base.TryGet(model, out value);
            }
        }

        /// <summary>
        /// Try to get all stored objects that belong to the specified key.
        /// </summary>
        /// <typeparam name="T">Type of stored item.</typeparam>
        /// <param name="key">Key used in key model.</param>
        /// <param name="value">Stored value.</param>
        /// <returns>Stored item as type.</returns>
        public override bool TryGet<T>(string key, out IEnumerable<T> value)
        {
            using (MiniProfiler.Current.Step(string.Format("SessionService: TryGet {0}", key)))
            {
                return base.TryGet(key, out value);
            }
        }

        /// <summary>
        /// Cancels the current session.
        /// </summary>
        /// <remarks>
        /// To be used on logout by Infrastructure only.
        /// </remarks>
        public override void Abandon()
        {
            using (MiniProfiler.Current.Step("SessionService: Abandon()"))
            {
                base.Abandon();
            }
        }
    }
}
