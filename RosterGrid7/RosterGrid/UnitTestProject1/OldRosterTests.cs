using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterGrid;
using RosterLib;
using System.IO;
using TFLLib;

namespace RosterGridTests
{
	[TestClass]
	public class OldRosterTests
	{
		[TestMethod]
		public void TestOldRostersTN()
		{
			TestOldRostersTN_QB();
			TestOldRostersTN_RB();
			TestOldRostersTN_WR();
			TestOldRostersTN_TE();
			TestOldRostersTN_PK();
		}

		[TestMethod]
		public void TestOldRostersTN_QB()
		{
			Utility.CurrentLeague = Constants.K_LEAGUE_50_Dollar_Challenge;
			var rg = new NFLRosterGrid(Constants.K_QUARTERBACK_CAT) { Focus = "QB" };
			rg.CurrentRoster();
			Assert.IsTrue(File.Exists(rg.FileOut ));
		}

		[TestMethod]
		public void TestOldRostersTN_RB()
		{
			Utility.CurrentLeague = Constants.K_LEAGUE_50_Dollar_Challenge;
			var rg = new NFLRosterGrid(Constants.K_RUNNINGBACK_CAT) { Focus = "RB" };
			rg.CurrentRoster();
			Assert.IsTrue(File.Exists(rg.FileOut));
		}

		[TestMethod]
		public void TestOldRostersTN_WR()
		{
			Utility.CurrentLeague = Constants.K_LEAGUE_50_Dollar_Challenge;
			var rg = new NFLRosterGrid(Constants.K_RECEIVER_CAT) { Focus = "WR" };
			rg.CurrentRoster();
			Assert.IsTrue(File.Exists(rg.FileOut));
		}

		[TestMethod]
		public void TestOldRostersTN_TE()
		{
			Utility.CurrentLeague = Constants.K_LEAGUE_50_Dollar_Challenge;
			var rg = new NFLRosterGrid(Constants.K_RECEIVER_CAT) { Focus = "TE" };
			rg.CurrentRoster();
			Assert.IsTrue(File.Exists(rg.FileOut));
		}

		[TestMethod]
		public void TestOldRostersTN_PK()
		{
			Utility.CurrentLeague = Constants.K_LEAGUE_50_Dollar_Challenge;
			var rg = new NFLRosterGrid(Constants.K_KICKER_CAT) { Focus = "K" };
			rg.CurrentRoster();
			Assert.IsTrue(File.Exists(rg.FileOut));
		}
	}
}
