using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Data;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using RosterLib.Models;
using TFLLib;

namespace RosterGridTests
{
	[TestClass]
	public class UnitPredictorTests
	{
		[TestMethod]
		public void TestSeasonProjection2013()
		{
			var season = new NflSeason("2013", loadGames: false, loadDivisions: true);
			season.Predict();
			Assert.IsTrue(true);
		}

		[TestMethod]
		public void TestUnitPredictorPredictGameWeek8()
		{
			var predictor = new UnitPredictor
			{
				TakeActuals = true,
				AuditTrail = true,
				WriteProjection = false,
				StorePrediction = true,
				RatingsService = new UnitRatingsService()
			};
			var game = new NFLGame( "2013:08-E" );  //  BB @ NO
			var result = predictor.PredictGame( game, new FakePredictionStorer(), new DateTime( 2013, 10, 17 ) );
			Assert.IsTrue( result.HomeWin() );
			Assert.IsTrue( result.HomeScore.Equals( 34 ), string.Format( "Home score should be 34 not {0}", result.HomeScore ) );
			Assert.IsTrue( result.AwayScore.Equals( 10 ), string.Format( "Away score should be 10 not {0}", result.AwayScore ) );
			Assert.IsTrue( result.AwayTDr.Equals( 0 ), string.Format( "Away TDR should be 0 not {0}", result.AwayTDr ) );
			Assert.IsTrue( result.AwayTDp.Equals( 1 ), string.Format( "Away TDP should be 1 not {0}", result.AwayTDp ) );
			Assert.IsTrue( result.AwayYDp.Equals( 250 ), string.Format( "Away YDP should be 222 not {0}", result.AwayYDp ) );
		}

		[TestMethod]
		public void TestSeasonProjectionStats2013()
		{
			var method = "unit";
			var nGames = 0;
			var totPoints = 0M;
			var totHome = 0M;
			var totAway = 0M;
			var ds = Utility.TflWs.GetAllPredictions("2013", method);
			var totPredictions = ds.Tables["prediction"].Rows.Count;
			Utility.Announce(string.Format("{0} predictions loaded for method {1}", totPredictions, method));
			if (totPredictions > 0)
			{
				foreach (var result in ds.Tables["prediction"].Rows )
											  
				{
					var prediction =  new Prediction(result as DataRow);
					totPoints += prediction.HomeScore + prediction.AwayScore;
					totHome += prediction.HomeScore;
					totAway += prediction.AwayScore;
					nGames++;
				}
			}
			Utility.Announce(string.Format("{0} total points loaded for method {1}", totPoints, method));
			Utility.Announce(string.Format("{0} total games loaded for method {1}", nGames, method));
			if (nGames > 0)
			{
				Utility.Announce(string.Format("Average Team Score is {0:0.0} for method {1}", totPoints / (nGames * 2), method));
				Utility.Announce(string.Format("Average Home Score is {0:0.0} for method {1}", totHome / nGames, method));
				Utility.Announce(string.Format("Average Away Score is {0:0.0} for method {1}", totAway / nGames, method));
			}
			Assert.IsTrue(true);
		}

		[TestMethod]
		public void TestPredictionsFor2013()
		{
			const string theSeason = "2013";
			var rr = new NFLRosterReport(theSeason);
			rr.SeasonProjection("Spread", theSeason, "0", Utility.StartOfSeason(theSeason));
			Assert.IsTrue(File.Exists(rr.FileOut), string.Format("Cannot find {0}", rr.FileOut));
		}

		[TestMethod]
		public void TestUnitPredictorPredictGame()
		{
			var predictor = new UnitPredictor
			{
				TakeActuals = true,
				AuditTrail = true,
				WriteProjection = false,
				StorePrediction = true,
				RatingsService = new UnitRatingsService()
			};
			var game = new NFLGame("2013:01-A");  //  DB @ BR
			var result = predictor.PredictGame(game, new FakePredictionStorer(), Utility.StartOfSeason());
			Assert.IsTrue(result.HomeWin());
			Assert.IsTrue( result.HomeScore.Equals( 34 ), string.Format("Home score should be 34 not {0}", result.HomeScore));
			Assert.IsTrue( result.AwayScore.Equals( 10 ), string.Format("Away score should be 10 not {0}", result.AwayScore));
			Assert.IsTrue( result.AwayTDr.Equals( 0 ), string.Format( "Away TDR should be 0 not {0}", result.AwayTDr ) );
			Assert.IsTrue( result.AwayTDp.Equals( 1 ), string.Format( "Away TDP should be 1 not {0}", result.AwayTDp ) );
			Assert.IsTrue( result.AwayYDp.Equals( 250), string.Format( "Away YDP should be 222 not {0}", result.AwayYDp ) );
		}

		[TestMethod]
		public void TestUnitPredictorPredictGame2()
		{
			var predictor = new UnitPredictor
			{
				TakeActuals = true,
				AuditTrail = true,
				WriteProjection = false,
				StorePrediction = true,
				RatingsService = new UnitRatingsService()
			};
			var game = new NFLGame( "2013:01-B" );  //  NE @ BB
			var result = predictor.PredictGame( game, new FakePredictionStorer(), Utility.StartOfSeason() );
			Assert.IsTrue( result.HomeWin() );
			Assert.IsTrue( result.HomeScore.Equals( 20 ), string.Format( "Home score should be 34 not {0}", result.HomeScore ) );
			Assert.IsTrue( result.AwayScore.Equals( 31 ), string.Format( "Away score should be 10 not {0}", result.AwayScore ) );
		}

		[TestMethod]
		public void TestStartOfSeason()
		{
			var dStartSeason = Utility.StartOfSeason("2013");
			Assert.AreEqual(new DateTime(2013, 9, 8), dStartSeason);
		}

		[TestMethod]
		public void TestSunday()
		{
			//  lets deal with just Sundays
			var dSunday = new DateTime(2013, 7, 28);
			Assert.IsTrue( Utility.IsSunday( dSunday ) );
		}

		[TestMethod]
		public void TestGameOne()
		{
			//  lets deal with just Sundays
			var dGameOne = Utility.GetGameOne("2013");
			Assert.IsTrue(dGameOne == new DateTime(2013, 9, 5));
		}

		[TestMethod]
		public void TestTouchdownPassesBC()
		{
			var predictor = new UnitPredictor
			{
				TakeActuals = true,
				AuditTrail = true,
				WriteProjection = false,
				StorePrediction = true,
				RatingsService = new UnitRatingsService()
			};
			var TDp = predictor.TouchdownPasses("B", "C");
			Assert.AreEqual( 2, TDp); 
		}

		[TestMethod]
		public void TestTouchdownPassesAE()
		{
			var predictor = new UnitPredictor
			{
				TakeActuals = true,
				AuditTrail = true,
				WriteProjection = false,
				StorePrediction = true,
				RatingsService = new UnitRatingsService()
			};
			var TDp = predictor.TouchdownPasses("A", "E");
			Assert.AreEqual(3, TDp);
		}

		[TestMethod]
		public void TestPrdictions()
		{
			var predictor = new UnitPredictor
			{
				TakeActuals = true,
				AuditTrail = true,
				WriteProjection = false,
				StorePrediction = true,
				RatingsService = new UnitRatingsService()
			};
			var TDp = predictor.TouchdownPasses("A", "E");
			Assert.AreEqual(3, TDp);
		}
	}
}
