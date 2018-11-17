// Copyright 2009-2012 Josh Close
// This file is a part of CsvHelper and is licensed under the MS-PL
// See LICENSE.txt for details or visit http://www.opensource.org/licenses/ms-pl.html
// http://csvhelper.com
#if WINRT_4_5
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif
using Employment.Web.Mvc.Infrastructure.Csv;

namespace Employment.Web.Mvc.Infrastructure.Tests.Csv
{
	[TestClass]
	public class StringExtensionTests
	{
		[TestMethod]
		public void IsNullOrWhiteSpaceTest()
		{
			string nullString = null;
			Assert.IsTrue( nullString.IsNullOrWhiteSpace() );
			Assert.IsTrue( "".IsNullOrWhiteSpace() );
			Assert.IsTrue( " ".IsNullOrWhiteSpace() );
			Assert.IsTrue( "	".IsNullOrWhiteSpace() );
			Assert.IsFalse( "a".IsNullOrWhiteSpace() );
		}
	}
}
