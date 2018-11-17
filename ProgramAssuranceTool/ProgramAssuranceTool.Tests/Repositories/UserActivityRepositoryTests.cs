using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProgramAssuranceTool.Models;
using ProgramAssuranceTool.Repositories;

namespace ProgramAssuranceTool.Tests.Repositories
{
	[TestClass]
	public class UserActivityRepositoryTests
	{
		[TestMethod]
		public void TestUserActivityStringOverflow()
		{
			var _activityRepository = new UserActivityRepository<UserActivity>();
			var userActivity = new UserActivity
			{
				UserId = "1234567890-the rest is superflus",
				Activity = "123456789012345678901234567890123456789012345678901234567890-superfluous",
				CreatedBy = "UnitTest"
			};
			_activityRepository.Add( userActivity );
			Assert.IsTrue( true );
		}

		[TestMethod]
		public void TestUserRecentActivity()
		{
			var sut = new PatService();
			var activityList = sut.GetActivities( string.Empty, DateTime.Now.Subtract( new TimeSpan( 4, 0, 0, 0 ) ), DateTime.Now );
			Assert.IsTrue( activityList.Count > 0 );
		}

	}
}
