using Employment.Web.Mvc.Infrastructure.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.Configuration
{
    /// <summary>
    ///This is a test class for MenuSectionTest and is intended
    ///to contain all MenuSectionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MenuSectionTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        ///A test for MenuSection Constructor
        ///</summary>
        [TestMethod()]
        public void MenuSectionConstructorTest()
        {
            var target = new MenuSection();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for Tiles
        ///</summary>
        [TestMethod()]
        public void TilesTest()
        {
            var target = new MenuSection(); 
            var actual = target.Tiles;
            Assert.IsTrue(actual.Count==0);
        }

        [TestMethod()]
        public void TilesSetTest()
        {
            var target = new MenuSection();
            Assert.IsNotNull(target);
            target.Tiles = new MenuTiles();
            Assert.IsNotNull(target.Tiles);
        }
    }
}