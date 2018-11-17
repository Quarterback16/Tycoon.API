using System;
using System.Collections.Generic;

namespace Employment.Web.Mvc.Area.Example.Service.Interfaces
{
    /// <summary>
    /// Defines the methods and properties that are required for a Dummy Service.
    /// </summary>
    /// <remarks>
    /// This is for example purposes only. A service should only ever exist in the Service projects.
    /// </remarks>
    public interface IDummyService
    {

        long Add(string name, DateTime? date, string emailAddress);
        void Edit(long id, string name, DateTime? date, string emailAddress);
        DummyModel Get(long ID);

        /// <summary>
        /// Finds all dummies that match the criteria.
        /// </summary>
        /// <param name="startsWith">Find dummy data that starts with the specified string.</param>
        /// <returns>A collection of <see cref="DummyModel" /> that match the criteria.</returns>
        IEnumerable<DummyModel> FindAll(string startsWith);


        /// <summary>
        /// Gets all the records that match the name.
        /// </summary>
        /// <param name="name">Specified string for search.</param>
        /// <returns>A collection of <see cref="SortModel"/> that match the criteria.</returns>
        IEnumerable<SortModel> GetAllForSorting(string name);

        /// <summary>
        /// Finds dummies that match the criteria. Basic sumulation of mainframe paging.
        /// </summary>
        /// <param name="startsWith">Find dummy data that starts with the specified string.</param>
        /// <param name="nextSequenceID">Starting ID of next sequence (used by mainframe for retrieving next page).</param>
        /// <returns>A collection of <see cref="DummyModel" /> that match the criteria.</returns>
        DummiesModel Find(string startsWith, long nextSequenceID);
    }
}
