using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterGrid;
using RosterLib;
using TFLLib;
using System.IO;
using System.Linq;

namespace RosterGridTests
{
	[TestClass]
	public class FantasyProjectionReportTests
	{
		[TestMethod]
		public void TestRenderAllFantasyProjections()
		{
			var dao = new DbfPlayerGameMetricsDao();  //  Could use a Fake here
			var scorer = new YahooProjectionScorer();  //  Could use a Fake here
			var sut = new FantasyProjectionReport( "2013", Utility.CurrentWeek(), dao, scorer );
			sut.League = Constants.K_LEAGUE_Yahoo;
			sut.RenderAll();
			Assert.IsTrue( File.Exists( sut.FileName() ) );
			sut.League = Constants.K_LEAGUE_50_Dollar_Challenge;
			sut.RenderAll();
			Assert.IsTrue( File.Exists( sut.FileName() ) );
			sut.League = Constants.K_LEAGUE_Rants_n_Raves;
			sut.RenderAll();
			Assert.IsTrue( File.Exists( sut.FileName() ) );
		}

		[TestMethod]
		public void TestRenderTommysRunningbacksProjection()
		{
			var dao = new DbfPlayerGameMetricsDao();  //  Could use a Fake here
			var scorer = new YahooProjectionScorer();  //  Could use a Fake here
			var sut = new FantasyProjectionReport("2013", "6", dao, scorer);
			sut.League = Constants.K_LEAGUE_50_Dollar_Challenge;
			sut.RenderRunningbacks();
			Assert.IsTrue(File.Exists(sut.FileName()));
		}

		[TestMethod]
		public void TestRenderYahooProjection()
		{
			var dao = new DbfPlayerGameMetricsDao();  //  Could use a Fake here
			var scorer = new YahooProjectionScorer();  //  Could use a Fake here
			var sut = new FantasyProjectionReport( "2013", "2", dao, scorer );
			sut.League = Constants.K_LEAGUE_Yahoo;
			sut.RenderAll();
			Assert.IsTrue( File.Exists( sut.FileName() ) );
		}

		[TestMethod]
		public void TestRenderTommysProjection()
		{
			var dao = new DbfPlayerGameMetricsDao();  //  Could use a Fake here
			var scorer = new YahooProjectionScorer();  //  Could use a Fake here
			var sut = new FantasyProjectionReport("2013", "4", dao, scorer);
			sut.League = Constants.K_LEAGUE_50_Dollar_Challenge;
			sut.RenderAll();
			Assert.IsTrue(File.Exists(sut.FileName()));
		}

		[TestMethod]
		public void TestRenderRantsProjection()
		{
			var dao = new DbfPlayerGameMetricsDao();  //  Could use a Fake here
			var scorer = new YahooProjectionScorer();  //  Could use a Fake here
			var sut = new FantasyProjectionReport("2013", "2", dao, scorer);
			sut.League = Constants.K_LEAGUE_Rants_n_Raves;
			sut.RenderAll();
			Assert.IsTrue(File.Exists(sut.FileName()));
		}

		[TestMethod]
		public void TestDaoGetsRightWeek()
		{
			var sut = new DbfPlayerGameMetricsDao();  
			var pgmList = sut.GetWeek( "2013", "02");
			Assert.IsTrue( pgmList.All( p => p.Week() == "02" ) );
		}

		[TestMethod]
		public void TestFileGetsOutput()
		{
			var dao = new DbfPlayerGameMetricsDao();  //  Could use a Fake here
			var scorer = new YahooProjectionScorer();  //  Could use a Fake here
			var sut = new FantasyProjectionReport( "2013", "01", dao, scorer );
			sut.League = Constants.K_LEAGUE_Yahoo;
			sut.Render();
			Assert.IsTrue( File.Exists( sut.FileName() ) );
		}

		[TestMethod]
		public void TestFileGetsOutputForSF()
		{
			var dao = new DbfPlayerGameMetricsDao();  //  Could use a Fake here
			var scorer = new YahooProjectionScorer();  //  Could use a Fake here
			var sut = new FantasyProjectionReport( "2013", "01", dao, scorer );
			sut.TeamFilter = "SF";
			sut.CategoryFilter = "3";
			sut.Render();
			Assert.IsTrue( File.Exists( sut.FileName() ) );
		}

		[TestMethod]
		public void TestFileGetsOutputForWR()
		{
			var dao = new DbfPlayerGameMetricsDao();  //  Could use a Fake here
			var scorer = new YahooProjectionScorer();  //  Could use a Fake here
			var sut = new FantasyProjectionReport( "2013", "1", dao, scorer );
			sut.League = Constants.K_LEAGUE_Yahoo;
			sut.CategoryFilter = "3";
			sut.Render();
			Assert.IsTrue( File.Exists( sut.FileName() ) );
		}

		[TestMethod]
		public void TestFileGetsOutputForBR()
		{
			var dao = new DbfPlayerGameMetricsDao();  //  Could use a Fake here
			var scorer = new YahooProjectionScorer();  //  Could use a Fake here
			var sut = new FantasyProjectionReport( "2013", "1", dao, scorer );
			sut.TeamFilter = "BR";
			sut.League = Constants.K_LEAGUE_Yahoo;
			sut.Render();
			Assert.IsTrue( File.Exists( sut.FileName() ) );
		}

	}
}
