using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProgramAssuranceTool.Models;
using ProgramAssuranceTool;

namespace Tests.Integration
{
	[TestClass]
	public class SampleRepositoryTests
	{
		[TestMethod]
		public void TestGetSessions()
		{
			var sut = new PatService();
			var list = sut.GetDistinctSessionKeys();
			Assert.IsTrue( list.Length > 0 );
		}

		[TestMethod]
		public void TestGetSampleExpiry()
		{
			var sut = new PatService();
			var list = sut.GetDistinctSessionKeys();
			foreach ( var s in list )
			{
				List<Sample> sample = sut.GetSample( s );
				var rec1 = sample.FirstOrDefault();
				if ( rec1 != null )
					Console.WriteLine( " Session {0} is {1} expired ", s, rec1.IsExpired() ? "" : "not" );
			}
			Assert.IsTrue( list.Length > 0 );
		}
	}
}
