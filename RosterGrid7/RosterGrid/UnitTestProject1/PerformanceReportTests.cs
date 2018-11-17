using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterGrid;
using RosterLib;
using TFLLib;
using System.IO;

namespace RosterGridTests
{
	[TestClass]
	public class PerformanceReportTests
	{
		[TestMethod]
		public void TestRosterGridPerfomanceReport()
		{
			var master = new YahooMaster( "Yahoo", "YahooOutput.xml" );
			var cat = Constants.K_RUNNINGBACK_CAT;
			RosterGrid.RosterGrid.GenericEspnPerformance( cat, master, Constants.K_LEAGUE_Rants_n_Raves, "RB" );
		}

		[TestMethod]
		public void TestEspnPerformanceReportQuarterbacksYahooLeague()
		{
			var pr = new PerformanceReport( season: "2013", week: 3 );
			var week = new NFLWeek( seasonIn: 2013, weekIn: 3, loadGames: false );
			var gs = new EspnScorer( week ) { ScoresOnly = true };
			pr.Scorer = gs;
			pr.Render( catCode: "1", sPos: "QB", leagueId: Constants.K_LEAGUE_Yahoo, startersOnly: true );
			Assert.IsTrue( File.Exists( pr.FileOut ), string.Format( "Cannot find {0}", pr.FileOut ) );
		}

		[TestMethod]
		public void TestEspnPerformanceReportWideReceiversWeek5()
		{
			var pr = new PerformanceReport( season: "2012", week: 5 );
			var week = new NFLWeek( seasonIn: 2012, weekIn: 5, loadGames: false );
			var gs = new EspnScorer( week ) { ScoresOnly = true };
			pr.Scorer = gs;
			pr.Render( catCode: "3", sPos: "WR", leagueId: Constants.K_LEAGUE_Yahoo, startersOnly: true );
			Assert.IsTrue( File.Exists( pr.FileOut ), string.Format( "Cannot find {0}", pr.FileOut ) );
		}

		[TestMethod]
		public void TestEspnPerformanceReportTightEndsWeek5()
		{
			var pr = new PerformanceReport( season: "2012", week: 5 );
			var week = new NFLWeek( seasonIn: 2012, weekIn: 5, loadGames: false );
			var gs = new EspnScorer( week ) { ScoresOnly = true };
			pr.Scorer = gs;
			pr.Render( catCode: "3", sPos: "TE", leagueId: Constants.K_LEAGUE_Yahoo, startersOnly: true );
			Assert.IsTrue( File.Exists( pr.FileOut ), string.Format( "Cannot find {0}", pr.FileOut ) );
		}

		[TestMethod]
		public void TestEspnPerformanceReportKickersWeek5()
		{
			var pr = new PerformanceReport( season: "2012", week: 5 );
			var week = new NFLWeek( seasonIn: 2012, weekIn: 5, loadGames: false );
			var gs = new EspnScorer( week ) { ScoresOnly = true };
			pr.Scorer = gs;
			pr.Render( catCode: "4", sPos: "PK", leagueId: Constants.K_LEAGUE_Yahoo, startersOnly: true );
			Assert.IsTrue( File.Exists( pr.FileOut ), string.Format( "Cannot find {0}", pr.FileOut ) );
		}

		[TestMethod]
		public void TestEspnPerformanceReportRunningBacksWeek5()
		{
			var pr = new PerformanceReport( season: "2012", week: 5 );
			var week = new NFLWeek( seasonIn: 2012, weekIn: 5, loadGames: false );
			var gs = new EspnScorer( week ) { ScoresOnly = true };
			pr.Scorer = gs;
			pr.Render( catCode: "2", sPos: "RB", leagueId: Constants.K_LEAGUE_Yahoo, startersOnly: true );
			Assert.IsTrue( File.Exists( pr.FileOut ), string.Format( "Cannot find {0}", pr.FileOut ) );
		}

		[TestMethod]
		public void TestGsPerformanceReportRunningBacksWeek12()
		{
			var pr = new PerformanceReport( season: "2012", week: 12 );
			var week = new NFLWeek( seasonIn: 2012, weekIn: 12, loadGames: false );
			var gs = new GS4Scorer( week ) { ScoresOnly = true, Master = new GridStatsMaster( "GridStats", "GridStats.xml" ) };
			pr.Scorer = gs;
			pr.Render( catCode: "2", sPos: "RB", leagueId: Constants.K_LEAGUE_Gridstats_NFL1, startersOnly: true );
			Assert.IsTrue( File.Exists( pr.FileOut ), string.Format( "Cannot find {0}", pr.FileOut ) );
		}



		[TestMethod]
		public void TestYahooPerformanceUptoCurrent()
		{
			var pl = new PlayerLister();
			var theWeek = new NFLWeek( seasonIn: 2013, weekIn: System.Int32.Parse( Utility.CurrentWeek() ), loadGames: false );
			var gs = new EspnScorer( theWeek );
			var leagueId = Constants.K_LEAGUE_Yahoo;
			var rptPos = "RB";

			pl.SetScorer( gs );
			pl.StartersOnly = true; // more thorough to do false
			pl.SetFormat( "weekly" );  //  as opposed to "annual" totals
			pl.AllWeeks = false; //  just the regular season
			pl.Season = theWeek.Season;
			pl.Collect( catCode: Constants.K_RUNNINGBACK_CAT, sPos: rptPos, fantasyLeague: leagueId );
			var targetFile = string.Format( "{2}{0}//Performance//Yahoo {1} Performance {0}-{3}.htm", pl.Season, rptPos, Utility.OutputDirectory(), leagueId );
			pl.Render( targetFile );
			Assert.IsTrue( File.Exists( pl.FileOut ), string.Format( "Cannot find {0}", pl.FileOut ) );
		}

		[TestMethod]
		public void TestTommyHawksPerformanceUptoCurrent()
		{
			var pl = new PlayerLister();
			var theWeek = new NFLWeek( seasonIn: 2013, weekIn: System.Int32.Parse( Utility.CurrentWeek() ), loadGames: false );
			var gs = new EspnScorer( theWeek );

			var leagueId = Constants.K_LEAGUE_50_Dollar_Challenge;

			DoRantsReport( pl, theWeek, gs, leagueId, "QB", Constants.K_QUARTERBACK_CAT );
			DoRantsReport( pl, theWeek, gs, leagueId, "RB", Constants.K_RUNNINGBACK_CAT );
			DoRantsReport( pl, theWeek, gs, leagueId, "WR", Constants.K_RECEIVER_CAT );
			DoRantsReport( pl, theWeek, gs, leagueId, "TE", Constants.K_RECEIVER_CAT );
			DoRantsReport( pl, theWeek, gs, leagueId, "PK", Constants.K_KICKER_CAT );
		}


		[TestMethod]
		public void TestSpitzyPerformanceUptoCurrent()
		{
			var pl = new PlayerLister();
			var theWeek = new NFLWeek( seasonIn: 2013, weekIn: System.Int32.Parse( Utility.CurrentWeek() ), loadGames: false );
			var gs = new EspnScorer( theWeek );

			var leagueId = Constants.K_LEAGUE_Yahoo;

			DoRantsReport( pl, theWeek, gs, leagueId, "QB", Constants.K_QUARTERBACK_CAT );
			DoRantsReport( pl, theWeek, gs, leagueId, "RB", Constants.K_RUNNINGBACK_CAT );
			DoRantsReport( pl, theWeek, gs, leagueId, "WR", Constants.K_RECEIVER_CAT );
			DoRantsReport( pl, theWeek, gs, leagueId, "TE", Constants.K_RECEIVER_CAT );
			DoRantsReport( pl, theWeek, gs, leagueId, "PK", Constants.K_KICKER_CAT );
		}

		[TestMethod]
		public void TestRantsPerformanceUptoCurrent()
		{
			var pl = new PlayerLister();
			var theWeek = new NFLWeek( seasonIn: 2013, weekIn: System.Int32.Parse( Utility.CurrentWeek() ), loadGames: false );
			var gs = new EspnScorer( theWeek );

			var leagueId = Constants.K_LEAGUE_Rants_n_Raves;

			DoRantsReport( pl, theWeek, gs, leagueId, "QB", Constants.K_QUARTERBACK_CAT );
			DoRantsReport( pl, theWeek, gs, leagueId, "RB", Constants.K_RUNNINGBACK_CAT );
			DoRantsReport( pl, theWeek, gs, leagueId, "WR", Constants.K_RECEIVER_CAT );
			DoRantsReport( pl, theWeek, gs, leagueId, "TE", Constants.K_RECEIVER_CAT );
			DoRantsReport( pl, theWeek, gs, leagueId, "PK", Constants.K_KICKER_CAT );
		}

		private static void DoRantsReport( PlayerLister pl, NFLWeek theWeek, EspnScorer gs, string leagueId, string rptPos, string cat )
		{
			pl.SetScorer( gs );
			pl.StartersOnly = true; // more thorough to do false
			pl.SetFormat( "weekly" );  //  as opposed to "annual" totals
			pl.AllWeeks = false; //  just the regular season
			pl.Season = theWeek.Season;
			pl.Collect( catCode: cat, sPos: rptPos, fantasyLeague: leagueId );
			var targetFile = string.Format( "{2}{0}//Performance//{3}-Yahoo {1} Performance {0}.htm", pl.Season, rptPos, Utility.OutputDirectory(), leagueId );
			pl.Render( targetFile );
			Assert.IsTrue( File.Exists( pl.FileOut ), string.Format( "Cannot find {0}", pl.FileOut ) );
		}

	}
}
