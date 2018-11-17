using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="ReadOnlyIfFalseAttribute" />.
    /// </summary>
    [TestClass]
    public class ReadOnlyIfFalseAttributeTest
    {
        private class Model : ContingentModel<ReadOnlyIfFalseAttribute>
        {
            public bool Value1 { get; set; }

            [ReadOnlyIfFalse("Value1")]
            public string Value2 { get; set; }

            public bool? Value3 { get; set; }

            [ReadOnlyIfFalse("Value3", PassOnNull = true)]
            public string Value4 { get; set; }

            [ReadOnlyIfFalse("Value3")]
            public string Value5 { get; set; }

            [ReadOnlyIfFalse]
            public bool Value7 { get; set; }

            [ReadOnlyIfFalse(PassOnNull = true)]
            public bool? Value8 { get; set; }
        }

        /// <summary>
        /// Test property is read only if condition is met with false.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfFalse_ConditionMetWithFalse_ReadOnly()
        {
            var model = new Model() { Value1 = false };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not read only if condition is met with true.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfFalse_ConditionMetWithTrue_NotReadOnly()
        {
            var model = new Model() { Value1 = true };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is read only if condition is met with null and pass on null is true.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfFalse_ConditionMetWithNullAndPassOnNull_ReadOnly()
        {
            var model = new Model() { Value3 = null };
            Assert.IsTrue(model.IsConditionMet("Value4"));
        }

        /// <summary>
        /// Test property is editable if condition is met with false and pass on null is true.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfFalse_ConditionMetWithFalseAndPassOnNull_ReadOnly()
        {
            var model = new Model() { Value3 = false };
            Assert.IsTrue(model.IsConditionMet("Value4"));
        }

        /// <summary>
        /// Test property is editable if condition on self is met with false.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfFalse_ConditionOnSelfMetWithFalse_ReadOnly()
        {
            var model = new Model() { Value7 = false };
            Assert.IsTrue(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is not editable if condition on self is met with true.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfFalse_ConditionOnSelfMetWithTrue_NotReadOnly()
        {
            var model = new Model() { Value7 = true };
            Assert.IsFalse(model.IsConditionMet("Value7"));
        }

        /// <summary>
        /// Test property is editable if condition on self is met with null and pass on null is true.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfFalse_ConditionOnSelfMetWithNullAndPassOnNull_ReadOnly()
        {
            var model = new Model() { Value8 = null };
            Assert.IsTrue(model.IsConditionMet("Value8"));
        }

        /// <summary>
        /// Test property is editable if condition on self is met with false and pass on null is true.
        /// </summary>
        [TestMethod]
        public void ReadOnlyIfFalse_ConditionOnSelfMetWithFalseAndPassOnNull_ReadOnly()
        {
            var model = new Model() { Value8 = false };
            Assert.IsTrue(model.IsConditionMet("Value8"));
        }
    }
}
