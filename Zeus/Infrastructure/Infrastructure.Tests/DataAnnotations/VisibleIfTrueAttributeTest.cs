using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="VisibleIfTrueAttribute" />.
    /// </summary>
    [TestClass]
    public class VisibleIfTrueAttributeTest
    {
        private class Model : ContingentModel<VisibleIfTrueAttribute>
        {
            public bool Value1 { get; set; }

            [VisibleIfTrue("Value1")]
            public string Value2 { get; set; }

            public bool? Value3 { get; set; }

            [VisibleIfTrue("Value3", PassOnNull = true)]
            public string Value4 { get; set; }

            [VisibleIfTrue("Value3")]
            public string Value5 { get; set; }

            [VisibleIfTrue]
            public bool Value7 { get; set; }

            [VisibleIfTrue(PassOnNull = true)]
            public bool? Value8 { get; set; }
        }

        /// <summary>
        /// Test property is visible if condition is met with true.
        /// </summary>
        [TestMethod]
        public void VisibleIfTrue_ConditionMetWithTrue_Visible()
        {
            var model = new Model() { Value1 = true };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not visible if condition is met with false.
        /// </summary>
        [TestMethod]
        public void VisibleIfTrue_ConditionMetWithFalse_NotVisible()
        {
            var model = new Model() { Value1 = false };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is visible if condition is met with null and pass on null is true.
        /// </summary>
        [TestMethod]
        public void VisibleIfTrue_ConditionMetWithNullAndPassOnNull_Visible()
        {
            var model = new Model() { Value3 = null };
            Assert.IsTrue(model.IsConditionMet("Value4"));
        }

        /// <summary>
        /// Test property is visible if condition is met with true and pass on null is true.
        /// </summary>
        [TestMethod]
        public void VisibleIfTrue_ConditionMetWithTrueAndPassOnNull_Visible()
        {
            var model = new Model() { Value3 = true };
            Assert.IsTrue(model.IsConditionMet("Value4"));
        }

        /// <summary>
        /// Test property is visible if condition on self is met with true.
        /// </summary>
        [TestMethod]
        public void VisibleIfTrue_ConditionOnSelfMetWithTrue_Visible()
        {
            var model = new Model() { Value7 = true };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not visible if condition on self is met with false.
        /// </summary>
        [TestMethod]
        public void VisibleIfTrue_ConditionOnSelfMetWithFalse_NotVisible()
        {
            var model = new Model() { Value7 = false };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is visible if condition on self is met with null and pass on null is true.
        /// </summary>
        [TestMethod]
        public void VisibleIfTrue_ConditionOnSelfMetWithNullAndPassOnNull_Visible()
        {
            var model = new Model() { Value8 = null };
            Assert.IsTrue(model.IsConditionMet("Value8"));
        }

        /// <summary>
        /// Test property is visible if condition on self is met with true and pass on null is true.
        /// </summary>
        [TestMethod]
        public void VisibleIfTrue_ConditionOnSelfMetWithTrueAndPassOnNull_Visible()
        {
            var model = new Model() { Value8 = true };
            Assert.IsTrue(model.IsConditionMet("Value8"));
        }
    }
}
