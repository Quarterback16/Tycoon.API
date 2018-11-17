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
    /// Unit tests for <see cref="CrnAttribute" />.
    /// </summary>
    [TestClass]
    public class CrnAttributeTest
    {
        /// <summary>
        /// Test returns formatted error message.
        /// </summary>
        [TestMethod]
        public void Crn_FormatErrorMessage_ReturnsFormattedErrorMessage()
        {
            var sut = new CrnAttribute();

            var error = sut.FormatErrorMessage("property");

            Assert.IsTrue(!string.IsNullOrEmpty(error));
        }

        /// <summary>
        /// Test returns formatted error message.
        /// </summary>
        [TestMethod]
        public void Crn_FormatErrorMessageWithErrorMessage_ReturnsFormattedErrorMessage()
        {
            var sut = new CrnAttribute();

            sut.ErrorMessage = "My error.";

            var error = sut.FormatErrorMessage("property");

            Assert.IsTrue(error == "My error.");
        }

        /// <summary>
        /// Test returns formatted error message.
        /// </summary>
        [TestMethod]
        public void Crn_FormatErrorMessageWithErrorResource_ReturnsFormattedErrorMessage()
        {
            var sut = new CrnAttribute();

            sut.ErrorMessageResourceName = "CrnAttribute_Invalid";
            sut.ErrorMessageResourceType = typeof(DataAnnotationsResources);

            var error = sut.FormatErrorMessage("property");

            Assert.IsTrue(error == string.Format(DataAnnotationsResources.CrnAttribute_Invalid, "property"));
        }

        private class Model
        {
            [Crn]
            public string Value1 { get; set; }
        }

        /// <summary>
        /// Test returns client validation rules.
        /// </summary>
        [TestMethod]
        public void Crn_GetClientValidationRules_ReturnsClientValidationRules()
        {
            var model = new Model() { Value1 = "123456789T" };

            System.Web.Mvc.ModelMetadataProviders.Current = new InfrastructureModelMetadataProvider();

            var metadata = System.Web.Mvc.ModelMetadataProviders.Current.GetMetadataForProperties(model, model.GetType()).FirstOrDefault(m => m.PropertyName == "Value1");

            var sut = (CrnAttribute)model.GetType().GetProperty("Value1").GetCustomAttributes(typeof(CrnAttribute), false)[0];

            var rules = sut.GetClientValidationRules(metadata, new ControllerContext());

            Assert.IsTrue(rules.Any());
        }

        /// <summary>
        /// Test validates if value is valid.
        /// </summary>
        [TestMethod]
        public void Crn_ValidValue_Validates()
        {
            var sut = new CrnAttribute();

            Assert.IsTrue(sut.IsValid("123456789T"));
        }

        /// <summary>
        /// Test validates if value is null.
        /// </summary>
        [TestMethod]
        public void Crn_NullValue_Validates()
        {
            var sut = new CrnAttribute();

            Assert.IsTrue(sut.IsValid(null));
        }

        /// <summary>
        /// Test validates if value is null.
        /// </summary>
        [TestMethod]
        public void Crn_EmptyValue_Validates()
        {
            var sut = new CrnAttribute();

            Assert.IsTrue(sut.IsValid(string.Empty));
        }

        /// <summary>
        /// Test fails if checksum is invalid.
        /// </summary>
        [TestMethod]
        public void Crn_InvalidChecksum_Fails()
        {
            var sut = new CrnAttribute();

            Assert.IsFalse(sut.IsValid("123456789X"));
        }

        /// <summary>
        /// Test fail if value is not 10 characters long.
        /// </summary>
        [TestMethod]
        public void Crn_InvalidLength_Fails()
        {
            var sut = new CrnAttribute();

            Assert.IsFalse(sut.IsValid("123"));
        }

        /// <summary>
        /// Test fail if value does not have valid characters.
        /// </summary>
        [TestMethod]
        public void Crn_InvalidCharacters_Fails()
        {
            var sut = new CrnAttribute();

            Assert.IsFalse(sut.IsValid("ABCDEFGHIJ"));
        }
    }
}
