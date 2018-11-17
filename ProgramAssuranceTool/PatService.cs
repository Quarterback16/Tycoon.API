using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web.Mvc;
using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Infrastructure.Interfaces;
using ProgramAssuranceTool.Infrastructure.Services;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Models;
using ProgramAssuranceTool.Repositories;
using ProgramAssuranceTool.ViewModels;
using ProgramAssuranceTool.ViewModels.Claims;
using ProgramAssuranceTool.ViewModels.Project;
using ProgramAssuranceTool.ViewModels.Report;
using Filter = MvcJqGrid.Filter;
using GridSettings = MvcJqGrid.GridSettings;
using Rule = MvcJqGrid.Rule;

namespace ProgramAssuranceTool
{
	/// <summary>
	///    The Pat Service will if required combine repositories to comeup with the results required by the front
	///    end clients.  Repositories should besimple wrappers around a set of related stored procedures.
	/// </summary>
	public class PatService : IPatService
	{
		#region  Dependencies

		/// <summary>
		/// The ADW repository
		/// </summary>
		public IAdwRepository AdwRepository;
		/// <summary>
		/// The project repository
		/// </summary>
		protected readonly IProjectRepository ProjectRepository;
		/// <summary>
		/// The project contract repository
		/// </summary>
		protected readonly IProjectContractRepository ProjectContractRepository;
		/// <summary>
		/// The upload repository
		/// </summary>
		public readonly IUploadRepository UploadRepository;
		/// <summary>
		/// The review repository
		/// </summary>
		protected readonly IReviewRepository ReviewRepository;
		/// <summary>
		/// The check list repository
		/// </summary>
		protected readonly ICheckListRepository CheckListRepository;
		/// <summary>
		/// The questionnaire repository
		/// </summary>
		protected readonly IQuestionnaireRepository QuestionnaireRepository;
		/// <summary>
		/// The compliance indicator repository
		/// </summary>
		protected readonly IComplianceIndicatorRepository ComplianceIndicatorRepository;
		/// <summary>
		/// The claims repository
		/// </summary>
		protected readonly IClaimsRepository ClaimsRepository;
		/// <summary>
		/// The sample repository
		/// </summary>
		protected readonly ISampleRepository SampleRepository;
		/// <summary>
		/// The bulletin repository
		/// </summary>
		protected readonly IBulletinRepository BulletinRepository;
		/// <summary>
		/// The project attachment repository
		/// </summary>
		protected readonly IProjectAttachmentRepository ProjectAttachmentRepository;
		/// <summary>
		/// The control repository
		/// </summary>
		protected readonly IControlRepository ControlRepository;
		/// <summary>
		/// The user settings repository
		/// </summary>
		protected readonly IUserSettingsRepository UserSettingsRepository;
		/// <summary>
		/// The cache service
		/// </summary>
		protected readonly ICacheService CacheService;
		/// <summary>
		/// The audit service
		/// </summary>
		protected readonly IAuditService AuditService;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="PatService"/> class.
		/// </summary>
		public PatService()
		{
			AdwRepository = new AdwRepository();
			ProjectRepository = new ProjectRepository();
			ProjectContractRepository = new ProjectContractRepository();
			UploadRepository = new UploadRepository();
			ReviewRepository = new ReviewRepository();
			CheckListRepository = new CheckListRepository();
			QuestionnaireRepository = new QuestionnaireRepository();
			ComplianceIndicatorRepository = new ComplianceIndicatorRepository();
			ClaimsRepository = new ClaimsRepository();
			SampleRepository = new SampleRepository();
			BulletinRepository = new BulletinRepository();
			ProjectAttachmentRepository = new ProjectAttachmentRepository();
			ControlRepository = new ControlRepository();
			UserSettingsRepository = new UserSettingsRepository();
			CacheService = new HttpCacheService();
			AuditService = new AuditService();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PatService"/> class.
		/// </summary>
		/// <param name="adwRepository">The adw repository.</param>
		/// <param name="projectRepository">The project repository.</param>
		/// <param name="uploadRepository">The upload repository.</param>
		/// <param name="reviewRepository">The review repository.</param>
		/// <param name="checkListRepository">The check list repository.</param>
		/// <param name="questionnaireRepository">The questionnaire repository.</param>
		/// <param name="claimsRepository">The claims repository.</param>
		/// <param name="sampleRepository">The sample repository.</param>
		/// <param name="projectContractRepository">The project contract repository.</param>
		/// <param name="bulletinRepository">The bulletin repository.</param>
		/// <param name="projectAttachmentRepository">The project attachment repository.</param>
		/// <param name="controlRepository">The control repository.</param>
		/// <param name="userSettingsRepository">The user settings repository.</param>
		/// <param name="cacheService">The cache service.</param>
		/// <param name="auditService">The audit service.</param>
		public PatService(
			IAdwRepository adwRepository
			, IProjectRepository projectRepository
			, IUploadRepository uploadRepository
			, IReviewRepository reviewRepository
			, ICheckListRepository checkListRepository
			, IQuestionnaireRepository questionnaireRepository
			, IClaimsRepository claimsRepository
			, ISampleRepository sampleRepository
			, IProjectContractRepository projectContractRepository
			, IBulletinRepository bulletinRepository
			, IProjectAttachmentRepository projectAttachmentRepository
			, IControlRepository controlRepository
			, IUserSettingsRepository userSettingsRepository
			, ICacheService cacheService
			, IAuditService auditService
			)
		{
			//  Dependency inhection version, unit tests can pass in fake or mock repositories
			AdwRepository = adwRepository;
			ProjectRepository = projectRepository;
			UploadRepository = uploadRepository;
			ReviewRepository = reviewRepository;
			CheckListRepository = checkListRepository;
			QuestionnaireRepository = questionnaireRepository;
			ClaimsRepository = claimsRepository;
			SampleRepository = sampleRepository;
			ProjectContractRepository = projectContractRepository;
			BulletinRepository = bulletinRepository;
			ProjectAttachmentRepository = projectAttachmentRepository;
			ControlRepository = controlRepository;
			UserSettingsRepository = userSettingsRepository;
			CacheService = cacheService;
			AuditService = auditService;
		}

		#endregion Constructors

		#region Business Object = Project

		/// <summary>
		/// Gets a Project record based on an ID
		/// </summary>
		/// <param name="projectId"></param>
		/// <returns>
		/// Project object
		/// </returns>
		public Project GetProject( int projectId )
		{
			return ProjectRepository.GetById( projectId );
		}

		/// <summary>
		/// Updates the project.
		/// </summary>
		/// <param name="project">The project.</param>
		public void UpdateProject( Project project )
		{
			ProjectRepository.Update( project );
		}

		/// <summary>
		/// Creates a new Project
		/// </summary>
		/// <param name="project">The candidate project object</param>
		/// <returns>
		/// the id of the newly created project
		/// </returns>
		public int CreateProject( Project project )
		{
			ProjectRepository.Add( project );
			var newId = project.ProjectId;
			var userActivity = string.Format( "Project {0} added by {1}", newId, project.CreatedBy );
			SaveActivity( userActivity, project.CreatedBy );
			return newId;
		}

		/// <summary>
		/// Checks to see if a project name has already been used
		/// </summary>
		/// <param name="projectName">the project name</param>
		/// <param name="projectId">the ID of the project that has the name</param>
		/// <returns>
		/// used or not used
		/// </returns>
		public bool IsProjectNameUsed( string projectName, int projectId )
		{
			return ProjectRepository.IsProjectNameUsed( projectName, projectId );
		}

		/// <summary>
		/// Gets a collection of project record based on the parameters in the GridSettings object
		/// </summary>
		/// <param name="gridSettings">selection criteria</param>
		/// <returns>
		/// List of Project objects
		/// </returns>
		public List<Project> GetProjects( GridSettings gridSettings )
		{
			var results = ProjectRepository.GetAll( gridSettings, new DateTime(1,1,1), new DateTime(1,1,1) );
			var list = new List<Project>();
			list.AddRange( results );
			return list;
		}

		/// <summary>
		/// Gets a collection of project record based on the parameters in the GridSettings object
		///    and a date range
		/// 	Normal Project Search but now with an non Project Table extra criteria
		/// </summary>
		/// <param name="gridSettings">selection criteria</param>
		/// <param name="uploadedFrom">from date</param>
		/// <param name="uploadedTo">to date</param>
		/// <returns>
		/// List of Project objects
		/// </returns>
		public List<Project> GetProjects( GridSettings gridSettings, DateTime uploadedFrom, DateTime uploadedTo )
		{
			SaveActivity( string.Format( "Project Search {0} {1:d} {2:d}",
													AppHelper.SearchCriteria( gridSettings ), uploadedFrom, uploadedTo ), AppHelper.Environment() );

//			var filteredList = new List<Project>();
			var results = ProjectRepository.GetAll( gridSettings, uploadedFrom, uploadedTo  );
			var list = new List<Project>();
			list.AddRange( results );

			//if ( uploadedFrom == new DateTime( 1, 1, 1 ) && uploadedTo == new DateTime( 1, 1, 1 ) )
				return list;

			//filteredList.AddRange( list.Where( project => HasUploadinRange( project, uploadedFrom, uploadedTo ) ) );

			//return filteredList;
		}

		//private bool HasUploadinRange( Project project, DateTime uploadedFrom, DateTime uploadedTo )
		//{
		//	var gs = AppHelper.DefaultGridSettings();
		//	var uploads = UploadRepository.GetAllByProjectId( project.ProjectId, gs );
		//	return uploads.Any( upload => upload.DateUploaded.Date >= uploadedFrom && upload.DateUploaded.Date <= uploadedTo );
		//}

		/// <summary>
		/// Gets the projects.
		/// </summary>
		/// <returns>list of projects</returns>
		public List<Project> GetProjects()
		{
			var results = ProjectRepository.GetAll(AppHelper.DefaultGridSettings(), 
				new DateTime(1,1,1), new DateTime(1,1,1) );
			var list = new List<Project>();
			list.AddRange( results );
			return list;
		}

		/// <summary>
		/// Find up to n projects by part of the name
		///   used by Autocomplete fields
		/// </summary>
		/// <param name="searchText"></param>
		/// <param name="maxResults">The maximum number of projects to find</param>
		/// <returns>
		/// List of Project objects
		/// </returns>
		public List<Project> LookupProject( string searchText, int maxResults )
		{
			var results = new List<Project>();

			int number;
			if ( int.TryParse( searchText, out number ) )
			{
				var project = ProjectRepository.GetById( number );
				if ( project != null )
				{
					results.Add( project );
				}
			}
			else
			{
				var gridSettings = new GridSettings
					{
						IsSearch = true,
						PageSize = 99999999,
						PageIndex = 1,
						SortColumn = "ProjectName",
						SortOrder = "asc"
					};

				var whereClause = new Filter { rules = new Rule[ 1 ] };
				whereClause.rules[ 0 ] = new Rule
					{
						field = "ProjectName",
						op = DataConstants.SqlOperationContains,
						data = searchText
					};
				gridSettings.Where = whereClause;

				results = ProjectRepository.GetAll( gridSettings, new DateTime(1,1,1), new DateTime(1,1,1)  );
			}

			return results.Take( maxResults ).ToList();
		}

		/// <summary>
		/// Counts the number of projects matching the selection criteria
		///   including an upload date range
		/// </summary>
		/// <param name="gridSettings">selection criteria</param>
		/// <param name="uploadedFrom">from date</param>
		/// <param name="uploadedTo">to date</param>
		/// <returns>
		/// the count of projects
		/// </returns>
		public int CountProjects( GridSettings gridSettings, DateTime uploadedFrom, DateTime uploadedTo )
		{
			//  We want to count all of the projects that match, including the filter criteria but disregarding the page size
			var myGs = new GridSettings
				{
					SortColumn = gridSettings.SortColumn,
					SortOrder = gridSettings.SortOrder,
					Where = gridSettings.Where ?? new Filter(),
					PageIndex = 1,
					PageSize = 99999999,
					IsSearch = true
				};
			if ( myGs.Where.rules == null )
				myGs.IsSearch = false;

			var projectList = ProjectRepository.GetAll( myGs, uploadedFrom, uploadedTo );

//			if ( uploadedFrom == new DateTime( 1, 1, 1 ) && uploadedTo == new DateTime( 1, 1, 1 ) )
				return projectList.Count;

			////  apply the upload range criteria
			//var filteredList = new List<Project>();
			//filteredList.AddRange( projectList.Where( project => HasUploadinRange( project, uploadedFrom, uploadedTo ) ) );
			//return filteredList.Count;
		}

		/// <summary>
		/// Counts the number of projects matching the selection criteria
		/// </summary>
		/// <param name="gridSettings">selection criteria</param>
		/// <returns>
		/// the count
		/// </returns>
		public int CountProjects( GridSettings gridSettings )
		{
			if ( !gridSettings.IsSearch ) return ProjectRepository.CountProjects();

			var myGs = new GridSettings
				{
					SortColumn = gridSettings.SortColumn,
					SortOrder = gridSettings.SortOrder,
					Where = gridSettings.Where,
					PageIndex = 1,
					PageSize = 99999999,
					IsSearch = true
				};

			var projectList = ProjectRepository.GetAll( myGs, new DateTime(1,1,1), new DateTime(1,1,1)  );
			return projectList.Count;
		}

		/// <summary>
		/// Looks at the database and determines which project types have been used
		/// </summary>
		/// <returns>
		/// array of project type codes
		/// </returns>
		public string[] GetAllDistinctProjectTypes()
		{
			return ProjectRepository.GetAllProjectTypes();
		}

		/// <summary>
		/// Get the description of the project type given the c
		/// </summary>
		/// <param name="projectType">project type code</param>
		/// <returns>
		/// description
		/// </returns>
		public string GetProjectTypeDescription( string projectType )
		{
			return AdwRepository.GetDescription( DataConstants.AdwListCodeForProjectTypes, projectType );
		}

		/// <summary>
		/// Deletes a project from existance
		/// </summary>
		/// <param name="id">the identifier of the project being deleted</param>
		/// <param name="userId">the user id of the person requesting the delete</param>
		/// <returns></returns>
		public bool DeleteProject( int id, string userId )
		{
			ProjectRepository.Delete( id );
			var userActivity = string.Format( "Project {0} deleted by {1}", id, userId );
			SaveActivity( userActivity, userId );
			return true;
		}

		/// <summary>
		/// The number of uploads a particular project has
		/// </summary>
		/// <param name="projectId">the id of the project</param>
		/// <returns>
		/// the count
		/// </returns>
		public int CountUploads( int projectId )
		{
#if DEBUG
			var stopwatch = new Stopwatch();
			stopwatch.Start();
#endif
			var uploadCount = UploadRepository.CountUploads( projectId );
#if DEBUG
			var elapsed = AppHelper.StopTheWatch( stopwatch );
			SaveActivity( string.Format( "CountUploads {1} took {0}", elapsed, projectId ), "PRJDETS" );

#endif
			return uploadCount;
		}

		/// <summary>
		/// Counts the reviews for a project.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>
		/// the count
		/// </returns>
		public int CountReviews( int projectId )
		{
			var reviewCount = ReviewRepository.CountReviews( projectId );
			return reviewCount;
		}

		/// <summary>
		/// Counts the reviews.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="gs">The gs.</param>
		/// <returns></returns>
		public int CountReviews( int projectId, GridSettings gs )
		{
			var myGs = new GridSettings
				{
					SortColumn = gs.SortColumn,
					SortOrder = gs.SortOrder,
					Where = gs.Where ?? new Filter(),
					PageIndex = 1,
					PageSize = 99999999,
					IsSearch = true
				};
			if ( myGs.Where.rules == null )
				myGs.IsSearch = false;
			var list = ReviewRepository.GetAllByProjectIdPaged( projectId, myGs );
			return list.Count();
		}

		/// <summary>
		/// Works out if the Project has findings. 
		/// </summary>
		/// <param name="projectId">The identifier.</param>
		/// <returns>
		/// yes it has or no it doesnt
		/// </returns>
		public bool ProjectHasFindings( int projectId )
		{
			var nFinished = ReviewRepository.CountFinishedReviews( projectId );
			return nFinished > 0;
		}

		/// <summary>
		///    Get all the contracts
		/// </summary>
		/// <returns></returns>
		public List<AdwItem> GetProjectContracts()
		{
			var list = AdwRepository.ListCode( DataConstants.AdwListCodeForProjectContracts );
			return list.Select( item => new AdwItem { Code = item.Value, Description = item.Text } ).ToList();
		}

		/// <summary>
		/// Gets the project uploads.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="gs">The gs.</param>
		/// <returns></returns>
		public List<Upload> GetProjectUploads( int projectId, GridSettings gs )
		{
			return UploadRepository.GetAllByProjectId( projectId, gs );
		}

		/// <summary>
		/// Gets the project reviews.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="gs">The gs.</param>
		/// <returns></returns>
		public List<Review> GetProjectReviews( int projectId, GridSettings gs )
		{
			var list = new List<Review>();
			if ( projectId > 0 )
			   list = ReviewRepository.GetAllByProjectIdPaged( projectId, gs );
			return list;
		}

		/// <summary>
		/// Gets the review list search terms for the Grid filters.
		/// This is way too slow for large projects
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="orgCodes">The org codes.</param>
		/// <param name="siteCodes">The site codes.</param>
		/// <param name="esaCodes">The esa codes.</param>
		/// <param name="stateCodes">The state codes.</param>
		public void GetReviewListSearchTerms( int projectId, out string[] orgCodes, out string[] siteCodes,
														  out string[] esaCodes, out string[] stateCodes )
		{
#if DEBUG
			var stopwatch = new Stopwatch();
			stopwatch.Start();
#endif
			var reviewList = ReviewRepository.GetAllByProjectId( projectId );
			var orgDict = new Dictionary<string, int>();
			var esaDict = new Dictionary<string, int>();
			var siteDict = new Dictionary<string, int>();
			var stateDict = new Dictionary<string, int>();
			foreach ( var reviewItem in reviewList )
			{
				if ( reviewItem.OrgCode != null )
					AppHelper.AddToDict( reviewItem.OrgCode, orgDict );
				if ( reviewItem.ESACode != null )
					AppHelper.AddToDict( reviewItem.ESACode, esaDict );
				if ( reviewItem.SiteCode != null )
					AppHelper.AddToDict( reviewItem.SiteCode, siteDict );
				if ( reviewItem.StateCode != null )
					AppHelper.AddToDict( reviewItem.StateCode, stateDict );
			}
			orgCodes = orgDict.Select( x => x.Key ).ToArray();
			siteCodes = siteDict.Select( x => x.Key ).ToArray();
			esaCodes = esaDict.Select( x => x.Key ).ToArray();
			stateCodes = stateDict.Select( x => x.Key ).ToArray();
#if DEBUG
			var elapsed = AppHelper.StopTheWatch( stopwatch );
			SaveActivity( string.Format( "GetReviewListSearchTerms {1} took {0}", elapsed, projectId ), "SQL-REVSCH" );
#endif
		}

		/// <summary>
		/// Exports the reviews to a CSV file.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="gs">The grid settings.</param>
		/// <returns>
		/// CSV string
		/// </returns>
		public string ExportReviews( int projectId, GridSettings gs )
		{
			var re = new ReviewExporter();
			var reviewList = ReviewRepository.GetAllByProjectIdPaged( projectId, gs );
			return re.ExportReviews( reviewList, this );
		}

		/// <summary>
		/// Exports the reviews for a particular upload to CSV.
		/// </summary>
		/// <param name="uploadId">The upload identifier.</param>
		/// <returns>
		/// CSV string
		/// </returns>
		public string ExportReviewsForUpload( int uploadId )
		{
			var re = new ReviewExporter();
			var reviewList = ReviewRepository.GetAllByUploadId( uploadId );
			return re.ExportReviews( reviewList, this );
		}

		/// <summary>
		/// Projects the is contract monitoring or contract site visit.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		public bool ProjectIsContractMonitoringOrContractSiteVisit( int id )
		{
			var project = GetProject( id );
			return project != null && project.IsContractMonitoringOrContractSiteVisit();
		}

		#endregion Business Object = Project

		#region Business Object = Project Questions

		/// <summary>
		/// Validates the question upload CSV file.
		/// </summary>
		/// <param name="stream">The memory stream based on the CSV file.</param>
		/// <returns>
		/// a collection of validation errors
		/// </returns>
		public IEnumerable<CellValidationError> ValidateQuestionUpload( Stream stream )
		{
			var validator = new UploadQuestionsValidator();
			return validator.ValidateFile( stream, AdwRepository );
		}

		/// <summary>
		/// Stores the project question data from a memory stream.
		/// </summary>
		/// <param name="vm">The view model.</param>
		/// <param name="stream">The memory stream.</param>
		/// <returns>
		/// number of records stored
		/// </returns>
		public int StoreProjectQuestionData( UploadQuestionsViewModel vm, Stream stream )
		{
			var nQuestions = ProjectRepository.StoreQuestionData( vm, stream );
			return nQuestions;
		}

		/// <summary>
		/// Gets the project questions contained in the Questionnaire for the project.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>
		/// List of Questions
		/// </returns>
		public List<PatQuestion> GetProjectQuestions( int projectId )
		{
			//  This way works out the questions from Camenza's question schema
			var list = QuestionnaireRepository.GetProjectQuestions( projectId );
			return list.Select( question => new PatQuestion { ProjectId = projectId, Text = question } ).ToList();
		}

		#endregion Business Object = Project Questions

		#region Business Object = Project Contract

		/// <summary>
		/// Creates the project contract.
		/// </summary>
		/// <param name="projectContract">The project contract.</param>
		/// <returns></returns>
		public int CreateProjectContract( ProjectContract projectContract )
		{
			ProjectContractRepository.Add( projectContract );
			var newId = projectContract.Id;
			return newId;
		}

		/// <summary>
		/// Gets the project contracts by project.
		/// </summary>
		/// <param name="id">The project identifier.</param>
		/// <returns>
		/// The contracts for the project
		/// </returns>
		public List<ProjectContract> GetProjectContractsByProject( int id )
		{
			var list = ProjectContractRepository.GetAllByProjectId( id );
			return list;
		}

		/// <summary>
		/// Counts the contracts for a particular project ID.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>
		/// theContract count
		/// </returns>
		public int CountContracts( int projectId )
		{
			return ProjectContractRepository.GetAllByProjectId( projectId ).Count;
		}

		/// <summary>
		/// Saves the project contracts.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="list">The list of contracts.</param>
		/// <param name="userId">The user identifier.</param>
		public void SaveProjectContracts( int projectId, List<ProjectContract> list, string userId )
		{
			ProjectContractRepository.SaveProjectContracts( projectId, list, userId );
		}

		/// <summary>
		/// Saves the project contracts.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="list">The list.</param>
		/// <param name="userId">The user identifier.</param>
		public void SaveProjectContracts( int projectId, string[] list, string userId )
		{
			var gList =
				list.Select(
					contractType => new ProjectContract { ContractType = contractType, ProjectId = projectId, CreatedBy = userId } )
					 .ToList();
			ProjectContractRepository.SaveProjectContracts( projectId, gList, userId );
		}

		/// <summary>
		/// Gets the project contract selections.
		///  return a CSV string of Project Contracts
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		public string GetProjectContractSelections( int id )
		{
			var selections = string.Empty;
			var list = GetProjectContractsByProject( id );
			selections = list.Aggregate( selections, ( current, pc ) => current + ( pc.ContractType + "," ) );
			return selections.Length > 0 ? selections.Remove( selections.Length - 1, 1 ) : selections;
		}

		#endregion Business Object = Project Contract

		#region Business Object = Project Attachment

		/// <summary>
		/// Stores the attachment into Sql Server.
		/// </summary>
		/// <param name="fileData">The file data in the form of a byte array.</param>
		/// <param name="attachmentId">The attachment identifier.</param>
		public void StoreAttachment( byte[] fileData, int attachmentId )
		{
			var repository = new ProjectAttachmentRepository();
			repository.StoreAttachment( fileData, attachmentId );
		}

		/// <summary>
		/// Retrieves the attachment from the database as a .
		/// </summary>
		/// <param name="attachmentId">The attachment identifier.</param>
		/// <returns>
		/// a byte array representing
		/// </returns>
		public byte[] RetrieveAttachment( int attachmentId )
		{
			var repository = new ProjectAttachmentRepository();
			return repository.RetrieveAttachment( attachmentId );
		}

		/// <summary>
		/// Gets all the attachments for a project based on grid criteria.
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="projectId">The project identifier.</param>
		/// <returns></returns>
		public List<ProjectAttachment> GetAttachments( GridSettings gridSettings, int projectId )
		{
			return ProjectAttachmentRepository.GetAll( gridSettings, projectId );
		}

		/// <summary>
		/// Counts the attachments for a project based on grid criteria.
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>
		/// the number of attachments
		/// </returns>
		public int CountAttachments( GridSettings gridSettings, int projectId )
		{
			if ( !gridSettings.IsSearch ) return ProjectAttachmentRepository.Count( projectId );

			var myGs = new GridSettings
				{
					SortColumn = gridSettings.SortColumn,
					SortOrder = gridSettings.SortOrder,
					Where = gridSettings.Where,
					PageIndex = 1,
					PageSize = 99999999,
					IsSearch = true
				};

			var projectAttachmentCount = ProjectAttachmentRepository.Count( myGs, projectId );
			return projectAttachmentCount;
		}

		/// <summary>
		/// Gets the attachment.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		public ProjectAttachment GetAttachment( int id )
		{
			return ProjectAttachmentRepository.GetById( id );
		}

		/// <summary>
		/// Deletes the attachment given the attachment identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		public void DeleteAttachment( int id )
		{
			ProjectAttachmentRepository.Delete( id );
		}

		/// <summary>
		/// Saves the attachment into the database.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <param name="fileData">The file data.</param>
		/// <returns>
		/// A projectAttachment onject
		/// </returns>
		public ProjectAttachment SaveAttachment( ProjectAttachment model, byte[] fileData )
		{
			if (model.Id < 1)
			{
				ProjectAttachmentRepository.Insert(model, fileData);
				var activity = string.Format(@"Project Document: {0} - {1} created successfully.", model.Id, model.DocumentName);
				SaveActivity(activity, model.CreatedBy);
			}
			else
			{
				ProjectAttachmentRepository.Update(model);
				var activity = string.Format(@"Project Document: {0} - {1} updated successfully.", model.Id, model.DocumentName);
				SaveActivity(activity, model.UpdatedBy);
			}

			return model;
		}

		#endregion Business Object = Project Attachment

		#region Business Object = User Activity

		/// <summary>
		/// Saves the activity log item given the log item and an existing repository.
		/// </summary>
		/// <param name="userActivity">The user activity.</param>
		/// <param name="uaRep">The ua rep.</param>
		public void SaveActivity( UserActivity userActivity, UserActivityRepository<UserActivity> uaRep )
		{
			uaRep.Add( userActivity );
		}

		/// <summary>
		/// Saves an audit log item.
		/// </summary>
		/// <param name="userActivity">The user log item.</param>
		public void SaveActivity( UserActivity userActivity )
		{
			var activityRepository = new UserActivityRepository<UserActivity>();
			activityRepository.Add( userActivity );
		}

		/// <summary>
		/// Saves the log item given the message and the user id.
		/// </summary>
		/// <param name="activity">The activity.</param>
		/// <param name="userId">The user identifier.</param>
		public void SaveActivity( string activity, string userId )
		{
			var userActivity = new UserActivity
				{
					Activity = activity,
					UserId = userId
				};
			var activityRepository = new UserActivityRepository<UserActivity>();
			activityRepository.Add( userActivity );
		}

		/// <summary>
		/// Gets the audit records for a particular user and date range.
		/// </summary>
		/// <param name="userId">The user identifier.</param>
		/// <param name="from">audit transactions From.</param>
		/// <param name="to">audit transactions To.</param>
		/// <returns>
		/// list of audit transactions
		/// </returns>
		public List<UserActivity> GetActivities( string userId, DateTime from, DateTime to )
		{
			var activityRepository = new UserActivityRepository<UserActivity>();
			return activityRepository.GetAll( userId, from, to );
		}

		/// <summary>
		/// Deletes the audit log items prior to a certain date.
		/// </summary>
		/// <param name="priorToDate">The prior to date.</param>
		public void DeleteLogItemsPriorTo( DateTime priorToDate )
		{
			var activityRepository = new UserActivityRepository<UserActivity>();
			activityRepository.DeletePriorTo( priorToDate );
		}

		#endregion Business Object = User Activity

		#region Business Object = Bulletin

		/// <summary>
		/// Gets the bulletin type description.
		/// </summary>
		/// <param name="bulletinType">Type of the bulletin.</param>
		/// <returns></returns>
		public string GetBulletinTypeDescription(string bulletinType)
		{
			return AdwRepository.GetDescription(DataConstants.AdwListCodeForBulletinTypes, bulletinType);
		}

		/// <summary>
		/// Gets the bulletins by its type and admin privilage
		/// </summary>
		/// <param name="bulletinType">Type of the bulletin.</param>
		/// <param name="isAdmin">if set to <c>true</c> [is admin].</param>
		/// <returns>
		/// a list of bulletin
		/// </returns>
		public List<Bulletin> GetBulletins( string bulletinType, bool isAdmin )
		{
			var result = BulletinRepository.GetAll( bulletinType, isAdmin );
			var list = new List<Bulletin>();
			list.AddRange( result );
			return list;
		}

		/// <summary>
		/// Gets the bulletins by its grid setting, type and admin privilage
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="bulletinType">Type of the bulletin.</param>
		/// <param name="isAdmin">if set to <c>true</c> [is admin].</param>
		/// <returns>
		/// a list of bulletin
		/// </returns>
		public List<Bulletin> GetBulletins( GridSettings gridSettings, string bulletinType, bool isAdmin )
		{
			var result = BulletinRepository.GetAll( gridSettings, bulletinType, isAdmin );
			var list = new List<Bulletin>();
			list.AddRange( result );
			return list;
		}

		/// <summary>
		/// Gets the bulletin by its id.
		/// </summary>
		/// <param name="id">The bulletin id.</param>
		/// <returns>
		/// bulletin
		/// </returns>
		public Bulletin GetBulletin( int id )
		{
			return BulletinRepository.GetById( id );
		}

		/// <summary>
		/// Updates the bulletin.
		/// </summary>
		/// <param name="bulletin">The bulletin.</param>
		/// <returns></returns>
		public Bulletin UpdateBulletin( Bulletin bulletin )
		{
			BulletinRepository.Update( bulletin );
			var userActivity = string.Format("Bulletin {0} updated by {1}", bulletin.BulletinId, bulletin.UpdatedBy);
			SaveActivity(userActivity, bulletin.UpdatedBy);
			return bulletin;
		}

		/// <summary>
		/// Creates the bulletin.
		/// </summary>
		/// <param name="bulletin">The bulletin.</param>
		/// <returns></returns>
		public int CreateBulletin( Bulletin bulletin )
		{
			BulletinRepository.Add( bulletin );
			var newID = bulletin.BulletinId;
			var userActivity = string.Format( "Bulletin {0} added by {1}", newID, bulletin.CreatedBy );
			SaveActivity( userActivity, bulletin.CreatedBy );
			return newID;
		}

		/// <summary>
		/// Deletes the bulletin by its id
		/// </summary>
		/// <param name="id">The bulletin id.</param>
		public void DeleteBulletin( int id )
		{
			BulletinRepository.Delete( id );
		}

		/// <summary>
		/// Counts the bulletins by its grid setting, type and admin privilage
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="bulletinType">Type of the bulletin.</param>
		/// <param name="isAdmin">if set to <c>true</c> [is admin].</param>
		/// <returns>
		/// a number of bulletin as specified in criteria
		/// </returns>
		public int CountBulletins( GridSettings gridSettings, string bulletinType, bool isAdmin )
		{
			if ( !gridSettings.IsSearch ) return BulletinRepository.Count( bulletinType, isAdmin );

			var myGs = new GridSettings
				{
					SortColumn = gridSettings.SortColumn,
					SortOrder = gridSettings.SortOrder,
					Where = gridSettings.Where,
					PageIndex = 1,
					PageSize = 99999999,
					IsSearch = true
				};

			var bulletinCount = BulletinRepository.Count( myGs, bulletinType, isAdmin );
			return bulletinCount;
		}

		#endregion Business Object = Bulletin

		#region Business Object = UserSetting

		/// <summary>
		/// Resets the user's grid customisations back to the default.
		/// </summary>
		/// <param name="userId">The user identifier.</param>
		public void ResetGridCustomisations( string userId )
		{
			UserSettingsDelete( userId );
			var customiser = new GridCustomiser( UserSettingsRepository );
			var list = customiser.GetDefaultUserSettings( userId );
			UserSettingInsertAll( list );
		}

		/// <summary>
		/// Retrieves the grid customisations.
		/// </summary>
		/// <param name="vm">The view model.</param>
		/// <returns>
		/// a view of the grid customisations
		/// </returns>
		public CustomiseReviewGridViewModel GetGridCustomisations( CustomiseReviewGridViewModel vm )
		{
			var list = GetAllUserSettings( vm.UserId );
			var customiser = new GridCustomiser( UserSettingsRepository );
			var vmOut = customiser.GetGridViewModelFromUserSettings( list );
			vmOut.UserId = vm.UserId;
			vmOut.UploadId = vm.UploadId;
			return vmOut;
		}

		/// <summary>
		/// reorganises the grid customisations.
		/// </summary>
		/// <param name="vm">The view of the customisations.</param>
		/// <returns></returns>
		public CustomiseReviewGridViewModel RefreshGridCustomisations( CustomiseReviewGridViewModel vm )
		{
			var customiser = new GridCustomiser( UserSettingsRepository );
			var vmOut = customiser.RefreshCustomisations( vm );
			return vmOut;
		}

		/// <summary>
		/// Saves the grid customisations.
		/// </summary>
		/// <param name="vm">The view of the customisations.</param>
		public void SaveGridCustomisations( CustomiseReviewGridViewModel vm )
		{
			var customiser = new GridCustomiser( UserSettingsRepository );
			var list = customiser.GetUserSettingsFromGridViewModel( vm );
			UserSettingsDelete( vm.UserId );
			UserSettingInsertAll( list );
		}

		private void UserSettingInsertAll( List<UserSetting> userSettings )
		{
			UserSettingsRepository.InsertAll( userSettings );
		}

		/// <summary>
		/// Users the setting insert.
		/// </summary>
		/// <param name="userSetting">The user setting.</param>
		public void UserSettingInsert( UserSetting userSetting )
		{
			var repository = new UserSettingsRepository();
			repository.Add( userSetting );
		}

		/// <summary>
		/// Gets all user settings.
		/// </summary>
		/// <param name="userId">The user identifier.</param>
		/// <returns></returns>
		public List<UserSetting> GetAllUserSettings( string userId )
		{
			var list = UserSettingsRepository.GetAllByUserId( userId );
			return list.ToList();
		}

		/// <summary>
		/// Gets the settings for.
		/// </summary>
		/// <param name="userId">The user identifier.</param>
		/// <returns></returns>
		public List<UserSetting> GetSettingsFor( string userId )
		{
			var repository = new UserSettingsRepository();
			var results = repository.GetAllByUserId( userId );
			var list = new List<UserSetting>();
			list.AddRange( results );
			return list;
		}

		/// <summary>
		/// Users the settings delete.
		/// </summary>
		/// <param name="userId">The user identifier.</param>
		public void UserSettingsDelete( string userId )
		{
			UserSettingsRepository.DeleteAll( userId );
		}

		#endregion Business Object = UserSetting

		#region Business Object = PAT User

		/// <summary>
		/// Gets the list of program assurance users.
		/// </summary>
		/// <returns>
		/// list of user objects
		/// </returns>
		public List<PatUser> GetProgramAssuranceUsers()
		{
			var userRepository = new UserRepository( AuditService );
			return userRepository.GetPatUsers( CacheService );
		}

		/// <summary>
		/// Gets the data to populate into the program assurance users dropdown list.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<SelectListItem> GetProgramAssuranceDropdownList()
		{
			var userRepository = new UserRepository( AuditService );
			return userRepository.GetProgramAssuranceDropdownList( CacheService );
		}

		/// <summary>
		///  Returns a PatUser object for the person logged in.
		/// </summary>
		/// <param name="user">The user.</param>
		/// <returns>a Pat User Object</returns>
		public PatUser LoggedInUser( WindowsIdentity user )
		{
			var userRepository = new UserRepository();
			return userRepository.PatUserFor( user );
		}

		/// <summary>
		/// The name of the Active directory.
		/// </summary>
		/// <returns></returns>
		public string ActiveDirectoryName()
		{
			var userRepository = new UserRepository();
			return userRepository.LdapGroupQueryPath;
		}

		/// <summary>
		/// Gets the program assurance groups.
		/// </summary>
		/// <returns>a collection of Project Assurance Group names</returns>
		public IEnumerable<string> GetProgramAssuranceGroups()
		{
			var userRepository = new UserRepository( AuditService );
			return userRepository.GetProgramAssuranceGroups();
		}

		/// <summary>
		/// Gets the list of users who are program assurance admin users.
		/// </summary>
		/// <returns>
		/// list of user objects
		/// </returns>
		public List<PatUser> GetProgramAssuranceAdminUsers()
		{
			var userRepository = new UserRepository( AuditService );
			return userRepository.GetProgramAssuranceAdminUsers( CacheService );
		}

		#endregion Business Object = PAT User

		#region Business Object = Upload or Sample

		/// <summary>
		/// Retrieves an upload by its identifier.
		/// </summary>
		/// <param name="uploadId">The upload identifier.</param>
		/// <returns>
		/// upload record
		/// </returns>
		public Upload GetUploadById( int uploadId )
		{
			return UploadRepository.GetById( uploadId );
		}

		/// <summary>
		/// Gets all the reviews in the upload that match the grid criteria.
		/// </summary>
		/// <param name="uploadId">The upload identifier.</param>
		/// <param name="gs">The gridsettings.</param>
		/// <returns></returns>
		public List<Review> GetUpload( int uploadId, GridSettings gs )
		{
			return ReviewRepository.GetAllByUploadIdPaged( uploadId, gs );
		}

		/// <summary>
		/// Updates the upload record.
		/// </summary>
		/// <param name="upload">The upload record.</param>
		public void UpdateUpload( Upload upload )
		{
			UploadRepository.Update( upload );
		}

		/// <summary>
		/// Counts the reviews by upload identifier.
		/// </summary>
		/// <param name="uploadId">The upload identifier.</param>
		/// <returns>
		/// the number of reviews an upload contains
		/// </returns>
		public int CountReviewsByUploadId( int uploadId )
		{
			return ReviewRepository.CountReviewsByUploadId( uploadId );
		}

		/// <summary>
		/// Counts the completed reviews by upload identifier.
		/// </summary>
		/// <param name="uploadId">The upload identifier.</param>
		/// <returns></returns>
		public int CountCompletedReviewsByUploadId( int uploadId )
		{
			return ReviewRepository.CountCompletedReviewsByUploadId( uploadId );
		}

		/// <summary>
		/// Gets the earliest and latest upload dates on record.
		/// </summary>
		/// <param name="latest">The latest upload.</param>
		/// <returns>
		/// the earliest upload
		/// </returns>
		public DateTime GetEarliestAndLatestUploadDates( out DateTime latest )
		{
			latest = DateTime.Now;

			var list = UploadRepository.GetAll();
			var firstUpload = list.FirstOrDefault();
			var lastUpload = list.LastOrDefault();

			if ( lastUpload != null ) latest = lastUpload.DateUploaded;
			return firstUpload != null ? firstUpload.DateUploaded : new DateTime( 1, 1, 1 );
		}

		/// <summary>
		/// Validates the uploaded CSV data.
		/// </summary>
		/// <param name="stream">Tye CSV upload as a memory stream.</param>
		/// <param name="includesOutcomes">if set to <c>true</c> [includes outcomes].</param>
		/// <returns>
		/// a collection of validation errors
		/// </returns>
		public IEnumerable<CellValidationError> ValidateUpload( Stream stream, bool includesOutcomes )
		{
			var validator = new UploadValidator();
			return validator.ValidateFile( stream, AdwRepository, includesOutcomes );
		}

		/// <summary>
		/// Creates the upload record.
		/// </summary>
		/// <param name="upload">The upload object.</param>
		/// <returns>
		/// the id of the newly created upload or 0
		/// </returns>
		public int CreateUpload( Upload upload )
		{
			UploadRepository.Add( upload );
			var newId = upload.UploadId;
			var userActivity = string.Format( "Upload {0} added by {1}", newId, upload.CreatedBy );
			SaveActivity( userActivity, upload.CreatedBy );
			return newId;
		}

		/// <summary>
		/// Deletes the upload record.
		/// </summary>
		/// <param name="upload">The upload record.</param>
		public void DeleteUpload( Upload upload )
		{
			AppHelper.DeleteFile( upload.ServerFile );
			UploadRepository.Delete( upload.UploadId );
			var userActivity = string.Format( "Upload {0} deleted by {1}", upload.UploadId, upload.UpdatedBy );
			SaveActivity( userActivity, upload.CreatedBy );
		}

		/// <summary>
		/// Checks if the Samples name is used.
		/// </summary>
		/// <param name="sampleName">Name of the sample.</param>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>
		/// whether or not the sample name is used
		/// </returns>
		public bool SampleNameIsUsed( string sampleName, int projectId )
		{
			var list = UploadRepository.GetAllByProjectId( projectId, AppHelper.DefaultGridSettings() );
			return list.Any( upload => upload.Name.Equals( sampleName ) );
		}

		#endregion Business Object = Upload or Sample

		#region Business Object = Review

		/// <summary>
		/// Gets a particular review based on its id.
		/// </summary>
		/// <param name="reviewId">The review identifier.</param>
		/// <returns>
		/// a review object
		/// </returns>
		public Review GetReview( int reviewId )
		{
			return ReviewRepository.GetById( reviewId );
		}

		/// <summary>
		/// Updates the review.
		/// </summary>
		/// <param name="review">The review.</param>
		public void UpdateReview( Review review )
		{
			ReviewRepository.Update( review );
		}

		/// <summary>
		/// Deletes the review.
		/// </summary>
		/// <param name="review">The review.</param>
		/// <param name="userId">The user identifier.</param>
		public void DeleteReview( Review review, string userId )
		{
			ReviewRepository.Delete( review.ReviewId, userId );
			SaveActivity( string.Format( "Review {0} deleted by {1}", review.ReviewId, userId ), userId );
		}

		/// <summary>
		/// Deletes the reviews.
		/// </summary>
		/// <param name="reviewIds">The review ids.</param>
		/// <param name="userId">The user identifier.</param>
		public void DeleteReviews( List<int> reviewIds, string userId )
		{
			ReviewRepository.DeleteReviews( reviewIds, userId );
			foreach ( var id in reviewIds )
				SaveActivity( string.Format( "Review {0} deleted by {1}", id, userId ), userId );
		}

		/// <summary>
		/// Determin if any of the reviews have outcomes recorded against them.
		/// </summary>
		/// <param name="reviewIds">The review ids.</param>
		/// <returns></returns>
		public bool AnyOutComesFor( int[] reviewIds )
		{
			return reviewIds.Select( id => ReviewRepository.GetById( id ) ).Any( review => review.HasOutcome() );
		}

		/// <summary>
		/// Records the same outcome for a collection of reviews.
		/// </summary>
		/// <param name="reviewIds">The review ids.</param>
		/// <param name="outcome">The outcome.</param>
		/// <param name="userId">The user identifier.</param>
		public void BulkOutcome( int[] reviewIds, Review outcome, string userId )
		{
			ReviewRepository.BulkOutcome( reviewIds, outcome, userId );
			SaveActivity( string.Format( "Bulk update {0} reviews by {1}", reviewIds.Count(), userId ), userId );
		}

		/// <summary>
		/// Stores the review data from a CSV upload.
		/// </summary>
		/// <param name="upload">The upload.</param>
		/// <param name="stream">The stream.</param>
		/// <returns>
		/// number of reviews stored
		/// </returns>
		public int StoreReviewData( Upload upload, Stream stream )
		{
			var nReviews = ReviewRepository.StoreReviewData( upload, stream );
			UpateUploadWithPotentiallyNewName( upload );
			return nReviews;
		}

		/// <summary>
		/// Upates the new name of the upload with potentially.
		/// </summary>
		/// <param name="upload">The upload.</param>
		private void UpateUploadWithPotentiallyNewName( Upload upload )
		{
			var reviewList = GetUpload( upload.UploadId, AppHelper.DefaultGridSettings() );
			string orgCode, esaCode, siteCode;
			AppHelper.DetermineOrgEsaAndSite( reviewList, out orgCode, out esaCode, out siteCode );
			upload.EsaCode = esaCode;
			upload.OrgCode = orgCode;
			upload.SiteCode = siteCode;
			upload.Name = upload.SampleName();
			UploadRepository.Update( upload );
		}

		/// <summary>
		/// Appends a number of reviews  based on a stream of CSV data.
		/// </summary>
		/// <param name="upload">The upload.</param>
		/// <param name="stream">The stream.</param>
		/// <returns>
		/// number of records appended
		/// </returns>
		public int AppendReviewData( Upload upload, Stream stream )
		{
			var nReviews = ReviewRepository.StoreReviewData( upload, stream );
			UpateUploadWithPotentiallyNewName( upload );
			return nReviews;
		}

		#endregion Business Object = Review

		#region Business Object ComplianceIndicator

		/// <summary>
		/// Saves the compliance indicator.
		/// </summary>
		/// <param name="complianceIndicator">The compliance indicator.</param>
		public void SaveComplianceIndicator( ComplianceIndicator complianceIndicator )
		{
			ComplianceIndicatorRepository.Update( complianceIndicator );
		}

		#endregion Business Object ComplianceIndicator

		#region Business Object = Control

		/// <summary>
		/// Gets the control file, and if it cant find one it adds one (there should be one and only one).
		/// </summary>
		/// <returns>
		/// control record
		/// </returns>
		public PatControl GetControlFile()
		{
			var controlList = ControlRepository.GetAll();
			var controlRec = controlList.FirstOrDefault();

			if ( controlRec == null )
			{
				ControlRepository.Add();
				controlRec = new PatControl
					{
						SystemAvailable = true,
						ProjectCompletion = 0.0M,
						TotalComplianceIndicator = 0.0M,
						ProjectCount = 0,
						SampleCount = 0,
						ReviewCount = 0
					};
			}
			return controlRec;
		}

		public void UpdateControl( PatControl control )
		{
			ControlRepository.Update( control );
		}

		#endregion Business Object = Control

		#region Reports

		/// <summary>
		/// Gets the compliance risk report sort order.
		/// </summary>
		/// <returns></returns>
		public List<SelectListItem> GetComplianceRiskReportSortOrder()
		{
			return AdwRepository.ListCode( DataConstants.AdwListCodeForComplianceRiskReportSortOrder );
		}

		/// <summary>
		/// Return All the records of Report Compliance Report data
		/// </summary>
		/// <param name="viewModel">search criteria</param>
		/// <returns></returns>
		public List<ComplianceRiskIndicator> GetComplianceRiskIndicatorReport( SearchCriteriaViewModel viewModel )
		{
			var reportRepository = new ReportRepository();
			return reportRepository.GetComplianceRiskIndicatorReport( AppHelper.DefaultGridSettings(), viewModel );
		}

		/// <summary>
		/// Return a set of records of Report Compliance data
		/// </summary>
		/// <param name="gridSettings">e.g pageSize and pageIndex</param>
		/// <param name="viewModel">search criteria</param>
		/// <returns></returns>
		public List<ComplianceRiskIndicator> GetComplianceRiskIndicatorReport( GridSettings gridSettings, SearchCriteriaViewModel viewModel )
		{
			var reportRepository = new ReportRepository();
			return reportRepository.GetComplianceRiskIndicatorReport( gridSettings, viewModel );
		}

		/// <summary>
		/// Return a total records of Compliance Indicator Report's data
		/// </summary>
		/// <param name="viewModel">search criteria</param>
		/// <returns></returns>
		public int CountComplianceRiskIndicatorReport( SearchCriteriaViewModel viewModel )
		{
			var reportRepository = new ReportRepository();
			return reportRepository.CountComplianceRiskIndicatorReport( AppHelper.DefaultGridSettings(), viewModel );
		}

		/// <summary>
		/// Gets the site visit report sort order.
		/// </summary>
		/// <returns></returns>
		public List<SelectListItem> GetSiteVisitReportSortOrder()
		{
			return AdwRepository.ListCode( DataConstants.AdwListCodeForSiteVisitReportSortOrder );
		}

		/// <summary>
		/// Gets the site visit report.
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public List<SiteVisit> GetSiteVisitReport( SearchCriteriaViewModel viewModel )
		{		
			var reportRepository = new ReportRepository();
			return reportRepository.GetSiteVisitReport(AppHelper.DefaultGridSettings(), viewModel);
		}

		/// <summary>
		/// Gets the site visit report.
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public List<SiteVisit> GetSiteVisitReport( GridSettings gridSettings, SearchCriteriaViewModel viewModel )
		{
			var reportRepository = new ReportRepository();
			return reportRepository.GetSiteVisitReport( gridSettings, viewModel );
		}

		/// <summary>
		/// Counts the site visit report.
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public int CountSiteVisitReport( SearchCriteriaViewModel viewModel )
		{
			var reportRepository = new ReportRepository();
			return reportRepository.CountSiteVisitReport(AppHelper.DefaultGridSettings(), viewModel);
		}

		/// <summary>
		/// Gets the provider summary report sort order.
		/// </summary>
		/// <returns></returns>
		public List<SelectListItem> GetProviderSummaryReportSortOrder()
		{
			return AdwRepository.ListCode( DataConstants.AdwListCodeForProviderSummaryReportSortOrder );
		}

		/// <summary>
		/// Gets the provider summary report.
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public List<ProviderSummary> GetProviderSummaryReport( SearchCriteriaViewModel viewModel )
		{
			var reportRepository = new ReportRepository();
			return reportRepository.GetProviderSummaryReport(AppHelper.DefaultGridSettings(), viewModel);
		}

		/// <summary>
		/// Gets the provider summary report.
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public List<ProviderSummary> GetProviderSummaryReport( GridSettings gridSettings, SearchCriteriaViewModel viewModel )
		{
			var reportRepository = new ReportRepository();
			return reportRepository.GetProviderSummaryReport( gridSettings, viewModel );
		}

		/// <summary>
		/// Counts the provider summary report.
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public int CountProviderSummaryReport( SearchCriteriaViewModel viewModel )
		{
			var reportRepository = new ReportRepository();
			return reportRepository.CountProviderSummaryReport(AppHelper.DefaultGridSettings(), viewModel);
		}

		/// <summary>
		/// Gets the progress report sort order.
		/// </summary>
		/// <returns></returns>
		public List<SelectListItem> GetProgressReportSortOrder()
		{
			return AdwRepository.ListCode( DataConstants.AdwListCodeForProgressReportSortOrder );
		}

		/// <summary>
		/// Gets the progress report.
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public List<Progress> GetProgressReport( SearchCriteriaViewModel viewModel )
		{
			var reportRepository = new ReportRepository();
			return reportRepository.GetProgressReport(AppHelper.DefaultGridSettings(), viewModel);
		}

		/// <summary>
		/// Gets the progress report.
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public List<Progress> GetProgressReport( GridSettings gridSettings, SearchCriteriaViewModel viewModel )
		{
			var reportRepository = new ReportRepository();
			return reportRepository.GetProgressReport( gridSettings, viewModel );
		}

		/// <summary>
		/// Counts the progress report.
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public int CountProgressReport( SearchCriteriaViewModel viewModel )
		{
			var reportRepository = new ReportRepository();
			return reportRepository.CountProgressReport(AppHelper.DefaultGridSettings(), viewModel);
		}

		/// <summary>
		/// Gets the project type report.
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public ProjectType GetProjectTypeReport( SearchCriteriaViewModel viewModel )
		{
			var reportRepository = new ReportRepository();
			return reportRepository.GetProjectTypeReport( viewModel );
		}

		/// <summary>
		/// Gets the project type by.
		/// </summary>
		/// <param name="projectTypeBy">The project type by.</param>
		/// <param name="projectType">Type of the project.</param>
		/// <returns></returns>
		public List<ProjectTypeDetail> GetProjectTypeBy( string projectTypeBy, ProjectType projectType )
		{
			var reportRepository = new ReportRepository();
			return reportRepository.GetProjectTypeBy( projectTypeBy, projectType );
		}

		/// <summary>
		/// Gets the finding summary report.
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public FindingSummary GetFindingSummaryReport( SearchCriteriaViewModel viewModel )
		{
			var reportRepository = new ReportRepository();
			return reportRepository.GetFindingSummaryReport( viewModel );
		}

		/// <summary>
		/// Gets the type of the finding summary.
		/// </summary>
		/// <param name="findingSummaryType">Type of the finding summary.</param>
		/// <param name="findingSummary">The finding summary.</param>
		/// <returns></returns>
		public List<FindingSummaryDetail> GetFindingSummaryType( string findingSummaryType, FindingSummary findingSummary )
		{
			var reportRepository = new ReportRepository();
			return reportRepository.GetFindingSummaryType( findingSummaryType, findingSummary );
		}

		/// <summary>
		/// Gets the dashboard report.
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public List<Dashboard> GetDashboardReport( SearchCriteriaViewModel viewModel )
		{
			var reportRepository = new ReportRepository();
			return reportRepository.GetDashboardReport( viewModel );
		}

		/// <summary>
		/// Lookups the esa by.
		/// </summary>
		/// <param name="searchText">The search text.</param>
		/// <param name="orgCode">The org code.</param>
		/// <param name="maxResults">The maximum results.</param>
		/// <returns></returns>
		public List<SelectListItem> LookupESABy( string searchText, string orgCode, int maxResults )
		{
			var reportRepository = new ReportRepository();
			var esaList = reportRepository.GetESACodeList( orgCode );

			// empty searchText to return all in the list
			var result = from o in esaList
							 where ( string.IsNullOrWhiteSpace( searchText ) || o.Value.IndexOf( searchText, StringComparison.OrdinalIgnoreCase ) >= 0 )
							 orderby o.Value
							 select o;

			return result.Take( maxResults ).ToList();
		}

		/// <summary>
		/// Lookups the site by.
		/// </summary>
		/// <param name="searchText">The search text.</param>
		/// <param name="orgCode">The org code.</param>
		/// <param name="esaCode">The esa code.</param>
		/// <param name="maxResults">The maximum results.</param>
		/// <returns></returns>
		public List<SelectListItem> LookupSiteBy( string searchText, string orgCode, string esaCode, int maxResults )
		{
			var reportRepository = new ReportRepository();
			var siteList = reportRepository.GetSiteCodeList( orgCode, esaCode );

			// empty searchText to return all in the list
			var result = from o in siteList
							 where ( string.IsNullOrWhiteSpace( searchText ) || o.Value.IndexOf( searchText, StringComparison.OrdinalIgnoreCase ) >= 0 )
							 orderby o.Value
							 select o;

			return result.Take( maxResults ).ToList();
		}

		#endregion Reports

		#region Business Object = Sample

		/// <summary>
		/// Saves the extract claims into the data store.
		/// </summary>
		/// <param name="sessionKey">The session key.</param>
		/// <param name="claims">The claims.</param>
		/// <param name="userId">The user identifier.</param>
		public void SaveExtract( string sessionKey, List<PatClaim> claims, string userId )
		{
			SampleRepository.SaveSample( sessionKey, claims, userId );
		}

		/// <summary>
		/// Gets the sample the user is building up in session.
		/// </summary>
		/// <param name="sessionKey">The session key.</param>
		/// <returns>
		/// list of sample records
		/// </returns>
		public List<Sample> GetSample( string sessionKey )
		{
			return SampleRepository.GetSample( sessionKey );
		}

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
		/// <returns>
		/// the id of the sample
		/// </returns>
		public int SaveSample( int projectId, string sessionKey, List<int> ids, bool outOfScope, bool additional,
									  DateTime dueDate,
									  string userId )
		{
			var sampleList = GetSample( sessionKey );
			var selectedList = GetSelected( sampleList, ids );

			var isRandom = DeterminRandomFlag( sampleList, ids );

			var upload = new Upload
				{
					ProjectId = projectId,
					AcceptedFlag = true,  //  Accepted flag is only for manual uploads
					RandomFlag = isRandom,
					InScope = !outOfScope,
					AdditionalReview = additional,
					DateUploaded = DateTime.Now,
					SourceFile = string.Empty,
					ServerFile = string.Empty,
					Rows = 0,
					Name = "deferred",
					Status = "Uploaded",
					OrgCode = "defered",
					EsaCode = "defered",
					SiteCode = "defered",
					DueDate = dueDate,
					CreatedBy = userId,
					UploadedBy = userId
				};

			UploadRepository.Add( upload );
			var uploadId = upload.UploadId;

			if ( uploadId <= 0 ) return 0;

			var reviewList = ReviewListFromSample( selectedList, upload, additional, outOfScope );

			foreach ( var review in reviewList )
				ReviewRepository.Add( review );

			//  workout the Upload Name
			DetermineUploadNameFromReviews( reviewList, upload );

			SampleRepository.DeleteBySessionKey( sessionKey );

			return uploadId;
		}

		/// <summary>
		/// Saves the sample selections.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="sessionKey">The session key.</param>
		/// <param name="ids">The ids.</param>
		/// <param name="userId">The user identifier.</param>
		public void SaveSampleSelections( int projectId, string sessionKey, List<int> ids, string userId )
		{
			var sampleList = GetSample( sessionKey );
			var selectedList = GetSelected( sampleList, ids );
			SampleRepository.SaveSampleDeSelections( sessionKey, sampleList, selectedList, userId );
		}

		/// <summary>
		/// Deletes the session.
		/// </summary>
		/// <param name="sessionKey">The session key.</param>
		/// <param name="userId">The user identifier.</param>
		public void DeleteSession( string sessionKey, string userId )
		{
			SampleRepository.DeleteBySessionKey( sessionKey );
			var userActivity = string.Format( "Session {0} deleted by {1}", sessionKey, userId );
			SaveActivity( userActivity, userId );
		}

		private static bool DeterminRandomFlag( ICollection sampleList, ICollection ids )
		{
			return ( sampleList.Count.Equals( ids.Count ) );
		}

		private void DetermineUploadNameFromReviews( List<Review> reviewList, Upload upload )
		{
			string orgCode;
			string esaCode;
			string siteCode;
			AppHelper.DetermineOrgEsaAndSite( reviewList, out orgCode, out esaCode, out siteCode );
			upload.OrgCode = orgCode;
			upload.EsaCode = esaCode;
			upload.SiteCode = siteCode;
			upload.Name = upload.SampleName();
			UploadRepository.Update( upload );
		}

		private static List<Sample> GetSelected( IEnumerable<Sample> sampleList, IEnumerable<int> ids )
		{
			return sampleList.Where( sample => IdsContain( sample.Id, ids ) ).ToList();
		}

		private static bool IdsContain( int id, IEnumerable<int> ids )
		{
			return ids.Any( i => i.Equals( id ) );
		}

		private static List<Review> ReviewListFromSample( IEnumerable<Sample> sampleList, Upload upload,
																		  bool additional, bool outOfScope )
		{
			return sampleList.Select( sample => ReviewFromSample( sample, upload, additional, outOfScope ) ).ToList();
		}

		private static Review ReviewFromSample( Sample sample, Upload upload, bool additional, bool outOfScope )
		{
			var review = new Review
				{
					UploadId = upload.UploadId,
					ProjectId = upload.ProjectId,
					OrgCode = sample.OrgCode,
					ESACode = sample.EsaCode,
					SiteCode = sample.SiteCode,
					StateCode = sample.StateCode,
					ManagedBy = sample.ManagedBy,
					ClaimId = sample.ClaimId,
					JobseekerId = sample.JobseekerId,
					ActivityId = sample.ActivityId,
					ClaimSequenceNumber = sample.ClaimSequenceNumber,
					ClaimAmount = sample.ClaimAmount,
					ClaimRecoveryAmount = 0,
					IsAdditionalReview = additional,
					IsOutOfScope = outOfScope,
					JobSeekerGivenName = sample.GivenName,
					JobSeekerSurname = sample.Surname,
					ContractType = sample.ContractType,
					ClaimType = sample.ClaimType,
					ClaimCreationDate = sample.ClaimCreationDate,
					AutoSpecialClaim = sample.AutoSpecialClaimFlag.Equals( "Y" ),
					ManualSpecialClaim = sample.ManSpecialClaimFlag.Equals( "Y" ),
					CreatedBy = upload.CreatedBy
				};
			return review;
		}

		/// <summary>
		/// Popolates a view with an Extract of claims.
		/// </summary>
		/// <param name="criteria">The claim criteria.</param>
		/// <returns>
		/// the populated view
		/// </returns>
		public ClaimSampleViewModel ExtractClaims( SampleCriteria criteria )
		{
			//  MF is fussy about nulls
			if ( string.IsNullOrEmpty( criteria.SiteCode ) ) criteria.SiteCode = string.Empty;
			if ( criteria.FromClaimDate == null ) criteria.FromClaimDate = new DateTime( 1, 1, 1 );
			if ( criteria.ToClaimDate == null ) criteria.ToClaimDate = new DateTime( 1, 1, 1 );
			if (!string.IsNullOrEmpty( criteria.ClaimType )) criteria.ClaimType = criteria.ClaimType.ToUpper();
			if ( !string.IsNullOrEmpty( criteria.OrgCode ) ) criteria.OrgCode = criteria.OrgCode.ToUpper();
			if ( !string.IsNullOrEmpty( criteria.EsaCode ) ) criteria.EsaCode = criteria.EsaCode.ToUpper();
			if ( !string.IsNullOrEmpty( criteria.SiteCode ) ) criteria.SiteCode = criteria.SiteCode.ToUpper();

#if DEBUG
			criteria.Audit( AuditService );
#endif
			var vm = ClaimsRepository.GetClaimSample( criteria.ClaimType,
																 criteria.OrgCode,
																 criteria.EsaCode,
																 criteria.SiteCode,
																 criteria.FromClaimDate,
																 criteria.ToClaimDate,
																 criteria.MaxSampleSize,
																 criteria.IncludeSpecialClaims
				);

			if (!string.IsNullOrEmpty( vm.ErrorMessage ))
				AuditService.AuditActivity( vm.ErrorDiagnostic( criteria.RequestingUser ), 
					"P81-ERROR" );				

			var claimsFound = 0;
			if ( vm.Claims != null )
				claimsFound = vm.Claims.Count;
			AuditService.AuditActivity( string.Format( "{0} claims found for {1}", 
				claimsFound, criteria.RequestingUser ), "P81" );

			return vm;
		}

		/// <summary>
		/// Gets the sample selections.
		/// </summary>
		/// <param name="sessionKey">The session key.</param>
		/// <returns>
		/// the ids of the selected claims
		/// </returns>
		public string GetSampleSelections( string sessionKey )
		{
			//  Repository classes just responsible for data not interpretinging it, Pat Service can do this
			var selections = string.Empty;
			var list = SampleRepository.GetSample( sessionKey );
			selections = list.Where( sample => sample.Selected )
								  .Aggregate( selections, ( current, sample ) => current + string.Format( "{0},", sample.Id ) );
			return selections.Length > 0 ? selections.Remove( selections.Length - 1, 1 ) : selections;
		}

		/// <summary>
		/// Gets the sample message.
		/// </summary>
		/// <param name="sessionKey">The session key.</param>
		/// <returns></returns>
		public string GetSampleMessage( string sessionKey )
		{
			var message = "New Sample";
			var list = SampleRepository.GetSample( sessionKey );
			var nExtracted = list.Count;
			var nSelected = list.Count( sample => sample.Selected );
			if ( nExtracted > 0 )
				message = string.Format( "{0} claims extracted and {1} claims selected.", nExtracted, nSelected );
			return message;
		}

		/// <summary>
		/// Gets the distinct session keys that have been used.
		/// </summary>
		/// <returns>
		/// an array of keys
		/// </returns>
		public string[] GetDistinctSessionKeys()
		{
			return SampleRepository.GetDistinctSessionKeys();
		}

		#endregion Business Object = Sample

		#region Business Object = CheckList

		/// <summary>
		/// Gets the check list.
		/// </summary>
		/// <param name="reviewID">The review identifier.</param>
		/// <returns></returns>
		public CheckList GetCheckList( int reviewID )
		{
			return CheckListRepository.GetById( reviewID );
		}

		/// <summary>
		/// Saves the check list.
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <returns></returns>
		public CheckList SaveCheckList( CheckList viewModel )
		{
			CheckListRepository.Save( viewModel );
			var userActivity = string.Format("CheckList {0} saved by {1}", viewModel.ReviewID, viewModel.CreatedBy);
			SaveActivity(userActivity, viewModel.CreatedBy);
			return viewModel;
		}

		/// <summary>
		/// Gets the ZEIS0P82 details.
		/// </summary>
		/// <param name="claimID">The claim identifier.</param>
		/// <param name="claimSequenceNo">The claim sequence no.</param>
		/// <returns></returns>
		public ZEIS0P82ViewModel GetZEIS0P82Details( long claimID, int claimSequenceNo )
		{
			if (claimSequenceNo == 0) claimSequenceNo = 1;
			return ClaimsRepository.GetRelatedData( claimID, claimSequenceNo );
		}

		#endregion Business Object = CheckList

		#region ADW shortcuts

		/// <summary>
		/// Gets the claim type description for a claim type code.
		/// </summary>
		/// <param name="claimType">Type of the claim.</param>
		/// <returns>
		/// claim type description
		/// </returns>
		public string GetClaimTypeDescription( string claimType )
		{
			return AdwRepository.GetDescription( DataConstants.AdwListCodeForClaimTypes, claimType );
		}

		/// <summary>
		/// Gets the ESA description from a code.
		/// </summary>
		/// <param name="esaCode">The esa code.</param>
		/// <returns>
		/// ESA name
		/// </returns>
		public string GetEsaDescription( string esaCode )
		{
			return AdwRepository.GetDescription( DataConstants.AdwListCodeForEsaCodes, esaCode );
		}

		/// <summary>
		/// Gets the site description.
		/// </summary>
		/// <param name="siteCode">The site code.</param>
		/// <returns></returns>
		public string GetSiteDescription( string siteCode )
		{
			return AdwRepository.GetDescription( DataConstants.AdwListCodeForSiteCodes, siteCode );
		}

		/// <summary>
		/// Gets the contract type description from a contract type code.
		/// </summary>
		/// <param name="contractType">Type of the contract.</param>
		/// <returns>
		/// the Contract name
		/// </returns>
		public string GetContractTypeDescription( string contractType )
		{
			return AdwRepository.GetDescription( DataConstants.AdwListCodeForContractTypes, contractType );
		}

		/// <summary>
		/// Gets the assessment description from the assessment code.
		/// </summary>
		/// <param name="assessmentCode">The assessment code.</param>
		/// <returns>
		/// assessment description
		/// </returns>
		public string GetAssessmentDescription( string assessmentCode )
		{
			return AdwRepository.GetDescription( DataConstants.AdwListCodeForAssessmentCodes, assessmentCode, false );
		}

		/// <summary>
		/// Gets the recovery reason description from a recovery reason code.
		/// </summary>
		/// <param name="recoveryReason">The recovery reason.</param>
		/// <returns>
		/// recovery reason name
		/// </returns>
		public string GetRecoveryReasonDescription( string recoveryReason )
		{
			return AdwRepository.GetDescription( DataConstants.AdwListCodeForRecoveryReasonCodes, recoveryReason, false );
		}

		/// <summary>
		/// Gets the assessment action description from the assessment action code.
		/// </summary>
		/// <param name="assessmentAction">The assessment action.</param>
		/// <returns>
		/// assessment action description
		/// </returns>
		public string GetAssessmentActionDescription( string assessmentAction )
		{
			return AdwRepository.GetDescription( DataConstants.AdwListCodeForAssessmentActionCodes, assessmentAction, false );
		}

		/// <summary>
		/// Gets the final outcome description from the final outcome code.
		/// </summary>
		/// <param name="outcomeCode">The outcome code.</param>
		/// <returns>
		/// final outcome description
		/// </returns>
		public string GetFinalOutcomeDescription( string outcomeCode )
		{
			return AdwRepository.GetDescription( DataConstants.AdwListCodeForOutcomeCodes, outcomeCode, false );
		}

		/// <summary>
		/// Gets the final outcomes.
		/// </summary>
		/// <returns>
		/// array of final outcomes
		/// </returns>
		public string[] GetFinalOutcomes()
		{
			var items = AdwRepository.ListCode( DataConstants.AdwListCodeForOutcomeCodes );
			return items.Select( x => x.Value ).ToArray();
		}

		/// <summary>
		/// Gets the recovery reasons.
		/// </summary>
		/// <returns>
		/// array of Recovery reasons
		/// </returns>
		public string[] GetRecoveryReasons()
		{
			var items = AdwRepository.ListCode( DataConstants.AdwListCodeForRecoveryReasonCodes );
			return items.Select( x => x.Value ).ToArray();
		}

		/// <summary>
		/// Gets the assessment actions.
		/// </summary>
		/// <returns>
		/// array of assessment actions
		/// </returns>
		public string[] GetAssessmentActions()
		{
			var items = AdwRepository.ListCode( DataConstants.AdwListCodeForAssessmentActionCodes );
			return items.Select( x => x.Value ).ToArray();
		}

		/// <summary>
		/// Gets the assessment codes.
		/// </summary>
		/// <returns>
		/// array of assessment codes
		/// </returns>
		public string[] GetAssessmentCodes()
		{
			var items = AdwRepository.ListCode( DataConstants.AdwListCodeForAssessmentCodes );
			return items.Select( x => x.Value ).ToArray();
		}

		/// <summary>
		/// Determines whether the specified org code is a valid org code.
		/// </summary>
		/// <param name="orgCode">The org code.</param>
		/// <returns>
		/// valid or not
		/// </returns>
		public bool IsValidOrgCode( string orgCode )
		{
			var orgName = AdwRepository.GetOrgName( orgCode );
			return !orgName.EndsWith( "?" );
		}

		/// <summary>
		/// Gets the name of the orgfrom the Org Code using ADW.
		/// </summary>
		/// <param name="orgCode">The org code.</param>
		/// <returns></returns>
		public string GetOrgName( string orgCode )
		{
			return AdwRepository.GetOrgName( orgCode );
		}

		/// <summary>
		/// Get adw code and description regardless of the end date status
		/// </summary>
		/// <param name="listCode"></param>
		/// <param name="codeValue"></param>
		/// <param name="selected">if true it will be set as selected</param>
		/// <returns></returns>
		public SelectListItem GetAdwCode(string listCode, string codeValue, bool selected)
		{
			SelectListItem result = null;

			// get it regardless of the end date status 
			var description = AdwRepository.GetDescription(listCode, codeValue, false);
			if (!string.IsNullOrWhiteSpace(description))
			{
				result = new SelectListItem { Value = codeValue, Text = description, Selected = selected };
			}

			return result;
		}
		#endregion ADW shortcuts

		#region Business Object = Questionnaire

		/// <summary>
		/// Gets the review questionnaire.
		/// </summary>
		/// <param name="reviewId">The review identifier.</param>
		/// <returns></returns>
		public ReviewQuestionnaire GetReviewQuestionnaire( int reviewId )
		{
			return QuestionnaireRepository.GetReviewQuestionnaireByReviewId( reviewId );
		}

		/// <summary>
		/// Get the list of Questions & Answers data of the Questionnaire 
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="reviewId">The review identifier.</param>
		/// <returns></returns>
		public List<QuestionAnswer> GetQuestionAnswers( GridSettings gridSettings, int reviewId )
		{
			return QuestionnaireRepository.GetQuestionAnswersByReviewId( gridSettings, reviewId );
		}

		/// <summary>
		/// Counts the question answers by review identifier.
		/// </summary>
		/// <param name="reviewId">The review identifier.</param>
		/// <returns></returns>
		public int CountQuestionAnswersByReviewId( int reviewId )
		{
			return QuestionnaireRepository.CountQuestionAnswersByReviewId( reviewId );
		}

		/// <summary>
		/// Gets the question answers by upload identifier.
		/// </summary>
		/// <param name="uploadId">The upload identifier.</param>
		/// <returns></returns>
		public List<QuestionAnswer> GetQuestionAnswersByUploadId( int uploadId )
		{
			return QuestionnaireRepository.GetQuestionAnswersByUploadId(AppHelper.DefaultGridSettings(), uploadId);
		}

		/// <summary>
		/// Gets the review questionnaire data.
		/// </summary>
		/// <param name="uploadId">The upload identifier.</param>
		/// <returns></returns>
		public List<ReviewQuestionnaire> GetReviewQuestionnaireData( int uploadId )
		{
			return QuestionnaireRepository.GetReviewQuestionnaireData(AppHelper.DefaultGridSettings(), uploadId);
		}

		/// <summary>
		/// Gets the review questionnaire data.
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="uploadId">The upload identifier.</param>
		/// <returns></returns>
		public List<ReviewQuestionnaire> GetReviewQuestionnaireData( GridSettings gridSettings, int uploadId )
		{
			return QuestionnaireRepository.GetReviewQuestionnaireData( gridSettings, uploadId );
		}

		/// <summary>
		/// Counts the review questionnaires by upload identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		public int CountReviewQuestionnairesByUploadId( int id )
		{
			return QuestionnaireRepository.CountReviewQuestionnairesByUploadId( id );
		}

		/// <summary>
		/// Gets the project question text.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns></returns>
		public List<String> GetProjectQuestionText( int projectId )
		{
			return QuestionnaireRepository.GetProjectQuestions( projectId );
		}

		public string ConvertQuestionnaireDataToCsv( List<ReviewQuestionnaire> data )
		{
			var result = new StringBuilder();

			// add the fixed header e.g.
			// QuestionnaireId, ReviewId, UserId, Assessment Outcome Code, Recovery Reason Code, etc...
			result.Append( AddCell( "QuestionnaireId" ) );
			result.Append( AddCell( "ReviewId" ) );
			result.Append( AddCell( "UserId" ) );
			result.Append( AddCell( "AssessmentOutcomeCode" ) );
			result.Append( AddCell( "RecoveryReasonCode" ) );
			result.Append( AddCell( "RecoveryActionCode" ) );
			result.Append( AddCell( "FinalOutcomeCode" ) );
			result.Append( AddCell( "Date" ) );

			//var totalExtraColumn = 0;
			// add the extra header
			var questionAnswer = data.First( q => q.QuestionAnswers != null && q.QuestionAnswers.Count > 0 ).QuestionAnswers;
			foreach ( var qa in questionAnswer )
			{
				result.Append( AddCell( qa.QuestionText ) );
				//totalExtraColumn++;
			}

			result.AppendLine( "" );

			// add the data
			foreach ( var rq in data )
			{
				result.Append( AddCell( rq.QuestionnaireID ) );
				result.Append( AddCell( rq.ReviewID ) );
				result.Append( AddCell( rq.UserID ) );
				result.Append( AddCell( rq.AssessmentOutcomeCode ) );
				result.Append( AddCell( rq.RecoveryReasonCode ) );
				result.Append( AddCell( rq.RecoveryActionCode ) );
				result.Append( AddCell( rq.FinalOutcomeCode ) );
				result.Append( AddCell( rq.Date ) );

				// add the extra data
				if ( rq.QuestionAnswers == null || rq.QuestionAnswers.Count < 1 )
				{
					for ( var i = 0; i < questionAnswer.Count; i++ )
						result.Append( AddCell( string.Empty ) );
				}
				else
				{
					foreach ( var qa in rq.QuestionAnswers )
						result.Append( AddCell( qa.AnswerText ) );
				}

				result.AppendLine( "" );
			}

			return result.ToString();
		}

		private static string AddCell( object value )
		{
			const char delimiter = ',';
			return string.Format( "\"{0}\"{1}", value, delimiter );
		}

		#endregion Business Object = Questionnaire

		#region Repositories

		/// <summary>
		/// Exposes the Upload Repository
		/// </summary>
		/// <returns>
		/// IUploadRepository
		/// </returns>
		public IUploadRepository GetUploadRepository()
		{
			return UploadRepository;
		}

		/// <summary>
		/// Exposes the Questionnaire Repository
		/// </summary>
		/// <returns>
		/// IQuestionnaireRepository
		/// </returns>
		public IQuestionnaireRepository GetQuestionaireRepository()
		{
			return QuestionnaireRepository;
		}

		/// <summary>
		/// Exposes the Review Repository
		/// </summary>
		/// <returns>
		/// IReviewRepository
		/// </returns>
		public IReviewRepository GetReviewRepository()
		{
			return ReviewRepository;
		}

		/// <summary>
		/// Exposes the ADW Repository
		/// </summary>
		/// <returns>
		/// IAdwRepository
		/// </returns>
		public IAdwRepository GetAdwRepository()
		{
			return AdwRepository;
		}

		/// <summary>
		/// Exposes the Audit Service
		/// </summary>
		/// <returns>
		/// IAuditService
		/// </returns>
		public IAuditService GetAuditService()
		{
			return AuditService;
		}

		#endregion Repositories
	}
}