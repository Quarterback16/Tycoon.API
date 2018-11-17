using System.IdentityModel.Selectors;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security.Tokens;
using System.IdentityModel.Protocols.WSTrust;
using System.ServiceModel.Security;
using System.ServiceModel.Security.Tokens;
using System.ServiceModel.Channels;

namespace Employment.Web.Mvc.Infrastructure.Services
{
    /// <summary>
    /// Security Token manager for use with cache client
    /// </summary>
    internal class CacheClientCredentialsSecurityTokenManager : ClientCredentialsSecurityTokenManager
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="federatedClientCredentials"></param>
        public CacheClientCredentialsSecurityTokenManager(ClientCredentials federatedClientCredentials) : base(federatedClientCredentials) { }

        /// <summary>
        /// Create Security Token provider
        /// </summary>
        /// <param name="tokenRequirement"></param>
        /// <returns></returns>
        public override SecurityTokenProvider CreateSecurityTokenProvider(SecurityTokenRequirement tokenRequirement)
        {
            var provider = base.CreateSecurityTokenProvider(tokenRequirement);
            var federatedSecurityTokenProvider = provider as IssuedSecurityTokenProvider;

            if (federatedSecurityTokenProvider != null && IsIssuedSecurityTokenRequirement(tokenRequirement))
            {
                //var federatedClientCredentialsParameters = FindIssuedTokenClientCredentialsParameters(tokenRequirement);
                provider = new CacheSecurityTokenProvider(tokenRequirement, federatedSecurityTokenProvider);
            }
            return provider;
        }

        //// Lifted from FederatedClientCredentialsSecurityTokenManager's private method by the same name.
        //private static FederatedClientCredentialsParameters FindIssuedTokenClientCredentialsParameters(SecurityTokenRequirement tokenRequirement)
        //{
        //    var parameters = new FederatedClientCredentialsParameters();

        //    ChannelParameterCollection result;
        //    if (tokenRequirement.TryGetProperty(ServiceModelSecurityTokenRequirement.ChannelParametersCollectionProperty, out result) && result != null)
        //    {
        //        foreach (var obj2 in result)
        //        {
        //            if (obj2 is  FederatedClientCredentialsParameters)
        //            {
        //                break;
        //            }
        //        }
        //    }
        //    return parameters;
        //}
    }
}