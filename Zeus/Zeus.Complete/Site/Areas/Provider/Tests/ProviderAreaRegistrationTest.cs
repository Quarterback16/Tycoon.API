using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Employment.Web.Mvc.Area.Provider.Tests
{
    /// <summary>
    /// Unit tests for <see cref="ProviderAreaRegistration" />.
    /// </summary>
    [TestClass]
    public class ProviderAreaRegistrationTest
    {
        private ProviderAreaRegistration SystemUnderTest()
        {
            return new ProviderAreaRegistration();
        }

        /// <summary>
        /// Test Provider Area Registration is named correctly.
        /// </summary>
        [TestMethod]
        public void ProviderAreaRegistration_Valid_AreaNameIsProvider()
        {
            var area = "Provider";
            var areaRegistration = SystemUnderTest();

            // Area is named correctly
            Assert.AreEqual(area, areaRegistration.AreaName);
        }

        /// <summary>
        /// Test Provider Area Registration returns a valid route when a http request is made to it.
        /// </summary>
        [TestMethod]
        public void ProviderAreaRegistration_HttpRequestMadeToArea_ReturnsValidRoute()
        {
            var area = "Provider";
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
