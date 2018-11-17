using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="EditableIfRegExMatchAttribute" />.
    /// </summary>
    [TestClass]
    public class EditableIfRegExMatchAttributeTest
    {
        private class Model : ContingentModel<EditableIfRegExMatchAttribute>
        {
            public string Value1 { get; set; }

            [EditableIfRegExMatch("Value1", "^ *(1[0-2]|0?[1-9]):[0-5][0-9] *(a|p|A|P)(m|M) *$")]
            public string Value2 { get; set; }

            [EditableIfRegExMatch("^ *(1[0-2]|0?[1-9]):[0-5][0-9] *(a|p|A|P)(m|M) *$")]
            public string Value7 { get; set; }
        }

        /// <summary>
        /// Test property is editable if condition is met with a matching value.
        /// </summary>
        [TestMethod]
        public void EditableIfNotRegExMatch_ConditionMetWithMatchingValue_Editable()
        {
            var model = new Model() { Value1 = "8:30 AM" };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not editable if condition is met with a non-matching value.
        /// </summary>
        [TestMethod]
        public void EditableIfNotRegExMatch_ConditionMetWithNonMatchingValue_NotEditable()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is editable if condition on self is met with a matching value.
        /// </summary>
        [TestMethod]
        public void EditableIfNotRegExMatch_ConditionOnSelfMetWithMatchingValue_Editable()
        {
            var model = new Model() { Value7 = "8:30 AM" };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not editable if condition on self is met with a non-matching value.
        /// </summary>
        [TestMethod]
        public void EditableIfNotRegExMatch_ConditionOnSelfMetWithNonMatchingValue_NotEditable()
        {
            var model = new Model() { Value7 = "hello" };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }
    }
}
