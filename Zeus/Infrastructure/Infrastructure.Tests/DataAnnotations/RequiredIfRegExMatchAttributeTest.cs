using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="RequiredIfRegExMatchAttribute" />.
    /// </summary>
    [TestClass]
    public class RequiredIfRegExMatchAttributeTest
    {
        private class Model : ContingentValidationModel<RequiredIfRegExMatchAttribute>
        {
            public string Value1 { get; set; }

            [RequiredIfRegExMatch("Value1", "^ *(1[0-2]|0?[1-9]):[0-5][0-9] *(a|p|A|P)(m|M) *$")]
            public string Value2 { get; set; }
        }

        /// <summary>
        /// Test property is required and validates if condition is met with a matching value and a value is supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIfRegExMatch_ConditionMetWithMatchingValueAndValueSupplied_RequiredAndValidates()
        {
            var model = new Model() { Value1 = "8:30 AM", Value2 = "supplied" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property is required and fails if condition is met with a matching value and a value is not supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIfRegExMatch_ConditionMetWithMatchingValueAndNoValueSupplied_RequiredAndFails()
        {
            var model = new Model() { Value1 = "8:30 AM" };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property is not required and validates if condition is met with a non-matching value.
        /// </summary>
        [TestMethod]
        public void RequiredIfRegExMatch_ConditionMetWithNonMatchingValue_NotRequiredAndValidates()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsTrue(model.IsValid("Value2"));
        }
    }
}
