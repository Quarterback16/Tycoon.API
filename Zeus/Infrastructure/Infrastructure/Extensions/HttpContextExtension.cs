using System.Collections.Generic;
using System.Web;
using Employment.Web.Mvc.Infrastructure.Http;

namespace Employment.Web.Mvc.Infrastructure.Extensions
{
    /// <summary>
    /// Extensions for <see cref="HttpContext"/>.
    /// </summary>
    public static class HttpContextExtension
    {
        /// <summary>
        /// Handle error status codes.
        /// </summary>
        /// <remarks>
        /// This is limited to handling the following:
        ///  - 401 Unauthorized
        ///  - 403 Forbidden
        ///  - 404 Not Found
        ///  - 500 Server Error
        /// </remarks>
        /// <param name="httpContext">The HTTP context instance.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <returns><c>true</c> if the error status code was handled; otherwise, <c>false</c>.</returns>
        public static bool HandleErrorStatusCode(this HttpContext httpContext, int statusCode)
        {
            return new HttpContextWrapper(httpContext).HandleErrorStatusCode(statusCode, null);
        }

        /// <summary>
        /// Handle error status codes.
        /// </summary>
        /// <remarks>
        /// This is limited to handling the following:
        ///  - 401 Unauthorized
        ///  - 403 Forbidden
        ///  - 404 Not Found
        ///  - 500 Server Error
        /// </remarks>
        /// <param name="httpContext">The HTTP context instance.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <param name="statusDescription">The HTTP status description.</param>
        /// <returns><c>true</c> if the error status code was handled; otherwise, <c>false</c>.</returns>
        public static bool HandleErrorStatusCode(this HttpContext httpContext, int statusCode, string statusDescription)
        {
            return new HttpContextWrapper(httpContext).HandleErrorStatusCode(statusCode, statusDescription);
        }

        /// <summary>
        /// Handle error status codes.
        /// </summary>
        /// <remarks>
        /// This is limited to handling the following:
        ///  - 401 Unauthorized
        ///  - 403 Forbidden
        ///  - 404 Not Found
        ///  - 500 Server Error
        /// </remarks>
        /// <param name="httpContext">The HTTP context instance.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <returns><c>true</c> if the error status code was handled; otherwise, <c>false</c>.</returns>
        public static bool HandleErrorStatusCode(this HttpContextBase httpContext, int statusCode)
        {
            return httpContext.HandleErrorStatusCode(statusCode, null);
        }

        /// <summary>
        /// Handle error status codes.
        /// </summary>
        /// <remarks>
        /// This is limited to handling the following:
        ///  - 401 Unauthorized
        ///  - 403 Forbidden
        ///  - 404 Not Found
        ///  - 500 Server Error
        /// </remarks>
        /// <param name="httpContext">The HTTP context instance.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <param name="statusDescription">The HTTP status description.</param>
        /// <returns><c>true</c> if the error status code was handled; otherwise, <c>false</c>.</returns>
        public static bool HandleErrorStatusCode(this HttpContextBase httpContext, int statusCode, string statusDescription)
        {
            var statusCodesToHandle = new List<int> { 401, 403, 404, 500 };

            // Only handle specific status code errors and only when custom error pages are enabled
            if (httpContext.IsCustomErrorEnabled && statusCodesToHandle.Contains(statusCode))
            {
                // Clear response, set error code and skip IIS custom error page
                httpContext.Response.Clear();
                httpContext.Response.StatusCode = statusCode;
                httpContext.Response.TrySkipIisCustomErrors = true;

                // Add status description which will be used in the ErrorController errors pages
                if (!string.IsNullOrEmpty(statusDescription))
                {
                    httpContext.Items.Add("ErrorStatusCodeDescription", statusDescription);
                }

                // Clear server
                httpContext.Server.ClearError();

                var errorPath = string.Format("~/Error/Http{0}", statusCode);

                if (HttpRuntime.UsingIntegratedPipeline)
                {
                    // Transfer to error path
                    httpContext.Server.TransferRequest(errorPath, true);
                }
                else
                {
                    // Get original path
                    string path = httpContext.Request.Path;

                    // Rewrite to error path
                    httpContext.RewritePath(errorPath, false);

                    // Process error path request for response
                    new InternalHttpHandler().ProcessRequest(httpContext);

                    // Rewrite path to original path so the user appears to be on the url they originally requested
                    httpContext.RewritePath(path, false);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Handle error status codes.
        /// </summary>
        /// <remarks>
        /// This is limited to handling the following:
        ///  - 401 Unauthorized
        ///  - 403 Forbidden
        ///  - 404 Not Found
        ///  - 500 Server Error
        /// </remarks>
        /// <param name="httpContext">The HTTP context instance.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <returns><c>true</c> if the error status code was handled; otherwise, <c>false</c>.</returns>
        public static bool HandleErrorStatusCode(this HttpContextWrapper httpContext, int statusCode)
        {
            return httpContext.HandleErrorStatusCode(statusCode, null);
        }

        /// <summary>
        /// Handle error status codes.
        /// </summary>
        /// <remarks>
        /// This is limited to handling the following:
        ///  - 401 Unauthorized
        ///  - 403 Forbidden
        ///  - 404 Not Found
        ///  - 500 Server Error
        /// </remarks>
        /// <param name="httpContext">The HTTP context instance.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <param name="statusDescription">The HTTP status description.</param>
        /// <returns><c>true</c> if the error status code was handled; otherwise, <c>false</c>.</returns>
        public static bool HandleErrorStatusCode(this HttpContextWrapper httpContext, int statusCode, string statusDescription)
        {
            var statusCodesToHandle = new List<int> { 401, 403, 404, 500 };

            // Only handle specific status code errors and only when custom error pages are enabled
            if (httpContext.IsCustomErrorEnabled && statusCodesToHandle.Contains(statusCode))
            {
                // Clear response, set error code and skip IIS custom error page
                httpContext.Response.Clear();
                httpContext.Response.StatusCode = statusCode;
                httpContext.Response.TrySkipIisCustomErrors = true;

                // Add status description which will be used in the ErrorController errors pages
                if (!string.IsNullOrEmpty(statusDescription))
                {
                    httpContext.Items.Add("ErrorStatusCodeDescription", statusDescription);
                }
                
                // Clear server
                httpContext.Server.ClearError();

                var errorPath = string.Format("~/Error/Http{0}", statusCode);

                if (HttpRuntime.UsingIntegratedPipeline)
                {
                    // Transfer to error path
                    httpContext.Server.TransferRequest(errorPath, true);
                }
                else
                {
                    // Get original path
                    string path = httpContext.Request.Path;

                    // Rewrite to error path
                    httpContext.RewritePath(errorPath, false);

                    // Process error path request for response
                    new InternalHttpHandler().ProcessRequest(httpContext);

                    // Rewrite path to original path so the user appears to be on the url they originally requested
                    httpContext.RewritePath(path, false);
                }

                return true;
            }

            return false;
        }
    }
}
