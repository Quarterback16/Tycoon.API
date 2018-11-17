using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Employment.Web.Mvc.Infrastructure.Ioc.Simple
{
    /// <summary>
    /// Simple injector controller activation.
    /// </summary>
    public class SimpleInjectorControllerActivator : IControllerActivator
    {
        private readonly SimpleInjector.Container container;
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleInjectorControllerActivator"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public SimpleInjectorControllerActivator(SimpleInjector.Container container)
        {
            this.container = container;
        }

        /// <summary>
        /// Creates the specified controller.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        /// <param name="controllerType">Type of the controller.</param>
        /// <returns></returns>
        public IController Create(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
                return null;

            return (IController)container.GetInstance(controllerType);
        }
    }
}