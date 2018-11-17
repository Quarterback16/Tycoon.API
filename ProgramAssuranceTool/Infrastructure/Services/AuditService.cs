using ProgramAssuranceTool.Infrastructure.Interfaces;
using ProgramAssuranceTool.Models;
using ProgramAssuranceTool.Repositories;

namespace ProgramAssuranceTool.Infrastructure.Services
{
	public class AuditService : IAuditService
	{
		public void AuditActivity( string activity, string userId )
		{
			if (string.IsNullOrEmpty( activity )) return;

			var userActivity = new UserActivity
				{
					Activity = activity,
					UserId = userId ?? "not passed"
				};
			var activityRepository = new UserActivityRepository<UserActivity>();
			activityRepository.Add( userActivity );
		}

		public void AuditActivity( string activity, string userId, UserActivityRepository<UserActivity> activityRepository )
		{
			if ( string.IsNullOrEmpty( activity ) ) return;
			if (activityRepository == null)
			{
				AuditActivity( activity, userId );
				return;
			}

			var userActivity = new UserActivity
			{
				Activity = activity,
				UserId = userId ?? "not passed"
			};
			activityRepository.Add( userActivity );
		}
	}
}