using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HsEventStore.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class HsEventIntegrationTests
    {
        private HsEventStore _sut;

        [TestInitialize]
        public void Setup()
        {
            _sut = new HsEventStore();
        }

        [TestMethod]
        public void HsEventStore_ConstructOk()
        {
            Assert.IsNotNull(_sut);
        }

        [TestMethod]
        public void HsEventStore_LoadsAtLeastOneEvent()
        {
            Assert.IsTrue(_sut.Events.Count > 0, "No events loaded");
        }
    }
}
