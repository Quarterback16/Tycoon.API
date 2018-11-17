using Employment.Web.Mvc.Infrastructure.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.Configuration
{
    /// <summary>
    ///This is a test class for MenuTileTest and is intended
    ///to contain all MenuTileTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MenuTileTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }


        /// <summary>
        ///A test for MenuTile Constructor
        ///</summary>
        [TestMethod()]
        public void MenuTileConstructorTest()
        {
            var target = new MenuTile();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for AreaName
        ///</summary>
        [TestMethod()]
        public void AreaNameTest()
        {
            var target = new MenuTile {AreaName = "TestArea"};
            var actual = target.AreaName;
            Assert.AreEqual("TestArea", actual);
        }

        /// <summary>
        ///A test for DisplayName
        ///</summary>
        [TestMethod()]
        public void DisplayNameTest()
        {
            var target = new MenuTile {DisplayName = "TestDisplay"};
            var actual = target.DisplayName;
            Assert.AreEqual("TestDisplay", actual);
        }
    }
}