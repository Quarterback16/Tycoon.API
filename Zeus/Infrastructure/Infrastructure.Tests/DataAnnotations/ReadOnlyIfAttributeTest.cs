using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Employment.Web.Mvc.Infrastructure.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="ReadOnlyIfAttribute" />.
    /// </summary>
    [TestClass]
    public class ReadOnlyIfAttributeTest
    {
        private class Model : ContingentModel<ReadOnlyIfAttribute>
        {
            public string Value1 { get; set; }

            [ReadOnlyIf("Value1", "hello")]
            public string Value2 { get; set; }

            [ReadOnlyIf("Value1", "hello", PassOnNull = true)]
            public string Value3 { get; set; }

            [ReadOnlyIf("Value1", new object[] { "hello", "world" })]
            public string Value4 { get; set; }

            [ReadOnlyIf("hello")]
            public string Value7 { get; set; }

            [ReadOnlyIf(ComparisonType.EqualTo, "hello")]
            public string Value8 { get; set; }

            [ReadOnlyIf("Value1", "hello", FailOnNull = true)]
            public string Value9 { get; set; }
        }

        /// <summary>
        /// Test property is read only if condition is met with the expected value.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIf_ConditionMetWithExpectedValue_ReadOnly()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not read only if condition is met with a different value.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIf_ConditionMetWithDifferentValue_NotReadOnly()
        {
            var model = new Model() { Value1 = "goodbye" };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not read only if condition is met with an empty string.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIf_ConditionMetWithEmptyString_NotReadOnly()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not read only if condition is met with a null value.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIf_ConditionMetWithNull_NotReadOnly()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is read only if condition is met with a null value and pass on null is true.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIf_ConditionMetWithNullAndPassOnNull_ReadOnly()
        {
            var model = new Model() { Value1 = null };
            Assert.IsTrue(model.IsConditionMet("Value3"));
        }

        /// <summary>
        /// Test property is not read only if condition is met with an empty string and pass on null is true.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIf_ConditionMetWithEmptyStringAndPassOnNull_NotReadOnly()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsFalse(model.IsConditionMet("Value3"));
        }

        /// <summary>
        /// Test property is read only if condition is met with the expected value.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIf_ConditionMetWithOneOfTheExpectedValues_ReadOnly()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsTrue(model.IsConditionMet("Value4"));
        }

        /// <summary>
        /// Test property is not read only if condition is met with a different value.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIf_ConditionMetWithDifferentValueToOneOfTheExpectedValues_NotReadOnly()
        {
            var model = new Model() { Value1 = "goodbye" };
            Assert.IsFalse(model.IsConditionMet("Value4"));
        }

        /// <summary>
        /// Test property is not read only if condition is met with an empty string.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIf_ConditionMetWithEmptyStringInsteadOfOneOfTheExpectedValues_NotReadOnly()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsFalse(model.IsConditionMet("Value4"));
        }

        /// <summary>
        /// Test property is not read only if condition is met with a null value.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIf_ConditionMetWithNullInsteadOfOneOfTheExpectedValues_NotReadOnly()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsConditionMet("Value4"));
        }

        /// <summary>
        /// Test property is read only if condition on self is met with the expected value.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIf_ConditionOnSelfMetWithOneOfTheExpectedValues_ReadOnly()
        {
            var model = new Model() { Value7 = "hello" };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not read only if condition on self is met with a different value.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIf_ConditionOnSelfMetWithDifferentValueToOneOfTheExpectedValues_NotReadOnly()
        {
            var model = new Model() { Value7 = "goodbye" };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not read only if condition on self is met with an empty string.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIf_ConditionOnSelfMetWithEmptyStringInsteadOfOneOfTheExpectedValues_NotReadOnly()
        {
            var model = new Model() { Value7 = string.Empty };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not read only if condition on self is met with a null value.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIf_ConditionOnSelfMetWithNullInsteadOfOneOfTheExpectedValues_NotReadOnly()
        {
            var model = new Model() { Value7 = null };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is read only if condition on self is met with the expected value.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIf_ConditionOnSelfMetWithOneOfTheExpectedValuesComparisonTypeSpecified_ReadOnly()
        {
            var model = new Model() { Value8 = "hello" };
            Assert.IsTrue(model.IsConditionMet("Value8"));
        }

        /// <summary>
        /// Test property is not read only if condition is met with a null value and fail on null is true.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfEmpty_ConditionNotMetWithNullAndFailOnNull_NotReadOnly()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsConditionMet("Value9"));
        }
    }
}
