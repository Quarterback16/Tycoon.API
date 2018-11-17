using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterGrid;
using RosterLib;
using TFLLib;

namespace RosterGridTests
{
	[TestClass]
	public class ProjectionsTests
	{
		[TestMethod]
		public void TestProjectionUrl()
		{
			//  this version will show all starters and backups on Playoff Teams
			var p = new NFLPlayer( "MORRAL01" );
			var expectedlink = string.Format( "file:///{0}//2013//PlayerProjections/MORRAL01.htm", Utility.OutputDirectory() );
			var link = p.ProjectionLink( "2013" );
			Assert.AreEqual( expectedlink, link );
		}
	}
}
