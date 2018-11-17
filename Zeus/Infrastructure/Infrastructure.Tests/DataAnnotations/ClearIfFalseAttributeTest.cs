using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="ClearIfFalseAttribute" />.
    /// </summary>
    [TestClass]
    public class ClearIfFalseAttributeTest
    {
        private class Model : ContingentModel<ClearIfFalseAttribute>
        {
            public bool Value1 { get; set; }

            [ClearIfFalse("Value1")]
            public string Value2 { get; set; }

            public bool? Value3 { get; set; }

            [ClearIfFalse("Value3", PassOnNull = true)]
            public string Value4 { get; set; }

            [ClearIfFalse("Value3")]
            public string Value5 { get; set; }
        }

        /// <summary>
        /// Test property is clear if condition is met with false.
        /// </summary>
        [TestMethod]
        public void ClearIfFalse_ConditionMetWithFalse_Clear()
        {
            var model = new Model() { Value1 = false };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not clear if condition is met with true.
        /// </summary>
        [TestMethod]
        public void ClearIfFalse_ConditionMetWithTrue_NotClear()
        {
            var model = new Model() { Value1 = true };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is clear if condition is met with null and pass on null is true.
        /// </summary>
        [TestMethod]
        public void ClearIfFalse_ConditionMetWithNullAndPassOnNull_Clear()
        {
            var model = new Model() { Value3 = null };
            Assert.IsTrue(model.IsConditionMet("Value4"));
        }

        /// <summary>
        /// Test property is editable if condition is met with false and pass on null is true.
        /// </summary>
        [TestMethod]
        public void ClearIfFalse_ConditionMetWithFalseAndPassOnNull_Clear()
        {
            var model = new Model() { Value3 = false };
            Assert.IsTrue(model.IsConditionMet("Value4"));
        }
    }
}
