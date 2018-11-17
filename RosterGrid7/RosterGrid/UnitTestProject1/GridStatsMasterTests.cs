using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterGrid;
using RosterLib;
using TFLLib;
using System.IO;

namespace RosterGridTests
{
	[TestClass]
	public class GridStatsMasterTests
	{
		/// <summary>
		///   Player Performance reports with scores saved in XML
		/// </summary>
		[TestMethod]
		public void TestGridStatsMasterCalculatingSeasonToDate()
		{
			var m = new GridStatsMaster( "GridStats", "GridStatsOutput.xml" );
			m.Calculate( "2012" );
			m.Dump2Xml();
			Assert.IsTrue( File.Exists( m.Filename ) );
		}

		[TestMethod]
		public void TestGridStatsMasterConstructor()
		{
			var m = new GridStatsMaster( "GridStats", "GridStatsOutput.xml" );
			Assert.IsNotNull( m );
		}

		[TestMethod]
		public void TestGridStatsMasterCalculating()
		{
			var m = new GridStatsMaster( "GridStats", "GridStatsOutput.xml" );
			m.Calculate( "2012", "07" );
			m.Dump2Xml();
			Assert.IsTrue( File.Exists( m.Filename ) );
		}



	}
}
