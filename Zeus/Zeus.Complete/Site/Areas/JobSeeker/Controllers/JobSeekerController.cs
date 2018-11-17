using System.Web.Mvc; 
using Employment.Web.Mvc.Infrastructure.Controllers;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.ViewModels;

namespace Employment.Web.Mvc.Area.JobSeeker.Controllers
{
	/// <summary>
	/// Defines the JobSeekerController controller.
	/// </summary>
	[Security(AllowAny = true)]
    public class JobSeekerController : InfrastructureController
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="JobSeekerController" /> class.
        /// </summary> 
        /// <param name="userService">User service for retrieving user data.</param>
        /// <param name="adwService">Adw service for retrieving ADW data.</param>
        public JobSeekerController(IUserService userService, IAdwService adwService) : base(userService, adwService) { }

        /// <summary>
        /// Index action.
        /// </summary>
		[Menu("JobSeeker")]
        public ActionResult Index()
        {
			var model = new ContentViewModel()
				.AddTitle("Hello World!")
				.AddParagraph("Welcome to the JobSeeker area.");

            return View(model);
        }
    }
}
