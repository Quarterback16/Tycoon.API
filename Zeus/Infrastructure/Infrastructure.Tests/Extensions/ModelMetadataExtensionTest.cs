using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Employment.Web.Mvc.Infrastructure.Tests.Extensions
{

    public class TestModel
    {
        [Display(Name = "Value",Order = 1, Description = "Value Description")]        
        public string Value { get; set; }

        [DefaultValue(TestEnum.Three)]
        public TestEnum Enum { get; set;  }
    }

    public class TestModelMetadata : ModelMetadata
    {
        public TestModelMetadata(ModelMetadataProvider provider, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName) : base(provider, containerType, modelAccessor, modelType, propertyName)
        {
        }
    }

    /// <summary>
    ///This is a test class for ModelMetadataExtensionTest and is intended
    ///to contain all ModelMetadataExtensionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ModelMetadataExtensionTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

      
        [TestMethod()]
        public void GetAttributeTest()
        {
            var provider = new DataAnnotationsModelMetadataProvider();
            var modelType = new TypeDelegator(typeof(TestModel));
            var t = new ModelMetadata(provider, null, null, modelType, string.Empty);

            t.AdditionalValues["Attributes"] = new List<DisplayAttribute>() { new DisplayAttribute()};
            var a = t.GetAttribute<DisplayAttribute>();

            Assert.IsNotNull(a);
        }


        [TestMethod()]
        public void GetAttribute_PredicateTest()
        {
            var provider = new DataAnnotationsModelMetadataProvider();
            var modelType = new TypeDelegator(typeof(TestModel));
            var t = new ModelMetadata(provider, null, null, modelType, string.Empty);

            t.AdditionalValues["Attributes"] = new List<DisplayAttribute>() { new DisplayAttribute() {Name = "TestField", Order = 1 } };
            var a = t.GetAttribute<DisplayAttribute>(p => p.Name.Equals("TestField"));

            Assert.IsNotNull(a);
            Assert.AreEqual("TestField",a.Name);
            Assert.AreEqual(1, a.Order);
        }

        
        [TestMethod()]
        public void GetAttributesTest()
        {
            var provider = new DataAnnotationsModelMetadataProvider();
            var modelType = new TypeDelegator(typeof(TestModel));
            var t = new ModelMetadata(provider, null, null, modelType, string.Empty);

            t.AdditionalValues["Attributes"] = new List<DisplayAttribute>() { new DisplayAttribute() { Name = "TestField", Order = 1 } };
            var a = t.GetAttributes<DisplayAttribute>().ToList();

            Assert.IsNotNull(a);
            Assert.AreEqual(1, a.Count());
            Assert.AreEqual("TestField", a[0].Name);
            Assert.AreEqual(1, a[0].Order);
        }

        [TestMethod()]
        public void GetAttributesNullTest()
        {
            var provider = new DataAnnotationsModelMetadataProvider();
            var modelType = new TypeDelegator(typeof(TestModel));
            var t = new ModelMetadata(provider, null, null, modelType, string.Empty);

            t.AdditionalValues["Attributes"] = new List<DisplayAttribute>();
            var a = t.GetAttributes<DisplayAttribute>();

            Assert.IsNotNull(a);
            Assert.AreEqual(0, a.Count());
        }
    }
}