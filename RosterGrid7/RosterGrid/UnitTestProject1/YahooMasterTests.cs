using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterGrid;
using RosterLib;
using TFLLib;
using System.IO;

namespace RosterGridTests
{
	[TestClass]
	public class YahooMasterTests
	{

		[TestMethod]
		public void TestYahooMasterCalculatingForSingleWeek()
		{
			var m = new YahooMaster( "Yahoo", "YahooOutput.xml" );
			m.Calculate( "2013", "13" );
			m.Dump2Xml();
			Assert.IsTrue( File.Exists( m.Filename ) );
		}

		[TestMethod]
		public void TestYahooMasterCalculatingSeasonToDate()
		{
			var m = new YahooMaster( "Yahoo", "YahooOutput.xml" );
			m.Calculate( "2013" );
			m.Dump2Xml();
			Assert.IsTrue( File.Exists( m.Filename ) );
		}


		[TestMethod]
		public void TestYahooMasterGet()
		{
			var m = new YahooMaster( "Yahoo", "YahooOutput.xml" );
			var stat = m.GetStat( "2013:02:MANNPE01" );
			Assert.AreEqual( 19.0M, stat );
		}

		[TestMethod]
		public void TestYahooMasterGetVick2013Week03()
		{
			var m = new YahooMaster( "Yahoo", "YahooOutput.xml" );
			var stat = m.GetStat( "2013:03:VICKMI01" );
			Assert.AreEqual( 17.0M, stat );
		}

		[TestMethod]
		public void TestYahooMasterCalculating()
		{
			var m = new YahooMaster( "Yahoo", "YahooOutput.xml" );
			m.Calculate( "2013", "06" );
			m.Dump2Xml();
			Assert.IsTrue( File.Exists( m.Filename ) );
		}

		[TestMethod]
		public void TestTallyYahooForASingleGame()
		{
			var game = new NFLGame( "2012:04-H" );
			game.TallyYahooFor( game.AwayNflTeam, announceIt: true );
			Assert.AreEqual( 10, game.YahooList.Count );
		}

		[TestMethod]
		public void TestYahooMasterConstructor()
		{
			var m = new YahooMaster( "Yahoo", "YahooOutput.xml" );
			Assert.IsNotNull( m );
		}

		[TestMethod]
		public void TestYahooMasterPersistence()
		{
			var m = new YahooMaster( "Yahoo", "YahooOutput.xml" );
			var stat = new RosterLib.Models.YahooOutput
			{
				Season = "2012",
				Week = "01",
				PlayerId = "GRIFRO01",
				Opponent = "NO",
				Quantity = 30.0M
			};
			m.PutStat( stat );
			m.Dump2Xml();
			Assert.IsTrue( File.Exists( m.Filename ), string.Format( "{0} not found", m.Filename ) );
		}

		[TestMethod]
		public void TestLoadingTheNewOrleansLineupForGame3()
		{
			var game = new NFLGame( "2012:03-G" );
			var lineup = game.LoadPlayers( "NO" );
			Assert.IsTrue( lineup.Count > 22 );
		}

		[TestMethod]
		public void TestEspnScorerAccesstoYahooXml()
		{
			var m = new YahooMaster( "Yahoo", "YahooOutput.xml" );
			var pts = m.GetStat( "2012:04:AKERDA01" );
			Assert.AreEqual( 11, pts, "Akers got 11 pts in week 4 of 2012" );
		}

		[TestMethod]
		public void TestEspnScorer()
		{
			var week = new NFLWeek( seasonIn: 2013, weekIn: 3, loadGames: false );
			var gs = new EspnScorer( week )
			{
				Master = new YahooMaster( "Yahoo", "YahooOutput.xml" )
			};
			var player = new NFLPlayer( "VICKMI01" );
			var pts = gs.RatePlayer( player, week );
			Assert.AreEqual( 11, pts, "Vick got 11 pts in week 3 of 2013" );
		}

	}
}
