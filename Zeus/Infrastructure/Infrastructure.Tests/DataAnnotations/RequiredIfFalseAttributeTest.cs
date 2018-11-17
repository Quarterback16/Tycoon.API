using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="RequiredIfFalseAttribute" />.
    /// </summary>
    [TestClass]
    public class RequiredIfFalseAttributeTest
    {
        private class Model : ContingentValidationModel<RequiredIfFalseAttribute>
        {
            public bool Value1 { get; set; }

            [RequiredIfFalse("Value1")]
            public string Value2 { get; set; }

            public bool? Value3 { get; set; }

            [RequiredIfFalse("Value3")]
            public bool? Value4 { get; set; }
        }

        /// <summary>
        /// Test property is required and validates if condition is met with false and a value is supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIfFalse_ConditionMetWithFalseAndValueSupplied_RequiredAndValidates()
        {
            var model = new Model() { Value1 = false, Value2 = "supplied" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property is required and fails if condition is met with false and a value is not supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIfFalse_ConditionMetWithFalseAndNoValueSupplied_RequiredAndFails()
        {
            var model = new Model() { Value1 = false, Value2 = string.Empty };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property is not required and validates if condition is met with true.
        /// </summary>
        [TestMethod]
        public void RequiredIfFalse_ConditionMetWithTrue_NotRequiredAndValidates()
        {
            var model = new Model() { Value1 = true };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property is not required and validates if condition is met with null.
        /// </summary>
        [TestMethod]
        public void RequiredIfFalse_ConditionMetWithNull_NotRequiredAndValidates()
        {
            var model = new Model() { Value3 = null };
            Assert.IsTrue(model.IsValid("Value4"));
        }
    }
}
