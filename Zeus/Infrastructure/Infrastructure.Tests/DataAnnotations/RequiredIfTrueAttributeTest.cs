using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="RequiredIfTrueAttribute" />.
    /// </summary>
    [TestClass]
    public class RequiredIfTrueAttributeTest
    {
        private class Model : ContingentValidationModel<RequiredIfTrueAttribute>
        {
            public bool Value1 { get; set; }

            [RequiredIfTrue("Value1")]
            public string Value2 { get; set; }

            public bool? Value3 { get; set; }

            [RequiredIfTrue("Value3")]
            public bool? Value4 { get; set; }
        }

        /// <summary>
        /// Test property is required and validates if condition is met with true and a value is supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIfTrue_ConditionMetWithTrueAndValueSupplied_RequiredAndValidates()
        {
            var model = new Model() { Value1 = true, Value2 = "supplied" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property is required and fails if condition is met with true and a value is not supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIfTrue_ConditionMetWithTrueAndNoValueSupplied_RequiredAndFails()
        {
            var model = new Model() { Value1 = true, Value2 = string.Empty };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property is not required and validates if condition is met with false.
        /// </summary>
        [TestMethod]
        public void RequiredIfTrue_ConditionMetWithFalse_NotRequiredAndValidates()
        {
            var model = new Model() { Value1 = false };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property is not required and validates if condition is met with null.
        /// </summary>
        [TestMethod]
        public void RequiredIfTrue_ConditionMetWithNull_NotRequiredAndValidates()
        {
            var model = new Model() { Value3 = null };
            Assert.IsTrue(model.IsValid("Value4"));
        }
    }
}
