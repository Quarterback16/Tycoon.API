using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Models;
using ProgramAssuranceTool;

namespace Tests.Integration
{
	[TestClass]
	public class ProjectRepositoryTests
	{
		[TestMethod]
		public void TestProjectGetAllCurrent()
		{
			var gridSettings = AppHelper.DefaultGridSettings();

			var sut = new PatService();
			var results = sut.GetProjects( gridSettings );
			Assert.IsTrue( results.Count > 0 );
		}

		[TestMethod]
		public void TestProjectGetPage1()
		{
			var gridSettings = AppHelper.DefaultGridSettings();
			gridSettings.PageSize = 10;
			var sut = new PatService();
			var results = sut.GetProjects( gridSettings );
			Assert.IsTrue( results.Count == 10 );
		}

		[TestMethod]
		public void TestProjectCount()
		{
			var gridSettings = AppHelper.DefaultGridSettings();
			var sut = new PatService();
			var projectCount = sut.CountProjects( gridSettings );
			Assert.IsTrue( projectCount > 0 );
		}

		[TestMethod]
		public void TestProjectCount2()
		{
			var gridSettings = new MvcJqGrid.GridSettings();
			var sut = new PatService();
			var projectCount = sut.CountProjects( gridSettings );
			Assert.IsTrue( projectCount > 0 );
		}

		[TestMethod]
		public void TestProjectDelete()
		{
			var sut = new PatService();
			var success = sut.DeleteProject( 23, "UnitTest" );
			Assert.IsTrue( success );
		}

		[TestMethod]
		public void TestProjectCreate()
		{
			var sut = new PatService();
			var project = new Project
				{
					ProjectName = "Steves Unit Test Project",
					ProjectType = "PAP",
					Coordinator = "SC0779",
					OrgCode = "MISN",
					Resource_NO = true,
					Resource_NSW_ACT = false,
					Resource_TAS = false,
					Resource_WA = false,
					Resource_SA = false,
					Resource_QLD = false,
					Resource_VIC = false,
					Resource_NT = false,
					CreatedBy = "UnitTest"
				};

			var newId = sut.CreateProject( project );
			Assert.IsTrue( newId > 0 );
		}

		[TestMethod]
		public void TestUniqueProjectNameOnMissingProject()
		{
			var sut = new PatService();
			var result = sut.IsProjectNameUsed( "Steves Project", 0 );
			Assert.IsFalse( result );
		}

		[TestMethod]
		public void TestUniqueProjectNameOnExistingProject()
		{
			var sut = new PatService();
			var result = sut.IsProjectNameUsed( "Steves unit test project", 0 );
			Assert.IsTrue( result );
		}

		[TestMethod]
		public void TestProjectUpdate()
		{
			var sut = new PatService();
			var project = sut.GetProject( 46 );
			const string newProjectName = "Steve Millard - SMAS - Contract Management";
			project.ProjectName = newProjectName;
			project.OrgCode = "SMAS";
			var oldComments = project.Comments;
			sut.UpdateProject( project );
			project = sut.GetProject( 46 );
			Assert.AreEqual( newProjectName, project.ProjectName );
			Assert.AreEqual( oldComments, project.Comments );
		}

		[TestMethod]
		public void TestProjectGetAll()
		{
			var sut = new PatService();
			var projectList = sut.GetProjects();
			Assert.IsTrue( projectList.Count > 0 );
		}

		[TestMethod]
		public void TestProjectGet()
		{
			var sut = new PatService();
			var project = sut.GetProject(46);
			Assert.IsTrue( project.OrgCode.Equals( "SMAS" ) );
			Assert.IsTrue( project.Resource_NSW_ACT );
		}

		[TestMethod]
		public void TestProjectGetByProjectName()
		{
			var sut = new PatService();
			var gridSettings = new MvcJqGrid.GridSettings { IsSearch = true, PageSize = 99999999, PageIndex = 1, SortColumn = "ProjectId" };
			var rule1 = new MvcJqGrid.Rule { field = "ProjectName", op = DataConstants.SqlOperationContains, data = "Steve" };
			var ruleArray = new MvcJqGrid.Rule[ 1 ];
			ruleArray[ 0 ] = rule1;
			var filter = new MvcJqGrid.Filter { rules = ruleArray };
			gridSettings.Where = filter;
			var projectList = sut.GetProjects( gridSettings );
			Assert.IsTrue( projectList.Any() );
		}

		[TestMethod]
		public void TestProjectGetResourceSetShort()
		{
			var sut = new PatService();
			var project = sut.GetProject( 13 );
			var resourceSet = project.ResourcesSetShort();
			Assert.IsTrue( resourceSet.Equals( "NSWACT,QLD,VIC,SA,WA,TAS,NT"	) );
		}

		[TestMethod]
		public void TestProjectExportRule()
		{
			var user = new PatUser( "SC0779" );
			// to check user resource membership : var userResource = user.ResourceSet();
			var sut = new PatService();
			var project = sut.GetProject( 13 );
			// to check project resource membership : var resourceSet = project.ResourcesSetShort();
			Assert.IsTrue( project.CanExport( user.LoginName ) );
		}

		[TestMethod]
		public void TestGetProjectsByUploadDateRange()
		{
			var sut = new PatService();
			var list = sut.GetProjects( AppHelper.DefaultGridSettings(), new DateTime( 2012, 1, 1 ), new DateTime( 2012, 12, 31 ) );
			Assert.IsTrue( list.Count == 1 );
			var list2 = sut.GetProjects( AppHelper.DefaultGridSettings(), new DateTime( 2011, 1, 1 ), new DateTime( 2011, 12, 31 ) );
			Assert.IsTrue( list2.Count == 4 );
		}

		[TestMethod]
		public void TestCountProjectsByUploadDateRange()
		{
			var sut = new PatService();
			var count2011 = sut.CountProjects( AppHelper.DefaultGridSettings(), new DateTime( 2011, 1, 1 ), new DateTime( 2011, 12, 31 ) );
			Assert.IsTrue( count2011 == 4 );
			var count2012 = sut.GetProjects( AppHelper.DefaultGridSettings(), new DateTime( 2012, 1, 1 ), new DateTime( 2012, 12, 31 ) );
			Assert.IsTrue( count2012.Count == 1 );
		}

		[TestMethod]
		public void TestCountProjectsByUploadBoundaryDateRange()
		{
			var sut = new PatService();
			var count= sut.CountProjects( AppHelper.DefaultGridSettings(), new DateTime( 2013, 9, 27 ), new DateTime( 2013, 9, 27 ) );
			Assert.IsTrue( count == 1 );
		}
	}
}
