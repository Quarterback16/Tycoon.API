using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterGrid;
using RosterLib;
using TFLLib;
using System.IO;

namespace RosterGridTests
{
	[TestClass]
	public class NflResultTests
	{
		[TestMethod]
		public void TestResultIsValid()
		{
			var sut = new NFLResult( home: "BB", homePts: 20, away: "NE", awayPts: 31 );
			sut.AwayFg = 1;
			sut.AwayTDr = 2;
			sut.AwayTDp = 2;
			sut.HomeTDp = 1;
			sut.HomeTDd = 1;
			sut.HomeFg = 2;
			var isValid = sut.IsValid();
			Assert.IsTrue( isValid );
		}

		[TestMethod]
		public void TestAtsResult()
		{
			var prediction = new NFLResult { HomeTeam = "PE", AwayTeam = "BR", AwayScore = 22, HomeScore = 21 };
			var game = new NFLGame( "2012:02-F" );  //  BR @ PE
			var atsResult = game.EvaluatePredictionAts( prediction, game.Spread );
			Assert.AreEqual( "ATS:PUSH", atsResult );
		}

		[TestMethod]
		public void TestAtsResultTie()
		{
			var prediction = new NFLResult { HomeTeam = "IC", AwayTeam = "MV", AwayScore = 22, HomeScore = 21 };
			var game = new NFLGame( "2012:02-E" );  //  BR @ PE
			var atsResult = game.EvaluatePredictionAts( prediction, game.Spread );
			Assert.AreEqual( "ATS:PUSH", atsResult );
		}

		[TestMethod]
		public void TestMarginForTeam()
		{
			var result = new NFLResult( "SF", 30, "GB", 22 );
			Assert.AreEqual( 8, result.MarginForTeam( "SF" ), "SF margin should be +8" );
			Assert.AreEqual( -8, result.MarginForTeam( "GB" ), "GB margin should be -8" );
		}

	}
}
