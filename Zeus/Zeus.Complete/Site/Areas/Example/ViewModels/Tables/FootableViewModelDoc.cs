using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DisplayNameAttribute = System.ComponentModel.DisplayNameAttribute;

namespace Employment.Web.Mvc.Area.Example.ViewModels.Tables
{
    [DisplayName(".....")]
    [Group("FootableBasic", Order = 1)]
    public class FootableViewModelDoc
    {
        [Display(GroupName = "FootableBasic", Order = 1)]
        public ContentViewModel ContentForFootableRows1
        {
            get
            {
                var content = new ContentViewModel()
                    .AddParagraph("Footable is a third party \"Responsive\" table control.  It allows you to specify screen sizes (Breakpoints) at which columns of a table row will be 'hidden'.  Hidden Footable columns will appear in a hidden row which can be expanded by clicking a + symbol at the beginning of the row.")
                    .AddParagraph("To display a table on your page, the table row needs to be defined:")
                    .AddPreformatted(@"
[Serializable]
[DisplayName(""Demo Data Row"")]
public class FootableDemoRow
{
    [Key]
    [Bindable]
    public string JobseekerId { get; set; }

    [Bindable]
    public string Firstname { get; set; }

    [Bindable]
    public string Surname { get; set; }

    [Bindable]
    public string PostCode { get; set; }

    [Bindable]
    public string EPPStatus { get; set; }

}")
                    .AddParagraph("A table is defined on your ViewModel as IEnumerable&lt;your_row_type&gt;.  Using the FootableAttribute will render a FooTable:")
                    .AddPreformatted(@"
[Display(GroupName = ""<name of your group>"", Order = 2)]
[FooTable]
public IEnumerable&lt;FootableDemoRow&gt; FootableRows { get; set; }
")
                    .AddParagraph("Mark your properties with the FooTableColumnAttribute passing in a DataHideClassType enum value:")
                    .AddPreformatted(@"
DataHideClassType.All   /// The column on the table will always be shown in the hidden row
DataHideClassType.Phone   /// The column on the table will be hidden when the browser width is 'phone'
DataHideClassType.Tablet   /// The column on the table will be hidden when the browser width is 'tablet'
( Note: If you specify no DataHideClassType value then the column will always appear on the main row )

Applied to the previous row class:
[Serializable]
[DisplayName(""Demo Data Row"")]
public class FootableDemoRow
{
    [Key]
    [Bindable]
    [FooTableColumn(DataHideClassType.Phone)]
    public string JobseekerId { get; set; }

    [Bindable]
    [FooTableColumn(DataHideClassType.Tablet)]
    public string Firstname { get; set; }

    [Bindable]
    public string Surname { get; set; }

    [Bindable]
    [FooTableColumn(DataHideClassType.Tablet)]
    public string PostCode { get; set; }

    [Bindable]
    [FooTableColumn(DataHideClassType.Phone)]
    public string EPPStatus { get; set; }

}
");

                return content;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        [Display(GroupName = "FootableBasic", Order = 2)]
        [DataType(CustomDataType.Grid)]
        [FooTable]
        public IEnumerable<FootableDemoRowDoc> FootableRows { get; set; }


        //[Display(GroupName = "FootableBasic", Order = 3)]
        //public ContentViewModel ContentForFootableRows2
        //{
        //    get
        //    {
        //        var content = new ContentViewModel()
        //            .AddParagraph("You also specify data annotation Attributes on the underlying row class's properties dictating if they're editable as well other other details.  Here is the full set of related attributes.")
        //            .AddParagraph("")
        //            .AddPreformatted("Here's some code for ya!!!");

        //        return content;
        //    }
        //}

    }
}