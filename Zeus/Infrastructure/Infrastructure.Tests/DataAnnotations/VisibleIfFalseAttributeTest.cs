using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="VisibleIfFalseAttribute" />.
    /// </summary>
    [TestClass]
    public class VisibleIfFalseAttributeTest
    {
        private class Model : ContingentModel<VisibleIfFalseAttribute>
        {
            public bool Value1 { get; set; }

            [VisibleIfFalse("Value1")]
            public string Value2 { get; set; }

            public bool? Value3 { get; set; }

            [VisibleIfFalse("Value3", PassOnNull = true)]
            public string Value4 { get; set; }

            [VisibleIfFalse("Value3")]
            public string Value5 { get; set; }

            [VisibleIfFalse]
            public bool Value7 { get; set; }

            [VisibleIfFalse(PassOnNull = true)]
            public bool? Value8 { get; set; }
        }

        /// <summary>
        /// Test property is visible if condition is met with false.
        /// </summary>
        [TestMethod]
        public void VisibleIfFalse_ConditionMetWithFalse_Visible()
        {
            var model = new Model() { Value1 = false };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not visible if condition is met with true.
        /// </summary>
        [TestMethod]
        public void VisibleIfFalse_ConditionMetWithTrue_NotVisible()
        {
            var model = new Model() { Value1 = true };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is visible if condition is met with null and pass on null is true.
        /// </summary>
        [TestMethod]
        public void VisibleIfFalse_ConditionMetWithNullAndPassOnNull_Visible()
        {
            var model = new Model() { Value3 = null };
            Assert.IsTrue(model.IsConditionMet("Value4"));
        }

        /// <summary>
        /// Test property is visible if condition is met with false and pass on null is true.
        /// </summary>
        [TestMethod]
        public void VisibleIfFalse_ConditionMetWithFalseAndPassOnNull_Visible()
        {
            var model = new Model() { Value3 = false };
            Assert.IsTrue(model.IsConditionMet("Value4"));
        }

        /// <summary>
        /// Test property is visible if condition on self is met with false.
        /// </summary>
        [TestMethod]
        public void VisibleIfFalse_ConditionOnSelfMetWithFalse_Visible()
        {
            var model = new Model() { Value7 = false };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not visible if condition on self is met with true.
        /// </summary>
        [TestMethod]
        public void VisibleIfFalse_ConditionOnSelfMetWithTrue_NotVisible()
        {
            var model = new Model() { Value7 = true };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is visible if condition on self is met with null and pass on null is true.
        /// </summary>
        [TestMethod]
        public void VisibleIfFalse_ConditionOnSelfMetWithNullAndPassOnNull_Visible()
        {
            var model = new Model() { Value8 = null };
            Assert.IsTrue(model.IsConditionMet("Value8"));
        }

        /// <summary>
        /// Test property is visible if condition on self is met with false and pass on null is true.
        /// </summary>
        [TestMethod]
        public void VisibleIfFalse_ConditionOnSelfMetWithFalseAndPassOnNull_Visible()
        {
            var model = new Model() { Value8 = false };
            Assert.IsTrue(model.IsConditionMet("Value8"));
        }
    }
}
