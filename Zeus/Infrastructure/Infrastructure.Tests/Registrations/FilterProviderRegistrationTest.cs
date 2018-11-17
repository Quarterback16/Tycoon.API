using System.Linq;
using Employment.Web.Mvc.Infrastructure.FilterProviders;
using Employment.Web.Mvc.Infrastructure.Registrations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.Registrations
{
    /// <summary>
    /// Unit tests for <see cref="FilterProviderRegistration" />.
    /// </summary>
    [TestClass]
    public class FilterProviderRegistrationTest
    {
        /// <summary>
        /// Test that the <see cref="ConditionalFilterProvider" /> is registered.
        /// </summary>
        [TestMethod]
        public void FilterProviderRegistration_RunRegister_ConditionalFilterProviderIsRegistered()
        {
            // Not registered
            Assert.IsFalse(System.Web.Mvc.FilterProviders.Providers.Any(f => f.GetType() == typeof(ConditionalFilterProvider)));

            var systemUnderTest = new FilterProviderRegistration();

            // Run registration
            systemUnderTest.Register();

            // Is registered
            Assert.IsTrue(System.Web.Mvc.FilterProviders.Providers.Any(f => f.GetType() == typeof(ConditionalFilterProvider)));
        }
    }
}
