using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Models;
using ProgramAssuranceTool;

namespace Tests.Integration
{
	[TestClass]
	public class UserRepositoryTests
	{
		[TestMethod]
		public void TestCoordinatorsDropdown()
		{
			var sut = new PatService();
			var items = sut.GetProgramAssuranceDropdownList();

			Assert.IsNotNull( items );
			AppHelper.Announce( string.Format( "Environment : {0}", AppHelper.Environment() ) );
			var i = 0;
			foreach ( var item in items )
			{
				i++;
				AppHelper.Announce( string.Format( "{0,3}:{1,-25}:{2}", i, item.Text, item.Value ) );
			}
		}

		[TestMethod]
		public void TestGetProgramAssuranceUsers()
		{
			var sut = new PatService();
			var items = sut.GetProgramAssuranceUsers();

			Assert.IsNotNull( items );
			AppHelper.Announce( string.Format( "Environment : {0}", AppHelper.Environment() ) );
			var i = 0;
			foreach ( var item in items )
			{
				i++;
				AppHelper.Announce( string.Format( "{0,3}:{1}:{2}:{3}",
					i, item.LoginName, item.FullName, ( item.IsAdmin() ? "ADMIN" : string.Empty ) ) );
			}
		}

		[TestMethod]
		public void TestGetProgramAssuranceGroups()
		{
			var sut = new PatService();
			var items = sut.GetProgramAssuranceGroups();

			Assert.IsNotNull( items );
			AppHelper.Announce( string.Format( "Environment : {0}", AppHelper.Environment() ) );
			var i = 0;
			foreach ( var item in items )
			{
				i++;
				AppHelper.Announce( string.Format( "{0,3}:{1}", i, item ) );
			}
		}

		[TestMethod]
		public void TestGetProgramAssuranceAdministrators()
		{
			var sut = new PatService();
			var items = sut.GetProgramAssuranceAdminUsers();

			Assert.IsNotNull( items );
			AppHelper.Announce( string.Format( "Environment : {0}", AppHelper.Environment() ) );
			var i = 0;
			foreach ( var item in items )
			{
				i++;
				AppHelper.Announce( string.Format( "{0,3}:{1}:{2}", i, item.LoginName, item.FullName ) );
			}
		}

		[TestMethod]
		public void TestGettingAPatUserFromAUserId()
		{
			var sut = new PatUser( "SC0779" );
			sut.DumpUser();
			Assert.AreEqual( "Stephen Colonna", sut.FullName );
			Assert.AreEqual( "SC0779", sut.LoginName );
			Assert.AreEqual( "Stephen.Colonna@employment.gov.au", sut.EmailAddress ); //  emails arent stored in DEV active directory
			Assert.IsTrue( sut.IsQld() );
			Assert.IsTrue( sut.InState( "QLD" ) );
			Assert.IsTrue( sut.IsAdmin() );
		}

		[TestMethod]
		public void TestGettingAPatUserFromAUserId2()
		{
			var sut = new PatUser( "PD2505" );
			sut.DumpUser();
			Assert.IsFalse( sut.IsAdmin() );
		}

		[TestMethod]
		public void TestGettingAPatUserFromAUserIdManchi()
		{
			var sut = new PatUser( "MS3087" );
			sut.DumpUser();
			Assert.IsFalse( sut.IsAdmin() );
		}

		[TestMethod]
		public void TestGettingAPatUserFromAUserId3()
		{
			var sut = new PatUser( "MP2648" );
			sut.DumpUser();
			Assert.IsTrue( sut.IsAdmin() );
		}

		[TestMethod]
		public void TestGroupMembership()
		{
			var sut = new PatUser( "SC0779" );
			sut.DumpUser();
			Assert.IsTrue( sut.IsInAnyOfTheseGroups( "QLD" ) );
			Assert.IsFalse( sut.IsInAnyOfTheseGroups( "TAS,WA" ) );
			Assert.IsTrue( sut.IsAdmin() );
		}

		[TestMethod]
		public void TestAdminStatusOfPhillipDimond()
		{
			var sut = new PatUser( "PD2505" );
			sut.DumpUser();
			Assert.IsTrue( sut.IsAdmin() );
		}

	}
}
