using System.Collections.Generic;

namespace Employment.Web.Mvc.Area.Example.Service.Interfaces
{
    /// <summary>
    /// A collection of Dummy models with pretended mainframe paging information.
    /// </summary>
    /// <remarks>
    /// This is for example purposes only. A service should only ever exist in the Service projects.
    /// </remarks>
    public class DummiesModel
    {
        /// <summary>
        /// A collection of Dummy models.
        /// </summary>
        public IEnumerable<DummyModel> Dummies { get; set; }

        /// <summary>
        /// Starting ID of next sequence (used by mainframe for retrieving next page).
        /// </summary>
        public long NextSequenceID { get; set; }
    }
}