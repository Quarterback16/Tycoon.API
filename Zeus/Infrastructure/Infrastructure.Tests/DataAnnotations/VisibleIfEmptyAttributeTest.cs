using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="VisibleIfEmptyAttribute" />.
    /// </summary>
    [TestClass]
    public class VisibleIfEmptyAttributeTest
    {
        private class Model : ContingentModel<VisibleIfEmptyAttribute>
        {
            public string Value1 { get; set; }

            [VisibleIfEmpty("Value1")]
            public string Value2 { get; set; }

            [VisibleIfEmpty("Value1", PassOnNull = true)]
            public string Value3 { get; set; }

            [VisibleIfEmpty]
            public string Value7 { get; set; }

            [VisibleIfEmpty(PassOnNull = true)]
            public string Value8 { get; set; }

            [VisibleIfEmpty]
            public DateTime Value9 { get; set; }

            [VisibleIfEmpty(PassOnNull = true)]
            public DateTime? Value10 { get; set; }

            [VisibleIfEmpty("Value1", FailOnNull = true)]
            public string Value11 { get; set; }

            [VisibleIfEmpty("Value1", FailOnNull = true)]
            public DateTime? Value12 { get; set; }
        }

        /// <summary>
        /// Test property is visible if condition is met with an empty string.
        /// </summary>
        [TestMethod]
        public void VisibleIfEmpty_ConditionMetWithEmptyString_Visible()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is visible if condition is met with a null value.
        /// </summary>
        [TestMethod]
        public void VisibleIfEmpty_ConditionMetWithNull_Visible()
        {
            var model = new Model() { Value1 = null };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not visible if condition is met with a string that is not null or empty.
        /// </summary>
        [TestMethod]
        public void VisibleIfEmpty_ConditionMetWithNotEmptyOrNullString_NotVisible()
        {
            var model = new Model() { Value1 = "goodbye" };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is visible if condition is met with a null value and pass on null is true.
        /// </summary>
        [TestMethod]
        public void VisibleIfEmpty_ConditionMetWithNullAndPassOnNull_Visible()
        {
            var model = new Model() { Value1 = null };
            Assert.IsTrue(model.IsConditionMet("Value3"));
        }

        /// <summary>
        /// Test property is visible if condition is met with an empty string and current property is null and pass on null is true.
        /// </summary>
        [TestMethod]
        public void VisibleIfEmpty_ConditionMetWithEmptyStringAndPassOnNull_Visible()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsTrue(model.IsConditionMet("Value3"));
        }

        /// <summary>
        /// Test property is visible if condition on self is met with an empty string.
        /// </summary>
        [TestMethod]
        public void VisibleIfEmpty_ConditionOnSelfMetWithEmptyString_Visible()
        {
            var model = new Model() { Value7 = string.Empty };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is visible if condition on self is met with a null value.
        /// </summary>
        [TestMethod]
        public void VisibleIfEmpty_ConditionOnSelfMetWithNull_Visible()
        {
            var model = new Model() { Value7 = null };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not visible if condition on self is met with a string that is not null or empty.
        /// </summary>
        [TestMethod]
        public void VisibleIfEmpty_ConditionOnSelfMetWithNotEmptyOrNullString_NotVisible()
        {
            var model = new Model() { Value7 = "goodbye" };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is visible if condition on self is met with a null value and pass on null is true.
        /// </summary>
        [TestMethod]
        public void VisibleIfEmpty_ConditionOnSelfMetWithNullAndPassOnNull_Visible()
        {
            var model = new Model() { Value8 = null };
            Assert.IsTrue(model.IsConditionMet("Value8"));
        }

        /// <summary>
        /// Test property is visible if condition on self is met with an empty string and current property is null and pass on null is true.
        /// </summary>
        [TestMethod]
        public void VisibleIfEmpty_ConditionOnSelfMetWithEmptyStringAndPassOnNull_Visible()
        {
            var model = new Model() { Value8 = string.Empty };
            Assert.IsTrue(model.IsConditionMet("Value8"));
        }

        /// <summary>
        /// Test property is visible if condition on self is met with an empty string.
        /// </summary>
        [TestMethod]
        public void VisibleIfEmpty_DateTimeConditionOnSelfMetWithMinValue_Visible()
        {
            var model = new Model() { Value9 = DateTime.MinValue };
            Assert.IsTrue(model.IsConditionMet("Value9"));
        }

        /// <summary>
        /// Test property is not visible if condition on self is met with a string that is not null or empty.
        /// </summary>
        [TestMethod]
        public void VisibleIfEmpty_DateTimeConditionOnSelfMetWithNotEmpty_NotVisible()
        {
            var model = new Model() { Value9 = DateTime.MaxValue };
            Assert.IsFalse(model.IsConditionMet("Value9"));
        }

        /// <summary>
        /// Test property is visible if condition on self is met with a null value.
        /// </summary>
        [TestMethod]
        public void VisibleIfEmpty_DateTimeConditionOnSelfMetWithNull_Visible()
        {
            var model = new Model() { Value10 = null };
            Assert.IsTrue(model.IsConditionMet("Value10"));
        }

        /// <summary>
        /// Test property is not visible if condition is met with a null value and fail on null is true.
        /// </summary>
        [TestMethod]
        public void VisibleIfEmpty_ConditionNotMetWithNullAndFailOnNull_NotVisible()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsConditionMet("Value11"));
        }

        /// <summary>
        /// Test property is visible if condition is met with an empty string and current property is null and fail on null is true.
        /// </summary>
        [TestMethod]
        public void VisibleIfEmpty_ConditionMetWithEmptyStringAndFailOnNull_Visible()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsTrue(model.IsConditionMet("Value11"));
        }

        /// <summary>
        /// Test property is not visible if condition is met with a null value and fail on null is true.
        /// </summary>
        [TestMethod]
        public void VisibleIfEmpty_DateTimeConditionNotMetWithNullAndFailOnNull_NotVisible()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsConditionMet("Value12"));
        }
    }
}
