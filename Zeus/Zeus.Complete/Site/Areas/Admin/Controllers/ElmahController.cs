using System.Web.Mvc;

using Employment.Web.Mvc.Area.Admin.ActionResults;
using Employment.Web.Mvc.Infrastructure.Controllers;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Area.Admin.Controllers
{
    /// <summary>
    /// Defines the Elmah controller.
    /// </summary>
    [Security(Roles = new[] { "DAD" }, AllowWindowsAuthentication = true)]
    public class ElmahController : InfrastructureController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElmahController" /> class.
        /// </summary>
        /// <param name="userService">User service for retrieving user data.</param>
        /// <param name="adwService">Adw service for retrieving ADW data.</param>
        public ElmahController( IUserService userService, IAdwService adwService) : base( userService, adwService) { }
        
        /// <summary>
        /// Index action.
        /// </summary>
        [Menu("Error Log")]
        public ActionResult Index()
        {
            return new ElmahResult();
        }
    }
}
