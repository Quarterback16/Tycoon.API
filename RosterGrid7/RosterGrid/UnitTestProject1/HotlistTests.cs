using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterGrid;
using RosterLib;
using TFLLib;

namespace RosterGridTests
{
	[TestClass]
	public class HotlistTests
	{
		[TestMethod]
		public void TestG1HotLists()
		{
			var hl = new HotListReporter();
			hl.RunAllHotlists();
		}

		[TestMethod]
		public void TestRunningBackHotList()
		{
			//  this version will show all starters and backups on Playoff Teams
			var hl = new HotListReporter();
			hl.League = Constants.K_LEAGUE_50_Dollar_Challenge;
			hl.HotList( "2", "RB", freeAgentsOnly: true, startersOnly:false );
			Assert.IsTrue( true );
		}

		[TestMethod]
		public void TestWideReceiverHotList()
		{
			var hl = new HotListReporter();
			hl.HotList( "3", "WR", true, true );
			Assert.IsTrue( true );
		}

		[TestMethod]
		public void TestWideReceiverProjectionMalcomFloyd()
		{
			var r = new PlayerProjection( "FLOYMA02", "2011" );
			r.Render();
			Assert.IsTrue( true );
		}

		[TestMethod]
		public void TestTightEndHotList()
		{
			var hl = new HotListReporter();
			hl.HotList( "3", "TE", true, true );
			Assert.IsTrue( true );
		}

		[TestMethod]
		public void TestAllHotLists()
		{
			var hl = new HotListReporter();
			hl.RunAllHotlists();
			hl.League = Constants.K_LEAGUE_50_Dollar_Challenge;
			hl.RunAllHotlists();
			hl.League = Constants.K_LEAGUE_Yahoo;
			hl.RunAllHotlists();
			hl.League = Constants.K_LEAGUE_Gridstats_NFL1;
			hl.RunAllHotlists();
		}

		[TestMethod]
		public void TestAllHotListsForSpitzy()
		{
			var hl = new HotListReporter();
			hl.League = Constants.K_LEAGUE_Yahoo;
			hl.RunAllHotlists();
			Assert.IsTrue( true );
		}


	}
}
