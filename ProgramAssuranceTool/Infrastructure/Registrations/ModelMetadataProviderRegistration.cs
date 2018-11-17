using System.Web.Mvc;
using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Infrastructure.DataAnnotations;
using ProgramAssuranceTool.Infrastructure.Interfaces;

namespace ProgramAssuranceTool.Infrastructure.Registrations
{
    /// <summary>
    /// Represents a registration that is used to register model metadata providers.
    /// </summary>
    public class ModelMetadataProviderRegistration : IRegistration
    {
        /// <summary>
        /// Register model metadata providers.
        /// </summary>
        public void Register()
        {
           ModelMetadataProviders.Current = new CustomModelMetadataProvider();
			  DataAnnotationsModelValidatorProvider.RegisterAdapter( typeof(ValidDate), typeof( ValidDateValidator ) );
        }
    }
}
