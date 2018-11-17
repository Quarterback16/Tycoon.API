using System;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="LessThanAttribute" />.
    /// </summary>
    [TestClass]
    public class LessThanAttributeTest
    {
        private class DateModel : ContingentValidationModel<LessThanAttribute>
        {
            public DateTime? Value1 { get; set; }

            [LessThan("Value1")]
            public DateTime? Value2 { get; set; }
        }

        private class DateModelWithPassOnNull : ContingentValidationModel<LessThanAttribute>
        {
            public DateTime? Value1 { get; set; }

            [LessThan("Value1", PassOnNull = true)]
            public DateTime? Value2 { get; set; }
        }

        private class Int16Model : ContingentValidationModel<LessThanAttribute>
        {
            public Int16 Value1 { get; set; }

            [LessThan("Value1")]
            public Int16 Value2 { get; set; }
        }

        /// <summary>
        /// Test property validates if its value is less than dependent value.
        /// </summary>
        [TestMethod]
        public void LessThan_PropertyValueIsLessThanDependentValue_Validates()
        {
            var model = new DateModel() { Value1 = DateTime.Now, Value2 = DateTime.Now.AddDays(-1) };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property fails validation if its value is not less than dependent value.
        /// </summary>
        [TestMethod]
        public void LessThan_PropertyValueIsNotLessThanDependentValue_Fails()
        {
            var model = new DateModel() { Value1 = DateTime.Now, Value2 = DateTime.Now.AddDays(1) };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property fails validation if both values are null.
        /// </summary>
        [TestMethod]
        public void LessThan_BothValuesNull_Fails()
        {
            var model = new DateModel() { };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property fails validation if both values are null.
        /// </summary>
        [TestMethod]
        public void LessThan_BothValuesNullWithPassOnNull_Validates()
        {
            var model = new DateModelWithPassOnNull() { };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property fails validation if dependent value is null.
        /// </summary>
        [TestMethod]
        public void LessThan_DependentValueNull_Fails()
        {
            var model = new DateModel() { Value2 = DateTime.Now };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property fails validation if property value is null.
        /// </summary>
        [TestMethod]
        public void LessThan_PropertyValueNull_Fails()
        {
            var model = new DateModel() { Value1 = DateTime.Now };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property validates if dependent value is null and pass on null is true.
        /// </summary>
        [TestMethod]
        public void LessThan_DependentValueNullWithPassOnNull_Validates()
        {
            var model = new DateModelWithPassOnNull() { Value2 = DateTime.Now };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property validates if property value is null and pass on null is true.
        /// </summary>
        [TestMethod]
        public void LessThan_PropertyValueNullWithPassOnNull_Validates()
        {
            var model = new DateModelWithPassOnNull() { Value1 = DateTime.Now };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property validates if its value is less than dependent value.
        /// </summary>
        [TestMethod]
        public void LessThan_NumericPropertyValueIsLessThanDependentValue_Validates()
        {
            var model = new Int16Model() { Value1 = 120, Value2 = 12 };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property fails validation if its value is not less than dependent value.
        /// </summary>
        [TestMethod]
        public void LessThan_NumericPropertyValueIsNotLessThanDependentValue_Fails()
        {
            var model = new Int16Model() { Value1 = 12, Value2 = 120 };
            Assert.IsFalse(model.IsValid("Value2"));
        }    
    }
}
