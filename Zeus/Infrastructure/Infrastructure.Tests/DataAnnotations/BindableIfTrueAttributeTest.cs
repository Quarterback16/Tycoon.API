using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="BindableIfTrueAttribute" />.
    /// </summary>
    [TestClass]
    public class BindableIfTrueAttributeTest
    {
        private class Model : ContingentModel<BindableIfTrueAttribute>
        {
            public bool Value1 { get; set; }

            [BindableIfTrue("Value1")]
            public string Value2 { get; set; }

            public bool? Value3 { get; set; }

            [BindableIfTrue("Value3", PassOnNull = true)]
            public string Value4 { get; set; }

            [BindableIfTrue("Value3")]
            public string Value5 { get; set; }

            [BindableIfTrue]
            public bool Value7 { get; set; }

            [BindableIfTrue(PassOnNull = true)]
            public bool? Value8 { get; set; }
        }

        /// <summary>
        /// Test property is bindable if condition is met with true.
        /// </summary>
        [TestMethod]
        public void BindableIfTrue_ConditionMetWithTrue_Bindable()
        {
            var model = new Model() { Value1 = true };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not bindable if condition is met with false.
        /// </summary>
        [TestMethod]
        public void BindableIfTrue_ConditionMetWithFalse_NotBindable()
        {
            var model = new Model() { Value1 = false };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is bindable if condition is met with null and pass on null is true.
        /// </summary>
        [TestMethod]
        public void BindableIfTrue_ConditionMetWithNullAndPassOnNull_Bindable()
        {
            var model = new Model() { Value3 = null };
            Assert.IsTrue(model.IsConditionMet("Value4"));
        }

        /// <summary>
        /// Test property is bindable if condition is met with true and pass on null is true.
        /// </summary>
        [TestMethod]
        public void BindableIfTrue_ConditionMetWithTrueAndPassOnNull_Bindable()
        {
            var model = new Model() { Value3 = true };
            Assert.IsTrue(model.IsConditionMet("Value4"));
        }

        /// <summary>
        /// Test property is bindable if condition on self is met with true.
        /// </summary>
        [TestMethod]
        public void BindableIfTrue_ConditionOnSelfMetWithTrue_Bindable()
        {
            var model = new Model() { Value7 = true };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not bindable if condition on self is met with false.
        /// </summary>
        [TestMethod]
        public void BindableIfTrue_ConditionOnSelfMetWithFalse_NotBindable()
        {
            var model = new Model() { Value7 = false };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is bindable if condition on self is met with null and pass on null is true.
        /// </summary>
        [TestMethod]
        public void BindableIfTrue_ConditionOnSelfMetWithNullAndPassOnNull_Bindable()
        {
            var model = new Model() { Value8 = null };
            Assert.IsTrue(model.IsConditionMet("Value8"));
        }

        /// <summary>
        /// Test property is bindable if condition on self is met with true and pass on null is true.
        /// </summary>
        [TestMethod]
        public void BindableIfTrue_ConditionOnSelfMetWithTrueAndPassOnNull_Bindable()
        {
            var model = new Model() { Value8 = true };
            Assert.IsTrue(model.IsConditionMet("Value8"));
        }
    }
}
