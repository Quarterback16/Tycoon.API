using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Employment.Web.Mvc.Infrastructure.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="VisibleIfAttribute" />.
    /// </summary>
    [TestClass]
    public class VisibleIfAttributeTest
    {
        private class Model : ContingentModel<VisibleIfAttribute>
        {
            public string Value1 { get; set; }

            [VisibleIf("Value1", "hello")]
            public string Value2 { get; set; }

            [VisibleIf("Value1", "hello", PassOnNull = true)]
            public string Value3 { get; set; }

            [VisibleIf("Value1", new object[] { "hello", "world" })]
            public string Value4 { get; set; }

            [VisibleIf("hello")]
            public string Value7 { get; set; }

            [VisibleIf(ComparisonType.EqualTo, "hello")]
            public string Value8 { get; set; }

            [VisibleIf("Value1", "hello", FailOnNull = true)]
            public string Value9 { get; set; }
        }

        /// <summary>
        /// Test property is visible if condition is met with the expected value.
        /// </summary>
        [TestMethod]
        public void VisibleIf_ConditionMetWithExpectedValue_Visible()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not visible if condition is met with a different value.
        /// </summary>
        [TestMethod]
        public void VisibleIf_ConditionMetWithDifferentValue_NotVisible()
        {
            var model = new Model() { Value1 = "goodbye" };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not visible if condition is met with an empty string.
        /// </summary>
        [TestMethod]
        public void VisibleIf_ConditionMetWithEmptyString_NotVisible()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not visible if condition is met with a null value.
        /// </summary>
        [TestMethod]
        public void VisibleIf_ConditionMetWithNull_NotVisible()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is visible if condition is met with a null value and pass on null is true.
        /// </summary>
        [TestMethod]
        public void VisibleIf_ConditionMetWithNullAndPassOnNull_Visible()
        {
            var model = new Model() { Value1 = null };
            Assert.IsTrue(model.IsConditionMet("Value3"));
        }

        /// <summary>
        /// Test property is visible if condition is met with an empty string and current property is null and pass on null is true.
        /// </summary>
        [TestMethod]
        public void VisibleIf_ConditionMetWithEmptyStringAndPassOnNull_NotVisible()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsFalse(model.IsConditionMet("Value3"));
        }

        /// <summary>
        /// Test property is visible if condition is met with the expected value.
        /// </summary>
        [TestMethod]
        public void VisibleIf_ConditionMetWithOneOfTheExpectedValues_Visible()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsTrue(model.IsConditionMet("Value4"));
        }

        /// <summary>
        /// Test property is not visible if condition is met with a different value.
        /// </summary>
        [TestMethod]
        public void VisibleIf_ConditionMetWithDifferentValueToOneOfTheExpectedValues_NotVisible()
        {
            var model = new Model() { Value1 = "goodbye" };
            Assert.IsFalse(model.IsConditionMet("Value4"));
        }

        /// <summary>
        /// Test property is not visible if condition is met with an empty string.
        /// </summary>
        [TestMethod]
        public void VisibleIf_ConditionMetWithEmptyStringInsteadOfOneOfTheExpectedValues_NotVisible()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsFalse(model.IsConditionMet("Value4"));
        }

        /// <summary>
        /// Test property is not visible if condition is met with a null value.
        /// </summary>
        [TestMethod]
        public void VisibleIf_ConditionMetWithNullInsteadOfOneOfTheExpectedValues_NotVisible()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsConditionMet("Value4"));
        }

        /// <summary>
        /// Test property is visible if condition on self is met with the expected value.
        /// </summary>
        [TestMethod]
        public void VisibleIf_ConditionOnSelfMetWithOneOfTheExpectedValues_Visible()
        {
            var model = new Model() { Value7 = "hello" };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not visible if condition on self is met with a different value.
        /// </summary>
        [TestMethod]
        public void VisibleIf_ConditionOnSelfMetWithDifferentValueToOneOfTheExpectedValues_NotVisible()
        {
            var model = new Model() { Value7 = "goodbye" };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not visible if condition on self is met with an empty string.
        /// </summary>
        [TestMethod]
        public void VisibleIf_ConditionOnSelfMetWithEmptyStringInsteadOfOneOfTheExpectedValues_NotVisible()
        {
            var model = new Model() { Value7 = string.Empty };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not visible if condition on self is met with a null value.
        /// </summary>
        [TestMethod]
        public void VisibleIf_ConditionOnSelfMetWithNullInsteadOfOneOfTheExpectedValues_NotVisible()
        {
            var model = new Model() { Value7 = null };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is visible if condition on self is met with the expected value.
        /// </summary>
        [TestMethod]
        public void VisibleIf_ConditionOnSelfMetWithOneOfTheExpectedValuesComparisonTypeSpecified_Visible()
        {
            var model = new Model() { Value8 = "hello" };
            Assert.IsTrue(model.IsConditionMet("Value8"));
        }

        /// <summary>
        /// Test property is not visible if condition is met with a null value and fail on null is true.
        /// </summary>
        [TestMethod]
        public void VisibleIfEmpty_ConditionNotMetWithNullAndFailOnNull_NotVisible()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsConditionMet("Value9"));
        }
    }
}
