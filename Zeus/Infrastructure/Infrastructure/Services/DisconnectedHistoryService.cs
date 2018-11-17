using System;
using System.Collections.Generic;
using System.Linq;

using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.Models;

namespace Employment.Web.Mvc.Infrastructure.Services
{
    /// <summary>
    /// Defines a service for accessing history data which is stored in the session.
    /// </summary>
    public class DisconnectedHistoryService : Service, IHistoryService
    {
        /// <summary>
        /// Configuration manager for interacting with the Web configuration.
        /// </summary>
        protected readonly IConfigurationManager ConfigurationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisconnectedHistoryService"/> class.
        /// </summary>
        /// <param name="configurationManager">The configuration manager.</param>
        /// <param name="client">Client for interacting with WCF services.</param>
        /// <param name="cacheService">Cache service for interacting with cached data.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="configurationManager"/> is <c>null</c>.</exception>
        public DisconnectedHistoryService(IConfigurationManager configurationManager, IClient client,  ICacheService cacheService)  : base(client,  cacheService)
        {
            if (configurationManager == null)
            {
                throw new ArgumentNullException("configurationManager");
            }

            ConfigurationManager = configurationManager;
        }

        /// <summary>
        /// Set an activity into recent history storage.
        /// </summary>
        /// <param name="activityID">The activity ID.</param>
        /// <param name="displayName">The activity display name.</param>
        public void SetActivity(long activityID, string displayName)
        {
            Set(HistoryType.Activity, new Dictionary<string, object> { { "activityId", activityID } }, displayName);
        }

        /// <summary>
        /// Set a contract into recent history storage.
        /// </summary>
        /// <param name="contractID">The contract ID.</param>
        /// <param name="displayName">The contract display name.</param>
        public void SetContract(string contractID, string displayName)
        {
            Set(HistoryType.Contract, new Dictionary<string, object> { { "contractId", contractID } }, displayName);
        }

        /// <summary>
        /// Set a employer into recent history storage.
        /// </summary>
        /// <param name="employerID">The employer ID.</param>
        /// <param name="displayName">The employer display name.</param>
        public void SetEmployer(long employerID, string displayName)
        {
            Set(HistoryType.Employer, new Dictionary<string, object> { { "employerID", employerID } }, displayName);
        }

        /// <summary>
        /// Set a job seeker into recent history storage.
        /// </summary>
        /// <param name="jobSeekerID">The job seeker ID.</param>
        /// <param name="displayName">The job seeker display name.</param>
        public void SetJobSeeker(long jobSeekerID, string displayName)
        {
            Set(HistoryType.JobSeeker, new Dictionary<string, object> { { "jobseekerId", jobSeekerID } }, displayName);
        }
        
        /// <summary>
        /// Set a vacancy into recent history storage.
        /// </summary>
        /// <param name="vacancyID">The vacancy ID.</param>
        /// <param name="displayName">The vacancy display name.</param>
        public void SetVacancy(long vacancyID, string displayName)
        {
            Set(HistoryType.Vacancy, new Dictionary<string, object> { { "vacancyID", vacancyID } }, displayName);
        }

        /// <summary>
        ///  Set an item into recent history storage.
        /// </summary>
        /// <param name="historyType">The type of history.</param>
        /// <param name="values">The values necessary for loading the object as a dictionary of key value pairs.</param>
        /// <param name="displayName">The object's display name.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is <c>null</c> or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="displayName"/> is <c>null</c> or empty.</exception>
        public void Set(HistoryType historyType, IDictionary<string, object> values, string displayName)
        {
            if (values == null || !values.Any())
            {
                throw new ArgumentNullException("values");
            }

            if (string.IsNullOrEmpty(displayName))
            {
                throw new ArgumentNullException("displayName");
            }

            var key = new KeyModel("History").Add(historyType);

            values.ForEach(v => key.Add(v.Key.ToLower()).Add(v.Value));

            var model = new HistoryModel
            {
                HistoryType = historyType,
                Values = values,
                DisplayName = displayName,
                DateAccessed = UserService.DateTime,
                IsPinned = false,
                Username = UserService.Username
            };

            UserService.Session.Set(key, model);
        }


        /// <summary>
        /// Removes the specified recent history item.
        /// </summary>
        /// <param name="historyType">The history type.</param>
        /// <param name="values">The values necessary for loading the object as a dictionary of key value pairs.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is <c>null</c> or empty.</exception>
        public void Remove(HistoryType historyType, IDictionary<string, object> values)
        {
            if (values == null || !values.Any())
            {
                throw new ArgumentNullException("values");
            }

            var key = new KeyModel("History").Add(historyType);

            values.ForEach(v => key.Add(v.Key.ToLower()).Add(v.Value));

            UserService.Session.Remove(key);
        }

        /// <summary>
        /// Pins the specified recent history record.
        /// </summary>
        /// <param name="historyType">The history type.</param>
        /// <param name="values">The values necessary for loading the object as a dictionary of key value pairs.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="values" /> is <c>null</c> or empty.</exception>
        public void Pin(HistoryType historyType, IDictionary<string, object> values)
        {
            if (values == null || !values.Any())
            {
                throw new ArgumentNullException("values");
            }

            var key = new KeyModel("History").Add(historyType);

            values.ForEach(v => key.Add(v.Key.ToLower()).Add(v.Value));

            HistoryModel model;

            if (UserService.Session.TryGet(key, out model))
            {
                model.IsPinned = true;

                UserService.Session.Set(key, model);
            }
        }

        /// <summary>
        /// Unpins the specified recent history record.
        /// </summary>
        /// <param name="historyType">The history type.</param>
        /// <param name="values">The values necessary for loading the object as a dictionary of key value pairs.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="values" /> is <c>null</c> or empty.</exception>
        public void Unpin(HistoryType historyType, IDictionary<string, object> values)
        {
            if (values == null || !values.Any())
            {
                throw new ArgumentNullException("values");
            }

            var key = new KeyModel("History").Add(historyType);

            values.ForEach(v => key.Add(v.Key.ToLower()).Add(v.Value));

            HistoryModel model;

            if (UserService.Session.TryGet(key, out model))
            {
                model.IsPinned = false;

                UserService.Session.Set(key, model);
            }
        }

        /// <summary>
        /// Gets the recent history item for a specified history type and object ID
        /// </summary>
        /// <param name="historyType">The history type.</param>
        /// <param name="values">The values necessary for loading the object as a dictionary of key value pairs.</param>
        /// <returns>Returns <see cref="IEnumerable{HistoryModel}"/> for a single matching recent history record. If no matching record found returns null.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is <c>null</c> or empty.</exception>
        public HistoryModel Get(HistoryType historyType, IDictionary<string, object> values)
        {
            if (values == null || !values.Any())
            {
                throw new ArgumentNullException("values");
            }

            var key = new KeyModel("History").Add(historyType);

            values.ForEach(v => key.Add(v.Key.ToLower()).Add(v.Value));

            HistoryModel model;

            return (UserService.Session.TryGet(key, out model)) ? model : null;
        }

        /// <summary>
        /// Gets the list of recent history records for the current user and a specified record type.
        /// </summary>
        /// <param name="historyType">The history type.</param>
        /// <returns>Returns <see cref="IEnumerable{HistoryModel}"/> for matching recent history records.</returns>
        public IEnumerable<HistoryModel> Get(HistoryType historyType)
        {
            IEnumerable<HistoryModel> model;

            if (UserService.Session.TryGet("History", out model))
            {
                return model.Where(m => m.HistoryType == historyType).OrderByDescending(m => m.DateAccessed).GroupBy(m => m.IsPinned).OrderByDescending(g => g.Key).SelectMany(m => m);
            }
            
            return Enumerable.Empty<HistoryModel>();
        }
    }
}
