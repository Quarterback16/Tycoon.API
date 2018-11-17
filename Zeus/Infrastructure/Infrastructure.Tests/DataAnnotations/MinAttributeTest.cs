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
    /// Unit tests for <see cref="MinAttribute" />.
    /// </summary>
    [TestClass]
    public class MinAttributeTest
    {
        /// <summary>
        /// Test validates if value is equal to the minimum value.
        /// </summary>
        [TestMethod]
        public void Min_MinimumIsIntegerAndTheValueIsEqualTo_Validates()
        {
            int value = 1;
            var sut = new MinAttribute(1);

            Assert.IsTrue(sut.IsValid(value));
        }

        /// <summary>
        /// Test validates if value is greater than the minimum value.
        /// </summary>
        [TestMethod]
        public void Min_MinimumIsIntegerAndTheValueIsGreaterThan_Validates()
        {
            int value = 2;
            var sut = new MinAttribute(1);

            Assert.IsTrue(sut.IsValid(value));
        }

        /// <summary>
        /// Test fails if value is less than the minimum value.
        /// </summary>
        [TestMethod]
        public void Min_MinimumIsIntegerAndTheValueIsLessThan_Fails()
        {
            int value = 0;
            var sut = new MinAttribute(1);

            Assert.IsFalse(sut.IsValid(value));
        }

        /// <summary>
        /// Test validates if value is equal to the minimum value.
        /// </summary>
        [TestMethod]
        public void Min_MinimumIsDoubleAndTheValueIsEqualTo_Validates()
        {
            double value = 1.1;
            var sut = new MinAttribute(1.1);

            Assert.IsTrue(sut.IsValid(value));
        }

        /// <summary>
        /// Test validates if value is greater than the minimum value.
        /// </summary>
        [TestMethod]
        public void Min_MinimumIsDoubleAndTheValueIsGreaterThan_Validates()
        {
            double value = 2.2;
            var sut = new MinAttribute(1.1);

            Assert.IsTrue(sut.IsValid(value));
        }

        /// <summary>
        /// Test fails if value is less than the minimum value.
        /// </summary>
        [TestMethod]
        public void Min_MinimumIsDoubleAndTheValueIsLessThan_Fails()
        {
            double value = 0;
            var sut = new MinAttribute(1.1);

            Assert.IsFalse(sut.IsValid(value));
        }

        /// <summary>
        /// Test validates if value is equal to the minimum value.
        /// </summary>
        [TestMethod]
        public void Min_MinimumIsSpecifiedTypeAndTheValueIsEqualTo_Validates()
        {
            long value = 1;
            var sut = new MinAttribute(typeof(long), "1");

            Assert.IsTrue(sut.IsValid(value));
        }

        /// <summary>
        /// Test validates if value is greater than the minimum value.
        /// </summary>
        [TestMethod]
        public void Min_MinimumIsSpecifiedTypeAndTheValueIsGreaterThan_Validates()
        {
            long value = 2;
            var sut = new MinAttribute(typeof(long), "1");

            Assert.IsTrue(sut.IsValid(value));
        }

        /// <summary>
        /// Test fails if value is less than the minimum value.
        /// </summary>
        [TestMethod]
        public void Min_MinimumIsSpecifiedTypeAndTheValueIsLessThan_Fails()
        {
            long value = 0;
            var sut = new MinAttribute(typeof(long), "1");

            Assert.IsFalse(sut.IsValid(value));
        }

        /// <summary>
        /// Test returns formatted error message.
        /// </summary>
        [TestMethod]
        public void Min_FormatErrorMessage_ReturnsFormattedErrorMessage()
        {
            var sut = new MinAttribute(1);

            var error = sut.FormatErrorMessage("property");

            Assert.IsTrue(!string.IsNullOrEmpty(error));
        }

        /// <summary>
        /// Test returns formatted error message.
        /// </summary>
        [TestMethod]
        public void Min_FormatErrorMessageWithErrorMessage_ReturnsFormattedErrorMessage()
        {
            var sut = new MinAttribute(1);

            sut.ErrorMessage = "My error.";

            var error = sut.FormatErrorMessage("property");

            Assert.IsTrue(error == "My error.");
        }

        /// <summary>
        /// Test returns formatted error message.
        /// </summary>
        [TestMethod]
        public void Min_FormatErrorMessageWithErrorResource_ReturnsFormattedErrorMessage()
        {
            var sut = new MinAttribute(1);

            sut.ErrorMessageResourceName = "MinAttribute_Invalid";
            sut.ErrorMessageResourceType = typeof(DataAnnotationsResources);

            var error = sut.FormatErrorMessage("property");

            Assert.IsTrue(error == string.Format(DataAnnotationsResources.MinAttribute_Invalid, "property", 1));
        }

        private class Model
        {
            [Min(1)]
            public int Value1 { get; set; }
        }

        /// <summary>
        /// Test returns client validation rules.
        /// </summary>
        [TestMethod]
        public void Min_GetClientValidationRules_ReturnsClientValidationRules()
        {
            var model = new Model() { Value1 = 1 };

            System.Web.Mvc.ModelMetadataProviders.Current = new InfrastructureModelMetadataProvider();

            var metadata = System.Web.Mvc.ModelMetadataProviders.Current.GetMetadataForProperties(model, model.GetType()).FirstOrDefault(m => m.PropertyName == "Value1");

            var sut = (MinAttribute)model.GetType().GetProperty("Value1").GetCustomAttributes(typeof(MinAttribute), false)[0];

            var rules = sut.GetClientValidationRules(metadata, new ControllerContext());

            Assert.IsTrue(rules.Any());
        }

        /// <summary>
        /// Test validates if value is null.
        /// </summary>
        [TestMethod]
        public void Min_NullValue_Validates()
        {
            var sut = new MinAttribute(1);

            Assert.IsTrue(sut.IsValid(null));
        }

        /// <summary>
        /// Test validates if value is null.
        /// </summary>
        [TestMethod]
        public void Min_EmptyValue_Validates()
        {
            var sut = new MinAttribute(1);

            Assert.IsTrue(sut.IsValid(string.Empty));
        }
    }
}
