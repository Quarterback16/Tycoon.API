using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DisplayNameAttribute = System.ComponentModel.DisplayNameAttribute;


namespace Employment.Web.Mvc.Area.Example.ViewModels.Tables
{
    [Serializable]
    [DisplayName("Demo Editable Data Row")]
    public class FootableEditableDemoRowExp
    {
        [Key]
        [Bindable]
        [FooTableColumn(DataHideClassType.Phone)]
        public string JobseekerId { get; set; }

        [Bindable]
        [EditableType(true, DataType.Text)]
        public string Surname { get; set; }

        [Bindable]
        [FooTableColumn(DataHideClassType.Tablet)]
        [EditableType(true, DataType.Text)]
        public string Firstname { get; set; }

        [Bindable]
        [FooTableColumn(DataHideClassType.Tablet)]
        [EditableType(true, DataType.Text)]
        public string PostCode { get; set; }

        [Bindable]
        [FooTableColumn(DataHideClassType.Phone)]
        [EditableType(true, DataType.Custom)]
        //[AdwSelection(SelectionType.Single, AdwType.ListCode, "AGE")]
        public string Country { get; set; }

        [Bindable]
        [FooTableColumn(DataHideClassType.Phone)]
        [EditableType(true, DataType.Text)]
        public string EPPStatus { get; set; }

    }

}
