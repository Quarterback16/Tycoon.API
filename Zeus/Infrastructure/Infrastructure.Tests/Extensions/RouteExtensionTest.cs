using System;
using System.Web.DynamicData;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Routing;

namespace Employment.Web.Mvc.Infrastructure.Tests.Extensions
{
    
    
    /// <summary>
    ///This is a test class for RouteExtensionTest and is intended
    ///to contain all RouteExtensionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RouteExtensionTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }


        /// <summary>
        ///A test for GetRouteName
        ///</summary>
        [TestMethod()]
        public void GetRouteNameTest()
        {
            string actual = ((Route) null).GetRouteName();
            Assert.IsNull(actual);
        }

     
        /// <summary>
        ///A test for SetRouteName
        ///</summary>
        [TestMethod()]
        public void SetRouteNameTest()
        {
            IRouteHandler routeHandler = new DynamicDataRouteHandler();
            var route = new Route("www.test.com", new RouteValueDictionary(), routeHandler);
            const string routeName = "TestRoute";
            Route actual = route.SetRouteName(routeName);

            Assert.IsTrue(actual.DataTokens.ContainsKey("RouteName"));
            Assert.IsTrue(actual.DataTokens.ContainsValue("TestRoute"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetRouteNameThrowsArgumentNullException()
        {
            Route route = null;
            route.SetRouteName(null);
        }

        [TestMethod]
        public void GetRouteName()
        {
            Route route = null;
            Assert.IsNull(route.GetRouteName());
        }

        [TestMethod]
        public void GetRouteName1()
        {
            IRouteHandler routeHandler = new DynamicDataRouteHandler();
            Route route = new Route("www.test.com", new RouteValueDictionary(), routeHandler);
            route.SetRouteName("TestRoute");
            var n = route.GetRouteName();
            Assert.AreEqual("TestRoute",n);
        }

        [TestMethod]
        public void GetRouteName2()
        {
            IRouteHandler routeHandler = new DynamicDataRouteHandler();
            var rvd = new RouteValueDictionary();
            //rvd.Add("test", new object());
            Route route = new Route("www.test.com", rvd, routeHandler);
            //route.SetRouteName("TestRoute");

            RouteData rd = new RouteData(route,routeHandler);
            rd.DataTokens.Add("RouteName","TestRoute");
            var n = rd.GetRouteName();
            Assert.AreEqual("TestRoute", n);
        }

        [TestMethod]
        public void GetRouteName3()
        {
            RouteData rd = null;
            Assert.IsNull(rd.GetRouteName());
        }

        [TestMethod]
        public void GetRouteName4()
        {
            RouteValueDictionary rd = null;
            Assert.IsNull(rd.GetRouteName());
        }
    }
}