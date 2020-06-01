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
		public void RosterEventStore_Constructs_Ok()
		{
			var sut = new RetroRosters(
				new RosterEventStore());
		}

		[Fact]
		public void RetroRosters_KnowsDeloreanRoster()
		{
			var sut = new RetroRosters(
				new RosterEventStore());
			var result = sut.GetRoster("CD");
			foreach (var item in result)
			{
				output.WriteLine(item);
			}
			Assert.True(result.Count > 0);
		}

		[Fact]
		public void RetroRosters_KnowsDuckHuntersRoster()
		{
			var sut = new RetroRosters(
				new RosterEventStore());
			var result = sut.GetRoster("DD");
			foreach (var item in result)
			{
				output.WriteLine(item);
			}
			Assert.True(result.Count > 0);
		}

		[Fact]
		public void RetroRosters_KnowsRaidersRoster()
		{
			var sut = new RetroRosters(
				new RosterEventStore());
			var result = sut.GetRoster("BR");
			foreach (var item in result)
			{
				output.WriteLine(item);
			}
			Assert.True(result.Count > 0);
		}

		[Fact]
		public void RetroRosters_KnowsRevivalRoster()
		{
			var sut = new RetroRosters(
				new RosterEventStore());
			var result = sut.GetRoster("SR");
			foreach (var item in result)
			{
				output.WriteLine(item);
			}
			Assert.True(result.Count > 0);
		}
	}
}
