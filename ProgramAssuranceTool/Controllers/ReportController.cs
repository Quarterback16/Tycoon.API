using DataDynamics.ActiveReports;
using DataDynamics.ActiveReports.DataSources;
using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Infrastructure.Controllers;
using ProgramAssuranceTool.Infrastructure.Interfaces;
using ProgramAssuranceTool.Infrastructure.Security;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Reports;
using ProgramAssuranceTool.ViewModels.Report;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Xml;
using GridSettings = MvcJqGrid.GridSettings;

namespace ProgramAssuranceTool.Controllers
{
	/// <summary>
	/// Report Controller
	/// </summary>
	[CustomAuthorize]
	public class ReportController : InfrastructureController
	{
		private readonly IReportService _reportService;

		/// <summary>
		/// Initializes a new instance of the <see cref="ReportController"/> class.
		/// </summary>
		/// <param name="controllerDependencies">The controller dependencies.</param>
		/// <param name="reportService">The report service.</param>
		public ReportController(IControllerDependencies controllerDependencies,
						 IReportService reportService)
			: base(controllerDependencies)
		{
			_reportService = reportService;
		}

		/// <summary>
		/// Index Action
		/// </summary>
		/// <returns></returns>
		public ActionResult Index()
		{
			return View();
		}

		#region *** Generic stuff

		private T GetValues<T>(T viewModel) where T : SearchCriteriaViewModel
		{
			if (!viewModel.IsAdvanceSearchType)
			{
				// ** to search by: OrgCode, EsaCode, SiteCode etc...

				// get the code only
				if (!string.IsNullOrWhiteSpace(viewModel.OrgCode))
				{
					viewModel.OrgCode = viewModel.OrgCode.GetCode();
				}

				// get the code only
				if (!string.IsNullOrWhiteSpace(viewModel.ESACode))
				{
					viewModel.ESACode = viewModel.ESACode.GetCode();
				}

				// get the code only
				if (!string.IsNullOrWhiteSpace(viewModel.SiteCode))
				{
					viewModel.SiteCode = viewModel.SiteCode.GetCode();
				}

				// must nullify these values so it can be ignored by stored proc
				viewModel.ProjectID = null;
				viewModel.ProjectType = null;
			}
			else
			{
				// ** Advance search type
				// ** to search by: ProjectID, ProjectType

				// get the code only
				if (!string.IsNullOrWhiteSpace(viewModel.ProjectID))
				{
					viewModel.ProjectID = viewModel.ProjectID.GetCode();
				}

				// must nullify these values so it can be ignored by stored proc
				viewModel.OrgCode = null;
				viewModel.ESACode = null;
				viewModel.SiteCode = null;
			}

			if (viewModel.UploadDateTo != null)
			{
				viewModel.UploadDateTo = viewModel.UploadDateTo.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
			}

			return viewModel;
		}

		private List<SelectListItem> GetSortBy()
		{
			var sortColumns = new List<SelectListItem>
                {
                    new SelectListItem {Value = "ASC", Text  = "Ascending"}, 
                    new SelectListItem {Value = "DESC", Text  = "Descending"}, 
                };

			return sortColumns;
		}

		private XMLDataSource CreateXmlDataSource<T>(T model)
		{
			var xmlDocument = new XmlDocument();

			using (var stringwriter = new StringWriter())
			{
				var serializer = new System.Xml.Serialization.XmlSerializer(model.GetType());
				serializer.Serialize(stringwriter, model);

				xmlDocument.LoadXml(stringwriter.ToString());
			}

			var xmlDataSource = new XMLDataSource();
			xmlDataSource.LoadXML(xmlDocument.InnerXml);
			xmlDataSource.FileURL = ".";
			if (xmlDocument.DocumentElement != null)
			{
				xmlDataSource.RecordsetPattern = xmlDocument.DocumentElement.Name;
			}
			return xmlDataSource;
		}

		private ActionResult StreamReport(string reportType, ActiveReport report, string reportTitle)
		{
			FileStreamResult result = null;

			switch (reportType)
			{
				case CommonConstants.Pdf:
					result = _reportService.ToPdfResult(report, reportTitle);
					break;

				case CommonConstants.Word:
					result = _reportService.ToRtfResult(report, reportTitle);
					break;

				case CommonConstants.Csv:
					result = _reportService.ToTextResult(report, reportTitle);
					break;
			}
			return result;
		}

		#endregion

		#region *** Compliance Risk Indicator  Report
		/// <summary>
		/// To display Compliance Risk Indicator Report Search Criteria
		/// </summary>
		/// <returns></returns>
		public ActionResult Compliance()
		{
			var viewModel = new SearchCriteriaViewModel();

			ViewBag.SortColumnList = PatService.GetComplianceRiskReportSortOrder();
			ViewBag.SortByList = GetSortBy();
			ViewBag.ModelIsValid = false;

			return View(viewModel);
		}

		/// <summary>
		/// To Search Compliance Indicator Report based on Search Criteria
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult Compliance(SearchCriteriaViewModel viewModel)
		{
			ViewBag.ModelIsValid = ModelState.IsValid;

			// re-populate the list
			ViewBag.SortColumnList = PatService.GetComplianceRiskReportSortOrder();
			ViewBag.SortByList = GetSortBy();

			if (ModelState.IsValid)
			{
				// pass the projet type as part of search criteria
				var projecType = viewModel.ProjectType;
				var searchCriteria = GetValues(viewModel);
				searchCriteria.ProjectType = projecType;

				viewModel.TotalRecords = PatService.CountComplianceRiskIndicatorReport(searchCriteria);
			}

			return View(viewModel);
		}

		/// <summary>
		/// To Get the data of Compliance Indicator Report
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public ActionResult ComplianceGetData(GridSettings gridSettings, SearchCriteriaViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				if (string.IsNullOrEmpty(gridSettings.SortColumn))
				{
					gridSettings.SortOrder = "DESC";
				}

				try
				{
					// pass the projet type as part of search criteria
					var projecType = viewModel.ProjectType;
					var searchCriteria = GetValues(viewModel);
					searchCriteria.ProjectType = projecType;

					var data = PatService.GetComplianceRiskIndicatorReport(gridSettings, searchCriteria);
					if (data != null)
					{
						var totalRecords = viewModel.TotalRecords;	// PatService.CountComplianceRiskIndicatorReport(searchCriteria);

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
								                           e.OrgCode,
								                           e.ESACode,
								                           e.SiteCode,
																	e.ProjectType,
								                           string.Format("{0}", e.TotalCompliancePoint),
								                           string.Format("{0}", e.ComplianceIndicator),
								                           string.Format("{0}", e.InProgressReviewCount),
								                           string.Format("{0}", e.CompletedReviewCount),
								                           string.Format("{0}", e.TotalReviewCount),
								                           string.Format("{0}", e.TotalRecoveryAmount),
								                           string.Format("{0}", e.ValidCount),
								                           string.Format("{0}", e.ValidAdminCount),
								                           string.Format("{0}", e.InvalidAdminCount),
								                           string.Format("{0}", e.InvalidRecovery),
								                           string.Format("{0}", e.InvalidNoRecovery)
							                           }
												 }
										 ).ToArray()
							};

						return Json(jsonData, JsonRequestBehavior.AllowGet);
					}
				}
				catch (Exception ex)
				{
					Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
					throw;
				}
			}

			return null;
		}

		/// <summary>
		/// To download Compliance Indicator Report 
		/// </summary>
		/// <param name="reportType">Type of the report.</param>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public ActionResult ComplianceDownload(string reportType, SearchCriteriaViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				// pass the projet type as part of search criteria
				var projecType = viewModel.ProjectType;
				var searchCriteria = GetValues(viewModel);
				searchCriteria.ProjectType = projecType;

				var data = PatService.GetComplianceRiskIndicatorReport(searchCriteria);

				var xmlDataSource = CreateXmlDataSource(data);

				var report = _reportService.Create<ComplianceReport>(xmlDataSource);

				var reportTitle = string.Format("{0}-{1}", "ComplianceReport", User.Identity.Name.RemoveDomain());

				return StreamReport(reportType, report, reportTitle);
			}

			// for some reason the model state is invalid, so we just send it back to the page
			return RedirectToAction("Compliance");
		}
		#endregion

		#region *** SiteVisit Report
		/// <summary>
		/// Go display the Site Visit Report Search Criteria
		/// </summary>
		/// <returns></returns>
		public ActionResult SiteVisit()
		{
			var viewModel = new SearchCriteriaViewModel();

			ViewBag.SortColumnList = PatService.GetSiteVisitReportSortOrder();
			ViewBag.SortByList = GetSortBy();
			ViewBag.ModelIsValid = false;

			return View(viewModel);
		}

		/// <summary>
		/// To Search for the Site Visit Report.
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult SiteVisit(SearchCriteriaViewModel viewModel)
		{
			ViewBag.ModelIsValid = ModelState.IsValid;

			// re-populate the list
			ViewBag.SortColumnList = PatService.GetSiteVisitReportSortOrder();
			ViewBag.SortByList = GetSortBy();

			if (ModelState.IsValid)
			{
				viewModel.TotalRecords = PatService.CountSiteVisitReport(GetValues(viewModel));
			}

			return View(viewModel);
		}

		/// <summary>
		/// To Geth the Site Visit Report
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public ActionResult SiteVisitGetData(GridSettings gridSettings, SearchCriteriaViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				if (string.IsNullOrEmpty(gridSettings.SortColumn))
				{
					gridSettings.SortOrder = "DESC";
				}

				try
				{
					var data = PatService.GetSiteVisitReport(gridSettings, GetValues(viewModel));
					if (data != null)
					{
						var totalRecords = viewModel.TotalRecords;	// PatService.CountSiteVisitReport(GetValues(viewModel));

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
                                                           e.OrgCode,
                                                           e.OrgName,
                                                           e.ESACode,
                                                           e.ESAName,
                                                           e.SiteCode,
                                                           e.SiteName,
                                                           string.Format("{0}", e.ProjectID),
                                                           e.ProjectName,
                                                           string.Format("{0}", e.JobSeekerID),
                                                           e.JobSeekerFirstName,
                                                           e.JobSeekerFamilyName,
                                                           string.Format("{0}", e.ClaimID),
                                                           e.ClaimType,
                                                           e.ClaimTypeDescription,
                                                           string.Format("{0}", e.ClaimAmount),
                                                           string.Format("{0}", e.ClaimCreationDate.ToShortDateString()),
                                                           e.ContractType,
                                                           string.Format("{0}", e.DaysOverdue),
                                                           e.AssessmentAction,
                                                           e.AssessmentOutcome,
                                                           string.Format("{0}", e.FinalOutcome),
                                                           string.Format("{0}", e.LastUpdateDate.ToShortDateString()),
                                                           e.RecoveryReason,
                                                           e.ReviewStatus,
                                                           string.Format("{0}", e.UploadDate.ToShortDateString())                                                       
																		 }
													 }
										  ).ToArray()
							 };

						return Json(jsonData, JsonRequestBehavior.AllowGet);
					}
				}
				catch (Exception ex)
				{
					Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
					throw;
				}
			}

			return null;
		}

		/// <summary>
		/// To Download the Site Visit Report
		/// </summary>
		/// <param name="reportType">Type of the report.</param>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public ActionResult SiteVisitDownload(string reportType, SearchCriteriaViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var data = PatService.GetSiteVisitReport(GetValues(viewModel));

				var xmlDataSource = CreateXmlDataSource(data);

				var report = _reportService.Create<SiteVisitReport>(xmlDataSource);

				var reportTitle = string.Format("{0}-{1}", "SiteVisitReport", User.Identity.Name.RemoveDomain());

				return StreamReport(reportType, report, reportTitle);
			}

			// for some reason the model state is invalid, so we just send it back to the page
			return RedirectToAction("SiteVisit");
		}
		#endregion

		#region *** Provider Summary Report
		/// <summary>
		/// To Display the Search Criteria of Provider Summary Report
		/// </summary>
		/// <returns></returns>
		public ActionResult ProviderSummary()
		{
			var viewModel = new SearchCriteriaViewModel();

			ViewBag.SortColumnList = PatService.GetProviderSummaryReportSortOrder();
			ViewBag.SortByList = GetSortBy();
			ViewBag.ModelIsValid = false;

			return View(viewModel);
		}

		/// <summary>
		/// To Search the Provider Summary Report
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult ProviderSummary(SearchCriteriaViewModel viewModel)
		{
			ViewBag.ModelIsValid = ModelState.IsValid;

			// re-populate the list
			ViewBag.SortColumnList = PatService.GetProviderSummaryReportSortOrder();
			ViewBag.SortByList = GetSortBy();

			if (ModelState.IsValid)
			{
				viewModel.TotalRecords = PatService.CountProviderSummaryReport(GetValues(viewModel));
			}

			return View(viewModel);
		}

		/// <summary>
		/// To get the Provider Summary Report data.
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public ActionResult ProviderSummaryGetData(GridSettings gridSettings, SearchCriteriaViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				if (string.IsNullOrEmpty(gridSettings.SortColumn))
				{
					gridSettings.SortOrder = "DESC";
				}

				try
				{
					var data = PatService.GetProviderSummaryReport(gridSettings, GetValues(viewModel));
					if (data != null)
					{
						var totalRecords = viewModel.TotalRecords;	// PatService.CountProviderSummaryReport(GetValues(viewModel));

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
                                                           e.OrgCode,
                                                           e.ESACode,
                                                           e.SiteCode,
                                                           e.State,
                                                           string.Format("{0}", e.TotalReviewCount),
                                                           string.Format("{0}", e.CompletedReviewCount),
                                                           string.Format("{0}", e.RecoveryCount),
                                                           string.Format("{0}", e.ValidCount),
                                                           string.Format("{0}", e.ValidAdminCount),
                                                           string.Format("{0}", e.InvalidAdminCount),
                                                           string.Format("{0}", e.InvalidRecovery),
                                                           string.Format("{0}", e.InvalidNoRecovery)
                                                       }
													 }
										  ).ToArray()
							 };

						return Json(jsonData, JsonRequestBehavior.AllowGet);
					}
				}
				catch (Exception ex)
				{
					Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
					throw;
				}
			}

			return null;
		}

		/// <summary>
		/// To download Provider Summary Report.
		/// </summary>
		/// <param name="reportType">Type of the report.</param>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public ActionResult ProviderSummaryDownload(string reportType, SearchCriteriaViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var data = PatService.GetProviderSummaryReport(GetValues(viewModel));

				var xmlDataSource = CreateXmlDataSource(data);

				var report = _reportService.Create<ProviderSummaryReport>(xmlDataSource);

				var reportTitle = string.Format("{0}-{1}", "ProviderSummaryReport", User.Identity.Name.RemoveDomain());

				return StreamReport(reportType, report, reportTitle);
			}

			// for some reason the model state is invalid, so we just send it back to the page
			return RedirectToAction("ProviderSummary");
		}
		#endregion

		#region *** Progress Report
		/// <summary>
		/// To display the Progress Report Search Criteria.
		/// </summary>
		/// <returns></returns>
		public ActionResult Progress()
		{
			var viewModel = new SearchCriteriaViewModel();

			ViewBag.SortColumnList = PatService.GetProgressReportSortOrder();
			ViewBag.SortByList = GetSortBy();
			ViewBag.ModelIsValid = false;

			return View(viewModel);
		}

		/// <summary>
		/// To Search Progress Report .
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult Progress(SearchCriteriaViewModel viewModel)
		{
			ViewBag.ModelIsValid = ModelState.IsValid;

			// re-populate the list
			ViewBag.SortColumnList = PatService.GetProgressReportSortOrder();
			ViewBag.SortByList = GetSortBy();

			if (ModelState.IsValid)
			{
				viewModel.TotalRecords= PatService.CountProgressReport(GetValues(viewModel));
			}

			return View(viewModel);
		}

		/// <summary>
		/// To get the Progress Report data.
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public ActionResult ProgressGetData(GridSettings gridSettings, SearchCriteriaViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				if (string.IsNullOrEmpty(gridSettings.SortColumn))
				{
					gridSettings.SortOrder = "DESC";
				}

				try
				{
					var data = PatService.GetProgressReport(gridSettings, GetValues(viewModel));
					if (data != null)
					{
						var totalRecords = viewModel.TotalRecords;	// PatService.CountProgressReport(GetValues(viewModel));

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
                                                           e.SampleName,
                                                           e.ProjectType,
                                                           string.Format("{0}", e.InProgressReviewCount),
                                                           string.Format("{0}", e.CompletedReviewCount),
                                                           string.Format("{0}", e.TotalReviewCount),
                                                           string.Format("{0}", e.PercentCompleted),
                                                           string.Format("{0}", e.LastUpdateDate.ToShortDateString())
                                                       }
													 }
										  ).ToArray()
							 };

						return Json(jsonData, JsonRequestBehavior.AllowGet);
					}
				}
				catch (Exception ex)
				{
					Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
					throw;
				}
			}

			return null;
		}

		/// <summary>
		/// To Download Progress report 
		/// </summary>
		/// <param name="reportType">Type of the report.</param>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public ActionResult ProgressDownload(string reportType, SearchCriteriaViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var data = PatService.GetProgressReport(GetValues(viewModel));

				var xmlDataSource = CreateXmlDataSource(data);

				var report = _reportService.Create<ProgressReport>(xmlDataSource);

				var reportTitle = string.Format("{0}-{1}", "ProgressReport", User.Identity.Name.RemoveDomain());

				return StreamReport(reportType, report, reportTitle);
			}

			// for some reason the model state is invalid, so we just send it back to the page
			return RedirectToAction("Progress");
		}
		#endregion

		#region *** Project Type
		/// <summary>
		/// To display the Project type Report search criteria
		/// </summary>
		/// <returns></returns>
		public ActionResult ProjectType()
		{
			var viewModel = new SearchCriteriaViewModel();

			ViewBag.ModelIsValid = false;
			ViewBag.HaveData = false;

			return View(viewModel);
		}

		/// <summary>
		/// To search Project Type Report
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult ProjectType(SearchCriteriaViewModel viewModel)
		{
			ViewBag.ModelIsValid = ModelState.IsValid;

			if (ModelState.IsValid)
			{
				var data = PatService.GetProjectTypeReport(GetValues(viewModel));
				ViewBag.HaveData = (data != null && (
																	 (data.ProjectByOrganisation != null && data.ProjectByOrganisation.Any()) ||
																	 (data.ProjectByType != null && data.ProjectByType.Any()) ||
																	 (data.ProjectByESA != null && data.ProjectByESA.Any()) ||
																	 (data.ProjectByState != null && data.ProjectByState.Any()) ||
																	 (data.ProjectByNational != null && data.ProjectByNational.Any())
																	 )
														 );
			}

			return View(viewModel);
		}

		/// <summary>
		/// To get data of Projects Type report.
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="projectTypeBy">The project type by.</param>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public ActionResult ProjectTypeGetData(GridSettings gridSettings, string projectTypeBy, SearchCriteriaViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				if (string.IsNullOrEmpty(gridSettings.SortColumn))
				{
					gridSettings.SortOrder = "DESC";
				}

				try
				{
					var projectType = PatService.GetProjectTypeReport(GetValues(viewModel));

					var data = PatService.GetProjectTypeBy(projectTypeBy, projectType);
					if (data != null)
					{
						var totalRecords = data.Count();

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
                                                           e.Type,
                                                           e.Code,
                                                           e.Description,
                                                           string.Format("{0}", e.ProjectCount),
                                                           string.Format("{0}", e.ReviewCount)
                                                       }
													 }
										  ).ToArray()
							 };

						return Json(jsonData, JsonRequestBehavior.AllowGet);
					}
				}
				catch (Exception ex)
				{
					Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
					throw;
				}
			}

			return null;
		}

		/// <summary>
		/// To download Project Type report
		/// </summary>
		/// <param name="reportType">Type of the report.</param>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public ActionResult ProjectTypeDownload(string reportType, SearchCriteriaViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var data = PatService.GetProjectTypeReport(GetValues(viewModel));

				var xmlDataSource = CreateXmlDataSource(data);

				var report = _reportService.Create<ProjectTypeReport>(xmlDataSource);

				var reportTitle = string.Format("{0}-{1}", "ProjectTypeReport", User.Identity.Name.RemoveDomain());

				return StreamReport(reportType, report, reportTitle);
			}

			// for some reason the model state is invalid, so we just send it back to the page
			return RedirectToAction("ProjectType");
		}
		#endregion

		#region *** Finding Summary
		/// <summary>
		/// To display Findings Summary Report Search Criteria.
		/// </summary>
		/// <returns></returns>
		public ActionResult FindingSummary()
		{
			var viewModel = new SearchCriteriaViewModel();

			ViewBag.ModelIsValid = false;

			return View(viewModel);
		}

		/// <summary>
		/// To search Finding Summary Report
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult FindingSummary(SearchCriteriaViewModel viewModel)
		{
			ViewBag.ModelIsValid = ModelState.IsValid;

			return View(viewModel);
		}

		/// <summary>
		/// To get Finding Summary report data.
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="findingSummaryType">Type of the finding summary.</param>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public ActionResult FindingSummaryGetData(GridSettings gridSettings, string findingSummaryType, SearchCriteriaViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				if (string.IsNullOrEmpty(gridSettings.SortColumn))
				{
					gridSettings.SortOrder = "DESC";
				}

				try
				{
					var findingSummary = PatService.GetFindingSummaryReport(GetValues(viewModel));

					var data = PatService.GetFindingSummaryType(findingSummaryType, findingSummary);
					if (data != null)
					{
						var totalRecords = data.Count();

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
                                                           e.Type,
                                                           e.Code,
                                                           e.Description,
                                                           string.Format("{0}", e.ReviewCount)
                                                       }
													 }
										  ).ToArray()
							 };

						return Json(jsonData, JsonRequestBehavior.AllowGet);
					}
				}
				catch (Exception ex)
				{
					Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
					throw;
				}
			}

			return null;
		}

		/// <summary>
		/// To download Finding Summary report.
		/// </summary>
		/// <param name="reportType">Type of the report.</param>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public ActionResult FindingSummaryDownload(string reportType, SearchCriteriaViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var data = PatService.GetFindingSummaryReport(GetValues(viewModel));

				var xmlDataSource = CreateXmlDataSource(data);

				var report = _reportService.Create<FindingSummaryReport>(xmlDataSource);

				var reportTitle = string.Format("{0}-{1}", "FindingSummaryReport", User.Identity.Name.RemoveDomain());

				return StreamReport(reportType, report, reportTitle);
			}

			// for some reason the model state is invalid, so we just send it back to the page
			return RedirectToAction("FindingSummary");
		}
		#endregion

		#region *** Dashboard
		/// <summary>
		/// To display Dashboards Report Search Criteria.
		/// </summary>
		/// <returns></returns>
		public ActionResult Dashboard()
		{
			var viewModel = new SearchCriteriaViewModel();

			ViewBag.ModelIsValid = false;

			return View(viewModel);
		}

		/// <summary>
		/// To search Dashboard Report as specified in the search criteria .
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult Dashboard(SearchCriteriaViewModel viewModel)
		{
			ViewBag.ModelIsValid = ModelState.IsValid;

			/* Dashboard chart is no longer applied
			if (ModelState.IsValid)
			{
				var data = PatService.GetDashboardReport(GetValues(viewModel));
				GenerateDashboardPieChart(data);
			}
			*/

			return View(viewModel);
		}

		/// <summary>
		/// To get Dashboard Report data.
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public ActionResult DashboardGetData(GridSettings gridSettings, SearchCriteriaViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				if (string.IsNullOrEmpty(gridSettings.SortColumn))
				{
					gridSettings.SortOrder = "DESC";
				}

				try
				{
					var data = PatService.GetDashboardReport(GetValues(viewModel));
					if (data != null)
					{
						var totalRecords = data.Count();

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
                                                           e.OutcomeCode,
                                                           e.OutcomeDescription,
                                                           string.Format("{0}", e.ReviewCount)
                                                       }
													 }
										  ).ToArray()
							 };

						return Json(jsonData, JsonRequestBehavior.AllowGet);
					}
				}
				catch (Exception ex)
				{
					Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
					throw;
				}
			}

			return null;
		}

		/// <summary>
		/// To download Dashboard Report
		/// </summary>
		/// <param name="reportType">Type of the report.</param>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public ActionResult DashboardDownload(string reportType, SearchCriteriaViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var data = PatService.GetDashboardReport(GetValues(viewModel));

				var xmlDataSource = CreateXmlDataSource(data);

				var report = _reportService.Create<DashboardReport>(xmlDataSource);

				var reportTitle = string.Format("{0}-{1}", "DashboardReport", User.Identity.Name.RemoveDomain());

				return StreamReport(reportType, report, reportTitle);
			}

			// for some reason the model state is invalid, so we just send it back to the page
			return RedirectToAction("Dashboard");
		}

		/*
		public ActionResult GetDashboardPieChart()
		{
			var imagePath = string.Format("{0}", TempData["ImagePath"]);
			if (string.IsNullOrWhiteSpace(imagePath))
			{
				imagePath = "/Report/No_Data_Found";
			}
			return File(imagePath, "image/jpeg");
		}

		/// <summary>
		/// Not in used at the moment, because the chart created by this is very basic and not so pretty compare to google chart.
		/// </summary>
		/// <param name="data"></param>
		private void GenerateDashboardPieChart(List<Dashboard> data)
		{
			var total = data.Sum(x => x.ReviewCount);

			var list = new List<string>();
			foreach (var x in data)
			{
				decimal value = 0;
				if (total != 0)
				{
					value = (decimal)x.ReviewCount / total * 100;
				}

				var formatted = string.Empty;

				if (value > 0)
				{
					formatted = string.Format("{0:f2} %", value);
				}
				list.Add(formatted);

			}

			var xStrings = list.ToArray();

			var myChart = new Chart(width: 600, height: 400)
				 .AddTitle("Outcome and Number of reviews")
				 .AddSeries(
					  name: "Dashboard",
					  chartType: "pie",
					  xValue: xStrings,
					  yValues: data.Select(x => x.ReviewCount).ToArray()
					  ).AddLegend("Legend");

			var imagePath = Path.GetTempFileName();
			myChart.Save(imagePath);

			TempData["ImagePath"] = imagePath;
		}
		*/

		#endregion

		/// <summary>
		/// Lookups the ESA by.
		/// </summary>
		/// <param name="searchText">The search text.</param>
		/// <param name="orgCode">The org code.</param>
		/// <param name="maxResults">The maximum results.</param>
		/// <returns></returns>
		[HttpPost] //  helps prevent scripting attacks or data exposure from direct JSON queries
		public JsonResult LookupESABy(string searchText, string orgCode, int maxResults)
		{
			var codeToSearch = searchText.GetCode();
			var result = PatService.LookupESABy(codeToSearch, orgCode, maxResults);
			if (result == null || result.Count < 1)
			{
				result = new List<SelectListItem>
					{
						new SelectListItem {Value = codeToSearch, Text = "Data not found, please try again !"}
					};
			}
			return Json(result);
		}

		/// <summary>
		/// Lookups the site by.
		/// </summary>
		/// <param name="searchText">The search text.</param>
		/// <param name="orgCode">The org code.</param>
		/// <param name="esaCode">The esa code.</param>
		/// <param name="maxResults">The maximum results.</param>
		/// <returns></returns>
		[HttpPost] //  helps prevent scripting attacks or data exposure from direct JSON queries
		public JsonResult LookupSiteBy(string searchText, string orgCode, string esaCode, int maxResults)
		{
			var codeToSearch = searchText.GetCode();
			var result = PatService.LookupSiteBy(codeToSearch, orgCode, esaCode, maxResults);
			if (result == null || result.Count < 1)
			{
				result = new List<SelectListItem>
					{
						new SelectListItem {Value = codeToSearch, Text = "Data not found, please try again !"}
					};
			}
			return Json(result);
		}

	}
}
