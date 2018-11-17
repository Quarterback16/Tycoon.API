using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Claim = System.Security.Claims.Claim;

namespace Employment.Web.Mvc.Infrastructure.Tests.Services
{
    [TestClass]
    public class UserServiceTest
    {
        private List<Claim> GetClaims(ClaimsIdentity subject)
        {
            var c = new List<Claim>();
            c.Add(new Claim("http://deewr.gov.au/es/2011/03/claims/orgcontract", "AAA"));
            c.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "XX999999"));
            c.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname", "First"));
            c.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname", "Last"));
            c.Add(new Claim("http://deewr.gov.au/es/2011/03/claims/org", "Org"));
            c.Add(new Claim("http://deewr.gov.au/es/2011/03/claims/baserole", "BBB"));
            c.Add(new Claim("http://deewr.gov.au/es/2011/03/claims/generalrole", "CCC"));
            c.Add(new Claim("http://deewr.gov.au/es/2011/03/claims/reportingrole", "DDD"));
            c.Add(new Claim("http://deewr.gov.au/es/2011/03/claims/defaultsite", "SITE"));

            return c;
        }

        private Mock<ISessionService> mockSession;
        private Mock<ClaimsIdentity> mockIdentity;
        private Mock<IContainerProvider> mockContainerProvider;
        private Mock<IHistoryService> mockHistoryService;
        private Mock<IConfigurationManager> mockConfigurationManager;
        private Mock<NameValueCollection> mockAppSettings;

        private UserService SystemUnderTest()
        {
            return new UserService(mockSession.Object, mockIdentity.Object);
        }

        //Use TestInitialize to run code before running each test
        [TestInitialize]
        public void TestInitialize()
        {
            mockSession = new Mock<ISessionService>();
            mockIdentity = new Mock<ClaimsIdentity>();
            mockContainerProvider = new Mock<IContainerProvider>();
            mockHistoryService = new Mock<IHistoryService>();
            mockConfigurationManager = new Mock<IConfigurationManager>();

            mockAppSettings = new Mock<NameValueCollection>();
            mockAppSettings.Setup(m => m.Get(It.IsAny<string>())).Returns(string.Empty);
            
            mockConfigurationManager.Setup(m => m.AppSettings).Returns(mockAppSettings.Object);

            var c = GetClaims(mockIdentity.Object);

            mockIdentity.Setup(i => i.Claims).Returns(c);
            mockIdentity.Setup(auth => auth.IsAuthenticated).Returns(true);

            // Setup Dependency Resolver to use mocked Container Provider
            mockContainerProvider.Setup(m => m.GetService<IHistoryService>()).Returns(mockHistoryService.Object);
            mockContainerProvider.Setup(m => m.GetService<IConfigurationManager>()).Returns(mockConfigurationManager.Object);
            DependencyResolver.SetResolver(mockContainerProvider.Object);

            var request = new HttpRequest("", "http://localhost/", "");
            request.Cookies.Add(new HttpCookie("UserDateTime", DateTime.Now.ToString("o")));
            HttpContext.Current = new HttpContext(request, new HttpResponse(new StringWriter()));
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UserService_ArgumentNullExceptionTest()
        {
            new UserService(null, null);
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UserService_ArgumentNullExceptionTest1()
        {
            new UserService(null,null);
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UserService_ArgumentNullExceptionTest2()
        {
            new UserService(new Mock<ISessionService>().Object, null);
            Assert.Fail();
        }


        [TestMethod]
        public void UserService_ConstructorTest()
        {
            var userService = SystemUnderTest();
            Assert.IsNotNull(userService);
        }

        [TestMethod]
        public void UserService_RolesTest()
        {
            var userService = SystemUnderTest();
            var actual = userService.Roles;
            Assert.IsNotNull(actual);
            var roles = actual as string[] ?? actual.ToArray();
            Assert.IsTrue(roles.Any());
            Assert.IsTrue(roles.Contains("BBB"));
            Assert.IsTrue(roles.Contains("CCC"));
            Assert.IsTrue(roles.Contains("DDD"));
        }

        [TestMethod]
        public void UserService_GeneralRolesTest()
        {
            var userService = SystemUnderTest();
            var actual = userService.GeneralRoles;
            Assert.IsNotNull(actual);
            var roles = actual as string[] ?? actual.ToArray();
            Assert.IsTrue(roles.Any());
            Assert.IsTrue(!roles.Contains("BBB"));
            Assert.IsTrue(roles.Contains("CCC"));
            Assert.IsTrue(!roles.Contains("DDD"));
        }

        [TestMethod]
        public void UserService_OrganisationCodesTest()
        {
            var userService = SystemUnderTest();
            var actual = userService.OrganisationCodes;
            Assert.IsNotNull(actual);
            var codes = actual as string[] ?? actual.ToArray();
            Assert.IsTrue(codes.Any());
            Assert.IsTrue(codes.Contains("Org"));
        }

        [TestMethod]
        public void UserService_OrganisationCodeTest()
        {
            var userService = SystemUnderTest();
            var actual = userService.OrganisationCode;
            Assert.IsNotNull(actual);
            Assert.AreEqual("Org",actual);
        }

        [TestMethod]
        public void UserService_SiteCodesTest()
        {
            var userService = SystemUnderTest();
            var actual = userService.SiteCodes;
            Assert.IsNotNull(actual);
            var sites = actual as string[] ?? actual.ToArray();
            Assert.IsTrue(sites.Any());
            Assert.IsTrue(sites.Contains("SITE"));
            
            Assert.IsFalse(sites.Contains("SITE4"));
        }

        [TestMethod]
        public void UserService_SiteCodeTest()
        {
            var userService = SystemUnderTest();
            var actual = userService.SiteCode;
            Assert.IsNotNull(actual);
            Assert.AreEqual("SITE",actual);
            Assert.AreNotEqual("SITE1",actual);
        }

        [TestMethod]
        public void UserService_ContractsTest()
        {
            var userService = SystemUnderTest();
            var actual = userService.Contracts;
            Assert.IsNotNull(actual);
            var c = actual as string[] ?? actual.ToArray();
            Assert.IsTrue(c.Any());
            Assert.IsTrue(c.Contains("AAA"));
            Assert.IsFalse(c.Contains("BBB"));
        }

        [TestMethod]
        public void UserService_FirstnameTest()
        {
            var userService = SystemUnderTest();
            var first = userService.FirstName;
            Assert.IsNotNull(first);
            Assert.AreEqual("First", first);
        }

        [TestMethod]
        public void UserService_LastnameTest()
        {
            var userService = SystemUnderTest();
            var last = userService.LastName;
            Assert.IsNotNull(last);
            Assert.AreEqual("Last", last);
        }

        [TestMethod]
        public void UserService_UserNameTest()
        {
            var userService = SystemUnderTest();
            var uname = userService.Username;
            Assert.IsFalse(string.IsNullOrEmpty(uname));
        }

        [TestMethod]
        public void UserService_SessionTest()
        {
            var userService = SystemUnderTest();
            Assert.IsNotNull(userService.Session);
        }

        /// <summary>
        ///A test for UserService_RecentHistoryTest
        ///</summary>
        [TestMethod]
        public void UserService_RecentHistoryTest()
        {
            var target = SystemUnderTest();
            Assert.IsNotNull(target.History);
        }

        /// <summary>
        /// Test property null reference exception when Dependency Resolver does not have the IRecentHistoryService registered.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void UserService_NoRecentHistoryServiceInDependencyResolver_ThrowsNullReferenceException()
        {
            mockContainerProvider = new Mock<IContainerProvider>();
            DependencyResolver.SetResolver(mockContainerProvider.Object);
            var target = SystemUnderTest();
            target.History.Get(Infrastructure.Types.HistoryType.JobSeeker);
        }

        /// <summary>
        /// Test property null reference exception when Dependency Resolver is not a Container Provider.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void UserService_NoContainerProviderInDependencyResolver_ThrowsNullReferenceException()
        {
            var mockDependencyResolver = new Mock<IDependencyResolver>();
            DependencyResolver.SetResolver(mockDependencyResolver.Object);
            var target = SystemUnderTest();
            target.History.Get(Infrastructure.Types.HistoryType.JobSeeker);
        }

        [TestMethod]
        public void UserService_IsAuthenticated()
        {
            var userService = SystemUnderTest();
            Assert.IsTrue(userService.IsAuthenticated);
        }

        [TestMethod]
        public void UserService_DateTimeTest()
        {
            var userService = SystemUnderTest();
            Assert.IsNotNull(userService.DateTime);
        }

        [TestMethod]
        public void UserService_IsInSiteTest()
        {
            var userService = SystemUnderTest();
            Assert.IsTrue(userService.IsInSite(new[] { "XXXX","SITE"}));
            Assert.IsFalse(userService.IsInSite(new[] { "XXXX" }));
        }

        [TestMethod]
        public void UserService_IsInRoleTest()
        {
            var userService = SystemUnderTest();
            Assert.IsTrue(userService.IsInRole(new[] { "BBB", "CCC" }));
            Assert.IsFalse(userService.IsInRole(new[] { "XXXX" }));
        }


        [TestMethod]
        public void UserService_IsInOrganisationTest()
        {
            var userService = SystemUnderTest();
            Assert.IsTrue(userService.IsInOrganisation(new[] { "Org1", "Org" }));
            Assert.IsFalse(userService.IsInRole(new[] { "Org9" }));
        }

        [TestMethod]
        public void UserService_IsInContractTest()
        {
            var userService = SystemUnderTest();
            Assert.IsTrue(userService.IsInContract(new[] { "BBB", "AAA" }));
            Assert.IsFalse(userService.IsInContract(new[] { "CCC" }));
        }

        [TestMethod]
        public void UserService_IdentityTest()
        {
            var userService = SystemUnderTest();
            Assert.IsNotNull(userService.Identity);
        }
    }
}
