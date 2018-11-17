using System;
using Employment.Web.Mvc.Infrastructure.Csv.TypeConversion;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.Csv.TypeConversion
{
    [TestClass]
	public class CharConverterTests
	{
		[TestMethod]
		public void ConvertToStringTest()
		{
			var converter = new CharConverter();

			Assert.AreEqual( "a", converter.ConvertToString( 'a' ) );

            Assert.AreEqual("True", converter.ConvertToString(true));

            Assert.AreEqual("", converter.ConvertToString(null));
		}

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
		public void ConvertFromStringTest()
		{
			var converter = new CharConverter();

            Assert.AreEqual('a', converter.ConvertFromString("a"));
            Assert.AreEqual('a', converter.ConvertFromString(" a "));

            converter.ConvertFromString(null);
		}

        [TestMethod]
        public void CanConvertFrom()
        {
            var converter = new CharConverter();
            Assert.IsTrue(converter.CanConvertFrom(typeof(string)));
        }
	}
}
