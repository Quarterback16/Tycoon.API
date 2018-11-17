using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterGrid;
using RosterLib;
using TFLLib;
using System.IO;

namespace RosterGridTests
{
	[TestClass]
	public class PlayerProjectionTests
	{
		[TestMethod]
		public void TestPlayerProjectionTomBrady2013()
		{
			var pp = new PlayerProjection( "BRADTO01", "2013" );
			pp.Render();
			var fileOut = pp.FileName();
			Assert.IsTrue( File.Exists( fileOut ), string.Format( "Cannot find {0}", fileOut ) );
		}

		[TestMethod]
		public void TestYahooProjectedPointsFrTomBrady2013Week01()
		{
			var p = new NFLPlayer( "BRADTO01" );
			var g = new NFLGame( "2013:01-B" );
			var c = new YahooCalculator();
			var msg = c.Calculate( p, g );
			var expected = 26;
			Assert.AreEqual( expected, msg.Player.Points );
		}

		[TestMethod]
		public void TestPlayerProjectionPrediction()
		{
			var g = new NFLGame( "2013:01-A" );  // YYYY:0W-X
			var prediction = g.GetPrediction( "unit" );
			Assert.IsNotNull( prediction );
			Assert.AreEqual( 0, prediction.AwayTDr, "Away TDr should be 0" );
			Assert.AreEqual( 1, prediction.AwayTDp, "Away TDp should be 1" );
			Assert.AreEqual( 1, prediction.AwayFg, "Away FG should be 1" );
		}

	}
}
