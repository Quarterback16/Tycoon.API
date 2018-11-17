using System.Web.Mvc; 
using Employment.Web.Mvc.Infrastructure.Controllers;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.ViewModels;

namespace Employment.Web.Mvc.Area.Calendar.Controllers
{
	/// <summary>
	/// Defines the CalendarController controller.
	/// </summary>
	[Security(AllowAny = true)]
    public class CalendarController : InfrastructureController
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="CalendarController" /> class.
        /// </summary> 
        /// <param name="userService">User service for retrieving user data.</param>
        /// <param name="adwService">Adw service for retrieving ADW data.</param>
        public CalendarController(IUserService userService, IAdwService adwService) : base(userService, adwService) { }

        /// <summary>
        /// Index action.
        /// </summary>
		[Menu("Calendar")]
        public ActionResult Index()
        {
			var model = new ContentViewModel()
				.AddTitle("Hello World!")
				.AddParagraph("Welcome to the Calendar area.");

            return View(model);
        }
    }
}
