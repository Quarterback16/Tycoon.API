using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterGrid;
using RosterLib;
using TFLLib;
using System.IO;

namespace RosterGridTests
{
	[TestClass]
	public class DepthChartTests
	{
		[TestMethod]
		public void TestAllDepthCharts2013()
		{
			var errors = 0;
			var errorTeams = string.Empty;
			var s = new NflSeason( "2013", true );
			foreach ( var t in s.TeamList )
			{
				var isError = false;
				var sut = new DepthChartReport( "2013", t.TeamCode );
				sut.Execute();
				if ( sut.HasIntegrityError() )
				{
					isError = true;
					sut.DumpErrors();
					Utility.Announce( string.Format( "   Need to fix Depth Chart {0}", t.Name ) );
				}
				t.LoadRushUnit();
				if (t.RushUnit.HasIntegrityError())
				{
					isError = true;
					t.RushUnit.DumpUnit();
					Utility.Announce( string.Format( "   Need to fix  Rushing Unit {0}", t.Name ) );
				}
				t.LoadPassUnit();
				if (t.PassUnit.HasIntegrityError())
				{
					isError = true;
					t.PassUnit.DumpUnit();
					Utility.Announce( string.Format( "   Need to fix  Passing Unit {0}", t.Name ) );
				}
				if ( isError )
				{
					errorTeams += t.TeamCode + ",";
					errors++;
				}
			}
			Utility.Announce( "   -------------------------------------------------" );
			Utility.Announce( string.Format( "   There are {0} broken teams - {1}", errors, errorTeams ) );
			Utility.Announce("   -------------------------------------------------");
			Assert.AreEqual(0, errors);
		}

		[TestMethod]
		public void TestDepthChartExecution()
		{
         var teamCode = "MV";
         var t = new NflTeam(teamCode);
         var sut = new DepthChartReport("2013", teamCode);
			sut.Execute();
			var isError = false;
			if ( sut.HasIntegrityError() )
			{
				isError = true;
				sut.DumpErrors();
				Utility.Announce( string.Format( "   Need to fix Depth Chart {0}", t.Name ) );
			}
			t.LoadRushUnit();
			if ( t.RushUnit.HasIntegrityError() )
			{
				isError = true;
				t.RushUnit.DumpUnit();
				Utility.Announce( string.Format( "   Need to fix  Rushing Unit {0}", t.Name ) );
			}
			t.LoadPassUnit();
			if ( t.PassUnit.HasIntegrityError() )
			{
				isError = true;
				t.PassUnit.DumpUnit();
				Utility.Announce( string.Format( "   Need to fix  Passing Unit {0}", t.Name ) );
			}
			Assert.IsFalse( isError );
		}

		[TestMethod]
		public void TestDepthChartConstructor()
		{
			var sut = new DepthChartReport();
			Assert.IsNotNull( sut );
		}

		[TestMethod]
		public void TestDepthChartLoadsStarters()
		{
			var sut = new DepthChartReport( "2013", "SF" );
			sut.Execute();
			Assert.IsTrue( sut.PlayerCount > 0 );
		}

		[TestMethod]
		public void TestMoranNorrisIsNotStarter()
		{
			var role = "?";
			NFLPlayer player;
			var sut = new DepthChartReport( "2013", "SF" );
			sut.Execute();
			foreach ( var p in sut.NflTeam.PlayerList )
			{
				player = (NFLPlayer) p;
				if ( p.ToString() == "Moran Norris" )
				{
					role = player.PlayerRole;
					break;
				}
			} 
			Assert.AreNotEqual( "S", role );
		}

		[TestMethod]
		public void TestDepthChartRatingsOut()
		{
			var sut = new NflTeam( "SF" );
			sut.Ratings = "CBEAAB";
			var spreadRatings = sut.RatingsOut();
			Assert.AreEqual( "C B E - A A B : 39", spreadRatings );
		}

		[TestMethod]
		public void TestSpreadOutRatings()
		{
			var sut = new NflTeam( "SF" );
			sut.Ratings = "ABCDEF";
			var spreadRatings = sut.SpreadoutRatings();
			Assert.AreEqual( "A B C - D E F", spreadRatings );
		}

		[TestMethod]
		public void TestSpreadOutRatingsError()
		{
			var sut = new NflTeam( "SF" );
			sut.Ratings = "ABC";
			var spreadRatings = sut.SpreadoutRatings();
			Assert.AreEqual( "??????", spreadRatings );
		}

		[TestMethod]
		public void TestPoRatings()
		{
			var sut = new NflTeam( "SF" );
			sut.Ratings = "ABCDEF";
			var rating = sut.PoRating();
			Assert.AreEqual( "A", rating );
		}

        [TestMethod]
        public void TestOldDate()
        {
            var sTo = System.DateTime.Parse("14/12/2013").ToString("dd/MM/yyyy");
            var dTo = sTo == "30/12/1899" ? System.DateTime.Now : System.DateTime.Parse(sTo);
            Assert.AreEqual(dTo, new System.DateTime(14, 12, 2014));
        }
	}
}
