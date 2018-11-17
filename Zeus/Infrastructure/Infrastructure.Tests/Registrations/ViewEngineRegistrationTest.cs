using System.Linq;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Registrations;
using Employment.Web.Mvc.Infrastructure.ViewEngines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StackExchange.Profiling.Mvc;

namespace Employment.Web.Mvc.Infrastructure.Tests.Registrations
{
    /// <summary>
    /// Unit tests for <see cref=" ViewEngineRegistration" />.
    /// </summary>
    [TestClass]
    public class ViewEngineRegistrationTest
    {
        /// <summary>
        /// Test that the <see cref="CsRazorViewEngine" /> is registered (now registered via <see cref="ProfilingViewEngine" />.
        /// </summary>
        [TestMethod]
        public void ViewEngineRegistration_RunRegister_CsRazorViewEngineIsRegistered()
        {
            // Not registered
            Assert.IsFalse(System.Web.Mvc.ViewEngines.Engines.Any(f => f.GetType() == typeof(ProfilingViewEngine)));
            

            var systemUnderTest = new ViewEngineRegistration();

            // Run registration
            systemUnderTest.Register();

            // Is registered
            Assert.IsTrue(System.Web.Mvc.ViewEngines.Engines.Any(f => f.GetType() == typeof(ProfilingViewEngine)));
        }
    }
}
