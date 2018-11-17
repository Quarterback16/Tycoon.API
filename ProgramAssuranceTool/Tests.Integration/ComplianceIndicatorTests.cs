using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Models;
using ProgramAssuranceTool;

namespace Tests.Integration
{
	[TestClass]
	public class ComplianceIndicatorTests
	{
		[TestMethod]
		public void TestProjectUpdate()
		{
			var sut = new PatService();
			var complianceIndicator = new ComplianceIndicator
				{
					ComplianceIndicatorId = 0, 
					SubjectTypeCode = DataConstants.ComplianceIndicatorSubject_Organisations,
					Subject = "XXXX",
					Value = 0.613M,
					Quarter = "20131",
					UpdatedBy = "UnitTest"
				};
			sut.SaveComplianceIndicator( complianceIndicator );
		}

	}
}
