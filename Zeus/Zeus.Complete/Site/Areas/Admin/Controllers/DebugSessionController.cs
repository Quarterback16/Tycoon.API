using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.ViewModels;

namespace Employment.Web.Mvc.Area.Admin.Controllers {
 #if DEBUG
    /// <summary>
    /// A debug mode page for viewing session state contents.
    /// </summary>
     [Security(AllowAny = true)]
  public class DebugSessionController :Controller 
    {
      /// <summary>
      /// Initializes a new instance of the <see cref="DebugSessionController"/> class.
      /// </summary>
		public DebugSessionController()
		{
		}
        /// <summary>
        /// Session view index
        /// </summary>
        /// <returns></returns>
        [Menu("Session contents")]
        public ActionResult Index()
        {
            var model = new ContentViewModel()
               .AddTitle("Debug Session");

            return View(model);
        }
  }
#endif
 }

