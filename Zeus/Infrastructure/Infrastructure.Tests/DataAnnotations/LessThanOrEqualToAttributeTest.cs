using System;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="LessThanOrEqualToAttribute" />.
    /// </summary>
    [TestClass]
    public class LessThanOrEqualToAttributeTest
    {
        private class DateModel : ContingentValidationModel<LessThanOrEqualToAttribute>
        {
            public DateTime? Value1 { get; set; }

            [LessThanOrEqualTo("Value1")]
            public DateTime? Value2 { get; set; }
        }

        private class Int16Model : ContingentValidationModel<LessThanOrEqualToAttribute>
        {
            public Int16 Value1 { get; set; }

            [LessThanOrEqualTo("Value1")]
            public Int16 Value2 { get; set; }
        }

        /// <summary>
        /// Test property validates if its value is less than dependent value.
        /// </summary>
        [TestMethod]
        public void LessThanOrEqualTo_PropertyValueIsLessThanDependentValue_Validates()
        {
            var model = new DateModel() { Value1 = DateTime.Now, Value2 = DateTime.Now.AddDays(-1) };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property validates if its value is equal to thedependent value.
        /// </summary>
        [TestMethod]
        public void LessThanOrEqualTo_PropertyValueIsEqualToDependentValue_Validates()
        {
            var date = DateTime.Now;
            var model = new DateModel() { Value1 = date, Value2 = date };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property validates if both values are null.
        /// </summary>
        [TestMethod]
        public void LessThanOrEqualTo_BothValuesNull_Validates()
        {
            var date = DateTime.Now;
            var model = new DateModel() { };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property fails validation if its value is not less than or equal to dependent value.
        /// </summary>
        [TestMethod]
        public void LessThanOrEqualTo_PropertyValueIsNotLessThanOrEqualToDependentValue_Fails()
        {
            var model = new DateModel() { Value1 = DateTime.Now, Value2 = DateTime.Now.AddDays(1) };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property fails validation if dependent value is null.
        /// </summary>
        [TestMethod]
        public void LessThanOrEqualTo_DependentValueNull_Fails()
        {
            var model = new DateModel() { Value2 = DateTime.Now };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property fails validation if property value is null.
        /// </summary>
        [TestMethod]
        public void LessThanOrEqualTo_PropertyValueNull_Fails()
        {
            var model = new DateModel() { Value1 = DateTime.Now };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property validates if its value is less than dependent value.
        /// </summary>
        [TestMethod]
        public void LessThanOrEqualTo_NumericPropertyValueIsLessThanDependentValue_Validates()
        {
            var model = new Int16Model() { Value1 = 120, Value2 = 12 };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property validates if its value is equal to the dependent value.
        /// </summary>
        [TestMethod]
        public void LessThanOrEqualTo_NumericPropertyValueIsEqualToDependentValue_Validates()
        {
            var model = new Int16Model() { Value1 = 12, Value2 = 12 };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property fails validation if its value is not less than dependent value.
        /// </summary>
        [TestMethod]
        public void LessThanOrEqualTo_NumericPropertyValueIsNotLessThanOrEqualToDependentValue_Fails()
        {
            var model = new Int16Model() { Value1 = 12, Value2 = 120 };
            Assert.IsFalse(model.IsValid("Value2"));
        }     
    }
}
