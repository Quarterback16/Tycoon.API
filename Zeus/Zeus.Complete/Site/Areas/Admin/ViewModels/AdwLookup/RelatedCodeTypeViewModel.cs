using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Employment.Web.Mvc.Area.Admin.ViewModels.AdwLookup
{
    /// <summary>
    /// View model for collection of Related Code Type search results.
    /// </summary>
    [Serializable]
    public class RelatedCodeTypeViewModel
    {

        /// <summary>
        /// Relationship.
        /// </summary>
        [Bindable]
        [Key]
        public string Relationship { get; set; }

        /// <summary>
        /// Sub-type.
        /// </summary>
        [Bindable]
        [Display(Name= "Sub Type")]
        public string SubType { get; set; }

        /// <summary>
        /// Sub Description.
        /// </summary>
        [Bindable]
        [FooTableColumn(DataHideClassType.Tablet)]
        [Display(Name = "Sub Description")]
        public string SubDescription { get; set; }

        /// <summary>
        /// Dom-type.
        /// </summary>
        [Bindable]
        [Display(Name = "Dom Type")]
        public string DomType { get; set; }

        /// <summary>
        /// Dom-description.
        /// </summary>
        [Bindable]
        [FooTableColumn(DataHideClassType.Tablet)]
        [Display(Name = "Dom Description")]
        public string DomDescription { get; set; }
    }
}