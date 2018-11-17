using System.Web;
using System.Web.Mvc;

namespace Employment.Web.Mvc.Infrastructure.Http
{
    /// <summary>
    /// Defines a HttpHandler for internal use.
    /// </summary>
    internal class InternalHttpHandler : MvcHttpHandler
    {
        /// <summary>
        /// Process request for a <see cref="HttpContextBase" />.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        internal new void ProcessRequest(HttpContextBase httpContext)
        {
            base.ProcessRequest(httpContext);
        }
    }
}
