using Employment.Web.Mvc.Infrastructure.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.Configuration
{
    /// <summary>
    ///This is a test class for HistorySectionTest and is intended
    ///to contain all HistorySectionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class HistorySectionTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        ///A test for HistorySection Constructor
        ///</summary>
        [TestMethod()]
        public void HistorySectionConstructorTest()
        {
            var target = new HistorySection();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for PageSize
        ///</summary>
        [TestMethod()]
        public void PageSizeTest()
        {
            var target = new HistorySection();
            const int expected = 99;
            target.PageSize = expected;
            int actual = target.PageSize;
            Assert.AreEqual(expected, actual);
        }
    }
}