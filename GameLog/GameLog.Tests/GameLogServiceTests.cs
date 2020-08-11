using GameLogService.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void GameStatsRepository_ForPlayer_Returns16rows()
		{
            var playerModel = new PlayerReportModel
            {
                Season = "1984",
                PlayerName = "Woody Bennett"
            };

            Console.WriteLine(
                _sut.PlayerLogUrl(
                    playerModel));

            var result = _sut.GetGameStats(
				model: playerModel);

            _sut.SendToConsole(playerModel);
            _sut.SendLineToConsole(playerModel);

            Assert.AreEqual(16, result.Count);
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
        public void GameStatsRepoForEntireTeam()
		{
            int[] weeks = new int[] { 1, 2, 3, 4  };
            List<string> teamList = new List<string>
			{
				"Neil Lomax",
				"Dave Krieg",
				"Jim McMahon",
				"Dan Fouts",
				"Sammy Winder",
				"James Jones",
				"Wendell Tyler",
				"James Brooks",
				"Ottis Anderson",
				"Derrick Ramsey",
				"Ozzie Newsome",
				"Kevin House",
				"Henry Marshall",
				"Stephen Starring",
				"Alfred Jackson",
				"Butch Johnson",
				"Stacey Bailey",
				"Mark Clayton"
			};
			foreach (var item in teamList)
			{
                var playerModel = new PlayerReportModel
                {
                    Season = "1984",
                    PlayerName = item
                };
                _sut.GetGameStats(
                    model: playerModel);

                _sut.SendToConsole(
                    playerModel,
                    weeks);
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

            _sut.SendKickerToConsole(playerModel);

            Assert.AreEqual(16, result.Count);
        }
    }
}
