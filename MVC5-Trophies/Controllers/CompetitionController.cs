using MVC5_Trophies.Infrastructure;
using MVC5_Trophies.Services;
using MVC5_Trophies.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5_Trophies.Controllers
{
    public class CompetitionController : Controller
    {
       public CompetitionService CompetitionService { get; set; }

       public CompetitionController()
       {
          CompetitionService = new CompetitionService();  //  this should be injected as an Interface (for unit testing)
       }

        public ActionResult Index()
        {
           Session[Profiler.SESSION_KEY_CURRENT_USER] = "NATION\\SC0779";
           var vm = new CompetitionIndexViewModel();
           vm.Competitions = CompetitionService.GetCompetitions();

           return View( vm );
        }
	}
}