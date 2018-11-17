using System.Collections.Generic;
using ProgramAssuranceTool.Models;

namespace ProgramAssuranceTool.Interfaces
{
	public interface IUserSettingsRepository
	{
		/// <summary>
		/// Adds the specified User Setting.
		/// </summary>
		/// <param name="entity">The user setting.</param>
		void Add( UserSetting entity );

		/// <summary>
		/// Gets all the settings for a particular user.
		/// </summary>
		/// <param name="userId">The user identifier.</param>
		/// <returns>list of settings</returns>
		IEnumerable<UserSetting> GetAllByUserId( string userId );

		/// <summary>
		/// Inserts a bunch of user settings into the data store
		/// </summary>
		/// <param name="userSettings">The user settings.</param>
		void InsertAll( List<UserSetting> userSettings );

		/// <summary>
		/// Deletes all the settings for a user.
		/// </summary>
		/// <param name="userId">The user identifier.</param>
		void DeleteAll( string userId );
	}
}