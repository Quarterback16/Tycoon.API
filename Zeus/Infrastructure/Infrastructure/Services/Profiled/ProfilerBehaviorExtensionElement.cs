using System;
using System.ServiceModel.Configuration;

namespace Employment.Web.Mvc.Infrastructure.Services.Profiled
{
    /// <summary>
    /// Defines a behaviour extension element for <see cref="ProfilerEndpointBehavior"/>.
    /// </summary>
    public class ProfilerBehaviorExtensionElement : BehaviorExtensionElement
    {
        /// <summary>
        /// The behaviour type.
        /// </summary>
        public override Type BehaviorType
        {
            get { return typeof(ProfilerEndpointBehavior); }
        }

        /// <summary>
        /// Create behaviour.
        /// </summary>
        /// <returns>A new <see cref="ProfilerEndpointBehavior" /> instance.</returns>
        protected override object CreateBehavior()
        {
            return new ProfilerEndpointBehavior();
        }
    }
}
