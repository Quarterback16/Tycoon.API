using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Ioc.Simple;
using Employment.Web.Mvc.Infrastructure.Ioc.Simple.FilterAttributeFilterProvider;
using SimpleInjector;

namespace Employment.Web.Mvc.Infrastructure.Ioc.Simple.Registrations
{
    /// <summary>
    /// Represents a registration that is used to register filter attribute filter providers.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class FilterAttributeFilterProviderRegistration : IRegistration
    {
        /// <summary>
        /// Register a Unity filter attribute filter provider.
        /// </summary>
        public void Register()
        {
            var container = DependencyResolver.Current as SimpleInjectorContainerProvider;

            // Remove standard FilterAttributeFilterProvider
            System.Web.Mvc.FilterProviders.Providers.Remove(System.Web.Mvc.FilterProviders.Providers.Single(f => f is System.Web.Mvc.FilterAttributeFilterProvider));

            var provider = new SimpleInjectorFilterAttributeFilterProvider(container.InternalContainer);

            // Add the Unity FilterAttributeFilterProvider in its place
            System.Web.Mvc.FilterProviders.Providers.Add(provider);
        }
    }
}
