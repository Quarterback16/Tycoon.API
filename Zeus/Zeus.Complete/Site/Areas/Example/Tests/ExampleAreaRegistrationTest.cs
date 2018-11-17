using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Employment.Web.Mvc.Area.Example.Tests
{
    /// <summary>
    /// Unit tests for <see cref="ExampleAreaRegistration" />.
    /// </summary>
    [TestClass]
    public class ExampleAreaRegistrationTest
    {
        private ExampleAreaRegistration SystemUnderTest()
        {
            return new ExampleAreaRegistration();
        }

        /// <summary>
        /// Test Example Area Registration is named correctly.
        /// </summary>
        [TestMethod]
        public void ExampleAreaRegistration_Valid_AreaNameIsExample()
        {
            var area = "Example";
            var areaRegistration = SystemUnderTest();

            // Area is named correctly
            Assert.AreEqual(area, areaRegistration.AreaName);
        }

        /// <summary>
        /// Test Example Area Registration returns a valid route when a http request is made to it.
        /// </summary>
        [TestMethod]
        public void ExampleAreaRegistration_HttpRequestMadeToArea_ReturnsValidRoute()
        {
            var area = "Example";
            var areaRegistration = SystemUnderTest();

            // Setup an area registration context
            var routes = new RouteCollection();
            var areaRegistrationContext = new AreaRegistrationContext(areaRegistration.AreaName, routes);

            // Register the area under test
            areaRegistration.RegisterArea(areaRegistrationContext);

            // Mock http context to imitate a request to the area
            var context = new Mock<HttpContextBase>();
            context.Setup(c => c.Request.AppRelativeCurrentExecutionFilePath).Returns(string.Format("~/{0}", area));

            // Get route data based on the http context request
            var routeData = routes.GetRouteData(context.Object);

            Assert.IsNotNull(routeData, "Route not found.");
            Assert.AreEqual(area, routeData.DataTokens["area"]);
            Assert.AreEqual("Default", routeData.Values["controller"]);
            Assert.AreEqual("Index", routeData.Values["action"]);
        }
    }
}
