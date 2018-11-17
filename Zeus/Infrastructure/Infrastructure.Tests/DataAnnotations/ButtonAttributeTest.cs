using System;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="ButtonAttribute" />.
    /// </summary>
    [TestClass]
    public class ButtonAttributeTest
    {
        /// <summary>
        /// Test throws exception if null name supplied.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Button_NullArgument_ThrowsArgumentNullException()
        {
            new ButtonAttribute(null, null);
        }

        /// <summary>
        /// Test throws exception if empty name supplied.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Button_EmptyArgument_ThrowsArgumentNullException()
        {
            new ButtonAttribute(string.Empty, string.Empty);
        }

        /// <summary>
        /// Test returns maximum int value for order if not specifically set.
        /// </summary>
        [TestMethod]
        public void Button_GetOrderNotSetDefaultsToIntMaxValue_ReturnsDefaultOrderValue()
        {
            var systemUnderTest = new ButtonAttribute("ButtonName","foo");

            var order = systemUnderTest.Order;

            Assert.IsTrue(order == int.MaxValue);
        }

        /// <summary>
        /// Test returns maximum int value for order if not specifically set.
        /// </summary>
        [TestMethod]
        public void Button_GetOrderSetValue_ReturnsSetOrderValue()
        {
            var systemUnderTest = new ButtonAttribute("ButtonName","foo");

            systemUnderTest.Order = 1;

            var order = systemUnderTest.Order;

            Assert.IsTrue(order == 1);
        }

        /// <summary>
        /// Test returns name as value.
        /// </summary>
        [TestMethod]
        public void Button_GetValue_ReturnsNameAsValue()
        {
            var systemUnderTest = new ButtonAttribute("ButtonName","foo");


            Assert.IsTrue(systemUnderTest.GetValue() == "ButtonName");
        }

        /// <summary>
        /// Test returns submit type as value.
        /// </summary>
        [TestMethod]
        public void Button_GetValueWithSubmitType_ReturnsSubmitTypeAsValue()
        {
            var systemUnderTest = new ButtonAttribute("ButtonName","foo");

            systemUnderTest.SubmitType = "SubmitType";

            Assert.IsTrue(systemUnderTest.GetValue() == "SubmitType");
        }

        /// <summary>
        /// Test returns type id.
        /// </summary>
        [TestMethod]
        public void Button_GetTypeId_ReturnsTypeId()
        {
            var systemUnderTest = new ButtonAttribute("ButtonName","foo");

            systemUnderTest.SubmitType = "SubmitType";

            Assert.IsNotNull(systemUnderTest.TypeId);
        }

        private class Model
        {
            [Button("ButtonName", "foo", ActionForDependencyType.Enabled, "DependentProperty1", ComparisonType.EqualTo, true, SubmitType = "SubmitType")]
            public string SinglePositive { get; set; }

            [Button("ButtonName", "foo", ActionForDependencyType.Enabled, "DependentProperty2", ComparisonType.EqualTo, new[] { "foo", "bar" }, SubmitType = "SubmitType")]
            public string MultiplePositive { get; set; }

            [Button("ButtonName", "foo", ActionForDependencyType.Enabled, "DependentProperty2", ComparisonType.NotEqualTo, new[] { "foo", "bar" }, SubmitType = "SubmitType")]
            public string MultipleNegative { get; set; }

            [Button("ButtonName", "foo", ActionForDependencyType.Enabled, "NotExist", ComparisonType.EqualTo, true, SubmitType = "SubmitType")]
            public string InvalidDependentProperty { get; set; }

            [Button("ButtonName","foo")]
            public string NoDependentProperty { get; set; }

            public bool DependentProperty1 { get; set; }

            public string DependentProperty2 { get; set; }
        }

        /// <summary>
        /// Test validates when condition is met.
        /// </summary>
        [TestMethod]
        public void Button_ConditionMet_Validates()
        {
            var model = new Model { DependentProperty1 = true };

            var systemUnderTest = (ButtonAttribute)model.GetType().GetProperty("SinglePositive").GetCustomAttributes(typeof(ButtonAttribute), false)[0];

            Assert.IsTrue(systemUnderTest.IsConditionMet("SinglePositive", null, model));
        }

        /// <summary>
        /// Test fails when condition is not met.
        /// </summary>
        [TestMethod]
        public void Button_ConditionNotMet_Fails()
        {
            var model = new Model { DependentProperty1 = false };

            var systemUnderTest = (ButtonAttribute)model.GetType().GetProperty("SinglePositive").GetCustomAttributes(typeof(ButtonAttribute), false)[0];

            Assert.IsFalse(systemUnderTest.IsConditionMet("SinglePositive", null, model));
        }

        /// <summary>
        /// Test validates when condition is met.
        /// </summary>
        [TestMethod]
        public void Button_ConditionMetMultiplePositive_Validates()
        {
            var model = new Model { DependentProperty2 = "foo" };

            var systemUnderTest = (ButtonAttribute)model.GetType().GetProperty("MultiplePositive").GetCustomAttributes(typeof(ButtonAttribute), false)[0];

            Assert.IsTrue(systemUnderTest.IsConditionMet("MultiplePositive", null, model));
        }

        /// <summary>
        /// Test fails when condition is not met.
        /// </summary>
        [TestMethod]
        public void Button_ConditionNotMetMultiplePositive_Fails()
        {
            var model = new Model { DependentProperty2 = "none" };

            var systemUnderTest = (ButtonAttribute)model.GetType().GetProperty("MultiplePositive").GetCustomAttributes(typeof(ButtonAttribute), false)[0];

            Assert.IsFalse(systemUnderTest.IsConditionMet("MultiplePositive", null, model));
        }

        /// <summary>
        /// Test validates when condition is met.
        /// </summary>
        [TestMethod]
        public void Button_ConditionMetMultipleNegative_Validates()
        {
            var model = new Model { DependentProperty2 = "none" };

            var systemUnderTest = (ButtonAttribute)model.GetType().GetProperty("MultipleNegative").GetCustomAttributes(typeof(ButtonAttribute), false)[0];

            Assert.IsTrue(systemUnderTest.IsConditionMet("MultipleNegative", null, model));
        }

        /// <summary>
        /// Test fails when condition is not met.
        /// </summary>
        [TestMethod]
        public void Button_ConditionNotMetMultipleNegative_Fails()
        {
            var model = new Model { DependentProperty2 = "foo" };

            var systemUnderTest = (ButtonAttribute)model.GetType().GetProperty("MultipleNegative").GetCustomAttributes(typeof(ButtonAttribute), false)[0];

            Assert.IsFalse(systemUnderTest.IsConditionMet("MultipleNegative", null, model));
        }

        /// <summary>
        /// Test fails when condition is not met.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Button_InvalidDependentProperty_ThrowsInvalidOperationException()
        {
            var model = new Model();

            var systemUnderTest = (ButtonAttribute)model.GetType().GetProperty("InvalidDependentProperty").GetCustomAttributes(typeof(ButtonAttribute), false)[0];

            systemUnderTest.IsConditionMet("InvalidDependentProperty", null, model);
        }

        /// <summary>
        /// Test validates with no dependent property.
        /// </summary>
        [TestMethod]
        public void Button_NoDependentProperty_Validates()
        {
            var model = new Model { DependentProperty1 = true };

            var systemUnderTest = (ButtonAttribute)model.GetType().GetProperty("NoDependentProperty").GetCustomAttributes(typeof(ButtonAttribute), false)[0];

            Assert.IsTrue(systemUnderTest.IsConditionMet("NoDependentProperty", null, model));
        }
    }
}
