using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="ClearIfNotRegExMatchAttribute" />.
    /// </summary>
    [TestClass]
    public class ClearIfNotRegExMatchAttributeTest
    {
        private class Model : ContingentModel<ClearIfNotRegExMatchAttribute>
        {
            public string Value1 { get; set; }

            [ClearIfNotRegExMatch("Value1", "^ *(1[0-2]|0?[1-9]):[0-5][0-9] *(a|p|A|P)(m|M) *$")]
            public string Value2 { get; set; }
        }

        /// <summary>
        /// Test property is clear if condition is met with a non-matching value.
        /// </summary>
        [TestMethod]
        public void ClearIfNotRegExMatch_ConditionMetWithNonMatchingValue_Clear()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsTrue(model.IsConditionMet("Value2"));
        }

        /// <summary>
        /// Test property is not clear if condition is met with a matching value.
        /// </summary>
        [TestMethod]
        public void ClearIfNotRegExMatch_ConditionMetWithMatchingValue_NotClear()
        {
            var model = new Model() { Value1 = "8:30 AM" };
            Assert.IsFalse(model.IsConditionMet("Value2"));
        }
    }
}
