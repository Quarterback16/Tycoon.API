using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using RosterLib.Models;
using TFLLib;

namespace RosterGridTests
{
	[TestClass]
	public class UnitRatingTests
	{
		[TestMethod]
		public void TestRatingsTeamLoadingNo()
		{
			var teamList = new List<NflTeam>();
			var sut = new UnitRatingsService();
			sut.TallyTeam( teamList, "2013", new DateTime( 2013, 10, 17 ), "NO" );
			var sutTeam = teamList[0];
			Assert.IsTrue( sutTeam.GameList.Count.Equals( 16 ) );
			Assert.IsTrue( sutTeam.TotYdp.Equals( 1957 ), string.Format( "Was expecting {0} but got {1}", 1957, sutTeam.TotYdp ) );
		}

		[TestMethod]
		public void TestRatingsRetrieval()
		{
			var team = new NflTeam( "SF" );
			var rr = new UnitRatingsService();
			var currRatings = rr.GetUnitRatingsFor( team, new DateTime( 2013, 9, 15 ) );
			const string expectedValue = "DBDABD";
			Assert.IsTrue( currRatings.Equals( expectedValue ),
				string.Format( "SF team rating should be {1} not {0}", currRatings, expectedValue ) );
		}

		[TestMethod]
		public void TestUnitRatingsRetrieval()
		{
			var sut = new UnitRatingsService();
			var bActual = sut.HaveAlreadyRated( new DateTime( 2013, 9 , 8 ) );
			Assert.IsTrue( bActual );
		}

		[TestMethod]
		public void TestUnitRatingsStorage()
		{
			Utility.TflWs.SaveUnitRatings( "XXXXXX", new DateTime( 2013, 8, 5 ), "??" );
			Assert.IsTrue( true );
		}
	}
}
