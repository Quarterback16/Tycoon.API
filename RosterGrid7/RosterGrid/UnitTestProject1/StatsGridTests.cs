using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterGrid;
using RosterLib;
using TFLLib;
using System.IO;

namespace RosterGridTests
{
	[TestClass]
	public class StatsGridTests
	{

		[TestMethod]
		public void TestAllTheStatsGrids()
		{
			AllStats("2012");
		}

		private static void AllStats( string season )
		{
			StatGridFor( season, "Sacks" );
			StatGridFor( season, "YDr" );
			StatGridFor( season, "YDp" );
			StatGridFor( season, "TDp" );
			StatGridFor( season, "TDr" );
			StatGridFor( season, "TDrAllowed" );
			StatGridFor( season, "Sacks" );
			StatGridFor( season, "SacksAllowed" );
			StatGridFor( season, "YDrAllowed" );
			StatGridFor( season, "YDpAllowed" );
			StatGridFor( season, "TDpAllowed" );
			StatGridFor( season, "INTs" );
			StatGridFor( season, "INTsThrown" );
		}

		private static void StatGridFor( string season, string statType )
		{
			var sg = new StatGrid( season, statType );
			sg.Render();
			Assert.IsTrue( File.Exists( sg.FileName() ), string.Format( "Cannot find {0}", sg.FileName() ) );
		}

		[TestMethod]
		public void TestStatsGridForSacks()
		{
			const string season = "2012";
			var sg = new StatGrid( season, "Sacks" );
			sg.Render();
			Assert.IsTrue( File.Exists( sg.FileName() ), string.Format( "Cannot find {0}", sg.FileName() ) );
		}

		[TestMethod]
		public void TestStatsGridForInts()
		{
			const string season = "2012";
			var sg = new StatGrid( season, "INTs" );
			sg.Render();
			Assert.IsTrue( File.Exists( sg.FileName() ), string.Format( "Cannot find {0}", sg.FileName() ) );
		}

		[TestMethod]
		public void TestStatsGridForIntsThrown()
		{
			const string season = "2012";
			var sg = new StatGrid( season, "INTsThrown" );
			sg.Render();
			Assert.IsTrue( File.Exists( sg.FileName() ), string.Format( "Cannot find {0}", sg.FileName() ) );
		}

		[TestMethod]
		public void TestStatsGridForSacksAllowed()
		{
			const string season = "2012";
			var sg = new StatGrid( season, "SacksAllowed" );
			sg.Render();
			Assert.IsTrue( File.Exists( sg.FileName() ), string.Format( "Cannot find {0}", sg.FileName() ) );
		}

		[TestMethod]
		public void TestStatsGridForTDpasses()
		{
			const string season = "2012";
			var sg = new StatGrid( season, "TDp" );
			sg.Render();
			Assert.IsTrue( File.Exists( sg.FileName() ), string.Format( "Cannot find {0}", sg.FileName() ) );
		}

		[TestMethod]
		public void TestStatsGridForTDpassesAllowed()
		{
			const string season = "2012";
			var sg = new StatGrid( season, "TDpAllowed" );
			sg.Render();
			Assert.IsTrue( File.Exists( sg.FileName() ), string.Format( "Cannot find {0}", sg.FileName() ) );
		}

		[TestMethod]
		public void TestStatsGridForTDruns()
		{
			const string season = "2012";
			var sg = new StatGrid( season, "TDr" );
			sg.Render();
			Assert.IsTrue( File.Exists( sg.FileName() ), string.Format( "Cannot find {0}", sg.FileName() ) );
		}

		[TestMethod]
		public void TestStatsGridForYDp()
		{
			const string season = "2012";
			var sg = new StatGrid( season, "YDp" );
			sg.Render();
			Assert.IsTrue( File.Exists( sg.FileName() ), string.Format( "Cannot find {0}", sg.FileName() ) );
		}

		[TestMethod]
		public void TestStatsGridForYDpAllowed()
		{
			const string season = "2012";
			var sg = new StatGrid( season, "YDpAllowed" );
			sg.Render();
			Assert.IsTrue( File.Exists( sg.FileName() ), string.Format( "Cannot find {0}", sg.FileName() ) );
		}

		[TestMethod]
		public void TestStatsGridForYDr()
		{
			const string season = "2012";
			var sg = new StatGrid( season, "YDr" );
			sg.Render();
			Assert.IsTrue( File.Exists( sg.FileName() ), string.Format( "Cannot find {0}", sg.FileName() ) );
		}

		[TestMethod]
		public void TestStatsGridForTDrAllowed()
		{
			const string season = "2012";
			var sg = new StatGrid( season, "TDrAllowed" );
			sg.Render();
			Assert.IsTrue( File.Exists( sg.FileName() ), string.Format( "Cannot find {0}", sg.FileName() ) );
		}

		[TestMethod]
		public void TestStatsGridForYDrAllowed()
		{
			const string season = "2012";
			var sg = new StatGrid( season, "YDrAllowed" );
			sg.Render();
			Assert.IsTrue( File.Exists( sg.FileName() ), string.Format( "Cannot find {0}", sg.FileName() ) );
		}

	}
}
