using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterGrid;
using RosterLib;
using TFLLib;

namespace RosterGridTests
{
	[TestClass]
	public class RushUnitTest
	{

		[TestMethod]
		public void TestAllRushingUnits2013()
		{
			var errors = 0;
			var s = new NflSeason("2013", true);
			foreach (var t in s.TeamList)
			{
				t.LoadRushUnit();
				Utility.Announce(string.Format("Loaded {0} runners for {1}", t.RushUnit.Runners.Count, t.Name));
				if (t.RushUnit.AceBack != null)
					Utility.Announce(string.Format("Ace Back is {0}", t.RushUnit.AceBack.PlayerName));
				if (t.RushUnit.GoalLineBack != null)
					Utility.Announce(string.Format("GoalLine Back is {0}", t.RushUnit.GoalLineBack.PlayerName));
				else
					Utility.Announce("GoalLine Back is missing");

				if (t.RushUnit.HasIntegrityError())
				{
					errors++;
					Utility.Announce(string.Format("Need to fix {0}", t.Name));
				}
			}
			Assert.AreEqual(0, errors);
		}

		[TestMethod]
		public void TestPlayerRushingUnitGetsLoaded()
		{
			var sut = new NflTeam( "AC" );
			sut.LoadRushUnit();
			Assert.IsTrue( sut.RushUnit.Runners.Count > 0 );
			Assert.IsTrue( sut.RushUnit.Runners.Count < 10, string.Format("{0} runners", sut.RushUnit.Runners.Count ) );
			Utility.Announce( string.Format( "Loaded {0} runners", sut.RushUnit.Runners.Count ) );
			Assert.IsFalse( sut.RushUnit.HasIntegrityError() );
			Utility.Announce( "No integrity errors" );
		}

		[TestMethod]
		public void TestJJPlayerRushingUnitGetsLoaded()
		{
			var sut = new NflTeam( "JJ" );
			sut.LoadRushUnit();
			Assert.IsTrue( sut.RushUnit.Runners.Count > 0 );
			Assert.IsTrue( sut.RushUnit.Runners.Count < 10, string.Format( "{0} runners", sut.RushUnit.Runners.Count ) );
			Utility.Announce( string.Format( "Loaded {0} runners", sut.RushUnit.Runners.Count ) );
			Assert.IsFalse( sut.RushUnit.HasIntegrityError() );
			sut.RushUnit.DumpUnit();
			Utility.Announce( "No integrity errors" );
		}


		[TestMethod]
		public void TestRushingUnitContainsGoallineBack()
		{
			var sut = new NflTeam( "SF" );
			sut.LoadRushUnit();
			Assert.IsNotNull( sut.RushUnit.GoalLineBack );
			Utility.Announce( string.Format( "GoalLine Back is {0}", sut.RushUnit.GoalLineBack.PlayerName ) );
		}

		[TestMethod]
		public void TestRushingUnitIsAce()
		{
			var sut = new RushUnit();
			sut.Load( "SF" );
			Assert.IsNotNull( sut.AceBack );
			Utility.Announce( string.Format( "Ace Back is {0}", sut.AceBack.PlayerName ) );
		}


		[TestMethod]
		public void TestRushingUnitIsRBBC()
		{
			var sut = new RushUnit();
			sut.Load( "PS" );
			Assert.IsNull( sut.AceBack );
			Utility.Announce( string.Format( "Back 1 is {0}", sut.R1.PlayerName ) );
			Utility.Announce( string.Format( "Back 2 is {0}", sut.R2.PlayerName ) );
		}

	}
}
