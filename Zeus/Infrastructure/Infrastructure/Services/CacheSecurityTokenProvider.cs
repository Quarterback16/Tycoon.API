using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.ServiceModel.Security.Tokens;
using System.Threading;

namespace Employment.Web.Mvc.Infrastructure.Services
{
    /// <summary>
    /// CacheSecurityTokenProvider
    /// </summary>
    internal class CacheSecurityTokenProvider : SecurityTokenProvider, IDisposable
    {
        private bool disposed;
        private readonly IssuedSecurityTokenProvider innerProvider;

        /// <summary>
        /// CacheSecurityTokenProvider
        /// </summary>
        public CacheSecurityTokenProvider(SecurityTokenRequirement requirement, IssuedSecurityTokenProvider federatedSecurityTokenProvider) : base( )
        {
            innerProvider = federatedSecurityTokenProvider;
            innerProvider.Open();
        }

        /// <summary>
        /// Check for token already cached for user and not expired
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        protected override SecurityToken GetTokenCore(TimeSpan timeout)
        {
            var userName = Thread.CurrentPrincipal.Identity.Name;
            var cacheKey = new Uri(string.Concat(innerProvider.TargetAddress.Uri, innerProvider.IssuerAddress.Uri, userName));
            var securityToken = TokenCacheHelper.GetToken(cacheKey);
            var cacheMiss = securityToken == null || IsSecurityTokenExpired(securityToken);

            if (cacheMiss)
            {
                securityToken = innerProvider.GetToken(timeout);
                // Only add the token to the cache if caching has been turned on in web/app.config.
                if (innerProvider.CacheIssuedTokens)
                {
                    TokenCacheHelper.AddToken(cacheKey, securityToken);
                }
            }

            return securityToken;
        }

        private static bool IsSecurityTokenExpired(SecurityToken serviceToken)
        {
            return DateTime.UtcNow >= serviceToken.ValidTo.ToUniversalTime();
        }

        ~CacheSecurityTokenProvider()
        {
            if (!disposed)
                ((IDisposable)this).Dispose();
        }

        void IDisposable.Dispose()
        {
            innerProvider.Close();
            disposed = true;
        }
    }
}