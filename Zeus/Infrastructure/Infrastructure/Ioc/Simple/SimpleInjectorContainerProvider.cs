using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Async;
using System.Web.Routing;
using System.Web.SessionState;
using Employment.Web.Mvc.Infrastructure.Configuration;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.Wrappers;
using SimpleInjector;

namespace Employment.Web.Mvc.Infrastructure.Ioc.Simple
{
    public class SimpleInjectorViewPageActivator : IViewPageActivator
    {
        private readonly SimpleInjector.Container container;
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleInjectorViewPageActivator"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public SimpleInjectorViewPageActivator(SimpleInjector.Container container)
        {
            this.container = container;
        }

        /// <summary>
        /// Creates the specified controller context.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public object Create(ControllerContext controllerContext, Type type)
        {
            if (type == null)
                return null;

            return container.GetInstance(type);
        }
    }



    /// <summary>
    /// Defines a provider for a SimpleInjector Inversion of Control container.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SimpleInjectorContainerProvider : ContainerProvider
    {
        /// <summary>
        /// Unity Container
        /// </summary>
        private readonly SimpleInjector.Container container;

        internal SimpleInjector.Container InternalContainer {
            get { return container; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleInjectorContainerProvider" /> class.
        /// </summary>
        public SimpleInjectorContainerProvider()
        {
            container = new SimpleInjector.Container();

            container.Register<HttpContextBase>(() => new HttpContextWrapper(HttpContext.Current));

            var controllerFactory = new DefaultControllerFactory();
            container.RegisterSingle<IControllerFactory>(controllerFactory);
            container.RegisterSingle<IControllerActivator>(new SimpleInjectorControllerActivator(container));
            container.RegisterSingle<IActionInvoker>(new ControllerActionInvoker());
            container.RegisterSingle<ITempDataProvider>(new SessionStateTempDataProvider());
            container.RegisterSingle<RouteCollection>(RouteTable.Routes);
            container.RegisterSingle<IAsyncActionInvoker>(new AsyncControllerActionInvoker());
            container.RegisterSingle<IViewPageActivator>(new SimpleInjectorViewPageActivator(container));
            container.RegisterSingle<ModelMetadataProvider>(System.Web.Mvc.ModelMetadataProviders.Current);
        }



        /// <summary>
        /// Register type mappings with container by configuration.
        /// </summary>
        /// <returns>The <see cref="IContainerProvider" /> object that this method was called on.</returns>
        public override IContainerProvider RegisterByConfiguration()
        {
            IoCSection section = ConfigurationManager.GetSection("ioc") as IoCSection; //<IoCSection>
            if (section == null)
            {
                throw new InvalidOperationException("Could not find IoC configuration section in Web.config");
            }

            var registerAllTypes = new Dictionary<Type, List<Type>>();

            foreach (IoCType typ in section.Types)
            {
                Type interfaceType = BuildManagerWrapper.Current.ResolveType(typ.Type);
                Type concreteType = BuildManagerWrapper.Current.ResolveType(typ.MapTo);

                LifetimeType lifetimeType = LifetimeType.Default;
                Lifestyle lifestyleType = Lifestyle.Transient;

                switch (typ.Lifetime)
                {
                    case "singleton":
                        lifetimeType = LifetimeType.Singleton;
                        lifestyleType = Lifestyle.Singleton;
                        break;
                    case "transient":
                        lifetimeType = LifetimeType.Transient;
                        lifestyleType = Lifestyle.Transient;
                        break;
                }

                if (typ.RegisterAll == "true")
                {
                    if (!registerAllTypes.ContainsKey(interfaceType))
                    {
                        registerAllTypes.Add(interfaceType, new List<Type>() {concreteType});
                    }
                    else
                    {
                        registerAllTypes[interfaceType].Add(concreteType);
                    }
                }
                else
                {
                    RegisterType(interfaceType, concreteType, lifetimeType);
                }
            }
            if (registerAllTypes.Count > 0)
            {
                foreach (KeyValuePair<Type, List<Type>> item in registerAllTypes)
                {
                    //if (item.Key == typeof (IController))
                    //{
                    //    foreach (var controller in item.Value)
                    //    {
                    //        RegisterType(controller, controller, LifetimeType.Transient);
                    //    }
                    //}
                    //else
                    //{
                        container.RegisterAll(item.Key, item.Value);
                    //}

                }
            }
            //container.RegisterMvcControllers(BuildManagerWrapper.Current.Assemblies.ToArray());
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
            if (lifetime == LifetimeType.Singleton)
            {
                container.Register(serviceType, implementationType, Lifestyle.Singleton);
            }
            else if (lifetime == LifetimeType.Transient)
            {
                container.Register(serviceType, implementationType, Lifestyle.Transient);
            }
            else
            {
                throw new Exception("Unknown lifetime type.");
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
            container.RegisterSingle(serviceType, instance);

            return this;
        }

        /// <summary>Resolves a single registered service.</summary>
        /// <typeparam name="TServiceType"><see cref="System.Type" /> of the requested service.</typeparam>
        /// <returns>The requested service.</returns>
        public override TServiceType GetService<TServiceType>()
        {
            var serviceType = typeof(TServiceType);

           // if (!typeof(WebViewPage).IsAssignableFrom(serviceType) /*&& (container.GetAllInstances<TServiceType>().Any() || container.Registrations.Any(registration => registration.MappedToType == typeof(TServiceType)))*/)
           // {
            return (TServiceType)container.GetInstance(serviceType);
                //if (instances.Any())
                //{
                //    return instances.FirstOrDefault();
                //}

                //return container.Resolve<TServiceType>();
          //  }

           // return container.GetInstance<TServiceType>();
        }

        /// <summary>Resolves a single registered service.</summary>
        /// <param name="serviceType"><see cref="System.Type" /> of the requested service.</param>
        /// <returns>The requested service.</returns>
        public override object GetService(Type serviceType)
        {
            return container.GetInstance(serviceType);
            //if (!typeof(WebViewPage).IsAssignableFrom(serviceType) 
            //    &&
            //    (container.IsRegistered(serviceType) || container.Registrations.Any(registration => registration.MappedToType == serviceType)))
            //{
            //    return container.Resolve(serviceType);
            //}

            //return container.ResolveAll(serviceType).FirstOrDefault();
        }

        /// <summary>Resolves multiple registered services.</summary>
        /// <typeparam name="TServiceType"><see cref="System.Type" /> of the requested service.</typeparam>
        /// <returns>The requested services.</returns>
        public override IEnumerable<TServiceType> GetServices<TServiceType>()
        {
            return container.GetAllInstances<TServiceType>();
            //var instances = new List<TServiceType>();

            //if (!typeof(WebViewPage).IsAssignableFrom(typeof(TServiceType)) && container.Registrations.Any(registration => string.IsNullOrEmpty(registration.Name) && (registration.RegisteredType == typeof(TServiceType) || registration.MappedToType == typeof(TServiceType))))
            //{
            //    instances.Add(container.Resolve<TServiceType>());
            //}

            //instances.AddRange(container.ResolveAll<TServiceType>());

            //return instances;
        }

        /// <summary>Resolves multiple registered services.</summary>
        /// <param name="serviceType"><see cref="System.Type" /> of the requested services.</param>
        /// <returns>The requested services.</returns>
        public override IEnumerable<object> GetServices(Type serviceType)
        {
            return container.GetAllInstances(serviceType);
            //var instances = new List<object>();

            //if (!typeof(WebViewPage).IsAssignableFrom(serviceType) && container.Registrations.Any(registration => string.IsNullOrEmpty(registration.Name) && (registration.RegisteredType == serviceType || registration.MappedToType == serviceType)))
            //{
            //    instances.Add(container.Resolve(serviceType));
            //}

            //instances.AddRange(container.ResolveAll(serviceType));

            //return instances;
        }

        /// <summary>
        /// Injects the matching dependences.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public override void Inject(object instance)
        {
            if (instance != null)
            {
                container.InjectProperties( instance);
            }
        }

        /// <summary>
        /// Dispose of the Unity Container.
        /// </summary>
        protected override void DisposeContainer()
        {
            //container.Dispose();
        }
    }
}
