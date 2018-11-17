using System;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="GreaterThanOrEqualToAttribute" />.
    /// </summary>
    [TestClass]
    public class GreaterThanOrEqualToAttributeTest
    {
        private class DateModel : ContingentValidationModel<GreaterThanOrEqualToAttribute>
        {
            public DateTime? Value1 { get; set; }

            [GreaterThanOrEqualTo("Value1")]
            public DateTime? Value2 { get; set; }
        }

        private class DateModelWithPassNull : ContingentValidationModel<GreaterThanOrEqualToAttribute>
        {
            public DateTime? Value1 { get; set; }

            [GreaterThanOrEqualTo("Value1", PassOnNull = true)]
            public DateTime? Value2 { get; set; }
        }

        private class Int16Model : ContingentValidationModel<GreaterThanOrEqualToAttribute>
        {
            public Int16 Value1 { get; set; }

            [GreaterThanOrEqualTo("Value1")]
            public Int16 Value2 { get; set; }
        }

        /// <summary>
        /// Test property validates if its value is greater than dependent value.
        /// </summary>
        [TestMethod]
        public void GreaterThanOrEqualTo_PropertyValueIsGreaterThanDependentValue_Validates()
        {
            var model = new DateModel() { Value1 = DateTime.Now, Value2 = DateTime.Now.AddDays(1) };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property validates if its value is equal to thedependent value.
        /// </summary>
        [TestMethod]
        public void GreaterThanOrEqualTo_PropertyValueIsEqualToDependentValue_Validates()
        {
            var date = DateTime.Now;
            var model = new DateModel() { Value1 = date, Value2 = date };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property validates if both values are null.
        /// </summary>
        [TestMethod]
        public void GreaterThanOrEqualTo_BothValuesNull_Validates()
        {
            var model = new DateModel() { };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property fails validation if its value is not greater than or equal to dependent value.
        /// </summary>
        [TestMethod]
        public void GreaterThanOrEqualTo_PropertyValueIsNotGreaterThanOrEqualToDependentValue_Fails()
        {
            var model = new DateModel() { Value1 = DateTime.Now, Value2 = DateTime.Now.AddDays(-1) };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property fails validation if dependent value is null.
        /// </summary>
        [TestMethod]
        public void GreaterThanOrEqualTo_DependentValueNull_Fails()
        {
            var model = new DateModel() { Value2 = DateTime.Now };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property fails validation if property value is null.
        /// </summary>
        [TestMethod]
        public void GreaterThanOrEqualTo_PropertyValueNull_Fails()
        {
            var model = new DateModel() { Value1 = DateTime.Now };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property validates if dependent value is null and pass on null is true.
        /// </summary>
        [TestMethod]
        public void GreaterThanOrEqualTo_DependentValueNullWithPassOnNull_Validates()
        {
            var model = new DateModelWithPassNull() { Value2 = DateTime.Now };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property validates if property value is null and pass on null is true.
        /// </summary>
        [TestMethod]
        public void GreaterThanOrEqualTo_PropertyValueNullWithPassOnNull_Validates()
        {
            var model = new DateModelWithPassNull() { Value1 = DateTime.Now };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property validates if its value is greater than dependent value.
        /// </summary>
        [TestMethod]
        public void GreaterThanOrEqualTo_NumericPropertyValueIsGreaterThanDependentValue_Validates()
        {
            var model = new Int16Model() { Value1 = 12, Value2 = 120 };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property validates if its value is equal to the dependent value.
        /// </summary>
        [TestMethod]
        public void GreaterThanOrEqualTo_NumericPropertyValueIsEqualToDependentValue_Validates()
        {
            var model = new Int16Model() { Value1 = 12, Value2 = 12 };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property fails validation if its value is not greater than dependent value.
        /// </summary>
        [TestMethod]
        public void GreaterThanOrEqualTo_NumericPropertyValueIsNotGreaterThanOrEqualToDependentValue_Fails()
        {
            var model = new Int16Model() { Value1 = 120, Value2 = 12 };
            Assert.IsFalse(model.IsValid("Value2"));
        }    
    }
}
