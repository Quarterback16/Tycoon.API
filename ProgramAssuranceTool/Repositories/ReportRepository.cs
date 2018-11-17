using System.Web.Mvc;
using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Models;
using ProgramAssuranceTool.ViewModels.Report;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using GridSettings = MvcJqGrid.GridSettings;

namespace ProgramAssuranceTool.Repositories
{
	/// <summary>
	/// Report Repository
	/// </summary>
	public class ReportRepository
	{
		private static string _dbConnection;
		public static string DbConnection
		{
			get
			{
				if (string.IsNullOrWhiteSpace(_dbConnection))
				{
					_dbConnection = ConfigurationManager.ConnectionStrings["ProgramAssuranceConnectionString"].ConnectionString;
				}
				return _dbConnection;
			}
		}

		/// <summary>
		/// Gets the compliance risk indicator report.
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public List<ComplianceRiskIndicator> GetComplianceRiskIndicatorReport(GridSettings gridSettings, SearchCriteriaViewModel viewModel)
		{
			var results = new List<ComplianceRiskIndicator>();

			using (var connection = new SqlConnection(DbConnection))
			{
				using (var command = new SqlCommand("PaReportComplianceIndicators", connection))
				{
					var sqlParams = new List<SqlParameter>();
					var paramReturnValue = new SqlParameter("@return_value", SqlDbType.Int)
						 {
							 Direction = ParameterDirection.ReturnValue
						 };

					sqlParams.Add(paramReturnValue);

					SqlHelper.AddIntPara(gridSettings.PageIndex, "@pageIndex", sqlParams);
					SqlHelper.AddIntPara(gridSettings.PageSize, "@pageSize", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.OrgCode, "@OrgCode", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ESACode, "@ESACode", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.SiteCode, "@SiteCode", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ProjectID, "@ProjectID", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ProjectType, "@ProjectType", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ContractType, "@ContractType", sqlParams);
					SqlHelper.AddDatePara(viewModel.UploadDateFrom, "@UploadDateFrom", sqlParams);
					SqlHelper.AddDatePara(viewModel.UploadDateTo, "@UploadDateTo", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.SortColumn, "@SortColumn", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.SortBy, "@SortBy", sqlParams);

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange(sqlParams.ToArray());
					connection.Open();

					SqlDataReader reader = null;

					try
					{
						reader = command.ExecuteReader();
						while (reader.Read())
						{
							var record = new ComplianceRiskIndicator
							{
								OrgCode = string.Format("{0}", reader["OrgCode"]),
								ESACode = string.Format("{0}", reader["ESACode"]),
								SiteCode = string.Format("{0}", reader["SiteCode"]),
								ProjectType= string.Format("{0}", reader["ProjectType"]),
								TotalCompliancePoint = AppHelper.ToDecimal(reader["TotalCompliancePoint"]),
								ComplianceIndicator = AppHelper.ToDecimal(reader["ComplianceIndicator"]),
								InProgressReviewCount = AppHelper.ToInt(reader["InProgressCount"]),
								CompletedReviewCount = AppHelper.ToInt(reader["CompletedCount"]),
								TotalReviewCount = AppHelper.ToInt(reader["TotalReviewsCount"]),
								TotalRecoveryAmount = AppHelper.ToDecimal(reader["TotalRecoveryAmount"]),
								ValidCount = AppHelper.ToInt(reader["OutcomeCodeVANCount"]),
								ValidAdminCount = AppHelper.ToInt(reader["OutcomeCodeVADCount"]),
								InvalidAdminCount = AppHelper.ToInt(reader["OutcomeCodeIADCount"]),
								InvalidRecovery = AppHelper.ToInt(reader["OutcomeCodeINRCount"]),
								InvalidNoRecovery = AppHelper.ToInt(reader["OutcomeCodeINNCount"])
							};

							results.Add(record);
						}
					}
					finally
					{
						if (reader != null) { reader.Close(); }
					}
				}
			}

			return results;
		}

		/// <summary>
		/// Counts the compliance risk indicator report.
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public int CountComplianceRiskIndicatorReport(GridSettings gridSettings, SearchCriteriaViewModel viewModel)
		{
			var count = 0;

			using (var connection = new SqlConnection(DbConnection))
			{
				using (var command = new SqlCommand("PaReportComplianceIndicators", connection))
				{
					var sqlParams = new List<SqlParameter>();
					var paramReturnValue = new SqlParameter("@return_value", SqlDbType.Int)
					{
						Direction = ParameterDirection.ReturnValue
					};

					sqlParams.Add(paramReturnValue);

					SqlHelper.AddIntPara(gridSettings.PageIndex, "@pageIndex", sqlParams);
					SqlHelper.AddIntPara(gridSettings.PageSize, "@pageSize", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.OrgCode, "@OrgCode", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ESACode, "@ESACode", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.SiteCode, "@SiteCode", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ProjectID, "@ProjectID", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ProjectType, "@ProjectType", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ContractType, "@ContractType", sqlParams);
					SqlHelper.AddDatePara(viewModel.UploadDateFrom, "@UploadDateFrom", sqlParams);
					SqlHelper.AddDatePara(viewModel.UploadDateTo, "@UploadDateTo", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.SortColumn, "@SortColumn", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.SortBy, "@SortBy", sqlParams);

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange(sqlParams.ToArray());
					connection.Open();

					SqlDataReader reader = null;

					try
					{
						reader = command.ExecuteReader();
						while (reader.Read())
						{
							count++;
						}
					}
					finally
					{
						if (reader != null) { reader.Close(); }
					}
				}
			}

			return count;
		}

		/// <summary>
		/// Gets the site visit report.
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public List<SiteVisit> GetSiteVisitReport(GridSettings gridSettings, SearchCriteriaViewModel viewModel)
		{
			var results = new List<SiteVisit>();

			using (var connection = new SqlConnection(DbConnection))
			{
				using (var command = new SqlCommand("PaReportSiteVisits", connection))
				{
					// this type of data need more than the default timeout 30 seconds.
					command.CommandTimeout = 120; // seconds

					var sqlParams = new List<SqlParameter>();
					var paramReturnValue = new SqlParameter("@return_value", SqlDbType.Int)
					{
						Direction = ParameterDirection.ReturnValue
					};

					sqlParams.Add(paramReturnValue);

					SqlHelper.AddIntPara(gridSettings.PageIndex, "@pageIndex", sqlParams);
					SqlHelper.AddIntPara(gridSettings.PageSize, "@pageSize", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.OrgCode, "@OrgCode", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ESACode, "@ESACode", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.SiteCode, "@SiteCode", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ProjectID, "@ProjectID", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ProjectType, "@ProjectType", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ContractType, "@ContractType", sqlParams);
					SqlHelper.AddDatePara(viewModel.UploadDateFrom, "@UploadDateFrom", sqlParams);
					SqlHelper.AddDatePara(viewModel.UploadDateTo, "@UploadDateTo", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.SortColumn, "@SortColumn", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.SortBy, "@SortBy", sqlParams);

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange(sqlParams.ToArray());
					connection.Open();

					SqlDataReader reader = null;

					try
					{
						reader = command.ExecuteReader();
						while (reader.Read())
						{
							var record = new SiteVisit
							{
								OrgCode = string.Format("{0}", reader["OrgCode"]),
								OrgName = string.Format("{0}", reader["OrgName"]),
								ESACode = string.Format("{0}", reader["ESACode"]),
								ESAName = string.Format("{0}", reader["ESAName"]),
								SiteCode = string.Format("{0}", reader["SiteCode"]),
								SiteName = string.Format("{0}", reader["SiteName"]),
								ProjectID = AppHelper.ToInt(reader["ProjectId"]),
								ProjectName = string.Format("{0}", reader["ProjectName"]),
								ClaimID = AppHelper.ToInt(reader["ClaimId"]),
								ClaimType = string.Format("{0}", reader["ClaimType"]),
								ClaimTypeDescription = string.Format("{0}", reader["ClaimTypeDescription"]),
								ClaimAmount = AppHelper.ToDecimal(reader["ClaimAmount"]),
								ClaimCreationDate = reader["ClaimCreationDate"] as DateTime? ?? default(DateTime),
								ContractType = string.Format("{0}", reader["ContractType"]),
								DaysOverdue = AppHelper.ToInt(reader["DaysOverdue"]),
								AssessmentAction = string.Format("{0}", reader["AssessmentAction"]),
								AssessmentOutcome = string.Format("{0}", reader["ReviewAssessmentCode"]),
								FinalOutcome = AppHelper.ToDecimal(reader["FinalOutcome"]),
								JobSeekerID = AppHelper.ToInt(reader["JobSeekerID"]),
								JobSeekerFirstName = string.Format("{0}", reader["JobSeekerGivenName"]),
								JobSeekerFamilyName = string.Format("{0}", reader["JobSeekerSurname"]),
								JobSeekerName = string.Format("{0} {1}", reader["JobSeekerGivenName"], reader["JobSeekerSurname"]),
								LastUpdateDate = reader["LastUpdateDate"] as DateTime? ?? default(DateTime),
								RecoveryReason = string.Format("{0}", reader["RecoveryReason"]),
								ReviewStatus = string.Format("{0}", reader["ReviewStatus"]),
								UploadDate = reader["UploadedDate"] as DateTime? ?? default(DateTime)
							};

							results.Add(record);

						}
					}
					finally
					{
						if (reader != null) { reader.Close(); }
					}
				}
			}

			return results;
		}

		/// <summary>
		/// Counts the site visit report.
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public int CountSiteVisitReport(GridSettings gridSettings, SearchCriteriaViewModel viewModel)
		{
			var count = 0;

			using (var connection = new SqlConnection(DbConnection))
			{
				using (var command = new SqlCommand("PaReportSiteVisits", connection))
				{
					// this type of data need more than the default timeout 30 seconds.
					command.CommandTimeout = 120; // seconds

					var sqlParams = new List<SqlParameter>();
					var paramReturnValue = new SqlParameter("@return_value", SqlDbType.Int)
					{
						Direction = ParameterDirection.ReturnValue
					};

					sqlParams.Add(paramReturnValue);

					SqlHelper.AddIntPara(gridSettings.PageIndex, "@pageIndex", sqlParams);
					SqlHelper.AddIntPara(gridSettings.PageSize, "@pageSize", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.OrgCode, "@OrgCode", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ESACode, "@ESACode", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.SiteCode, "@SiteCode", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ProjectID, "@ProjectID", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ProjectType, "@ProjectType", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ContractType, "@ContractType", sqlParams);
					SqlHelper.AddDatePara(viewModel.UploadDateFrom, "@UploadDateFrom", sqlParams);
					SqlHelper.AddDatePara(viewModel.UploadDateTo, "@UploadDateTo", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.SortColumn, "@SortColumn", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.SortBy, "@SortBy", sqlParams);

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange(sqlParams.ToArray());
					connection.Open();

					SqlDataReader reader = null;

					try
					{
						reader = command.ExecuteReader();
						while (reader.Read())
						{
							count++;
						}
					}
					finally
					{
						if (reader != null) { reader.Close(); }
					}
				}
			}

			return count;

		}

		/// <summary>
		/// Gets the provider summary report.
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public List<ProviderSummary> GetProviderSummaryReport(GridSettings gridSettings, SearchCriteriaViewModel viewModel)
		{
			var results = new List<ProviderSummary>();

			using (var connection = new SqlConnection(DbConnection))
			{
				using (var command = new SqlCommand("PaReportProviderSummary", connection))
				{
					var sqlParams = new List<SqlParameter>();
					var paramReturnValue = new SqlParameter("@return_value", SqlDbType.Int)
					{
						Direction = ParameterDirection.ReturnValue
					};

					sqlParams.Add(paramReturnValue);

					SqlHelper.AddIntPara(gridSettings.PageIndex, "@pageIndex", sqlParams);
					SqlHelper.AddIntPara(gridSettings.PageSize, "@pageSize", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.OrgCode, "@OrgCode", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ESACode, "@ESACode", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.SiteCode, "@SiteCode", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ProjectID, "@ProjectID", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ProjectType, "@ProjectType", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ContractType, "@ContractType", sqlParams);
					SqlHelper.AddDatePara(viewModel.UploadDateFrom, "@UploadDateFrom", sqlParams);
					SqlHelper.AddDatePara(viewModel.UploadDateTo, "@UploadDateTo", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.SortColumn, "@SortColumn", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.SortBy, "@SortBy", sqlParams);

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange(sqlParams.ToArray());
					connection.Open();

					SqlDataReader reader = null;

					try
					{
						reader = command.ExecuteReader();
						while (reader.Read())
						{
							var record = new ProviderSummary
							{
								OrgCode = string.Format("{0}", reader["OrgCode"]),
								ESACode = string.Format("{0}", reader["ESACode"]),
								SiteCode = string.Format("{0}", reader["SiteCode"]),
								//State = reader["State"] as string,
								RecoveryCount = AppHelper.ToInt(reader["NoOfRecoveries"]),
								CompletedReviewCount = AppHelper.ToInt(reader["NoOfCompletedReviews"]),
								TotalReviewCount = AppHelper.ToInt(reader["TotalReviewsCount"]),
								ValidCount = AppHelper.ToInt(reader["NoOfReviewVAN"]),
								ValidAdminCount = AppHelper.ToInt(reader["NoOfReviewVAD"]),
								InvalidAdminCount = AppHelper.ToInt(reader["NoOfReviewIAD"]),
								InvalidRecovery = AppHelper.ToInt(reader["NoOfReviewINR"]),
								InvalidNoRecovery = AppHelper.ToInt(reader["NoOfReviewINN"])
							};

							results.Add(record);

						}
					}
					finally
					{
						if (reader != null) { reader.Close(); }
					}
				}
			}

			return results;
		}

		/// <summary>
		/// Counts the provider summary report.
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public int CountProviderSummaryReport(GridSettings gridSettings, SearchCriteriaViewModel viewModel)
		{
			var count = 0;

			using (var connection = new SqlConnection(DbConnection))
			{
				using (var command = new SqlCommand("PaReportProviderSummary", connection))
				{
					var sqlParams = new List<SqlParameter>();
					var paramReturnValue = new SqlParameter("@return_value", SqlDbType.Int)
					{
						Direction = ParameterDirection.ReturnValue
					};

					sqlParams.Add(paramReturnValue);

					SqlHelper.AddIntPara(gridSettings.PageIndex, "@pageIndex", sqlParams);
					SqlHelper.AddIntPara(gridSettings.PageSize, "@pageSize", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.OrgCode, "@OrgCode", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ESACode, "@ESACode", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.SiteCode, "@SiteCode", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ProjectID, "@ProjectID", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ProjectType, "@ProjectType", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ContractType, "@ContractType", sqlParams);
					SqlHelper.AddDatePara(viewModel.UploadDateFrom, "@UploadDateFrom", sqlParams);
					SqlHelper.AddDatePara(viewModel.UploadDateTo, "@UploadDateTo", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.SortColumn, "@SortColumn", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.SortBy, "@SortBy", sqlParams);

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange(sqlParams.ToArray());
					connection.Open();

					SqlDataReader reader = null;

					try
					{
						reader = command.ExecuteReader();
						while (reader.Read())
						{
							count++;
						}
					}
					finally
					{
						if (reader != null) { reader.Close(); }
					}
				}
			}

			return count;
		}

		/// <summary>
		/// Gets the progress report.
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public List<Progress> GetProgressReport(GridSettings gridSettings, SearchCriteriaViewModel viewModel)
		{
			var results = new List<Progress>();

			using (var connection = new SqlConnection(DbConnection))
			{
				using (var command = new SqlCommand("PaReportProgresses", connection))
				{
					var sqlParams = new List<SqlParameter>();
					var paramReturnValue = new SqlParameter("@return_value", SqlDbType.Int)
					{
						Direction = ParameterDirection.ReturnValue
					};

					sqlParams.Add(paramReturnValue);

					SqlHelper.AddIntPara(gridSettings.PageIndex, "@pageIndex", sqlParams);
					SqlHelper.AddIntPara(gridSettings.PageSize, "@pageSize", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.OrgCode, "@OrgCode", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ESACode, "@ESACode", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.SiteCode, "@SiteCode", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ProjectID, "@ProjectID", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ProjectType, "@ProjectType", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ContractType, "@ContractType", sqlParams);
					SqlHelper.AddDatePara(viewModel.UploadDateFrom, "@UploadDateFrom", sqlParams);
					SqlHelper.AddDatePara(viewModel.UploadDateTo, "@UploadDateTo", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.SortColumn, "@SortColumn", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.SortBy, "@SortBy", sqlParams);

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange(sqlParams.ToArray());
					connection.Open();

					SqlDataReader reader = null;

					try
					{
						reader = command.ExecuteReader();
						while (reader.Read())
						{
							var record = new Progress
							{
								SampleName = string.Format("{0}", reader["UploadName"]),
								ProjectType = string.Format("{0}", reader["ProjectType"]),
								InProgressReviewCount = AppHelper.ToInt(reader["InProgressReviews"]),
								CompletedReviewCount = AppHelper.ToInt(reader["CompletedReviews"]),
								TotalReviewCount = AppHelper.ToInt(reader["TotalReviews"]),
								PercentCompleted = AppHelper.ToDecimal(reader["PercentCompleted"]),
								LastUpdateDate = reader["LastUpdateDate"] as DateTime? ?? default(DateTime)
							};

							results.Add(record);

						}
					}
					finally
					{
						if (reader != null) { reader.Close(); }
					}
				}
			}

			return results;
		}

		/// <summary>
		/// Counts the progress report.
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public int CountProgressReport(GridSettings gridSettings, SearchCriteriaViewModel viewModel)
		{
			var count = 0;

			using (var connection = new SqlConnection(DbConnection))
			{
				using (var command = new SqlCommand("PaReportProgresses", connection))
				{
					var sqlParams = new List<SqlParameter>();
					var paramReturnValue = new SqlParameter("@return_value", SqlDbType.Int)
					{
						Direction = ParameterDirection.ReturnValue
					};

					sqlParams.Add(paramReturnValue);

					SqlHelper.AddIntPara(gridSettings.PageIndex, "@pageIndex", sqlParams);
					SqlHelper.AddIntPara(gridSettings.PageSize, "@pageSize", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.OrgCode, "@OrgCode", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ESACode, "@ESACode", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.SiteCode, "@SiteCode", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ProjectID, "@ProjectID", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ProjectType, "@ProjectType", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ContractType, "@ContractType", sqlParams);
					SqlHelper.AddDatePara(viewModel.UploadDateFrom, "@UploadDateFrom", sqlParams);
					SqlHelper.AddDatePara(viewModel.UploadDateTo, "@UploadDateTo", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.SortColumn, "@SortColumn", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.SortBy, "@SortBy", sqlParams);

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange(sqlParams.ToArray());
					connection.Open();

					SqlDataReader reader = null;

					try
					{
						reader = command.ExecuteReader();
						while (reader.Read())
						{
							count++;
						}
					}
					finally
					{
						if (reader != null) { reader.Close(); }
					}
				}
			}

			return count;
		}

		/// <summary>
		/// Gets the project type report.
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public ProjectType GetProjectTypeReport(SearchCriteriaViewModel viewModel)
		{
			var results = new ProjectType();

			using (var connection = new SqlConnection(DbConnection))
			{
				using (var command = new SqlCommand("PaReportProjectType", connection))
				{
					var sqlParams = new List<SqlParameter>();
					var paramReturnValue = new SqlParameter("@return_value", SqlDbType.Int)
					{
						Direction = ParameterDirection.ReturnValue
					};

					sqlParams.Add(paramReturnValue);

					SqlHelper.AddVarcharPara(viewModel.OrgCode, "@OrgCode", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ESACode, "@ESACode", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.SiteCode, "@SiteCode", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ProjectID, "@ProjectID", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ProjectType, "@ProjectType", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ContractType, "@ContractType", sqlParams);
					SqlHelper.AddDatePara(viewModel.UploadDateFrom, "@UploadDateFrom", sqlParams);
					SqlHelper.AddDatePara(viewModel.UploadDateTo, "@UploadDateTo", sqlParams);

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange(sqlParams.ToArray());
					connection.Open();

					SqlDataReader reader = null;

					try
					{
						reader = command.ExecuteReader();

						while (reader.HasRows)
						{
							while (reader.Read())
							{
								var projectTypeDetail = new ProjectTypeDetail
									 {
										 Type = string.Format("{0}", reader["Type"]),
										 Code = string.Format("{0}", reader["Code"]),
										 Description = string.Format("{0}", reader["Description"]),
										 ProjectCount = AppHelper.ToInt(reader["NoOfProjects"]),
										 ReviewCount = AppHelper.ToInt(reader["NoOfReviews"])
									 };

								var projectBy = GetProjectTypeBy(projectTypeDetail.Type, results);
								projectBy.Add(projectTypeDetail);
							}

							reader.NextResult();
						}
					}
					finally
					{
						if (reader != null) { reader.Close(); }
					}
				}
			}

			return results;
		}

		/// <summary>
		/// Gets the project type by.
		/// </summary>
		/// <param name="projectTypeBy">The project type by.</param>
		/// <param name="projectType">Type of the project.</param>
		/// <returns></returns>
		public List<ProjectTypeDetail> GetProjectTypeBy(string projectTypeBy, ProjectType projectType)
		{
			switch (projectTypeBy.ToUpper())
			{
				case DataConstants.ProjectByOrg:
					return projectType.ProjectByOrganisation;

				case DataConstants.ProjectByType:
					return projectType.ProjectByType;

				case DataConstants.ProjectByESA:
					return projectType.ProjectByESA;

				case DataConstants.ProjectByState:
					return projectType.ProjectByState;

				case DataConstants.ProjectByNational:
					return projectType.ProjectByNational;

				default:
					return projectType.ProjectByOrganisation;
			}
		}

		/// <summary>
		/// Gets the finding summary report.
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public FindingSummary GetFindingSummaryReport(SearchCriteriaViewModel viewModel)
		{
			var results = new FindingSummary();

			using (var connection = new SqlConnection(DbConnection))
			{
				using (var command = new SqlCommand("PaReportFindingSummary", connection))
				{
					var sqlParams = new List<SqlParameter>();
					var paramReturnValue = new SqlParameter("@return_value", SqlDbType.Int)
					{
						Direction = ParameterDirection.ReturnValue
					};

					sqlParams.Add(paramReturnValue);

					SqlHelper.AddVarcharPara(viewModel.OrgCode, "@OrgCode", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ESACode, "@ESACode", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.SiteCode, "@SiteCode", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ProjectID, "@ProjectID", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ProjectType, "@ProjectType", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ContractType, "@ContractType", sqlParams);
					SqlHelper.AddDatePara(viewModel.UploadDateFrom, "@UploadDateFrom", sqlParams);
					SqlHelper.AddDatePara(viewModel.UploadDateTo, "@UploadDateTo", sqlParams);

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange(sqlParams.ToArray());
					connection.Open();

					SqlDataReader reader = null;

					try
					{
						reader = command.ExecuteReader();

						while (reader.HasRows)
						{
							while (reader.Read())
							{
								var findingSummaryDetail = new FindingSummaryDetail
								{
									Type = string.Format("{0}",reader["Type"]),
									Code = string.Format("{0}", reader["Code"]),
									Description = string.Format("{0}", reader["Description"]),
									ReviewCount = AppHelper.ToDecimal(reader["ReviewCount"])
								};

								var findingSummaryType = GetFindingSummaryType(findingSummaryDetail.Type, results);
								findingSummaryType.Add(findingSummaryDetail);
							}

							reader.NextResult();
						}
					}
					finally
					{
						if (reader != null) { reader.Close(); }
					}
				}
			}

			return results;
		}

		/// <summary>
		/// Gets the type of the finding summary.
		/// </summary>
		/// <param name="findingSummaryType">Type of the finding summary.</param>
		/// <param name="findingSummary">The finding summary.</param>
		/// <returns></returns>
		public List<FindingSummaryDetail> GetFindingSummaryType(string findingSummaryType, FindingSummary findingSummary)
		{
			switch (findingSummaryType.ToUpper())
			{
				case DataConstants.FindingSummaryInScope:
					return findingSummary.InScopeReview;

				case DataConstants.FindingSummaryOutScope:
					return findingSummary.OutScopeReview;

				case DataConstants.FindingSummaryRecovery:
					return findingSummary.RecoveryReview;

				default:
					return findingSummary.InScopeReview;
			}
		}

		/// <summary>
		/// Gets the dashboard report.
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public List<Dashboard> GetDashboardReport(SearchCriteriaViewModel viewModel)
		{
			var results = new List<Dashboard>();

			using (var connection = new SqlConnection(DbConnection))
			{
				using (var command = new SqlCommand("PaReportDashboard", connection))
				{
					var sqlParams = new List<SqlParameter>();
					var paramReturnValue = new SqlParameter("@return_value", SqlDbType.Int)
					{
						Direction = ParameterDirection.ReturnValue
					};

					sqlParams.Add(paramReturnValue);

					SqlHelper.AddVarcharPara(viewModel.OrgCode, "@OrgCode", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ESACode, "@ESACode", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.SiteCode, "@SiteCode", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ProjectID, "@ProjectID", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ProjectType, "@ProjectType", sqlParams);
					SqlHelper.AddVarcharPara(viewModel.ContractType, "@ContractType", sqlParams);
					SqlHelper.AddDatePara(viewModel.UploadDateFrom, "@UploadDateFrom", sqlParams);
					SqlHelper.AddDatePara(viewModel.UploadDateTo, "@UploadDateTo", sqlParams);

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange(sqlParams.ToArray());
					connection.Open();

					SqlDataReader reader = null;

					try
					{
						reader = command.ExecuteReader();
						while (reader.Read())
						{
							results.Add(new Dashboard { OutcomeCode = "InProgress", OutcomeDescription = "In Progress", ReviewCount = AppHelper.ToInt(reader["InProgressCount"]) });
							results.Add(new Dashboard { OutcomeCode = "Completed", OutcomeDescription = "Completed", ReviewCount = AppHelper.ToInt(reader["CompletedCount"]) });
							results.Add(new Dashboard { OutcomeCode = "VAN", OutcomeDescription = "Valid (NFA)", ReviewCount = AppHelper.ToInt(reader["OutcomeCodeVANCount"]) });
							results.Add(new Dashboard { OutcomeCode = "VAD", OutcomeDescription = "Valid (Admin Deficiency – Provider Education)", ReviewCount = AppHelper.ToInt(reader["OutcomeCodeVADCount"]) });
							results.Add(new Dashboard { OutcomeCode = "INR", OutcomeDescription = "Invalid (Recovery)", ReviewCount = AppHelper.ToInt(reader["OutcomeCodeINRCount"]) });
							results.Add(new Dashboard { OutcomeCode = "INN", OutcomeDescription = "Invalid (No Recovery)", ReviewCount = AppHelper.ToInt(reader["OutcomeCodeINNCount"]) });
							results.Add(new Dashboard { OutcomeCode = "IAD", OutcomeDescription = "Invalid (Admin Deficiency - Provider Education)", ReviewCount = AppHelper.ToInt(reader["OutcomeCodeIADCount"]) });
						}
					}
					finally
					{
						if (reader != null) { reader.Close(); }
					}
				}
			}

			return results;
		}

		/// <summary>
		/// Gets the esa code list.
		/// </summary>
		/// <param name="orgCode">The org code.</param>
		/// <returns></returns>
		public List<SelectListItem> GetESACodeList(string orgCode)
		{
			var results = new List<SelectListItem>();

			using (var connection = new SqlConnection(DbConnection))
			{
				using (var command = new SqlCommand("PaReportESACodeGetAllByOrgCode", connection))
				{
					var sqlParams = new List<SqlParameter>();
					var paramReturnValue = new SqlParameter("@return_value", SqlDbType.Int)
					{
						Direction = ParameterDirection.ReturnValue
					};

					sqlParams.Add(paramReturnValue);

					SqlHelper.AddVarcharPara(orgCode, "@OrgCode", sqlParams);

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange(sqlParams.ToArray());
					connection.Open();

					SqlDataReader reader = null;

					try
					{
						reader = command.ExecuteReader();
						while (reader.Read())
						{
							results.Add(new SelectListItem
								{
									Value = string.Format("{0}", reader["ESACode"]),
									Text = string.Format("{0}", reader["Description"])
								});
						}
					}
					finally
					{
						if (reader != null) { reader.Close(); }
					}
				}
			}

			return results;
		}

		/// <summary>
		/// Gets the site code list.
		/// </summary>
		/// <param name="orgCode">The org code.</param>
		/// <param name="esaCode">The esa code.</param>
		/// <returns></returns>
		public List<SelectListItem> GetSiteCodeList(string orgCode, string esaCode)
		{
			var results = new List<SelectListItem>();

			using (var connection = new SqlConnection(DbConnection))
			{
				using (var command = new SqlCommand("PaReportSiteCodeGetAllByESACode", connection))
				{
					var sqlParams = new List<SqlParameter>();
					var paramReturnValue = new SqlParameter("@return_value", SqlDbType.Int)
					{
						Direction = ParameterDirection.ReturnValue
					};

					sqlParams.Add(paramReturnValue);

					SqlHelper.AddVarcharPara(orgCode, "@OrgCode", sqlParams);
					SqlHelper.AddVarcharPara(esaCode, "@EsaCode", sqlParams);

					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange(sqlParams.ToArray());
					connection.Open();

					SqlDataReader reader = null;

					try
					{
						reader = command.ExecuteReader();
						while (reader.Read())
						{
							results.Add(new SelectListItem
							{
								Value = string.Format("{0}", reader["SiteCode"]),
								Text = string.Format("{0}", reader["Description"])
							});
						}
					}
					finally
					{
						if (reader != null) { reader.Close(); }
					}
				}
			}

			return results;
		}

	}
}