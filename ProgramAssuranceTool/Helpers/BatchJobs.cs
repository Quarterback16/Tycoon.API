using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ProgramAssuranceTool.Infrastructure.Interfaces;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Models;
using ProgramAssuranceTool.Repositories;
using ProgramAssuranceTool.ViewModels.System;

namespace ProgramAssuranceTool.Helpers
{
	/// <summary>
	///   Batch processes for the PAT that run in the background
	///   Main tasks are the generation of Compliance indicators and
	///   the updating of Reviews based on questionnaire data.
	/// </summary>
	public static class BatchJobs
	{
		public const string BatchUser = "BATCH";
		public const string DiskDiagnostics = "DISKS";
		public const string ConfigDiagnostics = "CONFIG";

		public static bool DoJob( string taskName, string how )
		{
			var activityRepository = new UserActivityRepository<UserActivity>();
			var patService = new PatService();
			var control = patService.GetControlFile();

			StartJob( taskName, how, activityRepository );

			//  Get rid of any unused samples
			SampleCleanup( patService );  

			//  Refresh the Compliance indicators for PAM
			UpdateComplianceIndicators( patService, how, control );

			//  Get outcomes from Staton's spreadsheet
			BatchUpdateReviewOutcomes( patService, how, control );

			HealthCheck( patService, control.LastBatchRun, how );

			EndJob( taskName, activityRepository );

			control.LastBatchRun = DateTime.Now;
			control.UpdatedBy = BatchUser;
			patService.UpdateControl( control );

			return true;
		}

		/// <summary>
		///   Perform a Health check on the system.
		/// </summary>
		/// <param name="patService">The pat service.</param>
		/// <param name="lastBatchRun">The last batch run date and time.</param>
		/// <param name="how">How the job was invoked.</param>
		public static void HealthCheck( IPatService patService, DateTime lastBatchRun, string how )
		{
			var auditService = patService.GetAuditService();
			string why;
			if ( !ReadyToGenerateAgain( lastBatchRun, how, out why ) )
			{
				auditService.AuditActivity( string.Format( "Health Check skipped ({1}) for now: last Run  {0}", lastBatchRun, why ), DiskDiagnostics );
				return;
			}
			var stopwatch = new Stopwatch();
			stopwatch.Start();
			CheckDiskSpace( auditService );
			CheckAppSettings( auditService );

			var timeElapsed = AppHelper.StopTheWatch( stopwatch );
			auditService.AuditActivity( string.Format( "Health Check complete:  in {0}", timeElapsed ), DiskDiagnostics );
		}

		private static void CheckAppSettings( IAuditService auditService )
		{
			var appSettings = ConfigurationManager.AppSettings;
			const string fmt = "{0,-35} = {1}";
			for ( var i = 0; i < appSettings.Count; i++ )
			{
				auditService.AuditActivity( string.Format( fmt, appSettings.GetKey( i ), appSettings[ i ] ),
													 ConfigDiagnostics );
			}
			var connectionStrings = ConfigurationManager.ConnectionStrings;
			foreach ( ConnectionStringSettings connection in connectionStrings )
			{
				var connectionString = connection.ConnectionString;
				auditService.AuditActivity( string.Format( fmt, connection.Name, connectionString ), ConfigDiagnostics );
			}
		}

		private static void CheckDiskSpace( IAuditService auditService )
		{
			var vm = DiskSpaceCheck();
			auditService.AuditActivity( string.Format( "There are {0} disks connected", vm.Disks.Count ), DiskDiagnostics );
			foreach ( var disk in vm.Disks )
			{
				auditService.AuditActivity( disk.Name, DiskDiagnostics );
				if ( !disk.IsAvailable ) continue;
				auditService.AuditActivity( disk.DriveType, DiskDiagnostics );
				auditService.AuditActivity( disk.AvailableFreeSpace, DiskDiagnostics );
				auditService.AuditActivity( disk.SpaceUsed, DiskDiagnostics );
				auditService.AuditActivity( disk.PercentFreeSpace, DiskDiagnostics );
			}
		}

		/// <summary>
		///   Checks that there is enough disk space available for the system to run.
		/// </summary>
		/// <returns>A view of the disk space available.</returns>
		public static DiskSpaceViewModel DiskSpaceCheck()
		{
			var vm = new DiskSpaceViewModel { Disks = new List<DiskDiagnostic>() };

			var drives = DriveInfo.GetDrives();

			foreach ( var drive in drives )
			{
				var disk = new DiskDiagnostic { IsAvailable = true };
				try
				{
					double fspc = drive.TotalFreeSpace;
					double tspc = drive.TotalSize;
					double percent = ( fspc / tspc );
					var num = (float) percent;

					disk.DriveType = String.Format( "Type: {0}", drive.DriveType );
					disk.Name = String.Format( "{0} has {1:p} free", drive.Name, num );
					disk.AvailableFreeSpace = String.Format( "Space Remaining    : {0}", FormatBytes( drive.AvailableFreeSpace ) );
					disk.SpaceUsed = String.Format( "Space used         : {0}", FormatBytes( drive.TotalSize ) );
					disk.PercentFreeSpace = String.Format( "Percent Free Space : {0:p}", percent );
					vm.Disks.Add( disk );
				}
				catch ( Exception ex )
				{
					disk.Name = string.Format( "{0} - {1} ", drive.Name, ex.Message );
					disk.IsAvailable = false;
					vm.Disks.Add( disk );
				}
			}
			return vm;
		}

		private static string FormatBytes( long bytes )
		{
			string[] suffix = { "B", "KB", "MB", "GB", "TB" };
			var i = 0;
			double dblSByte = bytes;
			if ( bytes > 1024 )
				for ( i = 0; ( bytes / 1024 ) > 0; i++, bytes /= 1024 )
					dblSByte = bytes / 1024.0;
			return String.Format( "{0:0.##} {1}", dblSByte, suffix[ i ] );
		}

		private static void BatchUpdateReviewOutcomes( IPatService patService
																	  , string how
																	  , PatControl control )
		{
			var lastBatchRun = control.LastBatchRun;
			string auditMsg;

			string why;
			if ( ReadyToGenerateAgain( lastBatchRun, how, out why ) )
			{
				var stopwatch = new Stopwatch();
				stopwatch.Start();

				var vm = new BuroViewModel();
				var buro = new BuroProcess();
				vm = buro.Execute( vm
										 , patService.GetUploadRepository()
										 , patService.GetQuestionaireRepository()
										 , patService.GetAuditService()
										 , patService.GetReviewRepository()
					);

				control.LastBatchRun = DateTime.Now;
				control.UpdatedBy = BatchUser;

				var timeElapsed = AppHelper.StopTheWatch( stopwatch );

				auditMsg = string.Format( "BURO complete: {0} Reviews checked; {1} updated {3} errors in {2}",
												  vm.ReviewsRead, vm.ReviewsUpdated, timeElapsed, vm.ValidationErrors );
			}
			else
				auditMsg = string.Format( "BURO skipped for now ({1}): last Run  {0}", lastBatchRun, why );

			patService.SaveActivity( CreateAuditRecord( auditMsg, "BURO" ) );
		}

		private static void UpdateComplianceIndicators( IPatService patService, string how, PatControl control )
		{
			var lastComplianceRun = control.LastComplianceRun;
			string auditMsg;
			string why;
			if ( ReadyToGenerateAgain( lastComplianceRun, how, out why ) )
			{
				var stopwatch = new Stopwatch();
				stopwatch.Start();

				var ciViewModel = new ComplianceIndicatorsViewModel();
				//  Options currently required by PAM
				var cigen = new ComplianceIndicators();
				ciViewModel = cigen.Generate( ciViewModel );

				control.LastComplianceRun = DateTime.Now;
				control.UpdatedBy = BatchUser;
				control.ReviewCount = ciViewModel.ReviewsRead;
				control.ProjectCount = ciViewModel.ProjectsRead;
				control.ProjectCompletion = AppHelper.Percent( ciViewModel.CompletedProjects, ciViewModel.ProjectsRead );
				control.SampleCount = ciViewModel.SamplesRead;
				control.TotalComplianceIndicator = AppHelper.Average( ciViewModel.TotalCompliancePoints, ciViewModel.ReviewsRead );

				var timeElapsed = AppHelper.StopTheWatch( stopwatch );

				auditMsg = string.Format( "CI Generation complete:  {0} indicators generated in {1}",
												  ciViewModel.IndicatorsGenerated, timeElapsed );
			}
			else
				auditMsg = string.Format( "CI Generation skipped ({1}) for now: last Run  {0}", lastComplianceRun, why );

			patService.SaveActivity( CreateAuditRecord( auditMsg, "CIGEN" ) );
		}

		private static void SampleCleanup( IPatService patService )
		{
			var deleteCount = 0;
			var sessionList = patService.GetDistinctSessionKeys();
			foreach ( var session in from session in sessionList
											 let list = patService.GetSample( sessionKey: session )
											 let rec1 = list.FirstOrDefault()
											 where rec1 != null
											 where rec1.IsExpired()
											 select session )
			{
				patService.DeleteSession( session, BatchUser );
				deleteCount++;
			}
			var msg = string.Format( "{0} sessions deleted", deleteCount );
			patService.SaveActivity( msg, BatchUser );
		}

		private static void EndJob( string taskName, UserActivityRepository<UserActivity> activityRepository )
		{
			var userActivity = CreateAuditRecord(
				string.Format( "{0} completed {1:r}", taskName, DateTime.Now ), BatchUser );
			activityRepository.Add( userActivity );
		}

		private static void StartJob( string taskName, string how, UserActivityRepository<UserActivity> activityRepository )
		{
			var userActivity = CreateAuditRecord(
				string.Format( "{0} invoked {1:r} {2}", taskName, DateTime.Now, how ), how );
			activityRepository.Add( userActivity );
		}

		private static UserActivity CreateAuditRecord( string message, string userCode )
		{
			var userActivity = new UserActivity
				{
					UserId = userCode,
					Activity = message,
					CreatedBy = "SYS000"
				};
			return userActivity;
		}

		private static bool ReadyToGenerateAgain( DateTime lastComplianceRun, string how, out string why )
		{
			why = string.Empty;

			if ( !string.IsNullOrEmpty( how ) )
				if ( how.Length > 5 )
					if ( how.Substring( 0, 6 ).ToUpper().Equals( "FORCED" ) )
					{
						why = how;
						return true;
					}

			var aDayHasPassed = DateTime.Now > lastComplianceRun.Add( new TimeSpan( 1, 0, 0, 0 ) );
			if ( !aDayHasPassed ) why += "day not passed";

			var isQuiet = IsItQuietTime( DateTime.Now );
			if ( !isQuiet ) why += " not b4 9am or aftr 5pm";

			return ( aDayHasPassed && isQuiet );
		}

		public static bool IsItQuietTime( DateTime theDate )
		{
			var hour = theDate.TimeOfDay.Hours;
			return ( hour < 9 ) || ( hour > 16 );
		}
	}
}