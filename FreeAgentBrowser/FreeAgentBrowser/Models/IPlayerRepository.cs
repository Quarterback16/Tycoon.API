using System.Collections.Generic;

namespace FreeAgentBrowser.Models
{
	public interface IPlayerRepository
	{
		IEnumerable<Player> AllPlayers { get; }
		Player GetPlayerById(int playerId);
	}
}
