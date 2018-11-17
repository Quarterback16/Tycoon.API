using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using StackExchange.Profiling;

namespace TrophyCaseWebApp.Controllers
{
   public class HomeController : Controller
   {
      public ActionResult Index()
      {
         var profiler = MiniProfiler.Current;
         using (profiler.Step("Doing complex stuff"))
         {
            using (profiler.Step("Step A"))
            { // something more interesting here
               Thread.Sleep(100);
            }
            using (profiler.Step("Step B"))
            { // and here
               Thread.Sleep(250);
            }
         }
         return View();
      }

      public ActionResult About()
      {
         ViewBag.Message = "Your application description page.";

         return View();
      }

      public ActionResult Contact()
      {
         ViewBag.Message = "Your contact page.";

         return View();
      }
   }
}