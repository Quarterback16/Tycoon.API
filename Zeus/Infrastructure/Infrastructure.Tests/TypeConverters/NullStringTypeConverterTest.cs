
using Employment.Web.Mvc.Infrastructure.TypeConverters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.TypeConverters
{
    /// <summary>
    ///This is a test class for NullStringTypeConverterTest and is intended
    ///to contain all NullStringTypeConverterTest Unit Tests
    ///</summary>
    [TestClass()]
    public class NullStringTypeConverterTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }



        /// <summary>
        ///A test for ConvertCore
        ///</summary>
        [TestMethod()]
        public void ConvertCoreTest()
        {
            string source = null;

            var result = NullStringTypeConverter.Convert(source);

            Assert.IsNotNull(result);
            Assert.AreEqual(string.Empty,result);
        }
    }
}
