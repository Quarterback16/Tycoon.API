using System;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Models;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to indicate the previous action should be remembered.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RememberPreviousActionAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// The key for accessing the history type in ViewData.
        /// </summary>
        public static readonly string SessionKey = "RememberedPreviousActionUri";

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
        /// Store referring Uri in the users session if its route is different to current action. Used for returning the user to the previous action they were on when exiting a workflow.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var routeData = filterContext.HttpContext.Request.UrlReferrer.ToRouteData();

            if (routeData != null)
            {
                // Referrer route details
                var referrerArea = routeData.GetArea();
                var referrerController = routeData.GetController();
                var referrerAction = routeData.GetAction();

                // Current route details
                var currentArea = filterContext.RouteData.GetArea();
                var currentController = filterContext.RouteData.GetController();
                var currentAction = filterContext.RouteData.GetAction();

                // Update the stored UrlReferrer if any of the action, controller or area route values are different
                if (string.Compare(referrerAction, currentAction, StringComparison.InvariantCultureIgnoreCase) != 0
                    || string.Compare(referrerController, currentController, StringComparison.InvariantCultureIgnoreCase) != 0
                    || string.Compare(referrerArea, currentArea, StringComparison.InvariantCultureIgnoreCase) != 0)
                {
                    // Update key with current action details
                    var keyModel = new KeyModel(SessionKey).Add(currentArea).Add(currentController).Add(currentAction);

                    UserService.Session.Set(keyModel, filterContext.HttpContext.Request.UrlReferrer);
                }
            }
        }
    }
}
