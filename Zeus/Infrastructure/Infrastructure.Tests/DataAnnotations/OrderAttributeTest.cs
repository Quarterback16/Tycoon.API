using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="OrderAttribute" />.
    /// </summary>
    [TestClass]
    public class OrderAttributeTest
    {
        /// <summary>
        /// Test property returns maximum int value for order if not specifically set.
        /// </summary>
        [TestMethod]
        public void Order_GetOrderNotSetDefaultsToIntMaxValue_ReturnsDefaultOrderValue()
        {
            var systemUnderTest = new OrderAttribute();

            Assert.IsTrue(systemUnderTest.PositionInSequence == int.MaxValue);
        }

        /// <summary>
        /// Test property returns maximum int value for order if not specifically set.
        /// </summary>
        [TestMethod]
        public void Order_GetOrderSetValue_ReturnsSetOrderValue()
        {
            var systemUnderTest = new OrderAttribute(1);

            Assert.IsTrue(systemUnderTest.PositionInSequence == 1);
        }
    }
}
