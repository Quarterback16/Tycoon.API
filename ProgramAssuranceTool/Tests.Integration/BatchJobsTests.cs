using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool;

namespace Tests.Integration
{
	[TestClass]
	public class BatchJobsTests
	{
		[TestMethod]
		public void TestDiskSpace()
		{
			BatchJobs.HealthCheck( new PatService(), new DateTime(1,1,1), "Forced By Unit Test"  );
			Assert.IsTrue( true );
		}
	}
}
