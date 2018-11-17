using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProgramAssuranceTool.Helpers;

namespace ProgramAssuranceTool.Tests.Utilities
{
	[TestClass]
	public class AppHelperTests
	{
		[TestMethod]        
		public void TestQuarterFor()
		{
			var theTestDate = new DateTime( 2014, 1, 21 );
			var quarter = AppHelper.QuarterFor( theTestDate );
			Assert.AreEqual( "20141", quarter );
		}

		[TestMethod]
		public void TestPercent()
		{
			var percent = AppHelper.Percent( 75, 100 );
			Assert.AreEqual( .75M, percent );
		}

		[TestMethod]
		public void TestDivideByZeroPercent()
		{
			var percent = AppHelper.Percent( 75, 0);
			Assert.AreEqual( 0.0M, percent );
		}

	}
}
