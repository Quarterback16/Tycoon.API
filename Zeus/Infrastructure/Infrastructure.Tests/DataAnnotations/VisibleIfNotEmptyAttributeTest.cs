using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="VisibleIfNotEmptyAttribute" />.
    /// </summary>
    [TestClass]
    public class VisibleIfNotEmptyAttributeTest
    {
        private class Model : ContingentModel<VisibleIfNotEmptyAttribute>
        {
            public string Value1 { get; set; }

            [VisibleIfNotEmpty("Value1")]
            public string Value2 { get; set; }

            [VisibleIfNotEmpty]
            public string Value7 { get; set; }

            [VisibleIfNotEmpty]
            public DateTime Value9 { get; set; }

            [VisibleIfNotEmpty(PassOnNull = true)]
            public DateTime? Value10 { get; set; }

            [VisibleIfNotEmpty("Value1", FailOnNull = true)]
            public string Value11 { get; set; }

            [VisibleIfNotEmpty("Value1", FailOnNull = true)]
            public DateTime? Value12 { get; set; }
        }

        /// <summary>
        /// Test property is visible if condition is met with a different value.
        /// </summary>
        [TestMethod]
        public void VisibleIfNotEmpty_ConditionMetWithDifferentValue_Visible()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not visible if condition is met with an empty string.
        /// </summary>
        [TestMethod]
        public void VisibleIfNotEmpty_ConditionMetWithEmptyString_NotVisible()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not visible if condition is met with null.
        /// </summary>
        [TestMethod]
        public void VisibleIfNotEmpty_ConditionMetWithNull_NotVisible()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is visible if condition on self is met with a different value.
        /// </summary>
        [TestMethod]
        public void VisibleIfNotEmpty_ConditionOnSelfMetWithDifferentValue_Visible()
        {
            var model = new Model() { Value7 = "hello" };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not visible if condition on self is met with an empty string.
        /// </summary>
        [TestMethod]
        public void VisibleIfNotEmpty_ConditionOnSelfMetWithEmptyString_NotVisible()
        {
            var model = new Model() { Value7 = string.Empty };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not visible if condition on self is met with null.
        /// </summary>
        [TestMethod]
        public void VisibleIfNotEmpty_ConditionOnSelfMetWithNull_NotVisible()
        {
            var model = new Model() { Value7 = null };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is visible if condition on self is met with an empty string.
        /// </summary>
        [TestMethod]
        public void VisibleIfNotEmpty_DateTimeConditionOnSelfMetWithMinValue_NotVisible()
        {
            var model = new Model() { Value9 = DateTime.MinValue };
            Assert.IsFalse(model.IsConditionMet("Value9"));
        }

        /// <summary>
        /// Test property is not visible if condition on self is met with a string that is not null or empty.
        /// </summary>
        [TestMethod]
        public void VisibleIfNotEmpty_DateTimeConditionOnSelfMetWithNotEmpty_Visible()
        {
            var model = new Model() { Value9 = DateTime.MaxValue };
            Assert.IsTrue(model.IsConditionMet("Value9"));
        }

        /// <summary>
        /// Test property is visible if condition on self is met with a null value and pass on null is true.
        /// </summary>
        [TestMethod]
        public void VisibleIfNotEmpty_DateTimeConditionOnSelfMetWithNullAndPassOnNull_Visible()
        {
            var model = new Model() { Value10 = null };
            Assert.IsTrue(model.IsConditionMet("Value10"));
        }

        /// <summary>
        /// Test property is not visible if condition not is met with a null value and fail on null is true.
        /// </summary>
        [TestMethod]
        public void VisibleIfNotEmpty_ConditionNotMetWithNullAndFailOnNull_NotVisible()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsConditionMet("Value11"));
        }

        /// <summary>
        /// Test property is visible if condition is not met with an empty string and current property is null and fail on null is true.
        /// </summary>
        [TestMethod]
        public void VisibleIfNotEmpty_ConditionMetWithEmptyStringAndFailOnNull_NotVisible()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsFalse(model.IsConditionMet("Value11"));
        }

        /// <summary>
        /// Test property is visible if condition is not met with an empty string and current property is null and fail on null is true.
        /// </summary>
        [TestMethod]
        public void VisibleIfNotEmpty_ConditionMetWithValueAndFailOnNull_Visible()
        {
            var model = new Model() { Value1 = "Value" };
            Assert.IsTrue(model.IsConditionMet("Value11"));
        }

        /// <summary>
        /// Test property is not visible if condition not is met with a null value and fail on null is true.
        /// </summary>
        [TestMethod]
        public void VisibleIfNotEmpty_DateTimeConditionNotMetWithNullAndFailOnNull_NotVisible()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsConditionMet("Value12"));
        }
    }
}
