using StattleShip.NflApi;
using System;
using System.Collections.Generic;

namespace Gerard.HostServer
{
	public sealed class PlayerQueryHandler
		: IQueryHandler<PlayerQuery, Player>
	{
		readonly Dictionary<string, string> TeamSlugs;

		public PlayerQueryHandler()
		{
			TeamSlugs = new Dictionary<string, string>
			{
				{ "SF", "nfl-sf" }
			};
		}

		public Player Handle(PlayerQuery query)
		{
			var rosterRequest = new RosterRequest();
			var roster = rosterRequest.LoadData(
				Constants.Seasons.Season2018,
				SlugFor(query.TeamCode));

			var result = new Player();
			foreach (var playerDto in roster)
			{
				if (playerDto.FirstName == query.FirstName
					&& playerDto.LastName == query.LastName)
				{
					var player = new Player
					{
						Name = $"{playerDto.FirstName} {playerDto.LastName}",
						BirthDate = DateTime.Parse(playerDto.BirthDate),
						IsActive = playerDto.Active
					};

					result = player;
					break;
				}
			}
			return result;
		}

		private string SlugFor(string teamCode)
		{
			return TeamSlugs[teamCode];
		}
	}
}
