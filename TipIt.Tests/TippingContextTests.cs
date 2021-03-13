using System.Collections.Generic;
using TipIt.Implementations;
using TipIt.Interfaces;
using TipIt.Models;
using Xunit;
using Xunit.Abstractions;

namespace TipIt.Tests
{
    public class TippingContextTests
    {
        private readonly ITestOutputHelper _output;
        public TippingContextTests(
            ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Context_KnowsNextNrlRound()
        {
            var cut = new TippingContext();
            var result = cut.NextRound("NRL");
            Assert.True(result == 1);
        }

        [Fact]
        public void Context_KnowsNextAflRound()
        {
            var cut = new TippingContext();
            var result = cut.NextRound("AFL");
            Assert.True(result == 1);
        }

        [Fact]
        public void Context_KnowsAflAverageScore()
        {
            var cut = new TippingContext();
            var result = cut.AverageScore("AFL");
            Assert.True(result == 80.0M,
                $"Average score Calculated = {result}");
        }

        [Fact]
        public void Context_KnowsAflMaxScore()
        {
            var cut = new TippingContext();
            var result = cut.MaxScore("AFL");
            Assert.True(result == 151,
                $"Max score Calculated = {result}");
        }

        [Fact]
        public void Context_KnowsAflMinScore()
        {
            var cut = new TippingContext();
            var result = cut.MinScore("AFL");
            Assert.True(result == 14,
                $"Min score Calculated = {result}");
        }

        [Fact]
        public void Context_KnowsNrlMaxScore()
        {
            var cut = new TippingContext();
            var result = cut.MaxScore("NRL");
            Assert.True(result == 64,
                $"Max score Calculated = {result}");
        }

        [Fact]
        public void Context_KnowsNrlMinScore()
        {
            var cut = new TippingContext();
            var result = cut.MinScore("NRL");
            Assert.True(result == 0,
                $"Min score Calculated = {result}");
        }

        [Fact]
        public void Context_KnowsNrlHomeFieldAdvantage()
        {
            var cut = new TippingContext();
            var result = cut.HomeFieldAdvantage("NRL");
            Assert.True(result == 3,
                $"Home field Advantage calculated = {result}");
        }

        [Fact]
        public void Context_KnowsAflHomeFieldAdvantage()
        {
            var cut = new TippingContext();
            var result = cut.HomeFieldAdvantage("AFL");
            Assert.True(result == 3,
                $"Home field Advantage calculated = {result}");
        }

        [Fact]
        public void Context_KnowsAflAverageMarginOfVictory()
        {
            var cut = new TippingContext();
            var result = cut.AverageMargin("AFL");
            Assert.True(result == 30M,
                $"Average margin calculated = {result}");
        }


        [Fact]
        public void Context_KnowsNrlAverageMarginOfVictory()
        {
            var cut = new TippingContext();
            var result = cut.AverageMargin("NRL");
            Assert.True(result == 14M,
                $"Average margin calculated = {result}");
        }

        [Fact]
        public void Context_KnowsAflAverageScoreForStKilda()
        {
            var teamCode = "STK";
            var cut = new TippingContext();
            var result = cut.AverageScore("AFL", teamCode);
            Assert.True(result == 75.0M,
                $"Average score calculated for {teamCode} = {result}");
        }

        [Fact]
        public void Context_KnowsAflPastRecordForStKilda()
        {
            var teamCode = "STK";
            var cut = new TippingContext();
            var result = cut.PastRecord("AFL", teamCode);
            _output.WriteLine(result.ToString());
            //Assert.True(result == 75.0M,
            //    $"Average score calculated for {teamCode} = {result}");
        }

        [Fact]
        public void Context_KnowsAflAverageScoreForHawthorn()
        {
            var teamCode = "HAW";
            var cut = new TippingContext();
            var result = cut.AverageScore("AFL", teamCode);
            Assert.True(result == 79.0M,
                $"Average score calculated for {teamCode} = {result}");
        }

        [Fact]
        public void Context_KnowsAflAverageScoreForGeelong()
        {
            var teamCode = "GEEL";
            var cut = new TippingContext();
            var result = cut.AverageScore("AFL", teamCode);
            Assert.True(result == 90M,
                $"Average score calculated for {teamCode} = {result}");
        }

        [Fact]
        public void Context_KnowsNrlAverageScore()
        {
            var cut = new TippingContext();
            var result = cut.AverageScore("NRL");
            Assert.True(result == 20.0M,
                $"Average score Calculated = {result}");
        }

        [Fact]
        public void Context_KnowsNrlFormOfTitans()
        {
            var cut = new TippingContext();
            Assert.True(
                cut.LeaguePastResults["NRL"].Count == 25); //rounds

            var result = cut.CurrentForm(
				leagueCode: "NRL",
				teamCode: "TITN");
            _output.WriteLine(result);
        }

        [Fact]
        public void Context_KnowsNrlLatestFormOfTitans()
        {
            var cut = new TippingContext();

            var result = cut.FormLast( 
                4, 
                leagueCode: "NRL",
                teamCode: "TITN");
            _output.WriteLine(result);
        }

        [Fact]
        public void Context_KnowsNrlPreviousStats()
        {
            var cut = new TippingContext();
            var result = cut.LastYearsStats("NRL");
            _output.WriteLine(result.ToString());
            Assert.True(result.TotalGames == 24 * 8,
                $"Average score Calculated = {result.TotalGames}");
            Assert.True(result.TeamScoreAverage == 20,
                $"Average team score = {result.TeamScoreAverage}");
            Assert.True(result.TeamScoreMode == 12,
                $"Most common score = {result.TeamScoreMode}");
        }

        [Fact]
        public void Context_KnowsAflPreviousStats()
        {
            var cut = new TippingContext();
            var result = cut.LastYearsStats("AFL");
            _output.WriteLine(result.ToString());
            Assert.True(result.TotalGames == 22 * 9,
                $"Average score Calculated = {result.TotalGames}");
            Assert.True(result.TeamScoreAverage == 80,
                $"Average team score = {result.TeamScoreAverage}");
            Assert.True(result.TeamScoreMode == 69,
                $"Most common score = {result.TeamScoreMode}");
        }

        [Fact]
        public void Context_KnowsAflTeams()
        {
            var cut = new TippingContext();
            var result = cut.GetTeams("AFL");
            foreach (var item in result)
                _output.WriteLine(item);
            Assert.True(result.Count == 18,
                $"AFL Teams counted = {result.Count}");
        }

        [Fact]
        public void Context_KnowsNrlTeams()
        {
            var cut = new TippingContext();
            var result = cut.GetTeams("NRL");
            foreach (var item in result)
                _output.WriteLine(item);
            Assert.True(result.Count == 16,
                $"AFL Teams counted = {result.Count}");
        }

        [Fact]
        public void Context_KnowsAflTeamRecords()
        {
            var cut = new TippingContext();
            var result = cut.TeamRecords("AFL");
            _output.WriteLine(result);
        }

        [Fact]
        public void Context_KnowsNrlTeamRecords()
        {
            var cut = new TippingContext();
            var result = cut.TeamRecords("NRL");
            _output.WriteLine(result);
        }

        [Fact]
        public void Context_CalculatesTotalEasyPointsForMyTeamSet()
        {
            var aflSet = new List<string>
            {
                "ADEL",
                "ESS",
                "GEEL",
                "PORT",
                "COLL"
            };
            var nrlSet = new List<string>
            {
                "WTIG",
                "BULL",
                "SSYD",
                "SHRK",
                "BRIS"
            };
            var cut = new TippingContext();
            var result = cut.EasyPoints(
                aflSet: aflSet,
                nrlSet: nrlSet);
            _output.WriteLine($"Points so far {result}");
        }

        [Fact]
        public void Context_CalculatesEasyPointsForMyTeamSet()
        {
            var aflSet = new List<string>
            {
                "ADEL",
                "ESS",
                "GEEL",
                "PORT",
                "COLL"
            };
            var nrlSet = new List<string>
            {
                "WTIG",
                "BULL",
                "SSYD",
                "SHRK",
                "BRIS"
            };
            var cut = new TippingContext();
            foreach (var team in aflSet)
            {
                _output.WriteLine($"{team} : {cut.EasyPointsForTeam("AFL",team)}");
            }
            foreach (var team in nrlSet)
            {
                _output.WriteLine($"{team} : {cut.EasyPointsForTeam("NRL", team)}");
            }
        }

        [Fact]
        public void Context_CanFeedGames()
        {
            var testProcessor = new TestProcessor(
                _output);
            var cut = new TippingContext();
            cut.ProcessLeagueSchedule(
                "NFL",
                testProcessor);
        }
    }

	public class TestProcessor : IGameProcessor
	{
        private readonly ITestOutputHelper _output;
		public TestProcessor(
            ITestOutputHelper output)
        {
            _output = output;
        }

        public void ProcessGame(
            Game g,
            int gameNum)
		{
            _output.WriteLine(g.ToString());
        }
	}
}
