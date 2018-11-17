using System.Collections.Generic;
using Elmah;
using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Infrastructure.Controllers;
using ProgramAssuranceTool.Infrastructure.Security;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ProgramAssuranceTool.Controllers
{
	/// <summary>
	/// Check List Controller
	/// </summary>
	[CustomAuthorize]
	public class CheckListController : InfrastructureController
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="CheckListController"/> class.
		/// </summary>
		/// <param name="controllerDependencies">Dependencies holder.</param>
		public CheckListController(IControllerDependencies controllerDependencies)
			: base(controllerDependencies)
		{
		}

		/// <summary>
		/// Navigates to Previous/ Next record.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="command">The command e.g. PREVIOUS or NEXT.</param>
		/// <returns></returns>
		public ActionResult Navigate(int id, string command = CommonConstants.ButtonUndefined)
		{
			var selections = (int[])Session["revids"];
			var reviewID = id;

			// check if reviewID exists in the pre-selected list 
			if (selections == null || !selections.Any(s => s.Equals(reviewID))) { return RedirectToInvalidSelection(reviewID); }

			switch (command)
			{
				case CommonConstants.ButtonPrevious:
					return RedirectToAction("Edit", new { id = AppHelper.GetPreviousItem(reviewID, selections) });

				case CommonConstants.ButtonNext:
					return RedirectToAction("Edit", new { id = AppHelper.GetNextItem(reviewID, selections) });

				default:
					AddErrorMessage(string.Format("Button: {0} is not defined. Please refresh your browser and try again.", command));
					return RedirectToAction("Edit", new { id = reviewID });
			}
		}

		/// <summary>
		/// To display the CheckList Edit screen.
		/// </summary>
		/// <param name="id">The review Id.</param>
		/// <param name="special">it's a special single review, it will overwrite the existing pre-selected review</param>
		/// <returns></returns>
		public ActionResult Edit(int id, bool special = false)
		{
			if (special)
			{
				Session["revids"] = new[] {id};
			}

			// check if reviewID exists in the pre-selected list 
			var selections = (int[])Session["revids"];

			var reviewID = id;

			// check if reviewID exists in the pre-selected list 
			if (selections == null || !selections.Any(s => s.Equals(reviewID))) { return RedirectToInvalidSelection(reviewID); }

			// find checklist from db with the 'original' values
			var viewModel = PatService.GetCheckList(reviewID);
			if (viewModel == null)
			{
				TempData[CommonConstants.FlashMessageTypeWarning] = string.Format(@"CheckList: {0} not found, Please try again", reviewID);
				return RedirectToPreviousAction();
			}

			if (!special && selections.Any() && selections.Count() > 1)
			{
				// if bulk update & more than 1 review selected then empty the dropdown and comment field
				viewModel.IsClaimDuplicateOverlapping = null;
				viewModel.IsClaimIncludedInDeedNonPayableOutcomeList = null;
				viewModel.DoesDocEvidenceMeetGuidelineRequirement = null;
				viewModel.IsDocEvidenceConsistentWithESS = null;
				viewModel.IsDocEvidenceSufficientToSupportPaymentType = null;
				viewModel.Comment = null;
			}

			// then update the UI from user-checklist from session if any
			var currentCheckList = AppHelper.GetSessionCheckList(Session);
			if (currentCheckList != null)
			{
				// then modified the original dropdown values with this values
				viewModel.IsClaimDuplicateOverlapping = currentCheckList.IsClaimDuplicateOverlapping;
				viewModel.IsClaimIncludedInDeedNonPayableOutcomeList = currentCheckList.IsClaimIncludedInDeedNonPayableOutcomeList;
				viewModel.DoesDocEvidenceMeetGuidelineRequirement = currentCheckList.DoesDocEvidenceMeetGuidelineRequirement;
				viewModel.IsDocEvidenceConsistentWithESS = currentCheckList.IsDocEvidenceConsistentWithESS;
				viewModel.IsDocEvidenceSufficientToSupportPaymentType = currentCheckList.IsDocEvidenceSufficientToSupportPaymentType;
				viewModel.Comment = (currentCheckList.Comment ?? string.Empty).Trim();
			}

			// check if user-checklist exist in the session & not null
			ViewBag.HasChanges = currentCheckList != null &&
			                     (
											!string.IsNullOrWhiteSpace(currentCheckList.IsClaimDuplicateOverlapping) ||
											!string.IsNullOrWhiteSpace(currentCheckList.IsClaimIncludedInDeedNonPayableOutcomeList) ||
											!string.IsNullOrWhiteSpace(currentCheckList.DoesDocEvidenceMeetGuidelineRequirement) ||
											!string.IsNullOrWhiteSpace(currentCheckList.IsDocEvidenceConsistentWithESS) ||
											!string.IsNullOrWhiteSpace(currentCheckList.IsDocEvidenceSufficientToSupportPaymentType) ||
				                     !string.IsNullOrWhiteSpace(currentCheckList.Comment)
			                     );

			// set the edit access. DR01039460 
			viewModel.CanEdit = PatService.GetProject(viewModel.ProjectID).CanEditCheckList(User.Identity.Name.RemoveDomain());
			viewModel.UpdatedBy = string.IsNullOrWhiteSpace(viewModel.UpdatedBy) ? User.Identity.Name.RemoveDomain() : viewModel.UpdatedBy;
			viewModel.AssessorName = new PatUser(viewModel.UpdatedBy).FullName;
			viewModel.UpdatedOn = viewModel.UpdatedOn.Equals(DateTime.MinValue) ? DateTime.Now : viewModel.UpdatedOn;
			viewModel.Status = viewModel.IsCheckListCompleted ? "Complete" : "In Progress";

			var currentPos = AppHelper.GetCurrentPosition(reviewID, selections);
			ViewBag.PageFooter = string.Format("{0} of {1}", currentPos, selections.Length);

			viewModel.PreviousButtonEnabled = AppHelper.IsPreviousButtonEnabled(currentPos, selections.Length) ? CommonConstants.EnabledElement : CommonConstants.DisabledElement;
			viewModel.NextButtonEnabled = AppHelper.IsNextButtonEnabled(currentPos, selections.Length) ? CommonConstants.EnabledElement : CommonConstants.DisabledElement;
			
			viewModel.SaveButtonEnabled = viewModel.CanEdit && ViewBag.HasChanges ? CommonConstants.EnabledElement : CommonConstants.DisabledElement; 

			if (viewModel.ClaimID > 0)
			{
				#region Retrieve Claim Start Date and End Date from Mainframe
				try
				{
					// get the claim start date and end date from mainframe
					var zeis0P82Detail = PatService.GetZEIS0P82Details(viewModel.ClaimID, viewModel.ClaimSequenceNumber);

					if (zeis0P82Detail != null && zeis0P82Detail.ClaimId > 0)
					{
						viewModel.ClaimStartDate = zeis0P82Detail.ClaimStartDate;
						viewModel.ClaimEndDate = zeis0P82Detail.ClaimEndDate;
					}
					else
					{
						TempData[CommonConstants.FlashMessageTypeWarning] =
							string.Format(@"Claim: {0} with Sequence Number: {1} not found in Mainframe. {2}. Please try again later",
											  viewModel.ClaimID,
											  viewModel.ClaimSequenceNumber,
											  zeis0P82Detail != null ? zeis0P82Detail.ErrorMessage : string.Empty);
					}
				}
				catch (Exception ex)
				{
					ErrorLog.GetDefault(null).Log(new Error(ex));

					// display the mainframe error message to user
					TempData[CommonConstants.FlashMessageTypeWarning] = string.Format(@"Error occured when requesting Claim Start Date and End Date from Mainframe. Please try again later");
				}

				#endregion
			}

			return View(viewModel);
		}

		/// <summary>
		/// Edits the specified view model.
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <param name="command">The command.</param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult Edit(CheckList viewModel, string command = CommonConstants.ButtonUndefined)
		{
			var selections = (int[])Session["revids"];
			var reviewID = viewModel.ReviewID;

			// validate the access. DR01039460 
			viewModel.CanEdit = PatService.GetProject(viewModel.ProjectID).CanEditCheckList(User.Identity.Name.RemoveDomain());
			if (!viewModel.CanEdit) { return RedirectToNoAccessAction(); }

			// check if reviewID exists in the pre-selected list 
			if (selections == null || !selections.Any(s => s.Equals(reviewID))) { return RedirectToInvalidSelection(reviewID); }

			// update the user-checklist into session
			AppHelper.SetSessionCheckList(Session, viewModel);

			switch (command)
			{
				case CommonConstants.ButtonSave:
					// save the user-checklist(s) which is in the session 
					return SaveAll(selections);

				case CommonConstants.ButtonPrevious:
					return RedirectToAction("Edit", new { id = AppHelper.GetPreviousItem(reviewID, selections) });

				case CommonConstants.ButtonNext:
					return RedirectToAction("Edit", new { id = AppHelper.GetNextItem(reviewID, selections) });

				default:
					AddErrorMessage(string.Format("Button: {0} is not defined. Please refresh your browser and try again.", command));
					return View(viewModel);
			}
		}

		/// <summary>
		/// Save all the checklists value with the current-user-modified checklist
		/// </summary>
		/// <param name="selections">the list of selected reviews</param>
		/// <returns></returns>
		private ActionResult SaveAll(int[] selections)
		{
			TempData[CommonConstants.FlashMessageTypeInfo] = string.Format(@"CheckList(s): 0 record saved");

			if (selections.Any())
			{
				var currentCheckList = AppHelper.GetSessionCheckList(Session);
				if (currentCheckList != null)
				{
					foreach (var reviewId in selections)
					{
						var checklist = new CheckList
							{
								ReviewID = reviewId,
								IsClaimDuplicateOverlapping = currentCheckList.IsClaimDuplicateOverlapping,
								IsClaimIncludedInDeedNonPayableOutcomeList = currentCheckList.IsClaimIncludedInDeedNonPayableOutcomeList,
								DoesDocEvidenceMeetGuidelineRequirement = currentCheckList.DoesDocEvidenceMeetGuidelineRequirement,
								IsDocEvidenceConsistentWithESS = currentCheckList.IsDocEvidenceConsistentWithESS,
								IsDocEvidenceSufficientToSupportPaymentType = currentCheckList.IsDocEvidenceSufficientToSupportPaymentType,
								Comment = (currentCheckList.Comment ?? string.Empty).Trim(),
								CreatedBy = User.Identity.Name.RemoveDomain(),
								CreatedOn = DateTime.Now,
								UpdatedBy = User.Identity.Name.RemoveDomain(),
								UpdatedOn = DateTime.Now
							};

						PatService.SaveCheckList(checklist);
					}

					var reviewIds = string.Join(", ", selections.Select(id => id));
					TempData[CommonConstants.FlashMessageTypeInfo] = string.Format(@"CheckList(s): {0} saved successfully", reviewIds);
				}
			}

			// clear the session
			AppHelper.ClearSessionCheckLists(Session);

			// must exit to the caller otherwise the session will become dirty again.
			return RedirectToPreviousAction();

		}

		#region Private Method

		private ActionResult SaveChanges(List<CheckList> viewModels)
		{

			TempData[CommonConstants.FlashMessageTypeInfo] = string.Format(@"CheckList(s): 0 record saved");

			if (viewModels.Any())
			{
				foreach (var viewModel in viewModels)
				{
					viewModel.CreatedBy = User.Identity.Name.RemoveDomain();
					viewModel.CreatedOn = DateTime.Now;
					viewModel.UpdatedBy = User.Identity.Name.RemoveDomain();
					viewModel.UpdatedOn = DateTime.Now;

					PatService.SaveCheckList(viewModel);
				}

				var reviewIds = string.Join(", ", viewModels.Select(c => c.ReviewID));
				TempData[CommonConstants.FlashMessageTypeInfo] = string.Format(@"CheckList(s): {0} saved successfully", reviewIds);
			}

			// clear the session
			AppHelper.ClearSessionCheckLists(Session);

			// must exit to the caller otherwise the session will become dirty again.
			return RedirectToPreviousAction();
		}


		private ActionResult RedirectToPreviousAction()
		{
			var uploadID = AppHelper.GetSessionUploadId(Session);
			if (uploadID > 0)
			{
				// return to the previous UI if possible: review item list UI 
				return RedirectToAction("Details", "Upload", new { Id = uploadID });
			}
			return RedirectToProjectList();
		}

		private ActionResult RedirectToProjectList()
		{
			TempData[CommonConstants.FlashMessageTypeWarning] = @"Your session has timeout, Please select again your Project.";
			return RedirectToAction("Index", "Project");
		}

		private ActionResult RedirectToNoAccessAction()
		{
			TempData[CommonConstants.FlashMessageTypeWarning] = @"You have no access to this feature. Please contact your administrator.";
			return RedirectToPreviousAction();
		}

		private ActionResult RedirectToInvalidSelection(int reviewID)
		{
			TempData[CommonConstants.FlashMessageTypeWarning] = string.Format(@"CheckList: {0} is not in the pre-selected review check list. Please try again", reviewID);
			return RedirectToPreviousAction();
		}
		
		#endregion
	}
}