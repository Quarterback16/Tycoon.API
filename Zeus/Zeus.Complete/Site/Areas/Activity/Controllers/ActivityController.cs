using System.Web.Mvc; 
using Employment.Web.Mvc.Infrastructure.Controllers;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.ViewModels;

namespace Employment.Web.Mvc.Area.Activity.Controllers
{
	/// <summary>
	/// Defines the ActivityController controller.
	/// </summary>
	[Security(AllowAny = true)]
    public class ActivityController : InfrastructureController
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="ActivityController" /> class.
        /// </summary> 
        /// <param name="userService">User service for retrieving user data.</param>
        /// <param name="adwService">Adw service for retrieving ADW data.</param>
        public ActivityController(IUserService userService, IAdwService adwService) : base(userService, adwService) { }

        /// <summary>
        /// Index action.
        /// </summary>
		[Menu("Activity")]
        public ActionResult Index()
        {
			var model = new ContentViewModel()
				.AddTitle("Hello World!")
				.AddParagraph("Welcome to the Activity area.");

            return View(model);
        }
    }
}
