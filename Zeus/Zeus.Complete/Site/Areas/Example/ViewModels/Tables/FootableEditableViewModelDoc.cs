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
    [Group("Footable Editable", Order = 1)]
    public class FootableEditableViewModelDoc
    {
        [Display(GroupName = "FootableBasic", Order = 1)]
        public ContentViewModel ContentForFootableRows1
        {
            get
            {
                var content = new ContentViewModel()
                    .AddParagraph("Footable is a third party \"Responsive\" table control.  It allows you to specify screen sizes (Breakpoints) at which columns of a table row will be 'hidden'.  Hidden Footable columns will appear in a hidden row which can be expanded by clicking a + symbol at the beginning of the row.")
                    .AddParagraph("To display an editable table on your page, the table row needs to be defined with the extra EditableTypeAttribute on the fields you wish to make editable:")
                    .AddPreformatted(@"
[Serializable]
[DisplayName(""Demo Data Row"")]
public class FootableEditableDemoRow
{
    [Key]
    [Bindable]
    public string JobseekerId { get; set; }

    [Bindable]
    [EditableType(true, DataType.Text)]
    public string Firstname { get; set; }

    [Bindable]
    [EditableType(true, DataType.Text)]
    public string Surname { get; set; }

    [Bindable]
    [EditableType(true, DataType.Text)]
    public string PostCode { get; set; }

    [Bindable]
    [EditableType(true, DataType.Text)]
    public string EPPStatus { get; set; }

}")
                    .AddParagraph("A table is defined on your ViewModel as IEnumerable&lt;your_row_type&gt;.  Using the FootableAttribute will render a FooTable:")
                    .AddPreformatted(@"
[Display(GroupName = ""<name of your group>"", Order = 2)]
[FooTable]
public IEnumerable&lt;FootableEditableDemoRow&gt; FootableEditableRows { get; set; }
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
public class FootableEditableDemoRow
{
    [Key]
    [Bindable]
    [FooTableColumn(DataHideClassType.Phone)]
    public string JobseekerId { get; set; }

    [Bindable]
    [FooTableColumn(DataHideClassType.Tablet)]
    [EditableType(true, DataType.Text)]
    public string Firstname { get; set; }

    [Bindable]
    [EditableType(true, DataType.Text)]
    public string Surname { get; set; }

    [Bindable]
    [FooTableColumn(DataHideClassType.Tablet)]
    [EditableType(true, DataType.Text)]
    public string PostCode { get; set; }

    [Bindable]
    [FooTableColumn(DataHideClassType.Phone)]
    [EditableType(true, DataType.Text)]
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
        [DataType(CustomDataType.GridEditable)]
        [FooTable]
        [Editable(true)]
        [AjaxGrid("Example", "SaveGrid1Data", "Tables")]
        public IEnumerable<FootableEditableDemoRow> FootableRows { get; set; }


        [Display(GroupName = "FootableBasic", Order = 3)]
        public ContentViewModel ContentForFootableRows2
        {
            get
            {
                var content = new ContentViewModel()
                    .AddParagraph("Dirty rows are detected and asynchronous saving can be enabled by attaching the AjaxGridAttribute to your table definition.  The attribute constructor takes \"Area\", \"Action\" and \"Controller\" which will receive the modified rows of the table.")
                    .AddPreformatted(@"
[Display(GroupName = ""<name of your group>"", Order = 2)]
[DataType(CustomDataType.GridEditable)]
[FooTable]
[AjaxGrid(""Example"", ""SaveGridData"", ""Tables"")]
public IEnumerable&lt;FootableEditableDemoRow&gt; FootableEditableRows { get; set; }
")
                    .AddParagraph("For the example shown we need to provide the SaveGridData MVC Action on the TablesController:")
                    .AddPreformatted(@"
[HttpPost]
public ActionResult SaveGridData(IEnumerable&lt;FootableEditableDemoRow&gt; rowData)
{
    ..
    ..
    ..
    ..

    return Json(""Success"");
}

");

                return content;
            }
        }



    }
}