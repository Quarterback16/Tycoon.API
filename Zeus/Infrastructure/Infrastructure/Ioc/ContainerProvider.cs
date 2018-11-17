using System;
using System.Collections.Generic;
using System.Diagnostics;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.Ioc
{
    /// <summary>
    /// Defines a provider for an Inversion of Control container.
    /// </summary>
    public abstract class ContainerProvider : IContainerProvider
    {
        /// <summary>
        /// Register type mappings with container by configuration.
        /// </summary>
        /// <returns>The <see cref="IContainerProvider" /> object that this method was called on.</returns>
        public abstract IContainerProvider RegisterByConfiguration();

        /// <summary>
        /// Register the type mapping with the container, with the given <see cref="LifetimeType" />.
        /// </summary>
        /// <typeparam name="TServiceType"><see cref="System.Type" /> of the service that will be requested.</typeparam>
        /// <typeparam name="TImplementationType"><see cref="System.Type" /> of the implementation that will be returned.</typeparam>
        /// <returns>The <see cref="IContainerProvider" /> object that this method was called on.</returns>
        public abstract IContainerProvider RegisterType<TServiceType, TImplementationType>() where TImplementationType : TServiceType;

        /// <summary>
        /// Register the type mapping with the container, with the given <see cref="LifetimeType" />.
        /// </summary>
        /// <typeparam name="TServiceType"><see cref="System.Type" /> of the service that will be requested.</typeparam>
        /// <typeparam name="TImplementationType"><see cref="System.Type" /> of the implementation that will be returned.</typeparam>
        /// <param name="lifetime">The <see cref="LifetimeType" /> of the service.</param>
        /// <returns>The <see cref="IContainerProvider" /> object that this method was called on.</returns>
        public abstract IContainerProvider RegisterType<TServiceType, TImplementationType>(LifetimeType lifetime) where TImplementationType : TServiceType;

        /// <summary>
        /// Register the type mapping with the container, with the default <see cref="LifetimeType" />.
        /// </summary>
        /// <param name="serviceType"><see cref="System.Type" /> of the service that will be requested.</param>
        /// <param name="implementationType"><see cref="System.Type" /> of the implementation that will be returned.</param>
        /// <returns>The <see cref="IContainerProvider" /> object that this method was called on.</returns>
        public abstract IContainerProvider RegisterType(Type serviceType, Type implementationType);

        /// <summary>
        /// Register the type mapping with the container, with the given <see cref="LifetimeType" />.
        /// </summary>
        /// <param name="serviceType"><see cref="System.Type" /> of the service that will be requested.</param>
        /// <param name="implementationType"><see cref="System.Type" /> of the implementation that will be returned.</param>
        /// <param name="lifetime">The <see cref="LifetimeType" /> of the service.</param>
        /// <returns>The <see cref="IContainerProvider" /> object that this method was called on.</returns>
        public abstract IContainerProvider RegisterType(Type serviceType, Type implementationType, LifetimeType lifetime);

        /// <summary>
        /// Register an instance with the container.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Instance registration is much like registering a type with <see cref="LifetimeType.Singleton" />,
        /// except that instead of the container creating the instance the first time it is requested, the
        /// instance is already created outside the container and that instance is added to the container.
        /// </para>
        /// </remarks>
        /// <typeparam name="TServiceType"><see cref="System.Type" /> of the service that will be requested.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns>The <see cref="IContainerProvider" /> object that this method was called on.</returns>
        public abstract IContainerProvider RegisterInstance<TServiceType>(object instance);

        /// <summary>
        /// Register an instance with the container.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Instance registration is much like registering a type with <see cref="LifetimeType.Singleton" />,
        /// except that instead of the container creating the instance the first time it is requested, the
        /// instance is already created outside the container and that instance is added to the container.
        /// </para>
        /// </remarks>
        /// <param name="serviceType"><see cref="System.Type" /> of the service that will be requested.</param>
        /// <param name="instance">The instance.</param>
        /// <returns>The <see cref="IContainerProvider" /> object that this method was called on.</returns>
        public abstract IContainerProvider RegisterInstance(Type serviceType, object instance);

        /// <summary>Resolves a single registered service.</summary>
        /// <typeparam name="TServiceType"><see cref="System.Type" /> of the requested service.</typeparam>
        /// <returns>The requested service.</returns>
        public abstract TServiceType GetService<TServiceType>();

        /// <summary>Resolves a single registered service.</summary>
        /// <param name="serviceType"><see cref="System.Type" /> of the requested service.</param>
        /// <returns>The requested service.</returns>
        public abstract object GetService(Type serviceType);

        /// <summary>Resolves multiple registered services.</summary>
        /// <typeparam name="TServiceType"><see cref="System.Type" /> of the requested service.</typeparam>
        /// <returns>The requested services.</returns>
        public abstract IEnumerable<TServiceType> GetServices<TServiceType>();

        /// <summary>Resolves multiple registered services.</summary>
        /// <param name="serviceType"><see cref="System.Type" /> of the requested services.</param>
        /// <returns>The requested services.</returns>
        public abstract IEnumerable<object> GetServices(Type serviceType);

        /// <summary>
        /// Injects the matching dependences.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public abstract void Inject(object instance);

        /// <summary>
        /// Releases unmanaged resources and cleanup operations before the
        /// <see cref="IDisposable"/> is reclaimed by garbage collection.
        /// </summary>
        [DebuggerStepThrough]
        ~ContainerProvider()
        {
            Dispose(false);
        }

        /// <summary>
        /// Dispose the container instance.
        /// </summary>
        /// <remarks>
        /// Disposing the container also disposes any child containers,
        /// and disposes any instances whose lifetimes are managed by the container.
        /// </remarks>
        [DebuggerStepThrough]
        protected virtual void DisposeContainer()
        {

        }

        private bool disposed;

        private void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                DisposeContainer();
            }

            disposed = true;
        }

        /// <summary>
        /// Dispose this instance.
        /// </summary>
        [DebuggerStepThrough]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
