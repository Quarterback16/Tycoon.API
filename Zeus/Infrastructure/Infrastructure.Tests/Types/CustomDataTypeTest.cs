using Employment.Web.Mvc.Infrastructure.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests
{
    /// <summary>
    ///This is a test class for CustomDataTypeTest and is intended
    ///to contain all CustomDataTypeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CustomDataTypeTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        ///A test for CustomDataType Constructor
        ///</summary>
        [TestMethod()]
        public void CustomDataTypeConstructorTest()
        {
            var target = new CustomDataType();
            Assert.IsNotNull(target);
            Assert.IsNotNull(CustomDataType.Grid);
        }
    }
}
