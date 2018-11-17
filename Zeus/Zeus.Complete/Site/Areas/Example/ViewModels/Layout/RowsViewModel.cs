using System.ComponentModel.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using DisplayNameAttribute = System.ComponentModel.DisplayNameAttribute;
using System;

namespace Employment.Web.Mvc.Area.Example.ViewModels.Layout
{
    [Group("Rows in groups", Order = 5)]
    [Group("Group A", Order = 10)]
    [Group("Group rows", Order = 15)]
    [Group("Group B", Order = 20, RowName = "GroupRowOne", RowType = GroupRowType.OneThird)]
    [Group("Group C" , Order = 30, RowName = "GroupRowOne", RowType = GroupRowType.TwoThird)]
    [Group("Group D", Order = 40, RowName = "GroupRowTwo", RowType = GroupRowType.Half)]
    [Group("Group E", Order = 50, RowName = "GroupRowTwo", RowType = GroupRowType.Half)]
    [Button("Group A Submit", "Group A")]
    [Button("Group B Submit", "Group B")]
    [Button("Group C Submit", "Group C")]
    public class RowsViewModel
    {
        [Display(GroupName="Rows in groups")]
        public ContentViewModel Introduction
        {
            get
            {
                return new ContentViewModel()
                    .AddParagraph(@"Elements on the page may take up the full width of the page, but this can be altered by adding the ""Row"" attribute to data elements with the page. This creates a flexible grid structure, that is responsive to browser window sizes.")
                    .AddParagraph(@"The avaialble row types inlcude: Half, Third, Quarter, and Flow (equally sized elements). Make sure to include only the correct number of elements for the row type that you're using (e.g. 2 elements with row type ""half"", 3 elements with row type ""third"". You can do combinations also, such as a row with 1 ""half"" element and two ""quarter"" elements.")
                    .AddPreformatted(@"        // Elements in the same row must have the same Row ""name"" (In this case: ""numbers"").
        [Display(Name = ""Data two"", GroupName = ""Group A"", Order = 20)]
        [Row(""numbers"", RowType.Half)]
        [Bindable]
        public int Data2 { get; set; }

        // half row
        [Display(Name = ""Data three"", GroupName = ""Group A"", Order = 30)]
        [Row(""numbers"", RowType.Half)]
        [Bindable]
        public int? Data3 { get; set; }
")
                ;
            }
        }


        #region Element rows
        // Group A
        // full row
        [Display(Name = "Data one", GroupName = "Group A", Order = 10)]
        [Row("first", RowType.Default)]
        [Bindable]
        public decimal Data1 { get; set; }

        [Display(Name = "Data two", GroupName = "Group A", Order = 20)]
        [Row("numbers", RowType.Half)]
        [Bindable]
        public int Data2 { get; set; }

        // half row
        [Display(Name = "Data three", GroupName = "Group A", Order = 30)]
        [Row("numbers", RowType.Half)]
        [Bindable]
        public int? Data3 { get; set; }

        // third row
        [Display(Name = "Data X", GroupName = "Group A", Order = 40)]
        [Row("letters", RowType.Third)]
        [Bindable]
        public string DataX { get; set; }

        [Display(Name = "Data Y", GroupName = "Group A", Order = 50)]
        [Row("letters", RowType.Third)]
        [Bindable]
        public string DataY { get; set; }

        [Display(Name = "Data Z", GroupName = "Group A", Order = 60)]
        [Row("letters", RowType.Third)]
        [Bindable]
        public string DataZ { get; set; }

        // quarter row
        [Display(Name = "Data A", GroupName = "Group A", Order = 70)]
        [Row("mixed", RowType.Quarter)]
        [Bindable]
        public string DataA { get; set; }

        [Display(Name = "Data B", GroupName = "Group A", Order = 80)]
        [Row("mixed", RowType.Half)]
        [Bindable]
        public string DataB { get; set; }

        [Display(Name = "Data C", GroupName = "Group A", Order = 90)]
        [Row("mixed", RowType.Quarter)]
        [Bindable]
        public string DataC { get; set; }

        #endregion

        [Display(GroupName = "Group rows")]
        public ContentViewModel Introduction2
        {
            get
            {
                return new ContentViewModel()
                    .AddParagraph(@"Groups themselves do not have to take up an entire row, and can be set to use only part of a row. To do this, use the RowName property in concert with the RowType property on your group declaration.")
                    .AddPreformatted(@"
    [Group(""Group B"", Order = 20, RowName = ""GroupRowOne"", RowType = GroupRowType.OneThird)]
    [Group(""Group C"" , Order = 30, RowName = ""GroupRowOne"", RowType = GroupRowType.TwoThird)]
    [Group(""Group D"", Order = 40, RowName = ""GroupRowTwo"", RowType = GroupRowType.Half)]
    [Group(""Group E"", Order = 50, RowName = ""GroupRowTwo"", RowType = GroupRowType.Half)]
    public class RowsViewModel
    {
        ....
    }
")
                ;
            }
        }


        #region group rows
        // Group B
        [Display(Name = "Foo 1", GroupName = "Group B")]
        [Row("FooOne", RowType.Half)]
        [Bindable]
        public string Foo1 { get; set; }

        [Display(Name = "Foo 2", GroupName = "Group B")]
        [Row("FooOne", RowType.Half)]
        [Bindable]
        public string Foo2 { get; set; }

        [Display(Name = "Foo 3", GroupName = "Group B")]
        [Row("FooTwo", RowType.Default)]
        [Bindable]
        public string Foo3 { get; set; }

        // Group C
        [Display(Name = "Bar 1", GroupName = "Group C")]
        [Row("BarOne", RowType.Half)]
        [Bindable]
        public string Bar1 { get; set; }

        [Display(Name = "Bar 2", GroupName = "Group C")]
        [Row("BarOne", RowType.Half)]
        [Bindable]
        public string Bar2 { get; set; }

        [Display(Name = "Bar 3", GroupName = "Group C")]
        [Row("BarTwo", RowType.Flow)]
        [Bindable]
        public string Bar3 { get; set; }

        // Group D
        [Display(Name = "Jumble D", GroupName = "Group D")]
        [Bindable]
        public string JumbleD { get; set; }

        // Group E
        [Display(Name = "Jumble E", GroupName = "Group E")]
        [Bindable]
        public string JumbleE { get; set; }

        #endregion
    }
}