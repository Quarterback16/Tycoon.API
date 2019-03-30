using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace FbbEventStore.Tests
{
	[TestClass]
	[ExcludeFromCodeCoverage]
	public class FbbEventStoreIntegrationTests
    {
		private FbbEventStore _sut;

		[TestInitialize]
		public void Setup()
		{
			_sut = new FbbEventStore();
		}

		[TestMethod]
		public void FbbEventStore_ConstructOk()
		{
			Assert.IsNotNull(_sut);
		}

		[TestMethod]
		public void FbbEventStore_LoadsAtLeastOneEvent()
		{
			Assert.IsTrue(_sut.Events.Count > 0, "No events loaded");
		}

		[TestMethod]
		public void FbbEventStore_ReturnsEvents()
		{
			var result = _sut.Get<FbbEvent>("events");
			Assert.IsTrue(result.Count() > 0, "No events returned");
		}
	}
}
