using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="BindableIfNotAttribute" />.
    /// </summary>
    [TestClass]
    public class BindableIfNotAttributeTest
    {
        private class Model : ContingentModel<BindableIfNotAttribute>
        {
            public string Value1 { get; set; }

            [BindableIfNot("Value1", "hello")]
            public string Value2 { get; set; }

            [BindableIfNot("Value1", new [] { "hello", "world" })]
            public string Value3 { get; set; }

            [BindableIfNot("hello")]
            public string Value7 { get; set; }
        }

        /// <summary>
        /// Test property is bindable if condition is met with a different value.
        /// </summary>
        [TestMethod]
        public void BindableIfNot_ConditionMetWithDifferentValue_Bindable()
        {
            var model = new Model() { Value1 = "goodbye" };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is bindable if condition is met with an empty string.
        /// </summary>
        [TestMethod]
        public void BindableIfNot_ConditionMetWithEmptyString_Bindable()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is bindable if condition is met with null.
        /// </summary>
        [TestMethod]
        public void BindableIfNot_ConditionMetWithNull_Bindable()
        {
            var model = new Model() { Value1 = null };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not bindable if condition is met with the 'not' value.
        /// </summary>
        [TestMethod]
        public void BindableIfNot_ConditionMetWithNotValue_NotBindable()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is bindable if condition is met with a different value.
        /// </summary>
        [TestMethod]
        public void BindableIfNot_ConditionMetWithDifferentValueToOneOfTheExpectedValues_Bindable()
        {
            var model = new Model() { Value1 = "goodbye" };
            Assert.IsTrue(model.IsConditionMet("Value3"));
        }

        /// <summary>
        /// Test property is bindable if condition is met with an empty string.
        /// </summary>
        [TestMethod]
        public void BindableIfNot_ConditionMetWithEmptyStringInsteadOfOneOfTheExpectedValues_Bindable()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsTrue(model.IsConditionMet("Value3"));
        }

        /// <summary>
        /// Test property is bindable if condition is met with null.
        /// </summary>
        [TestMethod]
        public void BindableIfNot_ConditionMetWithNullInsteadOfOneOfTheExpectedValues_Bindable()
        {
            var model = new Model() { Value1 = null };
            Assert.IsTrue(model.IsConditionMet("Value3"));
        }

        /// <summary>
        /// Test property is not bindable if condition is met with the 'not' value.
        /// </summary>
        [TestMethod]
        public void BindableIfNot_ConditionMetWithOneOfTheExpectedValues_NotBindable()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsFalse(model.IsConditionMet("Value3"));
        }

        /// <summary>
        /// Test property is bindable if condition on self is met with a different value.
        /// </summary>
        [TestMethod]
        public void BindableIfNot_ConditionOnSelfMetWithDifferentValue_Bindable()
        {
            var model = new Model() { Value7 = "goodbye" };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is bindable if condition on self is met with an empty string.
        /// </summary>
        [TestMethod]
        public void BindableIfNot_ConditionOnSelfMetWithEmptyString_Bindable()
        {
            var model = new Model() { Value7 = string.Empty };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is bindable if condition on self is met with null.
        /// </summary>
        [TestMethod]
        public void BindableIfNot_ConditionOnSelfMetWithNull_Bindable()
        {
            var model = new Model() { Value7 = null };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not bindable if condition on self is met with the 'not' value.
        /// </summary>
        [TestMethod]
        public void BindableIfNot_ConditionOnSelfMetWithNotValue_NotBindable()
        {
            var model = new Model() { Value7 = "hello" };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }
    }
}
