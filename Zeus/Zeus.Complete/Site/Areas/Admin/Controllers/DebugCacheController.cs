using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.ViewModels;

namespace Employment.Web.Mvc.Area.Admin.Controllers {
#if DEBUG
    /// <summary>
    /// A debug mode page for viewing cache contents
    /// </summary>
    [Security(AllowAny = true)]
    public class DebugCacheController :Controller 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DebugCacheController"/> class.
        /// </summary>
		public DebugCacheController()
		{
		}
        /// <summary>
        /// Cache view index
        /// </summary>
        /// <returns></returns>
        [Menu("Cache contents")]
        public ActionResult Index()
        {
            var model = new ContentViewModel()
               .AddTitle("Debug Cache");

            return View(model);
        }
  }
#endif
}

