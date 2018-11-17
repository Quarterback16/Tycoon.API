using System;
using System.Web.Mvc;
using System.Reflection;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that determines the selection of an action.
    /// </summary>
    public class ButtonHandlerAttribute : ActionNameSelectorAttribute
    {
        /// <summary>Determines whether the action name is valid in the specified controller context.</summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="methodInfo">Information about the action method.</param>
        /// <returns>True if the action name is valid in the specified controller context; otherwise, false.</returns>
        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            // On POST
            if (controllerContext.RequestContext.HttpContext.Request.HttpMethod == "POST")
            {
                // Get the submitted button action name
                var buttonActionName = controllerContext.RequestContext.HttpContext.Request[ButtonAttribute.SubmitTypeName];
                
                // Valid if button action name is equal to the primary action or button action name is matched with its handler for the primary action
                if (buttonActionName != null && methodInfo.Name.Equals(buttonActionName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
