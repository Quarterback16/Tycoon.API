using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using TflService.IntegrationTests.Fakes;

namespace TflService.IntegrationTests
{
	[TestClass]
    public class TflServiceIntegrationTests
    {
		private TflService _sut;

		[TestInitialize]
		public void Setup()
		{
			var librarian = RosterLib.Utility.TflWs;
			_sut = new TflService(
				librarian,
				new FakeLogger());
		}

		[TestMethod]
		public void TflService_Constructs_Ok()
		{
			Assert.IsNotNull(_sut);
		}

		[TestMethod]
		public void TflService_GetPlayerByName_FindsPlayer()
		{
			var result = _sut.GetNflPlayer("Joe", "Montana");
			Assert.IsInstanceOfType(result,typeof(NFLPlayer));
		}

		[TestMethod]
		public void TflService_GetPlayerById_FindsPlayer()
		{
			var result = _sut.GetNflPlayer("MONTJO01");
			Assert.IsInstanceOfType(result, typeof(NFLPlayer));
		}

		[TestMethod]
		public void TflService_GetCurrentPlayerByNameAndTeam_FindsPlayer()
		{
			var result = _sut.GetNflPlayer("Brent", "Novoselsky", "??");
			Assert.IsInstanceOfType(result, typeof(NFLPlayer));
		}

		[TestMethod]
		public void TflService_OldPlayerDobIsMissing()
		{
			var result = _sut.GetNflPlayer("Brent", "Novoselsky", "??");
			Assert.IsInstanceOfType(result, typeof(NFLPlayer));
			Assert.IsTrue(result.IsMissingDob());
		}

		[TestMethod]
		public void TflService_SetDobWorks()
		{
			var result = _sut.GetNflPlayer("Brent", "Novoselsky", "??");
			Assert.IsInstanceOfType(result, typeof(NFLPlayer));
			Assert.IsTrue(result.IsMissingDob());
			_sut.SetDob(result, new System.DateTime(1959, 11, 8));

			var result2 = _sut.GetNflPlayer("Brent", "Novoselsky", "??");
			Assert.IsFalse(result2.IsMissingDob());
			_sut.SetDob(result2, new System.DateTime(1899, 12, 30));

			var result3 = _sut.GetNflPlayer("Brent", "Novoselsky", "??");
			Assert.IsTrue(result3.IsMissingDob());
		}

		[TestMethod]
		public void TflService_SetHeightWorks()
		{
			var result = _sut.GetNflPlayer("Brent", "Novoselsky", "??");
			Assert.IsInstanceOfType(result, typeof(NFLPlayer));
			_sut.SetHeight(p: result, feet: 6, inches: 2);

			var result2 = _sut.GetNflPlayer("Brent", "Novoselsky", "??");
			Assert.AreEqual(6, result2.HeightFeet);
			Assert.AreEqual(2, result2.HeightInches);
		}

		[TestMethod]
		public void TflService_SetWeightWorks()
		{
			var result = _sut.GetNflPlayer("Brent", "Novoselsky", "??");
			Assert.IsInstanceOfType(result, typeof(NFLPlayer));
			_sut.SetWeight(p: result, pounds: 222);

			var result2 = _sut.GetNflPlayer("Brent", "Novoselsky", "??");
			Assert.AreEqual(222, result2.Weight);
		}
	}
}
