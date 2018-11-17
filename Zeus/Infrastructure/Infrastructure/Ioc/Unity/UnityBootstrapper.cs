using System;
using System.Diagnostics.CodeAnalysis;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Wrappers;
using Microsoft.Practices.Unity.Configuration;

namespace Employment.Web.Mvc.Infrastructure.Ioc.Unity
{
    /// <summary>
    /// Bootstrapper that automates Application configuration using Unity.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class UnityBootstrapper : Bootstrapper.Bootstrapper
    {
        /// <summary>
        /// Creates the Unity container provider.
        /// </summary>
        public override IContainerProvider CreateContainer()
        {
            var container = new UnityContainerProvider();

            var configuration = ConfigurationManagerWrapper.Current.GetSection<UnityConfigurationSection>("unity");

            if (configuration == null)
            {
                throw new InvalidOperationException("Could not find Unity configuration section in Web.config");
            }

            return container.RegisterByConfiguration();
        }
    }
}
