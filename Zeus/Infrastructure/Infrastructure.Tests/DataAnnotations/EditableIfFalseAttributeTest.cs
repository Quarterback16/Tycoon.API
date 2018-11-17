using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="EditableIfFalseAttribute" />.
    /// </summary>
    [TestClass]
    public class EditableIfFalseAttributeTest
    {
        private class Model : ContingentModel<EditableIfFalseAttribute>
        {
            public bool Value1 { get; set; }

            [EditableIfFalse("Value1")]
            public string Value2 { get; set; }

            public bool? Value3 { get; set; }

            [EditableIfFalse("Value3", PassOnNull = true)]
            public string Value4 { get; set; }

            [EditableIfFalse("Value3")]
            public string Value5 { get; set; }

            [EditableIfFalse]
            public bool Value7 { get; set; }

            [EditableIfFalse(PassOnNull = true)]
            public bool? Value8 { get; set; }
        }

        /// <summary>
        /// Test property is editable if condition is met with false.
        /// </summary>
        [TestMethod]
        public void EditableIfFalse_ConditionMetWithFalse_Editable()
        {
            var model = new Model() { Value1 = false };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not editable if condition is met with true.
        /// </summary>
        [TestMethod]
        public void EditableIfFalse_ConditionMetWithTrue_NotEditable()
        {
            var model = new Model() { Value1 = true };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is editable if condition is met with null and pass on null is true.
        /// </summary>
        [TestMethod]
        public void EditableIfFalse_ConditionMetWithNullAndPassOnNull_Editable()
        {
            var model = new Model() { Value3 = null };
            Assert.IsTrue(model.IsConditionMet("Value4"));
        }

        /// <summary>
        /// Test property is editable if condition is met with false and pass on null is true.
        /// </summary>
        [TestMethod]
        public void EditableIfFalse_ConditionMetWithFalseAndPassOnNull_Editable()
        {
            var model = new Model() { Value3 = false };
            Assert.IsTrue(model.IsConditionMet("Value4"));
        }

        /// <summary>
        /// Test property is editable if condition on self is met with false.
        /// </summary>
        [TestMethod]
        public void EditableIfFalse_ConditionOnSelfMetWithFalse_Editable()
        {
            var model = new Model() { Value7 = false };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not editable if condition on self is met with true.
        /// </summary>
        [TestMethod]
        public void EditableIfFalse_ConditionOnSelfMetWithTrue_NotEditable()
        {
            var model = new Model() { Value7 = true };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is editable if condition on self is met with null and pass on null is true.
        /// </summary>
        [TestMethod]
        public void EditableIfFalse_ConditionOnSelfMetWithNullAndPassOnNull_Editable()
        {
            var model = new Model() { Value8 = null };
            Assert.IsTrue(model.IsConditionMet("Value8"));
        }

        /// <summary>
        /// Test property is editable if condition on self is met with false and pass on null is true.
        /// </summary>
        [TestMethod]
        public void EditableIfFalse_ConditionOnSelfMetWithFalseAndPassOnNull_Editable()
        {
            var model = new Model() { Value8 = false };
            Assert.IsTrue(model.IsConditionMet("Value8"));
        }
    }
}
