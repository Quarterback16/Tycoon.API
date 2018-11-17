using System.Collections.Generic;
using System.Linq;
using ProgramAssuranceTool.Models;
using GridSettings = MvcJqGrid.GridSettings;

namespace ProgramAssuranceTool.Interfaces
{
	public interface IProjectAttachmentRepository
	{
		/// <summary>
		/// Gets all project attachments by its project id
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns></returns>
	    IQueryable<ProjectAttachment> GetAll(int projectId);


		 /// <summary>
		 /// Gets all project attachment by its grid setting and project id
		 /// </summary>
		 /// <param name="gridSettings">The grid settings.</param>
		 /// <param name="projectId">The project identifier.</param>
		 /// <returns></returns>
       List<ProjectAttachment> GetAll(GridSettings gridSettings, int projectId);

		 /// <summary>
		 /// Gets Project attachment  by identifier.
		 /// </summary>
		 /// <param name="id">The project attachment Id.</param>
		 /// <returns></returns>
		ProjectAttachment GetById(int id);

		/// <summary>
		/// Inserts the project attachment.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <param name="fileData">The file data.</param>
		void Insert(ProjectAttachment model, byte[] fileData);

		/// <summary>
		/// Updates the attachment.
		/// </summary>
		/// <param name="model">The model.</param>
		void Update(ProjectAttachment model);

		/// <summary>
		/// Deletes the specified attachment.
		/// </summary>
		/// <param name="id">The project attachment identifier.</param>
		void Delete(int id);

		/// <summary>
		/// Db connections the string.
		/// </summary>
		/// <returns></returns>
		string ConnectionString();

		/// <summary>
		/// Counts the  project attachment by its project id.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns></returns>
		int Count(int projectId);

		/// <summary>
		/// Counts the project attachment by its project id and  grid settings.
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="projectId">The project identifier.</param>
		/// <returns></returns>
		int Count(GridSettings gridSettings, int projectId);
    }
}