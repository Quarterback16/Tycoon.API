using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Repositories;
using ProgramAssuranceTool;

namespace Tests.Integration
{
	[TestClass]
	public class ReviewRepositoryTests
	{

		[TestMethod]
		public void TestReviewGet()
		{
			var sut = new PatService();
			var review = sut.GetReview( 200636 );
			Assert.AreEqual( "4HAS", review.ESACode );
			Assert.IsFalse( review.AutoSpecialClaim );
			Assert.IsFalse( review.ManualSpecialClaim );
		}

		[TestMethod]
		public void TestReviewUpdate()
		{
			var sut = new PatService();
			var review = sut.GetReview( 200636 );
			review.OutcomeCode = "NFA";
			sut.UpdateReview( review );
			review = sut.GetReview( 200636 );
			Assert.AreEqual( "NFA", review.OutcomeCode );
		}

		[TestMethod]
		public void TestGetAllByUploadId()
		{
			var repository = new ReviewRepository();
			var list = repository.GetAllByUploadId( 600 );
			Assert.IsTrue( list.Any() );
		}

		[TestMethod]
		public void TestCountReviews()
		{
			var repository = new ReviewRepository();
			var nReviews = repository.GetAllByProjectId( 13 ).Count();
			Assert.IsTrue( nReviews == 8 );
		}

		[TestMethod]
		public void TestReviewCount()
		{
			var sut = new PatService();
			var reviewCount = sut.CountReviews( 13 );
			Assert.AreEqual( 8, reviewCount );
		}

		[TestMethod]
		public void TestGetAllReviewsForUploadId()
		{
			var repository = new ReviewRepository();
			var list = repository.GetAllByUploadId( 141 );
			Assert.IsTrue( list.Any() );
		}

		[TestMethod]
		public void TestGetAllReviewsForProjectIdPaged()
		{
			var sut = new PatService();
			var list = sut.GetProjectReviews( 17, AppHelper.DefaultGridSettings() );
			Assert.IsTrue( list.Any() );
		}

		[TestMethod]
		public void TestCountReviewsForProjectIdPaged()
		{
			var sut = new PatService();
			var gs = AppHelper.DefaultGridSettings();
			var list = sut.GetProjectReviews( 17, gs );
			Assert.IsTrue( list.Count() == 19 );
		}

		[TestMethod]
		public void TestPage2ReviewsForProjectIdPaged()
		{
			var sut = new PatService();
			var gs = AppHelper.DefaultGridSettings();
			gs.PageSize = 10;
			gs.PageIndex = 2;
			var list = sut.GetProjectReviews( 17, gs );
			Assert.IsTrue( list.Count() == 9 );
		}

		[TestMethod]
		public void TestCountReviewsForProjectId()
		{
			var sut = new PatService();
			var gs = AppHelper.DefaultGridSettings();
			gs.PageSize = 10;
			gs.PageIndex = 1;
			var nRows = sut.CountReviews( 17, gs );
			Assert.IsTrue( nRows == 19 );
		}

	}
}
