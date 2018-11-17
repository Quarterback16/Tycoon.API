using System;

using Employment.Web.Mvc.Infrastructure.TypeConverters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.TypeConverters
{
    /// <summary>
    /// Unit tests for <see cref="NullFloatTypeConverter" />.
    ///</summary>
    [TestClass]
    public class NullFloatTypeConverterTest
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
        public void NullFloatTypeConverter_ConvertCoreWithValidString_Converts()
        {

            float number = 5;
            string source = number.ToString();

            var result = NullFloatTypeConverter.Convert(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result == number);
        }

        /// <summary>
        /// Test the null string is converted to a date time of min value.
        ///</summary>
        [TestMethod]
        public void NullFloatTypeConverter_ConvertCoreWithNullString_Converts()
        {

            string source = null;

            var result = NullFloatTypeConverter.Convert(source);

            Assert.IsNull(result);
        }

        /// <summary>
        /// Test the empty string is converted to a date time of min value.
        ///</summary>
        [TestMethod]
        public void NullFloatTypeConverter_ConvertCoreWithEmptyString_Converts()
        {

            string source = string.Empty;

            var result = NullFloatTypeConverter.Convert(source);

            Assert.IsNull(result);
        }
    }
}
