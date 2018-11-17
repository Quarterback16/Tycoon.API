using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="ClearIfEmptyAttribute" />.
    /// </summary>
    [TestClass]
    public class ClearIfEmptyAttributeTest
    {
        private class Model : ContingentModel<ClearIfEmptyAttribute>
        {
            public string Value1 { get; set; }

            [ClearIfEmpty("Value1")]
            public string Value2 { get; set; }

            [ClearIfEmpty("Value1", PassOnNull = true)]
            public string Value3 { get; set; }

            [ClearIfEmpty("Value1", FailOnNull = true)]
            public string Value4 { get; set; }

            [ClearIfEmpty("Value1", FailOnNull = true)]
            public DateTime? Value5 { get; set; }
        }

        /// <summary>
        /// Test property is clear if condition is met with an empty string.
        /// </summary>
        [TestMethod]
        public void ClearIfEmpty_ConditionMetWithEmptyString_Clear()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is clear if condition is met with a null value.
        /// </summary>
        [TestMethod]
        public void ClearIfEmpty_ConditionMetWithNull_Clear()
        {
            var model = new Model() { Value1 = null };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not clear if condition is met with a string that is not null or empty.
        /// </summary>
        [TestMethod]
        public void ClearIfEmpty_ConditionMetWithNotEmptyOrNullString_NotClear()
        {
            var model = new Model() { Value1 = "goodbye" };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is clear if condition is met with a null value and pass on null is true.
        /// </summary>
        [TestMethod]
        public void ClearIfEmpty_ConditionMetWithNullAndPassOnNull_Clear()
        {
            var model = new Model() { Value1 = null };
            Assert.IsTrue(model.IsConditionMet("Value3"));
        }

        /// <summary>
        /// Test property is clear if condition is met with an empty string and current property is null and pass on null is true.
        /// </summary>
        [TestMethod]
        public void ClearIfEmpty_ConditionMetWithEmptyStringAndPassOnNull_Clear()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsTrue(model.IsConditionMet("Value3"));
        }

        /// <summary>
        /// Test property is not cleared if condition is met with a null value and fail on null is true.
        /// </summary>
        [TestMethod]
        public void ClearIfEmpty_ConditionNotMetWithNullAndFailOnNull_NotClear()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsConditionMet("Value4"));
        }

        /// <summary>
        /// Test property is cleared if condition is met with an empty string and current property is null and fail on null is true.
        /// </summary>
        [TestMethod]
        public void ClearIfEmpty_ConditionMetWithEmptyStringAndFailOnNull_Clear()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsTrue(model.IsConditionMet("Value4"));
        }

        /// <summary>
        /// Test property is not cleared if condition is met with a null value and fail on null is true.
        /// </summary>
        [TestMethod]
        public void ClearIfEmpty_DateTimeConditionNotMetWithNullAndFailOnNull_NotClear()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsConditionMet("Value5"));
        }
    }
}
