using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Types;
using Microsoft.Practices.Unity;
using Employment.Web.Mvc.Infrastructure.Wrappers;
using Microsoft.Practices.Unity.Configuration;

namespace Employment.Web.Mvc.Infrastructure.Ioc.Unity
{
    /// <summary>
    /// Defines a provider for a Unity Inversion of Control container.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class UnityContainerProvider : ContainerProvider
    {
        /// <summary>
        /// Unity Container
        /// </summary>
        private readonly IUnityContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityContainerProvider" /> class.
        /// </summary>
        public UnityContainerProvider()
        {
            container = new UnityContainer();

            container.RegisterType<HttpContextBase, HttpContextWrapper>(new InjectionFactory(c => new HttpContextWrapper(HttpContext.Current)));
        }
        
        /// <summary>
        /// Register type mappings with container by configuration.
        /// </summary>
        /// <returns>The <see cref="IContainerProvider" /> object that this method was called on.</returns>
        public override IContainerProvider RegisterByConfiguration()
        {
            var section = ConfigurationManagerWrapper.Current.GetSection<UnityConfigurationSection>("unity");

            foreach (var sectionContainer in section.Containers)
            {
                foreach (var extension in sectionContainer.Extensions)
                {
                    var unityExtension = container.Resolve(BuildManagerWrapper.Current.ResolveType(extension.TypeName)) as UnityContainerExtension;

                    if (unityExtension != null)
                    {
                        container.AddExtension(unityExtension);
                    }
                }

                foreach (var registration in sectionContainer.Registrations)
                {
                    Type interfaceType = BuildManagerWrapper.Current.ResolveType(registration.TypeName);
                    Type concreteType = BuildManagerWrapper.Current.ResolveType(registration.MapToName);

                    LifetimeType lifetimeType = LifetimeType.Default;
                    
                    switch (registration.Lifetime.TypeName)
                    {
                        case "singleton": lifetimeType = LifetimeType.Singleton; break;
                        case "transient": lifetimeType = LifetimeType.Transient; break;
                    }

                    RegisterType(interfaceType, concreteType, lifetimeType);
                }

                foreach (var instance in sectionContainer.Instances)
                {
                    Type interfaceType = BuildManagerWrapper.Current.ResolveType(instance.TypeName);
                    Type typeConverterType = BuildManagerWrapper.Current.ResolveType(instance.TypeConverterTypeName);

                    var converter = container.Resolve(typeConverterType) as TypeConverter;

                    if (converter != null)
                    {
                        RegisterInstance(interfaceType, converter.ConvertFromInvariantString(instance.Value));
                    }
                }
            }
            
            return this;
        }

        /// <summary>
        /// Register the type mapping with the container, with the given <see cref="LifetimeType" />.
        /// </summary>
        /// <typeparam name="TServiceType"><see cref="System.Type" /> of the service that will be requested.</typeparam>
        /// <typeparam name="TImplementationType"><see cref="System.Type" /> of the implementation that will be returned.</typeparam>
        /// <returns>The <see cref="IContainerProvider" /> object that this method was called on.</returns>
        public override IContainerProvider RegisterType<TServiceType, TImplementationType>()
        {
            return RegisterType(typeof(TServiceType), typeof(TImplementationType));
        }

        /// <summary>
        /// Register the type mapping with the container, with the given <see cref="LifetimeType" />.
        /// </summary>
        /// <typeparam name="TServiceType"><see cref="System.Type" /> of the service that will be requested.</typeparam>
        /// <typeparam name="TImplementationType"><see cref="System.Type" /> of the implementation that will be returned.</typeparam>
        /// <param name="lifetime">The <see cref="LifetimeType" /> of the service.</param>
        /// <returns>The <see cref="IContainerProvider" /> object that this method was called on.</returns>
        public override IContainerProvider RegisterType<TServiceType, TImplementationType>(LifetimeType lifetime)
        {
            return RegisterType(typeof(TServiceType), typeof(TImplementationType), lifetime);
        }

        /// <summary>
        /// Register the type mapping with the container, with the default <see cref="LifetimeType" />.
        /// </summary>
        /// <param name="serviceType"><see cref="System.Type" /> of the service that will be requested.</param>
        /// <param name="implementationType"><see cref="System.Type" /> of the implementation that will be returned.</param>
        /// <returns>The <see cref="IContainerProvider" /> object that this method was called on.</returns>
        public override IContainerProvider RegisterType(Type serviceType, Type implementationType)
        {
            return RegisterType(serviceType, implementationType, LifetimeType.Transient);
        }

        /// <summary>
        /// Register the type mapping with the container, with the given <see cref="LifetimeType" />.
        /// </summary>
        /// <param name="serviceType"><see cref="System.Type" /> of the service that will be requested.</param>
        /// <param name="implementationType"><see cref="System.Type" /> of the implementation that will be returned.</param>
        /// <param name="lifetime">The <see cref="LifetimeType" /> of the service.</param>
        /// <returns>The <see cref="IContainerProvider" /> object that this method was called on.</returns>
        /// <exception cref="ArgumentException">Thrown when the implementation type does not inherit from IPerRequestTask and lifetime is PerRequest.</exception>
        public override IContainerProvider RegisterType(Type serviceType, Type implementationType, LifetimeType lifetime)
        {
            LifetimeManager lifetimeManager;
            
            switch (lifetime)
            {
                case LifetimeType.Singleton:
                    lifetimeManager = new ContainerControlledLifetimeManager();
                    break;
                default:
                    lifetimeManager = new TransientLifetimeManager();
                    break;
            }

            if (container.Registrations.Any(registration => registration.RegisteredType == serviceType))
            {
                container.RegisterType(serviceType, implementationType, implementationType.FullName, lifetimeManager);
            }
            else
            {
                container.RegisterType(serviceType, implementationType, lifetimeManager);
            }

            return this;
        }

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
        public override IContainerProvider RegisterInstance<TServiceType>(object instance)
        {
            return RegisterInstance(typeof(TServiceType), instance);
        }

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
        public override IContainerProvider RegisterInstance(Type serviceType, object instance)
        {
            container.RegisterInstance(serviceType, instance);

            return this;
        }

        /// <summary>Resolves a single registered service.</summary>
        /// <typeparam name="TServiceType"><see cref="System.Type" /> of the requested service.</typeparam>
        /// <returns>The requested service.</returns>
        public override TServiceType GetService<TServiceType>()
        {
            var serviceType = typeof(TServiceType);

            if (!typeof(WebViewPage).IsAssignableFrom(serviceType) && (container.IsRegistered<TServiceType>() || container.Registrations.Any(registration => registration.MappedToType == typeof(TServiceType))))
            {
                return container.Resolve<TServiceType>();
            }

            return container.ResolveAll<TServiceType>().FirstOrDefault();
        }

        /// <summary>Resolves a single registered service.</summary>
        /// <param name="serviceType"><see cref="System.Type" /> of the requested service.</param>
        /// <returns>The requested service.</returns>
        public override object GetService(Type serviceType)
        {
            if (!typeof(WebViewPage).IsAssignableFrom(serviceType) 
                &&
                (container.IsRegistered(serviceType) || container.Registrations.Any(registration => registration.MappedToType == serviceType)))
            {
                return container.Resolve(serviceType);
            }

            return container.ResolveAll(serviceType).FirstOrDefault();
        }

        /// <summary>Resolves multiple registered services.</summary>
        /// <typeparam name="TServiceType"><see cref="System.Type" /> of the requested service.</typeparam>
        /// <returns>The requested services.</returns>
        public override IEnumerable<TServiceType> GetServices<TServiceType>()
        {
            var instances = new List<TServiceType>();

            if (!typeof(WebViewPage).IsAssignableFrom(typeof(TServiceType)) && container.Registrations.Any(registration => string.IsNullOrEmpty(registration.Name) && (registration.RegisteredType == typeof(TServiceType) || registration.MappedToType == typeof(TServiceType))))
            {
                instances.Add(container.Resolve<TServiceType>());
            }

            instances.AddRange(container.ResolveAll<TServiceType>());

            return instances;
        }

        /// <summary>Resolves multiple registered services.</summary>
        /// <param name="serviceType"><see cref="System.Type" /> of the requested services.</param>
        /// <returns>The requested services.</returns>
        public override IEnumerable<object> GetServices(Type serviceType)
        {
            var instances = new List<object>();

            if (!typeof(WebViewPage).IsAssignableFrom(serviceType) && container.Registrations.Any(registration => string.IsNullOrEmpty(registration.Name) && (registration.RegisteredType == serviceType || registration.MappedToType == serviceType)))
            {
                instances.Add(container.Resolve(serviceType));
            }

            instances.AddRange(container.ResolveAll(serviceType));

            return instances;
        }

        /// <summary>
        /// Injects the matching dependences.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public override void Inject(object instance)
        {
            if (instance != null)
            {
                container.BuildUp(instance.GetType(), instance);
            }
        }

        /// <summary>
        /// Dispose of the Unity Container.
        /// </summary>
        protected override void DisposeContainer()
        {
            container.Dispose();
        }
    }
}
