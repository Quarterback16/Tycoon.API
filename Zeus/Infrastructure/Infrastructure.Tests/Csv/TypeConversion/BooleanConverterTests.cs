﻿using System;
using Employment.Web.Mvc.Infrastructure.Csv.TypeConversion;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.Csv.TypeConversion
{
    [TestClass]
	public class BooleanConverterTests
	{
		[TestMethod]
		public void ConvertToStringTest()
		{
			var converter = new BooleanConverter();

			Assert.AreEqual( "True", converter.ConvertToString( true ) );

            Assert.AreEqual("False", converter.ConvertToString(false));

            Assert.AreEqual("", converter.ConvertToString(null));
            Assert.AreEqual("1", converter.ConvertToString(1));
		}

        [TestMethod]
        public void ConvertFromStringTest()
        {
            var converter = new BooleanConverter();

            Assert.IsTrue((bool) converter.ConvertFromString("true"));
            Assert.IsTrue((bool) converter.ConvertFromString("True"));
            Assert.IsTrue((bool) converter.ConvertFromString("TRUE"));
            Assert.IsTrue((bool) converter.ConvertFromString("1"));
            Assert.IsTrue((bool) converter.ConvertFromString("yes"));
            Assert.IsTrue((bool) converter.ConvertFromString("YES"));
            Assert.IsTrue((bool) converter.ConvertFromString("y"));
            Assert.IsTrue((bool) converter.ConvertFromString("Y"));
            Assert.IsTrue((bool) converter.ConvertFromString(" true "));
            Assert.IsTrue((bool) converter.ConvertFromString(" yes "));
            Assert.IsTrue((bool) converter.ConvertFromString(" y "));

            Assert.IsFalse((bool) converter.ConvertFromString("false"));
            Assert.IsFalse((bool) converter.ConvertFromString("False"));
            Assert.IsFalse((bool) converter.ConvertFromString("FALSE"));
            Assert.IsFalse((bool) converter.ConvertFromString("0"));
            Assert.IsFalse((bool)converter.ConvertFromString("no"));
            Assert.IsFalse((bool)converter.ConvertFromString("NO"));
            Assert.IsFalse((bool)converter.ConvertFromString("n"));
            Assert.IsFalse((bool)converter.ConvertFromString("N"));
            Assert.IsFalse((bool) converter.ConvertFromString(" false "));
            Assert.IsFalse((bool) converter.ConvertFromString(" 0 "));
            Assert.IsFalse((bool)converter.ConvertFromString(" no "));
            Assert.IsFalse((bool)converter.ConvertFromString(" n "));
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ConverterThrowsNotSupported()
        {
            var converter = new BooleanConverter();
            converter.ConvertFromString(null);
		}
	}
}