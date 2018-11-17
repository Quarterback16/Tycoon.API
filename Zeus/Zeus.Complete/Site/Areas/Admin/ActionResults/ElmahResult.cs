using System;
using System.Web;
using System.Web.Mvc;
using Elmah;
using Employment.Web.Mvc.Area.Admin.Controllers;

namespace Employment.Web.Mvc.Area.Admin.ActionResults
{
    /// <summary>
    /// Defines an Elmah result for use by the <see cref="ElmahController" /> for viewing data logged by <see cref="Elmah" />.
    /// </summary>
    public class ElmahResult : ActionResult
    {
        /// <summary>
        /// Executes the result.
        /// </summary>
        /// <param name="context">The controller context.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            object resource = null;
            var values = context.RequestContext.RouteData.Values;
            var httpContext = context.HttpContext;
            var request = httpContext.Request;
            var path = request.Path;
            var queryString = request.QueryString;

            if (values.ContainsKey("resource"))
            {
                resource = values["resource"];

                // Ignore resource if it's the same as the action
                if (resource != null && resource.ToString().Equals(values["action"].ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    resource = null;
                }
            }

            if (resource != null)
            {
                // Rewrite path with resource included in a format useable by Elmah ErrorLogPageFactory
                string text = string.Format(".{0}", resource);
                
                httpContext.RewritePath(path.Remove(path.Length - text.Length), text, queryString.ToString());
            }
            else
            {
                if (path.EndsWith("/"))
                {
                    // Rewrite path with trailing slash removed
                    httpContext.RewritePath(path.TrimEnd('/'), null, queryString.ToString());
                }
            }

            IHttpHandler handler = new ErrorLogPageFactory().GetHandler(httpContext.ApplicationInstance.Context, null, null, null);

            handler.ProcessRequest(httpContext.ApplicationInstance.Context);
        }
    }
}