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

    [Button("Submit", GroupName = "Adw Lookup")]

    public class ListDeltasViewModel
    {

        /// <summary>
        /// Gives overview of this section.
        /// </summary>
        [Display(GroupName = "Adw Lookup")]
        public ContentViewModel Overview { get; set; }


        /// <summary>
        /// Delta type.
        /// </summary>
        [Bindable]
        [Display(Name = "Delta Type:", GroupName = "Adw Lookup", Order = 1)]
        [Row("one", Infrastructure.Types.RowType.Half)]
        [Selection(Infrastructure.Types.SelectionType.Single, new string[] { "C", "R", "P" }, new string[] { "Code", "Relationship", "Property" })]
        //[System.ComponentModel.DefaultValue("C")]
        [DataType(CustomDataType.RadioButtonGroupHorizontal)]
        public string DeltaType { get; set; }



        /// <summary>
        /// WS Type.
        /// </summary>
        [Bindable]
        [Display(Name = "WS Type:", GroupName = "Adw Lookup", Order = 2)]
        [Row("one", Infrastructure.Types.RowType.Half)]
        [EditableIf("DeltaType", Infrastructure.Types.ComparisonType.NotEqualTo, "P")]
        public bool WsType { get; set; }



        /// <summary>
        /// Start from table type.
        /// </summary>
        [Bindable]
        [Display(Name = "Code / Relationship Name:", GroupName = "Adw Lookup", Order = 3)]
        [Row("1", Infrastructure.Types.RowType.Third)]
        [StringLength(6, MinimumLength = 1, ErrorMessage = "Code / Relationship Name's length must be between 1 and 6.")]
        [EditableIf("DeltaType", Infrastructure.Types.ComparisonType.NotEqualTo, "P")]
        [RequiredIf("DeltaType", Infrastructure.Types.ComparisonType.NotEqualTo, "P")]
        public string CodeType { get; set; }


        /// <summary>
        /// Start from table type.
        /// </summary>
        [Bindable]
        [Display(Name = "Start Code:", GroupName = "Adw Lookup", Order = 4)]
        [Row("1", Infrastructure.Types.RowType.Third)]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Start Code's length must be between 1 and 10.")]
        [EditableIf("DeltaType", Infrastructure.Types.ComparisonType.EqualTo, "C")]
        public string StartCode { get; set; }




        /// <summary>
        /// Max rows returned.
        /// </summary>
        [Bindable]
        [Display(Name = "Maximum rows returned:", GroupName = "Adw Lookup", Order = 5)]
        [Row("1", Infrastructure.Types.RowType.Third)]
        [System.ComponentModel.DefaultValue(100)]
        public int MaxRows { get; set; }


        

        /// <summary>
        /// Row Position.
        /// </summary>
        [Bindable]
        [Display(Name = "Row Position:", GroupName = "Adw Lookup", Order = 6)]
        [Row("2", Infrastructure.Types.RowType.Third)]
        [EditableIf("DeltaType", Infrastructure.Types.ComparisonType.NotEqualTo, "C")]
        [RequiredIf("DeltaType", Infrastructure.Types.ComparisonType.EqualTo, "P")]
        public int Row { get; set; }


        /// <summary>
        /// Last Update Date.
        /// </summary>
        [Bindable]
        [Display(Name = "Last Update Date:", GroupName = "Adw Lookup", Order = 7)]
        [Row("2", Infrastructure.Types.RowType.Third)]
        [DataType(DataType.Date)]
        [EditableIf("DeltaType", Infrastructure.Types.ComparisonType.EqualTo, "P")]
        [RequiredIf("DeltaType", Infrastructure.Types.ComparisonType.EqualTo, "P")]
        public DateTime LastUpdateDate { get; set; }


        /// <summary>
        /// Last Update Time.
        /// </summary>
        [Bindable]
        [Display(Name = "Last Update Version:", GroupName = "Adw Lookup", Order = 8)]
        [Row("2", Infrastructure.Types.RowType.Third)]
        [EditableIf("DeltaType", Infrastructure.Types.ComparisonType.NotEqualTo, "P")]
        public int LastUpdateVersion { get; set; }



        /// <summary>
        /// Results.
        /// </summary>
        [Bindable]
        [DataType(CustomDataType.Grid)]
        [Display(GroupName = "Results")]
        [FooTable]
        public IPageable<DeltaViewModel> Results { get; set; }
    }
}