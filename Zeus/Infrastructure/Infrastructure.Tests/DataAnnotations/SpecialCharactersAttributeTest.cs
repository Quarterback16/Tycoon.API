using System.ComponentModel.DataAnnotations;

using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="SpecialCharactersAttribute" />.
    /// </summary>
    [TestClass]
    public class SpecialCharactersAttributeTest
    {
        private class TestModel
        {
            [SpecialCharacters]
            [Infrastructure.DataAnnotations.Bindable]
            public string TestProperty { get; set; }

            public SpecialCharactersAttribute GetAttribute()
            {
                return (SpecialCharactersAttribute)GetType().GetProperty("TestProperty").GetCustomAttributes(typeof(SpecialCharactersAttribute), false)[0];
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
        
        //Use TestInitialize to run code before running each test
        [TestInitialize]
        public void TestInitialize()
        {
            
        }

        /// <summary>
        /// Test property validates if it does not contain special characters.
        /// </summary>
        [TestMethod]
        public void SpecialCharacters_NoSpecialCharacters_Validates()
        {
            var text = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()`~-_=+\|]}[{;:'"",<.>/?";
            var model = new TestModel { TestProperty = text };

            Assert.IsTrue(model.IsValid());
        }

        /// <summary>
        /// Test property fails if it contains special characters.
        /// </summary>
        [TestMethod]
        public void SpecialCharacters_HasSpecialCharacters_Fails()
        {
            var text = @"abcdefõghijklmnopqrstuvwxyzABCDEFGHIJK¾LMNOåPQRSTUVWXYZ12Æ34567890!@#$%^&*()`~-_=+\|]}[{;:'"",<.>/?z æzõõ zöz€z™z…zzäzözz/z, zxc. -+`1234567890~!@#$%^&*()_=\\|][{}:';<>?/";

            var model = new TestModel { TestProperty = text };

            Assert.IsFalse(model.IsValid());
        }
    }
}
