using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TFLLib.IntegrationTests
{
	[TestClass]
	public class DataLibrarianIntegrationTests : IntegrationTestsBase
	{
		[TestInitialize]
		public void Setup()
		{
			Initialise();
		}

		[TestMethod]
		public void DataLibrarian_ContructsOk()
		{
			Assert.IsNotNull(Sut);
			Assert.IsTrue(Sut.UseCache);
		}

		[TestMethod]
		public void DataLibrarian_GetPlayer_LeavesDataInCache()
		{
			var result1 = Sut.GetPlayer("MONTJO01");
			var result2 = Sut.GetPlayer("MONTJO01");
			Assert.IsTrue(Sut.HitCount > 0);
		}

		[TestMethod]
		public void DataLibrarian_GetAllGames_ReturnsGames()
		{
			var result = Sut.GetAllGames(season: 2018);
			Assert.IsTrue(result.Tables["SCHED"].Rows.Count > 0);
		}
	}
}
