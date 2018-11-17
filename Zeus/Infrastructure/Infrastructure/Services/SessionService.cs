using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Models;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.Services
{
    /// <summary>
    /// Defines a service for interacting with data stored in the session.
    /// </summary>
    public class SessionService : ISessionService
    {
        ///// <summary>
        ///// Current activity ID the user is working with.
        ///// </summary>
        //public long ActivityID
        //{
        //    get { return GetContextID<long>(ContextType.Activity); }
        //    set { SetContextID(ContextType.Activity, value); }
        //}

        ///// <summary>
        ///// Current appointment ID the user is working with.
        ///// </summary>
        //public long AppointmentID
        //{
        //    get { return GetContextID<long>(ContextType.Appointment); }
        //    set { SetContextID(ContextType.Appointment, value); }
        //}

        ///// <summary>
        ///// Current contract ID the user is working with.
        ///// </summary>
        //public string ContractID
        //{
        //    get { return GetContextID<string>(ContextType.Contract); }
        //    set { SetContextID(ContextType.Contract, value); }
        //}

        ///// <summary>
        ///// Current Centrelink reference number the user is working with.
        ///// </summary>
        //public string CRN
        //{
        //    get { return GetContextID<string>(ContextType.CRN); }
        //    set { SetContextID(ContextType.CRN, value); }
        //}

        ///// <summary>
        ///// Current employer ID the user is working with.
        ///// </summary>
        //public long EmployerID
        //{
        //    get { return GetContextID<long>(ContextType.Employer); }
        //    set { SetContextID(ContextType.Employer, value); }
        //}

        ///// <summary>
        ///// Current job seeker ID the user is working with.
        ///// </summary>
        //public long JobSeekerID
        //{
        //    get { return GetContextID<long>(ContextType.JobSeeker); }
        //    set { SetContextID(ContextType.JobSeeker, value); }
        //}

        ///// <summary>
        ///// Current override ID the user is working with.
        ///// </summary>
        //public long OverrideID
        //{
        //    get { return GetContextID<long>(ContextType.Override); }
        //    set { SetContextID(ContextType.Override, value); }
        //}

        ///// <summary>
        ///// Current payment ID the user is working with.
        ///// </summary>
        //public long PaymentID
        //{
        //    get { return GetContextID<long>(ContextType.Payment); }
        //    set { SetContextID(ContextType.Payment, value); }
        //}

        ///// <summary>
        ///// Current provider ID the user is working with.
        ///// </summary>
        //public long ProviderID
        //{
        //    get { return GetContextID<long>(ContextType.Provider); }
        //    set { SetContextID(ContextType.Provider, value); }
        //}

        ///// <summary>
        ///// Current vacancy ID the user is working with.
        ///// </summary>
        //public long VacancyID
        //{
        //    get { return GetContextID<long>(ContextType.Vacancy); }
        //    set { SetContextID(ContextType.Vacancy, value); }
        //}

        ///// <summary>
        ///// Get the value of a context ID.
        ///// </summary>
        ///// <remarks>
        ///// For internal use by Infrastructure only. Use the ID properties instead.
        ///// </remarks>
        ///// <typeparam name="T">The object type of the context ID.</typeparam>
        ///// <param name="contextType">The context type.</param>
        ///// <returns>The context ID value.</returns>
        //public T GetContextID<T>(ContextType contextType)
        //{
        //    var model = new KeyModel("ContextIDs");

        //    DefaultNamespace(ref model);

        //    Dictionary<ContextType, object> contextTypes;

        //    if (TryGet(model, out contextTypes) && contextTypes.ContainsKey(contextType))
        //    {
        //        return (T)contextTypes[contextType];
        //    }

        //    return default(T);
        //}

        ///// <summary>
        ///// Set the value of a context ID.
        ///// </summary>
        ///// <remarks>
        ///// For internal use by Infrastructure only. Use the ID properties instead.
        ///// </remarks>
        ///// <typeparam name="T">The object type of the context ID.</typeparam>
        ///// <param name="contextType">The context type.</param>
        ///// <param name="value">The value to set the context ID to.</param>
        //public void SetContextID<T>(ContextType contextType, T value)
        //{
        //    var model = new KeyModel("ContextIDs");

        //    DefaultNamespace(ref model);

        //    Dictionary<ContextType, object> contextTypes;

        //    if (!TryGet(model, out contextTypes))
        //    {
        //        contextTypes = new Dictionary<ContextType, object>();
        //    }

        //    if (contextTypes.ContainsKey(contextType))
        //    {
        //        contextTypes[contextType] = value;
        //    }
        //    else
        //    {
        //        contextTypes.Add(contextType, value);
        //    }

        //    Set(model, contextTypes);
        //}

        /// <summary>
        /// Set an item in the session under a unique key.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="model">Key model.</param>
        /// <param name="o">Item to be stored in session.</param>
        /// <exception cref="TypeMismatchException">Thrown if the session already has an object set with the key and its actual type does not match the expected type.</exception>
        public virtual void Set<T>(KeyModel model, T o)
        {
            DefaultNamespace(ref model);
            T data;

            if (TryGet(model, out data))
            {
                if (typeof(T) != data.GetType())
                {
                    throw new TypeMismatchException(typeof(T), data.GetType());
                }
            }

            var key = model.GetKey();
            
            HttpContext.Current.Session[key] = o;
        }

        /// <summary>
        /// Remove an item from the session.
        /// </summary>
        /// <param name="model">Key model.</param>
        public virtual void Remove(KeyModel model)
        {
            DefaultNamespace(ref model);

            var key = model.GetKey();

            HttpContext.Current.Session.Remove(key);
        }

        /// <summary>
        /// Remove all variations of an item from the session based on a key name.
        /// </summary>
        /// <remarks>
        /// Uses <see cref="CacheType.Default" />.
        /// </remarks>
        /// <param name="key">Key used in key model.</param>
        public virtual void Remove(string key)
        {
            Remove(CacheType.Default, key);
        }
        
        /// <summary>
        /// Remove all variations of an item from the session based on a key name.
        /// </summary>
        /// <param name="cacheType">Cache type to use.</param>
        /// <param name="key">Key used in key model.</param>
        public virtual void Remove(CacheType cacheType, string key)
        {
            DefaultNamespace(cacheType, ref key);

            // Get all variations of key
            var keys = HttpContext.Current.Session.Keys.OfType<object>().Where(m => m.ToString().StartsWith(key,StringComparison.Ordinal)).Select(m => m.ToString()).ToList();

            // Remove key
            foreach (var k in keys)
            {
                HttpContext.Current.Session.Remove(k);
            }
        }

        /// <summary>
        /// Checks if the item is stored in the session.
        /// </summary>
        /// <param name="model">Key model.</param>
        /// <returns><c>true</c> if the item is stored in the session; otherwise, <c>false</c>.</returns>
        public virtual bool Contains(KeyModel model)
        {
            DefaultNamespace(ref model);

            return HttpContext.Current.Session[model.GetKey()] != null;
        }

        /// <summary>
        /// Try to get a stored object.
        /// </summary>
        /// <typeparam name="T">Type of stored item.</typeparam>
        /// <param name="model">Key model.</param>
        /// <param name="value">Stored value.</param>
        /// <returns>Stored item as type.</returns>
        public virtual bool TryGet<T>(KeyModel model, out T value)
        {
            DefaultNamespace(ref model);

            try
            {
                if (!Contains(model))
                {
                    value = default(T);

                    return false;
                }

                value = (T)HttpContext.Current.Session[model.GetKey()];
            }
            catch
            {
                value = default(T);

                return false;
            }

            return true;
        }

        /// <summary>
        /// Try to get all stored objects that belong to the specified key.
        /// </summary>
        /// <remarks>
        /// Uses <see cref="CacheType.Default" />.
        /// </remarks>
        /// <typeparam name="T">Type of stored item.</typeparam>
        /// <param name="key">Key used in key model.</param>
        /// <param name="value">Stored value.</param>
        /// <returns>Stored items as type.</returns>
        public virtual bool TryGet<T>(string key, out IEnumerable<T> value)
        {
            return TryGet(CacheType.Default, key, out value);
        }

        /// <summary>
        /// Try to get all stored objects that belong to the specified key.
        /// </summary>
        /// <param name="cacheType">Cache type to use.</param>
        /// <typeparam name="T">Type of stored item.</typeparam>
        /// <param name="key">Key used in key model.</param>
        /// <param name="value">Stored value.</param>
        /// <returns>Stored items as type.</returns>
        public virtual bool TryGet<T>(CacheType cacheType, string key, out IEnumerable<T> value)
        {
            DefaultNamespace(cacheType, ref key);

            value = Enumerable.Empty<T>();
            var values = new List<T>();

            // Get all variations of key
            var keys = HttpContext.Current.Session.Keys.OfType<object>().Where(m => m.ToString().StartsWith(key,StringComparison.Ordinal)).Select(m => m.ToString()).ToList();

            // Get each value of key
            foreach (var k in keys)
            {
                if (HttpContext.Current.Session[k] != null)
                {
                    try
                    {
                        values.Add((T)HttpContext.Current.Session[k]);
                    }
                    catch
                    {
                        return false;
                    }
                }
            }

            value = values.AsEnumerable();

            return true;
        }

        private void DefaultNamespace(ref KeyModel model)
        {
            if (model != null && string.IsNullOrEmpty(model.Namespace))
            {
                model.Namespace = GetType().Namespace;
            }
        }

        private void DefaultNamespace(CacheType cacheType, ref string key)
        {
            var @namespace = GetType().Namespace;

            if (!string.IsNullOrEmpty(key) && !key.Contains(@namespace))
            {
                var model = new KeyModel(cacheType, key);

                model.Namespace = GetType().Namespace;

                key = model.GetKey();
            }
        }

        /// <summary>
        /// Cancels the current session.
        /// </summary>
        /// <remarks>
        /// To be used on logout by Infrastructure only.
        /// </remarks>
        public virtual void Abandon()
        {
            HttpContext.Current.Session.Abandon();
        }
    }
}
