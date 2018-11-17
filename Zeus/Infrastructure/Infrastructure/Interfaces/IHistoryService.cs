using System.Collections.Generic;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.Models;

namespace Employment.Web.Mvc.Infrastructure.Interfaces
{
    /// <summary>
    /// Defines methods and properties that are required for a history service.
    /// </summary>
    public interface IHistoryService
    {
        /// <summary>
        /// Set an activity into recent history storage.
        /// </summary>
        /// <param name="activityID">The activity ID.</param>
        /// <param name="displayName">The activity display name.</param>
        void SetActivity(long activityID, string displayName);

        /// <summary>
        /// Set a contract into recent history storage.
        /// </summary>
        /// <param name="contractID">The contract ID.</param>
        /// <param name="displayName">The contract display name.</param>
        void SetContract(string contractID, string displayName);

        /// <summary>
        /// Set a employer into recent history storage.
        /// </summary>
        /// <param name="employerID">The employer ID.</param>
        /// <param name="displayName">The employer display name.</param>
        void SetEmployer(long employerID, string displayName);

        /// <summary>
        /// Set a job seeker into recent history storage.
        /// </summary>
        /// <param name="jobSeekerID">The job seeker ID.</param>
        /// <param name="displayName">The job seeker display name.</param>
        void SetJobSeeker(long jobSeekerID, string displayName);

        /// <summary>
        /// Set a vacancy into recent history storage.
        /// </summary>
        /// <param name="vacancyID">The vacancy ID.</param>
        /// <param name="displayName">The vacancy display name.</param>
        void SetVacancy(long vacancyID, string displayName);

        /// <summary>
        /// Set an item into recent history storage.
        /// </summary>
        /// <param name="historyType">The history type.</param>
        /// <param name="values">The values necessary for loading the object as a dictionary of key value pairs.</param>
        /// <param name="displayName">The object's display name.</param>
        void Set(HistoryType historyType, IDictionary<string, object> values, string displayName);

        /// <summary>
        /// Removes the specified recent history record.
        /// </summary>
        /// <param name="historyType">The history type.</param>
        /// <param name="values">The values necessary for loading the object as a dictionary of key value pairs.</param>
        void Remove(HistoryType historyType, IDictionary<string, object> values);

        /// <summary>
        /// Pins the specified recent history record.
        /// </summary>
        /// <param name="historyType">The history type.</param>
        /// <param name="values">The values necessary for loading the object as a dictionary of key value pairs.</param>
        void Pin(HistoryType historyType, IDictionary<string, object> values);

        /// <summary>
        /// Unpins the specified recent history record.
        /// </summary>
        /// <param name="historyType">The history type.</param>
        /// <param name="values">The values necessary for loading the object as a dictionary of key value pairs.</param>
        void Unpin(HistoryType historyType, IDictionary<string, object> values);

        /// <summary>
        /// Gets the recent history item for a specified history type and object values
        /// </summary>
        /// <param name="historyType">The history type.</param>
        /// <param name="values">The values necessary for loading the object as a dictionary of key value pairs.</param>
        /// <returns>Returns <see cref="IEnumerable{RecentHistoryModel}"/> for a single matching recent history record.</returns>
        HistoryModel Get(HistoryType historyType, IDictionary<string, object> values);

        /// <summary>
        /// Gets the list of recent history records for the current user and a specified record type.
        /// </summary>
        /// <param name="historyType">The history type.</param>
        /// <returns>Returns matching recent history records.</returns>
        IEnumerable<HistoryModel> Get(HistoryType historyType);
    }
}
