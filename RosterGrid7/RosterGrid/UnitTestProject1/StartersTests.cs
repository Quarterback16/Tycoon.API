using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using RosterLib.Models;
using TFLLib;
using System.IO;

namespace RosterGridTests
{
	[TestClass]
	public class StartersTests
	{
		[TestMethod]
		public void TestAllStarters()
		{
			var s = new Starters { PlayoffsOnly = false, Lister = { RenderToCsv = true } };
			s.AllStarters( Constants.K_LEAGUE_Gridstats_NFL1 );
			Assert.IsTrue( true );
		}

		[TestMethod]
		public void TestRunningBackStarters()
		{
			var s = new Starters { PlayoffsOnly = false, Lister = { RenderToCsv = true } };
			var fileOut = s.RenderStarters( Constants.K_RUNNINGBACK_CAT, "RB", Constants.K_LEAGUE_Gridstats_NFL1 );
			Assert.IsTrue( File.Exists( fileOut ), string.Format( "Cannot find {0}", fileOut ) );
		}

		[TestMethod]
		public void TestWideoutStarters()
		{
			var s = new Starters { PlayoffsOnly = false, Lister = { RenderToCsv = true } };
			var fileOut = s.RenderStarters( Constants.K_RECEIVER_CAT, "WR", Constants.K_LEAGUE_Gridstats_NFL1 );
			Assert.IsTrue( File.Exists( fileOut ), string.Format( "Cannot find {0}", fileOut ) );
		}

		[TestMethod]
		public void TestTightEndStarters()
		{
			var s = new Starters();
			var fileOut = s.RenderStarters( Constants.K_RECEIVER_CAT, "TE", Constants.K_LEAGUE_Gridstats_NFL1 );
			Assert.IsTrue( File.Exists( fileOut ), string.Format( "Cannot find {0}", fileOut ) );
		}

		[TestMethod]
		public void TestKickerStarters()
		{
			var s = new Starters();
			var fileOut = s.RenderStarters( Constants.K_KICKER_CAT, "K", Constants.K_LEAGUE_Gridstats_NFL1 );
			Assert.IsTrue( File.Exists( fileOut ), string.Format( "Cannot find {0}", fileOut ) );
		}



	}
}
