using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.ModelMetadataProviders;

namespace Employment.Web.Mvc.Infrastructure.Registrations
{
    /// <summary>
    /// Represents a registration that is used to register model metadata providers.
    /// </summary>
    public class ModelMetadataProviderRegistration : IRegistration
    {
        /// <summary>
        /// Register model metadata providers.
        /// </summary>
        public void Register()
        {
            System.Web.Mvc.ModelMetadataProviders.Current = new InfrastructureModelMetadataProvider();
        }
    }

    /// <summary>
    /// Represents a registration that is used to register model metadata providers.
    /// </summary>
    public class CachedModelMetadataProviderRegistration : IRegistration
    {
        /// <summary>
        /// Register model metadata providers.
        /// </summary>
        public void Register()
        {
            System.Web.Mvc.ModelMetadataProviders.Current = new InfrastructureCachedModelMetadataProvider();
        }
    }
}
