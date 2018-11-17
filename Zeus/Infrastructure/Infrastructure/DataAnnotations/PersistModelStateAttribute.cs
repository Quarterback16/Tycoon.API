using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Registrations;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to store ModelState in TempData on a POST request and restore them from TempData to ModelState on a GET request.
    /// This effectively persists the ModelState errors after a RedirectToAction is performed so they are still displayed.
    /// </summary>
    /// <remarks>
    /// Automatically applied to all Controller actions by Infrastructure via <see cref="FilterProviderRegistration" /> so there is no need to explicitly use it.
    /// </remarks>
    public class PersistModelStateAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// The key for accessing the ModelState in TempData.
        /// </summary>
        public static readonly string TempDataKey = "ModelState";

        /// <summary>
        /// Persist model states with errors only.
        /// </summary>
        public bool ErrorsOnly { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersistModelStateAttribute" /> class.
        /// </summary>
        public PersistModelStateAttribute() : this(false) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersistModelStateAttribute" /> class.
        /// </summary>
        /// <param name="errorsOnly">Persist model states with errors only.</param>
        public PersistModelStateAttribute(bool errorsOnly)
        {
            ErrorsOnly = errorsOnly;
        }

        /// <summary>
        /// Restore ModelState from TempData on a GET request.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (filterContext.HttpContext.Request.HttpMethod == "GET" && filterContext.Controller.TempData.ContainsKey(TempDataKey))
            {
                filterContext.Controller.ViewData.ModelState.Merge((ModelStateDictionary)filterContext.Controller.TempData[TempDataKey]);
            }
        }

        /// <summary>
        /// Store ModelState in TempData on a POST request.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            // Persist on both GET and POST requests (will be removed after it's displayed)
            var modelState = new ModelStateDictionary();

            // Only keep states with errors
            foreach (var ms in filterContext.Controller.ViewData.ModelState)
            {
                var hasErrors = (ms.Value != null && ms.Value.Errors != null && ms.Value.Errors.Any());

                if (!ErrorsOnly || hasErrors)
                {
                    modelState.Add(ms);
                }
            }

            filterContext.Controller.TempData[TempDataKey] = modelState;
        }
    }
}
