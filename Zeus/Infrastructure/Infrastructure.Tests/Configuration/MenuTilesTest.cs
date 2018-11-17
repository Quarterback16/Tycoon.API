using Employment.Web.Mvc.Infrastructure.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.Configuration
{
    /// <summary>
    ///This is a test class for MenuTilesTest and is intended
    ///to contain all MenuTilesTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MenuTilesTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }
      
        /// <summary>
        ///A test for MenuTiles Constructor
        ///</summary>
        [TestMethod()]
        public void MenuTilesConstructorTest()
        {
            var target = new MenuTiles();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        [TestMethod()]
        public void AddTest()
        {
            var target = new MenuTiles(); 
            var historyTile = new MenuTile();
            target.Add(historyTile);
            Assert.IsTrue(target.Count==1);
        }

        /// <summary>
        ///A test for Clear
        ///</summary>
        [TestMethod()]
        public void ClearTest()
        {
            var target = new MenuTiles();
            var historyTile = new MenuTile();
            target.Add(historyTile);
            Assert.IsTrue(target.Count == 1);
            target.Clear();
            Assert.IsTrue(target.Count==0);
        }

        /// <summary>
        ///A test for Remove
        ///</summary>
        [TestMethod()]
        public void RemoveTest()
        {
            MenuTiles target = new MenuTiles();
            MenuTile historyTile = new MenuTile();
            target.Add(historyTile);
            Assert.IsTrue(target.Count == 1);
            target.Remove(historyTile);
            Assert.IsTrue(target.Count==0);
        }

        /// <summary>
        ///A test for RemoveAt
        ///</summary>
        [TestMethod()]
        public void RemoveAtTest()
        {
            MenuTiles target = new MenuTiles();
            MenuTile historyTile = new MenuTile();
            target.Add(historyTile);
            Assert.IsTrue(target.Count == 1);
            target.RemoveAt(0);
            Assert.IsTrue(target.Count == 0);
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        [TestMethod()]
        public void ItemTest()
        {
            MenuTiles target = new MenuTiles();
            MenuTile actual = new MenuTile();
            target.Add(actual);
            Assert.AreEqual(target[0], actual);
        }
    }
}