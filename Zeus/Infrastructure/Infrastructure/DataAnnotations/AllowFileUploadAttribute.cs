using System;
using System.Web.Mvc;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to indicate the Controller or Action allows file uploads.
    /// </summary>
    /// <remarks>
    /// This ensures the HTML form tag will include enctype="multipart/form-data" to support file uploads.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AllowFileUploadAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// The ViewData key for accessing whether file upload is allowed.
        /// </summary>
        public static readonly string ViewDataKey = "Employment.Web.Mvc.Infrastructure.DataAnnotations.AllowFileUpload";

        /// <summary>
        /// Called after the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.Controller.ViewData.Add(ViewDataKey, true);

            base.OnActionExecuted(filterContext);
        }
    }
}
