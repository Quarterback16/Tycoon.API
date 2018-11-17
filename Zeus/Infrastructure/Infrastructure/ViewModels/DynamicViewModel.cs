using System;
using System.Collections.Generic;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.ViewModels.Dynamic;

namespace Employment.Web.Mvc.Infrastructure.ViewModels
{
    /// <summary>
    /// Defines a dynamic View Model.
    /// </summary>
    [Serializable]
    public class DynamicViewModel : IFluent
    {
        private List<object> values = new List<object>();

        /// <summary>
        /// List of dynamically added view models.
        /// </summary>
        public List<object> Values { get { return values; } }

        /// <summary>
        /// Add a dynamic view model.
        /// </summary>
        /// <typeparam name="T">Type of dynamic view model.</typeparam>
        /// <param name="model">Dynamic view model.</param>
        public void Add<T>(ObjectViewModel<T> model)
        {
            values.Add(model);
        }

        /// <summary>
        /// Get a dynamic view model value by its name.
        /// </summary>
        /// <typeparam name="T">Type of dynamic view model.</typeparam>
        /// <param name="propertyName">The property name.</param>
        /// <returns>The value of the dynamic view model.</returns>
        public T Get<T>(string propertyName)
        {
            foreach (var value in values)
            {
                var model = value as IObjectViewModel<T>;

                if (model != null && string.Equals( model.Name, propertyName, StringComparison.Ordinal))
                {
                    return model.Value;
                }
            }

            return default(T);
        }

        /// <summary>
        /// Begin a group of properties.
        /// </summary>
        /// <param name="groupName">Group name.</param>
        public void BeginGroup(string groupName)
        {
            values.Add(groupName);
        }

        /// <summary>
        /// End a group of properties.
        /// </summary>
        public void EndGroup(string groupName)
        {
            values.Add(groupName);
        }
    }
}
