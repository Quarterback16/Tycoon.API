using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using System.IdentityModel.Claims;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Claim = System.Security.Claims.Claim;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="SecurityAttribute" />.
    /// </summary>
    [TestClass]
    public class SecurityAttributeTest
    {
        private SecurityAttribute SystemUnderTest()
        {
            return new SecurityAttribute();
        }

        public class TestController : Controller
        {
            [SecurityAttribute(Roles = new[] { "Role" })]
            public ActionResult RequireRole()
            {
                return View();
            }

            [SecurityAttribute(AllowAny = true)]
            public ActionResult AllowAny()
            {
                return View();
            }

            [SecurityAttribute(AllowInProduction = false)]
            public ActionResult ProdViewDisabled()
            {
                return View();
            }
        }


        private Mock<IContainerProvider> mockContainerProvider;
        private Mock<IUserService> mockUserService;
        private Mock<ClaimsPrincipal> mockClaimsPrincipal;
        private Mock<ClaimsIdentity> mockClaimsIdentity;
        private Mock<IConfigurationManager> mockConfiguration;

        //Use TestInitialize to run code before running each test
        [TestInitialize]
        public void TestInitialize()
        {
            mockContainerProvider = new Mock<IContainerProvider>();
            mockUserService = new Mock<IUserService>();
            mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
            mockClaimsIdentity = new Mock<ClaimsIdentity>();
            mockConfiguration = new Mock<IConfigurationManager>();

            // Setup principal to return identity
            mockClaimsPrincipal.Setup(m => m.Identity).Returns(mockClaimsIdentity.Object);

            // Setup identity to default as authenticated
            mockClaimsIdentity.Setup(m => m.Name).Returns("User");
            mockClaimsIdentity.Setup(m => m.IsAuthenticated).Returns(true);

            // Setup claims identity in User Service
            mockUserService.Setup(m => m.DateTime).Returns(DateTime.Now);
            mockUserService.Setup(m => m.Identity).Returns(mockClaimsIdentity.Object);

            // Setup Dependency Resolver to use mocked Container Provider
            mockContainerProvider.Setup(m => m.GetService<IUserService>()).Returns(mockUserService.Object);
            mockContainerProvider.Setup(m => m.GetService<IConfigurationManager>()).Returns(mockConfiguration.Object);
            DependencyResolver.SetResolver(mockContainerProvider.Object);

            var app = new NameValueCollection();
            app.Add("Environment", "PROD");
            mockConfiguration.Setup(a => a.AppSettings).Returns(app);
        }

        /// <summary>
        /// Test null reference exception when Dependency Resolver does not have the IUserService registered.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Security_NoUserServiceInDependencyResolver_ThrowsNullReferenceException()
        {
            mockContainerProvider = new Mock<IContainerProvider>();
            DependencyResolver.SetResolver(mockContainerProvider.Object);

            var sut = SystemUnderTest();

            var date = sut.UserService.DateTime.Date;

            Assert.IsTrue(date != DateTime.MinValue);
        }

        /// <summary>
        /// Test null reference exception when Dependency Resolver is not a Container Provider.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Security_NoContainerProviderInDependencyResolver_ThrowsNullReferenceException()
        {
            var mockDependencyResolver = new Mock<IDependencyResolver>();
            DependencyResolver.SetResolver(mockDependencyResolver.Object);

            var sut = SystemUnderTest();

            var date = sut.UserService.DateTime.Date;

            Assert.IsTrue(date != DateTime.MinValue);
        }

        [TestMethod]
        public void Security_UserWindowsAuthentication()
        {
            var claimCollection = new List<System.Security.Claims.Claim>();
            claimCollection.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod", "http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/windows"));

            mockClaimsIdentity.Setup(m => m.Claims).Returns(claimCollection);
            var sut = SystemUnderTest();
            Assert.IsFalse(sut.AllowWindowsAuthentication);

            sut.AllowWindowsAuthentication = true;
            Assert.IsTrue(sut.IsAuthorized());
        }

        /// <summary>
        /// Test fails if user is not authenticated.
        /// </summary>
        [TestMethod]
        public void Security_UserNotAuthenticated_Fails()
        {
            // Set as not authenticated
            mockClaimsIdentity.Setup(m => m.IsAuthenticated).Returns(false);

            var sut = SystemUnderTest();

            sut.AllowAny = true;

            Assert.IsFalse(sut.IsAuthorized());
        }

        /// <summary>
        /// Test fails if user identity is null.
        /// </summary>
        [TestMethod]
        public void Security_UserIdentityIsNull_Fails()
        {
            var sut = SystemUnderTest();

            sut.AllowAny = true;

            Assert.IsFalse(sut.IsAuthorized(null));
        }

        /// <summary>
        /// Test validates if allow any is true.
        /// </summary>
        [TestMethod]
        public void Security_AllowAnyIsTrue_Validates()
        {
            var sut = SystemUnderTest();

            sut.AllowAny = true;

            Assert.IsTrue(sut.IsAuthorized());
        }

        /// <summary>
        /// Test validates if allow any is true.
        /// </summary>
        [TestMethod]
        public void Security_AllowedInProduction_Validates()
        {
            var sut = SystemUnderTest();

            Assert.IsTrue(mockConfiguration.Object.AppSettings["Environment"] == "PROD");

            sut.AllowInProduction = true;
            sut.AllowAny = true;
            Assert.IsTrue(sut.IsAuthorized());

            sut.AllowInProduction = false;
            Assert.IsFalse(sut.IsAuthorized());
        }

        /// <summary>
        /// Test validates if allow any is true.
        /// </summary>
        [TestMethod]
        public void Security_AllowedInProductionPreProd_Validates()
        {
            var sut = SystemUnderTest();

            var app = new NameValueCollection {{"Environment", "PREPROD"}};
            mockConfiguration.Setup(a => a.AppSettings).Returns(app);

            Assert.IsTrue(mockConfiguration.Object.AppSettings["Environment"] == "PREPROD");

            sut.AllowInProduction = true;
            sut.AllowAny = true;
            Assert.IsTrue(sut.IsAuthorized());

            sut.AllowInProduction = false;
            Assert.IsFalse(sut.IsAuthorized());
        }


        /// <summary>
        /// Test validates if user has one of the required roles.
        /// </summary>
        [TestMethod]
        public void Security_RolesSetWithRoleUserHas_Validates()
        {
            // Setup claims to contain role
            var claims = new List<System.Security.Claims.Claim>();
            claims.Add(new Claim(Infrastructure.Types.ClaimType.RolesBase, "Role"));
            mockClaimsIdentity.Setup(m => m.Claims).Returns(claims);

            var sut = SystemUnderTest();

            sut.Roles = new[] { "Role" };

            Assert.IsTrue(sut.IsAuthorized());
        }

        /// <summary>
        /// Test fails if user does not have any of the required roles.
        /// </summary>
        [TestMethod]
        public void Security_RolesSetWithRoleUserDoesNotHave_Fails()
        {
            // Setup claims to not contain role
            var claims = new List<System.Security.Claims.Claim>();
            mockClaimsIdentity.Setup(m => m.Claims).Returns(claims);

            var sut = SystemUnderTest();

            sut.Roles = new[] { "Role" };

            Assert.IsFalse(sut.IsAuthorized());
        }

        /// <summary>
        /// Test validates if user has one of the required contracts.
        /// </summary>
        [TestMethod]
        public void Security_ContractsSetWithContractUserHas_Validates()
        {
            // Setup claims to contain contract
            var claims = new List<System.Security.Claims.Claim>();
            claims.Add(new Claim(Infrastructure.Types.ClaimType.Contracts, "Contract"));
            mockClaimsIdentity.Setup(m => m.Claims).Returns(claims);

            var sut = SystemUnderTest();

            sut.Contracts = new[] { "Contract" };

            Assert.IsTrue(sut.IsAuthorized());
        }

        /// <summary>
        /// Test fails if user does not have any of the required contracts.
        /// </summary>
        [TestMethod]
        public void Security_ContractsSetWithContractUserDoesNotHave_Fails()
        {
            // Setup claims to not contain contract
            var claims = new List<System.Security.Claims.Claim>();
            mockClaimsIdentity.Setup(m => m.Claims).Returns(claims);

            var sut = SystemUnderTest();

            sut.Roles = new[] { "Contract" };

            Assert.IsFalse(sut.IsAuthorized());
        }

        /// <summary>
        /// Test validates if user has one of the required organisation codes.
        /// </summary>
        [TestMethod]
        public void Security_OrganisationCodesSetWithOrganisationCodeUserHas_Validates()
        {
            // Setup claims to contain organisation code
            var claims = new List<System.Security.Claims.Claim>();
            claims.Add(new Claim(Infrastructure.Types.ClaimType.Organisation, "OrganisationCode"));
            mockClaimsIdentity.Setup(m => m.Claims).Returns(claims);

            var sut = SystemUnderTest();

            sut.OrganisationCodes = new[] { "OrganisationCode" };

            Assert.IsTrue(sut.IsAuthorized());
        }

        /// <summary>
        /// Test fails if user does not have any of the required organisation codes.
        /// </summary>
        [TestMethod]
        public void Security_OrganisationCodesSetWithOrganisationCodeUserDoesNotHave_Fails()
        {
            // Setup claims to not contain organisation code
            var claims = new List<System.Security.Claims.Claim>();
            mockClaimsIdentity.Setup(m => m.Claims).Returns(claims);

            var sut = SystemUnderTest();

            sut.OrganisationCodes = new[] { "OrganisationCode" };

            Assert.IsFalse(sut.IsAuthorized());
        }

        /// <summary>
        /// Test validates if user is one of the required users.
        /// </summary>
        [TestMethod]
        public void Security_UsersSetWithUser_Validates()
        {
            // Setup claim name to return matching name
            mockClaimsIdentity.Setup(m => m.Name).Returns("User");

            var sut = SystemUnderTest();

            sut.Users = new[] { "User" };

            Assert.IsTrue(sut.IsAuthorized());
        }

        /// <summary>
        /// Test fails if user is not one of the required users.
        /// </summary>
        [TestMethod]
        public void Security_UsersNotSetWithUser_Fails()
        {
            // Setup claim name to return non-matching name
            mockClaimsIdentity.Setup(m => m.Name).Returns(string.Empty);

            var sut = SystemUnderTest();

            sut.Users = new[] { "User" };

            Assert.IsFalse(sut.IsAuthorized());
        }

        /// <summary>
        /// Test validates if allow any is true.
        /// </summary>
        [TestMethod]
        public void Security_AllowAnyIsTrueCausesOtherPropertiesToBeNull_OtherPropertiesAreNull()
        {
            var sut = SystemUnderTest();

            sut.AllowAny = true;

            Assert.IsNull(sut.Roles);
            Assert.IsNull(sut.Contracts);
            Assert.IsNull(sut.OrganisationCodes);
            Assert.IsNull(sut.Users);
        }

        /// <summary>
        /// Test unauthorized access returns Http Status Code of 401 Forbidden.
        /// </summary>
        [TestMethod]
        public void Security_InvokeControllerActionWithUserThatDoesNotHaveRequiredRole_UnauthorizedReturnsStatus401()
        {
            var claims = new List<System.Security.Claims.Claim>();
            mockClaimsIdentity.Setup(m => m.Claims).Returns(claims);

            var controller = new TestController();

            var httpContext = GetFakeAuthenticatedHttpContext(mockClaimsPrincipal.Object);

            controller.ControllerContext = new ControllerContext { Controller = controller, RequestContext = new RequestContext(httpContext, new RouteData()) };

            var result = controller.ActionInvoker.InvokeAction(controller.ControllerContext, "RequireRole");

            var statusCode = httpContext.Response.StatusCode;

            Assert.IsTrue(result, "Invoker Result");
            Assert.AreEqual(401, statusCode, "Status Code");
        }

        private HttpContextBase GetFakeAuthenticatedHttpContext(ClaimsPrincipal user)
        {
            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            var response = new Mock<HttpResponseBase>();
            var session = new Mock<HttpSessionStateBase>();
            var server = new Mock<HttpServerUtilityBase>();
            var policy = new Mock<HttpCachePolicyBase>();

            context.Setup(ctx => ctx.User).Returns(user);
            context.Setup(ctx => ctx.Items).Returns(new Dictionary<object, object>());
            context.Setup(ctx => ctx.Request).Returns(request.Object);
            context.Setup(ctx => ctx.Response).Returns(response.Object);
            context.Setup(ctx => ctx.Response.Cache).Returns(policy.Object);
            context.Setup(ctx => ctx.Session).Returns(session.Object);
            context.Setup(ctx => ctx.Server).Returns(server.Object);

            return context.Object;
        }
    }
}