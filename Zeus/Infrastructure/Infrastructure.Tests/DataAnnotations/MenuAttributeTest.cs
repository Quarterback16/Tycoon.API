using System.IO;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="MenuAttribute" />.
    /// </summary>
    [TestClass]
    public class MenuAttributeTest
    {
        /// <summary>
        /// Test property returns maximum int value for order if not specifically set.
        /// </summary>
        [TestMethod]
        public void Menu_GetOrderNotSetDefaultsToIntMaxValue_ReturnsDefaultOrderValue()
        {
            var systemUnderTest = new MenuAttribute("MenuName");

            var order = systemUnderTest.Order;

            Assert.IsTrue(order == int.MaxValue);
        }

        /// <summary>
        /// Test property returns maximum int value for order if not specifically set.
        /// </summary>
        [TestMethod]
        public void Menu_GetOrderSetValue_ReturnsSetOrderValue()
        {
            var systemUnderTest = new MenuAttribute("MenuName");

            systemUnderTest.Order = 1;

            var order = systemUnderTest.Order;

            Assert.IsTrue(order == 1);
        }
    }
}
