using System.Web.Mvc; 
using Employment.Web.Mvc.Infrastructure.Controllers;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.ViewModels;

namespace Employment.Web.Mvc.Area.Payment.Controllers
{
	/// <summary>
	/// Defines the PaymentController controller.
	/// </summary>
	[Security(AllowAny = true)]
    public class PaymentController : InfrastructureController
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="PaymentController" /> class.
        /// </summary> 
        /// <param name="userService">User service for retrieving user data.</param>
        /// <param name="adwService">Adw service for retrieving ADW data.</param>
        public PaymentController(IUserService userService, IAdwService adwService) : base(userService, adwService) { }

        /// <summary>
        /// Index action.
        /// </summary>
		[Menu("Payment")]
        public ActionResult Index()
        {
			var model = new ContentViewModel()
				.AddTitle("Hello World!")
				.AddParagraph("Welcome to the Payment area.");

            return View(model);
        }
    }
}
