using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="VisibleIfNotRegExMatchAttribute" />.
    /// </summary>
    [TestClass]
    public class VisibleIfNotRegExMatchAttributeTest
    {
        private class Model : ContingentModel<VisibleIfNotRegExMatchAttribute>
        {
            public string Value1 { get; set; }

            [VisibleIfNotRegExMatch("Value1", "^ *(1[0-2]|0?[1-9]):[0-5][0-9] *(a|p|A|P)(m|M) *$")]
            public string Value2 { get; set; }

            [VisibleIfNotRegExMatch("^ *(1[0-2]|0?[1-9]):[0-5][0-9] *(a|p|A|P)(m|M) *$")]
            public string Value7 { get; set; }
        }

        /// <summary>
        /// Test property is visible if condition is met with a non-matching value.
        /// </summary>
        [TestMethod]
        public void VisibleIfNotRegExMatch_ConditionMetWithNonMatchingValue_Visible()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not visible if condition is met with a matching value.
        /// </summary>
        [TestMethod]
        public void VisibleIfNotRegExMatch_ConditionMetWithMatchingValue_NotVisible()
        {
            var model = new Model() { Value1 = "8:30 AM" };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is visible if condition on self is met with a non-matching value.
        /// </summary>
        [TestMethod]
        public void VisibleIfNotRegExMatch_ConditionOnSelfMetWithNonMatchingValue_Visible()
        {
            var model = new Model() { Value7 = "hello" };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not visible if condition on self is met with a matching value.
        /// </summary>
        [TestMethod]
        public void VisibleIfNotRegExMatch_ConditionOnSelfMetWithMatchingValue_NotVisible()
        {
            var model = new Model() { Value7 = "8:30 AM" };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }
    }
}
