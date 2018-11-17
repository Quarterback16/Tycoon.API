using System;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Employment.Web.Mvc.Infrastructure.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="EditableIfAttribute" />.
    /// </summary>
    [TestClass]
    public class EditableIfAttributeTest
    {
        private class Model : ContingentModel<EditableIfAttribute>
        {
            public string Value1 { get; set; }

            [EditableIf("Value1", "hello")]
            public string Value2 { get; set; }

            [EditableIf("Value1", "hello", PassOnNull = true)]
            public string Value3 { get; set; }

            [EditableIf("Value1", new object[] { "hello", "world" })]
            public string Value4 { get; set; }

            [EditableIf(null)]
            public string Value5 { get; set; }

            [EditableIf("NotExist", "hello")]
            public string Value6 { get; set; }

            [EditableIf("hello")]
            public string Value7 { get; set; }

            [EditableIf(ComparisonType.EqualTo, "hello")]
            public string Value8 { get; set; }

            [EditableIf("Value1", "hello", FailOnNull = true)]
            public string Value9 { get; set; }
        }
        
        /// <summary>
        /// Test when called with null dependent property and null property it assumes valid as it has no property to compare against.
        /// </summary>
        [TestMethod]
        public void Constructor_CalledWithNullDependentPropertyAndNullProperty_AssumesValid()
        {
            var model = new Model() { Value5 = "hello" };

            var attribute = model.GetAttribute("Value5");

            Assert.IsTrue(attribute.IsConditionMet(model));
        }

        /// <summary>
        /// Test property is editable if condition is met with the expected value.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EditableIf_InvalidDependentProperty_ThrowsInvalidOperationException()
        {
            var model = new Model() { Value6 = "hello" };
            model.IsConditionMet("Value6");
        }

        /// <summary>
        /// Test property is editable if condition is met with the expected value.
        /// </summary>
        [TestMethod]
        public void EditableIf_ConditionMetWithExpectedValue_Editable()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not editable if condition is met with a different value.
        /// </summary>
        [TestMethod]
        public void EditableIf_ConditionMetWithDifferentValue_NotEditable()
        {
            var model = new Model() { Value1 = "goodbye" };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not editable if condition is met with an empty string.
        /// </summary>
        [TestMethod]
        public void EditableIf_ConditionMetWithEmptyString_NotEditable()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not editable if condition is met with a null value.
        /// </summary>
        [TestMethod]
        public void EditableIf_ConditionMetWithNull_NotEditable()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is editable if condition is met with a null value and pass on null is true.
        /// </summary>
        [TestMethod]
        public void EditableIf_ConditionMetWithNullAndPassOnNull_Editable()
        {
            var model = new Model() { Value1 = null };
            Assert.IsTrue(model.IsConditionMet("Value3"));
        }

        /// <summary>
        /// Test property is not editable if condition is met with an empty string and pass on null is true.
        /// </summary>
        [TestMethod]
        public void EditableIf_ConditionMetWithEmptyStringAndPassOnNull_NotEditable()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsFalse(model.IsConditionMet("Value3"));
        }

        /// <summary>
        /// Test property is editable if condition is met with the expected value.
        /// </summary>
        [TestMethod]
        public void EditableIf_ConditionMetWithOneOfTheExpectedValues_Editable()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsTrue(model.IsConditionMet("Value4"));
        }

        /// <summary>
        /// Test property is not editable if condition is met with a different value.
        /// </summary>
        [TestMethod]
        public void EditableIf_ConditionMetWithDifferentValueToOneOfTheExpectedValues_NotEditable()
        {
            var model = new Model() { Value1 = "goodbye" };
            Assert.IsFalse(model.IsConditionMet("Value4"));
        }

        /// <summary>
        /// Test property is not editable if condition is met with an empty string.
        /// </summary>
        [TestMethod]
        public void EditableIf_ConditionMetWithEmptyStringInsteadOfOneOfTheExpectedValues_NotEditable()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsFalse(model.IsConditionMet("Value4"));
        }

        /// <summary>
        /// Test property is not editable if condition is met with a null value.
        /// </summary>
        [TestMethod]
        public void EditableIf_ConditionMetWithNullInsteadOfOneOfTheExpectedValues_NotEditable()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsConditionMet("Value4"));
        }
        
        /// <summary>
        /// Test property is editable if condition on self is met with the expected value.
        /// </summary>
        [TestMethod]
        public void EditableIf_ConditionOnSelfMetWithOneOfTheExpectedValues_Editable()
        {
            var model = new Model() { Value7 = "hello" };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not editable if condition on self is met with a different value.
        /// </summary>
        [TestMethod]
        public void EditableIf_ConditionOnSelfMetWithDifferentValueToOneOfTheExpectedValues_NotEditable()
        {
            var model = new Model() { Value7 = "goodbye" };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not editable if condition on self is met with an empty string.
        /// </summary>
        [TestMethod]
        public void EditableIf_ConditionOnSelfMetWithEmptyStringInsteadOfOneOfTheExpectedValues_NotEditable()
        {
            var model = new Model() { Value7 = string.Empty };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not editable if condition on self is met with a null value.
        /// </summary>
        [TestMethod]
        public void EditableIf_ConditionOnSelfMetWithNullInsteadOfOneOfTheExpectedValues_NotEditable()
        {
            var model = new Model() { Value7 = null };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is editable if condition on self is met with the expected value.
        /// </summary>
        [TestMethod]
        public void EditableIf_ConditionOnSelfMetWithOneOfTheExpectedValuesComparisonTypeSpecified_Editable()
        {
            var model = new Model() { Value8 = "hello" };
            Assert.IsTrue(model.IsConditionMet("Value8"));
        }

        /// <summary>
        /// Test property is not editable if condition is met with a null value and fail on null is true.
        /// </summary>
        [TestMethod]
        public void EditableIfEmpty_ConditionNotMetWithNullAndFailOnNull_NotEditable()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsConditionMet("Value9"));
        }
    }
}
