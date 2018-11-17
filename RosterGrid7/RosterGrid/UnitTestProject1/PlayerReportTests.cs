using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterGrid;
using RosterLib;
using TFLLib;

namespace RosterGridTests
{
	[TestClass]
	public class PlayerReportTests
	{

		[TestMethod]
		public void TestPlayerReportForChrisJohnson()
		{
			var player = new NFLPlayer( "JOHNCH06" );
			Assert.IsNotNull( player );
			player.PlayerReport( forceIt: true );
		}

		[TestMethod]
		public void TestPlayerReportForDanarioAlexander()
		{
			var player = new NFLPlayer( "ALEXDA03" );
			Assert.IsNotNull( player );
			player.PlayerReport( forceIt: true );
		}

		[TestMethod]
		public void TestPlayerLoadPerfForDanarioAlexander()
		{
			var player = new NFLPlayer( "ALEXDA03" );
			Assert.IsNotNull( player );
			player.LoadPerformances( true, false, "2012" );
			var nGames = player.PerformanceList.Count;
			Assert.AreEqual( 60, nGames );

		}

		[TestMethod]
		public void TestPlayedForDanarioAlexander()
		{
			var teamCode = Utility.TflWs.PlayedFor( "ALEXDA03", 2012, 10 );
			Assert.AreEqual( "SD", teamCode );
		}

		[TestMethod]
		public void TestPlayedForTomBrady()
		{
			var teamCode = Utility.TflWs.PlayedFor( "BRADTO01", 2012, 11 );
			Assert.AreEqual( "NE", teamCode );
		}

		[TestMethod]
		public void TestRBForNiners()
		{
			var ds = Utility.TflWs.GetPlayer( "SF", "2", "*", "RB" );
			Assert.IsTrue( ds.Tables[ 0 ].Rows.Count > 3 );
		}

		[TestMethod]
		public void TestFBForNiners()
		{
			var ds = Utility.TflWs.GetPlayer( "SF", "2", "*", "FB" );
			Assert.IsTrue( ds.Tables[ 0 ].Rows.Count == 1 );
		}

		[TestMethod]
		public void TestPlayedForBrianUrlacher()
		{
			var teamCode = Utility.TflWs.PlayedFor( "URLABR01", 2012, 16 );
			Assert.AreEqual( "CH", teamCode );
		}

		[TestMethod]
		public void TestPlayedForJoeMontana()
		{
			var teamCode = Utility.TflWs.PlayedFor( "MONTJO01", 2012, 16 );
			Assert.AreEqual( "", teamCode );
		}

	}
}
