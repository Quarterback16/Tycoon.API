using TipIt.Events;
using TipIt.Implementations;
using TipIt.Models;
using TipIt.TippingStrategies;
using Xunit;
using Xunit.Abstractions;

namespace TipIt.Tests
{
	public class NibbleTipsterTests
	{
		private readonly ITestOutputHelper _output;

		public NibbleTipsterTests(
						ITestOutputHelper output)
		{
			_output = output;
		}

		[Fact]
		public void NibbleTipster_TipsNrlRound()
		{
			var cut = new NibbleTipster(
							 new TippingContext());

			var result = cut.ShowTips(
				league: "NRL",
				round: 1);
			_output.WriteLine(
				result);
			Assert.False(
				string.IsNullOrEmpty(result));
			DumpRatings(cut,"NRL");
			DumpMetrics(cut,"NRL");
		}

		private void DumpRatings(
			NibbleTipster cut,
			string leagueCode)
		{
			_output.WriteLine(
				cut.DumpRatings(
					leagueCode));
		}

		private void DumpMetrics(
			NibbleTipster cut,
			string leagueCode)
		{
			_output.WriteLine(
				$"Average Score      : {cut.AverageScore}");
			_output.WriteLine(
				$"Average Margin     : {cut.Context.AverageMargin(leagueCode)}");
			_output.WriteLine(
				$"Homefield Advantage: {cut.HomeFieldAdvantage}");
			_output.WriteLine(
				$"Max Score          : {cut.MaxScore}");
			_output.WriteLine(
				$"Min Score          : {cut.MinScore}");
		}

		[Fact]
		public void NibbleTipster_TipsAflRound()
		{
			var cut = new NibbleTipster(
							 new TippingContext());
			var result = cut.ShowTips("AFL", 1);
			_output.WriteLine(result);
			Assert.False(
				string.IsNullOrEmpty(result));
		}

		[Fact]
		public void NibbleTipster_RatesGame()
		{
			var testResult = new ResultEvent
			{
				LeagueCode = "NRL",
				HomeTeam = "MELB",
				AwayTeam = "BRIS",
				HomeScore = 22,
				AwayScore = 12
			};
			var testGame = new Game(testResult);
			var cut = new NibbleTipster(
							 new TippingContext());
			var result = cut.RateGame(testGame,20);
			_output.WriteLine(result.ToString());
			Assert.True(
				result.AwayRating.Offence.Equals(-2),
				$"Away Off adj was {result.AwayRating.Offence}");
			Assert.True(
				result.HomeRating.Defence.Equals(-2),
				$"Home Def adj was {result.AwayRating.Defence}");
		}

		[Fact]
		public void NibbleTipster_RatesLastYearsGames()
		{
			var cut = new NibbleTipster(
							 new TippingContext());
			_output.WriteLine(cut.RateResults("NRL"));
		}

		[Fact]
		public void NibbleTipster_TipsGame()
		{
			var testResult = new ResultEvent
			{
				LeagueCode = "NRL",
				HomeTeam = "MELB",
				AwayTeam = "BRIS",
			};
			var testGame = new Game(testResult);
			var cut = new NibbleTipster(
							 new TippingContext());
			_output.WriteLine(cut.RateResults("NRL"));
			_output.WriteLine(cut.Tip(testGame).ToString());
		}
	}
}
