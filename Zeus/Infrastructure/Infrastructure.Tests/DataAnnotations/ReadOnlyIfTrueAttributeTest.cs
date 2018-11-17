using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="ReadOnlyIfTrueAttribute" />.
    /// </summary>
    [TestClass]
    public class ReadOnlyIfTrueAttributeTest
    {
        private class Model : ContingentModel<ReadOnlyIfTrueAttribute>
        {
            public bool Value1 { get; set; }

            [ReadOnlyIfTrue("Value1")]
            public string Value2 { get; set; }

            public bool? Value3 { get; set; }

            [ReadOnlyIfTrue("Value3", PassOnNull = true)]
            public string Value4 { get; set; }

            [ReadOnlyIfTrue("Value3")]
            public string Value5 { get; set; }

            [ReadOnlyIfTrue]
            public bool Value7 { get; set; }

            [ReadOnlyIfTrue(PassOnNull = true)]
            public bool? Value8 { get; set; }
        }

        /// <summary>
        /// Test property is read only if condition is met with true.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfTrue_ConditionMetWithTrue_ReadOnly()
        {
            var model = new Model() { Value1 = true };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not read only if condition is met with false.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfTrue_ConditionMetWithFalse_NotReadOnly()
        {
            var model = new Model() { Value1 = false };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is read only if condition is met with null and pass on null is true.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfTrue_ConditionMetWithNullAndPassOnNull_ReadOnly()
        {
            var model = new Model() { Value3 = null };
            Assert.IsTrue(model.IsConditionMet("Value4"));
        }

        /// <summary>
        /// Test property is read only if condition is met with true and pass on null is true.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfTrue_ConditionMetWithTrueAndPassOnNull_ReadOnly()
        {
            var model = new Model() { Value3 = true };
            Assert.IsTrue(model.IsConditionMet("Value4"));
        }

        /// <summary>
        /// Test property is read only if condition on self is met with true.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfTrue_ConditionOnSelfMetWithTrue_ReadOnly()
        {
            var model = new Model() { Value7 = true };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not read only if condition on self is met with false.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfTrue_ConditionOnSelfMetWithFalse_NotReadOnly()
        {
            var model = new Model() { Value7 = false };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is read only if condition on self is met with null and pass on null is true.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfTrue_ConditionOnSelfMetWithNullAndPassOnNull_ReadOnly()
        {
            var model = new Model() { Value8 = null };
            Assert.IsTrue(model.IsConditionMet("Value8"));
        }

        /// <summary>
        /// Test property is read only if condition on self is met with true and pass on null is true.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfTrue_ConditionOnSelfMetWithTrueAndPassOnNull_ReadOnly()
        {
            var model = new Model() { Value8 = true };
            Assert.IsTrue(model.IsConditionMet("Value8"));
        }
    }
}
