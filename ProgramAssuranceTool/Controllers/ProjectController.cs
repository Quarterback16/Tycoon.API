using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MvcJqGrid;
using MvcJqGrid.Enums;
using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Infrastructure.Controllers;
using ProgramAssuranceTool.Infrastructure.Security;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Models;
using ProgramAssuranceTool.Repositories;
using ProgramAssuranceTool.ViewModels.Project;
using ProgramAssuranceTool.ViewModels.Review;

namespace ProgramAssuranceTool.Controllers
{
	[CustomAuthorize] 
	public class ProjectController : InfrastructureController
	{
		/// <summary>
		///   The Controller for all the Project pages
		/// </summary>
		/// <param name="commonDependencies">The combined dependancies as an Interface to allow for unit testing </param>
		public ProjectController( IControllerDependencies commonDependencies )
			: base( commonDependencies )
		{
		}

		#region Project list

		/// <summary>
		///   Renders the main Project List page - http://wde308498/openWiki/ow.asp?ProgramAssuranceToolProjectListPage
		///     http://localhost:6491/Project
		/// </summary>
		/// <returns>view of the Project List</returns>
		[HttpGet]
		public ActionResult Index()
		{

			// reset the active project
			AppHelper.SetSessionProjectId( Session, 0 );

			// this must be positioned before GenerateProjectsGrid
			ViewBag.ModelIsValid = true;
			ViewData["grid"] = GenerateProjectsGrid();

			var vm = new ProjectListViewModel
				{
					UploadFrom = AppHelper.ToNullDateTime(AppHelper.GetSessionUploadedFrom(Session)), 
					UploadTo = AppHelper.ToNullDateTime(AppHelper.GetSessionUploadedTo(Session)), 
					SavedSettings = GetSavedProjectGridSettings()
				};

			AppHelper.SetSessionProjectGrid( Session, ViewData[ "grid" ].ToString() );
			return View( vm );
		}

		/// <summary>
		///   Return the old settings as a JSON string
		/// </summary>
		/// <returns></returns>
		private string GetSavedProjectGridSettings()
		{
			// get any old settings
			var oldSettings = string.Empty;
			if ( Session[ CommonConstants.SessionProjectListFilters ] != null )
			{
				var gridsettingString = Session[ CommonConstants.SessionProjectListFilters ].ToString();
				if (!string.IsNullOrWhiteSpace(gridsettingString))
				{
					var gridsettings = gridsettingString.ToObject<MvcJqGrid.GridSettings>();
					if (gridsettings.Where != null)
					{
						var serializer = new JavaScriptSerializer();
						var json = serializer.Serialize(gridsettings.Where);
						oldSettings = json;
					}
				}
			}
			return oldSettings;
		}

		private DateTime GetUploadedDates( DateTime earliest, ref DateTime latest )
		{
			var uploadedFrom = AppHelper.GetSessionUploadedFrom( Session );
			var uploadedTo = AppHelper.GetSessionUploadedTo( Session );

			if ( !string.IsNullOrEmpty( uploadedFrom ) )
				earliest = DateTime.Parse( uploadedFrom );

			if ( !string.IsNullOrEmpty( uploadedTo ) )
				latest = DateTime.Parse( uploadedTo );

			return earliest;
		}

		/// <summary>
		///   Processed an upload range search
		/// </summary>
		/// <param name="vm">The vm.</param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult Index( ProjectListViewModel vm )
		{
			if ( ModelState.IsValid )
			{
				//  reload with new date parameters
				AppHelper.SetSessionUploadedFrom( Session, AppHelper.ShortDate( vm.UploadFrom ) );
				AppHelper.SetSessionUploadedTo( Session, AppHelper.ShortDate( vm.UploadTo ) );

				// reset the active project
				AppHelper.SetSessionProjectId(Session, 0); 

				// this must be positioned before GenerateProjectsGrid
				ViewBag.ModelIsValid = true;
				ViewData["grid"] = GenerateProjectsGrid();

				vm.SavedSettings = GetSavedProjectGridSettings();

				AppHelper.SetSessionProjectGrid(Session, ViewData["grid"].ToString());

				return View(vm);
			}

			// this must be positioned before GenerateProjectsGrid
			ViewBag.ModelIsValid = false;
			ViewData["grid"] = GenerateProjectsGrid();
			return View(vm);
		}

		/// <summary>
		///   The jqGrid definition
		/// </summary>
		/// <returns></returns>
		private string GenerateProjectsGrid()
		{
			var g = new Grid( "project" );
			//////  Column 1 --- ID
			g.AddColumn( new Column( "ProjectId" ).SetLabel( "Project Id" ).SetWidth( 70 ).SetAlign( Align.Center ) );

			//////  Column 2 --- Name
			g.AddColumn( new Column( "ProjectName" ).SetLabel( "Project Name" ).SetWidth( 250 ) );

			//////  Column 3 --- Project Type
			g.AddColumn(
				new Column( "ProjectType" ).SetLabel( "Project Type" )
													.SetWidth( 100 )
													.SetSearchType( Searchtype.Select )
													.SetSearchTerms(
														PatService.GetAllDistinctProjectTypes() ) );

			//////  Column 4 --- Review Count
			g.AddColumn( new Column( "ReviewCount" )
				.SetLabel( "Reviews" ).SetWidth( 60 ).SetSearch( false )
				.SetAlign( Align.Right ).SetSortable( false ) );

			// need to send back the information whether the model state is valid or not.
			g.SetUrl(Url.Action("GridDataProject", "Project", new {modelIsValid = ViewBag.ModelIsValid}));
			g.SetHeight( CommonConstants.GridStandardHeight ); //  set this tall enough and you wont see the scroll bars
			g.SetWidth( CommonConstants.GridStandardWidth );
			g.SetAutoWidth( true ); //  will force the grid to fit the width of the parent element
			g.SetRowNum( CommonConstants.GridStandardNoOfRows ); //  Sets the number of rows displayed initially
			g.SetViewRecords( true ); //  displays the total number of rows in the dataset
			g.SetRowList( new[] { 10, 15, 20, 50, 100 } );  //  Fills the dropdownlist in the pager which controls the number of rows per page
			g.SetPager( "projectPager" );
			g.SetSearchToolbar( true ); //  enable toolbar searching
			g.SetSearchOnEnter( false );
			g.SetEmptyRecords( "No projects found with these criteria" );
			g.OnLoadComplete( "pat.projectIndex_EmptyCheck();" );
			g.OnBeforeRequest( "pat.projectGridBeforeRequest();" );
			g.OnGridComplete( "pat.projectGridComplete();" );

			return AppHelper.WcagFixGrid( g, "project", "Program Assurance Tool Project List" );
		}

		/// <summary>
		///    The jqGrid url to extract the JSON data
		/// </summary>
		/// <param name="gridSettings"></param>
		/// <param name="modelIsValid">flag to indicate whether the main container screen has a valid model state or not</param>
		/// <returns></returns>
		public ActionResult GridDataProject( MvcJqGrid.GridSettings gridSettings, bool modelIsValid )
		{
			if (!modelIsValid)
			{
				// if the model state of the main container is not valid then return nothing to the grid
				return null;
			}

			Session[ CommonConstants.SessionProjectListFilters ] = gridSettings.ToJson();

			var earliest = new DateTime( 1, 1, 1 );
			var latest = new DateTime( 1, 1, 1 );

			earliest = GetUploadedDates( earliest, ref latest );

			var totProjects = PatService.CountProjects( gridSettings, earliest, latest );

			var projectList = PatService.GetProjects( gridSettings, earliest, latest );

			var jsonData = new
				{
					total = AppHelper.PagesInTotal( totProjects, gridSettings.PageSize ),
					page = gridSettings.PageIndex,
					records = totProjects,
					rows = (
								 from p in projectList.AsEnumerable()
								 select new
									 {
										 id = p.ProjectId,
										 cell = new List<String>
											 {
												 p.ProjectId.ToString( CultureInfo.InvariantCulture ),
												 AppHelper.ProjectDetailsLink( p.ProjectId, p.ProjectName ),
												 PatService.GetProjectTypeDescription( p.ProjectType ),
												 AppHelper.FormatInteger( PatService.CountReviews( p.ProjectId ) )
											 }
									 } ).ToArray()
				};

			return Json( jsonData, JsonRequestBehavior.AllowGet );
		}

		#endregion Project list

		#region Create Project use case


		/// <summary>
		///  Renders the Create Project page - http://wde308498/openWiki/ow.asp?ProgramAssuranceToolAddProject
		/// 
		///  http://localhost:6491/Project/Create
		/// </summary>
		/// <param name="formValues">The form values.</param>
		/// <returns></returns>
		[HttpGet]
		public ActionResult Create( FormCollection formValues )
		{
			var loggedInUser = new PatUser( Request.LogonUserIdentity );
			var proj = new Project
				{
					ProjectName = "Temporary Adding Project Name",
					Coordinator = loggedInUser.LoginName,
					CreatedBy = loggedInUser.LoginName,
					Resource_NSW_ACT = loggedInUser.IsNsw() || loggedInUser.IsNsw(),
					Resource_NO = loggedInUser.IsNo(),
					Resource_NT = loggedInUser.IsNt(),
					Resource_QLD = loggedInUser.IsQld(),
					Resource_SA = loggedInUser.IsSa(),
					Resource_TAS = loggedInUser.IsTas(),
					Resource_VIC = loggedInUser.IsVic(),
					Resource_WA = loggedInUser.IsWa()
				};

			var viewModel = new ProjectCreateViewModel
				{
					Project = proj,
					Coordinators = PatService.GetProgramAssuranceDropdownList()
				};

			return View( viewModel );
		}

		/// <summary>
		///   Autocomplete AJAX routine that finds the orgs.
		/// </summary>
		/// <param name="searchText">The search text.</param>
		/// <param name="maxResults">The maximum results.</param>
		/// <returns>JSON</returns>
		[HttpPost] //  helps prevent scripting attacks or data exposure from direct JSON queries
		public JsonResult FindOrgs( string searchText, int maxResults )
		{
			var repository = new AdwRepository();
			var result = repository.FindOrg( searchText, maxResults );
			if ( result == null || result.Count < 1 )
			{
				result = new List<AdwItem>
					{
						new AdwItem {Code = searchText, Description = "Data not found, please try again !"}
					};
			}
			return Json( result );
		}

		/// <summary>
		/// Autocomplete AJAX routine that finds projects.
		/// </summary>
		/// <param name="searchText">The search text.</param>
		/// <param name="maxResults">The maximum results.</param>
		/// <returns></returns>
		[HttpPost] //  helps prevent scripting attacks or data exposure from direct JSON queries
		public JsonResult LookupProject( string searchText, int maxResults )
		{
			var codeToSearch = searchText.GetCode();
			var result = PatService.LookupProject( codeToSearch, maxResults );
			if ( result == null || result.Count < 1 )
			{
				result = new List<Project>();

				int projectID;
				result.Add( int.TryParse( codeToSearch, out projectID )
									? new Project
										{
											ProjectId = projectID,
											ProjectName = "Data not found, please try again !"
										}
									: new Project
										{
											ProjectId = projectID,
											ProjectName = string.Format( "{0} - {1}", codeToSearch, "Data not found, please try again !" )
										} );
			}
			return Json( result );
		}

		/// <summary>
		///   Autocomplete AJAX routine that finds the org.
		/// </summary>
		/// <param name="searchText">The search text.</param>
		/// <param name="maxResults">The maximum results.</param>
		/// <returns></returns>
		[HttpPost] //  helps prevent scripting attacks or data exposure from direct JSON queries
		public JsonResult LookupOrg( string searchText, int maxResults )
		{
			var repository = new AdwRepository();
			var codeToSearch = searchText.GetCode();
			var result = repository.LookupOrg( codeToSearch, maxResults );
			if ( result == null || result.Count < 1 )
			{
				result = new List<SelectListItem>
					{
						new SelectListItem {Value = codeToSearch, Text = "Data not found, please try again !"}
					};
			}
			return Json( result );
		}

		/// <summary>
		/// Autocomplete AJAX routine that finds the ESA.
		/// </summary>
		/// <param name="searchText">The search text.</param>
		/// <param name="orgCode">The org code.</param>
		/// <param name="maxResults">The maximum results.</param>
		/// <returns></returns>
		[HttpPost] //  helps prevent scripting attacks or data exposure from direct JSON queries
		public JsonResult LookupESA( string searchText, string orgCode, int maxResults )
		{
			var repository = new AdwRepository();
			var codeToSearch = searchText.GetCode();
			var result = repository.LookupESA( codeToSearch, orgCode, maxResults );
			if ( result == null || result.Count < 1 )
			{
				result = new List<SelectListItem>
					{
						new SelectListItem {Value = codeToSearch, Text = "Data not found, please try again !"}
					};
			}
			return Json( result );
		}

		/// <summary>
		/// Autocomplete AJAX routine that finds the site.
		/// </summary>
		/// <param name="searchText">The search text.</param>
		/// <param name="esaCode">The esa code.</param>
		/// <param name="maxResults">The maximum results.</param>
		/// <returns></returns>
		[HttpPost] //  helps prevent scripting attacks or data exposure from direct JSON queries
		public JsonResult LookupSite( string searchText, string esaCode, int maxResults )
		{
			var repository = new AdwRepository();
			var codeToSearch = searchText.GetCode();
			var result = repository.LookupSite( codeToSearch, esaCode, maxResults );
			if ( result == null || result.Count < 1 )
			{
				result = new List<SelectListItem>
					{
						new SelectListItem {Value = codeToSearch, Text = "Data not found, please try again !"}
					};
			}
			return Json( result );
		}

		/// <summary>
		///   the routine that Creates the specified project.
		/// </summary>
		/// <param name="vm">The vm.</param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult Create( ProjectCreateViewModel vm )
		{
			CustomProjectValidation( vm, 0 );

			if ( ModelState.IsValid )
			{
				vm.Project.CreatedBy = User.Identity.Name.RemoveDomain();
				vm.Project.CreatedOn = DateTime.Now;
				var newId = PatService.CreateProject( vm.Project );

				TempData[ CommonConstants.FlashMessageTypeInfo ] = string.Format( "Project {0} : {1} successfully created",
																										newId, vm.Project.ProjectName );

				return newId > 0
							 ? RedirectToProjectDetailsPageTab( newId, CommonConstants.ProjectTab_Details )
							 : RedirectToAction( "Index" );
			}
			//  Invalid model state redisplay with errors
			vm.Coordinators = PatService.GetProgramAssuranceDropdownList();

			return View( vm );
		}

		private void CustomProjectValidation( ProjectCreateViewModel vm, int projectId )
		{
			if ( vm.Project.Organisation == null ) return;

			if ( vm.Project.Organisation.Length > 3 )
			{
				var orgCode = vm.Project.Organisation.Substring( 0, 4 );
				if ( PatService.IsValidOrgCode( orgCode ) )
				{
					vm.Project.OrgCode = orgCode;
					vm.Project.ProjectName = vm.Project.StandardProjectName();
					if ( PatService.IsProjectNameUsed( vm.Project.ProjectName.Trim(), projectId ) )
						ModelState.AddModelError( "Project.Organisation",
														  CommonConstants.DuplicateProjectNameMessage );
				}
				else
					ModelState.AddModelError( "Project.Organisation", "Invalid Org Code." );
			}
			else
				ModelState.AddModelError( "Project.Organisation",
												  "Organisation must be at least 4 characters long" );
		}

		#endregion Create Project use case

		#region Project Details use case

		public static int ProjectDetailsGridWidth = 880;

		/// <summary>
		///   Renders the Project Details page - http://wde308498/openWiki/ow.asp?ProgramAssuranceToolProjectDetails
		/// 
		/// http://localhost:6491/Project/Details/130?tabNo=2
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="tabNo">The tab no.</param>
		/// <returns></returns>
		[HttpGet]
		public ActionResult Details( int id, int tabNo )
		{
			AppHelper.SetSessionProjectId( Session, id );

			ViewBag.TabNo = tabNo;
			var loggedInUser = new PatUser( Request.LogonUserIdentity );

			var vm = new ProjectDetailsViewModel();
			var p = PatService.GetProject( id );
			if ( p == null )
			{
				p = new Project
					{
						ProjectId = id,
						ProjectName = "Does not exist",
					};
			}
			else
			{
				p.Organisation = p.GetOrgName();
				p.CoordinatorCode = p.Coordinator;

				p.Coordinator = new PatUser( p.Coordinator ).FullName;

				var contractList = PatService.GetProjectContractsByProject( id );  //  38:43
				vm.ContractCount = contractList.Count;
				ViewData[ "contracts" ] = ContractListFor(p.CanEdit( loggedInUser.LoginName, loggedInUser.IsAdmin() ), p.ProjectId );
				ViewData[ "reviews" ] = GenerateReviewsGrid( id );
				vm.ReviewCount = PatService.CountReviews( id );
				ViewData[ "samples" ] = GenerateSamplesGrid();  //  38:43
				ViewData[ "DeleteMsg" ] = DetermineDeleteMessage( p.ProjectId );
				ViewData[ "attachments" ] = GenerateAttachmentsGrid( id );  //  39:07
				ViewData[ "questions" ] = QuestionList( id );  //  39:07
			}
			vm.Project = p;
			vm.ResourceSet = p.ResourcesSet();
			vm.UserId = loggedInUser.LoginName;
			vm.UserIsAdmin = loggedInUser.IsAdministrator();
			ViewBag.CanEditAttachment = p.CanAddAttachment( loggedInUser.LoginName );
			ViewBag.Selections = PatService.GetProjectContractSelections( id );

			AppHelper.SetSessionProjectName( Session, p.ProjectName );

			if (vm.ContractCount == 0)
				TempData[ CommonConstants.FlashMessageTypeWarning ] =
					"A contract type must be added before a sample or upload can be added to this project.";

			return View( vm );
		}

		private string QuestionList( int projectId )
		{
			var list = PatService.GetProjectQuestions( projectId );
			if ( list == null || list.Count == 0 )
				return "No Questions have been uploaded for this Project.";
			return QuestionListFor( list );
		}

		private static string QuestionListFor( IEnumerable<PatQuestion> questionList )
		{
			var sb = new StringBuilder();
			sb.Append( "<ul class='list-group'>" );
			foreach ( var q in questionList )
				sb.Append( string.Format( "<li class='{1}'>{0}</li>", q, "list-group-item" ) );

			sb.Append( "</ul>" );
			return sb.ToString();
		}

		private object DetermineDeleteMessage( int id )
		{
#if DEBUG
			var stopwatch = new Stopwatch();
			stopwatch.Start();
#endif
			var nUploads = PatService.CountUploads( id );
			var msg = "The Project will be permanently deleted and cannot be recovered. Are you sure?";
			if ( nUploads > 0 )
				msg = PatService.ProjectHasFindings( id )
							? "Project has related upload(s) with outcomes Do you want to continue?"
							: "Project has related upload(s) Do you want to continue?";
#if DEBUG
			var elapsed = AppHelper.StopTheWatch( stopwatch );
			PatService.SaveActivity( string.Format( "DetermineDeleteMessage {1} took {0}", elapsed, id ), "PRJDETS" );

#endif
			return msg;
		}

		private string GenerateSamplesGrid()
		{
#if DEBUG
			var stopwatch = new Stopwatch();
			stopwatch.Start();
#endif
			var g = new Grid( "samples" );
			g.AddColumn( new Column( "Name" ).SetLabel( "Name" ).SetWidth( 370 ) );
			g.AddColumn( new Column( "NoOfReviews" ).SetLabel( "No. of Reviews" ).SetWidth( 110 ).SetAlign( Align.Right ).SetSortable(false) );
			g.AddColumn( new Column( "uploadDate" ).SetLabel( "Upload Date" ).SetWidth( 110 ).SetAlign( Align.Right ).SetSortable( false ) );
			g.AddColumn( new Column( "additionalFlag" ).SetLabel( "Additional" ).SetWidth( 80 ).SetAlign( Align.Center ).SetSortable( false ) );
			g.AddColumn( new Column( "outOfScopeFlag" ).SetLabel( "Out of Scope" ).SetWidth( 80 ).SetAlign( Align.Center ).SetSortable( false ) );
			g.SetUrl( VirtualPathUtility.ToAbsolute( "~/Project/GridDataUploads" ) );
			g.SetHeight( CommonConstants.GridStandardHeight ); //  set this tall enough and you wont see the scroll bars
			g.SetWidth( ProjectDetailsGridWidth );
			g.SetRowNum( CommonConstants.GridStandardNoOfRows ); //  Sets the number of rows displayed initially
			g.SetMultiSelect( false );
			g.SetViewRecords( true ); //  displays the total number of rows in the dataset
			g.SetPager( "samplePager" );
			g.OnGridComplete( "pat.grid_OnGridComplete();" );
#if DEBUG
			var elapsed = AppHelper.StopTheWatch( stopwatch );
			PatService.SaveActivity( string.Format( "Samples Grid construction took {0}", elapsed ), "PRJDETS" );
#endif
			return AppHelper.WcagFixGrid( g, "samples", "Samples for this Project" );
		}

		/// <summary>
		///   generates the Uploads grid
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <returns></returns>
		public ActionResult GridDataUploads( MvcJqGrid.GridSettings gridSettings )
		{
			var projectId = AppHelper.GetSessionProjectId( Session );

			var uploadList = PatService.GetProjectUploads( projectId, gridSettings );

			var totUploads = PatService.CountUploads( projectId );

			var jsonData = new
			{
				total = AppHelper.PagesInTotal( totUploads, gridSettings.PageSize ),
				page = gridSettings.PageIndex,
				records = totUploads,
				rows = (
							 from upload in uploadList.AsEnumerable()
							 select new
							 {
								 id = upload.UploadId,
								 cell = new List<String>
												 {
													 ReviewListLink( upload.UploadId, upload.Name ),
													 AppHelper.FormatInteger( NumberOfReviews( upload ) ),
													 AppHelper.ShortDate( upload.DateUploaded ),
													 AppHelper.FlagOut(upload.AdditionalReview ),
													 AppHelper.FlagOut( ! upload.InScope )
												 }
							 } ).ToArray()
			};
			return Json( jsonData, JsonRequestBehavior.AllowGet );
		}

		/// <summary>
		///   Creates the URL to the Review details page.
		/// </summary>
		/// <param name="uploadId">The upload identifier.</param>
		/// <param name="uploadName">Name of the upload.</param>
		/// <returns></returns>
		public static string ReviewListLink( int uploadId, string uploadName )
		{
			return string.Format( "<a href='/Upload/Details/{0}'>{1}</a>", uploadId, uploadName );
		}

		private int NumberOfReviews( Upload upload )
		{
			return PatService.CountReviewsByUploadId( upload.UploadId );
		}

		private string GenerateReviewsGrid( int projectId )
		{
#if DEBUG
			var stopwatch = new Stopwatch();
			stopwatch.Start();
#endif
			AppHelper.SetSessionProjectId( Session, projectId );
			var g = new Grid( "reviews" );
			g.AddColumn( new Column( "UploadId" )
				.SetLabel( "Upload ID" ).SetWidth( 80 ).SetFixedWidth( true ).SetAlign( Align.Right )
				.SetSearch( false ).SetSortable( false ) );
			g.AddColumn( new Column( "ReviewId" )
				.SetLabel( "Review ID" ).SetWidth( 80 ).SetFixedWidth( true ).SetAlign( Align.Right ) );
			g.AddColumn( new Column( "OrgCode" )
				.SetLabel( "Org Code" ).SetWidth( 80 ).SetFixedWidth( true ).SetAlign( Align.Center ) );
			g.AddColumn( new Column( CommonConstants.ReviewListFieldOrgName )
				.SetLabel( CommonConstants.ReviewListFieldOrgName ).SetWidth( 110 )
				.SetSortable( false ).SetFixedWidth( true ).SetSearch( false ) );
			g.AddColumn( new Column( "EsaCode" )
				.SetLabel( "ESA Code" ).SetWidth( 80 ).SetFixedWidth( true ).SetAlign( Align.Center ) );
			g.AddColumn( new Column( CommonConstants.ReviewListFieldEmploymentServiceAreaName )
				.SetLabel( "ESA Name" ).SetSortable( false )
				.SetWidth( 110 ).SetFixedWidth( true ).SetSearch( false ) );
			g.AddColumn( new Column( "SiteCode" )
				.SetLabel( "Site Code" ).SetWidth( 80 ).SetFixedWidth( true ).SetAlign( Align.Center ) );
			g.AddColumn( new Column( CommonConstants.ReviewListFieldSiteName )
				.SetLabel( CommonConstants.ReviewListHeadingSiteName ).SetWidth( 110 )
				.SetSortable( false ).SetFixedWidth( true ).SetSearch( false ) );
			g.AddColumn( new Column( "StateCode" )
				.SetLabel( "State Code" )
				.SetWidth( 80 ).SetFixedWidth( true ).SetAlign( Align.Center ) );
			g.AddColumn( new Column( "JobseekerId" )
				.SetLabel( "Job Seeker ID" ).SetFixedWidth( true ).SetWidth( 110 ).SetAlign( Align.Right ) );
			g.AddColumn( new Column( CommonConstants.ReviewListFieldJobSeekerGivenName )
				.SetLabel( "Given Name" ).SetSortable( false )
				.SetWidth( 100 ).SetFixedWidth( true ).SetSearch( false ) );
			g.AddColumn( new Column( CommonConstants.ReviewListFieldJobSeekerSurname )
				.SetLabel( "Surname" ).SetSortable( false ).SetWidth( 100 ).SetFixedWidth( true ).SetSearch( false ) );
			g.AddColumn( new Column( "ClaimId" )
				.SetLabel( "Claim ID" ).SetWidth( 80 ).SetFixedWidth( true ).SetAlign( Align.Right ) );
			g.AddColumn( new Column( CommonConstants.ReviewListFieldClaimAmount )
				.SetLabel( "Amount" ).SetWidth( 80 ).SetSortable( false )
				.SetFixedWidth( true ).SetAlign( Align.Right ).SetSearch( false ) );
			g.AddColumn( new Column( CommonConstants.ReviewListFieldClaimType )
				.SetLabel( CommonConstants.ReviewListHeadingClaimType )
				.SetWidth( 80 ).SetSortable( false ).SetFixedWidth( true ).SetAlign( Align.Center ).SetSearch( false ) );
			g.AddColumn( new Column( CommonConstants.ReviewListFieldClaimDescription )
				.SetLabel( "Claim Description" ).SetWidth( 120 ).SetSortable( false ).SetFixedWidth( true ).SetSearch( false ) );
			g.AddColumn( new Column( CommonConstants.ReviewListFieldClaimCreationDate )
				.SetLabel( "Creation Date" ).SetWidth( 100 ).SetSortable( false ).SetFixedWidth( true ).SetSearch( false ) );
			g.AddColumn( new Column( CommonConstants.ReviewListFieldManagedBy )
				.SetLabel( CommonConstants.ReviewListHeadingManagedBy )
				.SetWidth( 100 ).SetFixedWidth( true ).SetSortable( false ).SetSearch( false ).SetAlign( Align.Center ) );
			g.AddColumn( new Column( CommonConstants.ReviewListFieldContractType )
				.SetLabel( CommonConstants.ReviewListHeadingContractType )
				.SetWidth( 100 ).SetSortable( false ).SetFixedWidth( true ).SetAlign( Align.Center ).SetSearch( false ) );
			g.AddColumn( new Column( CommonConstants.ReviewListFieldContractTypeDescription )
				.SetLabel( CommonConstants.ReviewListHeadingContractTypeDescription )
				.SetWidth( 190 ).SetFixedWidth( true ).SetSearch( false ).SetSortable( false ) );
			g.AddColumn( new Column( "ActivityId" )
				.SetLabel( CommonConstants.ReviewListHeadingActivityId ).SetWidth( 80 ).SetFixedWidth( true ).SetAlign( Align.Right ) );
			g.AddColumn( new Column( CommonConstants.ReviewListFieldAutoSpecialClaim )
				.SetLabel( CommonConstants.ReviewListHeadingAutoSpecialClaim ).SetWidth( 60 ).SetSortable( false )
				.SetFixedWidth( true ).SetAlign( Align.Center ).SetSearch( false ) );
			g.AddColumn( new Column( CommonConstants.ReviewListFieldManualSpecialClaim )
				.SetLabel( CommonConstants.ReviewListHeadingManualSpecialClaim ).SetWidth( 60 ).SetSortable( false )
				.SetFixedWidth( true ).SetAlign( Align.Center ).SetSearch( false ) );
			g.AddColumn( new Column( "AssessmentCode" )
				.SetLabel( CommonConstants.ReviewListHeadingAssessmentOutcome )
				.SetWidth( 140 ).SetFixedWidth( true ).SetAlign( Align.Center )
				.SetSearchType( Searchtype.Select ).SetSearchTerms( PatService.GetAssessmentCodes() ) );
			g.AddColumn( new Column( "RecoveryReason" )
				.SetLabel( "Recovery Reason" ).SetWidth( 120 ).SetFixedWidth( true ).SetAlign( Align.Center )
				.SetSearchType( Searchtype.Select ).SetSearchTerms( PatService.GetRecoveryReasons() ) );
			g.AddColumn( new Column( "AssessmentAction" )
				.SetLabel( "Action" ).SetWidth( 70 ).SetFixedWidth( true )
				.SetAlign( Align.Center )
				.SetSearchType( Searchtype.Select ).SetSearchTerms( PatService.GetAssessmentActions() ) );
			g.AddColumn( new Column( "OutcomeCode" )
				.SetLabel( "Final Outcome" ).SetWidth( 100 ).SetFixedWidth( true ).SetAlign( Align.Center )
				.SetSearchType( Searchtype.Select ).SetSearchTerms( PatService.GetFinalOutcomes() ) );

			g.SetUrl( VirtualPathUtility.ToAbsolute( "~/Project/GridDataReviews" ) );
			g.SetHeight( CommonConstants.GridStandardHeight ); //  set this tall enough and you wont see the scroll bars
			g.SetWidth( CommonConstants.GridStandardWidth );  //  will get horizontal scrolling if required
			g.SetRowNum( CommonConstants.GridStandardNoOfRows ); //  Sets the number of rows displayed initially
			g.SetViewRecords( true ); //  displays the total number of rows in the dataset
			g.SetRowList( new[] { 10, 20, 50, 100 } );  //  Fills the dropdownlist in the pager which controls the number of rows per page
			g.SetPager( "reviewPager" );
			g.SetMultiSelect( false );
			g.SetSearchToolbar( true ); //  enable toolbar searching
			g.SetSearchOnEnter( false );
			g.OnGridComplete( "pat.grid_OnGridComplete();" );
#if DEBUG
			var elapsed = AppHelper.StopTheWatch( stopwatch );
			PatService.SaveActivity( string.Format( "Review Grid {1} took {0}", elapsed, projectId ), "PRJDETS" );
#endif
			return AppHelper.WcagFixGrid( g, "reviews", "Review List for the Project" );
		}

		/// <summary>
		///  Generates the Reviews grid.
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <returns></returns>
		public ActionResult GridDataReviews( MvcJqGrid.GridSettings gridSettings )
		{
#if DEBUG
			var stopwatch = new Stopwatch();
			stopwatch.Start();
#endif
			var projectId = AppHelper.GetSessionProjectId( Session );
			object jsonData;
			if (projectId > 0)
			{
				var reviewList = PatService.GetProjectReviews( projectId, gridSettings );

				var totReviews = PatService.CountReviews( projectId, gridSettings );

				jsonData = new
					{
						total = AppHelper.PagesInTotal( totReviews, gridSettings.PageSize ),
						page = gridSettings.PageIndex,
						records = totReviews,
						rows = (
									 from r in reviewList.AsEnumerable()
									 select new
										 {
											 id = r.ReviewId,
											 cell = new List<String>
												 {
													 AppHelper.FormatInteger( r.UploadId ),
													 AppHelper.FormatInteger( r.ReviewId ),
													 r.OrgCode,
													 PatService.GetOrgName( r.OrgCode ),
													 r.ESACode,
													 PatService.GetEsaDescription( r.ESACode ),
													 r.SiteCode,
													 PatService.GetSiteDescription( r.SiteCode ),
													 r.StateCode,
													 AppHelper.FormatLong( r.JobseekerId ),
													 r.JobSeekerGivenName,
													 r.JobSeekerSurname,
													 AppHelper.FormatLong( r.ClaimId ),
													 AppHelper.FormatCurrency( r.ClaimAmount ),
													 r.ClaimType,
													 PatService.GetClaimTypeDescription( r.ClaimType ),
													 AppHelper.ShortDate( r.ClaimCreationDate ),
													 r.ManagedBy,
													 r.ContractType,
													 PatService.GetContractTypeDescription( r.ContractType ),
													 AppHelper.FormatLong( r.ActivityId ),
													 AppHelper.FlagOut( r.AutoSpecialClaim ),
													 AppHelper.FlagOut( r.ManualSpecialClaim ),
													 r.AssessmentCode,
													 r.RecoveryReason,
													 r.AssessmentAction,
													 r.OutcomeCode
												 }
										 } ).ToArray()
					};
#if DEBUG
				var elapsed = AppHelper.StopTheWatch( stopwatch );
				PatService.SaveActivity( string.Format( "GridDataReviews {1} took {0}", elapsed, projectId ), "PRJRGRID" );
#endif
			}
			else
			{
				jsonData = null;
			}
			return Json( jsonData, JsonRequestBehavior.AllowGet );
		}

		private static string ContractListFor(bool canEdit, int projectId )
		{
			return AppHelper.WcagFixGrid( GenerateContractsGrid(canEdit, projectId ), "contracts", "Contract Types for this Project" );
		}

		private static Grid GenerateContractsGrid( bool canEdit, int projectId )
		{
			var g = new Grid( "contracts" );
			g.AddColumn( new Column( "Code" ).SetLabel( "Code" ).SetWidth( 100 ).SetSortable( false ) );
			g.AddColumn( new Column( "ContractDesc" ).SetLabel( "Contract" ).SetWidth( 450 ).SetSortable( false ) );
			g.SetUrl( canEdit
							 ? VirtualPathUtility.ToAbsolute( "~/Project/GridDataContracts" )
							 : VirtualPathUtility.ToAbsolute( string.Format("~/Project/GridDataContractsSelected/{0}", projectId ) ) );
			g.SetHeight( CommonConstants.GridStandardHeight ); //  set this tall enough and you wont see the scroll bars
			g.SetWidth( ProjectDetailsGridWidth );
			if ( canEdit )
				g.SetMultiSelect( true );
			g.SetScroll( true );
			g.OnLoadComplete("pat.projectEditContract_SelectContracts();");
			return g;
		}

		/// <summary>
		/// Generates the grid for the project contracts.
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <returns></returns>
		public ActionResult GridDataContracts( MvcJqGrid.GridSettings gridSettings )
		{
			var contractList = PatService.GetProjectContracts();

			var tot = contractList.Count();

			var jsonData = new
				{
					total = 1,
					page = gridSettings.PageIndex,
					records = tot,
					rows = (
								 from p in contractList.AsEnumerable()
								 select new
									 {
										 id = p.Code,
										 cell = new List<String>
											 {
												 p.Code,
												 p.Description
											 }
									 } ).ToArray()
				};

			return Json( jsonData, JsonRequestBehavior.AllowGet );
		}


		/// <summary>
		/// Generates the grid for the project contracts that have been selected.
		/// </summary>
		/// <param name="id">the project id</param>
		/// <param name="gridSettings">The grid settings.</param>
		/// <returns></returns>
		public ActionResult GridDataContractsSelected( int id, MvcJqGrid.GridSettings gridSettings )
		{
			var contractList = PatService.GetProjectContractsByProject(id);

			var tot = contractList.Count();

			var jsonData = new
			{
				total = 1,
				page = gridSettings.PageIndex,
				records = tot,
				rows = (
							 from p in contractList.AsEnumerable()
							 select new
							 {
								 id = p.Id,
								 cell = new List<String>
											 {
												 p.ContractType,
												 p.GetProjectContractDescription()
											 }
							 } ).ToArray()
			};

			return Json( jsonData, JsonRequestBehavior.AllowGet );
		}

		#endregion Project Details use case

		#region Edit Project use case

		// http://localhost:6491/Project/Edit/12
		/// <summary>
		///   Renders the Edit Project Details page - http://wde308498/openWiki/ow.asp?ProgramAssuranceToolEditProject
		/// 
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		[HttpGet]
		public ActionResult Edit( int id )
		{
			var loggedInUser = new PatUser( Request.LogonUserIdentity );

			var p = PatService.GetProject( id );

			var viewModel = new ProjectCreateViewModel();

			if ( p == null )
			{
				viewModel.Project = new Project { ProjectId = id, ProjectName = string.Format( "Project {0} is unknown", id ) };
				viewModel.Coordinators = new List<SelectListItem>();
				return View( viewModel );
			}

			if ( p.CanEdit( loggedInUser.LoginName, loggedInUser.IsAdministrator() ) )
			{
				AppHelper.SetSessionProjectId( Session, id );
				p.Organisation = p.GetFormattedOrgName();
				viewModel.Coordinators = PatService.GetProgramAssuranceDropdownList();
				viewModel.OriginalProjectId = p.ProjectId;
				viewModel.OriginalProjectName = p.ProjectName;
				viewModel.UserIsAdministrator = loggedInUser.IsAdministrator();
				viewModel.Project = p;

				return View( viewModel );
			}
			TempData[ CommonConstants.FlashMessageTypeWarning ] =
				"Only project coordinator or administrators can edit a project";

			return RedirectToProjectDetailsPageTab( id, CommonConstants.ProjectTab_Details );
		}

		/// <summary>
		///   Processes the Edit project request
		/// </summary>
		/// <param name="vm">The vm.</param>
		/// <param name="collection">The collection.</param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult Edit( ProjectCreateViewModel vm, FormCollection collection )
		{
			var loggedInUser = new PatUser( Request.LogonUserIdentity );
			var theUpdatedName = string.Empty;
			var orgCode = string.Empty;

			if ( loggedInUser.IsAdministrator() )
			{
				//  the project name can be over written
				theUpdatedName = vm.Project.ProjectName;
			}
			if ( loggedInUser.IsAdministrator() )
			{
				vm.Project.ProjectName = theUpdatedName;
				if ( theUpdatedName != vm.OriginalProjectName )
				{
					if ( !string.IsNullOrEmpty( vm.Project.ProjectName ) )
					{
						if (PatService.IsProjectNameUsed( vm.Project.ProjectName.Trim(), vm.OriginalProjectId ))
							ModelState.AddModelError( "Project.ProjectName",
															  CommonConstants.DuplicateProjectNameMessage );
					}
				}
			}

			if ( vm.Project.Organisation != null && vm.Project.Organisation.Length > 3 )
			{
				orgCode = vm.Project.Organisation.Substring( 0, 4 );
				if (!PatService.IsValidOrgCode( orgCode ))
					ModelState.AddModelError( "Project.Organisation", "Invalid Org Code." );
			}

			// model binder creates the Project object, but not the whole view

			if ( ModelState.IsValid )
			{
				vm.Project.UpdatedBy = User.Identity.Name.RemoveDomain();
				vm.Project.ProjectId = vm.OriginalProjectId;
				vm.Project.OrgCode = orgCode;
				PatService.UpdateProject( vm.Project );
				TempData[ CommonConstants.FlashMessageTypeInfo ] = "Project successfully updated";
				return RedirectToProjectDetailsPageTab( vm.Project.ProjectId, CommonConstants.ProjectTab_Details );
			}

			//  have another go
			vm.Coordinators = PatService.GetProgramAssuranceDropdownList();
			vm.Project.ProjectName = vm.OriginalProjectName;
			vm.Project.ProjectId = vm.OriginalProjectId;

			return View( vm );
		}

		#endregion Edit Project use case

		#region Edit Project Contracts use case

		/// <summary>
		///   Processes the request to save the selected contracts.
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public ActionResult SaveContracts()
		{
			var msg = String.Empty;
			try
			{
				var loggedInUser = new PatUser( Request.LogonUserIdentity );

				var projectId = AppHelper.GetSessionProjectId( Session );

				if ( Request.Form[ "ids[]" ] != null )
				{
					var arr = Array.ConvertAll( Request.Form[ "ids[]" ].Split( ',' ), Convert.ToString );
					if ( arr.Length > 0 )
					{
						PatService.SaveProjectContracts( projectId, arr, loggedInUser.LoginName );

						msg = "Project Contracts have been successfully saved.";
						TempData[ CommonConstants.FlashMessageTypeInfo ] = msg;
					}
					else
					{
						msg = "Project Contracts not saved. No selections";
						TempData[ CommonConstants.FlashMessageTypeError ] = msg;
					}
				}
			}
			catch ( KeyNotFoundException )
			{
				return Json( new { success = false, message = "Project Contracts not saved. - Key not found" },
								 JsonRequestBehavior.AllowGet );
			}
			catch ( SqlException ex )
			{
				return Json( new { success = false, message = "Project Contracts not saved. - Sql Server exception " + ex.Message },
								 JsonRequestBehavior.AllowGet );
			}
			return Json( new { success = true, message = msg }, JsonRequestBehavior.AllowGet );
		}

		#endregion Edit Project Contracts use case

		private ActionResult RedirectToProjectDetailsPageTab( int projectId, int tabNo )
		{
			return RedirectToAction( "Details", "Project", new { id = projectId, tabNo } );
		}

		#region Delete Project use case

		/// <summary>
		///   Processes the Delete Project request
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="collection">The collection.</param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult Delete( int id, FormCollection collection )
		{
			try
			{
				PatService.DeleteProject( id, User.Identity.Name.RemoveDomain() );
				TempData[ CommonConstants.FlashMessageTypeInfo ] = string.Format( "Project {0} has been deleted.", id );
			}
			catch ( Exception ex )
			{
				Elmah.ErrorLog.GetDefault( null ).Log( new Elmah.Error( ex ) );
				return View( "Error", new HandleErrorInfo( ex, "Project", "Delete" ) );
			}
			return RedirectToAction( "Index", "Project" );
		}

		#endregion Delete Project use case

		#region Export Reviews

		/// <summary>
		///   Processes the Save Export request
		/// </summary>
		/// <param name="vm">The view model containing data to be saved.</param>
		/// <returns></returns>
		[HttpPost]
		public ContentResult SaveExport( ReviewListCriteriaViewModel vm )
		{
			//  step one save to session

			SessionHelper.SetSessionReviewListCriteria( Session, vm );

			return new ContentResult
			{
				Content = string.Empty,
				ContentEncoding = Encoding.UTF8,
				ContentType = "text/csv"
			};
		}

		/// <summary>
		///   Processes the Export Reviews request
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public ContentResult ExportReviews()
		{
			var csvString = string.Empty;
			var criteria = SessionHelper.GetSessionReviewListCriteria( Session );
			var gs = AppHelper.GridSettingsFromReviewCriteria( criteria );

			if (criteria.ProjectId < 1 )
			{
				TempData[ CommonConstants.FlashMessageTypeWarning ] = "Project ID was not found in Session";
			}
			else
			{
				csvString = PatService.ExportReviews( criteria.ProjectId, gs );

				Response.AddHeader( "Content-Disposition",
					string.Format( "attachment; filename=ReviewsForProject-{0}.csv", criteria.ProjectId ) );			
			}

			return new ContentResult
			{
				Content = csvString,
				ContentEncoding = Encoding.UTF8,
				ContentType = "text/csv"
			};
		}

		#endregion Export Reviews

		#region Attachment

		private string GenerateAttachmentsGrid( int projectId )
		{
#if DEBUG
			var stopwatch = new Stopwatch();
			stopwatch.Start();
#endif
			var g = new Grid( "AttachmentGrid" );
			g.AddColumn( new Column( "Id" ).SetLabel( "Id" ).SetWidth( 100 ).SetAlign( Align.Center ) );
			g.AddColumn( new Column( "DocumentName" ).SetLabel( "Document Name" ).SetWidth( 500 ) );
			g.AddColumn( new Column( "Url" ).SetLabel( "File Name" ).SetWidth( 300 ) );
			g.SetUrl( Url.Action( "GridDataAttachment", "Project", new { projectId } ) );
			g.SetHeight( CommonConstants.GridStandardHeight ); // set this tall enough and you won't see the scroll bar
			g.SetWidth( ProjectDetailsGridWidth ); 
			g.SetRowNum( CommonConstants.GridStandardNoOfRows ); // set the number of rows displayed initially
			g.SetViewRecords( true ); // display the total number of rows in the dataset
			g.SetPager( "attachmentPager" );
			g.SetSearchToolbar( false ); // enable toolbar searching
			g.SetSearchOnEnter( false );
			g.OnGridComplete("pat.grid_OnGridComplete()");
#if DEBUG
			var elapsed = AppHelper.StopTheWatch( stopwatch );
			PatService.SaveActivity( string.Format( "Attachments Grid {1} construction took {0}", elapsed, projectId ), "PRJDETS" );

#endif
			return AppHelper.WcagFixGrid( g, "AttachmentGrid", "The Files attached to this Project" );
		}

		public ActionResult GridDataAttachment( MvcJqGrid.GridSettings gridSettings, int projectId )
		{
			var attachmentList = PatService.GetAttachments( gridSettings, projectId );
			var totalAttachment = PatService.CountAttachments( gridSettings, projectId );

			var jsonData = new
			{
				total = AppHelper.PagesInTotal( totalAttachment, gridSettings.PageSize ),
				page = gridSettings.PageIndex,
				records = totalAttachment,
				rows = (
							  from e in attachmentList.AsEnumerable()
							  select new
							  {
								  id = e.Id,
								  cell = new List<string>
														 {
															  e.Id.ToString( CultureInfo.InvariantCulture ),
															  string.Format("<a href='/Attachment/Edit/{0}'>{1}</a>", e.Id, e.DocumentName),
															  string.Format("<a href='/Attachment/Download/{0}'>{1}</a>", e.Id, e.Url)
														 }
							  }
						 ).ToArray()
			};

			return Json( jsonData, JsonRequestBehavior.AllowGet );
		}

		#endregion Attachment

		#region Upload Questions

		/// <summary>
		///   Renders the Input Project Questions page - http://wde308498/openWiki/ow.asp?ProgramAssuranceToolInputProjectQuestions
		/// http://localhost:6491/Project/Questions/58
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		/// <exception cref="System.Web.HttpException">404;Project Not Found</exception>
		[HttpGet]
		public ActionResult Questions( int id )
		{
			var loggedInUser = new PatUser( Request.LogonUserIdentity );

			var p = PatService.GetProject( id );
			if ( p == null ) throw new HttpException( 404, "Project Not Found" );
			SessionHelper.SetSessionProjectId( Session, id );

			if ( loggedInUser.IsAdministrator() )
			{
				var vm = new UploadQuestionsViewModel { ProjectId = p.ProjectId, ProjectName = p.ProjectNameWithId() };
				return View( vm );
			}
			TempData[ CommonConstants.FlashMessageTypeWarning ] = "Insufficient access rights";
			return RedirectToProjectDetailsPageTab( id, CommonConstants.ProjectTab_Details );
		}

		/// <summary>
		///   Processing the CSV file containing the Project questions
		/// </summary>
		/// <param name="vm">The vm.</param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult Questions( UploadQuestionsViewModel vm )
		{
			if ( Request.Files.Count == 0 )
				ModelState.AddModelError( "SourceFile",
												  string.Format( "File {0} failed to upload", vm.SourceFile ) );
			else
			{
				var filename = string.Empty;
				var hpf = Request.Files[ 0 ];
				if ( hpf == null )
					ModelState.AddModelError( "SourceFile", string.Format( "File {0} failed to upload", vm.SourceFile ) );
				else
				{
					var saveStream = ValidateQuestionsUpload( hpf );

					if ( ModelState.IsValid )
					{
						vm.ServerFile = filename;
						var questionsStored = PatService.StoreProjectQuestionData( vm, saveStream );

						TempData[ CommonConstants.FlashMessageTypeInfo ] =
						  string.Format( "{0} questions successfully uploaded", questionsStored );
						return RedirectToProjectDetailsPageTab( vm.ProjectId, CommonConstants.ProjectTab_Questions );
					}
				}
			}
			return View( vm );
		}

		private MemoryStream ValidateQuestionsUpload( HttpPostedFileBase hpf )
		{
			var saveStream = new MemoryStream();
			ValidateHpf( hpf );
			if ( ModelState.IsValid )
			{
				//  Copy the file into a Memory Stream so that it can be used more than once
				var validationStream = CopyAndClose( hpf.InputStream );
				//  This stream is to save the data on successful validation the
				//  validation stream cannot be reused as it gets automatically garbage collected
				saveStream = new MemoryStream( validationStream.ToByteArray() );
				//  Validate the upload
				validationStream.Position = 0;
				var errors = PatService.ValidateQuestionUpload( validationStream );
				foreach ( var cellValidationError in errors )
					ModelState.AddModelError( "", cellValidationError.CellErrorMessage() );
			}
			return saveStream;
		}

		private void ValidateHpf( HttpPostedFileBase hpf )
		{
			if ( string.IsNullOrEmpty( hpf.FileName ) )
				ModelState.AddModelError( "SourceFile",
												  "Please use the Browse button to select the CSV file you would like to upload" );
			else
			{
				if ( AppHelper.IsNotAnExcelCsv( hpf ) )
					ModelState.AddModelError( "SourceFile", CommonConstants.InvalidUploadFileOrFileIsOpenMessage );
				else
				{
					var filename = Path.GetFileName( hpf.FileName );
					if ( filename == null )
						ModelState.AddModelError( "SourceFile", "File name error" );
				}
			}
		}

		private static Stream CopyAndClose( Stream inputStream )
		{
			const int readSize = 256;
			var buffer = new byte[ readSize ];
			var ms = new MemoryStream();

			var count = inputStream.Read( buffer, 0, readSize );
			while ( count > 0 )
			{
				ms.Write( buffer, 0, count );
				count = inputStream.Read( buffer, 0, readSize );
			}
			ms.Position = 0;
			inputStream.Close();
			return ms;
		}

		#endregion Upload Questions
	}
}