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
    [Group("Simple Table Editable", Order = 1)]
    [Group("Simple Table Editable Other", Order = 2)]
    public class SimpleTableEditableViewModel
    {
        [Display(GroupName = "Simple Table Editable", Order = 1)]
        public ContentViewModel ContentForSimpleTableEditableRows
        {
            get
            {
                var content = new ContentViewModel()
                    .AddParagraph("Footable is a third party \"Responsive\" table control.  It allows to to specify screen sizes (Breakpoints) at which columns of a table row will be 'hidden'.  Hidden Footable columns will appear in a hidden row which can be expanded by clicking a + symbol at the beginning of the row.")
                    .AddParagraph("A table is defined on your ViewModel as IEnumerable<your_row_type>.  You create a Footable responsive table by adding the FootableAttribute.  You also specify data annotation Attributes on the underlying row class's properties dictating if they're editable as well other other details.  Here is the full set of related attributes.")
                    .AddParagraph("")
                    .AddParagraph("")
                    .AddPreformatted("Here's some code for ya!!!");

                return content;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        [Display(GroupName = "Simple Table Editable", Order = 2)]
        [DataType(CustomDataType.GridEditable)]
        [Bindable]
        [AjaxGrid("Example", "SaveGrid1Data", "Tables")]
        public IEnumerable<PersonRowEditable> SimpleTableEditableRows1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(GroupName = "Simple Table Editable Other", Order = 1)]
        [DataType(CustomDataType.GridEditable)]
        [Bindable]
        [AjaxGrid("Example", "SaveGrid2", "Tables")]
        public IEnumerable<PersonRowEditable> SimpleTableEditableRows2 { get; set; }

    }
}
