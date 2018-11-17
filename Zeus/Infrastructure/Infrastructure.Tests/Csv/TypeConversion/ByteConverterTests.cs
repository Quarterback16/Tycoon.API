using System;
using Employment.Web.Mvc.Infrastructure.Csv.TypeConversion;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.Csv.TypeConversion
{
    [TestClass]
	public class ByteConverterTests
	{
		[TestMethod]
		public void ConvertToStringTest()
		{
			var converter = new ByteConverter();

			Assert.AreEqual( "123", converter.ConvertToString( (byte)123 ) );

			Assert.AreEqual( "", converter.ConvertToString( null ) );
		}

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
		public void ConvertFromStringTest()
		{
			var converter = new ByteConverter();

			Assert.AreEqual((byte)123, converter.ConvertFromString( "123" ) );
            Assert.AreEqual((byte)123, converter.ConvertFromString(" 123 "));

			converter.ConvertFromString( null );
		}

        [TestMethod]
        public void CanConvertFrom()
        {
            var converter = new ByteConverter();
            Assert.IsTrue(converter.CanConvertFrom(typeof (string)));
        }
	}
}
