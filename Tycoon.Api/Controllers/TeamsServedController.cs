using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Tycoon.Api.Controllers
{
	[Route("api/players/")]
	public class TeamsServedController : BaseController
	{
		[HttpGet("{playerId}/teamsserved")]
		public IActionResult GetTeamsServed(string playerId)
		{
			var player = PlayersDataStore.Current.Players.FirstOrDefault(p => p.PlayerId == playerId);
			if (player == null)
				return NotFound();
			return Ok(player.TeamsServed);
		}

		[HttpGet("{playerId}/teamsserved/{id}")]
		public IActionResult GetTeamsServed(string playerId, int id)
		{
			var player = PlayersDataStore.Current.Players.FirstOrDefault(p => p.PlayerId == playerId);
			if (player == null)
				return NotFound();
			var teamServed = player.TeamsServed.FirstOrDefault(t => t.Id == id);
			if (teamServed == null)
				return NotFound();
			return Ok(teamServed);
		}

	}
}
