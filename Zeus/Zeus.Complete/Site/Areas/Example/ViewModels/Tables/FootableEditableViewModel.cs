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
    public class FootableEditableViewModel
    {
        [Display(GroupName = "Footable Editable", Order = 1)]
        public ContentViewModel ContentForFootableEditableRows
        {
            get
            {
                var content = new ContentViewModel()
                    .AddParagraph("Footable is a third party \"Responsive\" table control.  It allows to to specify screen sizes (Breakpoints) at which columns of a table row will be 'hidden'.  Hidden Footable columns will appear in a hidden row which can be expanded by clicking a + symbol at the beginning of the row.")
                    .AddParagraph("A table is defined on your ViewModel as IEnumerable&lt;your_row_type&gt;.  You create a Footable responsive table by adding the FootableAttribute.  You also specify data annotation Attributes on the underlying row class's properties dictating if they're editable as well other other details.  Here is the full set of related attributes.")
                    .AddParagraph("")
                    .AddParagraph("")
                    .AddPreformatted("Here's some code for ya!!!");

                return content;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        [Display(GroupName = "Footable Editable", Order = 2)]
        [DataType(CustomDataType.GridEditable)]
        [FooTable]
        [Editable(true)]
        //[Bindable]
        [AjaxGrid("Example", "SaveGrid1Data", "Tables")]
        public IEnumerable<PersonRowEditable> FootableEditableRows { get; set; }


    }
}