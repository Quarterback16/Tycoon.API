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
    public class PersonRow
    {
        [Key]
        [Bindable]
        public string JobseekerId { get; set; }

        [Bindable]
        [FooTableColumn(DataHideClassType.Phone)]
        public string Surname { get; set; }

        [Bindable]
        [FooTableColumn(DataHideClassType.Phone)]
        public string FirstName { get; set; }

        [Bindable]
        [FooTableColumn(DataHideClassType.Phone)]
        [ViewFormat("")]
        public DateTime BirthDate { get; set; }

        [Bindable]
        [FooTableColumn(DataHideClassType.Phone)]
        public string NextAppointment { get; set; }

        [Bindable]
        [FooTableColumn(DataHideClassType.Tablet)]
        public string Allowance { get; set; }

        [Bindable]
        [FooTableColumn(DataHideClassType.Tablet)]
        public string EPPStatus { get; set; }

        [Bindable]
        [FooTableColumn(DataHideClassType.All)]
        public string ContactNumber { get; set; }

        [Bindable]
        [FooTableColumn(DataHideClassType.Tablet)]
        public string Placement { get; set; }

        [Bindable]
        [FooTableColumn(DataHideClassType.All)]
        public string Status { get; set; }

        [Bindable]
        [FooTableColumn(DataHideClassType.All)]
        public string Postcode { get; set; }

        [Bindable]
        [FooTableColumn(DataHideClassType.All)]
        public string Suburb { get; set; }




    }

}
