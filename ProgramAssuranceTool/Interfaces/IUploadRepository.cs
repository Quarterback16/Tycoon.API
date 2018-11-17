using System.Collections.Generic;
using ProgramAssuranceTool.Models;

namespace ProgramAssuranceTool.Interfaces
{
	public interface IUploadRepository
	{
		/// <summary>
		/// Gets all the uploads.
		/// </summary>
		/// <returns>a list of uploads</returns>
		List<Upload> GetAll();

		/// <summary>
		/// Gets the Upload by its identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>upload record</returns>
		Upload GetById( int id );

		/// <summary>
		/// Adds the specified Upload.
		/// </summary>
		/// <param name="entity">The upload.</param>
		void Add( Upload entity );

		/// <summary>
		/// Updates the specified upload object.
		/// </summary>
		/// <param name="entity">The upload.</param>
		void Update( Upload entity );

		/// <summary>
		/// Deletes the specified upload.
		/// </summary>
		/// <param name="id">The identifier.</param>
		void Delete( int id );

		/// <summary>
		/// Counts the number of uploads for a project.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>the count</returns>
		int CountUploads( int projectId );

		/// <summary>
		/// Gets all the uploads for a particular project.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="gs">The gs.</param>
		/// <returns>a list of uploads</returns>
		List<Upload> GetAllByProjectId( int projectId, MvcJqGrid.GridSettings gs );
	}
}