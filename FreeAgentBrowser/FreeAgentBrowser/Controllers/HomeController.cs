using FreeAgentBrowser.Models;
using FreeAgentBrowser.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FreeAgentBrowser.Controllers
{
	public class HomeController : Controller
	{
		private readonly IPlayerRepository _playerRepository;

		public HomeController(
			IPlayerRepository playerRepository)
		{
			_playerRepository = playerRepository;
		}

		public IActionResult Index()
		{
			var homeViewModel = new HomeViewModel
			{
				PlayersOfTheWeek = _playerRepository.PlayersOfTheWeek
			};

			return View(homeViewModel);
		}
	}
}
