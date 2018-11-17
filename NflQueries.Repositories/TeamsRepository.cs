using System;
using System.Collections.Generic;
using NflQueries.Domain;
using NflQueries.Interfaces;
using NflQueries.Models;
using StattleShip.NflApi;
using StattleShip.NflApi.Dtos;

namespace NflQueries.Repositories
{
	public class TeamsRepository : ITeamsRepository
	{
		public TeamsViewModel FindTeams()
		{
			var response = new TeamsViewModel
			{
				Teams = new List<Team>()
			};
			var request = new TeamsRequest();
			var teams = request.LoadData("nfl-2018-2019");
			foreach (var team in teams)
			{
				response.Teams.Add(TeamFor(team));
			}
			return response;
		}

		private Team TeamFor(TeamDto dto)
		{
			var team = new Team
			{
				Name = $"{dto.Name} {dto.NickName}"
			};
			return team;
		}
	}
}
