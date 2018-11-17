using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Employment.Web.Mvc.Area.Admin.ViewModels.AdwLookup
{
    /// <summary>
    /// List Code Type Page Metadata.
    /// </summary>
    [Serializable]
    public class ListCodeTypePageMetadata : Employment.Web.Mvc.Infrastructure.Types.PageMetadata
    {
         
        /// <summary>
        /// Code Type.
        /// </summary>
        [Bindable]
        public string StartsWith { get; set; }


        /// <summary>
        /// Short Description.
        /// </summary>
        [Bindable]
        public string ListType { get; set; }


        /// <summary>
        /// Long Description.
        /// </summary>
        [Bindable]
        public bool ExactLookup { get; set; }

        /// <summary>
        /// Max Rows
        /// </summary>
        [Bindable]
        public int MaxRows { get; set; }
    }
}