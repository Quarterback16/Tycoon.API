using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Mappers;
using Employment.Web.Mvc.Infrastructure.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="SelectionTypeAttribute" />.
    /// </summary>
    [TestClass]
    public class BlueWordAttributeTest
    {
        private class TestModel
        {
            [BlueWord]
            public string TestProperty { get; set; }

            public BlueWordAttribute GetAttribute()
            {
                return (BlueWordAttribute)GetType().GetProperty("TestProperty").GetCustomAttributes(typeof(BlueWordAttribute), false)[0];
            }

            public bool IsValid()
            {
                var validationContext = new ValidationContext(this, null, null);
                validationContext.DisplayName = "TestProperty";

                var attribute = GetAttribute();
                var result = attribute.GetValidationResult(GetType().GetProperty("TestProperty").GetValue(this, null), validationContext);

                return result == ValidationResult.Success;
            }
        }

        //private IMappingEngine mappingEngine;

        //protected IMappingEngine MappingEngine
        //{
        //    get
        //    {
        //        if (mappingEngine == null)
        //        {
        //            var mapper = new AdwMapper();
        //            mapper.Map(Mapper.Configuration);
        //            mappingEngine = Mapper.Engine;
        //        }

        //        return mappingEngine;
        //    }
        //}

        private Mock<IContainerProvider> mockContainerProvider;
        private Mock<IAdwService> mockAdwService;
        private Mock<IUserService> mockUserService;

        //Use TestInitialize to run code before running each test
        [TestInitialize]
        public void TestInitialize()
        {
            mockContainerProvider = new Mock<IContainerProvider>();
            mockAdwService = new Mock<IAdwService>();
            mockUserService = new Mock<IUserService>();

            // Set ADW to mock blue word tables unacceptable and identified words
            var unacceptableWords = new List<RelatedCodeModel>();
            unacceptableWords.Add(new RelatedCodeModel { Dominant = true, RelatedCode = "OFTW", DominantCode = "UNA", SubordinateDescription = "Blue"});

            var identifiedWords = new List<RelatedCodeModel>();
            identifiedWords.Add(new RelatedCodeModel { Dominant = true, RelatedCode = "OFTW", DominantCode = "IDE", SubordinateDescription = "Blues" });

            mockAdwService.Setup(m => m.GetRelatedCodes("OFTW", "UNA")).Returns(unacceptableWords);
            mockAdwService.Setup(m => m.GetRelatedCodes("OFTW", "IDE")).Returns(identifiedWords);

            // Setup Dependency Resolver to use mocked Container Provider
            mockContainerProvider.Setup(m => m.GetService<IAdwService>()).Returns(mockAdwService.Object);
            mockContainerProvider.Setup(m => m.GetService<IUserService>()).Returns(mockUserService.Object);
            //mockContainerProvider.Setup(m => m.GetService<IMappingEngine>()).Returns(MappingEngine);
            DependencyResolver.SetResolver(mockContainerProvider.Object);
        }

        /// <summary>
        /// Test property validates if it does not contain blue words.
        /// </summary>
        [TestMethod]
        public void BlueWord_HasAcceptableWords_Validates()
        {
            var model = new TestModel { TestProperty = "foo bar" };

            Assert.IsTrue(model.IsValid());
        }

        /// <summary>
        /// Test property validates if it contains an identified blue word.
        /// </summary>
        [TestMethod]
        public void BlueWord_HasIdentifiedWords_Fails()
        {
            var model = new TestModel { TestProperty = "foo blues bar" };

            Assert.IsTrue(model.IsValid());
        }

        /// <summary>
        /// Test property fails if it contains blue words.
        /// </summary>
        [TestMethod]
        public void BlueWord_HasUnacceptableWords_Fails()
        {
            var model = new TestModel { TestProperty = "foo blueword bar" };

            Assert.IsFalse(model.IsValid());
        }
    }
}
