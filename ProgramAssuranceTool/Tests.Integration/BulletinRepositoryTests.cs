using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProgramAssuranceTool;
using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Models;

namespace Tests.Integration
{
	[TestClass]
	public class BulletinRepositoryTests
	{
		[TestMethod]
		public void TestBullettinInsert()
		{
			//  CREATE
			var sut = new PatService();
			var titleVal = "Integration Test";
			var typeVal = "Test";
			var descVal = "Unit Test bulletin description";
			var startDate = new DateTime( 2013, 9, 19 );
			var bulletin = new Bulletin
				{
					BulletinTitle = titleVal,
					BulletinType = typeVal,
					Description = descVal,
					StartDate = startDate,
					CreatedBy = "UnitTest"
				};
			sut.CreateBulletin( bulletin );
			Assert.IsTrue( bulletin.BulletinId > 0 );
			//  READ
			var newId = bulletin.BulletinId;
			bulletin = sut.GetBulletin( newId );
			Assert.AreEqual( titleVal, bulletin.BulletinTitle );
			Assert.AreEqual( typeVal, bulletin.BulletinType );
			Assert.AreEqual( descVal, bulletin.Description );
			Assert.AreEqual( startDate, bulletin.StartDate );
			Assert.AreEqual( bulletin.EndDate, new DateTime( 1, 1, 1 ) );
			//  UPDATE
			var updatedDescVal = "Unit Test bulletin description -- **UPDATED**";
			var updatedTitleVal = "Integration Test -- **UPDATED**";
			var updatedTypeVal = "*Test*";
			var updatedStartDate = new DateTime( 2013, 9, 11 );
			var updatedEndDate = new DateTime( 2013, 11, 11 );
			bulletin.BulletinTitle = updatedTitleVal;
			bulletin.Description = updatedDescVal;
			bulletin.BulletinType = updatedTypeVal;
			bulletin.StartDate = updatedStartDate;
			bulletin.EndDate = updatedEndDate;
			bulletin.UpdatedBy = "UnitTest";
			bulletin = sut.UpdateBulletin( bulletin );
			Assert.AreEqual( updatedDescVal, bulletin.Description );
			bulletin = sut.GetBulletin( bulletin.BulletinId );
			Assert.AreEqual( updatedTitleVal, bulletin.BulletinTitle );
			Assert.AreEqual( updatedTypeVal, bulletin.BulletinType );
			Assert.AreEqual( updatedDescVal, bulletin.Description );
			Assert.AreEqual( updatedStartDate, bulletin.StartDate );
			Assert.AreEqual( updatedEndDate, bulletin.EndDate );

			//  DELETE
			sut.DeleteBulletin( newId );
			bulletin = sut.GetBulletin( newId );
			Assert.IsNull( bulletin );
		}

		[TestMethod]
		public void TestBullettinInsertWithProjectId()
		{
			//  CREATE
			var sut = new PatService();
			var titleVal = "Integration Test";
			var typeVal = "Test";
			var descVal = "Unit Test bulletin description";
			var startDate = new DateTime( 2013, 9, 19 );
			var projectId = 13; //  known to exist
			var bulletin = new Bulletin
				{
					BulletinTitle = titleVal,
					BulletinType = typeVal,
					Description = descVal,
					ProjectId = projectId,
					StartDate = startDate,
					CreatedBy = "UnitTest"
				};
			sut.CreateBulletin( bulletin );
			Assert.IsTrue( bulletin.BulletinId > 0 );
			//  READ
			var newId = bulletin.BulletinId;
			bulletin = sut.GetBulletin( newId );
			Assert.AreEqual( projectId, bulletin.ProjectId );
			Assert.AreEqual( titleVal, bulletin.BulletinTitle );
			Assert.AreEqual( typeVal, bulletin.BulletinType );
			Assert.AreEqual( descVal, bulletin.Description );
			Assert.AreEqual( startDate, bulletin.StartDate );
			Assert.AreEqual( bulletin.EndDate, new DateTime( 1, 1, 1 ) );
			//  UPDATE
			var updatedProjectId = 16;
			var updatedDescVal = "Unit Test bulletin description -- **UPDATED**";
			var updatedTitleVal = "Integration Test -- **UPDATED**";
			var updatedTypeVal = "*Test*";
			var updatedStartDate = new DateTime( 2013, 9, 11 );
			var updatedEndDate = new DateTime( 2013, 11, 11 );
			bulletin.ProjectId = updatedProjectId;
			bulletin.BulletinTitle = updatedTitleVal;
			bulletin.Description = updatedDescVal;
			bulletin.BulletinType = updatedTypeVal;
			bulletin.StartDate = updatedStartDate;
			bulletin.EndDate = updatedEndDate;
			bulletin.UpdatedBy = "UnitTest";
			bulletin = sut.UpdateBulletin( bulletin );
			Assert.AreEqual( updatedProjectId, bulletin.ProjectId );
			bulletin = sut.GetBulletin( bulletin.BulletinId );
			Assert.AreEqual( updatedProjectId, bulletin.ProjectId );
			Assert.AreEqual( updatedTitleVal, bulletin.BulletinTitle );
			Assert.AreEqual( updatedTypeVal, bulletin.BulletinType );
			Assert.AreEqual( updatedDescVal, bulletin.Description );
			Assert.AreEqual( updatedStartDate, bulletin.StartDate );
			Assert.AreEqual( updatedEndDate, bulletin.EndDate );

			//  DELETE
			sut.DeleteBulletin( newId );
			bulletin = sut.GetBulletin( newId );
			Assert.IsNull( bulletin );
		}

		[TestMethod]
		public void TestBulletinGetFAQs()
		{
			var isAdmin = true;

			var sut = new PatService();
			InitialiseBulletins();
			var results = sut.GetBulletins( DataConstants.FaqBulletinType, isAdmin );
			Assert.IsTrue( results.Count == 2 );
		}

		[TestMethod]
		public void TestBulletinGetAll()
		{
			var isAdmin = true;

			var sut = new PatService();
			InitialiseBulletins();
			var results = sut.GetBulletins( DataConstants.StandardBulletinType, isAdmin );
			Assert.IsTrue( results.Count == 4 );
		}

		//  set up known data for testing purposes
		private static void InitialiseBulletins()
		{
			//  First flush out the data
			var sut = new PatService();
			ClearOutAllBulletins( sut );
			//  Add 4 known records
			AddDummyBulletin( sut, "Integration Test 1", "FAQ", "Unit Test bulletin 1 description",
			                  new DateTime( 2013, 9, 19 ), new DateTime( 2013, 9, 19 ) );
			AddDummyBulletin( sut, "Integration Test 2", "FAQ", "Unit Test bulletin 2 description",
			                  new DateTime( 2013, 9, 22 ), new DateTime( 2013, 10, 14 ) );
			AddDummyBulletin( sut, "Integration Test 3", "STD", "Unit Test bulletin 3 description",
			                  new DateTime( 2013, 9, 25 ), new DateTime( 2013, 10, 10 ) );
			AddDummyBulletin( sut, "Integration Test 4", "STD", "Unit Test bulletin 4 description",
			                  new DateTime( 2013, 9, 26 ), new DateTime( 1, 1, 1 ) );
		}

		private static void ClearOutAllBulletins( IPatService sut )
		{
			var isAdmin = true;

			var results = sut.GetBulletins( DataConstants.StandardBulletinType, isAdmin );
			foreach ( var bulletin in results )
				sut.DeleteBulletin( bulletin.BulletinId );
		}

		private static void AddDummyBulletin( PatService sut, string titleVal,
		                                      string typeVal, string descVal, DateTime startDate, DateTime endDate )
		{
			var bulletin1 = new Bulletin
				{
					BulletinTitle = titleVal,
					BulletinType = typeVal,
					Description = descVal,
					StartDate = startDate,
					EndDate = endDate,
					CreatedBy = "UnitTest"
				};
			sut.CreateBulletin( bulletin1 );
		}

		[TestMethod]
		public void TestGetFaqBulletins()
		{
			var isAdmin = true;

			var sut = new PatService();
			InitialiseBulletins();
			var results = sut.GetBulletins( DataConstants.FaqBulletinType, isAdmin );
			Assert.IsTrue( results.Count == 2 );
		}

		[TestMethod]
		public void TestGetStandardBulletins()
		{
			var isAdmin = true;

			var sut = new PatService();
			InitialiseBulletins();
			var results = sut.GetBulletins( DataConstants.StandardBulletinType, isAdmin );
			Assert.IsTrue( results.Count == 2 );
		}
	}
}