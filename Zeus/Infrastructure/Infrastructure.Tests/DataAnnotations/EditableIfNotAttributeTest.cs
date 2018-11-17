using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="EditableIfNotAttribute" />.
    /// </summary>
    [TestClass]
    public class EditableIfNotAttributeTest
    {
        private class Model : ContingentModel<EditableIfNotAttribute>
        {
            public string Value1 { get; set; }

            [EditableIfNot("Value1", "hello")]
            public string Value2 { get; set; }

            [EditableIfNot("Value1", new [] { "hello", "world" })]
            public string Value3 { get; set; }

            [EditableIfNot("hello")]
            public string Value7 { get; set; }
        }

        /// <summary>
        /// Test property is editable if condition is met with a different value.
        /// </summary>
        [TestMethod]
        public void EditableIfNot_ConditionMetWithDifferentValue_Editable()
        {
            var model = new Model() { Value1 = "goodbye" };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is editable if condition is met with an empty string.
        /// </summary>
        [TestMethod]
        public void EditableIfNot_ConditionMetWithEmptyString_Editable()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is editable if condition is met with null.
        /// </summary>
        [TestMethod]
        public void EditableIfNot_ConditionMetWithNull_Editable()
        {
            var model = new Model() { Value1 = null };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not editable if condition is met with the 'not' value.
        /// </summary>
        [TestMethod]
        public void EditableIfNot_ConditionMetWithNotValue_NotEditable()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is editable if condition is met with a different value.
        /// </summary>
        [TestMethod]
        public void EditableIfNot_ConditionMetWithDifferentValueToOneOfTheExpectedValues_Editable()
        {
            var model = new Model() { Value1 = "goodbye" };
            Assert.IsTrue(model.IsConditionMet("Value3"));
        }

        /// <summary>
        /// Test property is editable if condition is met with an empty string.
        /// </summary>
        [TestMethod]
        public void EditableIfNot_ConditionMetWithEmptyStringInsteadOfOneOfTheExpectedValues_Editable()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsTrue(model.IsConditionMet("Value3"));
        }

        /// <summary>
        /// Test property is editable if condition is met with null.
        /// </summary>
        [TestMethod]
        public void EditableIfNot_ConditionMetWithNullInsteadOfOneOfTheExpectedValues_Editable()
        {
            var model = new Model() { Value1 = null };
            Assert.IsTrue(model.IsConditionMet("Value3"));
        }

        /// <summary>
        /// Test property is not editable if condition is met with the 'not' value.
        /// </summary>
        [TestMethod]
        public void EditableIfNot_ConditionMetWithOneOfTheExpectedValues_NotEditable()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsFalse(model.IsConditionMet("Value3"));
        }

        /// <summary>
        /// Test property is editable if condition on self is met with a different value.
        /// </summary>
        [TestMethod]
        public void EditableIfNot_ConditionOnSelfMetWithDifferentValue_Editable()
        {
            var model = new Model() { Value7 = "goodbye" };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is editable if condition on self is met with an empty string.
        /// </summary>
        [TestMethod]
        public void EditableIfNot_ConditionOnSelfMetWithEmptyString_Editable()
        {
            var model = new Model() { Value7 = string.Empty };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is editable if condition on self is met with null.
        /// </summary>
        [TestMethod]
        public void EditableIfNot_ConditionOnSelfMetWithNull_Editable()
        {
            var model = new Model() { Value7 = null };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not editable if condition on self is met with the 'not' value.
        /// </summary>
        [TestMethod]
        public void EditableIfNot_ConditionOnSelfMetWithNotValue_NotEditable()
        {
            var model = new Model() { Value7 = "hello" };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }
    }
}
