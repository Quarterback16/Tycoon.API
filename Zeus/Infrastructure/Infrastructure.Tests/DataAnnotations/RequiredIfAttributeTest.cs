using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using System.Linq;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.ModelMetadataProviders;
using Employment.Web.Mvc.Infrastructure.Properties;
using Employment.Web.Mvc.Infrastructure.Tests.Base;
using Employment.Web.Mvc.Infrastructure.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="RequiredIfAttribute" />.
    /// </summary>
    [TestClass]
    public class RequiredIfAttributeTest
    {
        private Mock<IUserService> mockUserService;
        private Mock<IAdwService> mockAdwService;
        private Mock<IContainerProvider> mockContainerProvider;

        private readonly Dictionary<string, string> States = new Dictionary<string, string>{ {"ACT", "ACT"}, {"SA", "SA"}, {"NT", "NT" }, {"NSW", "NSW"}, {"QLD", "QLD"}, {"TAS", "TAS"}, {"VIC", "VIC"}, {"WA", "WA"} } ;

        //Use TestInitialize to run code before running each test
        [TestInitialize]
        public void TestInitialize()
        {
            mockUserService = new Mock<IUserService>();
            mockAdwService = new Mock<IAdwService>();
            mockContainerProvider = new Mock<IContainerProvider>();

            // Used by attributes via directly access of Dependency Resolver (attributes aren't able have these populated by Dependency Injection)
            mockContainerProvider.Setup(m => m.GetService<IAdwService>()).Returns(mockAdwService.Object);
            mockContainerProvider.Setup(m => m.GetService<IUserService>()).Returns(mockUserService.Object);

            DependencyResolver.SetResolver(mockContainerProvider.Object);
        }

        private class Model : ContingentValidationModel<RequiredIfAttribute>
        { 
            public string Value1 { get; set; }

            [RequiredIf("Value1", "hello")]
            public string Value2 { get; set; }

            [RequiredIf("Value1", new[] { "hello", "world" })]
            public string Value3 { get; set; }

            [RequiredIf("Value1", new[] { "hello", "world" }, ErrorMessageResourceName = "RequiredIfAttribute_Invalid", ErrorMessageResourceType = typeof(DataAnnotationsResources))]
            public string Value4 { get; set; }

            [RequiredIf("Value1", new[] { "hello", "world" }, ErrorMessage = "My error.")]
            public string Value5 { get; set; }

            [RequiredIf("NotExist", "Non-Existant DependentProperty")]
            public string Value6 { get; set; }
            
            [RequiredIf("Value1", "hello")]
            public int Value7 { get; set; }

            [RequiredIf("Value1", "hello")]
            public decimal Value8 { get; set; }

            [RequiredIf("Value1", "")]
            public string Value9 { get; set; }

            [RequiredIf("Value1", "hello", PassOnNull = true)]
            public string Value10 { get; set; }

            [RequiredIf("Value1", "hello", PassOnNull = true)]
            public DateTime? Value11 { get; set; }

            [RequiredIf("Value1", "hello", FailOnNull = true)]
            public string Value12 { get; set; }

            [RequiredIf("Value1", "hello", FailOnNull = true)]
            public DateTime? Value13 { get; set; }

            [RequiredIf("Value15", new[] { "hello", "world" })]
            public DateTime? Value14 { get; set; }

            public SelectList Value15 { get; set; }

            [RequiredIf("Value17", new[] { "hello", "world" })]
            public DateTime? Value16 { get; set; }

            [RequiredIf("Value15", "world")]
            public MultiSelectList Value17 { get; set; }

            public SelectList BaseSelectList { get; set; }

            [RequiredIf("BaseSelectList", ComparisonType.EqualTo , "ACT")]
            public SelectList SingleValueDependentSelectList { get; set; }


            [RequiredIf("BaseSelectList", ComparisonType.NotEqualTo, "NSW")]
            public SelectList SingleValueNotEqualToDependentSelectList { get; set; }

            [RequiredIf("BaseSelectList", ComparisonType.EqualTo, new [] {"ACT", "NSW"})]
            public SelectList MultipleValuesDependentSelectList { get; set; }


            [RequiredIf("BaseSelectList", ComparisonType.NotEqualTo, new []{ "QLD", "NT"}, PassOnNull = true)]
            public SelectList MultipleValuesNotEqualToDependentSelectList { get; set; }


            [RequiredIf("BaseSelectList", ComparisonType.EqualTo, "ACT")]
            public MultiSelectList SingleValueDependentMultiSelect { get; set; }


            [RequiredIf("BaseSelectList", ComparisonType.NotEqualTo, "ACT")]
            public MultiSelectList SingleValueNotEqualToDependentMultiSelect { get; set; }

            
            [RequiredIf("BaseSelectList", ComparisonType.EqualTo, new[] { "SA", "QLD" })]    //new Dictionary<string, string> { { "SA", "SA" }, { "QLD", "QLD" } }
            public MultiSelectList MultipleValuesDependentMultiSelect { get; set; }

            [RequiredIf("BaseSelectList", ComparisonType.NotEqualTo, new[] { "NSW", "NT" }, PassOnNull = true)]
            public MultiSelectList MultipleValuesNotEqualToDependentMultiSelect { get; set; }



            public MultiSelectList BaseMultiSelect { get; set; }

            [RequiredIf("BaseMultiSelect", ComparisonType.EqualTo, new []{"ACT"})]
            public SelectList SingleValueDependentSelectList2 { get; set; }


            [RequiredIf("BaseMultiSelect", ComparisonType.NotEqualTo, new[] { "ACT" })]
            public SelectList SingleValueNotEqualToDependentSelectList2 { get; set; }

            [RequiredIf("BaseMultiSelect", ComparisonType.EqualTo, new[] { "SA", "QLD" })]
            public SelectList MultipleValuesDependentSelectList2 { get; set; }


            [RequiredIf("BaseMultiSelect", ComparisonType.NotEqualTo, new[] { "NSW", "NT" })]
            public SelectList MultipleValuesNotEqualToDependentSelectList2 { get; set; }


            [RequiredIf("BaseMultiSelect", ComparisonType.EqualTo, new []{"ACT"})]
            public MultiSelectList SingleValueDependentMultiSelect2 { get; set; }


            [RequiredIf("BaseMultiSelect", ComparisonType.NotEqualTo, "ACT")]
            public MultiSelectList SingleValueNotEqualToDependentMultiSelect2 { get; set; }


            [RequiredIf("BaseMultiSelect", ComparisonType.EqualTo, new[] { "SA", "QLD" })]  
            public MultiSelectList MultipleValuesDependentMultiSelect2 { get; set; }

            [RequiredIf("BaseMultiSelect", ComparisonType.NotEqualTo, new[] { "NSW", "NT" })]
            public MultiSelectList MultipleValuesNotEqualToDependentMultiSelect2 { get; set; }

            
        }

        private class TestModel : ContingentValidationModel<TestAttribute>
        {
            public string Value1 { get; set; }

            [TestAttribute("Value1")]
            public string Value2 { get; set; }
        }

        public class TestAttribute : ContingentValidationAttribute
        {
            public TestAttribute(string dependentProperty) : base(dependentProperty) { }

            public override bool IsConditionMet(object propertyValue, object dependentPropertyValue)
            {
                return true;
            }
        }


       

        /// <summary>
        /// Test default error.
        /// </summary>
        [TestMethod]
        public void RequiredIf_DefaultError_ReturnsDefaultErrorMessage()
        {
            var model = new TestModel() { Value1 = "hello", Value2 = string.Empty };

            var error = model.GetAttribute("Value2").FormatErrorMessage("Value2");

            Assert.IsTrue(!string.IsNullOrEmpty(error));
        }

        /// <summary>
        /// Test exception is thrown for invalid dependent property.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RequiredIf_InvalidDependentProperty_ThrowsInvalidOperationException()
        {
            var model = new Model() { Value6 = "hello" };
            model.IsValid("Value6");
        }

        /// <summary>
        /// Test property returns formatted error message.
        /// </summary>
        [TestMethod]
        public void RequiredIf_FormatErrorMessage_ReturnsFormattedErrorMessage()
        {
            var model = new Model() { Value1 = "hello", Value2 = string.Empty };

            var error = model.GetAttribute("Value2").FormatErrorMessage("Value2");

            Assert.IsTrue(!string.IsNullOrEmpty(error));
        }

        /// <summary>
        /// Test property returns formatted error message.
        /// </summary>
        [TestMethod]
        public void RequiredIf_FormatErrorMessageWithErrorMessage_ReturnsFormattedErrorMessage()
        {
            var model = new Model() { Value1 = "hello", Value5 = string.Empty };

            var error = model.GetAttribute("Value5").FormatErrorMessage("Value5");

            Assert.IsTrue(!string.IsNullOrEmpty(error));
        }

        /// <summary>
        /// Test property returns formatted error message.
        /// </summary>
        [TestMethod]
        public void RequiredIf_FormatErrorMessageWithErrorResource_ReturnsFormattedErrorMessage()
        {
            var model = new Model() { Value1 = "hello", Value4 = string.Empty };

            var error = model.GetAttribute("Value4").FormatErrorMessage("Value4");

            Assert.IsTrue(!string.IsNullOrEmpty(error));
        }

        /// <summary>
        /// Test property returns formatted error message.
        /// </summary>
        [TestMethod]
        public void RequiredIf_GetOtherDisplayNameWithNullContainer_UsesDependentProperty()
        {
            var model = new Model() { Value1 = "hello", Value2 = string.Empty };

            System.Web.Mvc.ModelMetadataProviders.Current = new InfrastructureModelMetadataProvider();

            var metadata = System.Web.Mvc.ModelMetadataProviders.Current.GetMetadataForProperties(model, model.GetType()).FirstOrDefault(m => m.PropertyName == "Value2");

            // Set container to null
            metadata.AdditionalValues["ParentModel"] = null;

            var rules = model.GetAttribute("Value2").GetClientValidationRules(metadata, new ControllerContext());

            Assert.IsTrue(rules.Any());
        }

        /// <summary>
        /// Test property is required and fails if condition is met with the expected value and a value is not supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIf_GetClientValidationRules_ReturnsClientValidationRules()
        {
            var model = new Model() { Value1 = "hello", Value2 = string.Empty };

            System.Web.Mvc.ModelMetadataProviders.Current = new InfrastructureModelMetadataProvider();

            var metadata = System.Web.Mvc.ModelMetadataProviders.Current.GetMetadataForProperties(model, model.GetType()).FirstOrDefault(m => m.PropertyName == "Value2");

            var rules = model.GetAttribute("Value2").GetClientValidationRules(metadata, new ControllerContext());

            Assert.IsTrue(rules.Any());
        }
         
        


        /// <summary>
        /// Test property is required and validates if condition is met with the expected value and a value is supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIf_ConditionMetWithExpectedValueAndValueSupplied_RequiredAndValidates()
        {
            var model = new Model() { Value1 = "hello", Value2 = "supplied" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property is required and fails if condition is met with the expected value and a value is not supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIf_ConditionMetWithExpectedValueAndNoValueSupplied_RequiredAndFails()
        {
            var model = new Model() { Value1 = "hello", Value2 = string.Empty };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property is not required and validates if condition is met with a different value.
        /// </summary>
        [TestMethod]
        public void RequiredIf_ConditionMetWithDifferentValue_NotRequiredAndValidates()
        {
            var model = new Model() { Value1 = "goodbye" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property is not required and validates if condition is met with an empty string.
        /// </summary>
        [TestMethod]
        public void RequiredIf_ConditionMetWithEmptyString_NotRequiredAndValidates()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property is not required and validates if condition is met with null.
        /// </summary>
        [TestMethod]
        public void RequiredIf_ConditionMetWithNull_NotRequiredAndValidates()
        {
            var model = new Model() { Value1 = null };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        /// <summary>
        /// Test property is required and validates if condition is met with the expected value and a value is supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIf_ConditionMetWithOneOfTheExpectedValuesAndValueSupplied_RequiredAndValidates()
        {
            var model = new Model() { Value1 = "world", Value3 = "supplied" };
            Assert.IsTrue(model.IsValid("Value3"));
        }

        /// <summary>
        /// Test property is required and fails if condition is met with the expected value and a value is not supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIf_ConditionMetWithOneOfTheExpectedValuesAndEmptyStringSupplied_RequiredAndFails()
        {
            var model = new Model() { Value1 = "world", Value3 = string.Empty };
            Assert.IsFalse(model.IsValid("Value3"));
        }

        /// <summary>
        /// Test property is required and fails if condition is met with the expected value and a value is not supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIf_ConditionMetWithOneOfTheExpectedValuesAndNullSupplied_RequiredAndFails()
        {
            var model = new Model() { Value1 = "world", Value3 = null };
            Assert.IsFalse(model.IsValid("Value3"));
        }

        /// <summary>
        /// Test property is not required and validates if condition is met with a different value.
        /// </summary>
        [TestMethod]
        public void RequiredIf_ConditionMetWithDifferentValueToOneOfTheExpectedVales_NotRequiredAndValidates()
        {
            var model = new Model() { Value1 = "goodbye" };
            Assert.IsTrue(model.IsValid("Value3"));
        }

        /// <summary>
        /// Test property is not required and validates if condition is met with an empty string.
        /// </summary>
        [TestMethod]
        public void RequiredIf_ConditionMetWithEmptyStringInsteadOfOneOfTheExpectedValesWithValueSupplied_NotRequiredAndValidates()
        {
            var model = new Model() { Value1 = string.Empty, Value3 = "supplied" };
            Assert.IsTrue(model.IsValid("Value3"));
        }

        /// <summary>
        /// Test property is not required and validates if condition is met with an empty string.
        /// </summary>
        [TestMethod]
        public void RequiredIf_ConditionMetWithEmptyStringInsteadOfOneOfTheExpectedVales_NotRequiredAndValidates()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsTrue(model.IsValid("Value3"));
        }

        /// <summary>
        /// Test property is not required and validates if condition is met with null.
        /// </summary>
        [TestMethod]
        public void RequiredIf_ConditionMetWithNullInsteadOfOneOfTheExpectedValesWithValueSupplied_NotRequiredAndValidates()
        {
            var model = new Model() { Value1 = null, Value3 = "supplied" };
            Assert.IsTrue(model.IsValid("Value3"));
        }

        /// <summary>
        /// Test property is not required and validates if condition is met with null.
        /// </summary>
        [TestMethod]
        public void RequiredIf_ConditionMetWithNullInsteadOfOneOfTheExpectedVales_NotRequiredAndValidates()
        {
            var model = new Model() { Value1 = null };
            Assert.IsTrue(model.IsValid("Value3"));
        }

        /// <summary>
        /// Test property is required and fails if condition is met with the expected value and a value on the now required property is an integer with no value supplied.
        /// </summary>
        [TestMethod]
        public void RequiredIf_ConditionMetWithExpectedValueAndNoIntegerValueSupplied_RequiredAndFails()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsFalse(model.IsValid("Value7"));
        }

        /// <summary>
        /// Test property is required and fails if condition is met with the expected value and a value on the now required property is an decimal with no value supplied.
        /// </summary>
        [TestMethod]
        public void RequiredIf_ConditionMetWithExpectedValueAndNoDecimalValueSupplied_RequiredAndFails()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsFalse(model.IsValid("Value8"));
        }

        /// <summary>
        /// Test property is required and validates if condition is met with the expected value and a value on the now required property is an integer with a value supplied.
        /// </summary>
        [TestMethod]
        public void RequiredIf_ConditionMetWithExpectedValueAndIntegerValueSupplied_RequiredAndValidates()
        {
            var model = new Model() { Value1 = "hello", Value7 = 1 };
            Assert.IsTrue(model.IsValid("Value7"));
        }

        /// <summary>
        /// Test property is required and validates if condition is met with the expected value and a value on the now required property is an decimal with a value supplied.
        /// </summary>
        [TestMethod]
        public void RequiredIf_ConditionMetWithExpectedValueAndDecimalValueSupplied_RequiredAndValidates()
        {
            var model = new Model() { Value1 = "hello", Value8 = 1 };
            Assert.IsTrue(model.IsValid("Value8"));
        }

        /// <summary>
        /// Test property is required and fails if condition is met with the expected value which is an empty string, and no value is supplied.
        /// </summary>
        [TestMethod]
        public void RequiredIf_ConditionMetWithExpectedValueWhichIsAnEmptyString_RequiredAndFails()
        {
            var model = new Model() { Value1 = string.Empty };
            Assert.IsFalse(model.IsValid("Value9"));
        }

        /// <summary>
        /// Test property is not required and validates if condition is met with a null value and pass on null is true.
        /// </summary>
        [TestMethod]
        public void RequiredIf_ConditionMetWithNullAndPassOnNull_NotRequiredAndValidates()
        {
            var model = new Model() { Value1 = null };
            Assert.IsTrue(model.IsValid("Value10"));
        }

        /// <summary>
        /// Test property is not required and validates if condition is met with a null value and pass on null is true.
        /// </summary>
        [TestMethod]
        public void RequiredIf_DateTimeConditionMetWithNullAndPassOnNull_NotRequiredAndValidates()
        {
            var model = new Model() { Value1 = null };
            Assert.IsTrue(model.IsValid("Value11"));
        }

        /// <summary>
        /// Test property is required as it fails 'condition is met' with a null value and fail on null is true but passes because a value is supplied.
        /// </summary>
        [TestMethod]
        public void RequiredIf_ConditionMetWithNullAndFailOnNullAndValueSupplied_RequiredAndValidates()
        {
            var model = new Model() { Value1 = null, Value12 = "value" };
            Assert.IsTrue(model.IsValid("Value12"));
        }


        /// <summary>
        /// Test property is required and fails if condition is met with a null value and fail on null is true.
        /// </summary>
        [TestMethod]
        public void RequiredIf_ConditionMetWithNullAndFailOnNull_RequiredAndFails()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsValid("Value12"));
        }

        /// <summary>
        /// Test property is not required and validates if condition is met with a null value and fail on null is true.
        /// </summary>
        [TestMethod]
        public void RequiredIf_ConditionNotMetWithDifferentValueAndFailOnNull_NotRequiredAndValidates()
        {
            var model = new Model() { Value1 = "value" };
            Assert.IsTrue(model.IsValid("Value12"));
        }

        /// <summary>
        /// Test property is not required and validates if condition is met with a null value and fail on null is true.
        /// </summary>
        [TestMethod]
        public void RequiredIf_ConditionMetWithExpectedValueAndFailOnNull_RequiredAndFails()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsFalse(model.IsValid("Value12"));
        }

        /// <summary>
        /// Test property is not required and validates if condition is met with a null value and fail on null is true.
        /// </summary>
        [TestMethod]
        public void RequiredIf_ConditionMetWithExpectedValueAndFailOnNullAndValueProvided_RequiredAndValidates()
        {
            var model = new Model() { Value1 = "hello", Value12= "value" };
            Assert.IsTrue(model.IsValid("Value12"));
        }

        /// <summary>
        /// Test property is required and fails if condition is met with a null value and fail on null is true.
        /// </summary>
        [TestMethod]
        public void RequiredIf_DateTimeConditionMetWithNullAndFailOnNull_RequiredAndFails()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsValid("Value13"));
        }

        /// <summary>
        /// Test property is not required and validates if condition is met with a null value and fail on null is true.
        /// </summary>
        [TestMethod]
        public void RequiredIf_DateTimeConditionNotMetWithNullAndFailOnNull_NotRequiredAndValidates()
        {
            var model = new Model() { Value1 = "value" };
            Assert.IsTrue(model.IsValid("Value13"));
        }

        /// <summary>
        /// Test property is required and validates if condition is met with the expected value and a value is supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIf_DateTimeConditionMetWithOneOfTheExpectedValuesAndValueSupplied_RequiredAndValidates()
        {
            var list = new SelectList(new Dictionary<string, string> { { "hello", "hello" }, { "world", "world" }, { "goodbye", "goodbye"} }, "Key", "Value", "world");

            var model = new Model() { Value15 = list, Value14 = DateTime.Now };
            Assert.IsTrue(model.IsValid("Value14"));
        }



        #region Select List   <---->    Select List Tests

        // *** Single value and EQUAL TO Tests ***

        /// <summary>
        /// Validates if the condition is met with expected value and Value isn't supplied to SingleValueDependentSelectList.
        /// </summary>
        /// <remarks>
        /// SingleValueDependentSelectList is RequiredIF  BaseSelectList has "act" selected. 
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetWithExpectedValueAndValueNotSupplied_SelectListToSelectList()
        { 
            string dependentSelectedValue = "ACT";

            var model = new Model()
            {
                BaseSelectList = new SelectList(States, "Key", "Value", dependentSelectedValue),
                SingleValueDependentSelectList = new SelectList(States, "Key", "Value", null)
            };

            Assert.IsFalse(model.IsValid("SingleValueDependentSelectList"));
            /*
            var list = new SelectList(new Dictionary<string, string> { { "hello", "hello" }, { "world", "world" }, { "goodbye", "goodbye" } }, "Key", "Value", "world");
            var multiSelectList = new MultiSelectList(new Dictionary<string, string> { { "hello", "hello" }, { "world", "world" }, { "goodbye", "goodbye" } }, "Key", "Value", new[] { "goodbye" });

            var model = new Model() { Value15 = list, Value17 = multiSelectList };
            Assert.IsTrue(model.IsValid("Value17"));
            */
        }

        /// <summary>
        /// Validates if the condition is met with expected value and Value is supplied to SingleValueDependentSelectList.
        /// </summary>
        /// <remarks>
        /// SingleValueDependentSelectList is RequiredIF  BaseSelectList has "act" selected. 
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetWithExpectedValueAndValueSupplied_SelectListToSelectList()
        {
            string dependentSelectedValue = "ACT";

            var model = new Model()
            {
                BaseSelectList = new SelectList(States, "Key", "Value", dependentSelectedValue),
                SingleValueDependentSelectList = new SelectList(States, "Key", "Value", "SA")
            };

            Assert.IsTrue(model.IsValid("SingleValueDependentSelectList"));
        }

        /// <summary>
        /// Tests if the condition is met with unexpected value and Value is not supplied to SingleValueDependentSelectList.
        /// </summary>
        /// <remarks>
        /// SingleValueDependentSelectList is RequiredIF  BaseSelectList has "act" selected. 
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionNotMetDueToUnexpectedValueAndValueNotSupplied_SelectListToSelectList()
        {
            string dependentSelectedValue = "SA";

            var model = new Model()
            {
                BaseSelectList = new SelectList(States, "Key", "Value", dependentSelectedValue),
                SingleValueDependentSelectList = new SelectList(States, "Key", "Value", null)
            };

            Assert.IsTrue(model.IsValid("SingleValueDependentSelectList"));
        }



        // *** Single Value and Not Equal to Tests ***

        /// <summary>
        /// Validates if the condition is met with expected value and Value isn't supplied to SingleValueNotEqualToDependentSelectList.
        /// </summary>
        /// <remarks>
        /// SingleValueNotEqualToDependentSelectList is RequiredIF "NSW" is not selected in BaseSelectList.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetWithExpectedValueAndValueNotSupplied_NotEqualTo_SelectListToSelectList()
        {
            string dependentSelectedValue = "ACT";   // Condition is "Required if Not Equal to 'NSW'"

            var model = new Model()
            {
                BaseSelectList = new SelectList(States, "Key", "Value", dependentSelectedValue),
                SingleValueNotEqualToDependentSelectList = new SelectList(States, "Key", "Value", null)
            };

            Assert.IsFalse(model.IsValid("SingleValueNotEqualToDependentSelectList")); 
        }

        /// <summary>
        /// Validates if the condition is met with expected value and Value is supplied to SingleValueNotEqualToDependentSelectList.
        /// </summary>
        /// <remarks>
        /// SingleValueNotEqualToDependentSelectList is RequiredIF "NSW" is not selected in BaseSelectList.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetWithExpectedValueAndValueSupplied_NotEqualTo_SelectListToSelectList()
        {
            string dependentSelectedValue = "ACT";

            var model = new Model()
            {
                BaseSelectList = new SelectList(States, "Key", "Value", dependentSelectedValue),
                SingleValueNotEqualToDependentSelectList = new SelectList(States, "Key", "Value", "SA")
            };

            Assert.IsTrue(model.IsValid("SingleValueNotEqualToDependentSelectList"));
        }

        /// <summary>
        /// Tests if the condition is not met and Value is not supplied to SingleValueNotEqualToDependentSelectList.
        /// </summary>
        /// <remarks>
        /// SingleValueNotEqualToDependentSelectList is RequiredIF "NSW" is not selected in BaseSelectList. 
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionNotMetDueToUnexpectedValueAndValueNotSupplied_NotEqualTo_SelectListToSelectList()
        {
            string dependentSelectedValue = "NSW";

            var model = new Model()
            {
                BaseSelectList = new SelectList(States, "Key", "Value", dependentSelectedValue),
                SingleValueNotEqualToDependentSelectList = new SelectList(States, "Key", "Value", null)
            };

            Assert.IsTrue(model.IsValid("SingleValueNotEqualToDependentSelectList"));  // Model must be valid as Control is required ONLY IF value is NOT EQUAL to NSW
        }


        // *** Multiple Values and EQUAL TO ***

        /// <summary>
        /// Validates if the condition is met with expected value and Value is supplied to MultipleValuesDependentSelectList.
        /// </summary>
        /// <remarks>
        /// MultipleValuesDependentSelectList is RequiredIF  "ACT" OR "NSW" is selected in BaseSelectList.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetWithExpectedValueAndValueSupplied__SelectListToSelectList_MultipleDependencies()
        {
            string dependentSelectedValue = "ACT";

            var model = new Model()
            {
                BaseSelectList = new SelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesDependentSelectList = new SelectList(States, "Key", "Value", "SA")
            };

            Assert.IsTrue(model.IsValid("MultipleValuesDependentSelectList"));
        }

        /// <summary>
        /// Tests if the condition is not met and Value is not supplied to MultipleValuesDependentSelectList.
        /// </summary>
        /// <remarks>
        /// MultipleValuesDependentSelectList is RequiredIF  "ACT" OR "NSW" is selected in BaseSelectList.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionNotMetDueToDifferentValueAndValueNotSupplied__SelectListToSelectList_MultipleDependencies()
        {
            string dependentSelectedValue = "SA";

            var model = new Model()
            {
                BaseSelectList = new SelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesDependentSelectList = new SelectList(States, "Key", "Value", null)
            };

            Assert.IsTrue(model.IsValid("MultipleValuesDependentSelectList"));  // Model must be valid as Control is required ONLY IF value is EQUAL to ACT OR NSW.
        }

        /// <summary>
        /// Tests if the condition is met and Value is not supplied to MultipleValuesDependentSelectList. 
        /// </summary>
        /// <remarks>
        /// MultipleValuesDependentSelectList is RequiredIF  "ACT" OR "NSW" is selected in BaseSelectList. Selection: NSW
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetWithSecondValueAndValueNotSupplied__SelectListToSelectList_MultipleDependencies()
        {
            string dependentSelectedValue = "NSW";

            var model = new Model()
            {
                BaseSelectList = new SelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesDependentSelectList = new SelectList(States, "Key", "Value", null)
            };

            Assert.IsFalse(model.IsValid("MultipleValuesDependentSelectList"));  // Model must be invalid as Control is required IF value is EQUAL to ACT OR NSW.
        }


        /// <summary>
        /// Tests if the condition is not met and Value is supplied to MultipleValuesDependentSelectList. 
        /// </summary>
        /// <remarks>
        /// MultipleValuesDependentSelectList is RequiredIF  "ACT" OR "NSW" is selected in BaseSelectList.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetForExpectedValueAndValueSupplied__SelectListToSelectList_MultipleDependencies()
        {
            string dependentSelectedValue = "VIC";

            var model = new Model()
            {
                BaseSelectList = new SelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesDependentSelectList = new SelectList(States, "Key", "Value", "ACT")
            };

            Assert.IsTrue(model.IsValid("MultipleValuesDependentSelectList"));  // Model must be valid.
        }



        /// <summary>
        /// Tests if the condition is not met as value being null and Value is supplied to MultipleValuesDependentSelectList. 
        /// </summary>
        /// <remarks>
        /// MultipleValuesDependentSelectList is RequiredIF  "ACT" OR "NSW" is selected in BaseSelectList. Selection: none.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionNotMetNullValueAndValueSupplied__SelectListToSelectList_MultipleDependencies()
        {
            string dependentSelectedValue = null;

            var model = new Model()
            {
                BaseSelectList = new SelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesDependentSelectList = new SelectList(States, "Key", "Value", "ACT")
            };

            Assert.IsTrue(model.IsValid("MultipleValuesDependentSelectList"));  // Model must be valid.
        }
        


        // *** Multiple Values and NOT EQUAL To ***
        
        /// <summary>
        /// Validates if the condition is met with expected value and Value isn't supplied to MultipleValuesNotEqualToDependentSelectList.
        /// </summary>
        /// <remarks>
        /// MultipleValuesNotEqualToDependentSelectList is RequiredIF "NT" OR "QLD" is not selected in BaseSelectList.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetWithExpectedValueAndValueNotSupplied_NotEqualTo_SelectListToSelectList_MultipleDependencies()
        {
            var dependentSelectedValue = "SA" ;   // Condition is "Required if Not Equal to 'QLD' AND 'NT'"

            var model = new Model()
            {
                BaseSelectList = new SelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesNotEqualToDependentSelectList = new SelectList(States, "Key", "Value", null)
            };

            Assert.IsFalse(model.IsValid("MultipleValuesNotEqualToDependentSelectList")); // Model should be invalid as no selection has been made.
        }

        /// <summary>
        /// Validates if the condition is met with expected value and Value is supplied to MultipleValuesNotEqualToDependentSelectList.
        /// </summary>
        /// <remarks>
        /// MultipleValuesNotEqualToDependentSelectList is RequiredIF "NT" OR "QLD" is not selected in BaseSelectList.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetWithExpectedValueAndValueSupplied_NotEqualTo_SelectListToSelectList_MultipleDependencies()
        {
            string dependentSelectedValue = "ACT";

            var model = new Model()
            {
                BaseSelectList = new SelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesNotEqualToDependentSelectList = new SelectList(States, "Key", "Value", "SA")
            };

            Assert.IsTrue(model.IsValid("MultipleValuesNotEqualToDependentSelectList"));
        }

        /// <summary>
        /// Tests if the condition is not met and Value is not supplied to MultipleValuesNotEqualToDependentSelectList.
        /// </summary>
        /// <remarks>
        /// MultipleValuesNotEqualToDependentSelectList is RequiredIF "NT" OR "QLD" is not selected in BaseSelectList. Selection: NT
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionNotMetDueToDifferentValueAndValueNotSupplied_NotEqualTo_SelectListToSelectList_MultipleDependencies()
        {
            string dependentSelectedValue = "NT";

            var model = new Model()
            {
                BaseSelectList = new SelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesNotEqualToDependentSelectList = new SelectList(States, "Key", "Value", null)
            };

            Assert.IsTrue(model.IsValid("MultipleValuesNotEqualToDependentSelectList"));  // Model must be valid as Control is required ONLY IF value is NOT EQUAL to NT AND QLD
        }

        /// <summary>
        /// Tests if the condition is not met and Value is not supplied to MultipleValuesNotEqualToDependentSelectList. 
        /// </summary>
        /// <remarks>
        /// MultipleValuesNotEqualToDependentSelectList is RequiredIF "NT" OR "QLD" is not selected in BaseSelectList. Selection: QLD
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionNotMetDueToDifferentValueAndValueNotSupplied_NotEqualTo_SelectListToSelectList_MultipleDependencies_2()
        {
            string dependentSelectedValue = "QLD";

            var model = new Model()
            {
                BaseSelectList = new SelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesNotEqualToDependentSelectList = new SelectList(States, "Key", "Value", null)
            };

            Assert.IsTrue(model.IsValid("MultipleValuesNotEqualToDependentSelectList"));  // Model must be valid as Control is required ONLY IF value is NOT EQUAL to NT AND QLD
        }


        /// <summary>
        /// Tests if the condition is met and Value is supplied to MultipleValuesNotEqualToDependentSelectList. 
        /// </summary>
        /// <remarks>
        /// MultipleValuesNotEqualToDependentSelectList is RequiredIF "NT" OR "QLD" is not selected in BaseSelectList. Selection: WA
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetForExpectedValueAndValueSupplied_NotEqualTo_SelectListToSelectList_MultipleDependencies()
        {
            string dependentSelectedValue = "WA";

            var model = new Model()
            {
                BaseSelectList = new SelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesNotEqualToDependentSelectList = new SelectList(States, "Key", "Value", "ACT")
            };

            Assert.IsTrue(model.IsValid("MultipleValuesNotEqualToDependentSelectList"));  // Model must be valid.
        }


        /// <summary>
        /// Tests if the condition is not met due to no selection made and Value is supplied to MultipleValuesNotEqualToDependentSelectList. 
        /// </summary>
        /// <remarks>
        /// MultipleValuesNotEqualToDependentSelectList is RequiredIF "NT" OR "QLD" is not selected in BaseSelectList. Selection: none.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionNotMetNullValueAndValueSupplied_NotEqualTo_SelectListToSelectList_MultipleDependencies()
        {
            string dependentSelectedValue = null;

            var model = new Model()
            {
                BaseSelectList = null, // new SelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesNotEqualToDependentSelectList = new SelectList(States, "Key", "Value", "ACT")
            };

            Assert.IsTrue(model.IsValid("MultipleValuesNotEqualToDependentSelectList"));  // Model must be valid.
        }

        #endregion

        #region Select List   <---->    Multi-Select List Tests

        // *** Single value and EQUAL TO Tests ***

        /// <summary>
        /// Validates if the condition is met with expected value and Value isn't supplied to SingleValueDependentMultiSelect.
        /// </summary>
        /// <remarks>
        /// SingleValueDependentMultiSelect is RequiredIF  BaseSelectList has "ACT" selected. 
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetWithExpectedValueAndValueNotSupplied_SelectListToMultiSelect()
        {
            string dependentSelectedValue = "ACT";

            var model = new Model()
            {
                BaseSelectList = new SelectList(States, "Key", "Value", dependentSelectedValue),
                SingleValueDependentMultiSelect = new MultiSelectList(States, "Key", "Value", null)
            };

            Assert.IsFalse(model.IsValid("SingleValueDependentMultiSelect"));
        }

        /// <summary>
        /// Validates if the condition is met with expected value and Value is supplied to SingleValueDependentMultiSelect.
        /// </summary>
        /// <remarks>
        /// SingleValueDependentMultiSelect is RequiredIF  BaseSelectList has "ACT" selected. 
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetWithExpectedValueAndValueSupplied_SelectListToMultiSelect()
        {
            string dependentSelectedValue = "ACT";

            var actualSelection = new[] {"SA"};

            var model = new Model()
            {
                BaseSelectList = new SelectList(States, "Key", "Value", dependentSelectedValue),
                SingleValueDependentMultiSelect = new MultiSelectList(States, "Key", "Value", actualSelection)
            };

            Assert.IsTrue(model.IsValid("SingleValueDependentMultiSelect"));
        }

        /// <summary>
        /// Tests if the condition is not met due to unexpected value and Value is not supplied to SingleValueDependentMultiSelect.
        /// </summary>
        /// <remarks>
        /// SingleValueDependentMultiSelect is RequiredIF  BaseSelectList has "ACT" selected. 
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionNotMetDueToUnexpectedValueAndValueNotSupplied_SelectListToMultiSelect()
        {
            string dependentSelectedValue = "SA";

            var model = new Model()
            {
                BaseSelectList = new SelectList(States, "Key", "Value", dependentSelectedValue),
                SingleValueDependentMultiSelect = new MultiSelectList(States, "Key", "Value", null)
            };

            Assert.IsTrue(model.IsValid("SingleValueDependentMultiSelect"));
        }


        /// <summary>
        /// Tests if the condition is not met due to null value and Value is not supplied to SingleValueDependentMultiSelect.
        /// </summary>
        /// <remarks>
        /// SingleValueDependentMultiSelect is RequiredIF  BaseSelectList has "ACT" selected. 
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionNotMetDueToNullAndValueNotSupplied_SelectListToMultiSelect()
        {
            string dependentSelectedValue = null;

            var model = new Model()
            {
                BaseSelectList = new SelectList(States, "Key", "Value", dependentSelectedValue),
                SingleValueDependentMultiSelect = new MultiSelectList(States, "Key", "Value", null)
            };

            Assert.IsTrue(model.IsValid("SingleValueDependentMultiSelect")); // Model must be valid as Property is only Required IF Selection is ACT.
        }



        // *** Single Value and Not Equal to Tests ***

        /// <summary>
        /// Validates if the condition is met with expected value and Value isn't supplied to SingleValueNotEqualToDependentMultiSelect.
        /// </summary>
        /// <remarks>
        /// SingleValueNotEqualToDependentMultiSelect is RequiredIF "ACT" is not selected in BaseSelectList.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetWithExpectedValueAndValueNotSupplied_NotEqualTo_SelectListToMultiSelect()
        {
            string dependentSelectedValue = "NSW";   // Condition is "Required if Not Equal to 'ACT'"

            var model = new Model()
            {
                BaseSelectList = new SelectList(States, "Key", "Value", dependentSelectedValue),
                SingleValueNotEqualToDependentMultiSelect = new MultiSelectList(States, "Key", "Value", null)
            };

            Assert.IsFalse(model.IsValid("SingleValueNotEqualToDependentMultiSelect"));
        }

        /// <summary>
        /// Validates if the condition is met with expected value and Value is supplied to SingleValueNotEqualToDependentMultiSelect.
        /// </summary>
        /// <remarks>
        /// SingleValueNotEqualToDependentMultiSelect is RequiredIF "ACT" is not selected in BaseSelectList.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetWithExpectedValueAndValueSupplied_NotEqualTo_SelectListToMultiSelect()
        {
            string dependentSelectedValue = "WA";
            var actualSelection = new[] { "NSW", "SA", "ACT", "QLD", "WA" };

            var model = new Model()
            {
                BaseSelectList = new SelectList(States, "Key", "Value", dependentSelectedValue),
                SingleValueNotEqualToDependentMultiSelect = new MultiSelectList(States, "Key", "Value", actualSelection)
            };

            Assert.IsTrue(model.IsValid("SingleValueNotEqualToDependentMultiSelect"));
        }

        /// <summary>
        /// Tests if the condition is not met and Value is not supplied to SingleValueNotEqualToDependentMultiSelect.
        /// </summary>
        /// <remarks>
        /// SingleValueNotEqualToDependentMultiSelect is RequiredIF "ACT" is not selected in BaseSelectList. 
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionNotMetDueToUnexpectedValueAndValueNotSupplied_NotEqualTo_SelectListToMultiSelect()
        {
            string dependentSelectedValue = "ACT";

            var model = new Model()
            {
                BaseSelectList = new SelectList(States, "Key", "Value", dependentSelectedValue),
                SingleValueNotEqualToDependentMultiSelect = new MultiSelectList(States, "Key", "Value", null)
            };

            Assert.IsTrue(model.IsValid("SingleValueNotEqualToDependentMultiSelect"));  // Model must be valid as Control is required ONLY IF value is NOT EQUAL to ACT
        }


        /// <summary>
        /// Tests if the condition is not met due to property being null and Value is not supplied to SingleValueNotEqualToDependentMultiSelect.
        /// </summary>
        /// <remarks>
        /// SingleValueNotEqualToDependentMultiSelect is RequiredIF "ACT" is not selected in BaseSelectList. Selection: none.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionNotMetDueToNullAndValueNotSupplied_NotEqualTo_SelectListToMultiSelect()
        {
            string dependentSelectedValue = null;

            var model = new Model()
            {
                BaseSelectList = new SelectList(States, "Key", "Value", dependentSelectedValue),
                SingleValueNotEqualToDependentMultiSelect = new MultiSelectList(States, "Key", "Value", null)
            };

            Assert.IsFalse(model.IsValid("SingleValueNotEqualToDependentMultiSelect"));  // Model must be invalid as Control is required  IF value is NOT EQUAL to ACT
        }


        // *** Multiple Values and EQUAL TO ***

        /// <summary>
        /// Validates if the condition is met with expected value and Value is supplied to MultipleValuesDependentMultiSelect.
        /// </summary>
        /// <remarks>
        /// MultipleValuesDependentMultiSelect is RequiredIF  "SA" OR "QLD" is selected in BaseSelectList. Selection: SA.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetWithExpectedValueAndValueSupplied__SelectListToMultiSelect_MultipleDependencies()
        {
            var dependentSelectedValue = new [] {"SA"};   // For comparison between the value of MultiSelect & SelectList, SelectedValue is expected to be in the form of an array.
            var actualSelection = new Dictionary<string, string> { {"NSW", "NSW"}, {"ACT", "ACT"} };

            var model = new Model()
            {
                BaseSelectList = new SelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesDependentMultiSelect = new MultiSelectList(States, "Key", "Value", actualSelection)
            };

            Assert.IsTrue(model.IsValid("MultipleValuesDependentMultiSelect"));
        }

        /// <summary>
        /// Validates if the condition is met with expected (2nd) value and Value is supplied to MultipleValuesDependentMultiSelect.
        /// </summary>
        /// <remarks>
        /// MultipleValuesDependentMultiSelect is RequiredIF  "SA" OR "QLD" is selected in BaseSelectList. Selection: QLD.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetWithSecondValueAndValueSupplied__SelectListToMultiSelect_MultipleDependencies()
        {
            var dependentSelectedValue = new[] { "QLD" };   // For comparison between the value of MultiSelect & SelectList, SelectedValue is expected to be in the form of an array.
            var actualSelection = new Dictionary<string, string> { { "NSW", "NSW" }, { "ACT", "ACT" } };

            var model = new Model()
            {
                BaseSelectList = new SelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesDependentMultiSelect = new MultiSelectList(States, "Key", "Value", actualSelection)
            };

            Assert.IsTrue(model.IsValid("MultipleValuesDependentMultiSelect"));
        }

        /// <summary>
        /// Tests if the condition is not met and Value is not supplied to MultipleValuesDependentMultiSelect.
        /// </summary>
        /// <remarks>
        /// MultipleValuesDependentMultiSelect is RequiredIF  "QLD" OR "SA" is selected in BaseSelectList.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionNotMetDueToDifferentValueAndValueNotSupplied__SelectListToMultiSelect_MultipleDependencies()
        {
            string dependentSelectedValue = "NSW";

            var model = new Model()
            {
                BaseSelectList = new SelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesDependentMultiSelect = new MultiSelectList(States, "Key", "Value", null)
            };

            Assert.IsTrue(model.IsValid("MultipleValuesDependentMultiSelect"));  // Model must be valid as Control is required ONLY IF value is EQUAL to QLD OR SA.
        }

        /// <summary>
        /// Tests if the condition is met and Selection is not made in the MultipleValuesDependentMultiSelect. 
        /// </summary>
        /// <remarks>
        /// MultipleValuesDependentMultiSelect is RequiredIF  "QLD" OR "SA" is selected in BaseSelectList. Selection: ACT
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetWithSecondValueAndValueNotSupplied__SelectListToMultiSelect_MultipleDependencies()
        {
            string dependentSelectedValue = "QLD";

            var model = new Model()
            {
                BaseSelectList = new SelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesDependentMultiSelect = new MultiSelectList(States, "Key", "Value", null)
            };

            Assert.IsFalse(model.IsValid("MultipleValuesDependentMultiSelect"));  // Model must be invalid as Control is required IF value is EQUAL to QLD OR SA.
        }


        /// <summary>
        /// Tests if the condition is not met and Value is supplied to MultipleValuesDependentMultiSelect. 
        /// </summary>
        /// <remarks>
        /// MultipleValuesDependentMultiSelect is RequiredIF  "QLD" OR "SA" is selected in BaseSelectList.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetForExpectedValueAndValueSupplied__SelectListToMultiSelect_MultipleDependencies()
        {
            string dependentSelectedValue = "VIC";
            var actualSelection = new Dictionary<string, string> { { "NSW", "NSW" }, { "ACT", "ACT" } };

            var model = new Model()
            {
                BaseSelectList = new SelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesDependentMultiSelect = new MultiSelectList(States, "Key", "Value", actualSelection)
            };

            Assert.IsTrue(model.IsValid("MultipleValuesDependentMultiSelect"));  // Model must be valid.
        }



        /// <summary>
        /// Tests if the condition is not met due to property being null and Value is supplied to MultipleValuesDependentMultiSelect. 
        /// </summary>
        /// <remarks>
        /// MultipleValuesDependentMultiSelect is RequiredIF  "QLD" OR "SA" is selected in BaseSelectList. Selection: none
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionNotMetNullAndValueSupplied__SelectListToMultiSelect_MultipleDependencies()
        {
            string dependentSelectedValue = null;
            var actualSelection = new Dictionary<string, string> { { "NSW", "NSW" }, { "ACT", "ACT" } };

            var model = new Model()
            {
                BaseSelectList = new SelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesDependentMultiSelect = new MultiSelectList(States, "Key", "Value", actualSelection)
            };

            Assert.IsTrue(model.IsValid("MultipleValuesDependentMultiSelect"));  // Model must be valid.
        }



        // *** Multiple Values and NOT EQUAL To ***

        /// <summary>
        /// Validates if the condition is met with expected value and Value isn't supplied to MultipleValuesNotEqualToDependentMultiSelect.
        /// </summary>
        /// <remarks>
        /// MultipleValuesNotEqualToDependentMultiSelect is RequiredIF "NT" OR "NSW" is not selected in BaseSelectList.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetWithExpectedValueAndValueNotSupplied_NotEqualTo_SelectListToMultiSelect_MultipleDependencies()
        {
            var dependentSelectedValue = "SA";   // Condition is "Required if Not Equal to 'NSW' AND 'NT'"

            var model = new Model()
            {
                BaseSelectList = new SelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesNotEqualToDependentMultiSelect = new MultiSelectList(States, "Key", "Value", null)
            };

            Assert.IsFalse(model.IsValid("MultipleValuesNotEqualToDependentMultiSelect")); // Model should be invalid as no selection has been made.
        }

        /// <summary>
        /// Validates if the condition is met with expected value and Value is supplied to MultipleValuesNotEqualToDependentMultiSelect.
        /// </summary>
        /// <remarks>
        /// MultipleValuesNotEqualToDependentMultiSelect is RequiredIF "NT" OR "NSW" is not selected in BaseSelectList.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetWithExpectedValueAndValueSupplied_NotEqualTo_SelectListToMultiSelect_MultipleDependencies()
        {
            var dependentSelectedValue =  new []{"ACT"}; // comparison requires value in the form of an array. 
            var actualSelection = new[]{  "NSW" };// new Dictionary<string, string> { { "NSW", "NSW" } };//, { "SA", "SA" } 

            var model = new Model()
            {
                BaseSelectList = new SelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesNotEqualToDependentMultiSelect = new MultiSelectList(States, "Key", "Value", actualSelection)
            };

            Assert.IsTrue(model.IsValid("MultipleValuesNotEqualToDependentMultiSelect"));
        }

        /// <summary>
        /// Tests if the condition is not met and Value is not supplied to MultipleValuesNotEqualToDependentMultiSelect.
        /// </summary>
        /// <remarks>
        /// MultipleValuesNotEqualToDependentMultiSelect is RequiredIF "NT" OR "NSW" is not selected in BaseSelectList. Selection: NT
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionNotMetDueToDifferentValueAndValueNotSupplied_NotEqualTo_SelectListToMultiSelect_MultipleDependencies()
        {
            string dependentSelectedValue = "NT";

            var model = new Model()
            {
                BaseSelectList = new SelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesNotEqualToDependentMultiSelect = new MultiSelectList(States, "Key", "Value", null)
            };

            Assert.IsTrue(model.IsValid("MultipleValuesNotEqualToDependentMultiSelect"));  // Model must be valid as Control is required ONLY IF value is NOT EQUAL to NT AND NSW
        }

        /// <summary>
        /// Tests if the condition is not met (2nd value) and Value is not supplied to MultipleValuesNotEqualToDependentMultiSelect. 
        /// </summary>
        /// <remarks>
        /// MultipleValuesNotEqualToDependentMultiSelect is RequiredIF "NT" OR "NSW" is not selected in BaseSelectList. Selection: NSW
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionNotMetDueToDifferentValueAndValueNotSupplied_NotEqualTo_SelectListToMultiSelect_MultipleDependencies_2()
        {
            string dependentSelectedValue = "NSW";

            var model = new Model()
            {
                BaseSelectList = new SelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesNotEqualToDependentMultiSelect = new MultiSelectList(States, "Key", "Value", null)
            };

            Assert.IsTrue(model.IsValid("MultipleValuesNotEqualToDependentMultiSelect"));  // Model must be valid as Control is required ONLY IF value is NOT EQUAL to NT AND NSW
        }


        /// <summary>
        /// Tests if the condition is not met as no selection is made in BaseSelectList and Value is not supplied to MultipleValuesNotEqualToDependentMultiSelect. 
        /// </summary>
        /// <remarks>
        /// MultipleValuesNotEqualToDependentMultiSelect is RequiredIF "NT" OR "NSW" is not selected in BaseSelectList. Selection: none.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionNotMetNullAndValueNotSupplied_NotEqualTo_SelectListToMultiSelect_MultipleDependencies()
        {
            string dependentSelectedValue = string.Empty;

            var model = new Model()
            {
                BaseSelectList =  null,// new SelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesNotEqualToDependentMultiSelect = new MultiSelectList(States, "Key", "Value", null)
            };

            Assert.IsTrue(model.IsValid("MultipleValuesNotEqualToDependentMultiSelect"));  
        }

        #endregion


        #region Multi-Select List   <---->    Multi-Select List Tests

        // *** Single value and EQUAL TO Tests ***

        /// <summary>
        /// Validates if the condition is met with expected value and Value isn't supplied to SingleValueDependentMultiSelect2.
        /// </summary>
        /// <remarks>
        /// SingleValueDependentMultiSelect2 is RequiredIF  BaseSelectList has "ACT" selected. 
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetWithExpectedValueAndValueNotSupplied_MultiSelectToMultiSelect()
        {
            var dependentSelectedValue = new []{"ACT"};

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, "Key", "Value", dependentSelectedValue),
                SingleValueDependentMultiSelect2 = new MultiSelectList(States, "Key", "Value", null)
            };

            Assert.IsFalse(model.IsValid("SingleValueDependentMultiSelect2"));
        }

        /// <summary>
        /// Validates if the condition is met with expected value and Value is supplied to SingleValueDependentMultiSelect2.
        /// </summary>
        /// <remarks>
        /// SingleValueDependentMultiSelect2 is RequiredIF  BaseSelectList has "ACT" selected. 
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetWithExpectedValueAndValueSupplied_MultiSelectToMultiSelect()
        {
            var dependentSelectedValue = new []{"ACT", "WA"};

            var actualSelection = new[] { "SA", "NSW" };

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, dependentSelectedValue),
                SingleValueDependentMultiSelect2 = new MultiSelectList(States, actualSelection)
            };

            Assert.IsTrue(model.IsValid("SingleValueDependentMultiSelect2"));
        }

        /// <summary>
        /// Tests if the condition is met with expected value (along with other values) and Value is not supplied to SingleValueDependentMultiSelect2.
        /// </summary>
        /// <remarks>
        /// SingleValueDependentMultiSelect2 is RequiredIF  BaseSelectList has "ACT" selected. 
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetWithExpectedValuesAndValueNotSupplied_MultiSelectToMultiSelect()
        {
            var dependentSelectedValue = new[] {"NSW", "ACT", "SA"};   //Dictionary<string,string> {  {"ACT", "ACT"}, { "SA", "SA"} }};
            
            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, "Key", "Value", dependentSelectedValue),
                SingleValueDependentMultiSelect2 = null// new MultiSelectList(States, null)
            };

            Assert.IsFalse(model.IsValid("SingleValueDependentMultiSelect2")); // Model must be invalid.
        }


        /// <summary>
        /// Tests if the condition is not met due to unexpected value and Value is not supplied to SingleValueDependentMultiSelect2.
        /// </summary>
        /// <remarks>
        /// SingleValueDependentMultiSelect2 is RequiredIF  BaseSelectList has "ACT" selected. 
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionNotMetDueToUnexpectedValueAndValueNotSupplied_MultiSelectToMultiSelect()
        {
            var dependentSelectedValue = new[] { "SA" , "NSW"};

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, dependentSelectedValue),
                SingleValueDependentMultiSelect2 = new MultiSelectList(States,null)
            };

            Assert.IsTrue(model.IsValid("SingleValueDependentMultiSelect2"));
        }


        /// <summary>
        /// Tests if the condition is not met due to null value and Value is not supplied to SingleValueDependentMultiSelect2.
        /// </summary>
        /// <remarks>
        /// SingleValueDependentMultiSelect2 is RequiredIF  BaseSelectList has "ACT" selected. 
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionNotMetDueToNullAndValueNotSupplied_MultiSelectToMultiSelect()
        {
            string dependentSelectedValue = null;

            var model = new Model()
            {
                BaseMultiSelect = null, //new MultiSelectList(States, "Key", "Value", dependentSelectedValue),
                SingleValueDependentMultiSelect2 = new MultiSelectList(States, "Key", "Value", null)
            };

            Assert.IsTrue(model.IsValid("SingleValueDependentMultiSelect2")); // Model must be valid as Property is only Required IF Selection is ACT.
        }



        // *** Single Value and Not Equal to Tests ***

        /// <summary>
        /// Validates if the condition is met with expected value along with other values and Value isn't supplied to SingleValueNotEqualToDependentMultiSelect2.
        /// </summary>
        /// <remarks>
        /// SingleValueNotEqualToDependentMultiSelect2 is RequiredIF "ACT" is not selected in BaseSelectList.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetWithExpectedValueAndValueNotSupplied_NotEqualTo_MultiSelectToMultiSelect()
        {
            var dependentSelectedValue = new[] { "NSW", "WA", "QLD", "SA", "TAS", "VIC", "NT"};   // Condition is "Required if Not Equal to 'ACT'"

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, "Key", "Value", dependentSelectedValue),
                SingleValueNotEqualToDependentMultiSelect2 = new MultiSelectList(States, "Key", "Value", null)
            };

            Assert.IsFalse(model.IsValid("SingleValueNotEqualToDependentMultiSelect2"));
        }

        /// <summary>
        /// Validates if the condition is met with expected value and Value is supplied to SingleValueNotEqualToDependentMultiSelect2.
        /// </summary>
        /// <remarks>
        /// SingleValueNotEqualToDependentMultiSelect2 is RequiredIF "ACT" is not selected in BaseSelectList.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetWithExpectedValueAndValueSupplied_NotEqualTo_MultiSelectToMultiSelect()
        {
            var dependentSelectedValue =  new []{"WA"};
            var actualSelection = new[] { "NSW", "SA", "ACT", "QLD", "WA" };

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, "Key", "Value", dependentSelectedValue),
                SingleValueNotEqualToDependentMultiSelect2 = new MultiSelectList(States, "Key", "Value", actualSelection)
            };

            Assert.IsTrue(model.IsValid("SingleValueNotEqualToDependentMultiSelect2"));
        }

        /// <summary>
        /// Tests if the condition is not met and Value is not supplied to SingleValueNotEqualToDependentMultiSelect2.
        /// </summary>
        /// <remarks>
        /// SingleValueNotEqualToDependentMultiSelect2 is RequiredIF "ACT" is not selected in BaseSelectList. 
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionNotMetDueToUnexpectedValueAndValueNotSupplied_NotEqualTo_MultiSelectToMultiSelect()
        {
            var dependentSelectedValue = new []{"ACT"};

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, "Key", "Value", dependentSelectedValue),
                SingleValueNotEqualToDependentMultiSelect2 = new MultiSelectList(States, "Key", "Value", null)
            };

            Assert.IsTrue(model.IsValid("SingleValueNotEqualToDependentMultiSelect2"));  // Model must be valid as Control is required ONLY IF value is NOT EQUAL to ACT
        }


        /// <summary>
        /// Tests if the condition is not met due to property being null and Value is not supplied to SingleValueNotEqualToDependentMultiSelect2.
        /// </summary>
        /// <remarks>
        /// SingleValueNotEqualToDependentMultiSelect2 is RequiredIF "ACT" is not selected in BaseSelectList. Selection: none.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionNotMetDueToNullAndValueNotSupplied_NotEqualTo_MultiSelectToMultiSelect()
        {
            string dependentSelectedValue = null;

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, "Key", "Value", dependentSelectedValue),
                SingleValueNotEqualToDependentMultiSelect2 = new MultiSelectList(States, "Key", "Value", null)
            };

            Assert.IsFalse(model.IsValid("SingleValueNotEqualToDependentMultiSelect2"));  // Model must be invalid as Control is required  IF value is NOT EQUAL to ACT unless 'PassOnNull' is set to true.
        }


        // *** Multiple Values and EQUAL TO ***

        /// <summary>
        /// Validates if the condition is met with expected value and Value is supplied to MultipleValuesDependentMultiSelect2.
        /// </summary>
        /// <remarks>
        /// MultipleValuesDependentMultiSelect2 is RequiredIF  "SA" OR "QLD" is selected in BaseSelectList. Selection: SA.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetWithExpectedValueAndValueSupplied__MultiSelectToMultiSelect_MultipleDependencies()
        {
            var dependentSelectedValue = new[] { "SA"};   // For comparison between the value of MultiSelect & SelectList, SelectedValue is expected to be in the form of an array.
            var actualSelection = new[] {"ACT"}; // new Dictionary<string, string> { { "NSW", "NSW" }, { "ACT", "ACT" } };

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesDependentMultiSelect2 = new MultiSelectList(States, "Key", "Value", actualSelection)
            };

            Assert.IsTrue(model.IsValid("MultipleValuesDependentMultiSelect2"));
        }

        /// <summary>
        /// Validates if the condition is met with expected (2nd) value and Value is supplied to MultipleValuesDependentMultiSelect2.
        /// </summary>
        /// <remarks>
        /// MultipleValuesDependentMultiSelect2 is RequiredIF  "SA" OR "QLD" is selected in BaseSelectList. Selection: QLD.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetWithSecondValueAndValueSupplied__MultiSelectToMultiSelect_MultipleDependencies()
        {
            var dependentSelectedValue = new[] { "QLD" };   // For comparison between the value of MultiSelect & SelectList, SelectedValue is expected to be in the form of an array.
            var actualSelection = new[] {"ACT"};// new Dictionary<string, string> { { "NSW", "NSW" }, { "ACT", "ACT" } };

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesDependentMultiSelect2 = new MultiSelectList(States, "Key", "Value", actualSelection)
            };

            Assert.IsTrue(model.IsValid("MultipleValuesDependentMultiSelect2"));
        }


        /// <summary>
        /// Validates if the condition is met with expected value along with other values and Value is supplied to MultipleValuesDependentMultiSelect2.
        /// </summary>
        /// <remarks>
        /// MultipleValuesDependentMultiSelect2 is RequiredIF  "SA" OR "QLD" is selected in BaseSelectList. Selection: QLD.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetWithOtherValuesAndValueSupplied__MultiSelectToMultiSelect_MultipleDependencies()
        {
            var dependentSelectedValue = new[] { "QLD", "SA", "NSW", "ACT" };   // For comparison between the value of MultiSelect & SelectList, SelectedValue is expected to be in the form of an array.
            var actualSelection = new[] { "ACT" };// new Dictionary<string, string> { { "NSW", "NSW" }, { "ACT", "ACT" } };

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesDependentMultiSelect2 = new MultiSelectList(States, "Key", "Value", actualSelection)
            };

            Assert.IsTrue(model.IsValid("MultipleValuesDependentMultiSelect2"));
        }


        /// <summary>
        /// Validates if the condition is met with expected value along with other values and Value is not supplied to MultipleValuesDependentMultiSelect2.
        /// </summary>
        /// <remarks>
        /// MultipleValuesDependentMultiSelect2 is RequiredIF  "SA" OR "QLD" is selected in BaseSelectList. Selection: QLD, SA, NSW, ACT.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetWithOtherValuesAndValueNotSupplied__MultiSelectToMultiSelect_MultipleDependencies()
        {
            var dependentSelectedValue = new[] { "QLD", "SA", "NSW", "ACT" };   // For comparison between the value of MultiSelect & SelectList, SelectedValue is expected to be in the form of an array.
            var actualSelection = new[] { "ACT" };// new Dictionary<string, string> { { "NSW", "NSW" }, { "ACT", "ACT" } };

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesDependentMultiSelect2 = new MultiSelectList(States, "Key", "Value", null)
            };

            Assert.IsFalse(model.IsValid("MultipleValuesDependentMultiSelect2")); // Model must be invalid as the condition has been met.
        }

        /// <summary>
        /// Tests if the condition is not met and Value is not supplied to MultipleValuesDependentMultiSelect2.
        /// </summary>
        /// <remarks>
        /// MultipleValuesDependentMultiSelect2 is RequiredIF  "QLD" OR "SA" is selected in BaseSelectList.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionNotMetDueToDifferentValueAndValueNotSupplied__MultiSelectToMultiSelect_MultipleDependencies()
        {
            var dependentSelectedValue = new []{"NSW"};

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesDependentMultiSelect2 = new MultiSelectList(States, "Key", "Value", null)
            };

            Assert.IsTrue(model.IsValid("MultipleValuesDependentMultiSelect2"));  // Model must be valid as Control is required ONLY IF value is EQUAL to QLD OR SA.
        }

        /// <summary>
        /// Tests if the condition is met and Selection is not made in the MultipleValuesDependentMultiSelect2. 
        /// </summary>
        /// <remarks>
        /// MultipleValuesDependentMultiSelect2 is RequiredIF  "QLD" OR "SA" is selected in BaseSelectList. Selection: QLD
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetWithSecondValueAndValueNotSupplied__MultiSelectToMultiSelect_MultipleDependencies()
        {
            var dependentSelectedValue = new [] {"QLD"};

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesDependentMultiSelect2 = new MultiSelectList(States, "Key", "Value", null)
            };

            Assert.IsFalse(model.IsValid("MultipleValuesDependentMultiSelect2"));  // Model must be invalid as Control is required IF value is EQUAL to QLD OR SA.
        }


        /// <summary>
        /// Tests if the condition is not met and Value is supplied to MultipleValuesDependentMultiSelect2. 
        /// </summary>
        /// <remarks>
        /// MultipleValuesDependentMultiSelect2 is RequiredIF  "QLD" OR "SA" is selected in BaseSelectList.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetForExpectedValueAndValueSupplied__MultiSelectToMultiSelect_MultipleDependencies()
        {
            var dependentSelectedValue = new[] {"VIC", "ACT"};
            var actualSelection = new[] {"NSW", "ACT"};// new Dictionary<string, string> { { "NSW", "NSW" }, { "ACT", "ACT" } };

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesDependentMultiSelect2 = new MultiSelectList(States, "Key", "Value", actualSelection)
            };

            Assert.IsTrue(model.IsValid("MultipleValuesDependentMultiSelect2"));  // Model must be valid.
        }



        /// <summary>
        /// Tests if the condition is not met due to property being null and Value is supplied to MultipleValuesDependentMultiSelect2. 
        /// </summary>
        /// <remarks>
        /// MultipleValuesDependentMultiSelect2 is RequiredIF  "QLD" OR "SA" is selected in BaseSelectList. Selection: none
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionNotMetNullAndValueSupplied__MultiSelectToMultiSelect_MultipleDependencies()
        {
            string dependentSelectedValue = null;
            var actualSelection = new Dictionary<string, string> { { "NSW", "NSW" }, { "ACT", "ACT" } };

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesDependentMultiSelect2 = new MultiSelectList(States, "Key", "Value", actualSelection)
            };

            Assert.IsTrue(model.IsValid("MultipleValuesDependentMultiSelect2"));  // Model must be valid.
        }


        /// <summary>
        /// Tests if the condition is not met due to property being null and Value is not supplied to MultipleValuesDependentMultiSelect2. 
        /// </summary>
        /// <remarks>
        /// MultipleValuesDependentMultiSelect2 is RequiredIF  "QLD" OR "SA" is selected in BaseSelectList. Selection: none
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionNotMetNullAndValueNotSupplied__MultiSelectToMultiSelect_MultipleDependencies()
        {
            string dependentSelectedValue = null;
            

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesDependentMultiSelect2 = new MultiSelectList(States, "Key", "Value", null)
            };

            Assert.IsTrue(model.IsValid("MultipleValuesDependentMultiSelect2"));  // Model must be valid.
        }



        // *** Multiple Values and NOT EQUAL To ***

        /// <summary>
        /// Validates if the condition is met with expected value and Value isn't supplied to MultipleValuesNotEqualToDependentMultiSelect2.
        /// </summary>
        /// <remarks>
        /// MultipleValuesNotEqualToDependentMultiSelect2 is RequiredIF "NT" OR "NSW" is not selected in BaseSelectList.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetWithExpectedValueAndValueNotSupplied_NotEqualTo_MultiSelectToMultiSelect_MultipleDependencies()
        {
            var dependentSelectedValue = new [] {"SA", "ACT", "QLD"};   // Condition is "Required if Not Equal to 'NSW' AND 'NT'"

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesNotEqualToDependentMultiSelect2 = new MultiSelectList(States, "Key", "Value", null)
            };

            Assert.IsFalse(model.IsValid("MultipleValuesNotEqualToDependentMultiSelect2")); // Model must be invalid.
        }

        /// <summary>
        /// Validates if the condition is met with expected value and Value is supplied to MultipleValuesNotEqualToDependentMultiSelect2.
        /// </summary>
        /// <remarks>
        /// MultipleValuesNotEqualToDependentMultiSelect2 is RequiredIF "NT" OR "NSW" is not selected in BaseSelectList.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetWithExpectedValueAndValueSupplied_NotEqualTo_MultiSelectToMultiSelect_MultipleDependencies()
        {
            var dependentSelectedValue = new[] { "ACT" }; // comparison requires value in the form of an array. 
            var actualSelection = new[] { "NSW"};// new Dictionary<string, string> { { "NSW", "NSW" } };//, { "SA", "SA" } 

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesNotEqualToDependentMultiSelect2 = new MultiSelectList(States, "Key", "Value", actualSelection)
            };

            Assert.IsTrue(model.IsValid("MultipleValuesNotEqualToDependentMultiSelect2"));
        }

        /// <summary>
        /// Tests if the condition is not met and Value is not supplied to MultipleValuesNotEqualToDependentMultiSelect2.
        /// </summary>
        /// <remarks>
        /// MultipleValuesNotEqualToDependentMultiSelect2 is RequiredIF "NT" OR "NSW" is not selected in BaseSelectList. Selection:  "SA", "NSW", "NT", "ACT", "QLD".
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionNotMetDueToDifferentValueAndValueNotSupplied_NotEqualTo_MultiSelectToMultiSelect_MultipleDependencies()
        {
            var dependentSelectedValue = new[] { "SA", "NSW", "NT", "ACT", "QLD" };

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesNotEqualToDependentMultiSelect2 = new MultiSelectList(States, "Key", "Value", null)
            };

            Assert.IsTrue(model.IsValid("MultipleValuesNotEqualToDependentMultiSelect2"));  // Model must be valid as Control is required ONLY IF value is NOT EQUAL to NT AND NSW
        }

        /// <summary>
        /// Tests if the condition is not met (Contains only 'NSW') and Value is not supplied to MultipleValuesNotEqualToDependentMultiSelect2. 
        /// </summary>
        /// <remarks>
        /// MultipleValuesNotEqualToDependentMultiSelect2 is RequiredIF "NT" OR "NSW" is not selected in BaseSelectList. Selection: "SA", "NSW", "WA", "ACT", "QLD".
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionNotMetDueToDifferentValueAndValueNotSupplied_NotEqualTo_MultiSelectToMultiSelect_MultipleDependencies_2()
        {
            var dependentSelectedValue = new[] { "SA", "NSW", "WA", "ACT", "QLD" };

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesNotEqualToDependentMultiSelect2 = new MultiSelectList(States, "Key", "Value", null)
            };

            Assert.IsTrue(model.IsValid("MultipleValuesNotEqualToDependentMultiSelect2"));  // Model must be valid as Control is required ONLY IF value is NOT EQUAL to NT AND NSW
        }


        /// <summary>
        /// Tests if the condition is not met as no selection is made in BaseSelectList and Value is not supplied to MultipleValuesNotEqualToDependentMultiSelect2. 
        /// </summary>
        /// <remarks>
        /// MultipleValuesNotEqualToDependentMultiSelect2 is RequiredIF "NT" OR "NSW" is not selected in BaseSelectList. Selection: none.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionNotMetNullAndValueNotSupplied_NotEqualTo_MultiSelectToMultiSelect_MultipleDependencies()
        {
            

            var model = new Model()
            {
                BaseSelectList = null,// new SelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesNotEqualToDependentMultiSelect2 = new MultiSelectList(States, "Key", "Value", null)
            };

            Assert.IsFalse(model.IsValid("MultipleValuesNotEqualToDependentMultiSelect2")); // model must be invalid as no selection is made but condition has still been met (null != NT and null != NSW), hence property must be mandatory.
        }

        #endregion


        #region Multi-Select List   <---->    Select List Tests

        // *** Single value and EQUAL TO Tests ***

        /// <summary>
        /// Validates if the condition is met with expected value and Value isn't supplied to SingleValueDependentSelectList2.
        /// </summary>
        /// <remarks>
        /// SingleValueDependentSelectList2 is RequiredIF  BaseMultiSelect has "ACT" selected. 
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetWithExpectedValueAndValueNotSupplied_MultiSelectToSelectList()
        {
            var dependentSelectedValue = new []{"ACT", "NSW", "NT"};

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, "Key", "Value", dependentSelectedValue),
                SingleValueDependentSelectList2 = new SelectList(States, "Key", "Value", null)
            };

            Assert.IsFalse(model.IsValid("SingleValueDependentSelectList2")); 
        }

        /// <summary>
        /// Validates if the condition is met with expected value and Value is supplied to SingleValueDependentSelectList2.
        /// </summary>
        /// <remarks>
        /// SingleValueDependentSelectList2 is RequiredIF  BaseMultiSelect has "ACT" selected. 
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetWithExpectedValueAndValueSupplied_MultiSelectToSelectList()
        {
            var dependentSelectedValue = new[] { "ACT", "NSW", "NT" };

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, "Key", "Value", dependentSelectedValue),
                SingleValueDependentSelectList2 = new SelectList(States, "Key", "Value", "SA")
            };

            Assert.IsTrue(model.IsValid("SingleValueDependentSelectList2"));
        }

        /// <summary>
        /// Tests if the condition is met with unexpected value and Value is not supplied to SingleValueDependentSelectList2.
        /// </summary>
        /// <remarks>
        /// SingleValueDependentSelectList2 is RequiredIF  BaseMultiSelect has "ACT" selected. 
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionNotMetDueToUnexpectedValueAndValueNotSupplied_MultiSelectToSelectList()
        {
            var dependentSelectedValue = new[] { "SA", "NSW", "NT" };

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, "Key", "Value", dependentSelectedValue),
                SingleValueDependentSelectList2 = new SelectList(States, "Key", "Value", null)
            };

            Assert.IsTrue(model.IsValid("SingleValueDependentSelectList2"));
        }



        // *** Single Value and Not Equal to Tests ***

        /// <summary>
        /// Validates if the condition is met with expected value and Value isn't supplied to SingleValueNotEqualToDependentSelectList2.
        /// </summary>
        /// <remarks>
        /// SingleValueNotEqualToDependentSelectList2 is RequiredIF "ACT" is not selected in BaseMultiSelect.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetWithExpectedValueAndValueNotSupplied_NotEqualTo_MultiSelectToSelectList()
        {
            var dependentSelectedValue = new[] { "WA", "NSW", "NT" };   // Condition is "Required if Not Equal to 'NSW'"

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, "Key", "Value", dependentSelectedValue),
                SingleValueNotEqualToDependentSelectList2 = new SelectList(States, "Key", "Value", null)
            };

            Assert.IsFalse(model.IsValid("SingleValueNotEqualToDependentSelectList2"));
        }

        /// <summary>
        /// Validates if the condition is met with expected value and Value is supplied to SingleValueNotEqualToDependentSelectList2.
        /// </summary>
        /// <remarks>
        /// SingleValueNotEqualToDependentSelectList2 is RequiredIF "ACT" is not selected in BaseMultiSelect.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetWithExpectedValueAndValueSupplied_NotEqualTo_MultiSelectToSelectList()
        {
            var dependentSelectedValue = new[] { "NSW", "NT", "WA", "QLD", "VIC", "TAS" };

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, "Key", "Value", dependentSelectedValue),
                SingleValueNotEqualToDependentSelectList2 = new SelectList(States, "Key", "Value", "SA")
            };

            Assert.IsTrue(model.IsValid("SingleValueNotEqualToDependentSelectList2"));
        }

        /// <summary>
        /// Tests if the condition is not met and Value is not supplied to SingleValueNotEqualToDependentSelectList2.
        /// </summary>
        /// <remarks>
        /// SingleValueNotEqualToDependentSelectList2 is RequiredIF "ACT" is not selected in BaseMultiSelect. 
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionNotMetDueToUnexpectedValueAndValueNotSupplied_NotEqualTo_MultiSelectToSelectList()
        {
            var dependentSelectedValue = new[] { "ACT", "NT" };

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, "Key",  "Value", dependentSelectedValue),
                SingleValueNotEqualToDependentSelectList2 = new SelectList(States, string.Empty)
            };

            Assert.IsTrue(model.IsValid("SingleValueNotEqualToDependentSelectList2"));  // Model must be valid as Control is required ONLY IF value is NOT EQUAL to ACT
        }


        /// <summary>
        /// Tests if the condition is not met due to null value and Value is not supplied to SingleValueNotEqualToDependentSelectList2.
        /// </summary>
        /// <remarks>
        /// SingleValueNotEqualToDependentSelectList2 is RequiredIF "ACT" is not selected in BaseMultiSelect. 
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionNotMetDueToNullValueAndValueNotSupplied_NotEqualTo_MultiSelectToSelectList()
        {
            
            var model = new Model()
            {
                BaseMultiSelect = null , //new MultiSelectList(States, "Key", "Value", dependentSelectedValue),
                SingleValueNotEqualToDependentSelectList2 = new SelectList(States, "Key", "Value", null)
            };
            // Model must be invalid as Control is REQUIRED  IF value is NOT EQUAL to ACT (null != ACT, hence Condition MET) unless PassOnNull is set to true.
            Assert.IsFalse(model.IsValid("SingleValueNotEqualToDependentSelectList2"));  
        }


        // *** Multiple Values and EQUAL TO ***

        /// <summary>
        /// Validates if the condition is met with expected value and Value is supplied to MultipleValuesDependentSelectList2.
        /// </summary>
        /// <remarks>
        /// MultipleValuesDependentSelectList2 is RequiredIF  "SA" OR "QLD" is selected in BaseMultiSelect.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetWithExpectedValueAndValueSupplied__MultiSelectToSelectList_MultipleDependencies()
        {
            var dependentSelectedValue = new []{"SA"};

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesDependentSelectList2 = new SelectList(States, "Key", "Value", "SA")
            };

            Assert.IsTrue(model.IsValid("MultipleValuesDependentSelectList2"));
        }

        /// <summary>
        /// Tests if the condition is met and Value is not supplied to MultipleValuesDependentSelectList2. 
        /// </summary>
        /// <remarks>
        /// MultipleValuesDependentSelectList2 is RequiredIF  "SA" OR "QLD" is selected in BaseMultiSelect. Selection: NSW
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetWithSecondValueAndValueNotSupplied__MultiSelectToSelectList_MultipleDependencies()
        {
            var dependentSelectedValue = new[] { "QLD", "NSW", "WA" };

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesDependentSelectList2 = new SelectList(States, "Key", "Value", null)
            };

            Assert.IsFalse(model.IsValid("MultipleValuesDependentSelectList2"));  // Model must be invalid as Control is required IF value is EQUAL to ACT OR NSW.
        }

        /// <summary>
        /// Tests if the condition is met with both required values and Value is not supplied to MultipleValuesDependentSelectList2. 
        /// </summary>
        /// <remarks>
        /// MultipleValuesDependentSelectList2 is RequiredIF  "SA" OR "QLD" is selected in BaseMultiSelect. Selection: NSW
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetWithAllValuesAndValueNotSupplied__MultiSelectToSelectList_MultipleDependencies()
        {
            var dependentSelectedValue = new[] { "SA", "QLD", "NSW", "WA", "ACT" };

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesDependentSelectList2 = new SelectList(States, "Key", "Value", null)
            };

            Assert.IsFalse(model.IsValid("MultipleValuesDependentSelectList2"));  // Model must be invalid as Control is required IF value is EQUAL to ACT OR NSW.
        }


        /// <summary>
        /// Tests if the condition is not met and Value is supplied to MultipleValuesDependentSelectList2. 
        /// </summary>
        /// <remarks>
        /// MultipleValuesDependentSelectList2 is RequiredIF  "SA" OR "QLD" is selected in BaseMultiSelect.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetForExpectedValueAndValueSupplied__MultiSelectToSelectList_MultipleDependencies()
        {
            var dependentSelectedValue = new[] { "ACT", "NT", "NSW", "WA" };

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesDependentSelectList2 = new SelectList(States, "Key", "Value", "ACT")
            };

            Assert.IsTrue(model.IsValid("MultipleValuesDependentSelectList2"));  // Model must be valid.
        }

        /// <summary>
        /// Tests if the condition is not met and Value is not supplied to MultipleValuesDependentSelectList2.
        /// </summary>
        /// <remarks>
        /// MultipleValuesDependentSelectList2 is RequiredIF  "SA" OR "QLD" is selected in BaseMultiSelect.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionNotMetDueToDifferentValueAndValueNotSupplied__MultiSelectToSelectList_MultipleDependencies()
        {
            var dependentSelectedValue = new[] { "WA" , "NSW"};

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesDependentSelectList2 = new SelectList(States, "Key", "Value", null)
            };

            Assert.IsTrue(model.IsValid("MultipleValuesDependentSelectList2"));  // Model must be valid as Control is required ONLY IF value is EQUAL to ACT OR NSW.
        } 


        /// <summary>
        /// Tests if the condition is not met as value being null and Value is supplied to MultipleValuesDependentSelectList2. 
        /// </summary>
        /// <remarks>
        /// MultipleValuesDependentSelectList2 is RequiredIF  "SA" OR "QLD" is selected in BaseMultiSelect. Selection: none.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionNotMetNullValueAndValueSupplied__MultiSelectToSelectList_MultipleDependencies()
        {
            string dependentSelectedValue = null;

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesDependentSelectList2 = new SelectList(States, "Key", "Value", "ACT")
            };

            Assert.IsTrue(model.IsValid("MultipleValuesDependentSelectList2"));  // Model must be valid.
        }


        /// <summary>
        /// Tests if the condition is not met as value being null and Value is not supplied to MultipleValuesDependentSelectList2. 
        /// </summary>
        /// <remarks>
        /// MultipleValuesDependentSelectList2 is RequiredIF  "SA" OR "QLD" is selected in BaseMultiSelect. Selection: none.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionNotMetNullValueAndValueNotSupplied__MultiSelectToSelectList_MultipleDependencies()
        {
            string dependentSelectedValue = null;

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesDependentSelectList2 = new SelectList(States, "Key", "Value", null)
            };

            Assert.IsTrue(model.IsValid("MultipleValuesDependentSelectList2"));  // Model must be valid.
        }



        // *** Multiple Values and NOT EQUAL To ***

        /// <summary>
        /// Validates if the condition is met with expected value and Value isn't supplied to MultipleValuesNotEqualToDependentSelectList2.
        /// </summary>
        /// <remarks>
        /// MultipleValuesNotEqualToDependentSelectList2 is RequiredIF "NT" OR "NSW" is not selected in BaseMultiSelect.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetWithExpectedValueAndValueNotSupplied_NotEqualTo_MultiSelectToSelectList_MultipleDependencies()
        {
            var dependentSelectedValue = "SA";   // Condition is "Required if Not Equal to 'NSW' AND 'NT'"

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesNotEqualToDependentSelectList2 = new SelectList(States, "Key", "Value", null)
            };

            Assert.IsFalse(model.IsValid("MultipleValuesNotEqualToDependentSelectList2")); // Model should be invalid as no selection has been made.
        }

        /// <summary>
        /// Validates if the condition is met with expected value and Value is supplied to MultipleValuesNotEqualToDependentSelectList2.
        /// </summary>
        /// <remarks>
        /// MultipleValuesNotEqualToDependentSelectList2 is RequiredIF "NT" OR "NSW" is not selected in BaseMultiSelect.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetWithExpectedValueAndValueSupplied_NotEqualTo_MultiSelectToSelectList_MultipleDependencies()
        {
            string dependentSelectedValue = "ACT";

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesNotEqualToDependentSelectList2 = new SelectList(States, "Key", "Value", "SA")
            };

            Assert.IsTrue(model.IsValid("MultipleValuesNotEqualToDependentSelectList2"));
        }

        /// <summary>
        /// Tests if the condition is not met and Value is not supplied to MultipleValuesNotEqualToDependentSelectList2.
        /// </summary>
        /// <remarks>
        /// MultipleValuesNotEqualToDependentSelectList2 is RequiredIF "NT" OR "NSW" is not selected in BaseMultiSelect. Selection: NT, ACT.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionNotMetDueToDifferentValueAndValueNotSupplied_NotEqualTo_MultiSelectToSelectList_MultipleDependencies()
        {
            var dependentSelectedValue = new []{"NT", "ACT"};

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesNotEqualToDependentSelectList2 = new SelectList(States, "Key", "Value", null)
            };

            Assert.IsTrue(model.IsValid("MultipleValuesNotEqualToDependentSelectList2"));  // Model must be valid as Control is required ONLY IF value is NOT EQUAL to NT AND NSW
        }

        /// <summary>
        /// Tests if the condition is not met and Value is not supplied to MultipleValuesNotEqualToDependentSelectList2. 
        /// </summary>
        /// <remarks>
        /// MultipleValuesNotEqualToDependentSelectList2 is RequiredIF "NT" OR "NSW" is not selected in BaseMultiSelect. Selection: NSW
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionNotMetDueToDifferentValueAndValueNotSupplied_NotEqualTo_MultiSelectToSelectList_MultipleDependencies_2()
        {
            var dependentSelectedValue = new [] {"NSW"};

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesNotEqualToDependentSelectList2 = new SelectList(States, "Key", "Value", null)
            };

            Assert.IsTrue(model.IsValid("MultipleValuesNotEqualToDependentSelectList2"));  // Model must be valid as Control is required ONLY IF value is NOT EQUAL to NT AND NSW
        }


        /// <summary>
        /// Tests if the condition is met and Value is NOT supplied to MultipleValuesNotEqualToDependentSelectList2. 
        /// </summary>
        /// <remarks>
        /// MultipleValuesNotEqualToDependentSelectList2 is RequiredIF "NT" OR "NSW" is not selected in BaseMultiSelect. Selection: WA
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetForExpectedValueAndValueNotSupplied_NotEqualTo_MultiSelectToSelectList_MultipleDependencies()
        {
            var dependentSelectedValue = new [] {"WA", "ACT", "QLD", "SA", "VIC", "TAS"};

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States,dependentSelectedValue),
                MultipleValuesNotEqualToDependentSelectList2 = new SelectList(States, null)
            };

            Assert.IsFalse(model.IsValid("MultipleValuesNotEqualToDependentSelectList2"));  // Model must be invalid as condition is met.
        }

        /// <summary>
        /// Tests if the condition is met and Value is supplied to MultipleValuesNotEqualToDependentSelectList2. 
        /// </summary>
        /// <remarks>
        /// MultipleValuesNotEqualToDependentSelectList2 is RequiredIF "NT" OR "NSW" is not selected in BaseMultiSelect. Selection: WA
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetForExpectedValueAndValueSupplied_NotEqualTo_MultiSelectToSelectList_MultipleDependencies()
        {
            var dependentSelectedValue = new[] { "WA", "ACT", "QLD", "SA", "VIC", "TAS" };

            var model = new Model()
            {
                BaseMultiSelect = new MultiSelectList(States, dependentSelectedValue),
                MultipleValuesNotEqualToDependentSelectList2 = new SelectList(States, string.Empty)
            };

            Assert.IsFalse(model.IsValid("MultipleValuesNotEqualToDependentSelectList2"));  // Model must be invalid as condition is met.
        }


        /// <summary>
        /// Tests if the condition is not met due to no selection made and Value is supplied to MultipleValuesNotEqualToDependentSelectList2. 
        /// </summary>
        /// <remarks>
        /// MultipleValuesNotEqualToDependentSelectList2 is RequiredIF "NT" OR "NSW" is not selected in BaseMultiSelect. Selection: none.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionNotMetNullValueAndValueSupplied_NotEqualTo_MultiSelectToSelectList_MultipleDependencies()
        {
            string dependentSelectedValue = null;

            var model = new Model()
            {
                BaseMultiSelect = null, // new SelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesNotEqualToDependentSelectList2 = new SelectList(States, "Key", "Value", "ACT")
            };

            Assert.IsTrue(model.IsValid("MultipleValuesNotEqualToDependentSelectList2"));  // Model must be valid due to provision of value.
        }

        /// <summary>
        /// Tests if the condition is not met due to no selection made and Value isn't supplied to MultipleValuesNotEqualToDependentSelectList2. 
        /// </summary>
        /// <remarks>
        /// MultipleValuesNotEqualToDependentSelectList2 is RequiredIF "NT" OR "NSW" is not selected in BaseMultiSelect. Selection: none.
        /// </remarks>
        [TestMethod]
        public void RequiredIf_ConditionMetNullValueAndValueNotSupplied_NotEqualTo_MultiSelectToSelectList_MultipleDependencies()
        {
            string dependentSelectedValue = null;

            var model = new Model()
            {
                BaseMultiSelect = null, // new SelectList(States, "Key", "Value", dependentSelectedValue),
                MultipleValuesNotEqualToDependentSelectList2 = new SelectList(States, "Key", "Value", null)
            };

            Assert.IsFalse(model.IsValid("MultipleValuesNotEqualToDependentSelectList2"));  // Model must be invalid as Condition is Met (Null != NT and NSW) unless PassOnNull is set to true.
        }

        #endregion








































        /// <summary>
        /// Test property is required and fails if condition is met with the expected value and a value is not supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIf_DateTimeConditionMetWithOneOfTheExpectedValuesAndNullSupplied_RequiredAndFails()
        {
            var list = new SelectList(new Dictionary<string, string> { { "hello", "hello" }, { "world", "world" }, { "goodbye", "goodbye" } }, "Key", "Value", "world");

            var model = new Model() { Value15 = list, Value14 = null };
            Assert.IsFalse(model.IsValid("Value14"));
        }

        /// <summary>
        /// Test property is not required and validates if condition is met with a different value.
        /// </summary>
        [TestMethod]
        public void RequiredIf_DateTimeConditionMetWithDifferentValueToOneOfTheExpectedVales_NotRequiredAndValidates()
        {
            var list = new SelectList(new Dictionary<string, string> { { "hello", "hello" }, { "world", "world" }, { "goodbye", "goodbye" } }, "Key", "Value", "goodbye");

            var model = new Model() { Value15 = list };
            Assert.IsTrue(model.IsValid("Value14"));
        }

        /// <summary>
        /// Test property is not required and validates if condition is met with an empty string.
        /// </summary>
        [TestMethod]
        public void RequiredIf_DateTimeConditionMetWithEmptyStringInsteadOfOneOfTheExpectedValesWithValueSupplied_NotRequiredAndValidates()
        {
            var list = new SelectList(new Dictionary<string, string> { { "hello", "hello" }, { "world", "world" }, { "goodbye", "goodbye" } }, "Key", "Value");

            var model = new Model() { Value15 = list, Value14 = DateTime.Now };
            Assert.IsTrue(model.IsValid("Value14"));
        }

        /// <summary>
        /// Test property is not required and validates if condition is met with an empty string.
        /// </summary>
        [TestMethod]
        public void RequiredIf_DateTimeConditionMetWithEmptyStringInsteadOfOneOfTheExpectedVales_NotRequiredAndValidates()
        {
            var list = new SelectList(new Dictionary<string, string> { { "hello", "hello" }, { "world", "world" }, { "goodbye", "goodbye" } }, "Key", "Value", "goodbye");

            var model = new Model() { Value15 = list };
            Assert.IsTrue(model.IsValid("Value14"));
        }

        /// <summary>
        /// Test property is not required and validates if condition is met with null.
        /// </summary>
        [TestMethod]
        public void RequiredIf_DateTimeConditionMetWithNullInsteadOfOneOfTheExpectedValesWithValueSupplied_NotRequiredAndValidates()
        {
            var model = new Model() { Value15 = null, Value14 = DateTime.Now};
            Assert.IsTrue(model.IsValid("Value14"));
        }

        /// <summary>
        /// Test property is not required and validates if condition is met with null.
        /// </summary>
        [TestMethod]
        public void RequiredIf_DateTimeConditionMetWithNullInsteadOfOneOfTheExpectedVales_NotRequiredAndValidates()
        {
            var model = new Model() { Value15 = null };
            Assert.IsTrue(model.IsValid("Value14"));
        }


        /// <summary>
        /// Test property is required and validates if condition is met with the expected value and a value is supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIf_MultiSelectListConditionMetWithOneOfTheExpectedValuesAndValueSupplied_RequiredAndValidates()
        {
            var list = new MultiSelectList(new Dictionary<string, string> { { "hello", "hello" }, { "world", "world" }, { "goodbye", "goodbye" } }, "Key", "Value", new [] { "world" });

            var model = new Model() { Value17 = list, Value16 = DateTime.Now };
            Assert.IsTrue(model.IsValid("Value16"));
        }

        /// <summary>
        /// Test property is required and fails if condition is met with the expected value and a value is not supplied on the now required property.
        /// </summary>
        [TestMethod]
        public void RequiredIf_MultiSelectListConditionMetWithOneOfTheExpectedValuesAndNullSupplied_RequiredAndFails()
        {
            var list = new MultiSelectList(new Dictionary<string, string> { { "hello", "hello" }, { "world", "world" }, { "goodbye", "goodbye" } }, "Key", "Value", new[] { "world" });

            var model = new Model() { Value17 = list, Value16 = null };
            Assert.IsFalse(model.IsValid("Value16"));
        }

        /// <summary>
        /// Test property is not required and validates if condition is met with a different value.
        /// </summary>
        [TestMethod]
        public void RequiredIf_MultiSelectListConditionMetWithDifferentValueToOneOfTheExpectedVales_NotRequiredAndValidates()
        {
            var list = new MultiSelectList(new Dictionary<string, string> { { "hello", "hello" }, { "world", "world" }, { "goodbye", "goodbye" } }, "Key", "Value", new[] { "goodbye" });

            var model = new Model() { Value17 = list };
            Assert.IsTrue(model.IsValid("Value16"));
        }


        


    }
}
