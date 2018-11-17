using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Employment.Web.Mvc.Area.Admin.ViewModels.AdwLookup
{
    /// <summary>
    /// Model for the search results for List Related Codes Search.
    /// </summary>
    [Serializable]
    public class RelatedCodeViewModel
    {

        [Key]
        [Bindable]
        [Display(Name = "Relationship Code")]
        public string RelationshipTypeCode { get; set; }


        [Bindable]
        [Display(Name = "Subordinate")]
        public string SubCode { get; set; }


        [Bindable]
        [Display(Name = "Dominant")]
        public string DominantCode { get; set; }



        [Bindable]
        [Display(Name = "Currency Start")]
        [FooTableColumn(DataHideClassType.Phone)]
        public DateTime? CurrencyStart { get; set; }

        /*
        /// <summary>
        /// NOT Supported in ADW Service.
        /// </summary>
        [Bindable]
        public bool HasCurrencyEnd { get; set; }
        */


        [Bindable]
        [Display(Name = "Currency End")]
        [FooTableColumn(DataHideClassType.Phone)]
        public DateTime? CurrencyEnd { get; set; }


        [Bindable]
        [Display(Name = "Sub Short Desc")] 
        public string SubShortDescription { get; set; }


        [Bindable]
        [Display(Name = "Sub Long Desc")]
        [FooTableColumn(DataHideClassType.Tablet)]
        public string SubLongDescription { get; set; }


        [Bindable]
        [Display(Name = "Dominant Short Desc")]
        public string DominantShortDescription { get; set; }


        [Bindable]
        [Display(Name = "Dominant Long Desc")]
        [FooTableColumn(DataHideClassType.Tablet)]
        public string DominantLongDescription { get; set; }


        [Bindable]
        [Display(Name = "Postion")]
        [FooTableColumn(DataHideClassType.Tablet)]
        public int RowPosition { get; set; }
    }
}