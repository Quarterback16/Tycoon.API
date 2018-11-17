using System;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens;
using System.Runtime.Caching;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security.Tokens;
using System.Threading;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using System.IdentityModel.Claims;
using System.IdentityModel.Protocols.WSTrust;
#if DEBUG
using Employment.Web.Mvc.Infrastructure.Services.Profiled;
#endif

namespace Employment.Web.Mvc.Infrastructure.Services
{
    /// <summary>
    /// Provides methods for client services.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ServerClient : IClient
    {

        private readonly X509Certificate2 serviceCertificate;

        private readonly MemoryCache serviceChannelFactoryCache = MemoryCache.Default;

        /// <summary>
        /// Configuration manager for interacting with the Web configuration.
        /// </summary>
        protected readonly IConfigurationManager ConfigurationManager;

        /// <summary>
        /// User service <see cref="IUserService" />
        /// </summary>
        protected IUserService UserService
        {
            get
            {
                var containerProvider = DependencyResolver.Current as IContainerProvider;

                return (containerProvider != null) ? containerProvider.GetService<IUserService>() : null;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerClient" /> class.
        /// </summary>
        /// <param name="configurationManager">Configuration manager for interacting with the Web configuration.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="configurationManager" /> is <c>null</c>.</exception>
        public ServerClient(IConfigurationManager configurationManager)
        {
            if (configurationManager == null)
            {
                throw new ArgumentNullException("configurationManager");
            }

            ConfigurationManager = configurationManager;
            serviceCertificate = ServiceCertificatePublic;
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
        /// <param name="svcFileName"> </param>
        /// <returns></returns>
        private TServiceContract CreateInstance<TServiceContract>(string svcFileName) where TServiceContract : class
        {
            // get the security token to be used in creating the channel. 
            var securityToken = GetSecurityToken();
            var svcUrl = string.Format("{0}{1}", ConfigurationManager.AppSettings["SVCUrl"], svcFileName);
            // return a client proxy channel to the caller.
            return GetServiceChannel<TServiceContract>(securityToken, svcUrl);
        }

        private SecurityToken GetSecurityToken()
        {
            var claimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;

            if (claimsPrincipal != null)
            {
                var claimsIdentity = (ClaimsIdentity) claimsPrincipal.Identity;
                if (claimsIdentity.BootstrapContext != null && claimsIdentity.BootstrapContext as BootstrapContext !=null) // did we save a boot strap token (the configuration drives this)
                {
                    return (claimsIdentity.BootstrapContext as BootstrapContext).SecurityToken; // this will be our security token via the CreateChannelActingAs(token) call ...
                }
            }
            // if we have a claimsprincipal then we MUST also have savebootstraptokens set to true in the configuration
            throw new Exception("saveBootstrapTokens must be set to 'true' on the microsoft.identityModel/service element");
        }

        private TServiceContract GetServiceChannel<TServiceContract>(SecurityToken securityToken, string relyingPartyUrl) where TServiceContract : class
        {
            var channelFactory = GetChannelFactory<TServiceContract>(relyingPartyUrl);

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

            return channelFactory.CreateChannelWithActAsToken(securityToken);
        }

        private ChannelFactory<TServiceContract> GetChannelFactory<TServiceContract>(string relyingPartyUrl) where TServiceContract : class
        {
            var sf = serviceChannelFactoryCache.Get(typeof(TServiceContract).ToString());  
            if (sf == null)
            {
                string WifEncryptionCertname = ConfigurationManager.AppSettings["WifEncryptionCertname"];
                var rpUri = new Uri(relyingPartyUrl);
                var serviceEndpointAddress = new EndpointAddress(rpUri, EndpointIdentity.CreateDnsIdentity(WifEncryptionCertname), new AddressHeaderCollection());
                var secureTransportRequired = rpUri.Scheme.ToLower().Equals("https");

                Binding binding = secureTransportRequired ? CreateIssuedTokenOverTransportBinding() : CreateWs2007FederationHttpBinding(true);

                var serviceChannelFactory = new ChannelFactory<TServiceContract>(binding, serviceEndpointAddress);
                if (serviceChannelFactory.Credentials != null)
                {
                    serviceChannelFactory.Credentials.SupportInteractive = false;
                    if (!secureTransportRequired)
                    {
                        serviceChannelFactory.Credentials.ServiceCertificate.DefaultCertificate = serviceCertificate;
                    }
                }

                //serviceChannelFactory.ConfigureChannelFactory();
                CacheClientCredentials.ConfigureChannelFactory<TServiceContract>(serviceChannelFactory);
                serviceChannelFactoryCache.Add(typeof(TServiceContract).ToString(), serviceChannelFactory, DateTimeOffset.Now.AddMinutes(10));
                return serviceChannelFactory;
            }
            return (ChannelFactory<TServiceContract>) sf;
        }

        private X509Certificate2 ServiceCertificatePublic
        {
            get
            {
                var WifEncryptionCertname = ConfigurationManager.AppSettings["WifEncryptionCertname"];
                var store = new X509Store(StoreLocation.LocalMachine);
                try
                {
                    store.Open(OpenFlags.ReadOnly);
                    var certificateCollection = store.Certificates;
                    var certificate = certificateCollection.Find(X509FindType.FindBySubjectName, WifEncryptionCertname, true);
                    if (certificate.Count > 0)
                    {
                        return certificate[0];
                    }
                    else throw new ConfigurationErrorsException(string.Format("Did not find a valid X509 certificate with SubjectName = {0} in X509 Store", WifEncryptionCertname));
                }
                finally
                {
                    store.Close();
                }
            }
        }

        private Binding CreateIssuedTokenOverTransportBinding()
        {
            var securityTokenParameters = new IssuedSecurityTokenParameters
                {
                    KeyType = SecurityKeyType.AsymmetricKey,
                    IssuerBinding = CreateKerberosOverTransportBinding(),
                };

            var securityBindingElement = SecurityBindingElement.CreateIssuedTokenOverTransportBindingElement(securityTokenParameters);
            securityBindingElement.MessageSecurityVersion = MessageSecurityVersion.WSSecurity11WSTrust13WSSecureConversation13WSSecurityPolicy12BasicSecurityProfile10;

            var customBinding = new CustomBinding("Custom");
            customBinding.Elements.Insert(0, securityBindingElement);
            return customBinding;
        }

        private Binding CreateKerberosOverTransportBinding()
        {
            var securityBindingElement = SecurityBindingElement.CreateKerberosOverTransportBindingElement();
            securityBindingElement.MessageSecurityVersion = MessageSecurityVersion.WSSecurity11WSTrust13WSSecureConversation13WSSecurityPolicy12BasicSecurityProfile10;

            var httpsTransportBindingElement = new HttpsTransportBindingElement
                {
                    MaxBufferPoolSize = 524288,
                    MaxReceivedMessageSize = 200000000,
                    UseDefaultWebProxy = false
                };

            var bec = new BindingElementCollection
                {
                    securityBindingElement,
                    new TextMessageEncodingBindingElement(),
                    httpsTransportBindingElement
                };

            return new CustomBinding(bec);
        }

        private Binding CreateWs2007FederationHttpBinding(bool secureSessionOff)
        {
            Binding binding = new WS2007FederationHttpBinding("WS2007FederationHttpBinding_IServices");

            ((WSFederationHttpBinding) binding).Security.Message.IssuerBinding = CreateKerberosOverTransportBinding();
            if (secureSessionOff)
            {
                ((WSFederationHttpBinding) binding).Security.Message.NegotiateServiceCredential = false;
                var revisedBinding = CreateFederationBindingWithoutSecureSession(binding as WSFederationHttpBinding);
                return revisedBinding;
            }

            return binding;
        }

        private Binding CreateFederationBindingWithoutSecureSession(WSFederationHttpBinding inputBinding)
        {
            // This CustomBinding starts out identical to the specified WSFederationHttpBinding.
            var outputBinding = new CustomBinding(inputBinding.CreateBindingElements());

            // Find the SecurityBindingElement for message security.
            var security = outputBinding.Elements.Find<SecurityBindingElement>();

            // If the security mode is message, then the secure session settings are the protection token parameters.
            SecureConversationSecurityTokenParameters secureConversation;
            if (WSFederationHttpSecurityMode.Message == inputBinding.Security.Mode)
            {
                var symmetricSecurity = security as SymmetricSecurityBindingElement;
                secureConversation = symmetricSecurity.ProtectionTokenParameters as SecureConversationSecurityTokenParameters;
            }
                // If the security mode is message, then the secure session settings are the endorsing token parameters.
            else if (WSFederationHttpSecurityMode.TransportWithMessageCredential == inputBinding.Security.Mode)
            {
                var transportSecurity = security as TransportSecurityBindingElement;
                secureConversation = transportSecurity.EndpointSupportingTokenParameters.Endorsing[0] as SecureConversationSecurityTokenParameters;
            }
            else
            {
                throw new NotSupportedException(String.Format("Unhandled security mode {0}.", inputBinding.Security.Mode));
            }

            // Replace the secure session SecurityBindingElement with the bootstrap SecurityBindingElement.
            int securityIndex = outputBinding.Elements.IndexOf(security);
            outputBinding.Elements[securityIndex] = secureConversation.BootstrapSecurityBindingElement;

            // Return modified binding.
            return outputBinding;
        }
    }
}
