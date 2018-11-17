using Employment.Web.Mvc.Infrastructure.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.Extensions
{
    /// <summary>
    ///This is a test class for EnumExtensionTest and is intended
    ///to contain all EnumExtensionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EnumExtensionTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        ///A test for GetDescription
        ///</summary>
        [TestMethod()]
        public void GetDescriptionTest()
        {           
            Assert.AreEqual("1", TestEnum.One.GetDescription());
            Assert.AreEqual("2", TestEnum.Two.GetDescription());
        }
    }
}
