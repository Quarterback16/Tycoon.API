using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Infrastructure.Controllers;
using ProgramAssuranceTool.Infrastructure.Security;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Models;
using System.IO;
using System.Web.Mvc;

namespace ProgramAssuranceTool.Controllers
{
	/// <summary>
	/// Attachment Controller
	/// </summary>
	[CustomAuthorize]
	public class AttachmentController : InfrastructureController
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AttachmentController"/> class.
		/// </summary>
		/// <param name="commonDependencies">The common dependencies.</param>
		public AttachmentController(
			 IControllerDependencies commonDependencies
			 )
			: base(commonDependencies)
		{
		}

		/// <summary>
		/// Display the Edit attachment screen
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		public ActionResult Edit(int id)
		{
			var model = PatService.GetAttachment(id) ?? new ProjectAttachment();
			model.ProjectId = AppHelper.GetSessionProjectId(Session);
			model.ProjectName = AppHelper.GetSessionProjectName(Session);	// keep the project name intact

			if (model.ProjectId < 1)
			{
				// this case could be caused by session time
				return RedirectToProjectList();
			}

			ViewBag.Disabled = PatService.GetProject(model.ProjectId).CanAddAttachment(User.Identity.Name.RemoveDomain()) ? CommonConstants.EnabledElement : CommonConstants.DisabledElement;
			return View(model);
		}

		/// <summary>
		/// Save/Delete  the specified attachment.
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <param name="command">The command e.g. Save or Delete.</param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult Edit(ProjectAttachment viewModel, string command = CommonConstants.ButtonUndefined)
		{
			if (!PatService.GetProject(viewModel.ProjectId).CanAddAttachment(User.Identity.Name.RemoveDomain())) { return RedirectToNoAccessAction(viewModel.ProjectId, CommonConstants.ProjectTab_Documents); }

			switch (command)
			{
				case CommonConstants.ButtonSave:
					return Save(viewModel);

				case CommonConstants.ButtonDelete:
					return Delete(viewModel.Id);

				default:
					AddErrorMessage(string.Format("Button: {0} is not defined. Please refresh your browser and try again.", command));
					return View(viewModel);
			}
		}

		/// <summary>
		/// Downloads the specified attachment.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		public ActionResult Download(int id)
		{
			// check if attachment still exists
			var model = PatService.GetAttachment(id);
			if (model != null)
			{
				var attachmentByte = PatService.RetrieveAttachment(id);
				if (attachmentByte != null)
				{
					// set the content type based on the file name extension
					var contentType = string.Format("*/{0}", Path.GetExtension(model.Url));

					//dehydrate the binary into a file and then stream it to the client
					return File(attachmentByte, contentType, model.Url);
				}
				// set default to error
				TempData[CommonConstants.FlashMessageTypeWarning] = string.Format(@"Attachment: {0} is having a null byte. Please try again.", id);
			}
			else
			{
				// set default to error
				TempData[CommonConstants.FlashMessageTypeWarning] = string.Format(@"Attachment: {0} is not found. Please try again.", id);
			}

			var projectId = AppHelper.GetSessionProjectId(Session);
			return RedirectToProjectDetailsPageTab(projectId, CommonConstants.ProjectTab_Documents);
		}


		#region Private method

		private ActionResult RedirectToProjectList()
		{
			TempData[CommonConstants.FlashMessageTypeWarning] = @"Your session has timeout, Please select again your Project.";
			return RedirectToAction("Index", "Project");
		}

		private ActionResult RedirectToNoAccessAction(int projectId, int tabNo)
		{
			TempData[CommonConstants.FlashMessageTypeWarning] = @"You have no access to this feature. Please contact your administrator.";
			return RedirectToProjectDetailsPageTab(projectId, tabNo);
		}

		private ActionResult RedirectToProjectDetailsPageTab(int projectId, int tabNo)
		{
			if (projectId < 1)
			{
				return RedirectToProjectList();
			}
			return RedirectToAction("Details", "Project", new {id = projectId, tabNo});
		}

		private ActionResult Save(ProjectAttachment viewModel)
		{
			if (ModelState.IsValid)
			{
				// remove the path only get the file name
				viewModel.Url = Path.GetFileName(viewModel.Url);

				// save the attachment only applicable for uploading a new file (attachment is not ammendable) !!!
				byte[] attachmentInBytes = null;
				if (viewModel.Attachment != null && viewModel.Attachment.ContentLength > 0 && viewModel.Attachment.InputStream != null)
				{
					attachmentInBytes = viewModel.Attachment.InputStream.ToByteArray();

					// overwrite the url with the file name only (exclude the path)
					viewModel.Url = Path.GetFileName(viewModel.Attachment.FileName);
				}

				viewModel.CreatedBy = User.Identity.Name.RemoveDomain();
				viewModel.UpdatedBy = User.Identity.Name.RemoveDomain();
				PatService.SaveAttachment(viewModel, attachmentInBytes);
				TempData[CommonConstants.FlashMessageTypeInfo] = string.Format(@"Attachment uploaded/ saved successfully: {0} - {1}.", viewModel.Id, viewModel.DocumentName);
				return RedirectToProjectDetailsPageTab(viewModel.ProjectId, CommonConstants.ProjectTab_Documents);
			}
			return View("Edit", viewModel);
		}

		private ActionResult Delete(int id)
		{
			// check if attachment still exists
			var model = PatService.GetAttachment(id);

			if (model != null)
			{
				PatService.DeleteAttachment(id);

				var activity = string.Format(@"Attachment deleted successfully: {0} - {1}.", model.Id, model.DocumentName);
				PatService.SaveActivity(activity, User.Identity.Name.RemoveDomain());

				TempData[CommonConstants.FlashMessageTypeInfo] = activity;
			}
			else
			{
				TempData[CommonConstants.FlashMessageTypeWarning] = string.Format(@"Attachment: {0} is not found. Please try again.", id);
			}

			var projectId = AppHelper.GetSessionProjectId(Session);
			return RedirectToProjectDetailsPageTab(projectId, CommonConstants.ProjectTab_Documents);

		}

		#endregion

	}
}