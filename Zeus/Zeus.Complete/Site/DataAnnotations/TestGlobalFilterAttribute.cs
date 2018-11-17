using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Employment.Web.Mvc.Zeus.DataAnnotations
{
    /// <summary>
    /// Global Filter Attribute.
    /// </summary>
    public class TestGlobalFilterAttribute: ActionFilterAttribute
    {
        /// <summary>
        /// On Action Executing.
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            //filterContext.RequestContext.HttpContext.Response.Write("Global Filter is being executed.");
        }
    }
}