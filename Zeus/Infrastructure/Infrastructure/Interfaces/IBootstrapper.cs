using System;

namespace Employment.Web.Mvc.Infrastructure.Interfaces
{
    /// <summary>
    /// Defines the methods and properties that are required for a Bootstrapper.
    /// </summary>
    public interface IBootstrapper : IDisposable
    {
        /// <summary>
        /// Creates the container provider.
        /// </summary>
        /// <returns>The container provider.</returns>
        IContainerProvider CreateContainer();

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>The container.</value>
        IContainerProvider Container { get; }

        /// <summary>
        /// Bootstrapper start process to be called in Application_Start().
        /// </summary>
        void Start();

        /// <summary>
        /// Bootstrapper end process to be called in Application_End().
        /// </summary>
        void End();
    }
}
