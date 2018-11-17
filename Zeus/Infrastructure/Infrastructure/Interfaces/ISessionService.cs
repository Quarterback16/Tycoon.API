using System.Collections.Generic;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Models;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.Interfaces
{
    /// <summary>
    /// Defines the methods and properties that are required for a Session Service.
    /// </summary>
    public interface ISessionService
    {
        ///// <summary>
        ///// Current activity ID the user is working with.
        ///// </summary>
        //long ActivityID { get; set; }

        ///// <summary>
        ///// Current appointment ID the user is working with.
        ///// </summary>
        //long AppointmentID { get; set; }

        ///// <summary>
        ///// Current contract ID the user is working with.
        ///// </summary>
        //string ContractID { get; set; }

        ///// <summary>
        ///// Current Centrelink Reference number the user is working with.
        ///// </summary>
        //string CRN { get; set; }

        ///// <summary>
        ///// Current employer ID the user is working with.
        ///// </summary>
        //long EmployerID { get; set; }

        ///// <summary>
        ///// Current job seeker ID the user is working with.
        ///// </summary>
        //long JobSeekerID { get; set; }

        ///// <summary>
        ///// Current override ID the user is working with.
        ///// </summary>
        //long OverrideID { get; set; }

        ///// <summary>
        ///// Current payment ID the user is working with.
        ///// </summary>
        //long PaymentID { get; set; }

        ///// <summary>
        ///// Current provider ID the user is working with.
        ///// </summary>
        //long ProviderID { get; set; }

        ///// <summary>
        ///// Current vacancy ID the user is working with.
        ///// </summary>
        //long VacancyID { get; set; }

        ///// <summary>
        ///// Get the value of a context ID.
        ///// </summary>
        ///// <typeparam name="T">The object type of the context ID.</typeparam>
        ///// <param name="contextType">The context type.</param>
        ///// <returns>The context ID value.</returns>
        //T GetContextID<T>(ContextType contextType);

        ///// <summary>
        ///// Set the value of a context ID.
        ///// </summary>
        ///// <typeparam name="T">The object type of the context ID.</typeparam>
        ///// <param name="contextType">The context type.</param>
        ///// <param name="value">The value to set the context ID to.</param>
        //void SetContextID<T>(ContextType contextType, T value);

        /// <summary>
        /// Set an item in the session under a unique key.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="model">Key model.</param>
        /// <param name="o">Item to be stored in session.</param>
        /// <exception cref="TypeMismatchException">Thrown if the session already has an object set with the key and its actual type does not match the expected type.</exception>
        void Set<T>(KeyModel model, T o);

        /// <summary>
        /// Remove an item from the session.
        /// </summary>
        /// <param name="model">Key model.</param>
        void Remove(KeyModel model);

        /// <summary>
        /// Remove all variations of an item from the cache based on a key name.
        /// </summary>
        /// <remarks>
        /// Uses <see cref="CacheType.Default" />.
        /// </remarks>
        /// <param name="key">Key used in key model.</param>
        void Remove(string key);

        /// <summary>
        /// Remove all variations of an item from the cache based on a key name.
        /// </summary>
        /// <param name="cacheType">Cache type to use.</param>
        /// <param name="key">Key used in key model.</param>
        void Remove(CacheType cacheType, string key);

        /// <summary>
        /// Checks if the item is stored in the session.
        /// </summary>
        /// <param name="model">Key model.</param>
        /// <returns><c>true</c> if the item is stored in the session; otherwise, <c>false</c>.</returns>
        bool Contains(KeyModel model);

        /// <summary>
        /// Try to get a stored object.
        /// </summary>
        /// <typeparam name="T">Type of stored item.</typeparam>
        /// <param name="model">Key model.</param>
        /// <param name="value">Stored value.</param>
        /// <returns>Stored item as type.</returns>
        bool TryGet<T>(KeyModel model, out T value);

        /// <summary>
        /// Try to get all stored objects that belong to they specified key.
        /// </summary>
        /// <remarks>
        /// Uses <see cref="CacheType.Default" />.
        /// </remarks>
        /// <typeparam name="T">Type of stored item.</typeparam>
        /// <param name="key">Key used in key model.</param>
        /// <param name="value">Stored values.</param>
        /// <returns>Stored items as type.</returns>
        bool TryGet<T>(string key, out IEnumerable<T> value);

        /// <summary>
        /// Try to get all stored objects that belong to they specified key.
        /// </summary>
        /// <param name="cacheType">Cache type to use.</param>
        /// <typeparam name="T">Type of stored item.</typeparam>
        /// <param name="key">Key used in key model.</param>
        /// <param name="value">Stored values.</param>
        /// <returns>Stored items as type.</returns>
        bool TryGet<T>(CacheType cacheType, string key, out IEnumerable<T> value);

        /// <summary>
        /// Cancels the current session.
        /// </summary>
        /// <remarks>
        /// To be used on logout by Infrastructure only.
        /// </remarks>
        void Abandon();
    }
}
