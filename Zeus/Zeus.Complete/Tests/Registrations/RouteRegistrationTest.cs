using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Employment.Web.Mvc.Zeus.Registrations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Zeus.Tests.Registrations
{
    /// <summary>
    /// Unit tests for <see cref="RouteRegistration" />.
    /// </summary>
    [TestClass]
    public class RouteRegistrationTest
    {
        /// <summary>
        /// Test that the <see cref="HandleErrorAttribute" /> is registered.
        /// </summary>
        [TestMethod]
        public void RouteRegistration_RunRegister_RouteIsRegistered()
        {
            RouteTable.Routes.Clear();
            
            // Not registered
            Assert.IsFalse(RouteTable.Routes.Any());

            var systemUnderTest = new RouteRegistration();

            // Run registration
            systemUnderTest.Register();

            // Is registered
            Assert.IsTrue(RouteTable.Routes.Any());
        }
    }
}
