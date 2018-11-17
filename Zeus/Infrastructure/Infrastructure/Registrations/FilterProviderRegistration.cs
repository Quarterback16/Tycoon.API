using System;
using System.Linq;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.FilterProviders;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.Registrations
{
    /// <summary>
    /// Represents a registration that is used to register filter providers.
    /// </summary>
    [Order(2)]
    public class FilterProviderRegistration : IRegistration
    {
        /// <summary>
        /// Register filter providers.
        /// </summary>
        public void Register()
        {
            // Prepare conditional filters
            var conditions = new Func<ControllerContext, ActionDescriptor, object>[]
            {
                // Reason to comment out: Token generation will be modified to generate token based on session and not on request.
                // Apply [ValidateAntiForgeryTokenAttribute] to all actions with [HttpPost] that don't already have [ValidateAntiForgeryTokenAttribute]
                //(c, a) => a.GetCustomAttributes(true).OfType<HttpPostAttribute>().FirstOrDefault() != null && a.GetCustomAttributes(true).OfType<ValidateAntiForgeryTokenAttribute>().FirstOrDefault() == null ? new ValidateAntiForgeryTokenAttribute() : null,

                
                // Apply [ValidateAntiDuplicateSubmitTokenAttribute] to all actions with [HttpPost] that don't already have [ValidateAntiDuplicateSubmitTokenAttribute]
                (c, a) => a.GetCustomAttributes(true).OfType<HttpPostAttribute>().FirstOrDefault() != null && a.GetCustomAttributes(true).OfType<ValidateAntiDuplicateSubmitTokenAttribute>().FirstOrDefault() == null ? new ValidateAntiDuplicateSubmitTokenAttribute() : null,
                
                // Apply [SecurityAttribute] to all actions that don't already have the attribute but only if their controller also doesn't have the attribute (this will ensure the OnAuthorization will be run for all actions)
                (c, a) =>
                    {
                        var controllerSecurity = a.ControllerDescriptor.GetCustomAttributes(true).OfType<SecurityAttribute>().FirstOrDefault();
                        var actionSecurity = a.GetCustomAttributes(true).OfType<SecurityAttribute>().FirstOrDefault();

                        return controllerSecurity == null && actionSecurity == null ? new SecurityAttribute() : null;
                    },

                // Apply [PersistModelStateAttribute] to all actions so ModelState can be persisted after a redirect (unless it already has the attribute).
                (c, a) => a.GetCustomAttributes(true).OfType<PersistModelStateAttribute>().FirstOrDefault() == null ? new PersistModelStateAttribute(true) : null,

            };

            var conditionalProvider = new ConditionalFilterProvider(conditions);

            // Add the conditional filter provider
            System.Web.Mvc.FilterProviders.Providers.Add(conditionalProvider);
        }
    }
}
