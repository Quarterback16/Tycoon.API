using System;
using System.Collections.Generic;

using Employment.Web.Mvc.Infrastructure.TypeConverters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.TypeConverters
{
    /// <summary>
    /// Unit tests for <see cref="QueryStringTypeConverter" />.
    ///</summary>
    [TestClass]
    public class QueryStringTypeConverterTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }



        /// <summary>
        /// Test the valid query string is converted to a dictionary.
        ///</summary>
        [TestMethod]
        public void QueryStringTypeConverter_ConvertCoreWithValidString_Converts()
        {

            string source = "foo=bar&x=y";

            var result= QueryStringTypeConverter.Convert(source);

            Assert.IsTrue(result["foo"].ToString() == "bar");
            Assert.IsTrue(result["x"].ToString() == "y");
        }

        /// <summary>
        /// Test the null string is converted to a date time of min value.
        ///</summary>
        [TestMethod]
        public void BoolTypeConverter_ConvertCoreWithNullString_Converts()
        {

            string source = null;

            var result = BoolTypeConverter.Convert(source);

            Assert.IsFalse(result);
        }

        /// <summary>
        /// Test the empty string is converted to a date time of min value.
        ///</summary>
        [TestMethod]
        public void BoolTypeConverter_ConvertCoreWithEmptyString_Converts()
        {

            string source = string.Empty;

            var result = BoolTypeConverter.Convert(source);

            Assert.IsFalse(result);
        }
    }
}
