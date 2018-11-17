using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Infrastructure.Registrations
{
    /// <summary>
    /// Represents a registration that is used to register global filters.
    /// </summary>
    [Order(1)]
    public class GlobalFilterRegistration : IRegistration
    {
        /// <summary>
        /// Register global filters.
        /// </summary>
        public void Register()
        {
            GlobalFilters.Filters.Add(new HttpHeaderAttribute("X-Frame-Options", "SAMEORIGIN"));
            GlobalFilters.Filters.Add(new HandleAllErrorsAttribute());
        }
    }
}
