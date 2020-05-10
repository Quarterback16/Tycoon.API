using System.Collections.Generic;
using System.Linq;

namespace FreeAgentBrowser.Models
{
	public class MockPlayerRepository : IPlayerRepository
	{
		public IEnumerable<Player> AllPlayers => 
			new List<Player>
			{
				new Player
				{
					Id = 1,
					Name = "Joe Montana",
					Number = 4,
					Position = new Position
					{
						PositionId = 1,
						Description = "Quarterback",
						PositionName = "QB"
					}
				}
			};

		public Player GetPlayerById(int playerId)
		{
			return AllPlayers.FirstOrDefault(p => p.Id == playerId);
		}
	}
}
