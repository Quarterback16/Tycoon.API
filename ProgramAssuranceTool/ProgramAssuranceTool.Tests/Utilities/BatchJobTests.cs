using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Infrastructure.Wrappers;
using ProgramAssuranceTool.Models;
using ProgramAssuranceTool.ViewModels.System;

namespace ProgramAssuranceTool.Tests.Utilities
{
	[TestClass]
	public class BatchJobTests
	{
		[TestMethod]
		public void TestQuietTimeAm()
		{
			var isQuiet = BatchJobs.IsItQuietTime( new DateTime( 2014, 4, 30, 8, 30, 0, 0 ) );
			Assert.IsTrue( isQuiet );
		}

		[TestMethod]
		public void TestQuietTimePm()
		{
			var isQuiet = BatchJobs.IsItQuietTime( new DateTime( 2014, 4, 30, 17, 00, 0, 0 ) );
			Assert.IsTrue( isQuiet );
		}

		[TestMethod]
		public void TestDiskSpace()
		{
			var vm = BatchJobs.DiskSpaceCheck();
			foreach ( var disk in vm.Disks )
			{
				Console.WriteLine( "Name:    {0}",disk.Name);
				if (disk.IsAvailable)
				{
					Console.WriteLine( "Type:    {0}", disk.DriveType );
					Console.WriteLine( "Avail:   {0}", disk.AvailableFreeSpace );
					Console.WriteLine( "Used:    {0}", disk.SpaceUsed );
					Console.WriteLine( "% free:  {0}", disk.PercentFreeSpace );
				}
				Console.WriteLine( "---------------------------------------" );	
			}
			Assert.IsTrue( vm.Disks.Count > 0 );
		}

		[TestMethod]
		public void TestConfigurationManagerWrapper()
		{
			var sut = new ConfigurationManagerWrapper();
			sut.GetSection<AppSettingsSection>( "AppSettings" );
			var appSettings = ConfigurationManager.AppSettings;
			const string fmt = "{0,-35} = {1}";
			for ( var i = 0; i < appSettings.Count; i++ )
			{
				Console.WriteLine( fmt, appSettings.GetKey( i ), appSettings[ i ] );
			}
			Assert.IsTrue( appSettings.Count > 0 );
			var connectionStrings = ConfigurationManager.ConnectionStrings;
			foreach ( ConnectionStringSettings connection in connectionStrings )
			{
				var connectionString = connection.ConnectionString;
				Console.WriteLine( fmt, connection.Name, connectionString  );				
			}
			Assert.IsTrue( connectionStrings.Count > 0 );
		}

	}
}
