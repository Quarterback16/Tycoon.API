using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using LearningKendoUIWeb.Repository;
using System.Web.Mvc;

namespace LearningKendoUIWeb.Controllers
{
    public class KendoController : Controller
    {
        //
        // GET: /Kendo/

        public ActionResult Index()
        {
            return View();
        }

       public JsonResult RemoteData()
       {
          var repository = new SampleRepository();
          var data = repository.GetAllMovies();
          return Json( ResultExecutedContext, JsonRequestBehavior.AllowGet );
       }

    }
}
