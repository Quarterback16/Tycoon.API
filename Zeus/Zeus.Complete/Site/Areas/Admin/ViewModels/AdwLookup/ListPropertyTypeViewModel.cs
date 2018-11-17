using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Employment.Web.Mvc.Area.Admin.ViewModels.AdwLookup
{

    /// <summary>
    /// 
    /// </summary>
    [Group("Adw Lookup")]
    [Group("Results")]

    [Button("Submit", Primary = true, GroupName = "Adw Lookup")]
   
    public class ListPropertyTypeViewModel
    {

        /// <summary>
        /// Gives overview of this section.
        /// </summary>
        [Display(GroupName = "Adw Lookup")]
        public ContentViewModel Overview { get; set; }


        /// <summary>
        /// Start from table type.
        /// </summary>
        [Bindable]
        [Display(Name = "Start From Table Type:", GroupName = "Adw Lookup", Order = 1)]
        [Row("1", Infrastructure.Types.RowType.Half)]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Starting table type's length must be between 1 and 10.")]
        public string StartFromTableType { get; set; }




        /// <summary>
        /// Start from Property.
        /// </summary>
        [Bindable]
        [Display(Name = "Start From Property:", GroupName = "Adw Lookup", Order = 2)]
        [Row("1", Infrastructure.Types.RowType.Half)]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Start From Property's length must be between 1 and 10.")]
        public string StartFromProperty { get; set; }


        


        /// <summary>
        /// List type.
        /// </summary>
        [Bindable]
        [Display(Name = "List Type:", GroupName = "Adw Lookup", Order = 3)]
        [Row("2", Infrastructure.Types.RowType.Third)]
        [Selection(Infrastructure.Types.SelectionType.Single, new string[] { "A", "C", "E" }, new string[] { "All", "Current", "Ended" })]
//        [System.ComponentModel.DefaultValue("A")]
        [DataType(CustomDataType.RadioButtonGroupHorizontal)]
        public string ListType { get; set; }



        /// <summary>
        /// Max rows returned.
        /// </summary>
        [Bindable]
        [Display(Name = "Maximum rows returned:", GroupName = "Adw Lookup", Order = 4)]
        [Row("2", Infrastructure.Types.RowType.Third)]
        [System.ComponentModel.DefaultValue(100)]
        public int MaxRows { get; set; }



        /// <summary>
        /// Exact lookup.
        /// </summary>
        [Bindable]
        [Display(Name = "Exact Lookup:", GroupName = "Adw Lookup", Order = 5)]
        [Row("2", Infrastructure.Types.RowType.Third)]
        public bool ExactLookup { get; set; }

        /// <summary>
        /// Results.
        /// </summary>
        [Bindable]
        [DataType(CustomDataType.Grid)]
        [Display(GroupName = "Results")]
        [FooTable]
        public IPageable<PropertyViewModel> Results { get; set; }
    }
}