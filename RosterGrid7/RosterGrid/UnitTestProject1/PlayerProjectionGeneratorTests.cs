using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterGrid;
using RosterLib;
using TFLLib;

namespace RosterGridTests
{
	[TestClass]
	public class PlayerProjectionGeneratorTests
	{
		//TODO:  This needs to be migrated to production
		[TestMethod]
		public void TestWeeklyProjection()
		{
			var w = new NFLWeek( "2013", Utility.CurrentWeek() );  //  looking forward
			var sut = new PlayerProjectionGenerator();
			var nGames = 0;
			for ( int i = 0; i < w.GameList().Count; i++ )
			{
				var game = (NFLGame) w.GameList()[ i ];
				sut.Execute( game );
				nGames++;
			}
			Assert.AreEqual( 16, nGames );
		}

		[TestMethod]
		public void TestGettingGamePrediction()
		{
			var msg = new PlayerGameProjectionMessage();
			msg.Game = new NFLGame( "2013:06-M" );
			var sut = new GetGamePrediction( msg );
			Assert.IsNotNull( msg.Prediction );
			Utility.Announce( msg.Prediction.PredictedScore() );
		}

		[TestMethod]
		public void TestPullMetrics()
		{
			var msg = new PlayerGameProjectionMessage();
			msg.Game = new NFLGame("2013:13-M");
			var sut = new GetGamePrediction(msg);
			var sut2 = new PullMetricsFromPrediction(msg);
			Assert.IsNotNull(sut2);
		}

		[TestMethod]
		public void TestAllocationToAce()
		{
			var msg = new PlayerGameProjectionMessage();
			msg.Game = new NFLGame( "2013:01-B" );
			var sut = new GetGamePrediction( msg );
			Assert.IsNotNull( msg.Prediction );
			Utility.Announce( msg.Prediction.PredictedScore() );
			var sut2 = new PullMetricsFromPrediction( msg );
			Assert.IsNotNull( msg.Game.PlayerGameMetrics );
			Assert.AreEqual( 1, msg.Game.PlayerGameMetrics.Count );
		}

		[TestMethod]
		public void TestASavingMetrics()
		{
			var msg = new PlayerGameProjectionMessage();
			msg.Game = new NFLGame( "2013:01-B" );
			var sut = new GetGamePrediction( msg );
			var sut2 = new PullMetricsFromPrediction( msg );
			var sut3 = new SavePlayerGameMetrics( msg );
			var dpgmDoa = new DbfPlayerGameMetricsDao();
			List<PlayerGameMetrics> pgmList = msg.Game.PlayerGameMetrics;
			var expectedPgm = pgmList.FirstOrDefault();
			var pgm = dpgmDoa.Get( expectedPgm.PlayerId, expectedPgm.GameKey );
			Assert.IsNotNull( pgm );
		}

		[TestMethod]
		public void TestAllocation()
		{
			var msg = new PlayerGameProjectionMessage();
			msg.Game = new NFLGame( "2013:01-B" );
			var sut = new GetGamePrediction( msg );
			Assert.IsNotNull( msg.Prediction );
			Utility.Announce( msg.Prediction.PredictedScore() );
			var sut2 = new PullMetricsFromPrediction( msg );
			Assert.IsNotNull( msg.Game.PlayerGameMetrics );
			var sut3 = new SavePlayerGameMetrics(msg);
		}

		[TestMethod]
		public void TestDeleteMetricsWorks()
		{
			Utility.TflWs.ClearPlayerGameMetrics( "2013:01-B" );
			var ds = Utility.TflWs.GetPlayerGameMetrics( "HOPKDU01", "2013:01-B" );
			Assert.IsTrue( ds.Tables[ 0 ].Rows.Count == 0 );
		}

		[TestMethod]
		public void TestInjury()
		{
			var p = new NFLPlayer( "GOREFR01" );  // has injury rating 1
			var injury = PullMetricsFromPrediction.AllowForInjuryRisk( p, 100 );
			Assert.IsTrue( injury == 90 );
		}

		[TestMethod]
		public void TestVulture()
		{
			var msg = new PlayerGameProjectionMessage();
			msg.Game = new NFLGame( "2013:04-I" );
			msg.Game.PlayerGameMetrics = new List<PlayerGameMetrics>();
			var sut = new GetGamePrediction( msg );
			Assert.IsNotNull( msg.Prediction );
			Utility.Announce( msg.Prediction.PredictedScore() );
			var sut2 = new PullMetricsFromPrediction( msg );
			Assert.IsNotNull( msg.Game.PlayerGameMetrics );
			Assert.IsTrue( msg.Game.PlayerGameMetrics.Count > 0 );
		}

	}
}
