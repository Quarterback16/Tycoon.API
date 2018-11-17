using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProgramAssuranceTool.Models;

namespace ProgramAssuranceTool.Tests.Repositories
{
	[TestClass]
	public class ProjectRepositoryTests
	{
		[TestMethod]
		public void TestValidProjectTypes()
		{
			var sut = new Project {ProjectType = "PAN"};
			Assert.IsTrue( sut.IsValidProjectType() );
			sut.ProjectType = "SMO";
			Assert.IsTrue( sut.IsValidProjectType() );
			sut.ProjectType = "CMO";
			Assert.IsTrue( sut.IsValidProjectType() );
			sut.ProjectType = "PAS";
			Assert.IsTrue( sut.IsValidProjectType() );
			sut.ProjectType = "SCQ";
			Assert.IsTrue( sut.IsValidProjectType() );
		}

		[TestMethod]
		public void TestInValidProjectTypes()
		{
			var sut = new Project { ProjectType = "TIP" };
			Assert.IsFalse( sut.IsValidProjectType() );
			sut.ProjectType = "CNM";
			Assert.IsFalse( sut.IsValidProjectType() );
			sut.ProjectType = "PAP";
			Assert.IsFalse( sut.IsValidProjectType() );
		}
	}
}
