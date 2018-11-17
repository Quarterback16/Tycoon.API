using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterGrid;
using RosterLib;
using TFLLib;

namespace RosterGridTests
{
	[TestClass]
	public class SimplePreReportTests
	{
		[TestMethod]
		public void SimplePreReportConstructorTest()
		{
			var sut = new SimplePreReport();
			Assert.IsNotNull( sut );
		}

		[TestMethod]
		public void SimplePreReportFileOutTest()
		{
			var sut = new SimplePreReport();
			sut.Season = "2013";
			sut.Folder = "DepthCharts";
			sut.ReportType = "Depth Chart";
			sut.InstanceName = "SF";
			sut.RenderHtml();
			Assert.AreEqual( string.Format( "{0}2013//DepthCharts//SF.htm", Utility.OutputDirectory() ), sut.FileOut );
		}
	}
}
