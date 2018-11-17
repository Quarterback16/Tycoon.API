using System.Linq;
using System.Security.Claims;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Moq;
using Claim = System.Security.Claims.Claim;

namespace Employment.Web.Mvc.Infrastructure.Tests.Extensions
{
    /// <summary>
    ///This is a test class for ClaimsIdentityExtensionTest and is intended
    ///to contain all ClaimsIdentityExtensionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ClaimsIdentityExtensionTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        private List<Claim> GetClaims(ClaimsIdentity subject)
        {
            var c = new List<Claim>();
            c.Add(new Claim("http://deewr.gov.au/es/2011/03/claims/orgcontract", "AAA"));
            c.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name","XX999999"));
            c.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname", "First"));
            c.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname", "Last"));
            c.Add(new Claim("http://deewr.gov.au/es/2011/03/claims/org", "Org"));
            c.Add(new Claim("http://deewr.gov.au/es/2011/03/claims/baserole", "BBB"));
            c.Add(new Claim("http://deewr.gov.au/es/2011/03/claims/generalrole", "CCC"));
            c.Add(new Claim("http://deewr.gov.au/es/2011/03/claims/reportingrole", "DDD"));
            c.Add(new Claim("http://deewr.gov.au/es/2011/03/claims/defaultsite","SITE"));

            return c;
        }

        private ClaimsIdentity SystemUnderTest()
        {
            var identity = new Mock<ClaimsIdentity>();
            var subject = new Mock<ClaimsIdentity>();
            var c = GetClaims(subject.Object);

            identity.Setup(i => i.Claims).Returns(c);

            return identity.Object;
        }

        /// <summary>
        ///A test for GetClaims
        ///</summary>
        [TestMethod()]
        public void NullIdentityTest()
        {
            var c = ((ClaimsIdentity)null).GetContracts();
            Assert.IsNotNull(c);
            Assert.IsFalse(c.Any());
        }

        /// <summary>
        ///A test for GetContracts
        ///</summary>
        [TestMethod()]
        public void GetContractsTest()
        {
            IEnumerable<string> actual = SystemUnderTest().GetContracts();
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Any());
            Assert.IsTrue(actual.Contains("AAA"));
        }

        /// <summary>
        ///A test for GetFirstName
        ///</summary>
        [TestMethod()]
        public void GetFirstNameTest()
        {
            IEnumerable<string> actual = SystemUnderTest().GetFirstName();
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Any());
            Assert.IsTrue(actual.Contains("First"));
        }

        /// <summary>
        ///A test for GetLastName
        ///</summary>
        [TestMethod()]
        public void GetLastNameTest()
        {
            var actual = SystemUnderTest().GetLastName();
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Any());
            Assert.IsTrue(actual.Contains("Last"));
        }

        /// <summary>
        ///A test for GetOrganisationCodes
        ///</summary>
        [TestMethod()]
        public void GetOrganisationCodesTest()
        {
            var actual = SystemUnderTest().GetOrganisationCodes();
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Any());
            Assert.IsTrue(actual.Contains("Org"));
        }

        /// <summary>
        ///A test for GetRoles
        ///</summary>
        [TestMethod()]
        public void GetRolesTest()
        {
            var actual = SystemUnderTest().GetRoles();
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Any());
            Assert.IsTrue(actual.Contains("BBB"));
            Assert.IsTrue(actual.Contains("CCC"));
            Assert.IsTrue(actual.Contains("DDD"));
        }

        /// <summary>
        ///A test for GetSiteCodes
        ///</summary>
        [TestMethod()]
        public void GetSiteCodesTest()
        {
            var actual = SystemUnderTest().GetSiteCodes();
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Any());
            Assert.IsTrue(actual.Contains("SITE"));
        }

        /// <summary>
        ///A test for GetUsername
        ///</summary>
        [TestMethod()]
        public void GetUsernameTest()
        {
            var actual = SystemUnderTest().GetUsername();
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Any());
            Assert.IsTrue(actual.Contains("XX999999"));
        }

        /// <summary>
        ///A test for IsInContract
        ///</summary>
        [TestMethod()]
        public void IsInContractTest()
        {
            var actual = SystemUnderTest().IsInContract(new []{"AAA"});
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);

            actual = SystemUnderTest().IsInContract(new[] { "CCC" });
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual);

            actual = SystemUnderTest().IsInContract(null);
            Assert.IsFalse(actual);
        }

        /// <summary>
        ///A test for IsInOrganisation
        ///</summary>
        [TestMethod()]
        public void IsInOrganisationTest()
        {
            var actual = SystemUnderTest().IsInOrganisation(new[] { "AAA" });
            Assert.IsFalse(actual);

            actual = SystemUnderTest().IsInOrganisation(new[] { "Org" });
            Assert.IsTrue(actual);

            actual = SystemUnderTest().IsInOrganisation(null);
            Assert.IsFalse(actual);
        }

        /// <summary>
        ///A test for IsInRole
        ///</summary>
        [TestMethod()]
        public void IsInRoleTest()
        {
            var actual = SystemUnderTest().IsInRole(new[] { "BBB" });
            Assert.IsTrue(actual);

            actual = SystemUnderTest().IsInRole(new[] { "ZZZ" });
            Assert.IsFalse(actual);

            actual = SystemUnderTest().IsInRole(null);
            Assert.IsFalse(actual);

        }

        /// <summary>
        ///A test for IsInSite
        ///</summary>
        [TestMethod()]
        public void IsInSiteTest()
        {
            var actual = SystemUnderTest().IsInSite(new[] { "SITE" });
            Assert.IsTrue(actual);

            actual = SystemUnderTest().IsInSite(new[] { "WXYZ" });
            Assert.IsFalse(actual);


            actual = SystemUnderTest().IsInSite(null);
            Assert.IsFalse(actual);
        }
    }
}
