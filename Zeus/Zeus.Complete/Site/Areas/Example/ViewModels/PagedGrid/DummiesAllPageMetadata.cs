using System;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Area.Example.ViewModels.PagedGrid
{
    /// <summary>
    /// Data used for retrieving next page of <see cref="DummiesAllViewModel.Dummies" />.
    /// </summary>
    [Serializable]
    public class DummiesAllPageMetadata : PageMetadata
    {
        #region Data to reproduce call

        /// <summary>
        /// Filter to names starting with.
        /// </summary>
        public string StartsWith { get; set; }

        #endregion
    }
}