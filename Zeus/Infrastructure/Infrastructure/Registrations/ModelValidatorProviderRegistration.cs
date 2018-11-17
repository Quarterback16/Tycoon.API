using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.ModelValidators;

namespace Employment.Web.Mvc.Infrastructure.Registrations
{
    /// <summary>
    /// Represents a registration that is used to register model validator providers.
    /// </summary>
    [Order(5)]
    public class ModelValidatorProviderRegistration : IRegistration
    {
        /// <summary>
        /// Register model validator providers.
        /// </summary>
        public void Register()
        {
            // Register custom adapter for the [Required] attribute to include handling of SelectList, MultiSelectList, IEnumerable<SelectListItem> and bool with [Required]
            DataAnnotationsModelValidatorProvider.RegisterAdapterFactory(typeof(RequiredAttribute), (metadata, controllerContext, attribute) => new InfrastructureRequiredAttributeAdapter(metadata, controllerContext, (RequiredAttribute)attribute));
        }
    }
}
