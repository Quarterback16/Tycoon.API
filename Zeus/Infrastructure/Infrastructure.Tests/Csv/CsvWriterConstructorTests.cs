﻿// Copyright 2009-2012 Josh Close
// This file is a part of CsvHelper and is licensed under the MS-PL
// See LICENSE.txt for details or visit http://www.opensource.org/licenses/ms-pl.html
// http://csvhelper.com
using System;
using System.IO;
#if WINRT_4_5
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Employment.Web.Mvc.Infrastructure.Csv;
using Employment.Web.Mvc.Infrastructure.Csv.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace Employment.Web.Mvc.Infrastructure.Tests.Csv
{
	[TestClass]
	public class CsvWriterConstructorTests
	{
		[TestMethod]
		public void EnsureInternalsAreSetupWhenPasingWriterAndConfigTest()
		{
			using( var stream = new MemoryStream() )
			using( var writer = new StreamWriter( stream ) )
			{
				var config = new CsvConfiguration();
				using( var csv = new CsvWriter( writer, config ) )
				{
					Assert.AreSame( config, csv.Configuration );
				}
			}
		}
	}
}
