using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Data;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using RosterLib.Models;
using TFLLib;

namespace RosterGridTests
{
	[TestClass]
	public class MatchUpTests
	{
		[TestMethod]
		public void TestMatchupReportOneGameNoExtras()
		{
			var g = new NFLGame( "2013:07-A" );
			var sut = new MatchupReport( g );
		   sut.ShowLineups = false;
			sut.ShowRecent = false;
			sut.ShowFreeAgents = false;
			sut.ShowMatrix = false;
			sut.ShowPrediction = false;
			sut.ShowBets = false;
			sut.ShowInjuries = false;
			sut.ShowTeamCards = false;
			sut.ShowLineupCards = false;
			sut.ShowUnits = false;
			sut.Render( writeProjection: false );
			Assert.IsTrue( File.Exists( sut.FileOut ) );
		}
	}
}
