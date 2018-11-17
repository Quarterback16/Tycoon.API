using System.Collections.Generic;
using System.Linq;
using ProgramAssuranceTool.Models;

namespace ProgramAssuranceTool.Interfaces
{
	/// <summary>
	/// This Bulletin Repository Interface,
	/// </summary>
    public interface IBulletinRepository
    {
		 /// <summary>
		 /// To get all bulletin data based on its type and admin privilage.
		 /// </summary>
		 /// <param name="bulletinType">Type of the bulletin.</param>
		 /// <param name="isAdmin">if set to <c>true</c> [is admin].</param>
		 /// <returns>a list of bulletin</returns>
        IQueryable<Bulletin> GetAll(string bulletinType, bool isAdmin);

		  /// <summary>
		  /// To get all bulletin data based on its type and admin privilage and grid setting
		  /// </summary>
		  /// <param name="gridSettings">The grid settings.</param>
		  /// <param name="bulletinType">Type of the bulletin.</param>
		  /// <param name="isAdmin">if set to <c>true</c> [is admin].</param>
		  /// <returns>a list of bulletin</returns>
        List<Bulletin> GetAll(MvcJqGrid.GridSettings gridSettings, string bulletinType, bool isAdmin);

		  /// <summary>
		  /// Gets the bulletin by its id.
		  /// </summary>
		  /// <param name="id">The bulletin id.</param>
		  /// <returns>a bulletin</returns>
        Bulletin GetById(int id );

		  /// <summary>
		  /// Adds the specified bulletin.
		  /// </summary>
		  /// <param name="entity">The bulletin.</param>
        void Add(Bulletin entity);

		  /// <summary>
		  /// Updates the specified bulletin.
		  /// </summary>
		  /// <param name="entity">The bulletin.</param>
        void Update(Bulletin entity);

		  /// <summary>
		  /// Deletes the specified bulletin.
		  /// </summary>
		  /// <param name="id">The bulletin id.</param>
        void Delete(int id);

		  /// <summary>
		  /// Counts the bulletins by its type and admin privilage.
		  /// </summary>
		  /// <param name="bulletinType">Type of the bulletin.</param>
		  /// <param name="isAdmin">if set to <c>true</c> [is admin].</param>
		  /// <returns>number of bulletin matching those criterias</returns>
        int Count(string bulletinType, bool isAdmin);

		  /// <summary>
		  /// Counts the bulletin by its grid settings, bulletin type and admin privilage
		  /// </summary>
		  /// <param name="gridSettings">The grid settings.</param>
		  /// <param name="bulletinType">Type of the bulletin.</param>
		  /// <param name="isAdmin">if set to <c>true</c> [is admin].</param>
		  /// <returns>number of bulletin matching those criterias</returns>
        int Count(MvcJqGrid.GridSettings gridSettings, string bulletinType, bool isAdmin);

		  /// <summary>
		  /// Database connections string.
		  /// </summary>
		  /// <returns></returns>
        string ConnectionString();
    }
}