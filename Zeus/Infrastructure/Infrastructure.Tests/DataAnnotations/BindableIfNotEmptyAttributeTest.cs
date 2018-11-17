using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="BindableIfNotEmptyAttribute" />.
    /// </summary>
    [TestClass]
    public class BindableIfNotEmptyAttributeTest
    {
        private class Model : ContingentModel<BindableIfNotEmptyAttribute>
        {
            public string Value1 { get; set; }

            [BindableIfNotEmpty("Value1")]
            public string Value2 { get; set; }

            [BindableIfNotEmpty]
            public string Value7 { get; set; }

            [BindableIfNotEmpty]
            public DateTime Value9 { get; set; }

            [BindableIfNotEmpty(PassOnNull = true)]
            public DateTime? Value10 { get; set; }

            [BindableIfNotEmpty("Value1", FailOnNull = true)]
            public string Value11 { get; set; }

            [BindableIfNotEmpty("Value1", FailOnNull = true)]
            public DateTime? Value12 { get; set; }
        }

        /// <summary>
        /// Test property is bindable if condition is met with a different value.
        /// </summary>
        [TestMethod]
        public void BindableIfNotEmpty_ConditionMetWithDifferentValue_Bindable()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not bindable if condition is met with an empty string.
        /// </summary>
        [TestMethod]
        public void BindableIfNotEmpty_ConditionMetWithEmptyString_NotBindable()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not bindable if condition is met with null.
        /// </summary>
        [TestMethod]
        public void BindableIfNotEmpty_ConditionMetWithNull_NotBindable()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is bindable if condition on self is met with a different value.
        /// </summary>
        [TestMethod]
        public void BindableIfNotEmpty_ConditionOnSelfMetWithDifferentValue_Bindable()
        {
            var model = new Model() { Value7 = "hello" };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not bindable if condition on self is met with an empty string.
        /// </summary>
        [TestMethod]
        public void BindableIfNotEmpty_ConditionOnSelfMetWithEmptyString_NotBindable()
        {
            var model = new Model() { Value7 = string.Empty };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not bindable if condition on self is met with null.
        /// </summary>
        [TestMethod]
        public void BindableIfNotEmpty_ConditionOnSelfMetWithNull_NotBindable()
        {
            var model = new Model() { Value7 = null };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is bindable if condition on self is met with an empty string.
        /// </summary>
        [TestMethod]
        public void BindableIfNotEmpty_DateTimeConditionOnSelfMetWithMinValue_NotBindable()
        {
            var model = new Model() { Value9 = DateTime.MinValue };
            Assert.IsFalse(model.IsConditionMet("Value9"));
        }

        /// <summary>
        /// Test property is not bindable if condition on self is met with a string that is not null or empty.
        /// </summary>
        [TestMethod]
        public void BindableIfNotEmpty_DateTimeConditionOnSelfMetWithNotEmpty_Bindable()
        {
            var model = new Model() { Value9 = DateTime.MaxValue };
            Assert.IsTrue(model.IsConditionMet("Value9"));
        }

        /// <summary>
        /// Test property is bindable if condition on self is met with a null value and pass on null is true.
        /// </summary>
        [TestMethod]
        public void BindableIfNotEmpty_DateTimeConditionOnSelfMetWithNullAnPassOnNull_Bindable()
        {
            var model = new Model() { Value10 = null };
            Assert.IsTrue(model.IsConditionMet("Value10"));
        }

        /// <summary>
        /// Test property is not bindable if condition not is met with a null value and fail on null is true.
        /// </summary>
        [TestMethod]
        public void BindableIfNotEmpty_ConditionNotMetWithNullAndFailOnNull_NotBindable()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsConditionMet("Value11"));
        }

        /// <summary>
        /// Test property is bindable if condition is not met with an empty string and current property is null and fail on null is true.
        /// </summary>
        [TestMethod]
        public void BindableIfNotEmpty_ConditionMetWithEmptyStringAndFailOnNull_NotBindable()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsFalse(model.IsConditionMet("Value11"));
        }

        /// <summary>
        /// Test property is bindable if condition is not met with an empty string and current property is null and fail on null is true.
        /// </summary>
        [TestMethod]
        public void BindableIfNotEmpty_ConditionMetWithValueAndFailOnNull_Bindable()
        {
            var model = new Model() { Value1 = "Value" };
            Assert.IsTrue(model.IsConditionMet("Value11"));
        }

        /// <summary>
        /// Test property is not bindable if condition not is met with a null value and fail on null is true.
        /// </summary>
        [TestMethod]
        public void BindableIfNotEmpty_DateTimeConditionNotMetWithNullAndFailOnNull_NotBindable()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsConditionMet("Value12"));
        }
    }
}
