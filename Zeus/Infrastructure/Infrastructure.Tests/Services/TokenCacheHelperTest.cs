using System;
using System.IdentityModel.Tokens;
using Employment.Web.Mvc.Infrastructure.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.Services
{
    /// <summary>
    ///This is a test class for TokenCacheHelperTest and is intended
    ///to contain all TokenCacheHelperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TokenCacheHelperTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        ///A test for AddToken
        ///</summary>
        [TestMethod()]
        public void AddTokenTest()
        {
            var key = new Uri("http://www.tokencache.com/");
            SecurityToken token = new SamlSecurityToken(new SamlAssertion());
            TokenCacheHelper.AddToken(key, token);
            var t = TokenCacheHelper.GetToken(key);
            Assert.AreEqual(token,t);

            TokenCacheHelper.AddToken(key, token);
            t = TokenCacheHelper.GetToken(key);
            Assert.AreEqual(token, t);
        }

        /// <summary>
        ///A test for GetToken
        ///</summary>
        [TestMethod()]
        public void GetTokenTest()
        {
            var key = new Uri("http://www.contoso.com/");
            SecurityToken actual = TokenCacheHelper.GetToken(key);
            Assert.IsNull(actual);
        }
    }
}