using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.ModelBinders;
using Employment.Web.Mvc.Infrastructure.Registrations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.Registrations
{
    /// <summary>
    /// Unit tests for <see cref="ModelBinderRegistration" />.
    /// </summary>
    [TestClass]
    public class ModelBinderRegistrationTest
    {
        /// <summary>
        /// Test that the <see cref="InfrastructureModelBinder" /> is registered.
        /// </summary>
        [TestMethod]
        public void ModelBinderRegistration_RunRegister_InfrastructureModelBinderIsRegistered()
        {
            // Set to default
            System.Web.Mvc.ModelBinders.Binders.DefaultBinder = new DefaultModelBinder();

            // Not registered
            Assert.IsFalse(System.Web.Mvc.ModelBinders.Binders.DefaultBinder.GetType() == typeof(InfrastructureModelBinder));

            var systemUnderTest = new ModelBinderRegistration();

            // Run registration
            systemUnderTest.Register();

            // Is registered
            Assert.IsTrue(System.Web.Mvc.ModelBinders.Binders.DefaultBinder.GetType() == typeof(InfrastructureModelBinder));
        }
    }
}
