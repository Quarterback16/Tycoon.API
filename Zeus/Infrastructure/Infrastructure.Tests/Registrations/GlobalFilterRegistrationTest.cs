using System.Linq;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Registrations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.Registrations
{
    /// <summary>
    /// Unit tests for <see cref="GlobalFilterRegistration" />.
    /// </summary>
    [TestClass]
    public class GlobalFilterRegistrationTest
    {
        /// <summary>
        /// Test that the <see cref="HandleErrorAttribute" /> is registered.
        /// </summary>
        [TestMethod]
        public void GlobalFilterRegistration_RunRegister_HandleErrorAttributeIsRegistered()
        {
            // Not registered
            Assert.IsFalse(GlobalFilters.Filters.Any(f => f.GetType() == typeof(HandleErrorAttribute)));

            var systemUnderTest = new GlobalFilterRegistration();

            // Run registration
            systemUnderTest.Register();

            // Is registered
            Assert.IsTrue(GlobalFilters.Filters.Any(f => f.Instance.GetType() == typeof(HandleAllErrorsAttribute)));
        }
    }
}
