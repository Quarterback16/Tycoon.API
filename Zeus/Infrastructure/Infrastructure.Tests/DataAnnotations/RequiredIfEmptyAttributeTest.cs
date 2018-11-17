using System;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="RequiredIfEmptyAttribute" />.
    /// </summary>
    [TestClass]
    public class RequiredIfEmptyAttributeTest
    {
        private class Model : ContingentValidationModel<RequiredIfEmptyAttribute>
        {
            public string Value1 { get; set; }

            [RequiredIfEmpty("Value1")]
            public string Value2 { get; set; }

            public DateTime Value3 { get; set; }

            [RequiredIfEmpty("Value3")]
            public DateTime Value4 { get; set; }

            [RequiredIfEmpty("Value1", PassOnNull = true)]
            public string Value8 { get; set; }

            [RequiredIfEmpty("Value1", PassOnNull = true)]
            public DateTime? Value10 { get; set; }

            [RequiredIfEmpty("Value1", FailOnNull = true)]
            public string Value11 { get; set; }

            [RequiredIfEmpty("Value1", FailOnNull = true)]
            public DateTime? Value12 { get; set; }
        }

        /// <summary>
        /// Test property is required and validates if condition is met with the expected value and a value is supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIfEmpty_ConditionMetWithEmptyStringAndValueSupplied_RequiredAndValidates()
        {
            var model = new Model() { Value1 = string.Empty, Value2 = "supplied" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property is required and fails if condition is met with the expected value and a value is not supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIfEmpty_ConditionMetWithEmptyStringAndNoValueSupplied_RequiredAndFails()
        {
            var model = new Model() { Value1 = string.Empty, Value2 = string.Empty };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property is required and fails if condition is met with the expected value and a value is not supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIfEmpty_ConditionMetWithEmptyStringAndNullValueSupplied_RequiredAndFails()
        {
            var model = new Model() { Value1 = string.Empty, Value2 = null };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property is required and fails if condition is met with the expected value and a value is not supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIfEmpty_ConditionMetWithNullAndValueSupplied_RequiredAndValidates()
        {
            var model = new Model() { Value1 = null, Value2 = "supplied" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property is not required and fails if condition is met with the expected value and a value is not supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIfEmpty_ConditionMetWithNullAndNoValueSupplied_RequiredAndFails()
        {
            var model = new Model() { Value1 = null, Value2 = string.Empty };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property is not required and validates if condition is met with a different value.
        /// </summary>
        [TestMethod]
        public void RequiredIfEmpty_ConditionMetWithDifferentValue_NotRequiredAndValidates()
        {
            var model = new Model() { Value1 = "goodbye" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property is required and validates if condition is met with the expected value and a value is supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIfEmpty_DateTimeConditionMetAndValueSupplied_RequiredAndValidates()
        {
            var model = new Model() { Value3 = DateTime.MinValue, Value4 = DateTime.Now };
            Assert.IsTrue(model.IsValid("Value4"));
        }

        /// <summary>
        /// Test property is required and fails if condition is met with the expected value and a value is not supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIfEmpty_DateTimeConditionMetAndNoValueSupplied_RequiredAndFails()
        {
            var model = new Model() { Value3 = DateTime.MinValue, Value4 = DateTime.MinValue };
            Assert.IsFalse(model.IsValid("Value4"));
        }
        /// <summary>
        /// Test property is not required and validates if condition is met with a different value.
        /// </summary>
        [TestMethod]
        public void RequiredIfEmpty_DateTimeConditionMetWithDifferentValue_NotRequiredAndValidates()
        {
            var model = new Model() { Value3 = DateTime.Now };
            Assert.IsTrue(model.IsValid("Value4"));
        }

        /// <summary>
        /// Test property passes if condition is met with a null value and pass on null is true.
        /// </summary>
        [TestMethod]
        public void RequiredIfEmpty_ConditionNotMetWithNullAndPassOnNull_NotRequired()
        {
            var model = new Model() { Value1 = null };
            Assert.IsTrue(model.IsValid("Value8"));
        }

        /// <summary>
        /// Test property passes if condition is met with a null value and pass on null is true.
        /// </summary>
        [TestMethod]
        public void RequiredIfEmpty_DateTimeConditionNotMetWithNullAndPassOnNull_NotRequired()
        {
            var model = new Model() { Value1 = null };
            Assert.IsTrue(model.IsValid("Value10"));
        }

        /// <summary>
        /// Test property fails if condition is met with a null value and fail on null is true.
        /// </summary>
        [TestMethod]
        public void RequiredIfEmpty_ConditionNotMetWithNullAndFailOnNull_NotRequired()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsValid("Value11"));
        }
        
        /// <summary>
        /// Test property fails if condition is met with a null value and fail on null is true.
        /// </summary>
        [TestMethod]
        public void RequiredIfEmpty_DateTimeConditionNotMetWithNullAndFailOnNull_NotRequired()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsValid("Value12"));
        }
    }
}
