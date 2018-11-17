using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProgramAssuranceTool.Models;
using ProgramAssuranceTool;

namespace Tests.Integration
{
	[TestClass]
	public class ProjectContractRepositoryTests
	{
		[TestMethod]
		public void TestProjectContractCreate()
		{
			var sut = new PatService();
			var projectContract = new ProjectContract
				{
					ProjectId = 13,
					ContractType = "JSA",
					CreatedBy = "UnitTest"
				};
			var newId = sut.CreateProjectContract( projectContract );
			Assert.IsTrue( newId > 0 );
		}

		[TestMethod]
		public void TestProjectContractGetByProject()
		{
			var sut = new PatService();
			var list = sut.GetProjectContractsByProject( 13 );
			Assert.IsTrue( list.Any() );
		}

		[TestMethod]
		public void TestProjectContractContains()
		{
			var sut = new PatService();
			var projectContract = new ProjectContract
			{
				ProjectId = 13,
				ContractType = "JSA",
				CreatedBy = "UnitTest"
			};
			var list = sut.GetProjectContractsByProject( 13 );
			Assert.IsTrue( list.Contains( projectContract ) );
		}

		[TestMethod]
		public void TestProjectContractGetAll()
		{
			var sut = new PatService();
			var list = sut.GetProjectContracts();
			Assert.IsTrue( list.Any() );
		}

		[TestMethod]
		public void TestProjectContractSave()
		{
			var sut = new PatService();
			const int projectId = 13;
			var list = new List<ProjectContract>
				{
					new ProjectContract {ProjectId = projectId, ContractType = "DES"},
					new ProjectContract {ProjectId = projectId, ContractType = "HLS"},
					new ProjectContract {ProjectId = projectId, ContractType = "JSA"}
				};
			sut.SaveProjectContracts( projectId, list, "UnitTest" );

			Assert.IsTrue( list.Any() );
		}

		[TestMethod]
		public void TestGetProjectContractSelections()
		{
			var sut = new PatService();
			var contracts = sut.GetProjectContractSelections( 13 );
			Assert.AreEqual( "DES,HLS,JSA", contracts );
		}

	}
}
