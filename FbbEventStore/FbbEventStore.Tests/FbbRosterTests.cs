using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace FbbEventStore.Tests
{
	[TestClass]
	[ExcludeFromCodeCoverage]
	public class FbbRosterTests
	{
		private FbbRosters _sut;

		[TestInitialize]
		public void Setup()
		{
			_sut = new FbbRosters(
				new FbbEventStore());
		}

		[TestMethod]
		public void FbbEventStore_ConstructOk()
		{
			Assert.IsNotNull(_sut);
		}

		[TestMethod]
		public void FbbEventStore_KnowsRosterForCa()
		{
			var result = _sut.GetRoster("CA");
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Count > 0);
			_sut.DumpRoster("CA");
		}

		[TestMethod]
		public void FbbEventStore_KnowsWhoOwnsYelich()
		{
			var result = _sut.GetOwnerOf("Christian Yelich");
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Equals("CA"));
		}

		[TestMethod]
		public void FbbEventStore_KnowsWhoOwnsPlayer()
		{
			var plyr = "Kirby Yates";
			var result = _sut.GetOwnerOf(plyr);
			Assert.IsNotNull(result);
			System.Console.WriteLine($"{result} owns {plyr}");
			Assert.IsTrue(result.Equals("BBJ"));
		}

		[TestMethod]
		public void FbbEventStore_KnowsWhoOwnsPlayers()
		{
			string[] plyr = 
				{
					"Kirby Yates",
					"Hunter Strickland",
					"Josh Hader",
					"Taylor Rogers",
					"Matt Barnes",
					"Shane Green",
					"Jose Alvarado",
					"Brad Boxberger",
					"David Hernandez"
				};
			var result = _sut.GetOwnersOf(plyr);
			Assert.IsNotNull(result);
			foreach (var item in result)
			{
				System.Console.WriteLine(item);
			}
		}
	}
}
