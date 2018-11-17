using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;

namespace Employment.Web.Mvc.Infrastructure.Tests.Extensions
{
    /// <summary>
    ///This is a test class for TypeExtensionTest and is intended
    ///to contain all TypeExtensionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TypeExtensionTest
    {
        private Mock<IContainerProvider> mockContainerProvider;
        private Mock<IBuildManager> mockBuildManager;

        //Use TestInitialize to run code before running each test
        [TestInitialize]
        public void TestInitialize()
        {
            mockContainerProvider = new Mock<IContainerProvider>();
            mockBuildManager = new Mock<IBuildManager>();

            // Setup Dependency Resolver to use mocked Container Provider
            mockContainerProvider.Setup(m => m.GetService<IBuildManager>()).Returns(mockBuildManager.Object);
            DependencyResolver.SetResolver(mockContainerProvider.Object);
        }

        /// <summary>
        /// Test property null reference exception when Dependency Resolver does not have the IBuildManager registered.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TypeExtension_NoBuildManagerInDependencyResolver_ThrowsNullReferenceException()
        {
            mockContainerProvider = new Mock<IContainerProvider>();
            DependencyResolver.SetResolver(mockContainerProvider.Object);

            typeof (DataTypeAttribute).GetConcreteTypesImplementing();
        }

        /// <summary>
        /// Test property null reference exception when Dependency Resolver is not a Container Provider.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TypeExtension_NoContainerProviderInDependencyResolver_ThrowsNullReferenceException()
        {
            var mockDependencyResolver = new Mock<IDependencyResolver>();
            DependencyResolver.SetResolver(mockDependencyResolver.Object);

            typeof(DataTypeAttribute).GetConcreteTypesImplementing();
        }

        /// <summary>
        /// Test get public types implementing.
        /// </summary>
        [TestMethod]
        public void TypeExtension_GetPublicTypesImplementing_ReturnsPublicTypes()
        {
            var publicTypes = new [] { typeof(LocalClient)};

            mockBuildManager.Setup(m => m.PublicTypes).Returns(publicTypes);

            var types = typeof(IClient).GetPublicTypesImplementing();

            Assert.IsTrue(types.Any());
        }

        /// <summary>
        /// Test get concrete types implementing.
        /// </summary>
        [TestMethod]
        public void TypeExtension_GetConcreteTypesImplementing_ReturnsConcreteTypes()
        {
            var concreteTypes = new[] { typeof(EditableIfAttribute) };

            mockBuildManager.Setup(m => m.ConcreteTypes).Returns(concreteTypes);

            var types = typeof(ContingentAttribute).GetConcreteTypesImplementing();

            Assert.IsTrue(types.Any());
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod()]
        public void GetAttributeTest()
        {
            var a = typeof(TestType).GetAttribute<TestAttribute>();
            Assert.IsNotNull(a);
        }

        [TestMethod()]
        public void GetAttributeTest1()
        {
            var a = typeof(TestType).GetAttribute<TestAttribute>(m => m.Name.Equals("Test"));
            Assert.IsNotNull(a);
        }


        [TestMethod()]
        public void GetAttributesTest()
        {
            var a = typeof(TestType).GetAttributes<TestAttribute>();
            Assert.IsNotNull(a);
            Assert.IsTrue(a.Count() == 2);
        }

        [TestMethod()]
        public void GetAttributesTest1()
        {
            var a = typeof(TestType).GetAttributes<TestAttribute>(m => m.Name.Equals("Test"));
            Assert.IsNotNull(a);
            Assert.IsTrue(a.Count() == 1);
        }

        /// <summary>
        ///A test for GetEnumDescription
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void GetEnumDescriptionTest_ExpectedException()
        {
            typeof(TestType).GetEnumDescription("");
            Assert.Fail("Expected Exception");
        }


        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetEnumDescriptionTest_ExpectedException1()
        {
            var t = typeof(TestEnum).GetEnumDescription(null);
            Assert.Fail("Expected Exception");
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void GetEnumDescriptionTest_ExpectedException2()
        {
            int? v = 5;
            var t = typeof(TestEnum).GetEnumDescription(v);
            Assert.Fail("Expected Exception");
        }

        /// <summary>
        ///A test for GetEnumDescription
        ///</summary>
        [TestMethod()]
        public void GetEnumDescriptionTest()
        {
            var t = typeof(TestEnum).GetEnumDescription(TestEnum.Two);
            Assert.IsFalse(string.IsNullOrEmpty(t));
            Assert.AreEqual("2", t);
        }

        /// <summary>
        ///A test for GetNonNullableType
        ///</summary>
        [TestMethod()]
        public void GetNonNullableTypeTest()
        {
            var a = typeof(TestType).GetNonNullableType();
            Assert.IsNotNull(a);
        }

        /// <summary>
        ///A test for IsNumeric
        ///</summary>
        [TestMethod()]
        public void IsNumericTest()
        {
            var a = typeof(TestType).IsNumeric();
            Assert.IsFalse(a);
        }

        [TestMethod]
        public void IsNullableTest()
        {
            var a = typeof(TestType).IsNullableType();
            Assert.IsFalse(a);

            Assert.IsTrue(typeof (int?).IsNullableType());
        }

        [TestMethod]
        public void IsCollectionTypeTest()
        {
            Assert.IsTrue(typeof (List<string>).IsCollectionType());
            Assert.IsFalse(typeof(int).IsCollectionType());
        }

        [TestMethod]
        public void IsListTypeTest()
        {
            Assert.IsFalse(typeof(int).IsListType());
            Assert.IsTrue(typeof(List<string>).IsListType());
        }

        [TestMethod]
        public void IsEnumerableTypeTest()
        {
            Assert.IsFalse(typeof(int).IsEnumerableType());
            Assert.IsTrue(typeof(List<string>).IsEnumerableType());
        }

        [TestMethod]
        public void IsListOrDictionaryTypeTest()
        {
            Assert.IsFalse(typeof(int).IsListOrDictionaryType());
            Assert.IsTrue(typeof(List<string>).IsListOrDictionaryType());
        }

        [TestMethod]
        public void IsDictionaryTypeTest()
        {
            Assert.IsFalse(typeof(int).IsDictionaryType());
            Assert.IsTrue(typeof(Dictionary<int,string>).IsDictionaryType());
        }
    }

    public enum TestEnum
    {
        [System.ComponentModel.DescriptionAttribute("1")]
        One,

        [System.ComponentModel.DescriptionAttribute("2")]
        Two,

        [System.ComponentModel.DescriptionAttribute("3")]
        Three,
    
        [System.ComponentModel.DescriptionAttribute("4")]
        Four
    }

    [TestAttribute(Name = "Test")]
    [TestAttribute(Name = "Test1")]
    [AnotherAttribute(Name = "Another")]
    public class TestType
    {
        public string Text { get; set; }
        public int? Value { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class TestAttribute : Attribute
    {
        public override object TypeId { get { return this; } }
        public string Name { get; set; }
    }

    public class AnotherAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
