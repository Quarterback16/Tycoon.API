using System.Collections.Generic;
using ProgramAssuranceTool.Models;

namespace ProgramAssuranceTool.Interfaces
{
	public interface ISampleRepository
	{
		/// <summary>
		/// Gets the sample.
		/// </summary>
		/// <param name="sessionKey">The session key.</param>
		/// <returns>list of candidate sample claims</returns>
		List<Sample> GetSample( string sessionKey );

		/// <summary>
		/// Saves the sample.
		/// </summary>
		/// <param name="sessionKey">The session key.</param>
		/// <param name="claims">The claims.</param>
		/// <param name="userId">The user identifier.</param>
		void SaveSample( string sessionKey, List<PatClaim> claims, string userId );

		/// <summary>
		/// Saves the sample de selections.
		/// </summary>
		/// <param name="sessionKey">The session key.</param>
		/// <param name="list">The list.</param>
		/// <param name="selectedList">The selected list.</param>
		/// <param name="userId">The user identifier.</param>
		void SaveSampleDeSelections( string sessionKey, List<Sample> list, List<Sample> selectedList, string userId );

		/// <summary>
		/// Deletes the by session key.
		/// </summary>
		/// <param name="sessionKey">The session key.</param>
		void DeleteBySessionKey( string sessionKey );

		/// <summary>
		/// Gets the distinct session keys.
		/// </summary>
		/// <returns>array of keys</returns>
		string[] GetDistinctSessionKeys();
	}
}