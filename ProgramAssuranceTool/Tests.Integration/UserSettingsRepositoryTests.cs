using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProgramAssuranceTool;
using ProgramAssuranceTool.Models;
using ProgramAssuranceTool.ViewModels;

namespace Tests.Integration
{
	[TestClass]
	public class UserSettingsRepositoryTests
	{
		[TestMethod]
		public void TestUserSettingsInsert()
		{
			//  CREATE
			var sut = new PatService();
			const string name = "UnitTest-PageSize";
			const string serialiseAs = "numeric";
			const string value = "20";
			const string userId = "SC0779";
			var userSetting = new UserSetting
			{
				Name = name,
				SerialiseAs = serialiseAs,
				Value = value,
				UserId = userId,
				CreatedBy = "UnitTest"
			};
			sut.UserSettingInsert( userSetting );
			Assert.IsTrue( userSetting.Id > 0 );
		}

		[TestMethod]
		public void TestUserSettingsGetAll()
		{
			var sut = new PatService();
			var results = sut.GetSettingsFor( "SC0779" );
			Assert.IsTrue( results.Count > 0 );
		}

		[TestMethod]
		public void TestGridCustomisationsGetAll()
		{
			var sut = new PatService();
			var vm = new CustomiseReviewGridViewModel { UserId = "SC0779" };
			vm = sut.GetGridCustomisations( vm );
			Assert.IsTrue( vm.Col01 == "Job Seeker ID" );
		}

		[TestMethod]
		public void TestGridCustomisationsSave()
		{
			var sut = new PatService();
			var vm = new CustomiseReviewGridViewModel
				{
					UserId = "SC0779",
					Col01 = "Job Seeker ID",
					Col02 = "Claim ID",
					Col03 = "Claim Amount",
					PageSize = "50",
					GridWidth = 2000,
					SortOrder = "Claim ID",
					AscOrDescending = "Ascending"
				};
			sut.SaveGridCustomisations( vm );
			Assert.IsTrue( vm.Col01 == "Job Seeker ID" );
		}

		[TestMethod]
		public void TestGridCustomisationsDelete()
		{
			var sut = new PatService();
			sut.UserSettingsDelete( "SC0779" );
			var vm = new CustomiseReviewGridViewModel { UserId = "SC0779" };
			vm = sut.GetGridCustomisations( vm );
			Assert.IsTrue( vm.ColumnCount() == 25 );
		}

	}
}
