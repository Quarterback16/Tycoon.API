using StackExchange.Profiling;
using System.Web;

namespace Employment.Web.Mvc.Infrastructure.Profilers
{
    /// <summary>
    /// MiniPorfiler user profiler based on username.
    /// </summary>
    public class UsernameUserProfiler : IUserProvider
    {
        /// <summary>
        /// Get user by username, falling back to IP address if not found.
        /// </summary>
        /// <param name="request">Request instance.</param>
        /// <returns>Username.</returns>
        public string GetUser(HttpRequest request)
        {
            return request.RequestContext.HttpContext.User != null ? request.RequestContext.HttpContext.User.Identity.Name : string.Empty;
        }
    }
}
