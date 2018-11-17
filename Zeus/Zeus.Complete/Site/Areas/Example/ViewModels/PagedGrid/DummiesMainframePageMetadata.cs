using System;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Area.Example.ViewModels.PagedGrid
{
    /// <summary>
    /// Data used for retrieving next page of <see cref="DummiesMainframeViewModel.Dummies" />.
    /// </summary>
    /// <remarks>
    /// Note that HasMorePages has been overridden to indicates from the mainframe paging data whether there are more pages.
    /// </remarks>
    [Serializable]
    public class DummiesMainframePageMetadata : PageMetadata
    {
        #region Data to reproduce call

        /// <summary>
        /// Filter to names starting with.
        /// </summary>
        public string StartsWith { get; set; }

        #endregion

        #region Data used by mainframe for determining the next page

        /// <summary>
        /// The starting ID of the next page in the sequence.
        /// </summary>
        public long NextSequenceID { get; set; }

        #endregion

        /// <summary>
        /// Indicates whether there are more pages.
        /// </summary>
        /// <remarks>
        /// Must be overridden for mainframe based paging.
        /// </remarks>
        /// <returns><c>true</c> if there are more pages; otherwise, <c>false</c>.</returns>
        public override bool HasMorePages()
        {
            return NextSequenceID > 0;
        }
    }
}