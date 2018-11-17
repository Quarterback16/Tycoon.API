using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Employment.Web.Mvc.Area.Admin.ViewModels.AdwLookup
{
    /// <summary>
    /// Represents view model for search results of Delta Type.
    /// </summary>
    [Serializable]
    public class DeltaViewModel
    {

        [Key]
        [Bindable]
        public string Code { get; set; }

        [Bindable]
        public string ShortDescription { get; set; }


        [Bindable]
        [FooTableColumn(DataHideClassType.Tablet)]
        public string LongDescription { get; set; }



        [Bindable]
        [FooTableColumn(DataHideClassType.Phone)]
        public string CurrencyStart { get; set; }


        [Bindable]
        [FooTableColumn(DataHideClassType.Phone)]
        public string CurrencyEnd { get; set; }
    }
}