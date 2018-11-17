using System;
using System.Linq;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.ModelMetadataProviders;
using Employment.Web.Mvc.Infrastructure.Properties;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="EqualToAttribute" />.
    /// </summary>
    [TestClass]
    public class EqualToAttributeTest
    {
        private class Model : ContingentValidationModel<EqualToAttribute>
        {
            public string Value1 { get; set; }

            [EqualTo("Value1")]
            public string Value2 { get; set; }

            [EqualTo("Value1", ErrorMessage = "My error.")]
            public string Value3 { get; set; }

            [EqualTo("Value1", ErrorMessageResourceName = "IsAttribute_Invalid", ErrorMessageResourceType = typeof(DataAnnotationsResources))]
            public string Value4 { get; set; }
        }

        private class NullModel : ContingentValidationModel<EqualToAttribute>
        {
            [EqualTo(null)]
            public string Value5 { get; set; }
        }

        private class ModelWithPassOnNull : ContingentValidationModel<EqualToAttribute>
        {
            public string Value1 { get; set; }

            [EqualTo("Value1", PassOnNull = true)]
            public string Value2 { get; set; }
        }

        private class IsModel : ContingentValidationModel<IsAttribute>
        {
            public string Value1 { get; set; }

            

            [EqualTo("Value1", ErrorMessage = "My error.")]
            public string Value3 { get; set; }

            [EqualTo("Value1", ErrorMessageResourceName = "IsAttribute_Invalid", ErrorMessageResourceType = typeof(DataAnnotationsResources))]
            public string Value4 { get; set; }
        }

        /// <summary>
        /// Test <see cref="ArgumentNullException" /> is thrown when instantiated with null dependent property.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_CalledWithNullDependentPropertyArgument_ThrowsArgumentNullException()
        {
            var model = new NullModel();

            model.IsValid("Value5");
        }

        /// <summary>
        /// Test property validates if dependent value is equal to property value.
        /// </summary>
        [TestMethod]
        public void EqualTo_DependentValueIsEqualToPropertyValue_Validates()
        {
            var model = new Model() { Value1 = "hello", Value2 = "hello" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property fails validation if dependent value is not equal to property value.
        /// </summary>
        [TestMethod]
        public void EqualTo_DependentValueIsNotEqualToPropertyValue_Fails()
        {
            var model = new Model() { Value1 = "hello", Value2 = "goodbye" };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property fails validation if dependent value and property value are both null.
        /// </summary>
        [TestMethod]
        public void EqualTo_BothValuesNull_Fails()
        {
            var model = new Model() { };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property fails validation if dependent value is null.
        /// </summary>
        [TestMethod]
        public void EqualTo_DependentValueNull_Fails()
        {
            var model = new Model() { Value2 = "hello" };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property fails validation if property value is null.
        /// </summary>
        [TestMethod]
        public void EqualTo_PropertyValueNull_Fails()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property validates if dependent value is null and pass on null is true.
        /// </summary>
        [TestMethod]
        public void EqualTo_DependentValueNullAndPassOnNull_Validates()
        {
            var model = new ModelWithPassOnNull() { Value2 = "hello" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property validates if property value is null and pass on null is true.
        /// </summary>
        [TestMethod]
        public void EqualTo_PropertyValueNullAndPassOnNull_Validates()
        {
            var model = new ModelWithPassOnNull() { Value1 = "hello" };
            Assert.IsTrue(model.IsValid("Value2"));
        }


        /// <summary>
        /// Test property uses error message.
        /// </summary>
        [TestMethod]
        public void EqualTo_FormatErrorMessageUsesErrorMessage_ReturnsUsingErrorMessage()
        {
            var model = new IsModel() { Value1 = "hello" };
            var error = model.GetAttribute("Value3").FormatErrorMessage("Value3");
            Assert.IsTrue(error == "My error.");
        }

        /// <summary>
        /// Test property uses error resource.
        /// </summary>
        [TestMethod]
        public void EqualTo_FormatErrorMessageUsesErrorResource_ReturnsUsingErrorResource()
        {
            var model = new IsModel() { Value1 = "hello" };
            var error = model.GetAttribute("Value4").FormatErrorMessage("Value4");
            Assert.IsTrue(!string.IsNullOrEmpty(error));
        }

        /// <summary>
        /// Test get client validation rules.
        /// </summary>
        [TestMethod]
        public void EqualTo_GetClientValidationRules_ReturnsGetClientValidationRules()
        {
            var model = new IsModel();

            System.Web.Mvc.ModelMetadataProviders.Current = new InfrastructureModelMetadataProvider();

            var metadata = System.Web.Mvc.ModelMetadataProviders.Current.GetMetadataForProperties(model, model.GetType()).FirstOrDefault(m => m.PropertyName == "Value3");

            var rules = model.GetAttribute("Value3").GetClientValidationRules(metadata, new ControllerContext());

            Assert.IsTrue(rules.Any());
        }

        /// <summary>
        /// Test property returns formatted error message.
        /// </summary>
        [TestMethod]
        public void EqualTo_GetOtherDisplayNameWithNullContainer_UsesDependentProperty()
        {
            var model = new IsModel() { Value1 = "hello", Value3 = string.Empty };

            System.Web.Mvc.ModelMetadataProviders.Current = new InfrastructureModelMetadataProvider();

            var metadata = System.Web.Mvc.ModelMetadataProviders.Current.GetMetadataForProperties(model, model.GetType()).FirstOrDefault(m => m.PropertyName == "Value3");

            // Set container to null
            metadata.AdditionalValues["ParentModel"] = null;

            var rules = model.GetAttribute("Value3").GetClientValidationRules(metadata, new ControllerContext());

            Assert.IsTrue(rules.Any());
        }
    }
}
