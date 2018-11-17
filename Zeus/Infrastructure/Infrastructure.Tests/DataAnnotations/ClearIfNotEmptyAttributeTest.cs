using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="ClearIfNotEmptyAttribute" />.
    /// </summary>
    [TestClass]
    public class ClearIfNotEmptyAttributeTest
    {
        private class Model : ContingentModel<ClearIfNotEmptyAttribute>
        {
            public string Value1 { get; set; }

            [ClearIfNotEmpty("Value1")]
            public string Value2 { get; set; }

            [ClearIfNotEmpty("Value1", FailOnNull = true)]
            public string Value11 { get; set; }

            [ClearIfNotEmpty("Value1", FailOnNull = true)]
            public DateTime? Value12 { get; set; }
        }

        /// <summary>
        /// Test property is clear if condition is met with a different value.
        /// </summary>
        [TestMethod]
        public void ClearIfNotEmpty_ConditionMetWithDifferentValue_Clear()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not clear if condition is met with an empty string.
        /// </summary>
        [TestMethod]
        public void ClearIfNotEmpty_ConditionMetWithEmptyString_NotClear()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is clear if condition is met with null.
        /// </summary>
        [TestMethod]
        public void ClearIfNotEmpty_ConditionMetWithNull_Clear()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not clear if condition not is met with a null value and fail on null is true.
        /// </summary>
        [TestMethod]
        public void ClearIfNotEmpty_ConditionNotMetWithNullAndFailOnNull_NotClear()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsConditionMet("Value11"));
        }

        /// <summary>
        /// Test property is clear if condition is not met with an empty string and current property is null and fail on null is true.
        /// </summary>
        [TestMethod]
        public void ClearIfNotEmpty_ConditionMetWithEmptyStringAndFailOnNull_NotClear()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsFalse(model.IsConditionMet("Value11"));
        }

        /// <summary>
        /// Test property is clear if condition is not met with an empty string and current property is null and fail on null is true.
        /// </summary>
        [TestMethod]
        public void ClearIfNotEmpty_ConditionMetWithValueAndFailOnNull_Clear()
        {
            var model = new Model() { Value1 = "Value" };
            Assert.IsTrue(model.IsConditionMet("Value11"));
        }

        /// <summary>
        /// Test property is not clear if condition not is met with a null value and fail on null is true.
        /// </summary>
        [TestMethod]
        public void ClearIfNotEmpty_DateTimeConditionNotMetWithNullAndFailOnNull_NotClear()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsConditionMet("Value12"));
        }
    }
}
