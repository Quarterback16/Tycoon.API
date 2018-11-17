using MvcJqGrid;
using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Infrastructure.Controllers;
using ProgramAssuranceTool.Infrastructure.Security;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ProgramAssuranceTool.Controllers
{
	/// <summary>
	/// Questionnaire Controller
	/// </summary>
	[CustomAuthorize]
	public class QuestionnaireController : InfrastructureController
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="QuestionnaireController"/> class.
		/// </summary>
		/// <param name="controllerDependencies">Dependencies holder.</param>
		public QuestionnaireController(IControllerDependencies controllerDependencies)
			: base(controllerDependencies)
		{
		}

		/// <summary>
		/// Display the Review Questions and Answers screen
		/// </summary>
		/// <param name="id">The review Id.</param>
		/// <returns></returns>
		public ActionResult Edit(int id)
		{
			var reviewID = id;

			// find the detail Questionnare = the header data
			var viewModel = PatService.GetReviewQuestionnaire(reviewID);

			if (viewModel == null)
			{
				TempData[CommonConstants.FlashMessageTypeWarning] = string.Format(@"Questionnaire with Review Id: {0} not found, Please try again later.", reviewID);
				return RedirectToPreviousAction();
			}

			// double check DR01039408 
			var project = PatService.GetProject(viewModel.ProjectID);
			if (project != null && !project.IsContractMonitoringOrContractSiteVisit())
			{
				// that's fine
			}
			else 
			{
				return RedirectToInvalidRequest();
			}

			return View(viewModel);
			
		}

		/// <summary>
		/// Get the list of Questions & Answers data of the Questionnaire (the header data)
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="id">The review id.</param>
		/// <returns></returns>
		public ActionResult QuestionAnswerGetData(MvcJqGrid.GridSettings gridSettings, int id)
		{
			try
			{
				var data = PatService.GetQuestionAnswers(gridSettings, id);
				var totalRecords = PatService.CountQuestionAnswersByReviewId(id);

				var jsonData = new
					{
						total = AppHelper.PagesInTotal(totalRecords, gridSettings.PageSize),
						page = gridSettings.PageIndex,
						records = totalRecords,
						rows = (
							       from e in data.AsEnumerable()
							       select new
								       {
									       id = 1,
									       cell = new List<string>
										       {
											       string.Format("{0}", e.QuestionId),
											       string.Format("{0}", e.QuestionCode),
													 // this tabindex to make the tag able to receive tab key
											       string.Format("<a tabindex='0' class='wrap'>{0}</a>", e.QuestionText),
											       string.Format("{0}", e.AnswerText)
										       }
								       }
						       ).ToArray()
					};

				return Json(jsonData, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
				throw;
			}

		}

		/// <summary>
		/// To reviews questionnaire.
		/// </summary>
		/// <param name="id">The upload Id.</param>
		/// <param name="projectId">The project Id.</param>
		/// <returns></returns>
		public ActionResult ReviewQuestionnaire(int id, int projectId)
		{
			if (!CanEdit) { return RedirectToNoAccessAction(); }

			var uploadId = id;
			ViewBag.UploadId = uploadId;
			ViewBag.UploadName = AppHelper.GetSessionUploadName(Session);
			ViewBag.ProjectId = projectId;
			ViewBag.ProjectName = AppHelper.GetSessionProjectName(Session);

			if (ViewBag.UploadId < 1 || ViewBag.ProjectId < 1)
			{
				return RedirectToProjectList();
			}

			// find the extra colums of the questionnare based on the upload Id
			var viewModel = PatService.GetQuestionAnswersByUploadId(uploadId);

			if (viewModel == null || viewModel.Count < 1)
			{
				TempData[CommonConstants.FlashMessageTypeWarning] = string.Format(@"Extra columns of Questionnaire with Upload Id: {0} not found, Please try again later.", uploadId);
				return RedirectToPreviousAction();
			}

			// get the extra columns header 
			ViewBag.ExtraColumnHeaders = viewModel.Select(qa => new Column(qa.QuestionCode).SetLabel(qa.QuestionText)).ToList();

			return View("List");
		}

		/// <summary>
		/// to get Questionnaires data as JSON
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="id">The upload Id.</param>
		/// <returns></returns>
		public ActionResult QuestionnaireGetData(MvcJqGrid.GridSettings gridSettings, int id)
		{
			try
			{
				var data = PatService.GetReviewQuestionnaireData(gridSettings, id);
				var totalRecords = PatService.CountReviewQuestionnairesByUploadId(id);

				var jsonData = new
				{
					total = AppHelper.PagesInTotal(totalRecords, gridSettings.PageSize),
					page = gridSettings.PageIndex,
					records = totalRecords,
					rows = (
								 from e in data.AsEnumerable()
								 select new
								 {
									 id = 1,
									 cell = SetQuestionnaireColumns(e)
								 }
							 ).ToArray()
				};

				return Json(jsonData, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
				throw;
			}
		}

		/// <summary>
		/// To export the questionnaire data.
		/// </summary>																
		/// <param name="id">The upload Id.</param>
		/// <returns></returns>
		public ActionResult ExportQuestionnaireData(int id)
		{
			var data = PatService.GetReviewQuestionnaireData(id);
			var csvString = data == null ? string.Empty : PatService.ConvertQuestionnaireDataToCsv(data);

			Response.AddHeader("Content-Disposition", string.Format("attachment; filename=QuestionnaireData-{0}-{1}.csv", id, User.Identity.Name.RemoveDomain()));
			return new ContentResult
				{
					Content = csvString,
					ContentEncoding = Encoding.UTF8,
					ContentType = "text/csv"
				};
		}

		#region

		private ActionResult RedirectToPreviousAction()
		{
			var uploadID = AppHelper.GetSessionUploadId(Session);
			if (uploadID > 0)
			{
				return RedirectToAction("Details", "Upload", new {Id = uploadID});
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

		private ActionResult RedirectToInvalidRequest()
		{
			// DR01039408 
			// Button only displays for reviews that are attached to Project Types of 'PA National' 'PA State' & Special Claim Type'
			TempData[CommonConstants.FlashMessageTypeWarning] = @"Your request has invalid 'Project Type'. Please try again later.";
			return RedirectToPreviousAction();
		}

		private static List<string> SetQuestionnaireColumns(ReviewQuestionnaire e)
		{
			var cell = new List<string>
				{
					string.Format("{0}", e.QuestionnaireID),
					// this tabindex to make the tag able to receive tab key
					string.Format("<a tabindex='0'>{0}</a>", e.ReviewID),
					string.Format("{0}", e.UserID),
					string.Format("{0}", e.AssessmentOutcomeCode),
					string.Format("{0}", e.RecoveryReasonCode),
					string.Format("{0}", e.RecoveryActionCode),
					string.Format("{0}", e.FinalOutcomeCode),
					string.Format("{0}", e.Date)
				};

			if (e.QuestionAnswers != null)
			{
				cell.AddRange(e.QuestionAnswers.Select(qa => string.Format("{0}", qa.AnswerText)));
			}

			return cell;
		}

		private bool CanEdit
		{
			get
			{
				// if user is admin then it's fine otherwise to help on the development I use the `even number of minute' to allow the access
				return AppHelper.IsAdministrator(User.Identity) || DebugHelper.IsTemporaryAdmin;
			}
		}

		#endregion
	}
}