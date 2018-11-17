using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="ClearIfNotAttribute" />.
    /// </summary>
    [TestClass]
    public class ClearIfNotAttributeTest
    {
        private class Model : ContingentModel<ClearIfNotAttribute>
        {
            public string Value1 { get; set; }

            [ClearIfNot("Value1", "hello")]
            public string Value2 { get; set; }

            [ClearIfNot("Value1", new [] { "hello", "world" })]
            public string Value3 { get; set; }
        }

        /// <summary>
        /// Test property is clear if condition is met with a different value.
        /// </summary>
        [TestMethod]
        public void ClearIfNot_ConditionMetWithDifferentValue_Clear()
        {
            var model = new Model() { Value1 = "goodbye" };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is clear if condition is met with an empty string.
        /// </summary>
        [TestMethod]
        public void ClearIfNot_ConditionMetWithEmptyString_Clear()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is clear if condition is met with null.
        /// </summary>
        [TestMethod]
        public void ClearIfNot_ConditionMetWithNull_Clear()
        {
            var model = new Model() { Value1 = null };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not clear if condition is met with the 'not' value.
        /// </summary>
        [TestMethod]
        public void ClearIfNot_ConditionMetWithNotValue_NotClear()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is clear if condition is met with a different value.
        /// </summary>
        [TestMethod]
        public void ClearIfNot_ConditionMetWithDifferentValueToOneOfTheExpectedValues_Clear()
        {
            var model = new Model() { Value1 = "goodbye" };
            Assert.IsTrue(model.IsConditionMet("Value3"));
        }

        /// <summary>
        /// Test property is clear if condition is met with an empty string.
        /// </summary>
        [TestMethod]
        public void ClearIfNot_ConditionMetWithEmptyStringInsteadOfOneOfTheExpectedValues_Clear()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsTrue(model.IsConditionMet("Value3"));
        }

        /// <summary>
        /// Test property is clear if condition is met with null.
        /// </summary>
        [TestMethod]
        public void ClearIfNot_ConditionMetWithNullInsteadOfOneOfTheExpectedValues_Clear()
        {
            var model = new Model() { Value1 = null };
            Assert.IsTrue(model.IsConditionMet("Value3"));
        }

        /// <summary>
        /// Test property is not clear if condition is met with the 'not' value.
        /// </summary>
        [TestMethod]
        public void ClearIfNot_ConditionMetWithOneOfTheExpectedValues_NotClear()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsFalse(model.IsConditionMet("Value3"));
        }
    }
}
