using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterGrid;
using RosterLib;
using TFLLib;
using System.IO;

namespace RosterGridTests
{
	[TestClass]
	public class FrequencyTableTests
	{
		[TestMethod]
		public void TestFrequencyTables()
		{
			var po = new PlayerOutput
			{
				PlayerType = "RBs Scores",
				ScoreType = Constants.K_SCORE_TD_RUN,
				ScoreSlot = "1",
				wRange = { startWeek = new NFLWeek( 2012, 1 ), endWeek = new NFLWeek( 2012, 17 ) },
				Scorer = new GS4Scorer( Utility.CurrentNFLWeek() ) { ScoresOnly = true }
			};
			po.Load();
			po.Render( po.PlayerType, po.wRange.startWeek.Season );
			var fileOut = po.FileName;
			Assert.IsTrue( File.Exists( fileOut ), string.Format( "Cannot find {0}", fileOut ) );
		}
	}
}
