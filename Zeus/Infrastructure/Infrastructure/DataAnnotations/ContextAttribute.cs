using System;
using Employment.Web.Mvc.Infrastructure.Types;
using System.Web.Mvc;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /*
    /// <summary>
    /// Represents an attribute that is used to indicate the context type to use on the current controller or action.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ContextAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// The context type.
        /// </summary>
        public ContextType ContextType { get; set; }

        /// <summary>
        /// The key for accessing the context type in ViewData.
        /// </summary>
        public static readonly string ViewDataKey = "Context.ContextType";

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextAttribute" /> class.
        /// </summary>
        /// <param name="contextType">The context type.</param>
        public ContextAttribute(ContextType contextType)
        {
            ContextType = contextType;
        }

        /// <summary>
        /// Called after the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.Controller.ViewData.Add(ViewDataKey, ContextType);

            base.OnActionExecuted(filterContext);
        }
    }*/
}
