using GameLogService.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterService;
using System;
using System.Collections.Generic;

namespace GameLog.Tests
{
    [TestClass]
    public class GameLogServiceTests
    {
        private GameStatsRepository _sut;

        [TestInitialize]
        public void Initialise()
		{
            _sut = new GameStatsRepository();
		}

        [TestMethod]
        public void GameStatsRepository_ForEddieLee_ReturnsStatsIinCorrectWeeks()
        {
            var playerModel = new PlayerReportModel
            {
                Season = "1984",
                PlayerName = "Eddie Lee Ivery"
            };

            Console.WriteLine(
                _sut.PlayerLogUrl(
                    playerModel));

            var result = _sut.GetGameStats(
                model: playerModel);

            // week 12 he had 3TDs
            Assert.AreEqual(
                3, 
                result[11].RushingTds);

            _sut.SendToConsole(
                playerModel);
            _sut.SendLineToConsole(
                playerModel);

            Assert.AreEqual(16, result.Count);
        }

        [TestMethod]
        public void GameStatsRepository_ComparePlayers_Returns16rows()
		{
			PlayerLine(
                "Richard Todd");
            PlayerLine(
                "Steve DeBerg");
            PlayerLine(
                "Warren Moon");
        }

		private List<GameStats> PlayerLine(
            string playerName,
            string season = "1984")
		{
			var playerModel = new PlayerReportModel
			{
				Season = season,
				PlayerName = playerName
			};

			var result = _sut.GetGameStats(
				model: playerModel);

			_sut.SendLineToConsole(playerModel);
            Assert.AreEqual(16, result.Count);
            return result;
		}

		[TestMethod]
        public void GameStatsRepository_ForDwightClark_Returns16rows()
        {
            var playerModel = new PlayerReportModel
            {
                Season = "1984",
                PlayerName = "Dwight Clark"
            };

            var result = _sut.GetGameStats(
                model: playerModel);

            _sut.SendToConsole(playerModel);

            Assert.AreEqual(16, result.Count);
        }

        [TestMethod]
        public void GameStatsRepository_ForFrddieSolommon_Returns10catches()
        {
            var playerModel = new PlayerReportModel
            {
                Season = "1984",
                PlayerName = "Freddie Solomon"
            };

            var result = _sut.GetGameStats(
                model: playerModel);

            _sut.SendToConsole(playerModel);

            Assert.AreEqual(16, result.Count);
            Assert.AreEqual(0, result[0].ReceivingTds);
            Assert.AreEqual(0, result[1].ReceivingTds);
            Assert.AreEqual(1, result[2].ReceivingTds);
            Assert.AreEqual(1, result[3].ReceivingTds);
            Assert.AreEqual(0, result[4].ReceivingTds);
            Assert.AreEqual(0, result[5].ReceivingTds);
            Assert.AreEqual(0, result[6].ReceivingTds);
            Assert.AreEqual(0, result[7].ReceivingTds);
            Assert.AreEqual(1, result[8].ReceivingTds);
            Assert.AreEqual(1, result[9].ReceivingTds);
            Assert.AreEqual(2, result[10].ReceivingTds);
            Assert.AreEqual(0, result[11].ReceivingTds);
            Assert.AreEqual(1, result[12].ReceivingTds);
            Assert.AreEqual(1, result[13].ReceivingTds);
            Assert.AreEqual(1, result[14].ReceivingTds);
            Assert.AreEqual(1, result[15].ReceivingTds);
        }

        [TestMethod]
        public void GameStatsRepository_ForSurnameM_KnowsListUrl()
        {
            var result = _sut.PlayerListUrl(
				letter: "M");

            Assert.AreEqual(
                "https://www.pro-football-reference.com/players/M/", 
                result);
        }

        [TestMethod]
        public void GameStatsRepository_ForJoeMontana_KnowsListUrl()
        {
            var result = _sut.PlayerListUrl(
                season: "1984",
                playerName: "Joe Montana");

            Assert.AreEqual(
                "https://www.pro-football-reference.com/players/M/",
                result);
        }

        [TestMethod]
        public void GameStatsRepository_ForJoeMontana_KnowsPlayerCode()
        {
            var result = _sut.PlayerCode(
                season: "1984",
                playerName: "Joe Montana");

            Console.WriteLine(result);
            Assert.AreEqual(
                "MontJo01",
                result);
        }

        [TestMethod]
        public void GameStatsRepository_ForARetiredPlayer_ReturnsEmpty()
        {
            var result = _sut.PlayerCode(
                season: "1984",
                playerName: "Brian Sipe");

            Console.WriteLine(result);
            Assert.AreEqual(
                "",
                result);
        }

        [TestMethod]
        public void GameStatsRepository_WithLinkHtml_KnowsPlayerCode()
        {
            //<p><a href="/players/M/MontJo01.htm">Joe Montana</a>+ (QB) 1979-1994</p>
            var result = _sut.PlayerCodeFrom(
                href: @"<p><a href=""/players/M/MontJo01.htm"">Joe Montana</a>+ (QB) 1979-1994</p>");

            Console.WriteLine(result);
            Assert.AreEqual(
                "MontJo01",
                result);
        }

        [TestMethod]
        public void GameStatsRepository_WithLinkText_KnowsPlayerCareerRange()
        {
            //Joe Montana+ (QB) 1979-1994
            var result = _sut.CareerRange(
                nodeText: "Joe Montana+ (QB) 1979-1994");

            Console.WriteLine(result);
            Assert.AreEqual(
                (1979, 1994),
                result);
        }

        [TestMethod]
        public void GameStatsRepo_WithPlayerNameMontana_ReturnsM()
        {
            var result = _sut.FirstLetterOfSurname(
                "Joe Montana");

            Assert.AreEqual("M", result);
        }

        [TestMethod]
        public void GameStatsRepository_ForJoeMontana_KnowsGameLogUrl()
        {
            var result = _sut.PlayerLogUrl(
                season: "1984",
                playerName: "Joe Montana");

            Assert.AreEqual(
                "https://www.pro-football-reference.com/players/M/MontJo01/gamelog/1984/",
                result);
        }

        [TestMethod]
        public void GameStatsRepoForEntireTeamEntireYear()
        {
            var fantasyTeam = "CD";
            var rosterService = new RetroRosters(
                new RosterEventStore());
            var roster = rosterService.GetRoster(
                fantasyTeam);
            var teamList = new List<(string, string)>();
            AddRosterInOrder(roster, teamList);
            foreach (var item in teamList)
            {
                var playerModel = new PlayerReportModel
                {
                    Season = "1984",
                    PlayerName = item.Item1
                };

                if (item.Item2.Equals("KK"))
                {
                    _sut.GetKickerStats(
                        model: playerModel);
                    _sut.SendKickerLineToConsole(
                        playerModel);
                }
                else
                {
                    _sut.GetGameStats(
                        model: playerModel);
                    _sut.SendLineToConsole(
                        playerModel);
                }
            }
        }

        [TestMethod]
        public void GameStatsRepoForEntireTeam()
		{
			var fantasyTeam = "BR";
			var rosterService = new RetroRosters(
				new RosterEventStore());
			var roster = rosterService.GetRoster(
				fantasyTeam);
			var teamList = new List<(string, string)>();
			AddRosterInOrder(roster, teamList);
			//int[] weeks = new int[] { 5, 6, 7, 8 };
            //int[] weeks = new int[] { 9, 10, 11, 12 };
            int[] weeks = new int[] { 13, 14, 15, 16 };
            foreach (var item in teamList)
			{
				var playerModel = new PlayerReportModel
				{
					Season = "1984",
					PlayerName = item.Item1
				};

				if (item.Item2.Equals("KK"))
				{
					_sut.GetKickerStats(
						model: playerModel);
					_sut.SendKickerToConsole(
						playerModel,
						weeks);
				}
				else
				{
					_sut.GetGameStats(
						model: playerModel);
					_sut.SendToConsole(
						playerModel,
						weeks);
				}
			}
		}

		private static void AddRosterInOrder(
			List<string> roster,
			List<(string, string)> teamList)
		{
			AddPartial(
				"QB",
				roster,
				teamList);
			AddPartial(
				"RB",
				roster,
				teamList);
			AddPartial(
				"TE",
				roster,
				teamList);
			AddPartial(
				"WR",
				roster,
				teamList);
			AddPartial(
				"KK",
				roster,
				teamList);
		}

		private static void AddPartial(
            string desiredPos,
			List<string> roster,
			List<(string, string)> teamList)
		{
			foreach (var player in roster)
			{
				var thePlayer = player.Substring(7, 20)
					.Trim();
				var thePos = player.Substring(5, 2);
                if (thePos.Equals(desiredPos))
				    teamList.Add((thePlayer, thePos));
			}
		}

		[TestMethod]
        public void GameStatsRepository_ForKicker_Returns16rows()
        {
            var playerModel = new PlayerReportModel
            {
                Season = "1984",
                PlayerName = "Uwe Von Schamann"
            };

            var result = _sut.GetKickerStats(
                model: playerModel);

            _sut.SendKickerToConsole(
                playerModel);

            Assert.AreEqual(16, result.Count);
        }
    }
}
