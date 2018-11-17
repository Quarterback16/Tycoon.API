using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using MvcJqGrid;
using Players.Helpers;
using Players.Services;

namespace Players.Controllers
{
	public class HomeController : Controller
	{
		public TflService TflService { get; set; }

		public HomeController()
		{
			TflService = new TflService();
		}

		/// <summary>
		///   The Home page
		/// </summary>
		/// <returns></returns>
		public ActionResult Index()
		{
			return View();
		}

		/// <summary>
		///   The action that generates the JSON data for the jqGrid
		/// </summary>
		/// <param name="gridSettings"></param>
		/// <returns></returns>
		public ActionResult GridDataPlayers( GridSettings gridSettings  )
		{
			var list = TflService.GetPlayers( gridSettings );
			var totalRecs = TflService.CountPlayers( gridSettings );

			var jsonData = new
			{
				total = AppHelper.PagesInTotal( totalRecs, gridSettings.PageSize ),
				page = gridSettings.PageIndex,
				records = totalRecs,
				rows = (
							 from e in list.AsEnumerable()
							 select new
							 {
								 id = e.PlayerId,
								 cell = new List<string>
									       {
										       e.PlayerId.ToString( CultureInfo.InvariantCulture ),
										       e.FirstName,
										       e.Surname
									       }
							 }
						 ).ToArray()
			};

			return Json( jsonData, JsonRequestBehavior.AllowGet );
		}


		public ActionResult About()
		{
			return View();
		}

	}
}
