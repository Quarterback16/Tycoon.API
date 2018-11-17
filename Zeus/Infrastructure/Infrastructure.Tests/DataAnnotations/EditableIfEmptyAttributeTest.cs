using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="EditableIfEmptyAttribute" />.
    /// </summary>
    [TestClass]
    public class EditableIfEmptyAttributeTest
    {
        private class Model : ContingentModel<EditableIfEmptyAttribute>
        {
            public string Value1 { get; set; }

            [EditableIfEmpty("Value1")]
            public string Value2 { get; set; }

            [EditableIfEmpty("Value1", PassOnNull = true)]
            public string Value3 { get; set; }

            [EditableIfEmpty]
            public string Value7 { get; set; }

            [EditableIfEmpty(PassOnNull = true)]
            public string Value8 { get; set; }

            [EditableIfEmpty]
            public DateTime Value9 { get; set; }

            [EditableIfEmpty(PassOnNull = true)]
            public DateTime? Value10 { get; set; }

            [EditableIfEmpty("Value1", FailOnNull = true)]
            public string Value11 { get; set; }

            [EditableIfEmpty("Value1", FailOnNull = true)]
            public DateTime? Value12 { get; set; }
        }

        /// <summary>
        /// Test property is editable if condition is met with an empty string.
        /// </summary>
        [TestMethod]
        public void EditableIfEmpty_ConditionMetWithEmptyString_Editable()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is editable if condition is met with a null value.
        /// </summary>
        [TestMethod]
        public void EditableIfEmpty_ConditionMetWithNull_Editable()
        {
            var model = new Model() { Value1 = null };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not editable if condition is met with a string that is not null or empty.
        /// </summary>
        [TestMethod]
        public void EditableIfEmpty_ConditionMetWithNotEmptyOrNullString_NotEditable()
        {
            var model = new Model() { Value1 = "goodbye" };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is editable if condition is met with a null value and pass on null is true.
        /// </summary>
        [TestMethod]
        public void EditableIfEmpty_ConditionMetWithNullAndPassOnNull_Editable()
        {
            var model = new Model() { Value1 = null };
            Assert.IsTrue(model.IsConditionMet("Value3"));
        }

        /// <summary>
        /// Test property is editable if condition is met with an empty string and current property is null and pass on null is true.
        /// </summary>
        [TestMethod]
        public void EditableIfEmpty_ConditionMetWithEmptyStringAndPassOnNull_Editable()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsTrue(model.IsConditionMet("Value3"));
        }

        /// <summary>
        /// Test property is editable if condition on self is met with an empty string.
        /// </summary>
        [TestMethod]
        public void EditableIfEmpty_ConditionOnSelfMetWithEmptyString_Editable()
        {
            var model = new Model() { Value7 = string.Empty };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is editable if condition on self is met with a null value.
        /// </summary>
        [TestMethod]
        public void EditableIfEmpty_ConditionOnSelfMetWithNull_Editable()
        {
            var model = new Model() { Value7 = null };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not editable if condition on self is met with a string that is not null or empty.
        /// </summary>
        [TestMethod]
        public void EditableIfEmpty_ConditionOnSelfMetWithNotEmptyOrNullString_NotEditable()
        {
            var model = new Model() { Value7 = "goodbye" };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is editable if condition on self is met with a null value and pass on null is true.
        /// </summary>
        [TestMethod]
        public void EditableIfEmpty_ConditionOnSelfMetWithNullAndPassOnNull_Editable()
        {
            var model = new Model() { Value8 = null };
            Assert.IsTrue(model.IsConditionMet("Value8"));
        }

        /// <summary>
        /// Test property is editable if condition on self is met with an empty string and current property is null and pass on null is true.
        /// </summary>
        [TestMethod]
        public void EditableIfEmpty_ConditionOnSelfMetWithEmptyStringAndPassOnNull_Editable()
        {
            var model = new Model() { Value8 = string.Empty };
            Assert.IsTrue(model.IsConditionMet("Value8"));
        }

        /// <summary>
        /// Test property is editable if condition on self is met with an empty string.
        /// </summary>
        [TestMethod]
        public void EditableIfEmpty_DateTimeConditionOnSelfMetWithMinValue_Editable()
        {
            var model = new Model() { Value9 = DateTime.MinValue };
            Assert.IsTrue(model.IsConditionMet("Value9"));
        }

        /// <summary>
        /// Test property is not editable if condition on self is met with a string that is not null or empty.
        /// </summary>
        [TestMethod]
        public void EditableIfEmpty_DateTimeConditionOnSelfMetWithNotEmpty_NotEditable()
        {
            var model = new Model() { Value9 = DateTime.MaxValue };
            Assert.IsFalse(model.IsConditionMet("Value9"));
        }

        /// <summary>
        /// Test property is editable if condition on self is met with a null value.
        /// </summary>
        [TestMethod]
        public void EditableIfEmpty_DateTimeConditionOnSelfMetWithNull_Editable()
        {
            var model = new Model() { Value10 = null };
            Assert.IsTrue(model.IsConditionMet("Value10"));
        }

        /// <summary>
        /// Test property is not editable if condition is met with a null value and fail on null is true.
        /// </summary>
        [TestMethod]
        public void EditableIfEmpty_ConditionNotMetWithNullAndFailOnNull_NotEditable()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsConditionMet("Value11"));
        }

        /// <summary>
        /// Test property is editable if condition is met with an empty string and current property is null and fail on null is true.
        /// </summary>
        [TestMethod]
        public void EditableIfEmpty_ConditionMetWithEmptyStringAndFailOnNull_Editable()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsTrue(model.IsConditionMet("Value11"));
        }

        /// <summary>
        /// Test property is not editable if condition is met with a null value and fail on null is true.
        /// </summary>
        [TestMethod]
        public void EditableIfEmpty_DateTimeConditionNotMetWithNullAndFailOnNull_NotEditable()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsConditionMet("Value12"));
        }
    }
}
