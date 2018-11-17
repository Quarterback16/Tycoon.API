using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DisplayNameAttribute = System.ComponentModel.DisplayNameAttribute;


namespace Employment.Web.Mvc.Area.Example.ViewModels.Tables
{
    [Serializable]
    [DisplayName("Demo Data Row")]
    public class FootableDemoRowExp
    {
        [Key]
        [Bindable]
        [FooTableColumn(DataHideClassType.Phone)]
        public string JobseekerId { get; set; }

        [Bindable]
        public string Surname { get; set; }

        [Bindable]
        [FooTableColumn(DataHideClassType.Tablet)]
        public string Firstname { get; set; }
        
        [Bindable]
        [FooTableColumn(DataHideClassType.Tablet)]
        public string PostCode { get; set; }

        [Bindable]
        [FooTableColumn(DataHideClassType.Phone)]
        public string Country { get; set; }

        [Bindable]
        [FooTableColumn(DataHideClassType.All)]
        public string EPPStatus { get; set; }

    }

}
