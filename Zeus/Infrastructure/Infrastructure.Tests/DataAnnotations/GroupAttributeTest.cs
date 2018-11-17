using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="GroupAttribute" />.
    /// </summary>
    [TestClass]
    public class GroupAttributeTest
    {
        /// <summary>
        /// Test property returns maximum int value for order if not specifically set.
        /// </summary>
        [TestMethod]
        public void Group_GetOrderNotSetDefaultsToIntMaxValue_ReturnsDefaultOrderValue()
        {
            var systemUnderTest = new GroupAttribute("GroupName");

            var order = systemUnderTest.Order;

            Assert.IsTrue(order == int.MaxValue);
        }

        /// <summary>
        /// Test property returns maximum int value for order if not specifically set.
        /// </summary>
        [TestMethod]
        public void Group_GetOrderSetValue_ReturnsSetOrderValue()
        {
            var systemUnderTest = new GroupAttribute("GroupName");

            systemUnderTest.Order = 1;

            var order = systemUnderTest.Order;

            Assert.IsTrue(order == 1);
        }
    }
}
