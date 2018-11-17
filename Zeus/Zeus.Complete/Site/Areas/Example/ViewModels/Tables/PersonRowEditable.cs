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
    [DisplayName("Person Row")]
    public class PersonRowEditable
    {
        [Key]
        [Bindable]
        public string JobseekerId { get; set; }

        [Bindable]
        [FooTableColumn(DataHideClassType.Phone)]
        [EditableType(true, DataType.Text)]
        public string Surname { get; set; }

        [Bindable]
        [FooTableColumn(DataHideClassType.Phone)]
        [EditableType(true, DataType.Text)]
        public string FirstName { get; set; }

        [Bindable]
        [FooTableColumn(DataHideClassType.Phone)]
        [EditableType(true, DataType.Date)]
        [ViewFormat("")]
        public DateTime BirthDate { get; set; }

        [Bindable]
        [FooTableColumn(DataHideClassType.Phone)]
        [EditableType(true, DataType.Date)]
        public string NextAppointment { get; set; }

        [Bindable]
        [FooTableColumn(DataHideClassType.Tablet)]
        public string Allowance { get; set; }

        [Bindable]
        [FooTableColumn(DataHideClassType.Tablet)]
        [EditableType(true, DataType.Text)]
        public string EPPStatus { get; set; }

        [Bindable]
        [FooTableColumn(DataHideClassType.All)]
        [EditableType(true, DataType.Text)]
        public string ContactNumber { get; set; }

        [Bindable]
        [FooTableColumn(DataHideClassType.Tablet)]
        [EditableType(true, DataType.Text)]
        public string Placement { get; set; }

        [Bindable]
        [FooTableColumn(DataHideClassType.All)]
        [EditableType(true, DataType.Text)]
        public string Status { get; set; }

        [Bindable]
        [FooTableColumn(DataHideClassType.All)]
        [EditableType(true, DataType.Text)]
        public string Postcode { get; set; }

        [Bindable]
        [FooTableColumn(DataHideClassType.All)]
        [EditableType(true, DataType.Text)]
        public string Suburb { get; set; }

        [Hidden]
        public bool IsDirty { get; set; }

    }



    public class PersonRowEditable_old
    {
        [Key]
        [Bindable]
        public int Id { get; set; }

        [Bindable]
        [FooTableColumn(DataHideClassType.Phone)]
        [EditableType(true, DataType.Text)]
        public string FirstName { get; set; }

        [Bindable]
        [FooTableColumn(DataHideClassType.Phone)]
        //[EditableType(true, DataType.Text)]
        public string LastName { get; set; }

        [Bindable]
        [FooTableColumn(DataHideClassType.All)]
        [EditableType(true, DataType.Text)]
        public string Country { get; set; }
    }



    public class Test
    {
        


    }


}
