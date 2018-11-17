using System;
using Employment.Web.Mvc.Infrastructure.Csv.TypeConversion;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.Csv.TypeConversion
{
    [TestClass]
	public class DateTimeConverterTests
	{
		[TestMethod]
		public void ConvertToStringTest()
		{
			var converter = new DateTimeConverter();

			var dateTime = DateTime.Now;

			// Valid conversions.
			Assert.AreEqual( dateTime.ToString(), converter.ConvertToString( dateTime ) );

			// Invalid conversions.
			Assert.AreEqual( "1", converter.ConvertToString( 1 ) );
            Assert.AreEqual("", converter.ConvertToString(null));
		}

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
		public void ConvertFromStringTest()
		{
			var converter = new DateTimeConverter();

			var dateTime = DateTime.Now;

			// Valid conversions.
            Assert.AreEqual(dateTime.ToString(), converter.ConvertFromString(dateTime.ToString()).ToString());
            Assert.AreEqual(dateTime.ToString(), converter.ConvertFromString(dateTime.ToString("o")).ToString());
            Assert.AreEqual(dateTime.ToString(), converter.ConvertFromString(" " + dateTime + " ").ToString());

			// Invalid conversions.
            converter.ConvertFromString(null);
		}
	}
}
