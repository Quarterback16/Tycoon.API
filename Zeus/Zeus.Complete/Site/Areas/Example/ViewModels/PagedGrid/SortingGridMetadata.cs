using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Area.Example.ViewModels.PagedGrid
{
    /// <summary>
    /// Data received for retrieving next page of <see cref="SortingViewModel"/>.
    /// </summary>
    [Serializable]
    public class SortingGridMetadata: PageMetadata
    {
        #region data to reproduce call


        /// <summary>
        /// Filter to names property.
        /// </summary>
        public string Name { get; set; }
        
        
        #endregion
    }
}