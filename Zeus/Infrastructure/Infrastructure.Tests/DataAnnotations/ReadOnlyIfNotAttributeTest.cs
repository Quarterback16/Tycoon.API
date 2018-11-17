using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="ReadOnlyIfNotAttribute" />.
    /// </summary>
    [TestClass]
    public class ReadOnlyIfNotAttributeTest
    {
        private class Model : ContingentModel<ReadOnlyIfNotAttribute>
        {
            public string Value1 { get; set; }

            [ReadOnlyIfNot("Value1", "hello")]
            public string Value2 { get; set; }

            [ReadOnlyIfNot("Value1", new [] { "hello", "world" })]
            public string Value3 { get; set; }

            [ReadOnlyIfNot("hello")]
            public string Value7 { get; set; }
        }

        /// <summary>
        /// Test property is read only if condition is met with a different value.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfNot_ConditionMetWithDifferentValue_ReadOnly()
        {
            var model = new Model() { Value1 = "goodbye" };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is read only if condition is met with an empty string.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfNot_ConditionMetWithEmptyString_ReadOnly()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is read only if condition is met with null.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfNot_ConditionMetWithNull_ReadOnly()
        {
            var model = new Model() { Value1 = null };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not read only if condition is met with the 'not' value.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfNot_ConditionMetWithNotValue_NotReadOnly()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is read only if condition is met with a different value.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfNot_ConditionMetWithDifferentValueToOneOfTheExpectedValues_ReadOnly()
        {
            var model = new Model() { Value1 = "goodbye" };
            Assert.IsTrue(model.IsConditionMet("Value3"));
        }

        /// <summary>
        /// Test property is read only if condition is met with an empty string.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfNot_ConditionMetWithEmptyStringInsteadOfOneOfTheExpectedValues_ReadOnly()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsTrue(model.IsConditionMet("Value3"));
        }

        /// <summary>
        /// Test property is read only if condition is met with null.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfNot_ConditionMetWithNullInsteadOfOneOfTheExpectedValues_ReadOnly()
        {
            var model = new Model() { Value1 = null };
            Assert.IsTrue(model.IsConditionMet("Value3"));
        }

        /// <summary>
        /// Test property is not read only if condition is met with the 'not' value.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfNot_ConditionMetWithOneOfTheExpectedValues_NotReadOnly()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsFalse(model.IsConditionMet("Value3"));
        }

        /// <summary>
        /// Test property is read only if condition on self is met with a different value.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfNot_ConditionOnSelfMetWithDifferentValue_ReadOnly()
        {
            var model = new Model() { Value7 = "goodbye" };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is read only if condition on self is met with an empty string.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfNot_ConditionOnSelfMetWithEmptyString_ReadOnly()
        {
            var model = new Model() { Value7 = string.Empty };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is read only if condition on self is met with null.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfNot_ConditionOnSelfMetWithNull_ReadOnly()
        {
            var model = new Model() { Value7 = null };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not read only if condition on self is met with the 'not' value.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfNot_ConditionOnSelfMetWithNotValue_NotReadOnly()
        {
            var model = new Model() { Value7 = "hello" };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }
    }
}
