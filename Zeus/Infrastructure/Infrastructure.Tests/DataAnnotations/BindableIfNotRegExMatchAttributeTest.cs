using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="BindableIfNotRegExMatchAttribute" />.
    /// </summary>
    [TestClass]
    public class BindableIfNotRegExMatchAttributeTest
    {
        private class Model : ContingentModel<BindableIfNotRegExMatchAttribute>
        {
            public string Value1 { get; set; }

            [BindableIfNotRegExMatch("Value1", "^ *(1[0-2]|0?[1-9]):[0-5][0-9] *(a|p|A|P)(m|M) *$")]
            public string Value2 { get; set; }

            [BindableIfNotRegExMatch("^ *(1[0-2]|0?[1-9]):[0-5][0-9] *(a|p|A|P)(m|M) *$")]
            public string Value7 { get; set; }
        }

        /// <summary>
        /// Test property is bindable if condition is met with a non-matching value.
        /// </summary>
        [TestMethod]
        public void BindableIfNotRegExMatch_ConditionMetWithNonMatchingValue_Bindable()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not bindable if condition is met with a matching value.
        /// </summary>
        [TestMethod]
        public void BindableIfNotRegExMatch_ConditionMetWithMatchingValue_NotBindable()
        {
            var model = new Model() { Value1 = "8:30 AM" };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is bindable if condition on self is met with a non-matching value.
        /// </summary>
        [TestMethod]
        public void BindableIfNotRegExMatch_ConditionOnSelfMetWithNonMatchingValue_Bindable()
        {
            var model = new Model() { Value7 = "hello" };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not bindable if condition on self is met with a matching value.
        /// </summary>
        [TestMethod]
        public void BindableIfNotRegExMatch_ConditionOnSelfMetWithMatchingValue_NotBindable()
        {
            var model = new Model() { Value7 = "8:30 AM" };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }
    }
}
