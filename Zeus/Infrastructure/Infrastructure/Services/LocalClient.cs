using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens;
using System.Net;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using System.IdentityModel.Protocols.WSTrust;
//using WSTrustChannelFactory = System.IdentityModel.Protocols.WSTrust.WSTrustChannelFactory;
//using WSTrustChannel = System.IdentityModel.Protocols.WSTrust.WSTrustChannel;
using System.ServiceModel.Security;
using System.ServiceModel.Security.Tokens;
using System.ServiceModel.Channels;

#if DEBUG
using Employment.Web.Mvc.Infrastructure.Services.Profiled;
#endif

namespace Employment.Web.Mvc.Infrastructure.Services
{
    /// <summary>
    /// Provides methods for wcf client when working with local services and a local STS
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LocalClient : IClient
    {
        //private readonly List<IEndpointBehavior> endpointBehaviors = new List<IEndpointBehavior>();
        //private readonly List<IOperationBehavior> operationBehaviors = new List<IOperationBehavior>();
        private WSTrustChannelFactory trustChannelFactory;

        /// <summary>
        /// Configuration manager for interacting with the Web configuration.
        /// </summary>
        protected readonly IConfigurationManager ConfigurationManager;

        /// <summary>
        /// User service <see cref="IUserService" />
        /// </summary>
        public IUserService UserService
        {
            get
            {
                var containerProvider = DependencyResolver.Current as IContainerProvider;

                return (containerProvider != null) ? containerProvider.GetService<IUserService>() : null;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalClient" /> class.
        /// </summary>
        /// <param name="configurationManager">Configuration manager for interacting with the Web configuration.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="configurationManager" /> is <c>null</c>.</exception>
        public LocalClient(IConfigurationManager configurationManager)
        {
            if (configurationManager == null)
            {
                throw new ArgumentNullException("configurationManager");
            }

            ConfigurationManager = configurationManager;
        }

        /// <summary>
        /// Creates a new instance of the service.
        /// </summary>
        /// <typeparam name="TService">Type of service.</typeparam>
        /// <param name="serviceName">Name of service.</param>
        /// <returns>New instance of the service.</returns>
        public TService Create<TService>(string serviceName) where TService : class
        {
            return CreateInstance<TService>(serviceName);
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <typeparam name="TServiceContract">The type of the service contract.</typeparam>
        /// <param name="svcFileName">Name of the SVC file.</param>
        /// <returns></returns>
        private TServiceContract CreateInstance<TServiceContract>(string svcFileName) where TServiceContract : class
        {
            return CreateInstanceInternalClaims<TServiceContract>(svcFileName);
        }

        /// <summary>
        /// Creates the instance internal.
        /// </summary>
        /// <typeparam name="TServiceContract">The type of the service contract.</typeparam>
        /// <param name="svcFileName">Name of the SVC file.</param>
        /// <returns></returns>
        private TServiceContract CreateInstanceInternalClaims<TServiceContract>(string svcFileName) where TServiceContract : class
        {
            var svcUrl = string.Format("{0}{1}", ConfigurationManager.AppSettings["SVCUrl"], svcFileName);

            var serviceEndpointAddress = new EndpointAddress(
                new Uri(svcUrl),
                EndpointIdentity.CreateDnsIdentity(
                    ConfigurationManager.AppSettings["LocalHostEscServiceCertficateName"]),
                new AddressHeaderCollection());

            Binding binding = new WS2007FederationHttpBinding("WS2007FederationHttpBinding_IServices");

            var channelFactory = new ChannelFactory<TServiceContract>(binding, serviceEndpointAddress);
            if (channelFactory.Credentials != null) channelFactory.Credentials.SupportInteractive = false;
            //channelFactory.ConfigureChannelFactory();

            return CreateInstanceInternalClaims(channelFactory);
        }

        /// <summary>
        /// Creates the instance internal.
        /// </summary>
        /// <typeparam name="TServiceContract">The type of the service contract.</typeparam>
        /// <param name="channelFactory">The channel factory.</param>
        /// <returns></returns>
        private TServiceContract CreateInstanceInternalClaims<TServiceContract>(ChannelFactory<TServiceContract> channelFactory)
        {
            foreach (var operation in channelFactory.Endpoint.Contract.Operations)
            {
                var dataContractSerializerOperationBehavior = operation.Behaviors.Find<DataContractSerializerOperationBehavior>();
                if (dataContractSerializerOperationBehavior == null)
                {
                    dataContractSerializerOperationBehavior = new DataContractSerializerOperationBehavior(operation);

                    if (!operation.Behaviors.Contains(dataContractSerializerOperationBehavior))
                    {
                        operation.Behaviors.Add(dataContractSerializerOperationBehavior);
                    }
                }
                dataContractSerializerOperationBehavior.MaxItemsInObjectGraph = Int32.MaxValue;
            }

            // Add channelFactory.Endpoint Behaviors
            if (channelFactory.Endpoint.Behaviors.Find<CallerContextBehavior>() == null)
            {
                var endpointBehavior = new CallerContextBehavior();
                channelFactory.Endpoint.Behaviors.Add(endpointBehavior);
            }

#if DEBUG
            if (channelFactory.Endpoint.Behaviors.Find<ProfilerEndpointBehavior>() == null)
            {
                var profilerBehaviour = new ProfilerEndpointBehavior();
                channelFactory.Endpoint.Behaviors.Add(profilerBehaviour);
            }
#endif

            // Add channelFactory.Endpoint.Contract.Operations Behaviors

            // None

            /*
            
            var operationBehaviours = new List<IOperationBehavior>();
            
            foreach (var operationBehavior in operationBehaviors)
            {
                foreach (var operation in channelFactory.Endpoint.Contract.Operations)
                {
                    if (!operation.Behaviors.Contains(operationBehavior))
                    {
                        operation.Behaviors.Add(operationBehavior);
                    }
                }
            }
            */

            if (channelFactory.Credentials != null)
            {
                channelFactory.Credentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;
            }

            return channelFactory.CreateChannelWithIssuedToken(GetSecurityTokenDirectFromLocalSts());
        }

        private SecurityToken GetSecurityTokenDirectFromLocalSts()
        {
            if (trustChannelFactory == null)
            {
                var endpointidentifier = EndpointIdentity.CreateDnsIdentity(ConfigurationManager.AppSettings["LocalHostStsServiceCertficateName"]);
                var epa = new Uri(ConfigurationManager.AppSettings["UriIdpStsAddress"]);
                var address = new EndpointAddress(epa, endpointidentifier, new AddressHeaderCollection());
                var binding = new WS2007HttpBinding
                {
                    Security =
                    {
                        Mode = SecurityMode.Message,
                        Message =
                        {
                            ClientCredentialType = MessageCredentialType.UserName,
                            EstablishSecurityContext = false,
                            NegotiateServiceCredential = true
                        }
                    }
                };

                var channelFactory = new WSTrustChannelFactory(binding, address)
                {
                    TrustVersion = TrustVersion.WSTrust13
                };
                if (channelFactory.Credentials != null)
                {
                    channelFactory.Credentials.UserName.UserName = " "; //single space as local sts not using
                    channelFactory.Credentials.UserName.Password = " "; //single space as local sts not using
                    channelFactory.Credentials.Windows.ClientCredential = CredentialCache.DefaultNetworkCredentials;
                }

                trustChannelFactory = channelFactory;
            }

            var rst = new RequestSecurityToken( RequestTypes.Issue)
            {
                AppliesTo = new System.IdentityModel.Protocols.WSTrust. EndpointReference(ConfigurationManager.AppSettings["UriRpAppliesTo"])
            };

            var channel = (WSTrustChannel)trustChannelFactory.CreateChannel();
            RequestSecurityTokenResponse rstr;
            var token = channel.Issue(rst, out rstr);

            return token;
        }
    }
}