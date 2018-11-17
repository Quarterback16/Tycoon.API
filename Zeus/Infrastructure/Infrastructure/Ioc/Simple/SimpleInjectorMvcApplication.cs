using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Infrastructure.Ioc.Simple
{
    /// <summary>
    /// Defines a Unity application.
    /// </summary>
    public class SimpleInjectorMvcApplication : MvcApplication
    {
        /// <summary>
        /// Creates the bootstrapper.
        /// </summary>
        /// <returns></returns>
        protected override IBootstrapper CreateBootstrapper()
        {
            return new SimpleInjectorBootstrapper();
        }
    }
}
