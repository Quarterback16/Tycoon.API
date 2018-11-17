using System.Collections;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterGrid;
using RosterLib;
using TFLLib;

namespace RosterGridTests
{
	[TestClass]
	public class MatchUpReportTests
	{

		[TestMethod]
		public void TestMatchUpReportSeasonOpener2012()
		{
			var g = new NFLGame("2013:01-A");
			var mur = new MatchupReport(g);
			mur.Render( writeProjection: false );
			Assert.IsTrue( File.Exists(mur.FileOut), string.Format("Cannot find {0}", mur.FileOut));

		}

	}
}
