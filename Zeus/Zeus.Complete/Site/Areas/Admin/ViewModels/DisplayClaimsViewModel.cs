using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;

namespace Employment.Web.Mvc.Area.Admin.ViewModels
{
    [Group("Claims")]
    public class DisplayClaimsViewModel : ILayoutOverride
    {
        /// <summary>
        /// An enumerable of <see cref="ClaimViewModel" />.
        /// </summary>
        [Description("Current Security Claims")]
        [Display(GroupName = "Claims")]
        [DataType(CustomDataType.Grid)]
        [Infrastructure.DataAnnotations.Bindable]
        public IEnumerable<ClaimViewModel> Claims { get; set; }


        public DisplayClaimsViewModel()
        {
            Hidden = new[] { LayoutType.RequiredFieldsMessage };
        }

        public IEnumerable<LayoutType> Hidden
        {
            get; set;
        }
    }
}