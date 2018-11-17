using System;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="LinkAttribute" />.
    /// </summary>
    [TestClass]
    public class LinkAttributeTest
    {
        /// <summary>
        /// Test returns maximum int value for order if not specifically set.
        /// </summary>
        [TestMethod]
        public void Link_GetOrderNotSetDefaultsToIntMaxValue_ReturnsDefaultOrderValue()
        {
            var systemUnderTest = new LinkAttribute("LinkName");

            var order = systemUnderTest.Order;

            Assert.IsTrue(order == int.MaxValue);
        }

        /// <summary>
        /// Test returns maximum int value for order if not specifically set.
        /// </summary>
        [TestMethod]
        public void Link_GetOrderSetValue_ReturnsSetOrderValue()
        {
            var systemUnderTest = new LinkAttribute("LinkName");

            systemUnderTest.Order = 1;

            var order = systemUnderTest.Order;

            Assert.IsTrue(order == 1);
        }

        /// <summary>
        /// Test returns type id.
        /// </summary>
        [TestMethod]
        public void Link_GetTypeId_ReturnsTypeId()
        {
            var systemUnderTest = new LinkAttribute("LinkName");

            Assert.IsNotNull(systemUnderTest.TypeId);
        }

        private class Model
        {
            [Link("LinkName", ActionForDependencyType.Enabled, "DependentProperty1", ComparisonType.EqualTo, true)]
            public string SinglePositive { get; set; }

            [Link("LinkName", ActionForDependencyType.Enabled, "DependentProperty2", ComparisonType.EqualTo, new[] { "foo", "bar" })]
            public string MultiplePositive { get; set; }

            [Link("LinkName", ActionForDependencyType.Enabled, "DependentProperty2", ComparisonType.NotEqualTo, new[] { "foo", "bar" })]
            public string MultipleNegative { get; set; }

            [Link("LinkName", ActionForDependencyType.Enabled, "NotExist", ComparisonType.EqualTo, true)]
            public string InvalidDependentProperty { get; set; }

            [Link("LinkName")]
            public string NoDependentProperty { get; set; }

            public bool DependentProperty1 { get; set; }

            public string DependentProperty2 { get; set; }
        }

        /// <summary>
        /// Test validates when condition is met.
        /// </summary>
        [TestMethod]
        public void Link_ConditionMet_Validates()
        {
            var model = new Model { DependentProperty1 = true };

            var systemUnderTest = (LinkAttribute)model.GetType().GetProperty("SinglePositive").GetCustomAttributes(typeof(LinkAttribute), false)[0];

            Assert.IsTrue(systemUnderTest.IsConditionMet(model));
        }

        /// <summary>
        /// Test fails when condition is not met.
        /// </summary>
        [TestMethod]
        public void Link_ConditionNotMet_Fails()
        {
            var model = new Model { DependentProperty1 = false };

            var systemUnderTest = (LinkAttribute)model.GetType().GetProperty("SinglePositive").GetCustomAttributes(typeof(LinkAttribute), false)[0];

            Assert.IsFalse(systemUnderTest.IsConditionMet(model));
        }

        /// <summary>
        /// Test validates when condition is met.
        /// </summary>
        [TestMethod]
        public void Link_ConditionMetMultiplePositive_Validates()
        {
            var model = new Model { DependentProperty2 = "foo" };

            var systemUnderTest = (LinkAttribute)model.GetType().GetProperty("MultiplePositive").GetCustomAttributes(typeof(LinkAttribute), false)[0];

            Assert.IsTrue(systemUnderTest.IsConditionMet(model));
        }

        /// <summary>
        /// Test fails when condition is not met.
        /// </summary>
        [TestMethod]
        public void Link_ConditionNotMetMultiplePositive_Fails()
        {
            var model = new Model { DependentProperty2 = "none" };

            var systemUnderTest = (LinkAttribute)model.GetType().GetProperty("MultiplePositive").GetCustomAttributes(typeof(LinkAttribute), false)[0];

            Assert.IsFalse(systemUnderTest.IsConditionMet(model));
        }

        /// <summary>
        /// Test validates when condition is met.
        /// </summary>
        [TestMethod]
        public void Link_ConditionMetMultipleNegative_Validates()
        {
            var model = new Model { DependentProperty2 = "none" };

            var systemUnderTest = (LinkAttribute)model.GetType().GetProperty("MultipleNegative").GetCustomAttributes(typeof(LinkAttribute), false)[0];

            Assert.IsTrue(systemUnderTest.IsConditionMet(model));
        }

        /// <summary>
        /// Test fails when condition is not met.
        /// </summary>
        [TestMethod]
        public void Link_ConditionNotMetMultipleNegative_Fails()
        {
            var model = new Model { DependentProperty2 = "foo" };

            var systemUnderTest = (LinkAttribute)model.GetType().GetProperty("MultipleNegative").GetCustomAttributes(typeof(LinkAttribute), false)[0];

            Assert.IsFalse(systemUnderTest.IsConditionMet(model));
        }

        /// <summary>
        /// Test fails when condition is not met.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Link_InvalidDependentProperty_ThrowsInvalidOperationException()
        {
            var model = new Model();

            var systemUnderTest = (LinkAttribute)model.GetType().GetProperty("InvalidDependentProperty").GetCustomAttributes(typeof(LinkAttribute), false)[0];

            systemUnderTest.IsConditionMet(model);
        }

        /// <summary>
        /// Test validates with no dependent property.
        /// </summary>
        [TestMethod]
        public void Link_NoDependentProperty_Validates()
        {
            var model = new Model { DependentProperty1 = true };

            var systemUnderTest = (LinkAttribute)model.GetType().GetProperty("NoDependentProperty").GetCustomAttributes(typeof(LinkAttribute), false)[0];

            Assert.IsTrue(systemUnderTest.IsConditionMet(model));
        }
    }
}
