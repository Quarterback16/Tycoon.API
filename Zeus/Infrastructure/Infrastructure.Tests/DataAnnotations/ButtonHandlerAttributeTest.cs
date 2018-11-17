using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using System.IdentityModel.Claims;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web.Routing;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="ButtonHandlerAttribute" />.
    /// </summary>
    [TestClass]
    public class ButtonHandlerAttributeTest
    {
        public class TestController : Controller
        {
            [ButtonHandler]
            [HttpPost]
            public ActionResult Main()
            {
                return View();
            }

            [ButtonHandler]
            [HttpPost]
            public ActionResult AlternativeHandlerForMain()
            {
                return View();
            }
        }

        private Mock<IContainerProvider> mockContainerProvider;
        private Mock<IUserService> mockUserService;
        private Mock<ClaimsPrincipal> mockClaimsPrincipal;
        private Mock<ClaimsIdentity> mockClaimsIdentity;

        //Use TestInitialize to run code before running each test
        [TestInitialize]
        public void TestInitialize()
        {
            mockContainerProvider = new Mock<IContainerProvider>();
            mockUserService = new Mock<IUserService>();
            mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
            mockClaimsIdentity = new Mock<ClaimsIdentity>();

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
            DependencyResolver.SetResolver(mockContainerProvider.Object);
        }

        /// <summary>
        /// Test valid.
        /// </summary>
        [TestMethod]
        public void ButtonHandler_IsValidNameWithValid_Validates()
        {
            var controller = new TestController();

            var methodInfo = controller.GetType().GetMethods().FirstOrDefault(m => m.Name == "Main");

            var httpContext = GetFakeAuthenticatedHttpContext(mockClaimsPrincipal.Object, "Main");

            controller.ControllerContext = new ControllerContext { Controller = controller, RequestContext = new RequestContext(httpContext, new RouteData()) };

            var systemUnderTest = (ButtonHandlerAttribute)methodInfo.GetCustomAttributes(typeof(ButtonHandlerAttribute), false)[0];

            Assert.IsTrue(systemUnderTest.IsValidName(controller.ControllerContext, "Main", methodInfo));
        }

        /// <summary>
        /// Test valid alternative handler is found.
        /// </summary>
        [TestMethod]
        public void ButtonHandler_IsValidNameWithValidAlternative_Validates()
        {
            var controller = new TestController();

            var methodInfo = controller.GetType().GetMethods().FirstOrDefault(m => m.Name == "AlternativeHandlerForMain");

            var httpContext = GetFakeAuthenticatedHttpContext(mockClaimsPrincipal.Object, "AlternativeHandlerForMain");

            controller.ControllerContext = new ControllerContext { Controller = controller, RequestContext = new RequestContext(httpContext, new RouteData()) };

            var systemUnderTest = (ButtonHandlerAttribute)methodInfo.GetCustomAttributes(typeof(ButtonHandlerAttribute), false)[0];

            Assert.IsTrue(systemUnderTest.IsValidName(controller.ControllerContext, "AlternativeHandlerForMain", methodInfo));
        }

        /// <summary>
        /// Test fail not matching handler.
        /// </summary>
        [TestMethod]
        public void ButtonHandler_IsValidNameWithDifferentSubmitType_Fails()
        {
            var controller = new TestController();

            var methodInfo = controller.GetType().GetMethods().FirstOrDefault(m => m.Name == "AlternativeHandlerForMain");

            var httpContext = GetFakeAuthenticatedHttpContext(mockClaimsPrincipal.Object, "Main");

            controller.ControllerContext = new ControllerContext { Controller = controller, RequestContext = new RequestContext(httpContext, new RouteData()) };

            var systemUnderTest = (ButtonHandlerAttribute)methodInfo.GetCustomAttributes(typeof(ButtonHandlerAttribute), false)[0];

            Assert.IsFalse (systemUnderTest.IsValidName(controller.ControllerContext, "AlternativeHandlerForMain", methodInfo));
        }

        /// <summary>
        /// Test fail not matching handler.
        /// </summary>
        [TestMethod]
        public void ButtonHandler_IsValidNameWithNullSubmitType_Fails()
        {
            var controller = new TestController();

            var methodInfo = controller.GetType().GetMethods().FirstOrDefault(m => m.Name == "AlternativeHandlerForMain");

            var httpContext = GetFakeAuthenticatedHttpContext(mockClaimsPrincipal.Object, null);

            controller.ControllerContext = new ControllerContext { Controller = controller, RequestContext = new RequestContext(httpContext, new RouteData()) };

            var systemUnderTest = (ButtonHandlerAttribute)methodInfo.GetCustomAttributes(typeof(ButtonHandlerAttribute), false)[0];

            Assert.IsFalse(systemUnderTest.IsValidName(controller.ControllerContext, "AlternativeHandlerForMain", methodInfo));
        }

        private HttpContextBase GetFakeAuthenticatedHttpContext(ClaimsPrincipal user, string submitType)
        {
            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            var response = new Mock<HttpResponseBase>();
            var session = new Mock<HttpSessionStateBase>();
            var server = new Mock<HttpServerUtilityBase>();
            var policy = new Mock<HttpCachePolicyBase>();


            request.Setup(m => m.HttpMethod).Returns("POST");
            request.Setup(m => m[ButtonAttribute.SubmitTypeName]).Returns(submitType);

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
