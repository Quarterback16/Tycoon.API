using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterGrid;
using RosterLib;
using TFLLib;
using System.IO;

namespace RosterGridTests
{
	[TestClass]
	public class DbfPredictionStorerTests
	{
		[TestMethod]
		public void TestDbfPredictionStorer()
		{
			var game = new NFLGame( "2006:02-A" ); // BB @ MD         
			var result = new NFLResult( "MD", 24, "BB", 20 );
			result.HomeYDp = 250;
			result.HomeYDr = 116;
			result.AwayYDp = 350;
			result.AwayYDr = 68;
			result.AwayTDr = 1;
			var storer = new DbfPredictionStorer();
			storer.StorePrediction( "test", game, result );
			var getter = new DbfPredictionQueryMaster();
			var prediction = getter.Get( "test", "2006", "02", "A" );
			Assert.IsTrue( prediction.HomeScore.Equals( 24 ), "Home score should be 24" );
			Assert.IsTrue( prediction.AwayScore.Equals( 20 ), "Away score should be 20" );
			Assert.IsTrue( prediction.NflResult.AwayTDr.Equals( 1 ), "Away TDr should be 1" );
			Assert.IsTrue( prediction.NflResult.AwayYDr.Equals( 68 ), "Away YDr should be 68" );
		}

	}
}
