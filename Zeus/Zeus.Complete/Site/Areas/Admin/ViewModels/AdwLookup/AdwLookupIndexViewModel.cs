using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Employment.Web.Mvc.Area.Admin.ViewModels.AdwLookup
{
    /// <summary>
    /// Index view-model.
    /// </summary>

    [Group("Adw Lookup")]

    [Link(Name = "ListCodeType", Action = "ListCodeType", GroupName = "Adw Lookup", Order = 1)]
    [Link(Name = "ListCode", Action = "ListCode", GroupName = "Adw Lookup", Order = 2)]
    [Link(Name = "ListRelatedCodeType", Action = "ListRelatedCodeType", GroupName = "Adw Lookup", Order = 3)]
    [Link(Name = "ListRelatedCode", Action = "ListRelatedCode", GroupName = "Adw Lookup", Order = 4)]
    [Link(Name = "ListPropertyType", Action = "ListPropertyType", GroupName = "Adw Lookup", Order = 5)]
    [Link(Name = "ListProperty", Action = "ListProperty", GroupName = "Adw Lookup", Order = 6)]
    [Link(Name = "ListDeltas", Action = "ListDeltas", GroupName = "Adw Lookup", Order = 7)]

    public class AdwLookupIndexViewModel : ILayoutOverride
    {


        /// <summary>
        /// Gives overview of this section.
        /// </summary>
        [Display(GroupName = "Adw Lookup")]
        [Bindable]
        public ContentViewModel Overview { get; set; }


        public AdwLookupIndexViewModel()
        {
            Hidden = new Infrastructure.Types.LayoutType[] { LayoutType.RequiredFieldsMessage };
        }




        public IEnumerable<Infrastructure.Types.LayoutType> Hidden
        {
            get;
            set;
        }
    }
}