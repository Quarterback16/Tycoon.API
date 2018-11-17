using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using ProgramAssuranceTool.Infrastructure.Interfaces;
using ProgramAssuranceTool.Models;
using ProgramAssuranceTool.Repositories;
using ProgramAssuranceTool.ViewModels;
using ProgramAssuranceTool.ViewModels.Claims;
using ProgramAssuranceTool.ViewModels.Project;
using ProgramAssuranceTool.ViewModels.Report;
using GridSettings = MvcJqGrid.GridSettings;

namespace ProgramAssuranceTool.Interfaces
{
	/// <summary>
	///  For simplicity we will have just one do it all Service (contrast vs 1 per View)
	///  Controllers should get the PatService to do all their work for them so they can
	///  remain skinny Controllers, nobody likes a fat controller.
	///
	///  The PatService will make use of a set of Repository classes that are responsible
	///  for talking to the 2 back ends:  SqlServer and the Mainframe.
	///
	/// </summary>
	public interface IPatService
	{
		#region Repositories

		/// <summary>
		///    Exposes the Upload Repository
		/// </summary>
		/// <returns>IUploadRepository</returns>
		IUploadRepository GetUploadRepository();

		/// <summary>
		///    Exposes the Questionnaire Repository
		/// </summary>
		/// <returns>IQuestionnaireRepository</returns>
		IQuestionnaireRepository GetQuestionaireRepository();

		/// <summary>
		///    Exposes the Review Repository
		/// </summary>
		/// <returns>IReviewRepository</returns>
		IReviewRepository GetReviewRepository();

		/// <summary>
		///    Exposes the ADW Repository
		/// </summary>
		/// <returns>IAdwRepository</returns>
		IAdwRepository GetAdwRepository();

		/// <summary>
		///    Exposes the Audit Service
		/// </summary>
		/// <returns>IAuditService</returns>
		IAuditService GetAuditService();

		#endregion Repositories

		#region Business Object = Project

		/// <summary>
		///   Gets a Project record based on an ID
		/// </summary>
		/// <param name="projectId"></param>
		/// <returns>Project object</returns>
		Project GetProject( int projectId );

		/// <summary>
		///   Updates a project with new data
		/// </summary>
		/// <param name="p">the changed project object</param>
		void UpdateProject( Project p );

		/// <summary>
		///   Deletes a project from existance
		/// </summary>
		/// <param name="id">the identifier of the project being deleted</param>
		/// <param name="userId">the user id of the person requesting the delete</param>
		/// <returns></returns>
		bool DeleteProject( int id, string userId );

		/// <summary>
		///   Gets a collection of project record based on the parameters in the GridSettings object
		/// </summary>
		/// <param name="gridSettings">selection criteria</param>
		/// <returns>List of Project objects</returns>
		List<Project> GetProjects( GridSettings gridSettings );

		/// <summary>
		///   Gets a collection of project record based on the parameters in the GridSettings object
		///   and a date range
		///
		/// </summary>
		/// <param name="gridSettings">selection criteria</param>
		/// <param name="uploadedFrom">from date</param>
		/// <param name="uploadedTo">to date</param>
		/// <returns>List of Project objects</returns>
		List<Project> GetProjects( GridSettings gridSettings, DateTime uploadedFrom, DateTime uploadedTo );

		/// <summary>
		///   Find up to n projects by part of the name
		///   used by Autocomplete fields
		/// </summary>
		/// <param name="searchText"></param>
		/// <param name="maxResults">The maximum number of projects to find</param>
		/// <returns>List of Project objects</returns>
		List<Project> LookupProject( string searchText, int maxResults );

		/// <summary>
		///   Counts the number of projects matching the selection criteria
		/// </summary>
		/// <param name="gridSettings">selection criteria</param>
		/// <returns>the count</returns>
		int CountProjects( GridSettings gridSettings );

		/// <summary>
		///   Counts the number of projects matching the selection criteria
		///   including an upload date range
		/// </summary>
		/// <param name="gridSettings">selection criteria</param>
		/// <param name="uploadedFrom">from date</param>
		/// <param name="uploadedTo">to date</param>
		/// <returns>the count of projects</returns>
		int CountProjects( GridSettings gridSettings, DateTime uploadedFrom, DateTime uploadedTo );

		/// <summary>
		///   Looks at the database and determines which project types have been used
		/// </summary>
		/// <returns>array of project type codes</returns>
		string[] GetAllDistinctProjectTypes();

		/// <summary>
		///   Get the description of the project type given the c
		/// </summary>
		/// <param name="projectType">project type code</param>
		/// <returns>description</returns>
		string GetProjectTypeDescription( string projectType );

		/// <summary>
		///   Creates a new Project
		/// </summary>
		/// <param name="project">The candidate project object</param>
		/// <returns>the id of the newly created project</returns>
		int CreateProject( Project project );

		/// <summary>
		///   Checks to see if a project name has already been used
		/// </summary>
		/// <param name="projectName">the project name</param>
		/// <param name="projectId">the ID of the project that has the name</param>
		/// <returns>used or not used</returns>
		bool IsProjectNameUsed( string projectName, int projectId );

		/// <summary>
		///    The number of uploads a particular project has
		/// </summary>
		/// <param name="projectId">the id of the project</param>
		/// <returns>the count</returns>
		int CountUploads( int projectId );

		/// <summary>
		/// Counts the reviews for a project.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>the count</returns>
		int CountReviews( int projectId );

		/// <summary>
		/// Counts the reviews for a project using grid selection criteria.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="gridSettings">The grid settings.</param>
		/// <returns></returns>
		int CountReviews( int projectId, GridSettings gridSettings );

		/// <summary>
		/// Gets the project uploads for a particular project and grid selection criteria.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="gridSettings">The grid settings.</param>
		/// <returns>List of upload objects</returns>
		List<Upload> GetProjectUploads( int projectId, GridSettings gridSettings );

		/// <summary>
		///   Works out if the Project has findings.
		/// </summary>
		/// <param name="projectId">The identifier.</param>
		/// <returns>yes it has or no it doesnt</returns>
		bool ProjectHasFindings( int projectId );

		/// <summary>
		/// Gets the project reviews.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="gridSettings">The grid settings.</param>
		/// <returns>List of Review objects</returns>
		List<Review> GetProjectReviews( int projectId, GridSettings gridSettings );


		/// <summary>
		/// Gets the review list search terms for the Grid filters.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="orgCodes">The org codes.</param>
		/// <param name="siteCodes">The site codes.</param>
		/// <param name="esaCodes">The esa codes.</param>
		/// <param name="stateCodes">The state codes.</param>
		void GetReviewListSearchTerms( int projectId, out string[] orgCodes, out string[] siteCodes, out string[] esaCodes,
												 out string[] stateCodes );

		/// <summary>
		/// Exports the reviews to a CSV file.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="gs">The grid settings.</param>
		/// <returns>CSV string</returns>
		string ExportReviews( int projectId, GridSettings gs );

		/// <summary>
		/// Exports the reviews for a particular upload to CSV.
		/// </summary>
		/// <param name="uploadId">The upload identifier.</param>
		/// <returns>CSV string</returns>
		string ExportReviewsForUpload( int uploadId );

		/// <summary>
		/// Counts the contracts for a particular project ID.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>theContract count</returns>
		int CountContracts( int projectId );

		/// <summary>
		///   Works out if the Project is contract monitoring or contract site visit type of Project.
		///   The 2 major categories of PA project.
		/// </summary>
		/// <param name="projectId">The identifier.</param>
		/// <returns> true if the project is a Contract Monitoring or COntract Site one</returns>
		bool ProjectIsContractMonitoringOrContractSiteVisit( int projectId );

		#endregion Business Object = Project

		#region Business Object = Project Questions

		/// <summary>
		///   Validates the question upload CSV file.
		/// </summary>
		/// <param name="stream">The memory stream based on the CSV file.</param>
		/// <returns>a collection of validation errors</returns>
		IEnumerable<CellValidationError> ValidateQuestionUpload( Stream stream );

		/// <summary>
		///   Stores the project question data from a memory stream.
		/// </summary>
		/// <param name="vm">The view model.</param>
		/// <param name="stream">The memory stream.</param>
		/// <returns>number of records stored</returns>
		int StoreProjectQuestionData( UploadQuestionsViewModel vm, Stream stream );

		/// <summary>
		///   Gets the project questions contained in the Questionnaire for the project.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>List of Questions</returns>
		List<PatQuestion> GetProjectQuestions( int projectId );

		#endregion Business Object = Project Questions

		#region Business Object = Project Attachment

		/// <summary>
		///   Stores the attachment into Sql Server.
		/// </summary>
		/// <param name="fileData">The file data in the form of a byte array.</param>
		/// <param name="attachmentId">The attachment identifier.</param>
		void StoreAttachment( byte[] fileData, int attachmentId );

		/// <summary>
		///   Retrieves the attachment from the database as a .
		/// </summary>
		/// <param name="attachmentId">The attachment identifier.</param>
		/// <returns>a byte array representing</returns>
		byte[] RetrieveAttachment( int attachmentId );

		/// <summary>
		/// Gets all the attachments for a project based on grid criteria.
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="projectId">The project identifier.</param>
		/// <returns></returns>
		List<ProjectAttachment> GetAttachments( GridSettings gridSettings, int projectId );

		/// <summary>
		/// Gets an attachment by attachment id.
		/// </summary>
		/// <param name="attachmentId">The identifier.</param>
		/// <returns></returns>
		ProjectAttachment GetAttachment( int attachmentId );

		/// <summary>
		/// Counts the attachments for a project based on grid criteria.
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>the number of attachments</returns>
		int CountAttachments( GridSettings gridSettings, int projectId );

		/// <summary>
		/// Saves the attachment into the database.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <param name="fileData">The file data.</param>
		/// <returns>A projectAttachment onject</returns>
		ProjectAttachment SaveAttachment( ProjectAttachment model, byte[] fileData );

		/// <summary>
		/// Deletes the attachment given the attachment identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		void DeleteAttachment( int id );

		#endregion Business Object = Project Attachment

		#region Business Object - Project Contracts

		/// <summary>
		/// Gets the project contracts from ADW, there should be 2.
		/// </summary>
		/// <returns>List of Project Contract codes</returns>
		List<AdwItem> GetProjectContracts();

		/// <summary>
		/// Gets the project contracts by project.
		/// </summary>
		/// <param name="id">The project identifier.</param>
		/// <returns>The contracts for the project</returns>
		List<ProjectContract> GetProjectContractsByProject( int id );

		/// <summary>
		/// Saves the project contracts.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="list">The list of contracts.</param>
		/// <param name="userId">The user identifier.</param>
		void SaveProjectContracts( int projectId, List<ProjectContract> list, string userId );

		/// <summary>
		/// Saves the project contracts.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="list">The list.</param>
		/// <param name="loginName">Name of the login.</param>
		void SaveProjectContracts( int projectId, string[] list, string loginName );

		/// <summary>
		/// Gets the project contract selections.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		string GetProjectContractSelections( int id );

		#endregion Business Object - Project Contracts

		#region Business Object = User Activity

		/// <summary>
		/// Gets the audit records for a particular user and date range.
		/// </summary>
		/// <param name="userId">The user identifier.</param>
		/// <param name="from">audit transactions From.</param>
		/// <param name="to">audit transactions To.</param>
		/// <returns>list of audit transactions</returns>
		List<UserActivity> GetActivities( string userId, DateTime from, DateTime to );

		/// <summary>
		/// Deletes the audit log items prior to a certain date.
		/// </summary>
		/// <param name="priorToDate">The prior to date.</param>
		void DeleteLogItemsPriorTo( DateTime priorToDate );

		/// <summary>
		/// Saves an audit log item.
		/// </summary>
		/// <param name="userActivity">The user log item.</param>
		void SaveActivity( UserActivity userActivity );

		/// <summary>
		/// Saves the log item given the message and the user id.
		/// </summary>
		/// <param name="activity">The activity.</param>
		/// <param name="userId">The user identifier.</param>
		void SaveActivity( string activity, string userId );

		/// <summary>
		/// Saves the activity log item given the log item and an existing repository.
		/// </summary>
		/// <param name="userActivity">The user activity.</param>
		/// <param name="uaRep">The ua rep.</param>
		void SaveActivity( UserActivity userActivity, UserActivityRepository<UserActivity> uaRep );

		#endregion Business Object = User Activity

		#region Business Object = Bulletin

		/// <summary>
		/// Gets the bulletin type description.
		/// </summary>
		/// <param name="bulletinType">Type of the bulletin.</param>
		/// <returns></returns>
		string GetBulletinTypeDescription(string bulletinType);

		/// <summary>
		/// Gets the bulletins by its type and admin privilage
		/// </summary>
		/// <param name="bulletinType">Type of the bulletin.</param>
		/// <param name="isAdmin">if set to <c>true</c> [is admin].</param>
		/// <returns>a list of bulletin</returns>
		List<Bulletin> GetBulletins( string bulletinType, bool isAdmin );

		/// <summary>
		/// Gets the bulletins by its grid setting, type and admin privilage
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="bulletinType">Type of the bulletin.</param>
		/// <param name="isAdmin">if set to <c>true</c> [is admin].</param>
		/// <returns>a list of bulletin</returns>
		List<Bulletin> GetBulletins( GridSettings gridSettings, string bulletinType, bool isAdmin );

		/// <summary>
		/// Gets the bulletin by its id.
		/// </summary>
		/// <param name="id">The bulletin id.</param>
		/// <returns>bulletin</returns>
		Bulletin GetBulletin( int id );

		/// <summary>
		/// Creates the bulletin.
		/// </summary>
		/// <param name="bulletin">The bulletin.</param>
		/// <returns></returns>
		int CreateBulletin( Bulletin bulletin );

		/// <summary>
		/// Updates the bulletin.
		/// </summary>
		/// <param name="bulletin">The bulletin.</param>
		/// <returns></returns>
		Bulletin UpdateBulletin( Bulletin bulletin );

		/// <summary>
		/// Deletes the bulletin by its id
		/// </summary>
		/// <param name="id">The bulletin id.</param>
		void DeleteBulletin( int id );

		/// <summary>
		/// Counts the bulletins by its grid setting, type and admin privilage
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="bulletinType">Type of the bulletin.</param>
		/// <param name="isAdmin">if set to <c>true</c> [is admin].</param>
		/// <returns>a number of bulletin as specified in criteria</returns>
		int CountBulletins( GridSettings gridSettings, string bulletinType, bool isAdmin );

		#endregion Business Object = Bulletin

		#region Pat Users

		/// <summary>
		/// Gets the data to populate into the program assurance users dropdown list.
		/// </summary>
		/// <returns></returns>
		IEnumerable<SelectListItem> GetProgramAssuranceDropdownList();

		/// <summary>
		/// Gets the list of users who are program assurance admin users.
		/// </summary>
		/// <returns>list of user objects</returns>
		List<PatUser> GetProgramAssuranceAdminUsers();

		/// <summary>
		/// Gets the list of program assurance users.
		/// </summary>
		/// <returns>list of user objects</returns>
		List<PatUser> GetProgramAssuranceUsers();

		#endregion Pat Users

		#region Business Object = Control

		/// <summary>
		/// Gets the control file.
		/// </summary>
		/// <returns>control record</returns>
		PatControl GetControlFile();

		/// <summary>
		/// Updates the control record.
		/// </summary>
		/// <param name="control">The control.</param>
		void UpdateControl( PatControl control );

		#endregion Business Object = Control

		#region Business Object = Sample

		/// <summary>
		/// Popolates a view with an Extract of claims.
		/// </summary>
		/// <param name="criteria">The claim criteria.</param>
		/// <returns>the populated view</returns>
		ClaimSampleViewModel ExtractClaims( SampleCriteria criteria );

		/// <summary>
		/// Saves the extract claims into the data store.
		/// </summary>
		/// <param name="sessionKey">The session key.</param>
		/// <param name="claims">The claims.</param>
		/// <param name="userId">The user identifier.</param>
		void SaveExtract( string sessionKey, List<PatClaim> claims, string userId );

		/// <summary>
		/// Gets the sample the user is building up in session.
		/// </summary>
		/// <param name="sessionKey">The session key.</param>
		/// <returns>list of sample records</returns>
		List<Sample> GetSample( string sessionKey );

		/// <summary>
		/// Saves the sample against a certain project.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="sessionKey">The session key.</param>
		/// <param name="ids">The ids.</param>
		/// <param name="outOfScope">if set to <c>true</c> [out of scope].</param>
		/// <param name="additional">if set to <c>true</c> [additional].</param>
		/// <param name="dueDate">The due date.</param>
		/// <param name="userId">The user identifier.</param>
		/// <returns>the id of the sample</returns>
		int SaveSample( int projectId, string sessionKey, List<int> ids, bool outOfScope, bool additional, DateTime dueDate,
							 string userId );

		/// <summary>
		/// Saves the sample selections.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="sessionKey">The session key.</param>
		/// <param name="toList">To list.</param>
		/// <param name="loginName">the user id.</param>
		void SaveSampleSelections( int projectId, string sessionKey, List<int> toList, string loginName );

		/// <summary>
		/// Gets the sample selections.
		/// </summary>
		/// <param name="sessionKey">The session key.</param>
		/// <returns>the ids of the selected claims</returns>
		string GetSampleSelections( string sessionKey );

		/// <summary>
		/// Counts the reviews by upload identifier.
		/// </summary>
		/// <param name="uploadId">The upload identifier.</param>
		/// <returns>the number of reviews an upload contains</returns>
		int CountReviewsByUploadId( int uploadId );

		/// <summary>
		/// Counts the completed reviews by upload identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>the number of completed reviews an upload contains</returns>
		int CountCompletedReviewsByUploadId( int id );

		/// <summary>
		/// Retrieves an upload by its identifier.
		/// </summary>
		/// <param name="uploadId">The upload identifier.</param>
		/// <returns>upload record</returns>
		Upload GetUploadById( int uploadId );

		/// <summary>
		/// Gets all the reviews in the upload that match the grid criteria.
		/// </summary>
		/// <param name="uploadId">The upload identifier.</param>
		/// <param name="gs">The gridsettings.</param>
		/// <returns></returns>
		List<Review> GetUpload( int uploadId, GridSettings gs );

		/// <summary>
		/// Validates the uploaded CSV data.
		/// </summary>
		/// <param name="stream">Tye CSV upload as a memory stream.</param>
		/// <param name="includesOutcomes">if set to <c>true</c> [includes outcomes].</param>
		/// <returns>a collection of validation errors</returns>
		IEnumerable<CellValidationError> ValidateUpload( Stream stream, bool includesOutcomes );

		/// <summary>
		/// Creates the upload record.
		/// </summary>
		/// <param name="upload">The upload object.</param>
		/// <returns>the id of the newly created upload or 0</returns>
		int CreateUpload( Upload upload );

		/// <summary>
		/// Updates the upload record.
		/// </summary>
		/// <param name="upload">The upload record.</param>
		void UpdateUpload( Upload upload );

		/// <summary>
		/// Deletes the upload record.
		/// </summary>
		/// <param name="upload">The upload record.</param>
		void DeleteUpload( Upload upload );

		/// <summary>
		/// Checks if the Samples name is used.
		/// </summary>
		/// <param name="sampleName">Name of the sample.</param>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>whether or not the sample name is used</returns>
		bool SampleNameIsUsed( string sampleName, int projectId );

		/// <summary>
		/// Gets the sample message based on what has been stored in the users session.
		/// </summary>
		/// <param name="sessionId">The session identifier.</param>
		/// <returns>the message</returns>
		string GetSampleMessage( string sessionId );

		/// <summary>
		/// Gets the distinct session keys that have been used.
		/// </summary>
		/// <returns>an array of keys</returns>
		string[] GetDistinctSessionKeys();

		/// <summary>
		/// Deletes the session.
		/// </summary>
		/// <param name="sessionKey">The session key.</param>
		/// <param name="userId">The user identifier.</param>
		void DeleteSession( string sessionKey, string userId );

		#endregion Business Object = Sample

		#region Business Object = Review

		/// <summary>
		/// Gets a particular review based on its id.
		/// </summary>
		/// <param name="reviewId">The review identifier.</param>
		/// <returns>a review object</returns>
		Review GetReview( int reviewId );

		/// <summary>
		/// Updates the review.
		/// </summary>
		/// <param name="review">The review.</param>
		void UpdateReview( Review review );

		/// <summary>
		/// Deletes the review.
		/// </summary>
		/// <param name="review">The review.</param>
		/// <param name="userId">The user identifier.</param>
		void DeleteReview( Review review, string userId );

		/// <summary>
		/// Deletes the reviews that match a set of identifiers.
		/// </summary>
		/// <param name="ids">The ids.</param>
		/// <param name="removeDomain">The remove domain.</param>
		void DeleteReviews( List<int> ids, string removeDomain );

		/// <summary>
		/// Checks if any of the reviews in the list have outcomes recorded against them.
		/// </summary>
		/// <param name="selections">The selected reviews.</param>
		/// <returns>yes or no</returns>
		bool AnyOutComesFor( int[] selections );

		/// <summary>
		/// Records the same outcome for a collection of reviews.
		/// </summary>
		/// <param name="reviewIds">The review ids.</param>
		/// <param name="outcome">The outcome.</param>
		/// <param name="userId">The user identifier.</param>
		void BulkOutcome( int[] reviewIds, Review outcome, string userId );

		/// <summary>
		/// Gets the earliest and latest upload dates on record.
		/// </summary>
		/// <param name="latest">The latest upload.</param>
		/// <returns>the earliest upload</returns>
		DateTime GetEarliestAndLatestUploadDates( out DateTime latest );

		/// <summary>
		/// Stores the review data from a CSV upload.
		/// </summary>
		/// <param name="upload">The upload.</param>
		/// <param name="stream">The stream.</param>
		/// <returns>number of reviews stored</returns>
		int StoreReviewData( Upload upload, Stream stream );

		/// <summary>
		/// Appends a number of reviews  based on a stream of CSV data.
		/// </summary>
		/// <param name="upload">The upload.</param>
		/// <param name="stream">The stream.</param>
		/// <returns>number of records appended</returns>
		int AppendReviewData( Upload upload, Stream stream );

		#endregion Business Object = Review

		#region Reports

		/// <summary>
		/// Gets the compliance risk report sort order.
		/// </summary>
		/// <returns></returns>
		List<SelectListItem> GetComplianceRiskReportSortOrder();

		/// <summary>
		/// Gets the compliance risk indicator report.
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		List<ComplianceRiskIndicator> GetComplianceRiskIndicatorReport( SearchCriteriaViewModel viewModel );

		/// <summary>
		/// Gets the compliance risk indicator report.
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		List<ComplianceRiskIndicator> GetComplianceRiskIndicatorReport( GridSettings gridSettings, SearchCriteriaViewModel viewModel );

		/// <summary>
		/// Counts the compliance risk indicator report.
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		int CountComplianceRiskIndicatorReport( SearchCriteriaViewModel viewModel );

		/// <summary>
		/// Gets the site visit report sort order.
		/// </summary>
		/// <returns></returns>
		List<SelectListItem> GetSiteVisitReportSortOrder();

		/// <summary>
		/// Gets the site visit report.
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		List<SiteVisit> GetSiteVisitReport( SearchCriteriaViewModel viewModel );

		/// <summary>
		/// Gets the site visit report.
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		List<SiteVisit> GetSiteVisitReport( GridSettings gridSettings, SearchCriteriaViewModel viewModel );

		/// <summary>
		/// Counts the site visit report.
		/// </summary>
		/// <param name="getValues">The get values.</param>
		/// <returns></returns>
		int CountSiteVisitReport( SearchCriteriaViewModel getValues );

		/// <summary>
		/// Gets the provider summary report sort order.
		/// </summary>
		/// <returns></returns>
		List<SelectListItem> GetProviderSummaryReportSortOrder();

		/// <summary>
		/// Gets the provider summary report.
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		List<ProviderSummary> GetProviderSummaryReport( SearchCriteriaViewModel viewModel );

		/// <summary>
		/// Gets the provider summary report.
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		List<ProviderSummary> GetProviderSummaryReport( GridSettings gridSettings, SearchCriteriaViewModel viewModel );

		/// <summary>
		/// Counts the provider summary report.
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		int CountProviderSummaryReport( SearchCriteriaViewModel viewModel );

		/// <summary>
		/// Gets the progress report sort order.
		/// </summary>
		/// <returns></returns>
		List<SelectListItem> GetProgressReportSortOrder();

		/// <summary>
		/// Gets the progress report.
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		List<Progress> GetProgressReport( SearchCriteriaViewModel viewModel );

		/// <summary>
		/// Gets the progress report.
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		List<Progress> GetProgressReport( GridSettings gridSettings, SearchCriteriaViewModel viewModel );

		/// <summary>
		/// Counts the progress report.
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		int CountProgressReport( SearchCriteriaViewModel viewModel );

		/// <summary>
		/// Gets the project type report.
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		ProjectType GetProjectTypeReport( SearchCriteriaViewModel viewModel );

		/// <summary>
		/// Gets the project type by.
		/// </summary>
		/// <param name="projectTypeBy">The project type by.</param>
		/// <param name="projectType">Type of the project.</param>
		/// <returns></returns>
		List<ProjectTypeDetail> GetProjectTypeBy( string projectTypeBy, ProjectType projectType );

		/// <summary>
		/// Gets the finding summary report.
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		FindingSummary GetFindingSummaryReport( SearchCriteriaViewModel viewModel );

		/// <summary>
		/// Gets the type of the finding summary.
		/// </summary>
		/// <param name="findingSummaryType">Type of the finding summary.</param>
		/// <param name="findingSummary">The finding summary.</param>
		/// <returns></returns>
		List<FindingSummaryDetail> GetFindingSummaryType( string findingSummaryType, FindingSummary findingSummary );

		/// <summary>
		/// Gets the dashboard report.
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		List<Dashboard> GetDashboardReport( SearchCriteriaViewModel viewModel );

		/// <summary>
		/// Lookups the esa by.
		/// </summary>
		/// <param name="searchText">The search text.</param>
		/// <param name="orgCode">The org code.</param>
		/// <param name="maxResults">The maximum results.</param>
		/// <returns></returns>
		List<SelectListItem> LookupESABy( string searchText, string orgCode, int maxResults );

		/// <summary>
		/// Lookups the site by.
		/// </summary>
		/// <param name="searchText">The search text.</param>
		/// <param name="orgCode">The org code.</param>
		/// <param name="esaCode">The esa code.</param>
		/// <param name="maxResults">The maximum results.</param>
		/// <returns></returns>
		List<SelectListItem> LookupSiteBy( string searchText, string orgCode, string esaCode, int maxResults );

		#endregion Reports

		#region ADW shortcuts

		/// <summary>
		/// Gets the claim type description for a claim type code.
		/// </summary>
		/// <param name="claimType">Type of the claim.</param>
		/// <returns>claim type description</returns>
		string GetClaimTypeDescription( string claimType );

		/// <summary>
		/// Gets the ESA description from a code.
		/// </summary>
		/// <param name="esaCode">The esa code.</param>
		/// <returns>ESA name</returns>
		string GetEsaDescription( string esaCode );

		/// <summary>
		/// Gets the site description.
		/// </summary>
		/// <param name="siteCode">The site code.</param>
		/// <returns></returns>
		string GetSiteDescription( string siteCode );

		/// <summary>
		/// Gets the contract type description from a contract type code.
		/// </summary>
		/// <param name="contractType">Type of the contract.</param>
		/// <returns>the Contract name</returns>
		string GetContractTypeDescription( string contractType );

		/// <summary>
		/// Gets the assessment description from the assessment code.
		/// </summary>
		/// <param name="assessmentCode">The assessment code.</param>
		/// <returns>assessment description</returns>
		string GetAssessmentDescription( string assessmentCode );

		/// <summary>
		/// Gets the recovery reason description from a recovery reason code.
		/// </summary>
		/// <param name="recoveryReason">The recovery reason.</param>
		/// <returns>recovery reason name</returns>
		string GetRecoveryReasonDescription( string recoveryReason );

		/// <summary>
		/// Gets the assessment action description from the assessment action code.
		/// </summary>
		/// <param name="assessmentAction">The assessment action.</param>
		/// <returns>assessment action description</returns>
		string GetAssessmentActionDescription( string assessmentAction );

		/// <summary>
		/// Gets the final outcome description from the final outcome code.
		/// </summary>
		/// <param name="outcomeCode">The outcome code.</param>
		/// <returns>final outcome description</returns>
		string GetFinalOutcomeDescription( string outcomeCode );

		/// <summary>
		/// Gets the final outcomes.
		/// </summary>
		/// <returns>array of final outcomes</returns>
		string[] GetFinalOutcomes();

		/// <summary>
		/// Gets the recovery reasons.
		/// </summary>
		/// <returns>array of Recovery reasons</returns>
		string[] GetRecoveryReasons();

		/// <summary>
		/// Gets the assessment codes.
		/// </summary>
		/// <returns>array of assessment codes</returns>
		string[] GetAssessmentCodes();

		/// <summary>
		/// Gets the assessment actions.
		/// </summary>
		/// <returns>array of assessment actions</returns>
		string[] GetAssessmentActions();

		/// <summary>
		/// Determines whether the specified org code is a valid org code.
		/// </summary>
		/// <param name="orgCode">The org code.</param>
		/// <returns>valid or not</returns>
		bool IsValidOrgCode( string orgCode );

		/// <summary>
		/// Gets the name of the orgfrom the Org Code using ADW.
		/// </summary>
		/// <param name="orgCode">The org code.</param>
		/// <returns></returns>
		string GetOrgName( string orgCode );

		/// <summary>
		/// Get adw code and description regardless of the end date status
		/// </summary>
		/// <param name="listCode"></param>
		/// <param name="codeValue"></param>
		/// <param name="selected">if true it will be set as selected</param>
		/// <returns></returns>
		SelectListItem GetAdwCode(string listCode, string codeValue, bool selected);

		#endregion ADW shortcuts

		#region Business Object = CheckList

		/// <summary>
		/// Gets the check list.
		/// </summary>
		/// <param name="reviewID">The review identifier.</param>
		/// <returns></returns>
		CheckList GetCheckList( int reviewID );

		/// <summary>
		/// Saves the check list.
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		CheckList SaveCheckList( CheckList viewModel );

		/// <summary>
		/// Gets the ZEIS0P82 details.
		/// </summary>
		/// <param name="claimID">The claim identifier.</param>
		/// <param name="claimSequenceNo">The claim sequence no.</param>
		/// <returns></returns>
		ZEIS0P82ViewModel GetZEIS0P82Details( long claimID, int claimSequenceNo );

		#endregion Business Object = CheckList

		#region Customisations

		/// <summary>
		/// Retrieves the grid customisations.
		/// </summary>
		/// <param name="vm">The view model.</param>
		/// <returns>a view of the grid customisations</returns>
		CustomiseReviewGridViewModel GetGridCustomisations( CustomiseReviewGridViewModel vm );

		/// <summary>
		/// Saves the grid customisations.
		/// </summary>
		/// <param name="vm">The view of the customisations.</param>
		void SaveGridCustomisations( CustomiseReviewGridViewModel vm );

		/// <summary>
		/// Resets the user's grid customisations back to the default.
		/// </summary>
		/// <param name="userId">The user identifier.</param>
		void ResetGridCustomisations( string userId );

		/// <summary>
		/// reorganises the grid customisations.
		/// </summary>
		/// <param name="vm">The view of the customisations.</param>
		/// <returns></returns>
		CustomiseReviewGridViewModel RefreshGridCustomisations( CustomiseReviewGridViewModel vm );

		#endregion Customisations

		#region Business Object = Review Questionnaire

		/// <summary>
		/// Gets the review questionnaire.
		/// </summary>
		/// <param name="reviewId">The review identifier.</param>
		/// <returns></returns>
		ReviewQuestionnaire GetReviewQuestionnaire( int reviewId );

		/// <summary>
		/// Gets the question answers.
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="reviewId">The review identifier.</param>
		/// <returns></returns>
		List<QuestionAnswer> GetQuestionAnswers( GridSettings gridSettings, int reviewId );

		/// <summary>
		/// Counts the question answers by review identifier.
		/// </summary>
		/// <param name="reviewId">The review identifier.</param>
		/// <returns></returns>
		int CountQuestionAnswersByReviewId( int reviewId );

		/// <summary>
		/// Gets the question answers by upload identifier.
		/// </summary>
		/// <param name="uploadId">The upload identifier.</param>
		/// <returns></returns>
		List<QuestionAnswer> GetQuestionAnswersByUploadId( int uploadId );

		/// <summary>
		/// Gets the review questionnaire data.
		/// </summary>
		/// <param name="uploadId">The upload identifier.</param>
		/// <returns></returns>
		List<ReviewQuestionnaire> GetReviewQuestionnaireData( int uploadId );

		/// <summary>
		/// Gets the review questionnaire data.
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="uploadId">The upload identifier.</param>
		/// <returns></returns>
		List<ReviewQuestionnaire> GetReviewQuestionnaireData( GridSettings gridSettings, int uploadId );

		/// <summary>
		/// Counts the review questionnaires by upload identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		int CountReviewQuestionnairesByUploadId( int id );

		/// <summary>
		/// Gets the project question text.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns></returns>
		List<String> GetProjectQuestionText( int projectId );

		/// <summary>
		/// Converts the questionnaire data to CSV.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns></returns>
		string ConvertQuestionnaireDataToCsv( List<ReviewQuestionnaire> data );

		#endregion Business Object = Review Questionnaire

	}
}