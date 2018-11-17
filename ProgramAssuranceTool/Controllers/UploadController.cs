using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MvcJqGrid;
using MvcJqGrid.Enums;
using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Infrastructure.Controllers;
using ProgramAssuranceTool.Infrastructure.Security;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Models;
using ProgramAssuranceTool.ViewModels;
using ProgramAssuranceTool.ViewModels.Sample;
using GridSettings = MvcJqGrid.GridSettings;

namespace ProgramAssuranceTool.Controllers
{
	[CustomAuthorize]
	public class UploadController : InfrastructureController
	{
		public UploadController( IControllerDependencies controllerDependencies )
			: base( controllerDependencies )
		{
		}

		#region  Append Review Data

		// http://localhost:6491/Upload/Append/600
		[HttpGet]
		public ActionResult Append( int id )
		{
			var loggedInUser = new PatUser( Request.LogonUserIdentity );
			var vm = new AppendViewModel();
			var u = PatService.GetUploadById( id );
			if (u == null)
			{
				vm.ProjectName = string.Format( "Upload {0} unknown", id );
				return View( vm );
			}
			var projectId = u.ProjectId;
			var p = PatService.GetProject( projectId );
			if (p == null)
			{
				vm.ProjectName = string.Format( "Project {0} unknown", projectId );
				return View( vm );
			}

			SessionHelper.SetSessionProjectId( Session, projectId );

			var nContracts = PatService.CountContracts( projectId );

			if (p.CanAddUpload( loggedInUser.LoginName, nContracts ))
			{
				vm = new AppendViewModel
					{
						ProjectId = p.ProjectId,
						ProjectName = p.ProjectNameWithId(),
						UploadName = u.Name,
						UploadId = u.UploadId
					};
				return View( vm );
			}
			TempData[ CommonConstants.FlashMessageTypeWarning ] = "Insufficient access rights";
			return RedirectToProjectDetailsPageTab( vm.ProjectId, CommonConstants.ProjectTab_Details );
		}

		[HttpPost]
		public ActionResult Append( AppendViewModel vm )
		{
			CustomValidationAppend(vm);

			if (ModelState.IsValid)
			{
				if (Request.Files.Count == 0)
					ModelState.AddModelError( "SourceFile",
													  string.Format( "File {0} failed to upload", vm.SourceFile ) );
				else
				{
					var filename = string.Empty;
					var hpf = Request.Files[ 0 ];
					if (hpf == null)
						ModelState.AddModelError( "SourceFile", string.Format( "File {0} failed to upload", vm.SourceFile ) );
					else
					{
						var saveStream = ValidateUpload( vm.IncludesOutcomes, filename, hpf );
						if (ModelState.IsValid)
						{
							var upload = PatService.GetUploadById( vm.UploadId );
							upload.CreatedBy = User.Identity.Name.RemoveDomain();
							var reviewsStored = PatService.AppendReviewData( upload, saveStream );
							TempData[ CommonConstants.FlashMessageTypeInfo ] =
								string.Format( "{0} reviews have been appended to {1}", reviewsStored, upload.Name );
							return RedirectToReviewListPage( upload.UploadId );
						}
					}
				}
			}
			//  Redisplay the GET page
			var u = PatService.GetUploadById( vm.UploadId );
			var projectId = u.ProjectId;
			var p = PatService.GetProject( projectId );
			vm.ProjectId = projectId;
			vm.ProjectName = p.ProjectNameWithId();
			vm.UploadName = u.Name;
			
			return View( vm );
		}

		private void CustomValidationAppend( AppendViewModel vm )
		{
			if ( vm.OutOfScope && vm.AdditionalReview )
			{
				ModelState.AddModelError( "AdditionalReview", "An upload cannot be both an Additional upload and an Out of Scope upload." );
			}
		}

		private MemoryStream ValidateUpload( bool includesOutcomes, string filename, HttpPostedFileBase hpf )
		{
			var saveStream = new MemoryStream();
			ValidateHpf( hpf, filename );
			if (ModelState.IsValid)
			{
				//  Copy the file into a Memory Stream so that it can be used more than once
				var validationStream = CopyAndClose( hpf.InputStream );
				//  This stream is to save the data on successful validation the
				//  validation stream cannot be reused as it gets automatically garbage collected
				saveStream = new MemoryStream( validationStream.ToByteArray() );
				//  Validate the upload
				validationStream.Position = 0;
				var errors = PatService.ValidateUpload( validationStream, includesOutcomes );
				foreach ( var cellValidationError in errors )
					ModelState.AddModelError( "", cellValidationError.CellErrorMessage() );
			}
			return saveStream;
		}

		#endregion

		#region Upload Review Data
		
		
		// DFX http://localhost:6491/Upload/Create/12
		// TFX http://localhost:6491/Upload/Create/373
		[HttpGet]
		public ActionResult Create( int id )
		{
			var loggedInUser = new PatUser( Request.LogonUserIdentity );

			var p = PatService.GetProject( id );
			if (p == null) throw new HttpException( 404, "Project Not Found" );
			SessionHelper.SetSessionProjectId( Session, id );

			var nContracts = PatService.CountContracts( id );

			if (p.CanAddUpload( loggedInUser.LoginName, nContracts ))
			{
				var vm = new UploadViewModel
					{
						ProjectId = p.ProjectId, 
						ProjectName = p.ProjectNameWithId(), 
						IsAdminUser = loggedInUser.IsAdministrator()
					};
				return View( vm );
			}
			TempData[ CommonConstants.FlashMessageTypeWarning ] = "Insufficient access rights";
			return RedirectToProjectDetailsPageTab( id, CommonConstants.ProjectTab_Details );
		}

		private ActionResult RedirectToProjectDetailsPageTab( int projectId, int tabNo )
		{
			return RedirectToAction( "Details", "Project", new {id = projectId, tabNo} );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="uploadId"></param>
		/// <returns></returns>
		private ActionResult RedirectToReviewListPage( int uploadId )
		{
			return RedirectToAction( "Details", "Upload", new {id = uploadId} );
		}

		[HttpPost]
		public ActionResult Create( UploadViewModel vm )
		{
			CustomValidation(vm);

			if (ModelState.IsValid)
			{
				var filename = string.Empty;
				if (Request.Files.Count == 0)
					ModelState.AddModelError( "SourceFile",
													  string.Format( "File {0} failed to upload", vm.SourceFile ) );
				else
				{
					var hpf = Request.Files[ 0 ];
					if (hpf == null)
						ModelState.AddModelError( "SourceFile", string.Format( "File {0} failed to upload", vm.SourceFile ) );
					else
					{
						filename = ValidateHpf( hpf, filename );

						if (ModelState.IsValid)
						{
							var saveStream = ValidateUpload( vm.IncludesOutcomes, filename, hpf );

							if (ModelState.IsValid)
							{
								//  Create a new Upload record
								var upload = new Upload
									{
										AcceptedFlag = false,
										AdditionalReview = vm.AdditionalReview,
										CreatedBy = User.Identity.Name.RemoveDomain(),
										UploadedBy = User.Identity.Name.RemoveDomain(),
										DateUploaded = DateTime.Now,
										ProjectId = vm.ProjectId,
										IncludesOutcomes = vm.IncludesOutcomes,
										InScope = !vm.OutOfScope,
										SourceFile = filename,
										RandomFlag = vm.IsRandom,
										NationalFlag = vm.IsNational
									};
								var newId = PatService.CreateUpload( upload );
								if (newId > 0)
								{
									var reviewsStored = PatService.StoreReviewData( upload, saveStream );
									TempData[ CommonConstants.FlashMessageTypeInfo ] =
										string.Format( "{0} successfully created with {1} reviews", upload.Name, reviewsStored );
									return RedirectToReviewListPage( upload.UploadId );
								}
							}
						}
					}
				}
			}
			return View( vm );
		}

		private void CustomValidation( UploadViewModel vm )
		{
			if (vm.OutOfScope && vm.AdditionalReview)
			{
				ModelState.AddModelError( "AdditionalReview", "An upload cannot be both an Additional upload and an Out of Scope upload." );				
			}
		}

		private string ValidateHpf( HttpPostedFileBase hpf, string filename )
		{
			if (string.IsNullOrEmpty( hpf.FileName ))
				ModelState.AddModelError( "SourceFile",
												  "Please use the Browse button to select the CSV file you would like to upload" );
			else
			{
				if (AppHelper.IsNotAnExcelCsv( hpf ))
					ModelState.AddModelError( "SourceFile", CommonConstants.InvalidUploadFileOrFileIsOpenMessage );
				else
				{
					filename = Path.GetFileName( hpf.FileName );
					if (filename == null)
						ModelState.AddModelError( "SourceFile", "File name error" );
				}
			}
			return filename;
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

		#endregion

		#region  Review List

		// http://localhost:6491/Upload/Details/599
		public ActionResult Details( int id )
		{
			var loggedInUser = new PatUser( Request.LogonUserIdentity );

			var cvm = new CustomiseReviewGridViewModel {UserId = loggedInUser.LoginName, UploadId = id};
			var customisations = PatService.GetGridCustomisations( cvm );
			SessionHelper.SetSessionCustomisations( Session, customisations );

			var vm = new UploadReviewListViewModel();
			var uploadId = id; //  prefer to call the id, uploadId, over MVC convention
			var upload = PatService.GetUploadById( uploadId );
			if (upload == null)
			{
				vm.Project = new Project {ProjectId = 0, ProjectName = "Unknown"};
				vm.Upload = new Upload {UploadId = id, Name = "Unknown"};
				TempData[ CommonConstants.FlashMessageTypeError ] = string.Format( "Upload Id {0} not found", id );
				return View( vm );
			}

			var project = PatService.GetProject( upload.ProjectId );
			var canEdit = loggedInUser.IsAdministrator() || loggedInUser.IsInAnyOfTheseGroups( project.ResourcesSetShort() );
			AppHelper.SetSessionProjectId( Session, project.ProjectId );
			AppHelper.SetSessionUploadId( Session, uploadId );
			AppHelper.SetSessionUploadName(Session, upload.Name);

			vm = new UploadReviewListViewModel
				{
					Upload = upload, Project = project, CanEdit = canEdit, Margin = customisations.Margin, Width = customisations.GridWidth
				};

			ViewData[ "reviews" ] = GenerateReviewsGrid( uploadId, canEdit, project.ProjectType, customisations );

			// clear the previous selected review from session
			if (Session != null) Session["revids"] = new int[] {};

			// clear the checklist session
			AppHelper.ClearSessionCheckLists(Session);

			return View( vm );
		}

		private static Grid GenerateReviewsGrid( 
			int uploadId, bool canEdit, string projectType, CustomiseReviewGridViewModel customisations )
		{
			var g = new Grid( "reviews" );
			//  23 standard fields
			g.AddColumn(
				new Column( "ReviewId" ).SetLabel( "Review ID" ).SetWidth( 80 )
												.SetFixedWidth( true ).SetAlign( Align.Right ).SetSortable( false ) );

			// DR01039560 
			if (projectType.Equals( DataConstants.ProjectType_Contract_Monitoring, StringComparison.OrdinalIgnoreCase ) ||
				 projectType.Equals( DataConstants.ProjectType_Site_Monitoring, StringComparison.OrdinalIgnoreCase ))
			{
				g.AddColumn(
					new Column( "IsCheckListCompleted" ).SetLabel( "CheckList" )
																	.SetAlign( Align.Center )
																	.SetWidth( 80 )
																	.SetFixedWidth( true ) );
			}

			var columns = customisations.GetColumns();
			foreach ( var column in columns )
				AddReviewColumn( column, g );

			if (projectType.Equals( DataConstants.ProjectType_Contract_Monitoring, StringComparison.OrdinalIgnoreCase ) ||
				 projectType.Equals( DataConstants.ProjectType_Site_Monitoring, StringComparison.OrdinalIgnoreCase ))
			{
				g.SetUrl( canEdit
								 ? VirtualPathUtility.ToAbsolute( string.Format( "~/Upload/GridDataCheckListUploadEdit/{0}", uploadId ) )
								 : VirtualPathUtility.ToAbsolute( string.Format( "~/Upload/GridDataCheckListUpload/{0}", uploadId ) ) );
			}
			else
			{
				g.SetUrl( canEdit
								 ? VirtualPathUtility.ToAbsolute( string.Format( "~/Upload/GridDataUploadEdit/{0}", uploadId ) )
								 : VirtualPathUtility.ToAbsolute( string.Format( "~/Upload/GridDataUpload/{0}", uploadId ) ) );				
			}

			g.SetHeight( customisations.Depth == 0 
				? CommonConstants.GridStandardHeight : customisations.Depth );
			g.SetWidth( customisations.GridWidth == 0 
				? CommonConstants.GridStandardWidth : customisations.GridWidth );
			g.SetRowNum( customisations.PageSize == null
				? CommonConstants.GridStandardNoOfRows
				: Int32.Parse( customisations.PageSize ) );

			g.SetViewRecords( true );
			g.SetRowList( new[] {10, 15, 20, 50, 100, 200, 500} );
			if (canEdit) g.SetMultiSelect( true );
			g.SetPager( "pager" );
			g.OnGridComplete( "pat.grid_OnGridComplete();" );
			return g;
		}

		private static void AddReviewColumn( string columnName, Grid g )
		{
			switch (columnName)
			{
				case CommonConstants.ReviewListHeadingJobSeekerId:
					g.AddColumn(
						new Column( CommonConstants.ReviewListFieldJobSeekerId )
						.SetLabel( CommonConstants.ReviewListHeadingJobSeekerId )
						.SetWidth( 100 )
						.SetSortable( false )
						.SetFixedWidth( true )
						.SetAlign( Align.Right ) );
					break;

				case CommonConstants.ReviewListHeadingJobSeekerGivenName :
					g.AddColumn(
						new Column( CommonConstants.ReviewListFieldJobSeekerGivenName )
						.SetLabel( "Given Name" )
						.SetSortable( false )
						.SetWidth( 100 ).SetFixedWidth( true ) );
					break;

			  case CommonConstants.ReviewListHeadingJobSeekerSurname :
					g.AddColumn( new Column( CommonConstants.ReviewListFieldJobSeekerSurname )
						.SetLabel( "Surname" )
												.SetSortable( false )
						.SetWidth( 100 ).SetFixedWidth( true ) );
					break;

				case CommonConstants.ReviewListHeadingClaimId:
					g.AddColumn( new Column( CommonConstants.ReviewListFieldClaimId )
						.SetLabel( CommonConstants.ReviewListHeadingClaimId )
						.SetWidth( 80 ).SetFixedWidth( true )
												.SetSortable( false )
						.SetAlign( Align.Right ) );
					break;

				case CommonConstants.ReviewListHeadingClaimAmount:

					g.AddColumn( new Column( CommonConstants.ReviewListFieldClaimAmount )
						.SetLabel( CommonConstants.ReviewListHeadingClaimAmount )
						.SetWidth( 80 ).SetSortable( false )
						.SetFixedWidth( true )
						.SetAlign( Align.Right )
						.SetSearch( false ) );
					break;

				case CommonConstants.ReviewListHeadingClaimType:
					g.AddColumn( new Column( CommonConstants.ReviewListFieldClaimType )
						.SetLabel( CommonConstants.ReviewListHeadingClaimType )
						.SetWidth( 80 ).SetSortable( false )
						.SetFixedWidth( true )
						.SetAlign( Align.Center )
						.SetSearch( false ) );
					break;

				case CommonConstants.ReviewListHeadingClaimDescription:

					g.AddColumn( 
						new Column( CommonConstants.ReviewListFieldClaimDescription )
						.SetLabel( CommonConstants.ReviewListHeadingClaimDescription )
						.SetWidth( 120 ).SetSortable( false )
						.SetFixedWidth( true ) );
					break;

				case CommonConstants.ReviewListHeadingClaimCreationDate:
					g.AddColumn( 
						new Column( CommonConstants.ReviewListFieldClaimCreationDate )
						.SetLabel( CommonConstants.ReviewListHeadingClaimCreationDate )
														 .SetWidth( 100 ).SetSortable( false )
														 .SetFixedWidth( true )
														 .SetSearch( false ) );
					break;

				case CommonConstants.ReviewListHeadingOrgCode:

					g.AddColumn( new Column( CommonConstants.ReviewListFieldOrgCode )
						.SetLabel( "Org Code" )
						.SetWidth( 80 ).SetSortable( false )
						.SetFixedWidth( true )
						.SetAlign( Align.Center ) );
					break;

				case CommonConstants.ReviewListHeadingOrgName:

					g.AddColumn( new Column( CommonConstants.ReviewListFieldOrgName )
						.SetLabel( CommonConstants.ReviewListHeadingOrgName )
						.SetWidth( 110 ).SetSortable( false )
						.SetFixedWidth( true ).SetSearch( false ) );
					break;

				case CommonConstants.ReviewListHeadingEmploymentServiceAreaCode:
					g.AddColumn( 
						new Column( CommonConstants.ReviewListFieldEmploymentServiceAreaCode )
							.SetLabel( "ESA Code" )
							.SetWidth( 80 ).SetSortable( false )
							.SetFixedWidth( true )
							.SetAlign( Align.Center ) );
					break;

				case CommonConstants.ReviewListHeadingEmploymentServiceAreaName:

					g.AddColumn( 
						new Column( CommonConstants.ReviewListFieldEmploymentServiceAreaName )
						.SetLabel( "ESA Name" ).SetSortable( false )
						.SetWidth( 110 ).SetFixedWidth( true ) );
					break;

				case CommonConstants.ReviewListHeadingSiteCode:

					g.AddColumn( new Column( CommonConstants.ReviewListFieldSiteCode )
						.SetLabel( CommonConstants.ReviewListHeadingSiteCode )
						.SetWidth( 80 ).SetSortable( false )
						.SetFixedWidth( true )
						.SetAlign( Align.Center ) );
					break;

				case CommonConstants.ReviewListHeadingSiteName:

					g.AddColumn( new Column( CommonConstants.ReviewListFieldSiteName )
						.SetLabel( CommonConstants.ReviewListHeadingSiteName )
						.SetWidth( 110 ).SetSortable( false )
						.SetFixedWidth( true )
						.SetSearch( false ) );
					break;

				case CommonConstants.ReviewListHeadingStateCode:

					g.AddColumn( new Column( CommonConstants.ReviewListFieldStateCode )
						.SetLabel( CommonConstants.ReviewListHeadingStateCode )
						.SetWidth( 80 ).SetSortable( false )
						.SetFixedWidth( true )
						.SetAlign( Align.Center ) );
					break;

				case CommonConstants.ReviewListHeadingManagedBy:

					g.AddColumn( new Column( CommonConstants.ReviewListFieldManagedBy )
						.SetLabel( CommonConstants.ReviewListHeadingManagedBy )
						.SetWidth( 100 ).SetFixedWidth( true ).SetSortable( false )
						.SetSearch( false ).SetAlign( Align.Center ) );
					break;

				case CommonConstants.ReviewListHeadingContractType:

					g.AddColumn(
						new Column( CommonConstants.ReviewListFieldContractType )
						.SetLabel( CommonConstants.ReviewListHeadingContractType )
						.SetWidth( 100 ).SetSortable( false )
						.SetFixedWidth( true )
						.SetAlign( Align.Center ).SetSearch( false ) );
					break;

				case CommonConstants.ReviewListHeadingContractTypeDescription:

					g.AddColumn(
						new Column( CommonConstants.ReviewListFieldContractTypeDescription )
						.SetLabel( CommonConstants.ReviewListHeadingContractTypeDescription )
						.SetWidth( 190 )
						.SetFixedWidth( true ).SetSearch( false ) );
					break;

				case CommonConstants.ReviewListHeadingActivityId:

					g.AddColumn(
						new Column( CommonConstants.ReviewListFieldActivityId )
						.SetLabel( CommonConstants.ReviewListHeadingActivityId )
						.SetWidth( 80 ).SetSortable( false )
						.SetFixedWidth( true ).SetSearch( false ) );
					break;

				case CommonConstants.ReviewListHeadingAutoSpecialClaim:

					g.AddColumn(
						new Column( CommonConstants.ReviewListFieldAutoSpecialClaim )
							.SetLabel( CommonConstants.ReviewListHeadingAutoSpecialClaim )  
							.SetWidth( 60 ).SetSortable( false )
							.SetFixedWidth( true )
							.SetAlign( Align.Center )
							.SetSearch( false ) );
					break;

				case CommonConstants.ReviewListHeadingManualSpecialClaim:

					g.AddColumn(
						new Column( CommonConstants.ReviewListFieldManualSpecialClaim )
							.SetLabel( CommonConstants.ReviewColumnManualSpecialClaim)  
							.SetWidth( 60 ).SetSortable( false )
							.SetFixedWidth( true )
							.SetAlign( Align.Center )
							.SetSearch( false ) );
					break;

				case CommonConstants.ReviewListHeadingAssessmentOutcome:

					g.AddColumn( 
						new Column( CommonConstants.ReviewListFieldAssessmentOutcome )
						.SetLabel( CommonConstants.ReviewListHeadingAssessmentOutcome )
						.SetWidth( 140 ).SetSortable( false )
						.SetFixedWidth( true )
						.SetAlign( Align.Center ) );
					break;

				case CommonConstants.ReviewListHeadingRecoveryReason:

					g.AddColumn( 
						new Column( CommonConstants.ReviewListFieldRecoveryReason )
						.SetLabel( CommonConstants.ReviewListHeadingRecoveryReason )
						.SetWidth( 120 ).SetSortable( false )
						.SetFixedWidth( true )
						.SetAlign( Align.Center ) );
					break;

				case CommonConstants.ReviewListHeadingAssessmentAction:

					g.AddColumn( 
						new Column( CommonConstants.ReviewListFieldAssessmentAction )
						.SetLabel( CommonConstants.ReviewListHeadingAssessmentAction )
						.SetWidth( 120 ).SetFixedWidth( true ).SetSortable( false )
						.SetAlign( Align.Center ) );
					break;

				case CommonConstants.ReviewListHeadingFinalOutcome:

					g.AddColumn( 
						new Column( CommonConstants.ReviewListFieldFinalOutcome )
						.SetLabel( "Final Outcome" )
						.SetWidth( 100 ).SetSortable( false )
						.SetFixedWidth( true )
						.SetAlign( Align.Center ) );
					break;

				default:
					var ex = new ApplicationException( string.Format( "Unknown column {0}", columnName ) );
					Elmah.ErrorLog.GetDefault( null ).Log( new Elmah.Error( ex ) );
					throw ex;
			}
		}

		[HttpPost]
		public ContentResult ExportReviews()
		{
			var uploadId = AppHelper.GetSessionUploadId( Session );

			var csvString = PatService.ExportReviewsForUpload( uploadId );

			Response.AddHeader( "Content-Disposition",
									  string.Format( "attachment; filename=ReviewsForUpload-{0}.csv", uploadId ) );

			return new ContentResult
				{
					Content = csvString,
					ContentEncoding = Encoding.UTF8,
					ContentType = "text/csv"
				};
		}

		public ActionResult GridDataCheckListUploadEdit( int id, string userId, GridSettings gridSettings )
		{
			gridSettings = ApplyCustomisationsToGridSettings( gridSettings );
			List<Review> reviewList;
			var totRecords = SetGridData( id, gridSettings, out reviewList );
			var jsonData = GetJsonData( gridSettings, totRecords, reviewList, isEdit: true, hasChecklist: true );
			return Json( jsonData, JsonRequestBehavior.AllowGet );
		}

		public ActionResult GridDataCheckListUpload( int id, string userId, GridSettings gridSettings )
		{
			gridSettings = ApplyCustomisationsToGridSettings( gridSettings );
			List<Review> reviewList;
			var totRecords = SetGridData( id, gridSettings, out reviewList );
			var jsonData = GetJsonData( gridSettings, totRecords, reviewList, isEdit: false, hasChecklist: true );
			return Json( jsonData, JsonRequestBehavior.AllowGet );
		}

		public ActionResult GridDataUploadEdit( int id, string userId, GridSettings gridSettings )
		{
			gridSettings = ApplyCustomisationsToGridSettings( gridSettings );
			List<Review> reviewList;
			var totRecords = SetGridData( id, gridSettings, out reviewList );
			var jsonData = GetJsonData( gridSettings, totRecords, reviewList, isEdit: true, hasChecklist: false );
			return Json( jsonData, JsonRequestBehavior.AllowGet );
		}

		private GridSettings ApplyCustomisationsToGridSettings( GridSettings gridSettings )
		{
			var customisations = SessionHelper.GetSessionCustomisations( Session );
			if (customisations != null && customisations.AscOrDescending != null )
			{
				gridSettings.SortOrder = AscOrDescFor( customisations.AscOrDescending );
				gridSettings.SortColumn = FieldFor( customisations.SortOrder );
				if ( gridSettings.PageSize < Int32.Parse( customisations.PageSize ) )
					gridSettings.PageSize = Int32.Parse( customisations.PageSize );
			}
			return gridSettings;
		}

		public ActionResult GridDataUpload( int id, string userId, GridSettings gridSettings )
		{
			gridSettings = ApplyCustomisationsToGridSettings( gridSettings );
			List<Review> reviewList;
			var totRecords = SetGridData( id, gridSettings, out reviewList );
			var jsonData = GetJsonData( gridSettings, totRecords, reviewList, isEdit: false, hasChecklist: false );
			return Json( jsonData, JsonRequestBehavior.AllowGet );
		}

		private object GetJsonData( GridSettings gridSettings, int totRecords, IEnumerable<Review> reviewList, bool isEdit, bool hasChecklist )
		{
			var jsonData = new
			{
				total = AppHelper.PagesInTotal( totRecords, gridSettings.PageSize ),
				page = gridSettings.PageIndex,
				records = totRecords,
				rows = (
							 from s in reviewList.AsEnumerable()
							 select new
							 {
								 id = s.ReviewId,
								 cell = GridRow( s, isEdit, hasChecklist )
							 } ).ToArray()
			};
			return jsonData;
		}

		private static string DisplayCheckList( bool isCheckListCompleted )
		{
			// to make it readonly: onclick='return false' 
			return string.Format( "<input type='checkbox' onclick='return false' {0} </input>",
										 isCheckListCompleted ? "checked='checked'" : string.Empty );
		}

		private int SetGridData( int id, GridSettings gridSettings, out List<Review> reviewList )
		{
			var uploadId = id;
			var totRecords = PatService.CountReviewsByUploadId( uploadId );
			reviewList = PatService.GetUpload( uploadId, gridSettings );
			return totRecords;
		}

		private List<String> GridRow( Review s, bool isEdit, bool hasChecklist )
		{
			var customisations = SessionHelper.GetSessionCustomisations( Session );

			var theRow = new List<String>
				{
					isEdit ? AppHelper.ReviewEditLink( s.ReviewId ) : AppHelper.FormatInteger( s.ReviewId )
				};
			if (hasChecklist)
				theRow.Add( DisplayCheckList( s.IsCheckListCompleted ) );

			foreach ( var column in customisations.GetColumns() )
				AddReviewData( column, theRow, s );

			return theRow;
		}

		private void AddReviewData( string columnName, ICollection<string> cells, Review s )
		{
			switch ( columnName )
			{
				case CommonConstants.ReviewListHeadingJobSeekerId:
					cells.Add( AppHelper.FormatLong( s.JobseekerId ) );
					break;

				case CommonConstants.ReviewListHeadingJobSeekerGivenName:
					cells.Add( s.JobSeekerGivenName );
					break;

				case CommonConstants.ReviewListHeadingJobSeekerSurname:
					cells.Add( s.JobSeekerSurname );
					break;

				case CommonConstants.ReviewListHeadingClaimId:
					cells.Add( AppHelper.FormatLong( s.ClaimId ) );
					break;

				case CommonConstants.ReviewListHeadingClaimAmount:

					cells.Add( AppHelper.FormatCurrency( s.ClaimAmount ) );
					break;

				case CommonConstants.ReviewListHeadingClaimType:
					cells.Add( s.ClaimType );
					break;

				case CommonConstants.ReviewListHeadingClaimDescription:

					cells.Add( PatService.GetClaimTypeDescription( s.ClaimType ) );
					break;

				case CommonConstants.ReviewListHeadingClaimCreationDate:
					cells.Add( AppHelper.ShortDate( s.ClaimCreationDate ) );
					break;

				case CommonConstants.ReviewListHeadingOrgCode:

					cells.Add( s.OrgCode );
					break;

				case CommonConstants.ReviewListHeadingOrgName:
					var orgName = PatService.GetOrgName( s.OrgCode );
					cells.Add( orgName );
					break;

				case CommonConstants.ReviewListHeadingEmploymentServiceAreaCode:
					cells.Add( s.ESACode );
					break;

				case CommonConstants.ReviewListHeadingEmploymentServiceAreaName:

					cells.Add( PatService.GetEsaDescription( s.ESACode ) );
					break;

				case CommonConstants.ReviewListHeadingSiteCode:

					cells.Add( s.SiteCode );
					break;

				case CommonConstants.ReviewListHeadingSiteName:

					cells.Add( PatService.GetSiteDescription( s.SiteCode ) );
					break;

				case CommonConstants.ReviewListHeadingStateCode:

					cells.Add( s.StateCode );
					break;

				case CommonConstants.ReviewListHeadingManagedBy:

					cells.Add( s.ManagedBy );
					break;

				case CommonConstants.ReviewListHeadingContractType:

					cells.Add( s.ContractType );
					break;

				case CommonConstants.ReviewListHeadingContractTypeDescription:

					cells.Add( PatService.GetContractTypeDescription( s.ContractType ) );
					break;

				case CommonConstants.ReviewListHeadingActivityId:

					cells.Add( s.ActivityId.ToString( CultureInfo.InvariantCulture ) );
					break;

				case CommonConstants.ReviewListHeadingAutoSpecialClaim:

					cells.Add( AppHelper.FlagOut( s.AutoSpecialClaim ) );
					break;

				case CommonConstants.ReviewListHeadingManualSpecialClaim:

					cells.Add( AppHelper.FlagOut( s.ManualSpecialClaim ) );
					break;

				case CommonConstants.ReviewListHeadingAssessmentOutcome:

					cells.Add( PatService.GetAssessmentDescription( s.AssessmentCode ) );
					break;

				case CommonConstants.ReviewListHeadingRecoveryReason:

					cells.Add( PatService.GetRecoveryReasonDescription( s.RecoveryReason ) );
					break;

				case CommonConstants.ReviewListHeadingAssessmentAction:

					cells.Add( PatService.GetAssessmentActionDescription( s.AssessmentAction ) );
					break;

				case CommonConstants.ReviewListHeadingFinalOutcome:

					cells.Add( PatService.GetFinalOutcomeDescription( s.OutcomeCode ) );
					break;

				default:
					var ex = new ApplicationException( string.Format( "Unknown column {0}", columnName ) );
					Elmah.ErrorLog.GetDefault( null ).Log( new Elmah.Error( ex ) );
					throw ex;
			}


		}

		[HttpPost]
		public ActionResult SaveSelections()
		{
			var msg = String.Empty;
			try
			{
				if (HttpContext.Session != null)
				{
					//var sessionKey = HttpContext.Session.SessionID;

					if (Request.Form[ "ids[]" ] != null)
					{
						var intArr = Array.ConvertAll( Request.Form[ "ids[]" ].Split( ',' ), Convert.ToInt32 );
						if (intArr.Length > 0)
						{
							//  save the selections into the Session object
							Session[ "revids" ] = intArr;
							msg = "OK";
						}
					}
				}
			}
			catch (KeyNotFoundException)
			{
				return Json( new {success = false, message = "Selections not saved."}, JsonRequestBehavior.AllowGet );
			}
			return Json( new {success = true, message = msg}, JsonRequestBehavior.AllowGet );
		}

		#endregion

		#region  Edit Sample Details

		// http://localhost:6491/Upload/Edit/599
		[HttpGet]
		public ActionResult Edit( int id )
		{
			var loggedInUser = new PatUser( Request.LogonUserIdentity );
			var viewModel = new UploadEditViewModel();

			var upload = PatService.GetUploadById( id );
			if (upload == null)
			{
				viewModel.UploadId = id;
				viewModel.ProjectId = 0;
				viewModel.ProjectName = string.Format( "Upload Id {0} not found", id );
			}
			else
			{
				var nReviews = PatService.CountReviewsByUploadId( id );
				var nCompletedReviews = PatService.CountCompletedReviewsByUploadId( id );
				var project = PatService.GetProject( upload.ProjectId );
				viewModel.Reviews = nReviews;
				viewModel.CompletedReviews = nCompletedReviews;

				viewModel.UploadId = upload.UploadId;
				viewModel.ProjectName = project.ProjectNameWithId();
				viewModel.SampleName = upload.Name;
				viewModel.SampleDueDate = upload.DueDate;
				viewModel.OriginalName = upload.Name;
				viewModel.SampleStartDate = upload.DateUploaded;
				viewModel.IsAdditional = upload.AdditionalReview;
				viewModel.IsInScope = upload.InScope;
				viewModel.IsAccepted = upload.AcceptedFlag;
				viewModel.IsRandom = upload.RandomFlag;
				viewModel.IsNational = upload.NationalFlag;
				viewModel.IsAdministrator = loggedInUser.IsAdministrator();
				viewModel.IsProjectCoordinator = project.Coordinator.Equals( loggedInUser.LoginName );
				viewModel.ProjectIsContractMonitoringOrContractSiteVisit = project.IsContractMonitoringOrContractSiteVisit();
				viewModel.ProjectId = upload.ProjectId;
				ViewData[ "sample-data" ] = GenerateSampleData( upload, nReviews, nCompletedReviews );
			}

			return View( viewModel );
		}

		private static string GenerateSampleData( Upload upload, int nReviews, int nCompletedReviews )
		{
			string[] label;
			string[] data;
			string tableHeading;
			var isUpload = upload.HasSourceFile();
			if ( isUpload )  // has a source file 
			{
				tableHeading = "Upload Data";
				label = new[]
					{
						"Upload Id",
						"Source",
						"Uploaded",
						"Uploaded by",
						"Number of reviews",
						"Number of completed reviews",
						"Random",
						"Additional reviews",
						"Last updated",
						"Updated by",
						"Accepted"
					};

				data = new[]
					{
						AppHelper.FormatInteger( upload.UploadId ),
						"UPLOAD",
						AppHelper.ShortDate( upload.DateUploaded ),
						upload.UploadedBy,
						AppHelper.FormatInteger( nReviews ),
						AppHelper.FormatInteger( nCompletedReviews ),
						AppHelper.FlagOut( upload.RandomFlag ),
						AppHelper.FlagOut( upload.AdditionalReview ),
						AppHelper.ShortDate( upload.UpdatedOn ),
						upload.UpdatedBy,
						AppHelper.FlagOut( upload.AcceptedFlag )
					};
			} 
			else
			{
				tableHeading = "Sample Data";
				label = new[]
					{
						"Sample Id",
						"Source",
						"Sample start date",
						"Number of reviews",
						"Number of completed reviews",
						"Random",
						"Additional reviews",
						"Last updated",
						"Updated by"
					};

				data = new[]
					{
						AppHelper.FormatInteger( upload.UploadId ),
						"SAMPLE",
						AppHelper.ShortDate( upload.DateUploaded ),
						AppHelper.FormatInteger( nReviews ),
						AppHelper.FormatInteger( nCompletedReviews ),
						AppHelper.FlagOut( upload.RandomFlag ),
						AppHelper.FlagOut( upload.AdditionalReview ),
						AppHelper.ShortDate( upload.UpdatedOn ),
						upload.UpdatedBy
					};				
			}

			var html = AppHelper.TableFromArray( label, data, tableHeading, showEmpty:true );
			return html;
		}

		[HttpPost]
		public ActionResult Edit( UploadEditViewModel vm )
		{
			CustomSampleNameValidation( vm );
			var upload = PatService.GetUploadById( vm.UploadId );

			if (ModelState.IsValid)
			{
				upload.Name = vm.SampleName;
				upload.AcceptedFlag = vm.IsAccepted;
				upload.NationalFlag = vm.IsNational;
				upload.RandomFlag = vm.IsRandom;
				upload.DueDate = vm.SampleDueDate;
				upload.UpdatedBy = User.Identity.Name.RemoveDomain();
				PatService.UpdateUpload( upload );
				vm.OriginalName = upload.Name; //  in case of a second edit

				TempData[ CommonConstants.FlashMessageTypeInfo ] = string.Format( "{0} successfully edited", vm.SampleName );
				ViewData[ "sample-data" ] = GenerateSampleData( upload, vm.Reviews, vm.CompletedReviews );
				return RedirectToReviewListPage( vm.UploadId );
			}
			ViewData[ "sample-data" ] = GenerateSampleData( upload, vm.Reviews, vm.CompletedReviews );
			return View( vm );
		}

		private void CustomSampleNameValidation( UploadEditViewModel vm )
		{
			if ( vm.ProjectIsContractMonitoringOrContractSiteVisit)
			{
				if ((vm.SampleDueDate != new DateTime(1,1,1)) && vm.SampleDueDate < vm.SampleStartDate)
					ModelState.AddModelError( "SampleDueDate", "The Sample End Date cannot be before the Sample Start Date." );
			}

			if (vm.SampleName == null) return;

			//  Check Sample name
			if (vm.SampleName.Equals( vm.OriginalName )) return;

			if (PatService.SampleNameIsUsed( vm.SampleName, vm.ProjectId ))
				ModelState.AddModelError( "SampleName", "Sample Name already exists. Please enter a different Sample Name." );
		}

		#endregion

		#region Delete Upload

		// http://localhost:6491/Upload/Delete/597
		public ActionResult Delete( int id )
		{
			var loggedInUser = new PatUser( Request.LogonUserIdentity );
			var upload = PatService.GetUploadById( id );
			var p = PatService.GetProject( upload.ProjectId );
			if (p.CanEdit( loggedInUser ))
			{
				upload.UpdatedBy = loggedInUser.LoginName;
				PatService.DeleteUpload( upload );
				TempData[ CommonConstants.FlashMessageTypeInfo ] = string.Format( "Upload {0} deleted", id );
			}
			else
				TempData[ CommonConstants.FlashMessageTypeWarning ] = "Insufficient access rights";
			return RedirectToProjectDetailsPageTab( upload.ProjectId, CommonConstants.ProjectTab_Samples );
		}

		#endregion

		#region  Customise Grid View

		// http://localhost:6491/Upload/Customise/599

		[HttpGet]
		public ActionResult Customise( int id )
		{
			var loggedInUser = new PatUser( Request.LogonUserIdentity );

			var vm = SessionHelper.GetSessionCustomisations( Session );
			if (vm.UserId == null)
			{
				vm = new CustomiseReviewGridViewModel {UploadId = id, UserId = loggedInUser.LoginName};
				vm = PatService.GetGridCustomisations( vm );
			}
			vm.ProjectId = AppHelper.GetSessionProjectId( Session );
#if DEBUG
			vm.ChangeDepth = true;
			vm.ChangeGridWidth = true;
			vm.ChangeMargin = true;
#endif
			return View( vm );
		}

		[HttpPost]
		public ActionResult Customise( CustomiseReviewGridViewModel vm, string button, FormCollection formValues )
		{
			if (button == "reset")
			{
				PatService.ResetGridCustomisations( vm.UserId );
				vm = PatService.GetGridCustomisations( vm );
				SessionHelper.SetSessionCustomisations( Session, vm );
				return RedirectToCustomisePage( vm.UploadId );
			}
			if (button == "refresh")
			{
				var newVm = PatService.RefreshGridCustomisations( vm );
				SessionHelper.SetSessionCustomisations( Session, newVm );
				return RedirectToCustomisePage( vm.UploadId );
			}

			CustomCustomiseValidation( vm );

			if (ModelState.IsValid)
			{
				PatService.SaveGridCustomisations( vm );

				TempData[ CommonConstants.FlashMessageTypeInfo ] = "Customisations successfully saved";

				return RedirectToReviewListPage( vm.UploadId );
			}

			//  Invalid model state, repopoulate vm if required, redisplay with errors

			return View( vm );
		}

		private ActionResult RedirectToCustomisePage( int uploadId )
		{
			return RedirectToAction( "Customise", "Upload", new {id = uploadId} );
		}

		private void CustomCustomiseValidation( CustomiseReviewGridViewModel vm )
		{
			if (vm.ColumnCount() == 0)
				ModelState.AddModelError( "Col01", "Please select at least 1 column." );
			else
			{
				//  check that the sort order is valid
				if (!vm.ColumnsContain( vm.SortOrder ))
					ModelState.AddModelError( "SortOrder",
													  "Sort order should be one of the columns you have chosen." );
			}
		}

		private static string AscOrDescFor( string ascOrDescending )
		{
			if (string.IsNullOrEmpty( ascOrDescending )) return "asc";
			return ascOrDescending.Equals( "Ascending" ) ? "asc" : "desc";
		}

		private static string FieldFor( string sortOrder )
		{
			var fieldName = CommonConstants.ReviewListFieldClaimId;
			if ( sortOrder.Equals( CommonConstants.ReviewListHeadingAutoSpecialClaim ) )
			{
				fieldName = CommonConstants.ReviewListFieldAutoSpecialClaim;
			}
			else if ( sortOrder.Equals( CommonConstants.ReviewListHeadingManualSpecialClaim ) )
			{
				fieldName = CommonConstants.ReviewListFieldManualSpecialClaim;
			}
			else if ( sortOrder.Equals( CommonConstants.ReviewListHeadingJobSeekerId ) )
			{
				fieldName = CommonConstants.ReviewListFieldJobSeekerId;
			}
			else if ( sortOrder.Equals( CommonConstants.ReviewListHeadingClaimId ) )
			{
				fieldName = CommonConstants.ReviewListFieldClaimId;
			}
			else if ( sortOrder.Equals( CommonConstants.ReviewListHeadingJobSeekerGivenName ) )
			{
				fieldName = CommonConstants.ReviewListFieldJobSeekerGivenName;
			}
			else if ( sortOrder.Equals( CommonConstants.ReviewListHeadingJobSeekerSurname ) )
			{
				fieldName = CommonConstants.ReviewListFieldJobSeekerSurname;
			}
			else if ( sortOrder.Equals( CommonConstants.ReviewListHeadingClaimAmount ) )
			{
				fieldName = CommonConstants.ReviewListFieldClaimAmount;
			}
			else if ( sortOrder.Equals( CommonConstants.ReviewListHeadingClaimType ) )
			{
				fieldName = CommonConstants.ReviewListFieldClaimType;
			}
			else if ( sortOrder.Equals( CommonConstants.ReviewListHeadingClaimDescription ) )
			{
				fieldName = CommonConstants.ReviewListFieldClaimDescription;
			}
			else if ( sortOrder.Equals( CommonConstants.ReviewListHeadingEmploymentServiceAreaCode ) )
			{
				fieldName = CommonConstants.ReviewListFieldEmploymentServiceAreaCode;
			}
			else if ( sortOrder.Equals( CommonConstants.ReviewListHeadingEmploymentServiceAreaName ) )
			{
				fieldName = CommonConstants.ReviewListFieldEmploymentServiceAreaName;
			}
			else if ( sortOrder.Equals( CommonConstants.ReviewListHeadingOrgCode ) )
			{
				fieldName = CommonConstants.ReviewListFieldOrgCode;
			}
			else if ( sortOrder.Equals( CommonConstants.ReviewListHeadingOrgName ) )
			{
				fieldName = CommonConstants.ReviewListFieldOrgName;
			}
			else if ( sortOrder.Equals( CommonConstants.ReviewListHeadingSiteCode ) )
			{
				fieldName = CommonConstants.ReviewListFieldSiteCode;
			}
			else if ( sortOrder.Equals( CommonConstants.ReviewListHeadingSiteName ) )
			{
				fieldName = CommonConstants.ReviewListFieldSiteName;
			}
			else if ( sortOrder.Equals( CommonConstants.ReviewListHeadingStateCode ) )
			{
				fieldName = CommonConstants.ReviewListFieldStateCode;
			}
			else if ( sortOrder.Equals( CommonConstants.ReviewListHeadingManagedBy ) )
			{
				fieldName = CommonConstants.ReviewListFieldManagedBy;
			}
			else if ( sortOrder.Equals( CommonConstants.ReviewListHeadingContractType ) )
			{
				fieldName = CommonConstants.ReviewListFieldContractType;
			}
			else if ( sortOrder.Equals( CommonConstants.ReviewListHeadingContractTypeDescription ) )
			{
				fieldName = CommonConstants.ReviewListFieldContractTypeDescription;
			}
			else if ( sortOrder.Equals( CommonConstants.ReviewListHeadingClaimCreationDate ) )
			{
				fieldName = CommonConstants.ReviewListFieldClaimCreationDate;
			}
			else if ( sortOrder.Equals( CommonConstants.ReviewListHeadingActivityId ) )
			{
				fieldName = CommonConstants.ReviewListFieldActivityId;
			}
			else if ( sortOrder.Equals( CommonConstants.ReviewListHeadingAssessmentOutcome ) )
			{
				fieldName = CommonConstants.ReviewListFieldAssessmentOutcome;
			}
			else if ( sortOrder.Equals( CommonConstants.ReviewListHeadingRecoveryReason ) )
			{
				fieldName = CommonConstants.ReviewListFieldRecoveryReason;
			}
			else if ( sortOrder.Equals( CommonConstants.ReviewListHeadingAssessmentAction ) )
			{
				fieldName = CommonConstants.ReviewListFieldAssessmentAction;
			}
			else if ( sortOrder.Equals( CommonConstants.ReviewListHeadingFinalOutcome ) )
			{
				fieldName = CommonConstants.ReviewListFieldFinalOutcome;
			}
			return fieldName;
		}

		#endregion
	}
}