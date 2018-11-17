using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="BindableIfFalseAttribute" />.
    /// </summary>
    [TestClass]
    public class BindableIfFalseAttributeTest
    {
        private class Model : ContingentModel<BindableIfFalseAttribute>
        {
            public bool Value1 { get; set; }

            [BindableIfFalse("Value1")]
            public string Value2 { get; set; }

            public bool? Value3 { get; set; }

            [BindableIfFalse("Value3", PassOnNull = true)]
            public string Value4 { get; set; }

            [BindableIfFalse("Value3")]
            public string Value5 { get; set; }

            [BindableIfFalse]
            public bool Value7 { get; set; }

            [BindableIfFalse(PassOnNull = true)]
            public bool? Value8 { get; set; }
        }

        /// <summary>
        /// Test property is bindable if condition is met with false.
        /// </summary>
        [TestMethod]
        public void BindableIfFalse_ConditionMetWithFalse_Bindable()
        {
            var model = new Model() { Value1 = false };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not bindable if condition is met with true.
        /// </summary>
        [TestMethod]
        public void BindableIfFalse_ConditionMetWithTrue_NotBindable()
        {
            var model = new Model() { Value1 = true };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is bindable if condition is met with null and pass on null is true.
        /// </summary>
        [TestMethod]
        public void BindableIfFalse_ConditionMetWithNullAndPassOnNull_Bindable()
        {
            var model = new Model() { Value3 = null };
            Assert.IsTrue(model.IsConditionMet("Value4"));
        }

        /// <summary>
        /// Test property is bindable if condition is met with false and pass on null is true.
        /// </summary>
        [TestMethod]
        public void BindableIfFalse_ConditionMetWithFalseAndPassOnNull_Bindable()
        {
            var model = new Model() { Value3 = false };
            Assert.IsTrue(model.IsConditionMet("Value4"));
        }

        /// <summary>
        /// Test property is bindable if condition on self is met with false.
        /// </summary>
        [TestMethod]
        public void BindableIfFalse_ConditionOnSelfMetWithFalse_Bindable()
        {
            var model = new Model() { Value7 = false };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not bindable if condition on self is met with true.
        /// </summary>
        [TestMethod]
        public void BindableIfFalse_ConditionOnSelfMetWithTrue_NotBindable()
        {
            var model = new Model() { Value7 = true };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is bindable if condition on self is met with null and pass on null is true.
        /// </summary>
        [TestMethod]
        public void BindableIfFalse_ConditionOnSelfMetWithNullAndPassOnNull_Bindable()
        {
            var model = new Model() { Value8 = null };
            Assert.IsTrue(model.IsConditionMet("Value8"));
        }

        /// <summary>
        /// Test property is bindable if condition on self is met with false and pass on null is true.
        /// </summary>
        [TestMethod]
        public void BindableIfFalse_ConditionOnSelfMetWithFalseAndPassOnNull_Bindable()
        {
            var model = new Model() { Value8 = false };
            Assert.IsTrue(model.IsConditionMet("Value8"));
        }
    }
}
