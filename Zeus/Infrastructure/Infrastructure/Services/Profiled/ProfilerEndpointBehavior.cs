using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Employment.Web.Mvc.Infrastructure.Services.Profiled
{
    /// <summary>
    /// Defines an endpoint behaviour for profiling.
    /// </summary>
    public class ProfilerEndpointBehavior : IEndpointBehavior
    {
        /// <summary>
        /// Apply client behavior of including a <see cref="ProfilerMessageInspector" />.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="clientRuntime">The client runtime.</param>
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(new ProfilerMessageInspector());
        }

        #region No implementation necessary

        /// <summary>
        /// Add binding parameters.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="bindingParameters">The binding parameters.</param>
        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            // No implementation necessary
        }

        /// <summary>
        /// Apply dispatch behaviour.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="endpointDispatcher">The endpoint dispatcher.</param>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            // No implementation necessary
        }

        /// <summary>
        /// Validate.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        public void Validate(ServiceEndpoint endpoint)
        {
            // No implementation necessary
        }

        #endregion
    }
}
