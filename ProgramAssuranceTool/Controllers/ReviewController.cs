using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Elmah;
using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Infrastructure.Controllers;
using ProgramAssuranceTool.Infrastructure.Security;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Models;
using ProgramAssuranceTool.ViewModels.Review;

namespace ProgramAssuranceTool.Controllers
{
	[CustomAuthorize]
	public class ReviewController : InfrastructureController
	{
		  public ReviewController(IControllerDependencies controllerDependencies)
				: base(controllerDependencies)
		  {
		  }

		#region  Update Review Outcomes

		  //  http://localhost:6491/Review/Edit/175067
		[HttpGet]
		public ActionResult Edit( int id )
		{
			try
			{
				var loggedInUser = new PatUser(Request.LogonUserIdentity);
				var projectId = AppHelper.GetSessionProjectId( Session );

				if (projectId == 0) return RedirectToProjectsList();

				var project = PatService.GetProject( projectId );

				ViewBag.IsProjectContractMonitoringOrContractSiteVisit = project != null && project.IsContractMonitoringOrContractSiteVisit();
				ViewBag.CanEditCheckList = project != null && project.CanEditCheckList(User.Identity.Name.RemoveDomain());

				var questionnaire = PatService.GetReviewQuestionnaire(id);
				ViewBag.AnyQuestionsAndAnswers= questionnaire != null;

				var review = PatService.GetReview( id );
				if (project != null && review.CanEdit( loggedInUser, project.ResourcesSetShort() ))
				{
					var upload = PatService.GetUploadById( review.UploadId );
					var vm = new ReviewDetailsViewModel
						{
							ProjectId = project.ProjectId,
							ProjectName = project.ProjectName,
							UploadName = upload.Name,
							OutOfScope = upload.IsOutOfScope(),
							OutOfScopeFlag = !upload.InScope,	
							Additional = upload.AdditionalOrNot(),
							AdditionalFlag = upload.AdditionalReview,
							Review = review,
							CanDelete = review.CanDelete( loggedInUser ),
							CanEdit = true,
							ChangesMade = "N"
						};

					ViewData[ "review-details" ] = GenerateReviewDetails( review );
					ViewData[ "related-data" ] = GenerateRelatedData( review.ClaimId, review.ClaimSequenceNumber );
					vm.OldAssessmentOutcome = vm.Review.AssessmentCode;
					vm.OldRecoveryReason = vm.Review.RecoveryReason;
					vm.OldOutcomeCode = vm.Review.OutcomeCode;
					vm.OldAssessmentAction = vm.Review.AssessmentAction;
					vm.DeleteMessage = "Are you sure you want to delete this review?";
					if (vm.Review.Status().Equals( "Completed" ))
						vm.DeleteMessage = "Are you sure you want to delete this review? as it has outcomes recorded.";

					// keep the existing (`not current`) adw code displayed
					ViewBag.OldAssessmentOutcomeList = PatService.GetAdwCode(DataConstants.AdwListCodeForAssessmentCodes, vm.OldAssessmentOutcome, true);
					ViewBag.OldRecoveryReasonList = PatService.GetAdwCode(DataConstants.AdwListCodeForRecoveryReasonCodes, vm.OldRecoveryReason, true);
					ViewBag.OldAssessmentActionList = PatService.GetAdwCode(DataConstants.AdwListCodeForAssessmentActionCodes, vm.OldAssessmentAction, true);
					ViewBag.OldFinalOutcomeList = PatService.GetAdwCode(DataConstants.AdwListCodeForOutcomeCodes, vm.OldOutcomeCode, true);

					return View( vm );
				}
				TempData[ CommonConstants.FlashMessageTypeWarning ] =
					"You must be part of the Project resource groups to edit its' reviews.";
				return RedirectToProjectDetailsPageTab( projectId, CommonConstants.ProjectTab_Details );
			}
			catch ( Exception ex )
			{
				ErrorLog.GetDefault( null ).Log( new Error( ex ) );

				return View( "Error", new HandleErrorInfo( ex, "Review", "Edit" ) );
			}
		}

		private ActionResult RedirectToProjectDetailsPageTab( int projectId, int tabNo )
		{
			return RedirectToAction( "Details", "Project", new { id = projectId, tabNo } );
		}

		private ActionResult RedirectToProjectsList()
		{
			return RedirectToAction( "Index", "Project" );
		}

		[HttpPost]
		public ActionResult Edit( string button, ReviewDetailsViewModel vm )
		{
			try
			{
				var loggedInUser = new PatUser(Request.LogonUserIdentity);
				if ( button == "delete" || button == null )
				{
					PatService.DeleteReview( vm.Review, loggedInUser.LoginName );
					TempData[ CommonConstants.FlashMessageTypeInfo ] = string.Format( "Review {0} deleted", vm.Review.ReviewId );
					return RedirectToAction( "Details", "Upload", new { id = vm.Review.UploadId } );
				}
				if (ModelState.IsValid)
				{
					if (vm.Review.AssessmentCode == DataConstants.AssessmentValid ||
						 vm.Review.AssessmentCode == DataConstants.AssessmentValidwithQualification )
						vm.Review.OutcomeCode = DataConstants.FinalOutcomeValid_NFA;

					if (vm.Review.AssessmentCode != vm.OldAssessmentOutcome)
						vm.Review.AssessmentDate = DateTime.Now;
					if ( vm.Review.RecoveryReason != vm.OldRecoveryReason)
						vm.Review.RecoveryReasonDate = DateTime.Now;
					if ( vm.Review.OutcomeCode != vm.OldOutcomeCode )
						vm.Review.FinalOutcomeDate = DateTime.Now;
					if ( vm.Review.AssessmentAction != vm.OldAssessmentAction )
						vm.Review.AssessmentActionDate = DateTime.Now;
					//  save the update
					vm.Review.UpdatedBy = loggedInUser.LoginName;
					PatService.UpdateReview( vm.Review );
					TempData[ CommonConstants.FlashMessageTypeInfo ] = string.Format( "Review {0} successfully updated", vm.Review.ReviewId );
					return RedirectToAction( "Details", "Upload", new { id = vm.Review.UploadId } );
				}

				var projectId = AppHelper.GetSessionProjectId(Session);
				if (projectId == 0) return RedirectToProjectsList();
				var project = PatService.GetProject(projectId);

				ViewBag.IsProjectContractMonitoringOrContractSiteVisit = project != null && project.IsContractMonitoringOrContractSiteVisit();
				ViewBag.CanEditCheckList = project != null && project.CanEditCheckList(User.Identity.Name.RemoveDomain());

				var questionnaire = PatService.GetReviewQuestionnaire(vm.Review.ReviewId);
				ViewBag.AnyQuestionsAndAnswers = questionnaire != null;

					//  get the View Data back
				ViewData[ "review-details" ] = AppHelper.GetSessionReviewDetails( Session );
				ViewData[ "related-data" ] = AppHelper.GetSessionRelatedData( Session );
				return View( vm );
			}
			catch (Exception ex)
			{
				ErrorLog.GetDefault(null).Log(new Error(ex));

				return View( "Error", new HandleErrorInfo( ex, "Review", "Edit" ) );
			}

		}

		#endregion

		#region  Bulk Update
		// http://localhost:6491/Upload/Details/282
		[HttpGet]
		public ActionResult Details( int id )
		{
			try
			{	
				var projectId = AppHelper.GetSessionProjectId(Session);
				var project = PatService.GetProject(projectId);
				var review = PatService.GetReview(id);
				var upload = PatService.GetUploadById(review.UploadId);
				var vm = SessionHelper.GetSessionBulkOutcomes(Session);
				if (vm.Review == null)
				{
					vm = new ReviewDetailsViewModel
						{
							ProjectId = project.ProjectId,
							ProjectName = project.ProjectName,
							UploadName = upload.Name,
							OutOfScope = upload.IsOutOfScope(),
							OutOfScopeFlag = !upload.InScope,
							Additional = upload.AdditionalOrNot(),
							AdditionalFlag = upload.AdditionalReview,
							Review = review,
							ChangesMade = "N"
						};
				}

				ViewBag.IsProjectContractMonitoringOrContractSiteVisit = project != null && project.IsContractMonitoringOrContractSiteVisit();

				ViewData["review-details"] = GenerateReviewDetails(review);
				ViewData["related-data"] = GenerateRelatedData(review.ClaimId, review.ClaimSequenceNumber);
				if (vm.Review != null)
				{
					vm.OldAssessmentOutcome = vm.Review.AssessmentCode;
					vm.OldRecoveryReason = vm.Review.RecoveryReason;
					vm.OldOutcomeCode = vm.Review.OutcomeCode;
				}
				else
				{
					vm.Review = review;
				}

				vm.Review.ReviewId = id;
				var selections = (int[])Session["revids"];
				vm.Nav = PositionOf(id, selections);
				vm.NumberOfSelections = SelectionsMessageFor(selections, id);
				vm.DeleteMessage = CheckForOutcomes(selections);

				return View(vm);

			}
			catch (Exception ex)
			{
				ErrorLog.GetDefault(null).Log(new Error(ex));

				return View("Error", new HandleErrorInfo(ex, "Review", "Details"));
			}
		}

		private string CheckForOutcomes( int[] selections )
		{
			var msg = "Are you sure you want to delete these reviews?";
			if (PatService.AnyOutComesFor( selections ))
				msg += " - some of the reviews have outcomes.";
			return msg;
		}

		private static string PositionOf( int id, IList<int> selections )
		{
			if (selections.Count == 0) return "first";
			if (selections[ 0 ] == id)
				return "first";
			return selections[ selections.Count - 1 ] == id ? "last" : "middle";
		}

		[HttpPost]
		public ActionResult Details( string button, ReviewDetailsViewModel vm )
		{
			var selections = (int[]) Session[ "revids" ];
			if ( button == null )
			{
				var ids = selections.ToList();
				PatService.DeleteReviews( ids, User.Identity.Name.RemoveDomain() );
				TempData[ CommonConstants.FlashMessageTypeInfo ] = string.Format( "{0} Reviews deleted", ids.Count );
				return RedirectToAction( "Details", "Upload", new { id = vm.Review.UploadId } );
			}
			if (button == "next")
			{
				SessionHelper.SetSessionBulkReviewOutcomes( Session, vm );
				var nextId = GetNextId( vm.Review.ReviewId, selections );
				return RedirectToAction( "Details", "Review", new { id = nextId } );
			}
			if (button == "previous")
			{
				SessionHelper.SetSessionBulkReviewOutcomes( Session, vm );
				var prevId = GetPreviousId( vm.Review.ReviewId, selections );
				return RedirectToAction( "Details", "Review", new { id = prevId } );				
			}
			if ( ModelState.IsValid )
			{
				if ( vm.Review.AssessmentCode == DataConstants.AssessmentValid ||
					vm.Review.AssessmentCode == DataConstants.AssessmentValidwithQualification )
					vm.Review.OutcomeCode = DataConstants.FinalOutcomeValid_NFA;
				if ( vm.Review.AssessmentCode != vm.OldAssessmentOutcome )
					vm.Review.AssessmentDate = DateTime.Now;
				if ( vm.Review.RecoveryReason != vm.OldRecoveryReason )
					vm.Review.RecoveryReasonDate = DateTime.Now;
				if ( vm.Review.OutcomeCode != vm.OldOutcomeCode )
					vm.Review.FinalOutcomeDate = DateTime.Now;
				if ( vm.Review.AssessmentAction != vm.OldAssessmentAction )
					vm.Review.AssessmentActionDate = DateTime.Now;
				//  save the update
				vm.Review.UpdatedBy = User.Identity.Name.RemoveDomain();
				PatService.BulkOutcome( selections, vm.Review, vm.Review.UpdatedBy );
				SessionHelper.SetSessionBulkReviewOutcomes( Session, null );
				TempData[ CommonConstants.FlashMessageTypeInfo ] = string.Format( "{0} Reviews updated", selections.Count() );
				return RedirectToAction( "Details", "Upload", new { id = vm.Review.UploadId } );
			}

			//  get the View Data back
			ViewData[ "review-details" ] = AppHelper.GetSessionReviewDetails( Session );
			ViewData[ "related-data" ] = AppHelper.GetSessionRelatedData( Session );
			return View( vm );
		}

		private static int GetPreviousId( int reviewId, IList<int> selections )
		{
			var prevId = reviewId;
			for ( var i = selections.Count - 1; i > -1; i-- )
			{
				if ( selections[ i ] != reviewId ) continue;
				if ( i == 0 ) continue;
				prevId = selections[ i - 1 ];
				break;
			}
			return prevId;
		}

		private static int GetNextId( int reviewId, IList<int> selections )
		{
			var nextId = reviewId;
			for ( var i = 0; i < selections.Count; i++ )
			{
				if (selections[ i ] != reviewId) continue;
				if (i + 1 >= selections.Count) continue;
				nextId = selections[ i + 1 ];
				break;
			}
			return nextId;
		}

		private static string SelectionsMessageFor( IList<int> selections, int reviewId )
		{
			if (selections.Count > 1)
			{
				var ndx = 1;
				for ( var i = 0; i < selections.Count; i++ )
				{
					if (selections[ i ] != reviewId) continue;
					ndx = i + 1;
					break;
				}
				return string.Format( "{0} of {1} selected", ndx, selections.Count );
			}
			return selections.Count == 1 ? "1 review selected" : string.Format( "{0} reviews selected", selections.Count  );
		}

		#endregion

		private string GenerateRelatedData(long claimID, int claimSequenceNo)
		{
			var label = new [] {
				"Claim Status",
				"Claim Start Date",
				"Claim End Date",
				"Contract Ref Date",
				"Contract Ref Outcome cd",
				"Contract Ref Outcome Desc",
				"Contract Ref Placement Com",
				"Contract Ref Placement Act End",
				"Employer Name",
				"Job Creation Date",
				"Job Hours",
				"Job Id",
				"Job Tenure Desc",
				"Job Title",
				"LMR",
				"Placement Date",
				"Reg Status Code",
				"Specialty Code",
				"Stream"
			};

			// initialise it 
			var data = new[] {
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty
			};

			// get the detail from mainframe 
			var detail = PatService.GetZEIS0P82Details(claimID, claimSequenceNo);
			if (detail != null && detail.ClaimId > 0)
			{
				data = new[]
					{
						string.Format("{0}", detail.ClaimStatusCode),
						string.Format("{0}", detail.ClaimStartDate.ToShortDateString()),
						string.Format("{0}", detail.ClaimEndDate.ToShortDateString()),
						string.Format("{0}", detail.ReferralDate.ToShortDateString()),
						string.Format("{0}", detail.RefOutcomeCode),
						string.Format("{0}", detail.RefOutcomeDescription),
						string.Format("{0}", detail.RefPlacementComDate.ToShortDateString()),
						string.Format("{0}", detail.RefPlacementEndDate.ToShortDateString()),
						string.Format("{0}", detail.EmployerName),
						string.Format("{0}", detail.JobCreationDate.ToShortDateString()),
						string.Format("{0}", detail.JobHoursTxt),
						string.Format("{0}", detail.JobId),
						string.Format("{0}", detail.JobTenureDescription),
						string.Format("{0}", detail.JobTitleText),
						string.Format("{0}", detail.LMRCode),
						string.Format("{0}", detail.PlacementDate.ToShortDateString() ),
						string.Format("{0}", detail.RegStatusCode),
						string.Format("{0}", detail.SiteSpecialistType),
						string.Format("{0}", detail.Stream)
					};
			}

			var html = AppHelper.TableFromArray(label, data, "Related Data", showEmpty:false);
			AppHelper.SetSessionRelatedData( Session, html );
			return html;
		}

		private string GenerateReviewDetails( Review review )
		{
			var label = new[] {
				"Review ID",
				"Org Code",
				"Org Name",
				"ESA Code",
				"ESA Name",
				"Site Code",
				"Site Name",
				"State Code",
				"Job Seeker ID",
				"Job Seeker Given Name",
				"Job Seeker Surname",
				"Claim ID",
				"Claim Type",
				"Claim Description",
				"Claim Amount",
				"Claim Creation Date",
				"Managed By",
				"Contract Type",
				"Contract Type Description",
				"Auto Special Claim Flag",
				"Manual Special Claim Flag",
				"Activity ID",
				"Review Status"
			};

			var data = new[] {
				review.ReviewId.ToString( CultureInfo.InvariantCulture ),
				review.OrgCode,
				PatService.GetOrgName( review.OrgCode ),
				review.ESACode,
				PatService.GetEsaDescription( review.ESACode ),
				review.SiteCode,
				PatService.GetSiteDescription( review.SiteCode ),
				review.StateCode,
				review.JobseekerId.ToString( CultureInfo.InvariantCulture ),
				review.JobSeekerGivenName,
				review.JobSeekerSurname,
				review.ClaimId.ToString( CultureInfo.InvariantCulture ),
				review.ClaimType,
				PatService.GetClaimTypeDescription( review.ClaimType ),
				AppHelper.NullableDollarAmount( review.ClaimAmount ),
				AppHelper.ShortDate( review.ClaimCreationDate ),
				review.ManagedBy,
				review.ContractType,
				PatService.GetContractTypeDescription( review.ContractType ),
				AppHelper.FlagOut( review.AutoSpecialClaim ),
				AppHelper.FlagOut( review.ManualSpecialClaim ),
				review.ActivityId.ToString( CultureInfo.InvariantCulture ),
				review.Status()
			};

			var html = AppHelper.TableFromArray( label, data, "Review Details", showEmpty:true );
			AppHelper.SetSessionReviewDetails( Session, html );
			return html;
		}

	}
}