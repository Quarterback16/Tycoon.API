using StattleShip.NflApi;
using System;
using System.Collections.Generic;

namespace Gerard.HostServer
{
	public sealed class PlayerQueryHandler
		: IQueryHandler<PlayerQuery, Player>
	{
		private readonly Dictionary<string, string> TeamSlugs;

		public PlayerQueryHandler()
		{
			TeamSlugs = new Dictionary<string, string>
			{
				{ "AC", "nfl-ari" },
				{ "AF", "nfl-atl" },
				{ "BR", "nfl-bal" },
				{ "BB", "nfl-buf" },
				{ "CP", "nfl-car" },
				{ "CI", "nfl-cin" },
				{ "CH", "nfl-chi" },
				{ "CL", "nfl-cle" },
				{ "DC", "nfl-dal" },
				{ "DB", "nfl-den" },
				{ "DL", "nfl-det" },
				{ "GB", "nfl-gb" },
				{ "HT", "nfl-hou" },
				{ "IC", "nfl-ind" },
				{ "JJ", "nfl-jac" },
				{ "KC", "nfl-kc" },
				{ "SL", "nfl-stl" },
				{ "LC", "nfl-lac" },
				{ "MD", "nfl-mia" },
				{ "MV", "nfl-min" },
				{ "NE", "nfl-ne" },
				{ "NO", "nfl-no" },
				{ "NG", "nfl-nyg" },
				{ "NJ", "nfl-nyj" },
				{ "OR", "nfl-oak" },
				{ "PE", "nfl-phi" },
				{ "PS", "nfl-pit" },
				{ "SS", "nfl-sea" },
				{ "SF", "nfl-sf" },
				{ "TB", "nfl-tb" },
				{ "TT", "nfl-ten" },
				{ "WR", "nfl-was" },
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