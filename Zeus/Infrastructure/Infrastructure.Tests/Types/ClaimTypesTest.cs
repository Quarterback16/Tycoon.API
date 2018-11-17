using Employment.Web.Mvc.Infrastructure.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.Types
{
    /// <summary>
    ///This is a test class for ClaimTypesTest and is intended
    ///to contain all ClaimTypesTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ClaimTypesTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        ///A test for ClaimTypes Constructor
        ///</summary>
        [TestMethod()]
        public void ClaimTypesConstructorTest()
        {
            var target = new ClaimType();
            Assert.IsNotNull(target);
        }
    }
}
