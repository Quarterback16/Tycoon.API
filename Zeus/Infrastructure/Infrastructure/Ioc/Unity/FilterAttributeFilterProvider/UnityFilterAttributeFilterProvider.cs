using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Infrastructure.Ioc.Unity.FilterAttributeFilterProvider
{
    /// <summary>
    /// Defines a filter attribute filter provider using Unity.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class UnityFilterAttributeFilterProvider : System.Web.Mvc.FilterAttributeFilterProvider
    {
        private readonly IContainerProvider container;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityFilterAttributeFilterProvider" /> class.
        /// </summary>
        /// <param name="container">Unity container.</param>
        public UnityFilterAttributeFilterProvider(IContainerProvider container)
        {
            this.container = container;
        }

        /// <summary>
        /// Get controller attributes
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="actionDescriptor"></param>
        /// <returns></returns>
        protected override IEnumerable<FilterAttribute> GetControllerAttributes(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            IEnumerable<FilterAttribute> attributes = base.GetControllerAttributes(controllerContext, actionDescriptor).ToList();

            foreach (var attribute in attributes)
            {
                container.Inject(attribute);
            }

            return attributes;
        }

        /// <summary>
        /// Get action attributes
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="actionDescriptor"></param>
        /// <returns></returns>
        protected override IEnumerable<FilterAttribute> GetActionAttributes(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            IEnumerable<FilterAttribute> attributes = base.GetActionAttributes(controllerContext, actionDescriptor).ToList();

            foreach (var attribute in attributes)
            {
                container.Inject(attribute);
            }

            return attributes;
        }
    }
}
 
