using System;
using System.IdentityModel.Tokens;
using System.Runtime.Caching;

namespace Employment.Web.Mvc.Infrastructure.Services
{
    /// <summary> 
    /// Helper class used as cache for security tokens 
    /// </summary> 
    internal static class TokenCacheHelper
    {
        private static readonly MemoryCache tokens = MemoryCache.Default;
        private static readonly CacheItemPolicy cachePolicy = new CacheItemPolicy();
        
        static TokenCacheHelper()
        {
            cachePolicy.SlidingExpiration = new TimeSpan(0, 0, 0, 60);
        }

        /// <summary>
        /// Get a Token according to key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static SecurityToken GetToken(Uri key)
        {
                var ci = tokens.GetCacheItem(key.ToString());
                if (ci == null) return null;
                return (SecurityToken)ci.Value;
        }

        public static void AddToken(Uri key, SecurityToken token)
        {
            if (tokens.Contains(key.ToString()))
            {
                tokens.Remove(key.ToString());
            }

            tokens.Add(key.ToString(), token, cachePolicy);
        }
    }
}