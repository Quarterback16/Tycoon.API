using System.Web.Mvc;
using TflWebApi.ViewModels;

namespace TflWebApi.Controllers
{
   public class HomeController : Controller
   {
      // GET: Home
      public ActionResult Index()
      {
         return View();
      }

      public ActionResult FreeAgents()
      {
         return View();
      }

      [HttpPost]
      public ActionResult FreeAgents( FreeAgentsViewModel viewModel )
      {
         if ( ModelState.IsValid )
         {

            //  render results
            return View( "Thanks" );
         }
         else
         {
            return View();
         }
      }

      [ChildActionOnly]
      public ActionResult Players()
      {
         var playerService = new PlayerController();
         //  Call Data service
         var response = playerService.GetPlayers();
         return View( response );
      }
   }
}