using System.IdentityModel.Selectors;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.IdentityModel.Protocols.WSTrust;
using System.ServiceModel.Security;
using System.ServiceModel.Security.Tokens;
using System.ServiceModel.Channels;

namespace Employment.Web.Mvc.Infrastructure.Services
{
    /// Client-side token cache for WCF
    /// WCF by default maintains a cache for security tokens per channel instance (A channel is related to a contract). 
    /// Therefore, it is not possible to reuse the same token for different channel instances.
    /// http://msdn.microsoft.com/en-us/library/ms730868.aspx
    /// Code from
    /// http://travisspencer.com/blog/2009/03/caching-tokens-to-avoid-calls.html
    internal class CacheClientCredentials : ClientCredentials
    {
        /// <summary>
        /// Cache for Client credentials
        /// </summary>
        private CacheClientCredentials()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        private CacheClientCredentials(ClientCredentials other) : base(other)
        {
        }

        protected override ClientCredentials CloneCore()
        {
            return new CacheClientCredentials();
        }

        //TODO: Can this caching still be done? It looks like we've lost access to it
        public override SecurityTokenManager CreateSecurityTokenManager()
        {
            return new CacheClientCredentialsSecurityTokenManager(this);
        }

        /// <summary>
        /// Replace any ClientCredential behavior
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channelFactory"></param>
        public static void ConfigureChannelFactory<T>(ChannelFactory<T> channelFactory)
        {
            var other = channelFactory.Endpoint.Behaviors.Find<ClientCredentials>();
            if (other == null) return;

            channelFactory.Endpoint.Behaviors.Remove(other.GetType());
            var item = new CacheClientCredentials(other);
            channelFactory.Endpoint.Behaviors.Add(item);
        }
    }
}