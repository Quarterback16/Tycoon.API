
//using Employment.Web.Mvc.Infrastructure.TypeConverters;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.ComponentModel;
//using System.Globalization;
//using Moq;

//namespace Employment.Web.Mvc.Infrastructure.Tests.TypeConverters
//{
//    /// <summary>
//    ///This is a test class for ProfileExpressionTypeConverterTest and is intended
//    ///to contain all ProfileExpressionTypeConverterTest Unit Tests
//    ///</summary>
//    [TestClass()]
//    public class ProfileExpressionTypeConverterTest
//    {
//        /// <summary>
//        ///Gets or sets the test context which provides
//        ///information about and functionality for the current test run.
//        ///</summary>
//        public TestContext TestContext { get; set; }

//        /// <summary>
//        ///A test for ProfileExpressionTypeConverter Constructor
//        ///</summary>
//        [TestMethod()]
//        public void ProfileExpressionTypeConverterConstructorTest()
//        {
//            var target = new ProfileExpressionTypeConverter();
//            Assert.IsNotNull(target);
//        }

//        /// <summary>
//        ///A test for ConvertFrom
//        ///</summary>
//        [TestMethod()]
//        public void ConvertFromTest()
//        {
//            var target = new ProfileExpressionTypeConverter();
//            ITypeDescriptorContext context = new Mock<ITypeDescriptorContext>().Object;

//            CultureInfo culture = CultureInfo.CurrentCulture;

//            var result = target.ConvertFrom(context, culture, null);
//            Assert.IsNotNull(result as IConfiguration);
//        }
//    }
//}
