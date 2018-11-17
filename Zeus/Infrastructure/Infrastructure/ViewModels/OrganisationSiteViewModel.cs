using System.ComponentModel.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;
using DisplayNameAttribute = System.ComponentModel.DisplayNameAttribute;

namespace Employment.Web.Mvc.Infrastructure.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    [DisplayName("Organisation and site code")]
    [Group("Organisation")]
    public class OrganisationSiteViewModel
    {
        /// <summary>
        /// Organisation
        /// </summary>
        
        [Bindable]
        [AdwSelection(SelectionType.Single, AdwType.ListCode, "ORG")]
        public string Organisation { get; set; }

        /// <summary>
        /// Site
        /// </summary>
        
        [Bindable]
        [AdwSelection(SelectionType.Single, AdwType.RelatedCode, "ORGF", DependentProperty = "Organisation", Dominant = true)]
        public string Site { get; set; }
    }
}
