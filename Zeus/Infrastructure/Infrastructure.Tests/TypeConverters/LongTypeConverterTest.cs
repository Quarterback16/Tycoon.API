﻿using System;

using Employment.Web.Mvc.Infrastructure.TypeConverters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.TypeConverters
{
    /// <summary>
    /// Unit tests for <see cref="LongTypeConverter" />.
    ///</summary>
    [TestClass]
    public class LongTypeConverterTest
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
        public void LongTypeConverter_ConvertCoreWithValidString_Converts()
        {

            long number = 5;
            string source = number.ToString();

            var result = LongTypeConverter.Convert(source);

            Assert.IsTrue(result == number);
        }

        /// <summary>
        /// Test the null string is converted to a date time of min value.
        ///</summary>
        [TestMethod]
        public void LongTypeConverter_ConvertCoreWithNullString_Converts()
        {
            string source = null;

            var result = LongTypeConverter.Convert(source);

            Assert.IsTrue(result == 0);
        }

        /// <summary>
        /// Test the empty string is converted to a date time of min value.
        ///</summary>
        [TestMethod]
        public void LongTypeConverter_ConvertCoreWithEmptyString_Converts()
        {
            string source = string.Empty;

            var result = LongTypeConverter.Convert(source);
            Assert.IsTrue(result == 0);
        }
    }
}
