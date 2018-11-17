using System.Collections.Generic;
using ProgramAssuranceTool.Models;

namespace ProgramAssuranceTool.Interfaces
{
	public interface IProjectContractRepository
	{
		/// <summary>
		/// Adds a ProjectContract record.
		/// </summary>
		/// <param name="entity">The entity.</param>
		void Add( ProjectContract entity );
		/// <summary>
		/// Gets all the ProjectContracts for a specified projectId
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>list of Project Contracts</returns>
		List<ProjectContract> GetAllByProjectId( int projectId );
		/// <summary>
		/// Saves the project contracts.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="list">The list.</param>
		/// <param name="userId">The user identifier.</param>
		void SaveProjectContracts( int projectId, List<ProjectContract> list, string userId );
	}
}
