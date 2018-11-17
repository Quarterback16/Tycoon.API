
//using Employment.Web.Mvc.Infrastructure.TypeConverters;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.ComponentModel;
//using System.Globalization;
//using Moq;

//namespace Employment.Web.Mvc.Infrastructure.Tests.TypeConverters
//{
//    /// <summary>
//    ///This is a test class for MappingEngineTypeConverterTest and is intended
//    ///to contain all MappingEngineTypeConverterTest Unit Tests
//    ///</summary>
//    [TestClass()]
//    public class MappingEngineTypeConverterTest
//    {
//        /// <summary>
//        ///Gets or sets the test context which provides
//        ///information about and functionality for the current test run.
//        ///</summary>
//        public TestContext TestContext { get; set; }

//        /// <summary>
//        ///A test for MappingEngineTypeConverter Constructor
//        ///</summary>
//        [TestMethod()]
//        public void MappingEngineTypeConverterConstructorTest()
//        {
//            var target = new MappingEngineTypeConverter();
//            Assert.IsNotNull(target);
//        }

//        /// <summary>
//        ///A test for ConvertFrom
//        ///</summary>
//        [TestMethod()]
//        public void ConvertFromTest()
//        {
//            var target = new MappingEngineTypeConverter();
//            ITypeDescriptorContext context = new Mock<ITypeDescriptorContext>().Object;

//            CultureInfo culture = CultureInfo.CurrentCulture;

//            var eng = target.ConvertFrom(context, culture, null);
//            Assert.IsNotNull(eng as IMappingEngine);
//        }
//    }
//}