using Elmah;
using MvcJqGrid;
using MvcJqGrid.Enums;
using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Infrastructure.Controllers;
using ProgramAssuranceTool.Infrastructure.Security;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Models;
using ProgramAssuranceTool.ViewModels.Home;
using ProgramAssuranceTool.ViewModels.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ApplicationException = System.ApplicationException;
using GridSettings = MvcJqGrid.GridSettings;

namespace ProgramAssuranceTool.Controllers
{
	[CustomAuthorize]
	public class HomeController : InfrastructureController
	{
		public HomeController( IControllerDependencies controllerDependencies )
			: base( controllerDependencies )
		{
		}

		private bool CanEdit
		{
			get { return AppHelper.IsAdministrator( User.Identity ) || DebugHelper.IsTemporaryAdmin; }
		}

		#region Bulletin List

		/// <summary>
		/// To display the home page screen e.g. the Bulletin list.
		/// </summary>
		/// <param name="bulletinType">Type of the bulletin.e.g. STD or FAQ</param>
		/// <returns></returns>
		public ActionResult Index( string bulletinType = DataConstants.StandardBulletinType )
		{
			ViewData["BulletinType"] = bulletinType;
			ViewBag.CanEdit = CanEdit;
			return View();
		}

		/// <summary>
		/// To load the bulletin and return it as a JSON data
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="bulletinType">Type of the bulletin.e.g. STD or FAQ</param>
		/// <returns></returns>
		public ActionResult GridDataBulletin( GridSettings gridSettings,
		                                      string bulletinType = DataConstants.StandardBulletinType )
		{
			var bulletinList = PatService.GetBulletins( gridSettings, bulletinType, CanEdit );
			var totalBulletin = PatService.CountBulletins( gridSettings, bulletinType, CanEdit );

			var jsonData = new
				{
					total = AppHelper.PagesInTotal( totalBulletin, gridSettings.PageSize ),
					page = gridSettings.PageIndex,
					records = totalBulletin,
					rows = (
						       from e in bulletinList.AsEnumerable()
						       select new
							       {
								       id = e.BulletinId,
								       cell = new List<string>
									       {
										       AppHelper.FormatInteger( e.BulletinId ),
										       AppHelper.ShortDate( e.EndDate ),
										       BulletinViewLink( e )
									       }
							       }
					       ).ToArray()
				};

			return Json( jsonData, JsonRequestBehavior.AllowGet );
		}

		private string BulletinViewLink(Bulletin bulletin)
		{
			// only admin can edit the bulletin 
			if (AppHelper.IsAdministrator(User.Identity) || DebugHelper.TrueOnEvenNumber(bulletin.BulletinId))
			{
				return string.Format("<a href='/Bulletin/Edit/{0}'><span class='wrap'>{1}</span></a>", bulletin.BulletinId, bulletin.BulletinTitle);
			}

			// normal user can view the bulletin
			return string.Format("<a href='/Bulletin/View/{0}'><span class='wrap'>{1}</span></a>", bulletin.BulletinId, bulletin.BulletinTitle);
		}

		private string BulletinDetailLink( Bulletin bulletin )
		{
			// only admin can edit the bulletin 
			if (AppHelper.IsAdministrator( User.Identity ) || DebugHelper.TrueOnEvenNumber( bulletin.BulletinId ))
			{
				return string.Format( "<a href='/Bulletin/Edit?Id={0}'><span class='wrap'>{1}</span></a>", bulletin.BulletinId, bulletin.BulletinTitle );
			}

			// normal user can view the detail bulletin
			return string.Format( "<a href='/Bulletin/Details?id={0}&bulletinType={1}#{0}'><span class='wrap'>{2}</span></a>", bulletin.BulletinId,
			                      bulletin.BulletinType, bulletin.BulletinTitle );
		}

		#endregion

		#region  Activity List

		//http://localhost:6491/Home/RecentActivity
		public ActionResult RecentActivity()
		{
			AppHelper.SetSessionQueryFromDate( Session, DateTime.Now.Subtract( new TimeSpan( 1, 0, 0, 0 ) ).ToShortDateString() );
			AppHelper.SetSessionQueryToDate(   Session, DateTime.Now.Add(      new TimeSpan( 1, 0, 0, 0 ) ).ToShortDateString() );

			ViewData[ "grid" ] = GenerateActivityGrid();

			return View();
		}

		public ActionResult ActivityList( string userId, string from, string to )
		{
			AppHelper.SetSessionQueryUserId( Session, userId );
			AppHelper.SetSessionQueryFromDate( Session, from );
			AppHelper.SetSessionQueryToDate( Session, to );

			ViewData[ "grid" ] = GenerateActivityGrid();

			return View();
		}

		private Grid GenerateActivityGrid()
		{
			DateTime to;
			DateTime @from;
			var userId = GetQueryParas( out to, out @from );

			var totActivities = PatService.GetActivities( userId, from, to ).Count;

			var g = new Grid( "activity" );
			g.AddColumn( new Column( "rowNo" ).SetLabel( "#" ).SetWidth( 60 ).SetAlign( Align.Right ) );
			g.AddColumn( new Column( "ActivityId" ).SetLabel( "Activity Id" ).SetWidth( 60 ).SetAlign( Align.Center ) );
			g.AddColumn( new Column( "UserId" ).SetLabel( "UserId" ).SetWidth( 60 ).SetAlign( Align.Center ) );
			g.AddColumn( new Column( "Activity" ).SetLabel( "Activity" ).SetWidth( 200 ) );

			g.AddColumn( new Column( "CreatedOn" ).SetLabel( "Date" ).SetWidth( 80 ) );
			g.SetUrl( VirtualPathUtility.ToAbsolute( "~/Home/GridDataActivity" ) );

			g.SetHeight( CommonConstants.GridStandardHeight ); //  set this tall enough and you wont see the scroll bars
			g.SetWidth( CommonConstants.GridStandardWidth );
			g.SetAutoWidth( true ); //  will force the grid to fit the width of the parent element
			g.SetRowNum( totActivities ); //  Sets the number of rows displayed initially
			g.SetViewRecords( true ); //  isplays the total number of rows in the dataset
			return g;
		}

		private string GetQueryParas( out DateTime to, out DateTime @from )
		{
			var userId = AppHelper.GetSessionQueryUserId( Session );
			var strFrom = AppHelper.GetSessionQueryFromDate( Session );
			@from = new DateTime( 1, 1, 1 );
			if (!string.IsNullOrEmpty( strFrom )) from = DateTime.Parse( strFrom );
			var strTo = AppHelper.GetSessionQueryToDate( Session );
			to = new DateTime( 1, 1, 1 );
			if (!string.IsNullOrEmpty( strTo )) to = DateTime.Parse( strTo );
			return userId;
		}

		private const int MaxActivitiesToDisplay = 500;  //  too many will cause an error

		public ActionResult GridDataActivity( Models.GridSettings gridSettings )
		{
			DateTime to;
			DateTime @from;
			var userId = GetQueryParas( out to, out @from );

			var activityList = PatService.GetActivities( userId, from, to );

			var totActivities = activityList.Count;
			if (totActivities > MaxActivitiesToDisplay)
				totActivities = MaxActivitiesToDisplay;

			var r = 0;

			var jsonData = new
				{
					total = AppHelper.PagesInTotal( totActivities, gridSettings.PageSize ),
					page = gridSettings.PageIndex,
					records = totActivities,
					rows = (
						       from activity in activityList.AsEnumerable()
						       select new
							       {
								       id = activity.ActivityId,
								       cell = new List<String>
									       {
												 AppHelper.FormatInteger( ++r  ),
										       AppHelper.FormatInteger( activity.ActivityId ),
										       activity.UserId,
										       activity.Activity,
										       AppHelper.LongDateTime( activity.CreatedOn )
									       }
							       } ).Take(totActivities).ToArray()
				};

			return Json( jsonData, JsonRequestBehavior.AllowGet );
		}

		#endregion

		#region  About

		// http://localhost:6491/Home/About
		public ActionResult About()
		{
			var vm = new AboutViewModel {Version = AppHelper.VersionNumber()};
#if DEBUG
			vm.IsDebug = true;
			var loggedInUser = new PatUser( Request.LogonUserIdentity );
			var list = PatService.GetProgramAssuranceUsers();
			var groups = loggedInUser.MemberOf;
			ViewData[ "admins" ] = UserTable( list );
			ViewData[ "groups" ] = AppHelper.ListFor( groups );
			var control = PatService.GetControlFile();
			ViewData[ "about" ] = GenerateAboutData( control );
			ViewBag.IisVersion = ServerSoftware;
#endif
			return View(vm);
		}

		// http://localhost:6491/Home/Users
		public ActionResult Users()
		{
			var vm = new AboutViewModel { Version = AppHelper.VersionNumber() };
			vm.IsDebug = true;
			var loggedInUser = new PatUser( Request.LogonUserIdentity );
			var list = PatService.GetProgramAssuranceUsers();
			var groups = loggedInUser.MemberOf;
			ViewData[ "admins" ] = UserTable( list );
			ViewData[ "groups" ] = AppHelper.ListFor( groups );
			var control = PatService.GetControlFile();
			ViewData[ "about" ] = GenerateAboutData( control );
			return View( vm );
		}

		private static string UserTable( IEnumerable<PatUser> users )
		{
			var sb = new StringBuilder();
			sb.Append( "<div class='table-responsive'>" );
			sb.Append( "<table caption='User Table' class='table table-striped table-bordered table-hover table-condensed'>" );
			sb.Append( string.Format( "<tr><td><b>{0}</b></td><td><b>{1}</b></td><td><b>{2}</b></td><td><b>{3}</b></td></tr>", 
				"Full Name", "UserId", "Special", "Resource Groups" ) );
			foreach ( var u in users )
			{
				sb.Append( string.Format( 
					"<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>", 
					u.FullName, u.LoginName, u.IsAdminString( u.IsAdmin() ), u.ResourceSet() ) );
			}
			sb.Append( "</table>" );
			sb.Append( "</div>" );
			return sb.ToString();
		}

		private static string GenerateAboutData( PatControl control )
		{
			var label = new[] {
				"Last Batch Run",
				"Last Compliance Run",
				"Project Completion",
				"Compliance Indicator",
				"Projects",
				"Uploads/Samples",
				"Review Count"
			};

			var data = new[] {
				AppHelper.ShortDateAndTime( control.LastBatchRun ),
				AppHelper.ShortDateAndTime( control.LastComplianceRun ),
				AppHelper.FormatPercentage( control.ProjectCompletion ),
				AppHelper.FormatDecimal( control.TotalComplianceIndicator ),
				AppHelper.FormatInteger( control.ProjectCount ),
				AppHelper.FormatInteger( control.SampleCount ),
				AppHelper.FormatInteger( control.ReviewCount )
			};

			var html = AppHelper.TableFromArray( label, data, "System Details", showEmpty:false );
			return html;
		}

		public ActionResult System()
		{
			return View();
		}

		public ActionResult ClearSession()
		{
			Session.Clear();
			TempData[ CommonConstants.FlashMessageTypeInfo ] = "Session Cleared";
			return RedirectToAction( "System", "Home" );
		}

		public ActionResult ForceException()
		{
			//throw new ApplicationException( "Testing 123" );
			ErrorLog.GetDefault( null ).Log( new Error( new ApplicationException( "Testing 1234" ) ) );
			return RedirectToAction( "Index" );
		}

		#endregion

#if DEBUG

		#region  Routine Routines

		//http://localhost:6491/Home/ForceBatch
		[HttpGet]
		public ActionResult ForceBatch()
		{
			var vm = new SystemViewModel { SystemName = "Programme Assurance Tool", CurrentTime = DateTime.Now };
			return View( vm );
		}

		[HttpPost]
		public ActionResult ForceBatch( SystemViewModel vm )
		{
			try
			{
				if ( BatchJobs.DoJob( "PatBatch", string.Format( "Forced by {0}", HttpContext.User.Identity.Name.RemoveDomain() ) ) )
					TempData[ CommonConstants.FlashMessageTypeInfo ] = "Batch processing complete";
				else
					TempData[ CommonConstants.FlashMessageTypeError ] = "Batch processing detected Integrity Errors";

				return RedirectToAction( "RecentActivity", "Home" );
			}
			catch ( Exception ex )
			{
				ErrorLog.GetDefault( null ).Log( new Error( ex ) );
				return View( "Error", new HandleErrorInfo( ex, "Home", "ForceBatch" ) );
			}
		}

		#endregion

#endif

		#region  Clear Log

		//http://localhost:6491/Home/ClearLog

		public ActionResult ClearLog()
		{
			var vm = new ClearLogViewModel
				{
					PriorToDate = DateTime.Now.Subtract( new TimeSpan( 30, 0, 0, 0 ) )
				};
			return View( vm );
		}

		[HttpPost]
		public ActionResult ClearLog( ClearLogViewModel vm )
		{
			try
			{
				if (ModelState.IsValid)
				{
					PatService.DeleteLogItemsPriorTo( vm.PriorToDate );

					TempData[ CommonConstants.FlashMessageTypeInfo ] = "Log cleared";

					return RedirectToAction( "Index", "Home" );
				}
			}
			catch (Exception ex)
			{
				ErrorLog.GetDefault( null ).Log( new Error( ex ) );
				return View( "Error", new HandleErrorInfo( ex, "Home", "ClearLog" ) );
			}
			return View( vm );
		}

		#endregion

	}
}