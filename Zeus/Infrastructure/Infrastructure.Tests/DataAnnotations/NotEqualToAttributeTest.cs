using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="NotEqualToAttribute" />.
    /// </summary>
    [TestClass]
    public class NotEqualToAttributeTest
    {
        private class Model : ContingentValidationModel<NotEqualToAttribute>
        {
            public string Value1 { get; set; }

            [NotEqualTo("Value1")]
            public string Value2 { get; set; }
        }

        /// <summary>
        /// Test property validates if dependent value is not equal to property value.
        /// </summary>
        [TestMethod]
        public void NotEqualTo_DependentValueIsNotEqualToPropertyValue_Validates()
        {
            var model = new Model() { Value1 = "hello", Value2 = "goodbye" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property validates if dependent value is equal to property value.
        /// </summary>
        [TestMethod]
        public void NotEqualTo_DependentValueIsEqualToPropertyValue_Validates()
        {
            var model = new Model() { Value1 = "hello", Value2 = "hello" };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property fails validation if dependent value and property value are both null.
        /// </summary>
        [TestMethod]
        public void NotEqualTo_BothValuesNull_Fails()
        {
            var model = new Model() { };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property fails validation if dependent value is null.
        /// </summary>
        [TestMethod]
        public void NotEqualTo_DependentValueNull_Fails()
        {
            var model = new Model() { Value2 = "hello" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property fails validation if property value is null.
        /// </summary>
        [TestMethod]
        public void NotEqualTo_PropertyValueNull_Fails()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsTrue(model.IsValid("Value2"));
        }    
    }
}
