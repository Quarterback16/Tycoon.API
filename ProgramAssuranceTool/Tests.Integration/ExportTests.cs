using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool;

namespace Tests.Integration
{
	[TestClass]
	public class ExportTests
	{
		[TestMethod]
		public void TestReviewExport()
		{
			var ps = new PatService();
			var sut = new ReviewExporter();
			var list = ps.GetProjectReviews( 17, AppHelper.DefaultGridSettings() );
			var csv = sut.ExportReviews( list, ps );
			Assert.IsTrue( csv.Length > 0 );
		}
	}
}
