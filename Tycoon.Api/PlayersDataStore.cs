using System.Collections.Generic;
using Tycoon.Api.Models;

namespace Tycoon.Api
{
	public class PlayersDataStore
	{
		public static PlayersDataStore Current { get; } = new PlayersDataStore();  // C#6 auto assignment

		public List<PlayerDto> Players { get; set; }

		public PlayersDataStore()
		{
			//init dummy data
			Players = new List<PlayerDto>()
			{
				new PlayerDto
				{
					PlayerId = "MONTJO01",
					Name = "Joe Montana",
					FantasyPoints = 1000,
					TeamsServed = new List<TeamServedDto>{
						new TeamServedDto {
							Id = 1,
							Name = "San Francisco Forty Niners"
						},
						new TeamServedDto {
							Id = 2,
							Name = "Kansas City Chiefs"
						},
					}
				},
				new PlayerDto
				{
					PlayerId = "FITZRY01",
					Name = "Ryan Fitzmagic",
					FantasyPoints = 100,
					TeamsServed = new List<TeamServedDto>{
						new TeamServedDto {
							Id = 1,
							Name = "Buffalo Bills"
						},
						new TeamServedDto {
							Id = 2,
							Name = "New York Jets"
						},
						new TeamServedDto {
							Id = 3,
							Name = "Tampa Bay Buccaneers"
						},
					}
				}
			};


		}
	}
}
