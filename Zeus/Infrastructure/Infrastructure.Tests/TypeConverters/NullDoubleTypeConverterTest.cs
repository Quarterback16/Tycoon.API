using Employment.Web.Mvc.Infrastructure.TypeConverters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.TypeConverters
{
    /// <summary>
    /// Unit tests for <see cref="NullDoubleTypeConverter" />.
    ///</summary>
    [TestClass]
    public class NullDoubleTypeConverterTest
    {

        /// <summary>
        /// Test the valid string is converted to a date time.
        ///</summary>
        [TestMethod]
        public void NullDoubleTypeConverter_ConvertCoreWithValidString_Converts()
        {
            double number = 5;
            string source = number.ToString();

            var result = NullDoubleTypeConverter.Convert(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result == number);
        }

        /// <summary>
        /// Test the null string is converted to a date time of min value.
        ///</summary>
        [TestMethod]
        public void NullDoubleTypeConverter_ConvertCoreWithNullString_Converts()
        {

            string source = null;

            var result = NullDoubleTypeConverter.Convert(source);

            Assert.IsNull(result);
        }

        /// <summary>
        /// Test the empty string is converted to a date time of min value.
        ///</summary>
        [TestMethod]
        public void NullDoubleTypeConverter_ConvertCoreWithEmptyString_Converts()
        {

            string source = string.Empty;

            var result = NullDoubleTypeConverter.Convert(source);

            Assert.IsNull(result);
        }
    }
}
