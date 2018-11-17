using System;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Employment.Web.Mvc.Infrastructure.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="BindableIfAttribute" />.
    /// </summary>
    [TestClass]
    public class BindableIfAttributeTest
    {
        private class Model : ContingentModel<BindableIfAttribute>
        {
            public string Value1 { get; set; }

            [BindableIf("Value1", "hello")]
            public string Value2 { get; set; }

            [BindableIf("Value1", "hello", PassOnNull = true)]
            public string Value3 { get; set; }

            [BindableIf("Value1", new object[] { "hello", "world" })]
            public string Value4 { get; set; }

            [BindableIf(null)]
            public string Value5 { get; set; }

            [BindableIf("NotExist", "hello")]
            public string Value6 { get; set; }

            [BindableIf("hello")]
            public string Value7 { get; set; }

            [BindableIf(ComparisonType.EqualTo, "hello")]
            public string Value8 { get; set; }

            [BindableIf("Value1", "hello", FailOnNull = true)]
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
        /// Test property is bindable if condition is met with the expected value.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void BindableIf_InvalidDependentProperty_ThrowsInvalidOperationException()
        {
            var model = new Model() { Value6 = "hello" };
            model.IsConditionMet("Value6");
        }

        /// <summary>
        /// Test property is bindable if condition is met with the expected value.
        /// </summary>
        [TestMethod]
        public void BindableIf_ConditionMetWithExpectedValue_Bindable()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not bindable if condition is met with a different value.
        /// </summary>
        [TestMethod]
        public void BindableIf_ConditionMetWithDifferentValue_NotBindable()
        {
            var model = new Model() { Value1 = "goodbye" };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not bindable if condition is met with an empty string.
        /// </summary>
        [TestMethod]
        public void BindableIf_ConditionMetWithEmptyString_NotBindable()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not bindable if condition is met with a null value.
        /// </summary>
        [TestMethod]
        public void BindableIf_ConditionMetWithNull_NotBindable()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is bindable if condition is met with a null value and pass on null is true.
        /// </summary>
        [TestMethod]
        public void BindableIf_ConditionMetWithNullAndPassOnNull_Bindable()
        {
            var model = new Model() { Value1 = null };
            Assert.IsTrue(model.IsConditionMet("Value3"));
        }

        /// <summary>
        /// Test property is not bindable if condition is met with an empty string and pass on null is true.
        /// </summary>
        [TestMethod]
        public void BindableIf_ConditionMetWithEmptyStringAndPassOnNull_NotBindable()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsFalse(model.IsConditionMet("Value3"));
        }

        /// <summary>
        /// Test property is bindable if condition is met with the expected value.
        /// </summary>
        [TestMethod]
        public void BindableIf_ConditionMetWithOneOfTheExpectedValues_Bindable()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsTrue(model.IsConditionMet("Value4"));
        }

        /// <summary>
        /// Test property is not bindable if condition is met with a different value.
        /// </summary>
        [TestMethod]
        public void BindableIf_ConditionMetWithDifferentValueToOneOfTheExpectedValues_NotBindable()
        {
            var model = new Model() { Value1 = "goodbye" };
            Assert.IsFalse(model.IsConditionMet("Value4"));
        }

        /// <summary>
        /// Test property is not bindable if condition is met with an empty string.
        /// </summary>
        [TestMethod]
        public void BindableIf_ConditionMetWithEmptyStringInsteadOfOneOfTheExpectedValues_NotBindable()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsFalse(model.IsConditionMet("Value4"));
        }

        /// <summary>
        /// Test property is not bindable if condition is met with a null value.
        /// </summary>
        [TestMethod]
        public void BindableIf_ConditionMetWithNullInsteadOfOneOfTheExpectedValues_NotBindable()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsConditionMet("Value4"));
        }
        
        /// <summary>
        /// Test property is bindable if condition on self is met with the expected value.
        /// </summary>
        [TestMethod]
        public void BindableIf_ConditionOnSelfMetWithOneOfTheExpectedValues_Bindable()
        {
            var model = new Model() { Value7 = "hello" };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not bindable if condition on self is met with a different value.
        /// </summary>
        [TestMethod]
        public void BindableIf_ConditionOnSelfMetWithDifferentValueToOneOfTheExpectedValues_NotBindable()
        {
            var model = new Model() { Value7 = "goodbye" };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not bindable if condition on self is met with an empty string.
        /// </summary>
        [TestMethod]
        public void BindableIf_ConditionOnSelfMetWithEmptyStringInsteadOfOneOfTheExpectedValues_NotBindable()
        {
            var model = new Model() { Value7 = string.Empty };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not bindable if condition on self is met with a null value.
        /// </summary>
        [TestMethod]
        public void BindableIf_ConditionOnSelfMetWithNullInsteadOfOneOfTheExpectedValues_NotBindable()
        {
            var model = new Model() { Value7 = null };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is bindable if condition on self is met with the expected value.
        /// </summary>
        [TestMethod]
        public void BindableIf_ConditionOnSelfMetWithOneOfTheExpectedValuesComparisonTypeSpecified_Bindable()
        {
            var model = new Model() { Value8 = "hello" };
            Assert.IsTrue(model.IsConditionMet("Value8"));
        }

        /// <summary>
        /// Test property is not bindable if condition is met with a null value and fail on null is true.
        /// </summary>
        [TestMethod]
        public void BindableIfEmpty_ConditionNotMetWithNullAndFailOnNull_NotBindable()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsConditionMet("Value9"));
        }
    }
}
