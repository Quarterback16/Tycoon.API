using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Infrastructure.Controllers;
using ProgramAssuranceTool.Infrastructure.Security;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Models;
using System;
using System.Web.Mvc;

namespace ProgramAssuranceTool.Controllers
{
	/// <summary>
	/// Bulletin Controller
	/// </summary>
	[CustomAuthorize]
	public class BulletinController : InfrastructureController
	{
		private const int FourWeeks = 28;

		/// <summary>
		/// Initializes a new instance of the <see cref="BulletinController"/> class.
		/// </summary>
		/// <param name="controllerDependencies">Dependencies holder.</param>
		public BulletinController(IControllerDependencies controllerDependencies)
			: base(controllerDependencies)
		{
		}

		/// <summary>
		/// To view the Bulletin, Every PAT user can view detail bulletin.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="bulletinType">Type of the bulletin. e.g. STD or FAQ</param>
		/// <returns></returns>
		[NonAction]
		public ActionResult Details(int id, string bulletinType)
		{
			ViewData["BulletinType"] = bulletinType;
			ViewData["SelectedBulletinID"] = id;

			// this is a setting to retrieve all records
			var gridSettings = new MvcJqGrid.GridSettings
			{
				IsSearch = false,
				PageSize = 99999999,
				PageIndex = 1,
				SortColumn = "EndDate",
				SortOrder = DataConstants.Descending
			};

			var model = PatService.GetBulletins(gridSettings, bulletinType, CanEdit);

			// for each bulletin, populate the project name
			model.ForEach(b =>
				{
					if (b.ProjectId > 0)
					{
						var project = PatService.GetProject(b.ProjectId);
						if (project != null && !string.IsNullOrWhiteSpace(project.ProjectName))
						{
							b.ProjectField = string.Format("{0} - {1}", b.ProjectId, project.ProjectName);
						}
						else
						{
							b.ProjectField = string.Format("{0} - {1}", b.ProjectId, "Project name is `empty`");
						}
					}
					else
					{
						// bulletin does not have related project
						b.ProjectField = string.Empty;
					}
				});

			return View(model);
		}

		/// <summary>
		/// To display the Create Bulletin screen, only Admin user can have access to this screen
		/// </summary>
		/// <param name="bulletinType">Type of the bulletin.</param>
		/// <returns></returns>
		public ActionResult Create(string bulletinType)
		{
			if (!CanEdit) { return RedirectToNoAccessAction(); }

			var model = new Bulletin
				 {
					 BulletinType = bulletinType,

					 // set default to today's date
					 StartDate = DateTime.Today,

					 // and default to 4 weeks from start date, since this is the end date then hour minute seconds must be 0
					 EndDate = DateTime.Today.AddDays(FourWeeks)
				 };

			return View(model);
		}

		/// <summary>
		/// To create bulletin, only Admin user can have access to post to this screen
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult Create(Bulletin model)
		{
			if (!CanEdit) { return RedirectToNoAccessAction(); }

			if (ModelState.IsValid)
			{
				if (!string.IsNullOrWhiteSpace(model.ProjectField))
				{
					// check if project is valid
					var project = PatService.GetProject(model.ProjectId);
					if (project == null)
					{
						AddErrorMessage("ProjectField", string.Format("Project Id: {0} not found. Please use a valid project", model.ProjectId));
						return View(model);
					}
				}

				model.CreatedBy = User.Identity.Name.RemoveDomain();
				var newID = PatService.CreateBulletin(model);
				if (newID > 0)
				{
					// DR01039391; The Bulletin/FAQ has been successfully submitted.
					TempData[CommonConstants.FlashMessageTypeInfo] = string.Format(@"The Bulletin/ FAQ has been successfully submitted: {0}", model.BulletinTitle);
					return RedirectToAction("Index", "Home", new { bulletinType = model.BulletinType });
				}
				TempData[CommonConstants.FlashMessageTypeError] = string.Format(@"The Bulletin/ FAQ submission was failed: {0}. Please try again.", model.BulletinTitle);
			}

			return View(model);
		}

		/// <summary>
		/// To display the Bulletin Edit screen, only  Admin user can have access to this screen
		/// </summary>
		/// <param name="id">Bulletin Id</param>
		/// <returns></returns>
		public ActionResult Edit(int id)
		{
			if (!CanEdit) { return RedirectToNoAccessAction(); }
			ViewBag.CanSave = CommonConstants.EnabledElement;

			var model = PatService.GetBulletin(id);
			if (model != null)
			{
				/* Admin is allowed to modify the past bulletin 
				var isPastRecord = model.EndDate < DateTime.Today;
				if (isPastRecord)
				{
					TempData[CommonConstants.FlashMessageTypeWarning] = @"This Bulletin/ FAQ has already past the End Date, you cannot modify it anymore. Please create a new one";
					ViewBag.CanSave = CommonConstants.DisabledElement;
				}
				*/

				// allow editing for: active record or future record

				// get the project description
				if (model.ProjectId > 0)
				{
					var project = PatService.GetProject(model.ProjectId);
					if (project != null)
					{
						model.ProjectField = string.Format("{0} - {1}", project.ProjectId, project.ProjectName);
					}
				}
			}
			else
			{
				AddErrorMessage(string.Format(@"The Bulletin/ FAQ record with Id: {0} is not found. Please try again.", id));
			}
			return View(model);
		}

		/// <summary>
		/// To Save/ Delete Bulletin, only  Admin user can access this screen
		/// </summary>
		/// <param name="model">bulletin view model</param>
		/// <param name="command">The command e.g. SAVE or DELETE</param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult Edit(Bulletin model, string command = CommonConstants.ButtonUndefined)
		{
			if (!CanEdit) { return RedirectToNoAccessAction(); }
			ViewBag.CanSave = CommonConstants.EnabledElement;

			switch (command.ToUpper())
			{
				case CommonConstants.ButtonSave:
					return SaveChanges(model);

				case CommonConstants.ButtonDelete:
					return Delete(model.BulletinId);

				default:
					AddErrorMessage(string.Format("Button: {0} is not defined. Please refresh your browser and try again.", command));
					return View(model);
			}
		}

		/// <summary>
		/// To display the Bulletin View screen, everyone can have access to this screen
		/// </summary>
		/// <param name="id">Bulletin Id</param>
		/// <returns></returns>
		public ActionResult View(int id)
		{
			var model = PatService.GetBulletin(id);
			if (model != null)
			{
				// get the bulletin type description from ADW
				ViewBag.BulletinTypeDescription = PatService.GetBulletinTypeDescription(model.BulletinType);

				// get the project description
				if (model.ProjectId > 0)
				{
					var project = PatService.GetProject(model.ProjectId);
					if (project != null)
					{
						model.ProjectField = string.Format("{0} - {1}", project.ProjectId, project.ProjectName);
					}
				}
			}
			else
			{
				AddErrorMessage(string.Format(@"The Bulletin/ FAQ record with Id: {0} is not found. Please try again.", id));
			}
			ViewBag.CanEdit = CanEdit;
			return View(model);
		}

		/// <summary>
		/// Get the 4 weeks date from the start date
		/// </summary>
		/// <param name="startDate">The start date.</param>
		/// <returns></returns>
		[HttpPost] //  helps prevent scripting attacks or data exposure from direct JSON queries
		public JsonResult Get4WeeksDate(string startDate)
		{
			DateTime theStartDate;
			DateTime.TryParse(startDate, out theStartDate);

			var endDate = theStartDate.AddDays(FourWeeks).ToString("dd/MM/yyyy").Trim();
			return Json(endDate);
		}

		#region

		/// <summary>
		/// Only Admin user can have save changes
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		private ActionResult SaveChanges(Bulletin model)
		{
			if (!CanEdit) { return RedirectToNoAccessAction(); }

			if (ModelState.IsValid)
			{
				var existingRecord = PatService.GetBulletin(model.BulletinId);
				if (existingRecord != null)
				{

					/* Admin is allowed to modify the past bulletin 
					var isPastRecord = model.EndDate < DateTime.Today;
					if (isPastRecord)
					{
						TempData[CommonConstants.FlashMessageTypeWarning] = @"This Bulletin/ FAQ has already past the End Date, you cannot modify it anymore. Please create a new one";
						ViewBag.CanSave = CommonConstants.DisabledElement;
					}
					else
					*/
					{
						// allow editing for: active record or future record

						if (!string.IsNullOrWhiteSpace(model.ProjectField))
						{
							// check if project is still valid 
							var project = PatService.GetProject(model.ProjectId);
							if (project == null)
							{
								AddErrorMessage("ProjectField", string.Format("Project Id: {0} not found. Please use a valid  project", model.ProjectId));
								return View("Edit", model);
							}
						}

						model.UpdatedBy = User.Identity.Name.RemoveDomain();
						PatService.UpdateBulletin(model);
						TempData[CommonConstants.FlashMessageTypeInfo] = string.Format(@"The Bulletin/ FAQ has been successfully updated: {0}", model.BulletinTitle);
						return RedirectToAction("Index", "Home", new { bulletinType = model.BulletinType });
					}
				}
				AddErrorMessage(string.Format(@"The Bulletin/ FAQ record with Id: {0} not found. Please try again.", model.BulletinId));
			}

			return View("Edit", model);
		}

		/// <summary>
		/// Only Admin user can have access to delete
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		private ActionResult Delete(int id)
		{
			if (!CanEdit) { return RedirectToNoAccessAction(); }

			// check if bulletin still exists
			var model = PatService.GetBulletin(id);
			if (model != null)
			{
				PatService.DeleteBulletin(id);

				var activity = string.Format(@"The Bulletin / FAQ has been successfully deleted: {0}", model.BulletinTitle);
				PatService.SaveActivity(activity, User.Identity.Name.RemoveDomain());

				TempData[CommonConstants.FlashMessageTypeInfo] = activity;
			}
			else
			{
				TempData[CommonConstants.FlashMessageTypeWarning] = string.Format(@"The Bulletin / FAQ record with Id: {0} is not found. Please try again.", id);
			}

			return RedirectToAction("Index", "Home");
		}

		private bool CanEdit
		{
			get
			{
				// if user is admin then it's fine otherwise to help on the development I use the `even number of minute' to allow the access
				return AppHelper.IsAdministrator(User.Identity) || DebugHelper.IsTemporaryAdmin;
			}
		}

		private ActionResult RedirectToNoAccessAction()
		{
			TempData[CommonConstants.FlashMessageTypeWarning] = @"You have no access to this feature. Please contact your administrator.";
			return RedirectToAction("Index", "Home");
		}

		#endregion

	}
}
