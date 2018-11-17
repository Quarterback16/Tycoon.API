using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Employment.Web.Mvc.Area.Admin.ViewModels.AdwLookup
{
    /// <summary>
    /// Adw Code Type View-Model.
    /// </summary>
    [Serializable]
    public class CodeTypeViewModel
    {

        /// <summary>
        /// Code Type.
        /// </summary>
        [Bindable]
        [Key]
        [Display(Name = "Code Type")]
        public string CodeType { get; set; }


        /// <summary>
        /// Short Description.
        /// </summary>
        [Bindable]
        [Display(Name = "Short Description")]
        public string ShortDescription { get; set; }


        /// <summary>
        /// Long Description.
        /// </summary>
        [Bindable]
        [FooTableColumn(DataHideClassType.Phone)]
        [Display(Name = "Long Description")]
        public string LongDescription { get; set; }



        /// <summary>
        /// Long Description.
        /// </summary>
        [Bindable]
        [FooTableColumn(DataHideClassType.Phone)]
        [Display(Name = "Currency Start Date")]
        [DataType(DataType.Date)]
        [VisibleIfTrue("ShowListCodeColumns")]
        public DateTime? CurrencyStartDate { get; set; }


        /// <summary>
        /// Long Description.
        /// </summary>
        [Bindable]
        [FooTableColumn(DataHideClassType.Phone)]
        [Display(Name = "Currency End")]
        [VisibleIfTrue("ShowIntColumns")]
        public long CurrencyStart { get; set; }


        /// <summary>
        /// Long Description.
        /// </summary>
        [Bindable]
        [FooTableColumn(DataHideClassType.Phone)]
        [Display(Name = "Currency End")]
        [VisibleIfTrue("ShowIntColumns")]
        public long CurrencyEnd { get; set; }



        /// <summary>
        /// Long Description.
        /// </summary>
        [Bindable]
        [FooTableColumn(DataHideClassType.Phone)]
        [Display(Name = "Currency End Date")]
        [VisibleIfTrue("ShowListCodeColumns", PassOnNull = false)]
        public DateTime? CurrencyEndDate { get; set; }


        [Bindable]
        [Hidden]
        public bool ShowListCodeColumns { get; set; }


        [Bindable]
        [Hidden]
        public bool ShowIntColumns { get; set; }

    }
}