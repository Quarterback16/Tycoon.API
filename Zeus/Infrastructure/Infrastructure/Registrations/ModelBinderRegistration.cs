using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.ModelBinders;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using System;

namespace Employment.Web.Mvc.Infrastructure.Registrations
{
    /// <summary>
    /// Represents a registration that is used to register model binders.
    /// </summary>
    [Order(3)]
    public class ModelBinderRegistration : IRegistration
    {
        /// <summary>
        /// Register model binders.
        /// </summary>
        public void Register()
        {
            System.Web.Mvc.ModelBinders.Binders.DefaultBinder = new InfrastructureModelBinder();
            System.Web.Mvc.ModelBinders.Binders[typeof(InheritanceViewModel)] = new InheritanceModelBinder();
        }
    }
}
