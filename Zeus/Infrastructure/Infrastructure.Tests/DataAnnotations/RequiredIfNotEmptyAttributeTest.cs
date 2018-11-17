using System;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="RequiredIfNotEmptyAttribute" />.
    /// </summary>
    [TestClass]
    public class RequiredIfNotEmptyAttributeTest
    {
        private class Model : ContingentValidationModel<RequiredIfNotEmptyAttribute>
        {
            public string Value1 { get; set; }

            [RequiredIfNotEmpty("Value1")]
            public string Value2 { get; set; }

            public int Value3 { get; set; }

            [RequiredIfNotEmpty("Value3")]
            public int Value4 { get; set; }

            [RequiredIfNotEmpty("Value1", PassOnNull = true)]
            public string Value8 { get; set; }

            [RequiredIfNotEmpty("Value1", PassOnNull = true)]
            public DateTime? Value10 { get; set; }

            [RequiredIfNotEmpty("Value1", FailOnNull = true)]
            public string Value11 { get; set; }

            [RequiredIfNotEmpty("Value1", FailOnNull = true)]
            public DateTime? Value12 { get; set; }
        }

        /// <summary>
        /// Test property is required and validates if condition is met with a different value and a value is supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIfNotEmpty_ConditionMetWithDifferentValueAndValueSupplied_RequiredAndValidates()
        {
            var model = new Model() { Value1 = "goodbye", Value2 = "supplied" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property is required and fails if condition is met with a different value and a value is not supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIfNotEmpty_ConditionMetWithDifferentValueAndNoValueSupplied_RequiredAndFails()
        {
            var model = new Model() { Value1 = "goodbye", Value2 = string.Empty };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property is required and fails if condition is met with a different value and a value is not supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIfNotEmpty_ConditionMetWithDifferentValueAndNullValueSupplied_RequiredAndFails()
        {
            var model = new Model() { Value1 = "goodbye", Value2 = null };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property is not required and validates if condition is met with an empty string.
        /// </summary>
        [TestMethod]
        public void RequiredIfNotEmpty_ConditionMetWithEmptyString_NotRequiredAndValidates()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property is not required and validates if condition is met with null.
        /// </summary>
        [TestMethod]
        public void RequiredIfNotEmpty_ConditionMetWithNull_NotRequiredAndValidates()
        {
            var model = new Model() { Value1 = null };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property is required and validates if condition is met with a different value and a value is supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIfNotEmpty_IntConditionMetWithDifferentValueAndValueSupplied_RequiredAndValidates()
        {
            var model = new Model() { Value3 = 1, Value4 = 1 };
            Assert.IsTrue(model.IsValid("Value4"));
        }

        /// <summary>
        /// Test property is required and fails if condition is met with a different value and a value is not supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIfNotEmpty_IntConditionMetWithDifferentValueAndNoValueSupplied_RequiredAndFails()
        {
            var model = new Model() { Value3 = 1, Value4 = 0 };
            Assert.IsFalse(model.IsValid("Value4"));
        }

        /// <summary>
        /// Test property is not required and validates if condition is met with an empty value.
        /// </summary>
        [TestMethod]
        public void RequiredIfNotEmpty_IntConditionMetWithEmpty_NotRequiredAndValidates()
        {
            var model = new Model() { Value3 = 0 };
            Assert.IsTrue(model.IsValid("Value4"));
        }

        /// <summary>
        /// Test property passes if condition is met with a null value and pass on null is true.
        /// </summary>
        [TestMethod]
        public void RequiredIfNotEmpty_ConditionNotMetWithNullAndPassOnNull_NotRequired()
        {
            var model = new Model() { Value1 = null };
            Assert.IsTrue(model.IsValid("Value8"));
        }

        /// <summary>
        /// Test property passes if condition is met with a null value and pass on null is true.
        /// </summary>
        [TestMethod]
        public void RequiredIfNotEmpty_DateTimeConditionNotMetWithNullAndPassOnNull_NotRequired()
        {
            var model = new Model() { Value1 = null };
            Assert.IsTrue(model.IsValid("Value10"));
        }

        /// <summary>
        /// Test property fails if condition is met with a null value and fail on null is true.
        /// </summary>
        [TestMethod]
        public void RequiredIfNotEmpty_ConditionNotMetWithNullAndFailOnNull_NotRequired()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsValid("Value11"));
        }

        /// <summary>
        /// Test property fails if condition is met with a null value and fail on null is true.
        /// </summary>
        [TestMethod]
        public void RequiredIfNotEmpty_DateTimeConditionNotMetWithNullAndFailOnNull_NotRequired()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsValid("Value12"));
        }
    }
}
