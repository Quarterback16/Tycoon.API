using FreeAgentBrowser.Models;
using FreeAgentBrowser.ViewModels;
using Microsoft.AspNetCore.Mvc;


namespace FreeAgentBrowser.Controllers
{
	public class PlayerController : Controller
	{
		private readonly IPlayerRepository _playerRepository;
		private readonly IPositionRepository _positionRepository;

		public PlayerController(
			IPlayerRepository playerRepository,
			IPositionRepository positionRepository)
		{
			_playerRepository = playerRepository;
			_positionRepository = positionRepository;
		}

		public ViewResult List()
		{
			var viewModel = new PlayerListViewModel
			{
				CurrentPosition = "Quarterbacks",
				Players = _playerRepository.AllPlayers
			};
			return View(
				viewModel);
		}

		public IActionResult Details(
			int id)
		{
			var p = _playerRepository.GetPlayerById(id);
			if (p == null)
				return NotFound();
			return View(p);
		}
	}
}
