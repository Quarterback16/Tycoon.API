using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="BindableIfEmptyAttribute" />.
    /// </summary>
    [TestClass]
    public class BindableIfEmptyAttributeTest
    {
        private class Model : ContingentModel<BindableIfEmptyAttribute>
        {
            public string Value1 { get; set; }

            [BindableIfEmpty("Value1")]
            public string Value2 { get; set; }

            [BindableIfEmpty("Value1", PassOnNull = true)]
            public string Value3 { get; set; }

            [BindableIfEmpty]
            public string Value7 { get; set; }

            [BindableIfEmpty(PassOnNull = true)]
            public string Value8 { get; set; }

            [BindableIfEmpty]
            public DateTime Value9 { get; set; }

            [BindableIfEmpty(PassOnNull = true)]
            public DateTime? Value10 { get; set; }

            [BindableIfEmpty("Value1", FailOnNull = true)]
            public string Value11 { get; set; }

            [BindableIfEmpty("Value1", FailOnNull = true)]
            public DateTime? Value12 { get; set; }
        }

        /// <summary>
        /// Test property is bindable if condition is met with an empty string.
        /// </summary>
        [TestMethod]
        public void BindableIfEmpty_ConditionMetWithEmptyString_Bindable()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is bindable if condition is met with a null value.
        /// </summary>
        [TestMethod]
        public void BindableIfEmpty_ConditionMetWithNull_Bindable()
        {
            var model = new Model() { Value1 = null };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not bindable if condition is met with a string that is not null or empty.
        /// </summary>
        [TestMethod]
        public void BindableIfEmpty_ConditionMetWithNotEmptyOrNullString_NotBindable()
        {
            var model = new Model() { Value1 = "goodbye" };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is bindable if condition is met with a null value and pass on null is true.
        /// </summary>
        [TestMethod]
        public void BindableIfEmpty_ConditionMetWithNullAndPassOnNull_Bindable()
        {
            var model = new Model() { Value1 = null };
            Assert.IsTrue(model.IsConditionMet("Value3"));
        }

        /// <summary>
        /// Test property is bindable if condition is met with an empty string and current property is null and pass on null is true.
        /// </summary>
        [TestMethod]
        public void BindableIfEmpty_ConditionMetWithEmptyStringAndPassOnNull_Bindable()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsTrue(model.IsConditionMet("Value3"));
        }

        /// <summary>
        /// Test property is bindable if condition on self is met with an empty string.
        /// </summary>
        [TestMethod]
        public void BindableIfEmpty_ConditionOnSelfMetWithEmptyString_Bindable()
        {
            var model = new Model() { Value7 = string.Empty };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is bindable if condition on self is met with a null value.
        /// </summary>
        [TestMethod]
        public void BindableIfEmpty_ConditionOnSelfMetWithNull_Bindable()
        {
            var model = new Model() { Value7 = null };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not bindable if condition on self is met with a string that is not null or empty.
        /// </summary>
        [TestMethod]
        public void BindableIfEmpty_ConditionOnSelfMetWithNotEmptyOrNullString_NotBindable()
        {
            var model = new Model() { Value7 = "goodbye" };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is bindable if condition on self is met with a null value and pass on null is true.
        /// </summary>
        [TestMethod]
        public void BindableIfEmpty_ConditionOnSelfMetWithNullAndPassOnNull_Bindable()
        {
            var model = new Model() { Value8 = null };
            Assert.IsTrue(model.IsConditionMet("Value8"));
        }

        /// <summary>
        /// Test property is bindable if condition on self is met with an empty string and current property is null and pass on null is true.
        /// </summary>
        [TestMethod]
        public void BindableIfEmpty_ConditionOnSelfMetWithEmptyStringAndPassOnNull_Bindable()
        {
            var model = new Model() { Value8 = string.Empty };
            Assert.IsTrue(model.IsConditionMet("Value8"));
        }

        /// <summary>
        /// Test property is bindable if condition on self is met with an empty string.
        /// </summary>
        [TestMethod]
        public void BindableIfEmpty_DateTimeConditionOnSelfMetWithMinValue_Bindable()
        {
            var model = new Model() { Value9 = DateTime.MinValue };
            Assert.IsTrue(model.IsConditionMet("Value9"));
        }

        /// <summary>
        /// Test property is not bindable if condition on self is met with a string that is not null or empty.
        /// </summary>
        [TestMethod]
        public void BindableIfEmpty_DateTimeConditionOnSelfMetWithNotEmpty_NotBindable()
        {
            var model = new Model() { Value9 = DateTime.MaxValue };
            Assert.IsFalse(model.IsConditionMet("Value9"));
        }

        /// <summary>
        /// Test property is bindable if condition on self is met with a null value.
        /// </summary>
        [TestMethod]
        public void BindableIfEmpty_DateTimeConditionOnSelfMetWithNull_Bindable()
        {
            var model = new Model() { Value10 = null };
            Assert.IsTrue(model.IsConditionMet("Value10"));
        }

        /// <summary>
        /// Test property is not bindable if condition is met with a null value and fail on null is true.
        /// </summary>
        [TestMethod]
        public void BindableIfEmpty_ConditionNotMetWithNullAndFailOnNull_NotBindable()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsConditionMet("Value11"));
        }

        /// <summary>
        /// Test property is bindable if condition is met with an empty string and current property is null and fail on null is true.
        /// </summary>
        [TestMethod]
        public void BindableIfEmpty_ConditionMetWithEmptyStringAndFailOnNull_Bindable()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsTrue(model.IsConditionMet("Value11"));
        }

        /// <summary>
        /// Test property is not bindable if condition is met with a null value and fail on null is true.
        /// </summary>
        [TestMethod]
        public void BindableIfEmpty_DateTimeConditionNotMetWithNullAndFailOnNull_NotBindable()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsConditionMet("Value12"));
        }
    }
}
