using System;

using Employment.Web.Mvc.Infrastructure.TypeConverters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.TypeConverters
{
    /// <summary>
    /// Unit tests for <see cref="NullDateTimeTypeConverter" />.
    ///</summary>
    [TestClass]
    public class NullDateTimeTypeConverterTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }



        /// <summary>
        /// Test the valid string is converted to a date time.
        ///</summary>
        [TestMethod]
        public void NullDateTimeTypeConverter_ConvertCoreWithValidString_Converts()
        {

            string source = DateTime.Now.ToLongDateString();

            var result = NullDateTimeTypeConverter.Convert(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result != DateTime.MinValue);
        }

        /// <summary>
        /// Test the null string is converted to a date time of min value.
        ///</summary>
        [TestMethod]
        public void NullDateTimeTypeConverter_ConvertCoreWithNullString_Converts()
        {

            string source = null;

            var result = NullDateTimeTypeConverter.Convert(source);

            Assert.IsNull(result);
        }

        /// <summary>
        /// Test the empty string is converted to a date time of min value.
        ///</summary>
        [TestMethod]
        public void NullDateTimeTypeConverter_ConvertCoreWithEmptyString_Converts()
        {
            string source = string.Empty;

            var result = NullDateTimeTypeConverter.Convert(source);

            Assert.IsNull(result);
        }
    }
}
