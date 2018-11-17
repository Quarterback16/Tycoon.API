using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="EditableIfTrueAttribute" />.
    /// </summary>
    [TestClass]
    public class EditableIfTrueAttributeTest
    {
        private class Model : ContingentModel<EditableIfTrueAttribute>
        {
            public bool Value1 { get; set; }

            [EditableIfTrue("Value1")]
            public string Value2 { get; set; }

            public bool? Value3 { get; set; }

            [EditableIfTrue("Value3", PassOnNull = true)]
            public string Value4 { get; set; }

            [EditableIfTrue("Value3")]
            public string Value5 { get; set; }

            [EditableIfTrue]
            public bool Value7 { get; set; }

            [EditableIfTrue(PassOnNull = true)]
            public bool? Value8 { get; set; }
        }

        /// <summary>
        /// Test property is editable if condition is met with true.
        /// </summary>
        [TestMethod]
        public void EditableIfTrue_ConditionMetWithTrue_Editable()
        {
            var model = new Model() { Value1 = true };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not editable if condition is met with false.
        /// </summary>
        [TestMethod]
        public void EditableIfTrue_ConditionMetWithFalse_NotEditable()
        {
            var model = new Model() { Value1 = false };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is editable if condition is met with null and pass on null is true.
        /// </summary>
        [TestMethod]
        public void EditableIfTrue_ConditionMetWithNullAndPassOnNull_Editable()
        {
            var model = new Model() { Value3 = null };
            Assert.IsTrue(model.IsConditionMet("Value4"));
        }

        /// <summary>
        /// Test property is editable if condition is met with true and pass on null is true.
        /// </summary>
        [TestMethod]
        public void EditableIfTrue_ConditionMetWithTrueAndPassOnNull_Editable()
        {
            var model = new Model() { Value3 = true };
            Assert.IsTrue(model.IsConditionMet("Value4"));
        }

        /// <summary>
        /// Test property is editable if condition on self is met with true.
        /// </summary>
        [TestMethod]
        public void EditableIfTrue_ConditionOnSelfMetWithTrue_Editable()
        {
            var model = new Model() { Value7 = true };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not editable if condition on self is met with false.
        /// </summary>
        [TestMethod]
        public void EditableIfTrue_ConditionOnSelfMetWithFalse_NotEditable()
        {
            var model = new Model() { Value7 = false };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is editable if condition on self is met with null and pass on null is true.
        /// </summary>
        [TestMethod]
        public void EditableIfTrue_ConditionOnSelfMetWithNullAndPassOnNull_Editable()
        {
            var model = new Model() { Value8 = null };
            Assert.IsTrue(model.IsConditionMet("Value8"));
        }

        /// <summary>
        /// Test property is editable if condition on self is met with true and pass on null is true.
        /// </summary>
        [TestMethod]
        public void EditableIfTrue_ConditionOnSelfMetWithTrueAndPassOnNull_Editable()
        {
            var model = new Model() { Value8 = true };
            Assert.IsTrue(model.IsConditionMet("Value8"));
        }
    }
}
