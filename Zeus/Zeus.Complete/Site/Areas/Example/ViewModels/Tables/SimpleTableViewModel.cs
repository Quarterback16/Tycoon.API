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
    /// <summary>
    /// 
    /// </summary>
    [DisplayName(".....")]
    [Group("Simple Table", Order = 1)]
    [Button("Submit", "Simple Table")]
    public class SimpleTableViewModel
    {
        [Display(GroupName = "Simple Table", Order = 1)]
        public ContentViewModel ContentForSimpleTableRows
        {
            get
            {
                var content = new ContentViewModel()
                   .AddPreformatted(@"        /// A simple boolean select example
        [Display(GroupName = ""Selection"", Order = -6, Name = ""Basic boolean"")]
        [Bindable]
        public IEnumerable<PersonRow> SimpleTableRows { get; set; }");

                return content;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        [Display(GroupName = "Simple Table", Order = 2)]
        [DataType(CustomDataType.Grid)]
        [Bindable]
        public IEnumerable<PersonRow> SimpleTableRows { get; set; }


    }


   


}