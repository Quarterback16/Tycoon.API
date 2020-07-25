using System.Collections.Generic;

namespace FreeAgentBrowser.Models
{
	public interface IPlayerRepository
	{
		IEnumerable<Player> AllPlayers { get; }
		List<Player> PlayersOfTheWeek { get; }

		Player GetPlayerById(int playerId);
	}
}
