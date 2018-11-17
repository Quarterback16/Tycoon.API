using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="VisibleIfNotAttribute" />.
    /// </summary>
    [TestClass]
    public class VisibleIfNotAttributeTest
    {
        private class Model : ContingentModel<VisibleIfNotAttribute>
        {
            public string Value1 { get; set; }

            [VisibleIfNot("Value1", "hello")]
            public string Value2 { get; set; }

            [VisibleIfNot("Value1", new [] { "hello", "world" })]
            public string Value3 { get; set; }

            [VisibleIfNot("hello")]
            public string Value7 { get; set; }
        }

        /// <summary>
        /// Test property is visible if condition is met with a different value.
        /// </summary>
        [TestMethod]
        public void VisibleIfNot_ConditionMetWithDifferentValue_Visible()
        {
            var model = new Model() { Value1 = "goodbye" };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is visible if condition is met with an empty string.
        /// </summary>
        [TestMethod]
        public void VisibleIfNot_ConditionMetWithEmptyString_Visible()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is visible if condition is met with null.
        /// </summary>
        [TestMethod]
        public void VisibleIfNot_ConditionMetWithNull_Visible()
        {
            var model = new Model() { Value1 = null };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not visible if condition is met with the 'not' value.
        /// </summary>
        [TestMethod]
        public void VisibleIfNot_ConditionMetWithNotValue_NotVisible()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is visible if condition is met with a different value.
        /// </summary>
        [TestMethod]
        public void VisibleIfNot_ConditionMetWithDifferentValueToOneOfTheExpectedValues_Visible()
        {
            var model = new Model() { Value1 = "goodbye" };
            Assert.IsTrue(model.IsConditionMet("Value3"));
        }

        /// <summary>
        /// Test property is visible if condition is met with an empty string.
        /// </summary>
        [TestMethod]
        public void VisibleIfNot_ConditionMetWithEmptyStringInsteadOfOneOfTheExpectedValues_Visible()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsTrue(model.IsConditionMet("Value3"));
        }

        /// <summary>
        /// Test property is visible if condition is met with null.
        /// </summary>
        [TestMethod]
        public void VisibleIfNot_ConditionMetWithNullInsteadOfOneOfTheExpectedValues_Visible()
        {
            var model = new Model() { Value1 = null };
            Assert.IsTrue(model.IsConditionMet("Value3"));
        }

        /// <summary>
        /// Test property is not visible if condition is met with the 'not' value.
        /// </summary>
        [TestMethod]
        public void VisibleIfNot_ConditionMetWithOneOfTheExpectedValues_NotVisible()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsFalse(model.IsConditionMet("Value3"));
        }

        /// <summary>
        /// Test property is visible if condition on self is met with a different value.
        /// </summary>
        [TestMethod]
        public void VisibleIfNot_ConditionOnSelfMetWithDifferentValue_Visible()
        {
            var model = new Model() { Value7 = "goodbye" };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is visible if condition on self is met with an empty string.
        /// </summary>
        [TestMethod]
        public void VisibleIfNot_ConditionOnSelfMetWithEmptyString_Visible()
        {
            var model = new Model() { Value7 = string.Empty };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is visible if condition on self is met with null.
        /// </summary>
        [TestMethod]
        public void VisibleIfNot_ConditionOnSelfMetWithNull_Visible()
        {
            var model = new Model() { Value7 = null };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not visible if condition on self is met with the 'not' value.
        /// </summary>
        [TestMethod]
        public void VisibleIfNot_ConditionOnSelfMetWithNotValue_NotVisible()
        {
            var model = new Model() { Value7 = "hello" };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }
    }
}
