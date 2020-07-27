using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace RosterService.Tests
{
	public class RosterServiceTests
	{
		private readonly ITestOutputHelper output;

		public RosterServiceTests(
			ITestOutputHelper output)
		{
			this.output = output;
		}

		[Fact]
		public void RetroRosters_KnowsDeloreanRoster()
		{
			RosterDump("CD");
		}

		[Fact]
		public void RetroRosters_KnowsDuckHuntersRoster()
		{
			RosterDump("DD");
		}

		[Fact]
		public void RetroRosters_KnowsRaidersRoster()
		{
			RosterDump("BR");
		}

		[Fact]
		public void RetroRosters_KnowsRevivalRoster()
		{
			RosterDump("SR");
		}

		[Fact]
		public void RetroRosters_KnowsGalaxyRoster()
		{
			RosterDump("CG");
		}

		[Fact]
		public void RetroRosters_KnowsRhinosRoster()
		{
			RosterDump("RR");
		}

		[Fact]
		public void RetroRosters_KnowsFridgeRoster()
		{
			RosterDump("SF");
		}

		[Fact]
		public void RetroRosters_KnowsLightningRoster()
		{
			RosterDump("LL");
		}

		private void RosterDump(string teamCode)
		{
			output.WriteLine("--------------------------------------");
			output.WriteLine(
				Utility.FantasyTeamName(teamCode));
			output.WriteLine("--------------------------------------");
			var sut = new RetroRosters(
				new RosterEventStore());
			var result = sut.GetRoster(teamCode);
			PartialRoster(result,"QB");
			PartialRoster(result, "RB");
			PartialRoster(result, "TE");
			PartialRoster(result, "WR");
			PartialRoster(result, "KK");
			output.WriteLine($"Roster count: {result.Count}");
			Assert.True(result.Count > 0);
		}

		private void PartialRoster(
			List<string> result,
			string position)
		{
			foreach (var item in result)
			{
				if (item.Contains(position+" "))
					output.WriteLine(item);
			}
			output.WriteLine("");
		}

		[Fact]
		public void RetroRosters_KnowsWhoOwnsPlayer()
		{
			var testPlayer = "Ron Jaworski";
			var sut = new RetroRosters(
				new RosterEventStore());
			var result = sut.GetOwnerOf(testPlayer);
			Assert.NotNull(result);
			output.WriteLine(Utility.FantasyTeamName(result));
			if (result != "FA")
			{
				var rresult = sut.GetRoster(result);
				foreach (var item in rresult)
				{
					output.WriteLine(item);
				}
				output.WriteLine($"Roster count: {rresult.Count}");
				Assert.True(rresult.Count > 0);
			}
		}
	}
}
