using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Web.Mvc;
using Employment.Esc.Shared.Contracts;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Infrastructure.Services
{
    /// <summary>
    /// Client Caller behavior to add context headers - user date time, site and contracts
    /// </summary>
    public class CallerContextBehavior : IEndpointBehavior, IClientMessageInspector
    {
        /// <summary>
        /// User service <see cref="IUserService" />
        /// </summary>
        private IUserService UserService
        {
            get
            {
                var containerProvider = DependencyResolver.Current as IContainerProvider;

                return (containerProvider != null) ? containerProvider.GetService<IUserService>() : null;
            }
        }

        /// <summary>
        /// Validate
        /// </summary>
        /// <param name="endpoint"></param>
        public void Validate(ServiceEndpoint endpoint)
        {
            // Nothing to do
        }

        /// <summary>
        /// AddBindingParameters
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="bindingParameters"></param>
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            // Nothing to do
        }

        /// <summary>
        /// ApplyDispatchBehavior
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="endpointDispatcher"></param>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            // Nothing to do
        }

        /// <summary>
        /// ApplyClientBehavior - add the client side message inspector
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="clientRuntime"></param>
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(this);
        }

        /// <summary>
        /// BeforeSendRequest
        /// </summary>
        /// <param name="request"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            // can we find an existing caller contetx header
            var existingCallerContextHeaderIndex = request.Headers.FindHeader(Constants.CallerContextHeaderName, Constants.XmlNamespace);
            var userService = UserService;
            // if so, remove before adding another
            if (existingCallerContextHeaderIndex >= 0)
                request.Headers.RemoveAt(existingCallerContextHeaderIndex);

            var ctx = new CallerContext
            {
                EffectiveContracts = userService.Contracts.ToArray(),
                EffectiveDate = userService.DateTime,
                EffectiveSite = userService.SiteCode
            };

            request.Headers.Add(MessageHeader.CreateHeader(Constants.CallerContextHeaderName, Constants.XmlNamespace, ctx));
            return null;
        }

        /// <summary>
        /// AfterReceiveReply
        /// </summary>
        /// <param name="reply"></param>
        /// <param name="correlationState"></param>
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            // Nothing to do
        }
    }
}