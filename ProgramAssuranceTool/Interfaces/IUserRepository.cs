using System.Collections.Generic;
using System.Web.Mvc;
using ProgramAssuranceTool.Infrastructure.Interfaces;
using ProgramAssuranceTool.Models;

namespace ProgramAssuranceTool.Interfaces
{
	public interface IUserRepository
	{
		/// <summary>
		/// Gets the program assurance users.
		/// </summary>
		/// <returns>list of users</returns>
		List<PatUser> GetProgramAssuranceUsers();

		/// <summary>
		/// Gets the program assurance users for a particular state code.
		/// </summary>
		/// <param name="stateCode">The state code.</param>
		/// <param name="cacheService">The cache service.</param>
		/// <returns>list of users</returns>
		List<PatUser> GetProgramAssuranceUsers( string stateCode, ICacheService cacheService );

		/// <summary>
		/// Gets the program assurance user ids.
		/// </summary>
		/// <param name="stateCode">The state code.</param>
		/// <param name="cacheService">The cache service.</param>
		/// <returns></returns>
		List<string> GetProgramAssuranceUserIds( string stateCode, ICacheService cacheService );

		/// <summary>
		/// Gets the program assurance user dropdown list.
		/// </summary>
		/// <param name="cacheService">The cache service.</param>
		/// <returns>a list of user selection items</returns>
		IEnumerable<SelectListItem> GetProgramAssuranceDropdownList( ICacheService cacheService );

		/// <summary>
		/// Gets all pa users in a group.
		/// </summary>
		/// <param name="groupPaths">The group paths.</param>
		/// <returns></returns>
		List<PatUser> GetAllPAUsersByGroupPaths( List<string> groupPaths );
	}
}