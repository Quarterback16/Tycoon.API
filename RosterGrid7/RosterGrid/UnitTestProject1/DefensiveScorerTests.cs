using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterGrid;
using RosterLib;
using TFLLib;
using System.IO;

namespace RosterGridTests
{
	[TestClass]
	public class DefensiveScorerTests
	{
		[TestMethod]
		public void TestDefensivePatsyReport()
		{
			var week = Utility.CurrentNFLWeek();
			var ds = new DefensiveScorer();
			ds.RenderTeamToDefendAgainst( week );
			Assert.IsTrue( File.Exists( ds.FileOut ), string.Format( "Cannot find {0}", ds.FileOut ) );
		}

		[TestMethod]
		public void TestDefensiveScoringReport()
		{
			//  Lists the best Fantasy defence in the last season
			var week = Utility.CurrentNFLWeek();
			var ds = new DefensiveScorer();
			ds.RenderDefensiveScoringReport( week );
			Assert.IsTrue( File.Exists( ds.FileOut ), string.Format( "Cannot find {0}", ds.FileOut ) );
		}

		[TestMethod]
		public void TestPatriotsDefenseCalcs()
		{
			var week = new NFLWeek( 2010, 17 );
			ICalculate myCalculator = new DefensiveScoringCalculator( week, -1 );
			var team = new NflTeam( "NE" );
			team.CalculateDefensiveScoring( myCalculator, doOpponent: false );
			Assert.AreEqual( 17, myCalculator.Team.FantasyPoints );
			Assert.AreEqual( 1, myCalculator.Team.TotInterceptions );
			Assert.AreEqual( 5, myCalculator.Team.TotSacks );
		}

		[TestMethod]
		public void TestDefensivePatsyNextOpponent()
		{
			var team = new NflTeam("JJ");
			var opp = team.NextOpponent(new System.DateTime(2013, 9, 12));
			Assert.AreEqual( "@OR", opp );
		}

		[TestMethod]
		public void TestDefensivePatsyBBReport()
		{
			var team = new NflTeam( "BB" );
			var tl = new TeamLister(team);
			tl.Heading = "Defence Friendly Offences";
			ICalculate ttbCalculator = new DefensiveScoringCalculator( new NFLWeek( 2013, 1 ), -17 );
			var fileOut = tl.RenderTeamToBeat( ttbCalculator );
			Assert.IsTrue( File.Exists( fileOut ), string.Format( "Cannot find {0}", fileOut ) );
		}
	}
}
