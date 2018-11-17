using System.Web.Mvc;
using System.Web.Routing;
using Employment.Web.Mvc.Infrastructure.Controllers;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Models;
using Employment.Web.Mvc.Infrastructure.Properties;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that validates the anti-duplicate submit token.
    /// </summary>
    /// <remarks>
    /// When a duplicate submit is detected, Controllers inheriting from <see cref="InfrastructureController" /> will have the <see cref="InfrastructureController.IsDuplicateSubmit" /> property set to <c>true</c>.
    /// </remarks>
    public class ValidateAntiDuplicateSubmitTokenAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Whether to update the ModelState with an error on detection of a duplicate submit (default is false).
        /// </summary>
        public bool UpdateModelState { get; set; }

        /// <summary>
        /// The name of the input in the form.
        /// </summary>
        public static readonly string FormFieldName = "__AntiDuplicateSubmitToken";

        /// <summary>
        /// User service.
        /// </summary>
        public IUserService UserService
        {
            get
            {
                var containerProvider = DependencyResolver.Current as IContainerProvider;

                return (containerProvider != null) ? containerProvider.GetService<IUserService>() : null;
            }
        }
        
        /// <summary>
        /// Validate the anti-duplicate submit token. If invalid, will add an error to the <see cref="ModelState" />.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            // Check token on a non-Ajax POST if the model state is valid
            if (filterContext.HttpContext.Request.HttpMethod == "POST" && !filterContext.HttpContext.Request.IsAjaxRequest() && filterContext.Controller.ViewData.ModelState.IsValid)
            {
                var submittedToken = filterContext.HttpContext.Request.Form[FormFieldName];
                var storedToken = filterContext.Controller.TempData[GetKey(filterContext.RouteData)] as string;

                // Valid if there is a submitted token and it matches the stored token in Session
                var valid = !string.IsNullOrEmpty(submittedToken) && storedToken == submittedToken;

                if (!valid)
                {
                    if (filterContext.Controller is InfrastructureController)
                    {
                        // Indicate on Controller that this is a duplicate submit
                        ((InfrastructureController)filterContext.Controller).IsDuplicateSubmit = true;
                    }

                    if (UpdateModelState)
                    {
                        // Invalid so add an error to ModelState
                        filterContext.Controller.ViewData.ModelState.AddModelError(string.Empty, DataAnnotationsResources.ValidateAntiDuplicateSubmitTokenAttribute_Invalid);
                    }
                }
            }
        }

        /// <summary>
        /// Get key for accessing token in TempData based on route.
        /// </summary>
        /// <param name="routeData">The route data.</param>
        /// <returns>The anti-duplicate submit token based on route.</returns>
        public static string GetKey(RouteData routeData)
        {
            return new KeyModel("AntiDuplicateSubmitToken") { Namespace = "Employment.Web.Mvc.Infrastructure.DataAnnotations.ValidateAntiDuplicateSubmitTokenAttribute" }
                .Add(routeData.GetArea())
                .Add(routeData.GetController())
                .Add(routeData.GetAction()).GetKey();
        }
    }
}
