using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProgramAssuranceTool.Repositories;
using ProgramAssuranceTool;

namespace Tests.Integration
{
	[TestClass]
	public class UploadRepositoryTests
	{
		[TestMethod]
		public void TestUploadGet()
		{
			var sut = new PatService();
			var upload = sut.GetUploadById( 343 );
			Assert.IsFalse( upload.RandomFlag );
			Assert.IsFalse( upload.AcceptedFlag );
		}

		[TestMethod]
		public void TestUploadUpdate()
		{
			var sut = new PatService();
			var upload = sut.GetUploadById( 343 );
			upload.AcceptedFlag = true;
			upload.RandomFlag = true;
			sut.UpdateUpload( upload );
			upload = sut.GetUploadById( 343 );
			Assert.IsTrue( upload.RandomFlag );
			Assert.IsTrue( upload.AcceptedFlag );
		}

		[TestMethod]
		public void TestUploadGetAllByProjectId()
		{
			var uploadRepository = new UploadRepository();
			var uploadList = uploadRepository.GetAllByProjectId( 13 );
			Assert.IsTrue( uploadList.Count > 0 );
		}

		[TestMethod]
		public void TestUploadCountByProjectId()
		{
			var sut = new PatService();
			var nUploads = sut.CountUploads( 13 );
			Assert.IsTrue( nUploads > 0 );
		}

		[TestMethod]
		public void TestEarliestAndLatestUploadDates()
		{
			var sut = new PatService();
			DateTime latest;
			var earliest = sut.GetEarliestAndLatestUploadDates( out latest );
			Assert.AreEqual( new DateTime( 2011, 5, 11 ).Date, earliest.Date );
			Assert.AreEqual( new DateTime( 2013, 9, 27 ).Date, latest.Date );
		}

		[TestMethod]
		public void TestSampleNameIsUsed()
		{
			var sut = new PatService();
			var isUsed = sut.SampleNameIsUsed( "Upload 378 - ORSR - 4EAM - FQ40", 17 );
			Assert.IsTrue( isUsed );
		}

		[TestMethod]
		public void TestUpdateSample()
		{
			var sut = new PatService();
			var upload = sut.GetUploadById( 373 );
			upload.Name = "Steves new Upload Name";
			upload.DueDate = DateTime.Now;
			sut.UpdateUpload( upload );
			var upload2 = sut.GetUploadById( upload.UploadId );
			Assert.IsTrue( upload2.DueDate.Date.Equals( DateTime.Now.Date ) );
		}

		[TestMethod]
		public void TestUploadGetAllByProjectId2()
		{
			var uploadRepository = new UploadRepository();
			var uploadList = uploadRepository.GetAllByProjectId( 28 );
			foreach ( var upload in uploadList )
			{
				Assert.IsTrue( upload.NationalFlag );
			}

			Assert.IsTrue( uploadList.Count > 0 );
		}
	}
}
