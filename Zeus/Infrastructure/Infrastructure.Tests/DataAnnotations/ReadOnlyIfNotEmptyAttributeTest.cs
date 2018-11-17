using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="ReadOnlyIfNotEmptyAttribute" />.
    /// </summary>
    [TestClass]
    public class ReadOnlyIfNotEmptyAttributeTest
    {
        private class Model : ContingentModel<ReadOnlyIfNotEmptyAttribute>
        {
            public string Value1 { get; set; }

            [ReadOnlyIfNotEmpty("Value1")]
            public string Value2 { get; set; }

            [ReadOnlyIfNotEmpty]
            public string Value7 { get; set; }

            [ReadOnlyIfNotEmpty]
            public DateTime Value9 { get; set; }

            [ReadOnlyIfNotEmpty(PassOnNull = true)]
            public DateTime? Value10 { get; set; }

            [ReadOnlyIfNotEmpty("Value1", FailOnNull = true)]
            public string Value11 { get; set; }

            [ReadOnlyIfNotEmpty("Value1", FailOnNull = true)]
            public DateTime? Value12 { get; set; }
        }

        /// <summary>
        /// Test property is read only if condition is met with a different value.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfNotEmpty_ConditionMetWithDifferentValue_ReadOnly()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not read only if condition is met with an empty string.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfNotEmpty_ConditionMetWithEmptyString_NotReadOnly()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not read only if condition is met with null.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfNotEmpty_ConditionMetWithNull_NotReadOnly()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is read only if condition on self is met with a different value.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfNotEmpty_ConditionOnSelfMetWithDifferentValue_ReadOnly()
        {
            var model = new Model() { Value7 = "hello" };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not read only if condition on self is met with an empty string.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfNotEmpty_ConditionOnSelfMetWithEmptyString_NotReadOnly()
        {
            var model = new Model() { Value7 = string.Empty };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not read only if condition on self is met with null.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfNotEmpty_ConditionOnSelfMetWithNull_NotReadOnly()
        {
            var model = new Model() { Value7 = null };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is read only if condition on self is met with an empty string.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfNotEmpty_DateTimeConditionOnSelfMetWithMinValue_NotReadOnly()
        {
            var model = new Model() { Value9 = DateTime.MinValue };
            Assert.IsFalse(model.IsConditionMet("Value9"));
        }

        /// <summary>
        /// Test property is not read only if condition on self is met with a string that is not null or empty.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfNotEmpty_DateTimeConditionOnSelfMetWithNotEmpty_ReadOnly()
        {
            var model = new Model() { Value9 = DateTime.MaxValue };
            Assert.IsTrue(model.IsConditionMet("Value9"));
        }

        /// <summary>
        /// Test property is read only if condition on self is met with a null value and pass on null is true.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfNotEmpty_DateTimeConditionOnSelfMetWithNullAndPassOnNull_ReadOnly()
        {
            var model = new Model() { Value10 = null };
            Assert.IsTrue(model.IsConditionMet("Value10"));
        }

        /// <summary>
        /// Test property is not read only if condition not is met with a null value and fail on null is true.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfNotEmpty_ConditionNotMetWithNullAndFailOnNull_NotReadOnly()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsConditionMet("Value11"));
        }

        /// <summary>
        /// Test property is read only if condition is not met with an empty string and current property is null and fail on null is true.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfNotEmpty_ConditionMetWithEmptyStringAndFailOnNull_NotReadOnly()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsFalse(model.IsConditionMet("Value11"));
        }

        /// <summary>
        /// Test property is read only if condition is not met with an empty string and current property is null and fail on null is true.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfNotEmpty_ConditionMetWithValueAndFailOnNull_ReadOnly()
        {
            var model = new Model() { Value1 = "Value" };
            Assert.IsTrue(model.IsConditionMet("Value11"));
        }

        /// <summary>
        /// Test property is not read only if condition not is met with a null value and fail on null is true.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfNotEmpty_DateTimeConditionNotMetWithNullAndFailOnNull_NotReadOnly()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsConditionMet("Value12"));
        }
    }
}
