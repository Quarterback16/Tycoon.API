using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="ReadOnlyIfEmptyAttribute" />.
    /// </summary>
    [TestClass]
    public class ReadOnlyIfEmptyAttributeTest
    {
        private class Model : ContingentModel<ReadOnlyIfEmptyAttribute>
        {
            public string Value1 { get; set; }

            [ReadOnlyIfEmpty("Value1")]
            public string Value2 { get; set; }

            [ReadOnlyIfEmpty("Value1", PassOnNull = true)]
            public string Value3 { get; set; }

            [ReadOnlyIfEmpty]
            public string Value7 { get; set; }

            [ReadOnlyIfEmpty(PassOnNull = true)]
            public string Value8 { get; set; }

            [ReadOnlyIfEmpty]
            public DateTime Value9 { get; set; }

            [ReadOnlyIfEmpty(PassOnNull = true)]
            public DateTime? Value10 { get; set; }

            [ReadOnlyIfEmpty("Value1", FailOnNull = true)]
            public string Value11 { get; set; }

            [ReadOnlyIfEmpty("Value1", FailOnNull = true)]
            public DateTime? Value12 { get; set; }
        }

        /// <summary>
        /// Test property is read only if condition is met with an empty string.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfEmpty_ConditionMetWithEmptyString_ReadOnly()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is read only if condition is met with a null value.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfEmpty_ConditionMetWithNull_ReadOnly()
        {
            var model = new Model() { Value1 = null };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not read only if condition is met with a string that is not null or empty.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfEmpty_ConditionMetWithNotEmptyOrNullString_NotReadOnly()
        {
            var model = new Model() { Value1 = "goodbye" };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is read only if condition is met with a null value and pass on null is true.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfEmpty_ConditionMetWithNullAndPassOnNull_ReadOnly()
        {
            var model = new Model() { Value1 = null };
            Assert.IsTrue(model.IsConditionMet("Value3"));
        }

        /// <summary>
        /// Test property is read only if condition is met with an empty string and current property is null and pass on null is true.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfEmpty_ConditionMetWithEmptyStringAndPassOnNull_ReadOnly()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsTrue(model.IsConditionMet("Value3"));
        }

        /// <summary>
        /// Test property is read only if condition on self is met with an empty string.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfEmpty_ConditionOnSelfMetWithEmptyString_ReadOnly()
        {
            var model = new Model() { Value7 = string.Empty };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is read only if condition on self is met with a null value.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfEmpty_ConditionOnSelfMetWithNull_ReadOnly()
        {
            var model = new Model() { Value7 = null };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not read only if condition on self is met with a string that is not null or empty.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfEmpty_ConditionOnSelfMetWithNotEmptyOrNullString_NotReadOnly()
        {
            var model = new Model() { Value7 = "goodbye" };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is read only if condition on self is met with a null value and pass on null is true.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfEmpty_ConditionOnSelfMetWithNullAndPassOnNull_ReadOnly()
        {
            var model = new Model() { Value8 = null };
            Assert.IsTrue(model.IsConditionMet("Value8"));
        }

        /// <summary>
        /// Test property is read only if condition on self is met with an empty string and current property is null and pass on null is true.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfEmpty_ConditionOnSelfMetWithEmptyStringAndPassOnNull_ReadOnly()
        {
            var model = new Model() { Value8 = string.Empty };
            Assert.IsTrue(model.IsConditionMet("Value8"));
        }

        /// <summary>
        /// Test property is read only if condition on self is met with an empty string.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfEmpty_DateTimeConditionOnSelfMetWithMinValue_ReadOnly()
        {
            var model = new Model() { Value9 = DateTime.MinValue };
            Assert.IsTrue(model.IsConditionMet("Value9"));
        }

        /// <summary>
        /// Test property is not read only if condition on self is met with a string that is not null or empty.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfEmpty_DateTimeConditionOnSelfMetWithNotEmpty_NotReadOnly()
        {
            var model = new Model() { Value9 = DateTime.MaxValue };
            Assert.IsFalse(model.IsConditionMet("Value9"));
        }

        /// <summary>
        /// Test property is read only if condition on self is met with a null value.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfEmpty_DateTimeConditionOnSelfMetWithNull_ReadOnly()
        {
            var model = new Model() { Value10 = null };
            Assert.IsTrue(model.IsConditionMet("Value10"));
        }

        /// <summary>
        /// Test property is not read only if condition is met with a null value and fail on null is true.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfEmpty_ConditionNotMetWithNullAndFailOnNull_NotReadOnly()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsConditionMet("Value11"));
        }

        /// <summary>
        /// Test property is read only if condition is met with an empty string and current property is null and fail on null is true.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfEmpty_ConditionMetWithEmptyStringAndFailOnNull_ReadOnly()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsTrue(model.IsConditionMet("Value11"));
        }

        /// <summary>
        /// Test property is not read only if condition is met with a null value and fail on null is true.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfEmpty_DateTimeConditionNotMetWithNullAndFailOnNull_NotReadOnly()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsConditionMet("Value12"));
        }
    }
}
