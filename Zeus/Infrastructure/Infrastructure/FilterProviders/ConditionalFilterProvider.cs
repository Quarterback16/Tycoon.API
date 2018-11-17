using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Employment.Web.Mvc.Infrastructure.FilterProviders
{
    /// <summary>
    /// Represents a Filter Provider that is used to apply filters only when specified conditions are met.
    /// </summary>
    public class ConditionalFilterProvider : IFilterProvider
    {
        private readonly IEnumerable<Func<ControllerContext, ActionDescriptor, object>> _conditions;

        /// <summary>
        /// Initializes a new instance of the <see cref="Employment.Web.Mvc.Infrastructure.FilterProviders.ConditionalFilterProvider" /> class.
        /// </summary>
        /// <param name="conditions">Conditions to be met for filters to apply</param>
        /// <example> 
        /// This example shows how to instantiate the <see cref="Employment.Web.Mvc.Infrastructure.FilterProviders.ConditionalFilterProvider"/> class.
        /// <code>
        /// var conditions = new Func&lt;ControllerContext, ActionDescriptor, object&gt;[]
        /// {
        ///     // Apply [ValidateAntiForgeryTokenAttribute] to all actions with [HttpPost] that don't already have [ValidateAntiForgeryTokenAttribute]
        ///     (c, a) => a.GetCustomAttributes(true).OfType&lt;HttpPostAttribute&gt;().FirstOrDefault() != null &amp;&amp; a.GetCustomAttributes(true).OfType&lt;ValidateAntiForgeryTokenAttribute&gt;().FirstOrDefault() == null ? new ValidateAntiForgeryTokenAttribute() : null
        /// };
        ///
        /// var conditionalProvider = new ConditionalFilterProvider(conditions);
        /// </code>
        /// </example>
        public ConditionalFilterProvider(IEnumerable<Func<ControllerContext, ActionDescriptor, object>> conditions)
        {
            _conditions = conditions;
        }

        /// <summary>
        /// Returns the collection of filters that have had their conditions met.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="actionDescriptor">The action descriptor.</param>
        /// <returns>The collection of filters.</returns>
        public IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            return from condition in _conditions
                   select condition(controllerContext, actionDescriptor) into filter
                   where filter != null
                   select new Filter(filter, FilterScope.Global, null);
        }
    }
}
