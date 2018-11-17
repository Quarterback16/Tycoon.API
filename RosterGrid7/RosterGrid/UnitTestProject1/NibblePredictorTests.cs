using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using RosterLib.Models;
using TFLLib;

namespace RosterGridTests
{
	[TestClass]
	public class NibblePredictorTests
	{

		#region  Nibble stuff

		[TestMethod]
		public void TestNibbleRatingsServiceNiners2011()
		{
			var nrs = new NibbleRatingsService { WeeksToGoBack = 1 };
			var sf = new NflTeam("SF");
			var nr = nrs.GetNibbleRatingFor(sf, new DateTime(2010, 8, 17));
			Assert.IsTrue(nr.Offence.Equals(1),
				string.Format("Off Rating should be 1 not {0}", nr.Offence));
		}

		[TestMethod]
		public void TestNibbleRatingsServiceNiners2011B()
		{
			var nrs = new NibbleRatingsService { WeeksToGoBack = 17 };
			var sf = new NflTeam("SF");
			var nr = nrs.GetNibbleRatingFor(sf, new DateTime(2010, 8, 17));
			Assert.IsTrue(nr.Defence.Equals(-13),
				string.Format("Off Rating should be -13 not {0}", nr.Defence));
		}

		[TestMethod]
		public void TestNibblePredictor()
		{
			var rs = new NibbleRatingsService { AuditIt = true, WeeksToGoBack = 1 };
			var np = new NibblePredictor { RatingsService = rs, AuditTrail = true };
			var game = new NFLGame("2011:01-A");  //  NO @ GB
			var result = np.PredictGame(game, new FakePredictionStorer(), Utility.StartOfSeason());
			Assert.IsTrue(result.HomeWin());
			Assert.IsTrue(result.HomeScore.Equals(23), string.Format("Home score should be 23 not {0}", result.HomeScore));
			Assert.IsTrue(result.AwayScore.Equals(17), string.Format("Away score should be 17 not {0}", result.AwayScore));
		}

		[TestMethod]
		public void TestNibbleRatingforGreenBay()
		{
			var rs = new NibbleRatingsService { AuditIt = true, WeeksToGoBack = 1 };
			var team = new NflTeam("GB");
			var gbRating = rs.GetNibbleRatingFor(team, Utility.StartOfSeason("2011"));
			Assert.IsTrue(gbRating.Offence.Equals(-2), string.Format("Offence should be -2 not {0}",
				gbRating.Offence));
			Assert.IsTrue(gbRating.Defence.Equals(-4), string.Format("Defence should be -4 not {0}",
				gbRating.Defence));
		}

		//TODO:  This might be too big for a Unit test
		[TestMethod]
		public void TestNibblePredictionsFor2012()
		{
			const string season = "2012";
			var fileout = string.Format("{0}\\{1}\\Projections\\Nibble.htm",
				Utility.OutputDirectory(), season);
			var rs = new NibbleRatingsService { AuditIt = true, WeeksToGoBack = 17 };
			var ps = new DbfPredictionStorer();
			var np = new NibblePredictor
			{
				RatingsService = rs,
				AuditTrail = false,
				StorePrediction = true,
				PredictionStorer = ps
			};
			np.PredictSeason(season, Utility.StartOfSeason(season), fileout);
			Assert.IsTrue(File.Exists(fileout), string.Format("Cannot find {0}", fileout));
		}

		#endregion


		[TestMethod]
		public void TestNibbleRating()
		{
			var rg = new RosterGrid.RosterGrid();
			var r = new NibbleRanker("2006");
			r.RefreshRatings(1);  //  workout the ratings after week 1
			var game = new NFLGame("2006:02-A"); // BB @ MD         

			var actual = r.GetRating(game);
			Assert.AreEqual(-1, actual.AwayOff, string.Format("{0} {1:F} was {2}, not {3:F}",
													"Away Offence for week ", 2, actual.AwayOff, -1));
			Assert.AreEqual(-1, actual.AwayDef, string.Format("{0} {1:F} was {2}, not {3:F}",
													"Away Offence for week ", 2, actual.AwayDef, -1));
			Assert.AreEqual(-1, actual.HomeOff, string.Format("{0} {1:F} was {2}, not {3:F}",
													"Away Offence for week ", 2, actual.HomeOff, -1));
			Assert.AreEqual(1, actual.HomeDef, string.Format("{0} {1:F} was {2}, not {3:F}",
													"Away Offence for week ", 2, actual.HomeDef, -1));
		}
	}
}
