using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProgramAssuranceTool.Models;
using ProgramAssuranceTool.ViewModels.Project;

namespace ProgramAssuranceTool.Interfaces
{
	public interface IProjectRepository
	{
		/// <summary>
		/// Gets all the projects.
		/// </summary>
		/// <returns>a list of projects</returns>
		IQueryable<Project> GetAll( DateTime uploadFrom, DateTime uploadTo );

		/// <summary>
		/// Gets all the projects mathing the grid criteria.
		/// </summary>
		/// <param name="gridSettings">The grid settings.</param>
		/// <param name="uploadFrom"></param>
		/// <param name="uploadTo"></param>
		/// <returns>a list of projects</returns>
		List<Project> GetAll( MvcJqGrid.GridSettings gridSettings, DateTime uploadFrom, DateTime uploadTo );
		/// <summary>
		/// Gets the Project by its identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>Project object</returns>
		Project GetById( int id );
		/// <summary>
		/// Adds the specified Project.
		/// </summary>
		/// <param name="entity">The entity.</param>
		void Add( Project entity );
		/// <summary>
		/// Updates the specified Project.
		/// </summary>
		/// <param name="entity">The entity.</param>
		void Update( Project entity );
		/// <summary>
		/// Deletes the specified Project.
		/// </summary>
		/// <param name="id">The identifier.</param>
		void Delete( int id );
		/// <summary>
		/// Counts the number of projects.
		/// </summary>
		/// <returns>the number of projects</returns>
		int CountProjects();
		/// <summary>
		/// Gets all project types.
		/// </summary>
		/// <returns></returns>
		string[] GetAllProjectTypes();
		/// <summary>
		/// Determines whether the project name is used.
		/// </summary>
		/// <param name="projectName">Name of the project.</param>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>used or not</returns>
		bool IsProjectNameUsed( string projectName, int projectId );
		/// <summary>
		/// Stores the question data.
		/// </summary>
		/// <param name="vm">The vm.</param>
		/// <param name="stream">The stream.</param>
		/// <returns></returns>
		int StoreQuestionData( UploadQuestionsViewModel vm, Stream stream );
		/// <summary>
		/// Gets the project questions for the project.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns></returns>
		List<PatQuestion> GetProjectQuestions( int projectId );
	}
}