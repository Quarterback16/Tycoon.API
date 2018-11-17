using Logic.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Tycoon.Api.Models;
using Tycoon.Logic.Services;

namespace Tycoon.Api.Controllers
{
	[Route("api/players")]
	public class PlayerController : BaseController
	{
		private readonly MessageBus _messageBus;

		public PlayerController(MessageBus bus)
		{
			_messageBus = bus;
		}

		[HttpGet]
		public IActionResult GetPlayers()
		{
			return Ok( PlayersDataStore.Current.Players );
		}

//		[HttpGet("api/players/{id}")]
		[HttpGet("{id}")]
		public IActionResult GetPlayer(string id)
		{
			var playerToReturn = 
				PlayersDataStore.Current.Players.FirstOrDefault( p => p.PlayerId == id );
			if (playerToReturn == null)
				return NotFound();
			return Ok(playerToReturn);
		}

		[HttpPost]
		public IActionResult DataCheck([FromBody] DataCheckDto dto)
		{
			var command = new DataCheckCommand(
				dto.FirstName,
				dto.Surname,
				dto.TeamCode );

			var result = _messageBus.Dispatch(command);
			return FromResult(result);
		}
	}
}
