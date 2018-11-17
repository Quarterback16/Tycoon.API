using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="ReadOnlyIfNotRegExMatchAttribute" />.
    /// </summary>
    [TestClass]
    public class ReadOnlyIfNotRegExMatchAttributeTest
    {
        private class Model : ContingentModel<ReadOnlyIfNotRegExMatchAttribute>
        {
            public string Value1 { get; set; }

            [ReadOnlyIfNotRegExMatch("Value1", "^ *(1[0-2]|0?[1-9]):[0-5][0-9] *(a|p|A|P)(m|M) *$")]
            public string Value2 { get; set; }

            [ReadOnlyIfNotRegExMatch("^ *(1[0-2]|0?[1-9]):[0-5][0-9] *(a|p|A|P)(m|M) *$")]
            public string Value7 { get; set; }
        }

        /// <summary>
        /// Test property is read only if condition is met with a non-matching value.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfNotRegExMatch_ConditionMetWithNonMatchingValue_ReadOnly()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not read only if condition is met with a matching value.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfNotRegExMatch_ConditionMetWithMatchingValue_NotReadOnly()
        {
            var model = new Model() { Value1 = "8:30 AM" };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is read only if condition on self is met with a non-matching value.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfNotRegExMatch_ConditionOnSelfMetWithNonMatchingValue_ReadOnly()
        {
            var model = new Model() { Value7 = "hello" };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not read only if condition on self is met with a matching value.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfNotRegExMatch_ConditionOnSelfMetWithMatchingValue_NotReadOnly()
        {
            var model = new Model() { Value7 = "8:30 AM" };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }
    }
}
