using System.Diagnostics.CodeAnalysis;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Infrastructure.Ioc.Simple
{
    /// <summary>
    /// Bootstrapper that automates Application configuration using Unity.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SimpleInjectorBootstrapper : Bootstrapper.Bootstrapper
    {
        /// <summary>
        /// Creates the Unity container provider.
        /// </summary>
        public override IContainerProvider CreateContainer()
        {
            var container = new SimpleInjectorContainerProvider();
            
            return container.RegisterByConfiguration();
        }
    }
}
