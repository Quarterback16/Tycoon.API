using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace RosterService.Tests
{
	[TestClass]
	public class RosterServiceTests
	{
		[TestMethod]
		public void RetroRosters_KnowsDeloreanRoster()
		{
			RosterDump("CD");
		}

		[TestMethod]
		public void RetroRosters_KnowsDuckHuntersRoster()
		{
			RosterDump("DD");
		}

		[TestMethod]
		public void RetroRosters_KnowsRaidersRoster()
		{
			RosterDump("BR");
		}

		[TestMethod]
		public void RetroRosters_KnowsRevivalRoster()
		{
			RosterDump("SR");
		}

		[TestMethod]
		public void RetroRosters_KnowsGalaxyRoster()
		{
			RosterDump("CG");
		}

		[TestMethod]
		public void RetroRosters_KnowsRhinosRoster()
		{
			RosterDump("RR");
		}

		[TestMethod]
		public void RetroRosters_KnowsFridgeRoster()
		{
			RosterDump("SF");
		}

		[TestMethod]
		public void RetroRosters_KnowsLightningRoster()
		{
			RosterDump("LL");
		}

		private void RosterDump(string teamCode)
		{
			Console.WriteLine("--------------------------------------");
			Console.WriteLine(
				Utility.FantasyTeamName(teamCode));
			Console.WriteLine("--------------------------------------");
			var sut = new RetroRosters(
				new RosterEventStore());
			var result = sut.GetRoster(teamCode);
			PartialRoster(result, "QB");
			PartialRoster(result, "RB");
			PartialRoster(result, "TE");
			PartialRoster(result, "WR");
			PartialRoster(result, "KK");
			Console.WriteLine($"Roster count: {result.Count}");
			Assert.IsTrue(result.Count > 0);
		}

		private void PartialRoster(
			List<string> result,
			string position)
		{
			foreach (var item in result)
			{
				if (item.Contains(position + " "))
					Console.WriteLine(item);
			}
			Console.WriteLine("");
		}

		[TestMethod]
		public void RetroRosters_KnowsPriceOfMontana()
		{
			var sut = new RetroRosters(
				new RosterEventStore());
			var result = sut.GetPriceOf(
				"Joe Montana");
			Assert.AreEqual(59, result);
		}
	}
}
