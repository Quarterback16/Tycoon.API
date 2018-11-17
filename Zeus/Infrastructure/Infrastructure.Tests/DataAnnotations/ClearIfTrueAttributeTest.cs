using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="ClearIfTrueAttribute" />.
    /// </summary>
    [TestClass]
    public class ClearIfTrueAttributeTest
    {
        private class Model : ContingentModel<ClearIfTrueAttribute>
        {
            public bool Value1 { get; set; }

            [ClearIfTrue("Value1")]
            public string Value2 { get; set; }

            public bool? Value3 { get; set; }

            [ClearIfTrue("Value3", PassOnNull = true)]
            public string Value4 { get; set; }

            [ClearIfTrue("Value3")]
            public string Value5 { get; set; }
        }

        /// <summary>
        /// Test property is clear if condition is met with true.
        /// </summary>
        [TestMethod]
        public void ClearIfTrue_ConditionMetWithTrue_Clear()
        {
            var model = new Model() { Value1 = true };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not clear if condition is met with false.
        /// </summary>
        [TestMethod]
        public void ClearIfTrue_ConditionMetWithFalse_NotClear()
        {
            var model = new Model() { Value1 = false };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is clear if condition is met with null and pass on null is true.
        /// </summary>
        [TestMethod]
        public void ClearIfTrue_ConditionMetWithNullAndPassOnNull_Clear()
        {
            var model = new Model() { Value3 = null };
            Assert.IsTrue(model.IsConditionMet("Value4"));
        }

        /// <summary>
        /// Test property is clear if condition is met with true and pass on null is true.
        /// </summary>
        [TestMethod]
        public void ClearIfTrue_ConditionMetWithTrueAndPassOnNull_Clear()
        {
            var model = new Model() { Value3 = true };
            Assert.IsTrue(model.IsConditionMet("Value4"));
        }
    }
}
