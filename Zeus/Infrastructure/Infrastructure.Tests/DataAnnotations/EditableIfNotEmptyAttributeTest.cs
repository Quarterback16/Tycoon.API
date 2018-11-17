using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="EditableIfNotEmptyAttribute" />.
    /// </summary>
    [TestClass]
    public class EditableIfNotEmptyAttributeTest
    {
        private class Model : ContingentModel<EditableIfNotEmptyAttribute>
        {
            public string Value1 { get; set; }

            [EditableIfNotEmpty("Value1")]
            public string Value2 { get; set; }

            [EditableIfNotEmpty]
            public string Value7 { get; set; }

            [EditableIfNotEmpty]
            public DateTime Value9 { get; set; }

            [EditableIfNotEmpty(PassOnNull = true)]
            public DateTime? Value10 { get; set; }

            [EditableIfNotEmpty("Value1", FailOnNull = true)]
            public string Value11 { get; set; }

            [EditableIfNotEmpty("Value1", FailOnNull = true)]
            public DateTime? Value12 { get; set; }
        }

        /// <summary>
        /// Test property is editable if condition is met with a different value.
        /// </summary>
        [TestMethod]
        public void EditableIfNotEmpty_ConditionMetWithDifferentValue_Editable()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not editable if condition is met with an empty string.
        /// </summary>
        [TestMethod]
        public void EditableIfNotEmpty_ConditionMetWithEmptyString_NotEditable()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not editable if condition is met with null.
        /// </summary>
        [TestMethod]
        public void EditableIfNotEmpty_ConditionMetWithNull_NotEditable()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is editable if condition on self is met with a different value.
        /// </summary>
        [TestMethod]
        public void EditableIfNotEmpty_ConditionOnSelfMetWithDifferentValue_Editable()
        {
            var model = new Model() { Value7 = "hello" };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not editable if condition on self is met with an empty string.
        /// </summary>
        [TestMethod]
        public void EditableIfNotEmpty_ConditionOnSelfMetWithEmptyString_NotEditable()
        {
            var model = new Model() { Value7 = string.Empty };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not editable if condition on self is met with null.
        /// </summary>
        [TestMethod]
        public void EditableIfNotEmpty_ConditionOnSelfMetWithNull_NotEditable()
        {
            var model = new Model() { Value7 = null };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is editable if condition on self is met with an empty string.
        /// </summary>
        [TestMethod]
        public void EditableIfNotEmpty_DateTimeConditionOnSelfMetWithMinValue_NotEditable()
        {
            var model = new Model() { Value9 = DateTime.MinValue };
            Assert.IsFalse(model.IsConditionMet("Value9"));
        }

        /// <summary>
        /// Test property is not editable if condition on self is met with a string that is not null or empty.
        /// </summary>
        [TestMethod]
        public void EditableIfNotEmpty_DateTimeConditionOnSelfMetWithNotEmpty_Editable()
        {
            var model = new Model() { Value9 = DateTime.MaxValue };
            Assert.IsTrue(model.IsConditionMet("Value9"));
        }

        /// <summary>
        /// Test property is editable if condition on self is met with a null value and pass on null is true.
        /// </summary>
        [TestMethod]
        public void EditableIfNotEmpty_DateTimeConditionOnSelfMetWithNullAnPassOnNull_Editable()
        {
            var model = new Model() { Value10 = null };
            Assert.IsTrue(model.IsConditionMet("Value10"));
        }

        /// <summary>
        /// Test property is not editable if condition not is met with a null value and fail on null is true.
        /// </summary>
        [TestMethod]
        public void EditableIfNotEmpty_ConditionNotMetWithNullAndFailOnNull_NotEditable()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsConditionMet("Value11"));
        }

        /// <summary>
        /// Test property is editable if condition is not met with an empty string and current property is null and fail on null is true.
        /// </summary>
        [TestMethod]
        public void EditableIfNotEmpty_ConditionMetWithEmptyStringAndFailOnNull_NotEditable()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsFalse(model.IsConditionMet("Value11"));
        }

        /// <summary>
        /// Test property is editable if condition is not met with an empty string and current property is null and fail on null is true.
        /// </summary>
        [TestMethod]
        public void EditableIfNotEmpty_ConditionMetWithValueAndFailOnNull_Editable()
        {
            var model = new Model() { Value1 = "Value" };
            Assert.IsTrue(model.IsConditionMet("Value11"));
        }

        /// <summary>
        /// Test property is not editable if condition not is met with a null value and fail on null is true.
        /// </summary>
        [TestMethod]
        public void EditableIfNotEmpty_DateTimeConditionNotMetWithNullAndFailOnNull_NotEditable()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsConditionMet("Value12"));
        }
    }
}
