using System.Globalization;
using Elmah;
using MvcJqGrid;
using MvcJqGrid.Enums;
using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Infrastructure.Controllers;
using ProgramAssuranceTool.Infrastructure.Security;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Models;
using ProgramAssuranceTool.ViewModels.Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GridSettings = MvcJqGrid.GridSettings;

namespace ProgramAssuranceTool.Controllers
{
	[CustomAuthorize]
	public class SampleController : InfrastructureController
	{

		public SampleController( IControllerDependencies controllerDependencies )
			: base( controllerDependencies )
		{
		}

		//http://localhost:6491/Sample/Create/33
		[HttpGet]
		public ActionResult Create( int id )
		{
			var loggedInUser = new PatUser( Request.LogonUserIdentity );

			if ( HttpContext.Session != null )
			{
				var vm = new CreateSampleViewModel();

				var project = PatService.GetProject( id );
				if ( project != null )
				{
					vm.ProjectId = id;
					vm.ProjectName = project.ProjectName;
					vm.ContractMonitoringOrContractSiteVisitProject = PatService.ProjectIsContractMonitoringOrContractSiteVisit( id );
					vm.SampleStartDate = DateTime.Now.Date;
					vm.SampleDueDate = DateTime.Now.AddDays( 4 * 7 ).Date;
					vm.SampleMessage = PatService.GetSampleMessage( HttpContext.Session.SessionID );
					vm.IsAdministrator = loggedInUser.IsAdministrator();

					vm.Criteria = new SampleCriteria
						{
							MaxSampleSize = 20,
							RequestingUser = loggedInUser.FullName
						};

					vm.SessionKey = HttpContext.Session.SessionID;

					if ( HttpContext.Session != null )
					{
						var sessionKey = HttpContext.Session.SessionID;
						AppHelper.SetSessionProjectId( Session, id );
						vm.SessionKey = sessionKey;
					}
				}
				return View( vm );
			}

			return View( new CreateSampleViewModel { SampleMessage = "No Session available" } );
		}

		[HttpPost] //  helps prevent scripting attacks or data exposure from direct JSON queries
		public JsonResult LookupClaimType( string searchText, string esaCode, int maxResults )
		{
			var repository = PatService.GetAdwRepository();
			var codeToSearch = searchText.GetCode();
			var result = repository.LookupClaimType( codeToSearch, esaCode, maxResults );
			if ( result == null || result.Count < 1 )
			{
				result = new List<SelectListItem>
					{
						new SelectListItem {Value = codeToSearch, Text = "Data not found, please try again !"}
					};
			}
			return Json( result );
		}

		[HttpPost]
		public ActionResult Create( CreateSampleViewModel vm )
		{
			try
			{
				CustomCriteraValidation( vm );

				if ( ModelState.IsValid )
				{
					// get data from MF
					var claimSample = PatService.ExtractClaims( vm.Criteria );
					//  put data into temporary store (session or even database)
					if ( claimSample.Claims == null )
					{
						if ( string.IsNullOrEmpty( claimSample.ErrorMessage ) )
							TempData[ CommonConstants.FlashMessageTypeWarning ] = "No Claims found with these criteria";
						else
							TempData[ CommonConstants.FlashMessageTypeWarning ] = claimSample.ErrorMessage;
					}
					else
					{
						if ( claimSample.Claims.Count > 0 )
						{
							PatService.SaveExtract( vm.SessionKey, claimSample.Claims, HttpContext.User.Identity.Name.RemoveDomain() );
							return RedirectToAction( "ClaimList", new { sessionKey = vm.SessionKey, id = vm.ProjectId, due = vm.SampleDueDate } );
						}
						TempData[ CommonConstants.FlashMessageTypeWarning ] = "No Claims found with these criteria";
					}
				}

				//  Invalid model state redisplay with errors
				return View( vm );
			}
			catch ( Exception ex )
			{
				ErrorLog.GetDefault( null ).Log( new Error( ex ) );
				return View( "Error", new HandleErrorInfo( ex, "Sample", "Create" ) );
			}
		}

		private void CustomCriteraValidation( CreateSampleViewModel vm )
		{
			vm.SampleStartDate = DateTime.Now.Date;
			if ( vm.Criteria.Organisation != null && vm.Criteria.Organisation.Length > 3 )
				vm.Criteria.OrgCode = vm.Criteria.Organisation.Substring( 0, 4 );
			else
			{
				if ( !string.IsNullOrEmpty( vm.Criteria.Organisation ) )
					ModelState.AddModelError( "Organisation", "Invalid Organisation." );
			}

			if ( vm.Criteria.Esa != null && vm.Criteria.Esa.Length > 3 )
				vm.Criteria.EsaCode = vm.Criteria.Esa.Substring( 0, 4 );
			else
			{
				if ( !string.IsNullOrEmpty( vm.Criteria.Esa ) )
					ModelState.AddModelError( "Esa", "Invalid ESA." );
			}

			if ( vm.Criteria.ClaimTypeDescription != null && vm.Criteria.ClaimTypeDescription.Length > 3 )
				vm.Criteria.ClaimType = vm.Criteria.ClaimTypeDescription.Substring( 0, 4 );
			else
			{
				if ( !string.IsNullOrEmpty( vm.Criteria.ClaimTypeDescription ) )
					ModelState.AddModelError( "ClaimTypeDescription01", "Invalid Claim Type" );
			}

			if ( vm.Criteria.Site != null && vm.Criteria.Site.Length > 3 )
				vm.Criteria.SiteCode = vm.Criteria.Site.Substring( 0, 4 );

			if ( vm.ContractMonitoringOrContractSiteVisitProject )
			{
				if ( vm.SampleDueDate < vm.SampleStartDate )
					ModelState.AddModelError( "SampleDueDate", "Sample Due Date cannot be before Sample Start Date." );
			}

			if ( vm.Criteria.ToClaimDate < vm.Criteria.FromClaimDate )
				ModelState.AddModelError( "Criteria.ToClaimDate", "Claim To Date cannot be before Claim From Date." );

			if ( vm.Criteria.FromClaimDate == new DateTime( 1, 1, 1 ) )
				vm.Criteria.FromClaimDate = DateTime.Now.Date.Subtract( new TimeSpan( 365, 0, 0, 0 ) );
			if ( vm.Criteria.ToClaimDate == new DateTime( 1, 1, 1 ) )
				vm.Criteria.ToClaimDate = DateTime.Now.Date;

			if ( ( vm.Criteria.MaxSampleSize == null ) || vm.Criteria.MaxSampleSize < 1 )
			{
				vm.Criteria.MaxSampleSize = 20;
			}
			else
			{
				if ( vm.Criteria.MaxSampleSize > 50 )
					ModelState.AddModelError( "MaxSampleSize", "The field Number of Claims to Extract must be a maximum of 50." );
			}
		}

		[HttpGet]
		public ActionResult ClaimList( string sessionKey, int id, DateTime due )
		{
			var project = PatService.GetProject( id );
			var vm = new ClaimListViewModel { SessionKey = sessionKey, ProjectId = id, ProjectName = project.ProjectName, DueDate = due };
			//  display it for selection and or submission
			//  generate a scrollable grid
			ViewData[ "grid" ] = GenerateSampleGrid( sessionKey );
			ViewBag.Selections = PatService.GetSampleSelections( sessionKey );
			return View( vm );
		}

		private Grid GenerateSampleGrid( string sessionKey )
		{
			var totRecs = PatService.GetSample( sessionKey ).Count;
			var g = new Grid( "sample" );
			g.AddColumn( new Column( "rowNumber" ).SetLabel( "#" ).SetWidth( 50 ).SetFixedWidth( true ).SetAlign( Align.Right ).SetSortable( false ) );
			g.AddColumn( new Column( "ClaimId" ).SetLabel( CommonConstants.ReviewColumnClaimId ).SetWidth( 70 ).SetFixedWidth( true ).SetSortable( false ) );
			g.AddColumn( new Column( "JobseekerId" ).SetLabel( CommonConstants.ReviewColumnJobseekerId ).SetWidth( 70 ).SetFixedWidth( true ).SetAlign( Align.Center ).SetSortable( false ) );
			g.AddColumn( new Column( "Amount" ).SetLabel( CommonConstants.ReviewColumnClaimAmount ).SetWidth( 60 ).SetAlign( Align.Right ).SetFixedWidth( true ).SetSortable( false ) );
			g.AddColumn( new Column( "ManualSpecialClaimFlag" ).SetLabel( CommonConstants.ReviewColumnManualSpecialClaim ).SetWidth( 80 ).SetFixedWidth( true ).SetAlign( Align.Center ).SetSortable( false ) );
			g.AddColumn( new Column( "AutoSpecialClaimFlag" ).SetLabel( CommonConstants.ReviewColumnAutoSpecialClaim ).SetWidth( 80 ).SetFixedWidth( true ).SetAlign( Align.Center ).SetSortable( false ) );
			g.AddColumn( new Column( "ClaimStatusDesc" ).SetLabel( "Claim Status" ).SetWidth( 80 ).SetFixedWidth( true ).SetAlign( Align.Center ).SetSortable( false ) );
#if DEBUG
			//g.AddColumn( new Column( "ClaimType" ).SetLabel( "Type" ).SetWidth( 70 ).SetFixedWidth( true ).SetAlign( Align.Center ) );
			//g.AddColumn( new Column( "CreationDate" ).SetLabel( "Date" ).SetWidth( 60 ).SetFixedWidth( true ).SetAlign( Align.Right ) );

			//g.AddColumn( new Column( "OrgCode" ).SetLabel( "OrgCode" ).SetWidth( 70 ).SetFixedWidth( true ).SetAlign( Align.Center ) );
			//g.AddColumn( new Column( "OrgDesc" ).SetLabel( "Org" ).SetWidth( 150 ).SetFixedWidth( true ) );
			//g.AddColumn( new Column( "EsaCode" ).SetLabel( "EsaCode" ).SetWidth( 70 ).SetFixedWidth( true ).SetAlign( Align.Center ) );
			//g.AddColumn( new Column( "SiteCode" ).SetLabel( "SiteCode" ).SetWidth( 70 ).SetFixedWidth( true ).SetAlign( Align.Center ) );
			//g.AddColumn( new Column( "SiteDesc" ).SetLabel( "Site" ).SetWidth( 210 ).SetFixedWidth( true ) );
#endif
			g.SetHeight( CommonConstants.GridStandardHeight );
			g.SetScroll( true );
			//g.SetAutoWidth( true ); //  will force the grid to fit the width of the parent element
			g.SetWidth( CommonConstants.GridStandardWidth );
			g.SetMultiSelect( true );
			g.SetUrl( VirtualPathUtility.ToAbsolute( "~/Sample/GridDataSample/" ) );
			g.SetRowNum( totRecs ); //  Sets the number of rows displayed initially
			g.OnSelectAll( "pat.sampleClaimList_RowSelected()" );
			g.OnSelectRow( "pat.sampleClaimList_RowSelected()" );
			g.OnLoadComplete( "pat.sampleClaimList_LoadEvent();" );
			return g;
		}

		public ActionResult GridDataSample( GridSettings gridSettings )
		{
			if ( HttpContext.Session != null )
			{
				var sessionKey = HttpContext.Session.SessionID;
				var sampleList = PatService.GetSample( sessionKey );
				var totRecords = sampleList.Count;
				var rowNumber = 0;
				var jsonData = new
				{
					total = 1,
					page = gridSettings.PageIndex,
					records = totRecords,
					rows = (
								 from s in sampleList.AsEnumerable()
								 select new
								 {
									 id = s.Id,
									 cell = new List<String>
										       {
													 AppHelper.FormatInteger( ++rowNumber ),
											       AppHelper.FormatLong( s.ClaimId ),
													 AppHelper.FormatLong( s.JobseekerId ),
													 AppHelper.NullableDollarAmount( s.ClaimAmount ),
													 AppHelper.FlagOut( s.ManSpecialClaimFlag ),
													 AppHelper.FlagOut( s.AutoSpecialClaimFlag ),
													 s.StatusCodeDescription,
#if DEBUG
											       s.ClaimType,
													 AppHelper.ShortDate(  s.ClaimCreationDate ),
													 s.OrgCode,
													 s.OrgDescription,
													 s.EsaCode,
													 s.SiteCode,
													 s.SiteDescription
#endif
										       }
								 } ).ToArray()
				};
				return Json( jsonData, JsonRequestBehavior.AllowGet );
			}
			return Json( new { error = "No HttpContext" } );
		}

		[HttpPost]
		public ActionResult ClaimList( int projectId, string button, ClaimListViewModel vm, FormCollection input )
		{
			var uploadId = AppHelper.GetSessionUploadId( Session );
			return button == "more" ? RedirectToAction( "Create", "Sample", new { id = projectId } )
				: RedirectToAction( "Details", "Upload", new { id = uploadId } );
		}

		[HttpPost]
		public ActionResult SaveSample()
		{
			var msg = String.Empty;

			var projectId = AppHelper.GetSessionProjectId( Session );
			var loggedInUser = new PatUser( Request.LogonUserIdentity );
			var isAdditional = Request.Form[ "additional" ].Equals( "true" );
			var isOutOfScope = Request.Form[ "outOfScope" ].Equals( "true" );
			var dueDate = DateTime.Parse( Request.Form[ "due" ] );

			if ( HttpContext.Session != null )
			{
				var sessionKey = HttpContext.Session.SessionID;

				if ( Request.Form[ "ids[]" ] != null )
				{
					var intArr = Array.ConvertAll( Request.Form[ "ids[]" ].Split( ',' ), Convert.ToInt32 );
					if ( intArr.Length > 0 )
					{
						Session[ "ids" ] = intArr;
						var uploadId = PatService.SaveSample( projectId, sessionKey, intArr.ToList(), isOutOfScope, isAdditional,
																			dueDate,
																			loggedInUser.LoginName );
						AppHelper.SetSessionUploadId( Session, uploadId );
						msg = "Sample has been successfully created.";
						TempData[ CommonConstants.FlashMessageTypeInfo ] = msg;
					}
					else
					{
						msg = "Sample not created. No selections";
						TempData[ CommonConstants.FlashMessageTypeInfo ] = msg;
					}
				}
			}

			return Json( new { success = true, message = msg }, JsonRequestBehavior.AllowGet );
		}

		[HttpPost]
		public ActionResult AddMore()
		{
			var msg = String.Empty;

			var projectId = AppHelper.GetSessionProjectId( Session );
			var loggedInUser = new PatUser( Request.LogonUserIdentity );

			if ( HttpContext.Session != null )
			{
				var sessionKey = HttpContext.Session.SessionID;

				if ( Request.Form[ "ids[]" ] == null )
				{
					//  User has deselected all of the Claims
					var emptyList = new List<int>();
					Session[ "ids" ] = emptyList.ToArray();
					PatService.SaveSampleSelections( projectId, sessionKey, emptyList, loggedInUser.LoginName );
				}
				else
				{
					var intArr = Array.ConvertAll( Request.Form[ "ids[]" ].Split( ',' ), Convert.ToInt32 );
					Session[ "ids" ] = intArr;
					PatService.SaveSampleSelections( projectId, sessionKey, intArr.ToList(), loggedInUser.LoginName );
				}
			}
			return Json( new { success = true, message = msg }, JsonRequestBehavior.AllowGet );
		}
	}
}