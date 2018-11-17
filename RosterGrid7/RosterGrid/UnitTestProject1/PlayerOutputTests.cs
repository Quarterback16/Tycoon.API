using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterGrid;
using RosterLib;
using TFLLib;

namespace RosterGridTests
{
	[TestClass]
	public class PlayerOutputTests
	{
		[TestMethod]
		public void PlayerOutputRBTest()
		{
			//SeasonFreqTableGs4("Kickers", Constants.K_SCORE_FIELD_GOAL, "1", scoresOnly: false);
			//SeasonFreqTableGs4("QBs Scores", Constants.K_SCORE_TD_PASS, "2", scoresOnly: true);
			//SeasonFreqTableGs4("RBs Scores", Constants.K_SCORE_TD_RUN, "1", scoresOnly: true);
			//SeasonFreqTableGs4("WRs Scores", Constants.K_SCORE_TD_PASS, "1", scoresOnly: true);
			var po = new PlayerOutput
			         	{
			         		PlayerType = "RBs Scores",
								ScoreType = Constants.K_SCORE_TD_RUN,
								ScoreSlot = "1",
								wRange = { startWeek = new NFLWeek(2012, 1), endWeek = new NFLWeek(2012, 17) },
			         		Scorer = new GS4Scorer(Utility.CurrentNFLWeek()) {ScoresOnly = true}
			         	};
			po.Load();
			po.Render(po.PlayerType, po.wRange.startWeek.Season );

			Assert.IsTrue(true);
		}
	}
}
