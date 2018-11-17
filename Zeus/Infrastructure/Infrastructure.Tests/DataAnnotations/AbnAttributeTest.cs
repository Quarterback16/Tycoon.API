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
    /// Unit tests for <see cref="AbnAttribute" />.
    /// </summary>
    [TestClass]
    public class AbnAttributeTest
    {
        /// <summary>
        /// Test returns formatted error message.
        /// </summary>
        [TestMethod]
        public void Abn_FormatErrorMessage_ReturnsFormattedErrorMessage()
        {
            var sut = new AbnAttribute();

            var error = sut.FormatErrorMessage("property");

            Assert.IsTrue(!string.IsNullOrEmpty(error));
        }

        /// <summary>
        /// Test returns formatted error message.
        /// </summary>
        [TestMethod]
        public void Abn_FormatErrorMessageWithErrorMessage_ReturnsFormattedErrorMessage()
        {
            var sut = new AbnAttribute();

            sut.ErrorMessage = "My error.";

            var error = sut.FormatErrorMessage("property");

            Assert.IsTrue(error == "My error.");
        }

        /// <summary>
        /// Test returns formatted error message.
        /// </summary>
        [TestMethod]
        public void Abn_FormatErrorMessageWithErrorResource_ReturnsFormattedErrorMessage()
        {
            var sut = new AbnAttribute();

            sut.ErrorMessageResourceName = "AbnAttribute_Invalid";
            sut.ErrorMessageResourceType = typeof(DataAnnotationsResources);

            var error = sut.FormatErrorMessage("property");

            Assert.IsTrue(error == string.Format(DataAnnotationsResources.AbnAttribute_Invalid, "property"));
        }

        private class Model
        {
            [Abn]
            public string Value1 { get; set; }
        }

        /// <summary>
        /// Test returns client validation rules.
        /// </summary>
        [TestMethod]
        public void Abn_GetClientValidationRules_ReturnsClientValidationRules()
        {
            var model = new Model() { Value1 = "53004085616" };

            System.Web.Mvc.ModelMetadataProviders.Current = new InfrastructureModelMetadataProvider();

            var metadata = System.Web.Mvc.ModelMetadataProviders.Current.GetMetadataForProperties(model, model.GetType()).FirstOrDefault(m => m.PropertyName == "Value1");

            var sut = (AbnAttribute)model.GetType().GetProperty("Value1").GetCustomAttributes(typeof(AbnAttribute), false)[0];

            var rules = sut.GetClientValidationRules(metadata, new ControllerContext());

            Assert.IsTrue(rules.Any());
        }

        /// <summary>
        /// Test validates if value is valid.
        /// </summary>
        [TestMethod]
        public void Abn_ValidValue_Validates()
        {
            var sut = new AbnAttribute();

            Assert.IsTrue(sut.IsValid("53004085616"));
        }

        /// <summary>
        /// Test validates if value is null.
        /// </summary>
        [TestMethod]
        public void Abn_NullValue_Validates()
        {
            var sut = new AbnAttribute();

            Assert.IsTrue(sut.IsValid(null));
        }

        /// <summary>
        /// Test validates if value is null.
        /// </summary>
        [TestMethod]
        public void Abn_EmptyValue_Validates()
        {
            var sut = new AbnAttribute();

            Assert.IsTrue(sut.IsValid(string.Empty));
        }

        /// <summary>
        /// Test fails if abn is invalid.
        /// </summary>
        [TestMethod]
        public void Abn_InvalidAbn_Fails()
        {
            var sut = new AbnAttribute();

            Assert.IsFalse(sut.IsValid("12345678910"));
        }

        /// <summary>
        /// Test fail if value is not 11 characters long.
        /// </summary>
        [TestMethod]
        public void Abn_InvalidLength_Fails()
        {
            var sut = new AbnAttribute();

            Assert.IsFalse(sut.IsValid("123"));
        }

        /// <summary>
        /// Test fail if value does not have valid characters (all digits).
        /// </summary>
        [TestMethod]
        public void Abn_InvalidCharacters_Fails()
        {
            var sut = new AbnAttribute();

            Assert.IsFalse(sut.IsValid("ABCDEFGHIJK"));
        }
    }
}
