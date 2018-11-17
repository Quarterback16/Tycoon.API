using System;
using System.Linq;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.ModelMetadataProviders;
using Employment.Web.Mvc.Infrastructure.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="MaxAttribute" />.
    /// </summary>
    [TestClass]
    public class MaxAttributeTest
    {
        /// <summary>
        /// Test validates if value is equal to the maximum value.
        /// </summary>
        [TestMethod]
        public void Max_MaximumIsIntegerAndTheValueIsEqualTo_Validates()
        {
            int value = 1;
            var sut = new MaxAttribute(1);

            Assert.IsTrue(sut.IsValid(value));
        }

        /// <summary>
        /// Test fails if value is greater than the maximum value.
        /// </summary>
        [TestMethod]
        public void Max_MaximumIsIntegerAndTheValueIsGreaterThan_Fails()
        {
            int value = 2;
            var sut = new MaxAttribute(1);

            Assert.IsFalse(sut.IsValid(value));
        }

        /// <summary>
        /// Test validates if value is less than the maximum value.
        /// </summary>
        [TestMethod]
        public void Max_MaximumIsIntegerAndTheValueIsLessThan_Validates()
        {
            int value = 0;
            var sut = new MaxAttribute(1);

            Assert.IsTrue(sut.IsValid(value));
        }

        /// <summary>
        /// Test validates if value is equal to the maximum value.
        /// </summary>
        [TestMethod]
        public void Max_MaximumIsDoubleAndTheValueIsEqualTo_Validates()
        {
            double value = 1.1;
            var sut = new MaxAttribute(1.1);

            Assert.IsTrue(sut.IsValid(value));
        }

        /// <summary>
        /// Test fails if value is greater than the maximum value.
        /// </summary>
        [TestMethod]
        public void Max_MaximumIsDoubleAndTheValueIsGreaterThan_Fails()
        {
            double value = 2.2;
            var sut = new MaxAttribute(1.1);

            Assert.IsFalse(sut.IsValid(value));
        }

        /// <summary>
        /// Test validates if value is less than the maximum value.
        /// </summary>
        [TestMethod]
        public void Max_MaximumIsDoubleAndTheValueIsLessThan_Validates()
        {
            double value = 0;
            var sut = new MaxAttribute(1.1);

            Assert.IsTrue(sut.IsValid(value));
        }

        /// <summary>
        /// Test validates if value is equal to the maximum value.
        /// </summary>
        [TestMethod]
        public void Max_MaximumIsSpecifiedTypeAndTheValueIsEqualTo_Validates()
        {
            long value = 1;
            var sut = new MaxAttribute(typeof(long), "1");

            Assert.IsTrue(sut.IsValid(value));
        }

        /// <summary>
        /// Test fails if value is greater than the maximum value.
        /// </summary>
        [TestMethod]
        public void Max_MaximumIsSpecifiedTypeAndTheValueIsGreaterThan_Fails()
        {
            long value = 2;
            var sut = new MaxAttribute(typeof(long), "1");

            Assert.IsFalse(sut.IsValid(value));
        }

        /// <summary>
        /// Test validates if value is less than the maximum value.
        /// </summary>
        [TestMethod]
        public void Max_MaximumIsSpecifiedTypeAndTheValueIsLessThan_Validates()
        {
            long value = 0;
            var sut = new MaxAttribute(typeof(long), "1");

            Assert.IsTrue(sut.IsValid(value));
        }

        /// <summary>
        /// Test returns formatted error message.
        /// </summary>
        [TestMethod]
        public void Max_FormatErrorMessage_ReturnsFormattedErrorMessage()
        {
            var sut = new MaxAttribute(1);

            var error = sut.FormatErrorMessage("property");

            Assert.IsTrue(!string.IsNullOrEmpty(error));
        }

        /// <summary>
        /// Test returns formatted error message.
        /// </summary>
        [TestMethod]
        public void Max_FormatErrorMessageWithErrorMessage_ReturnsFormattedErrorMessage()
        {
            var sut = new MaxAttribute(1);

            sut.ErrorMessage = "My error.";

            var error = sut.FormatErrorMessage("property");

            Assert.IsTrue(error == "My error.");
        }

        /// <summary>
        /// Test returns formatted error message.
        /// </summary>
        [TestMethod]
        public void Max_FormatErrorMessageWithErrorResource_ReturnsFormattedErrorMessage()
        {
            var sut = new MaxAttribute(1);

            sut.ErrorMessageResourceName = "MaxAttribute_Invalid";
            sut.ErrorMessageResourceType = typeof(DataAnnotationsResources);

            var error = sut.FormatErrorMessage("property");

            Assert.IsTrue(error == string.Format(DataAnnotationsResources.MaxAttribute_Invalid, "property", 1));
        }

        private class Model
        {
            [Max(1)]
            public int Value1 { get; set; }
        }

        /// <summary>
        /// Test returns client validation rules.
        /// </summary>
        [TestMethod]
        public void Max_GetClientValidationRules_ReturnsClientValidationRules()
        {
            var model = new Model() { Value1 = 1 };

            System.Web.Mvc.ModelMetadataProviders.Current = new InfrastructureModelMetadataProvider();

            var metadata = System.Web.Mvc.ModelMetadataProviders.Current.GetMetadataForProperties(model, model.GetType()).FirstOrDefault(m => m.PropertyName == "Value1");

            var sut = (MaxAttribute)model.GetType().GetProperty("Value1").GetCustomAttributes(typeof(MaxAttribute), false)[0];

            var rules = sut.GetClientValidationRules(metadata, new ControllerContext());

            Assert.IsTrue(rules.Any());
        }

        /// <summary>
        /// Test validates if value is null.
        /// </summary>
        [TestMethod]
        public void Max_NullValue_Validates()
        {
            var sut = new MaxAttribute(1);

            Assert.IsTrue(sut.IsValid(null));
        }

        /// <summary>
        /// Test validates if value is null.
        /// </summary>
        [TestMethod]
        public void Max_EmptyValue_Validates()
        {
            var sut = new MaxAttribute(1);

            Assert.IsTrue(sut.IsValid(string.Empty));
        }
    }
}
