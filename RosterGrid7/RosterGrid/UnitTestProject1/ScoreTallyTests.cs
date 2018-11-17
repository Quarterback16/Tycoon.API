using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterGrid;
using RosterLib;
using TFLLib;
using System.IO;

namespace RosterGridTests
{
	[TestClass]
	public class ScoreTallyTests
	{
		[TestMethod]
		public void TestPredictedOutput()
		{
			var sut = new ScoreTally( "2013", "All Teams", true );
			sut.Render();
			var fileOut = sut.FileName();
			Assert.IsTrue( File.Exists( fileOut ), string.Format( "Cannot find {0}", fileOut ) );
		}

		[TestMethod]
		public void TestActualOutput()
		{
			var sut = new ScoreTally( "2012", "Actuals", usingPredictions:false );
			sut.ForceRefresh = false;
			sut.Render();
			var fileOut = sut.FileName();
			Assert.IsTrue( File.Exists( fileOut ), string.Format( "Cannot find {0}", fileOut ) );
		}

	}
}
