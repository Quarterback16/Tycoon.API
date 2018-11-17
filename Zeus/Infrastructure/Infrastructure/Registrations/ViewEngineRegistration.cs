using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.ViewEngines;
#if DEBUG
using StackExchange.Profiling.Mvc;
#endif

namespace Employment.Web.Mvc.Infrastructure.Registrations
{
    /// <summary>
    /// Represents a registration that is used to register view engines.
    /// </summary>
    [Order(4)]
    public class ViewEngineRegistration : IRegistration
    {
        /// <summary>
        /// Register view engines.
        /// </summary>
        public void Register()
        {
            // Clear engines and include only Cs Razor for more direct lookups (will only look for cshtml razor views)
            System.Web.Mvc.ViewEngines.Engines.Clear();
#if DEBUG
            System.Web.Mvc.ViewEngines.Engines.Add(new ProfilingViewEngine(new CsRazorViewEngine()));
#else
            System.Web.Mvc.ViewEngines.Engines.Add(new CsRazorViewEngine());
#endif
        }
    }
}
