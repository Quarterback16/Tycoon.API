using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.ModelMetadataProviders;
using Employment.Web.Mvc.Infrastructure.Registrations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.Registrations
{
    /// <summary>
    /// Unit tests for <see cref="ModelMetadataProviderRegistration" />.
    /// </summary>
    [TestClass]
    public class ModelMetadataProviderRegistrationTest
    {
        /// <summary>
        /// Test that the <see cref="InfrastructureModelMetadataProvider" /> is registered.
        /// </summary>
        [TestMethod]
        public void ModelMetadataProviderRegistration_RunRegister_InfrastructureModelMetadataProviderIsRegistered()
        {
            // Set to default
            System.Web.Mvc.ModelMetadataProviders.Current = new DataAnnotationsModelMetadataProvider();

            // Not registered
            Assert.IsFalse(System.Web.Mvc.ModelMetadataProviders.Current.GetType() == typeof(InfrastructureModelMetadataProvider));

            var systemUnderTest = new ModelMetadataProviderRegistration();

            // Run registration
            systemUnderTest.Register();

            // Is registered
            Assert.IsTrue(System.Web.Mvc.ModelMetadataProviders.Current.GetType() == typeof(InfrastructureModelMetadataProvider));
        }
    }
}
