using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="RequiredIfNotAttribute" />.
    /// </summary>
    [TestClass]
    public class RequiredIfNotAttributeTest
    {
        private class Model : ContingentValidationModel<RequiredIfNotAttribute>
        {
            public string Value1 { get; set; }

            [RequiredIfNot("Value1", "hello")]
            public string Value2 { get; set; }

            [RequiredIfNot("Value1", new [] { "hello", "world" })]
            public string Value3 { get; set; }

            [RequiredIfNot("Value1", "")]
            public string Value4 { get; set; }
        }

        /// <summary>
        /// Test property is required and validates if condition is met with a different value and a value is supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIfNot_ConditionMetWithDifferentValueAndValueSupplied_RequiredAndValidates()
        {
            var model = new Model() { Value1 = "goodbye", Value2 = "supplied" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property is required and fails if condition is met with a different value and a value is not supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIfNot_ConditionMetWithDifferentValueAndNoValueSupplied_RequiredAndFails()
        {
            var model = new Model() { Value1 = "goodbye", Value2 = string.Empty };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property is not required and validates if condition is met with the expected value.
        /// </summary>
        [TestMethod]
        public void RequiredIfNot_ConditionMetWithExpectedValue_NotRequiredAndValidates()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property is required and fails if condition is met with an empty string and a value is supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIfNot_ConditionMetWithEmptyStringAndValueSupplied_RequiredAndValidates()
        {
            var model = new Model() { Value1 = string.Empty, Value2 = "supplied" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property is required and fails if condition is met with an empty string and a value is not supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIfNot_ConditionMetWithEmptyStringAndNoValueSupplied_RequiredAndFails()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property is required and fails if condition is met with null and a value is supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIfNot_ConditionMetWithNullAndValueSupplied_RequiredAndValidates()
        {
            var model = new Model() { Value1 = null, Value2 = "supplied" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property is required and fails if condition is met with null and a value is not supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIfNot_ConditionMetWithNullAndNoValueSupplied_RequiredAndFails()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property is required and validates if condition is met with a different value and a value is supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIfNot_ConditionMetWithDifferentValueToOneOfTheExpectedValesAndValueSupplied_RequiredAndValidates()
        {
            var model = new Model() { Value1 = "goodbye", Value3 = "supplied" };
            Assert.IsTrue(model.IsValid("Value3"));
        }

        /// <summary>
        /// Test property is required and fails if condition is met with a different value and a value is not supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIfNot_ConditionMetWithDifferentValueToOneOfTheExpectedValesAndNoValueSupplied_RequiredAndFails()
        {
            var model = new Model() { Value1 = "goodbye", Value3 = string.Empty };
            Assert.IsFalse(model.IsValid("Value3"));
        }

        /// <summary>
        /// Test property is not required and validates if condition is met with the expected value.
        /// </summary>
        [TestMethod]
        public void RequiredIfNot_ConditionMetWithOneOfTheExpectedValues_NotRequiredAndValidates()
        {
            var model = new Model() { Value1 = "world" };
            Assert.IsTrue(model.IsValid("Value3"));
        }

        /// <summary>
        /// Test property is required and fails if condition is met with an empty string and a value is supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIfNot_ConditionMetWithEmptyStringInsteadOfOneOfTheExpectedValesAndValueSupplied_RequiredAndValidates()
        {
            var model = new Model() { Value1 = string.Empty, Value3 = "supplied" };
            Assert.IsTrue(model.IsValid("Value3"));
        }

        /// <summary>
        /// Test property is required and fails if condition is met with an empty string and a value is not supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIfNot_ConditionMetWithEmptyStringInsteadOfOneOfTheExpectedValesAndNoValueSupplied_RequiredAndFails()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsFalse(model.IsValid("Value3"));
        }

        /// <summary>
        /// Test property is required and fails if condition is met with null and a value is supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIfNot_ConditionMetWithNullInsteadOfOneOfTheExpectedValesAndValueSupplied_RequiredAndValidates()
        {
            var model = new Model() { Value1 = null, Value3 = "supplied" };
            Assert.IsTrue(model.IsValid("Value3"));
        }

        /// <summary>
        /// Test property is required and fails if condition is met with null and a value is not supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIfNot_ConditionMetWithNullInsteadOfOneOfTheExpectedValesAndNoValueSupplied_RequiredAndFails()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsValid("Value3"));
        }

        /// <summary>
        /// Test property is required and fails if condition is met with the expected value which is an empty string, and no value is supplied.
        /// </summary>
        [TestMethod]
        public void RequiredIfNot_ConditionMetWithExpectedValueWhichIsAnEmptyString_RequiredAndFails()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsFalse(model.IsValid("Value4"));
        }
    }
}
