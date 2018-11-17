using System.ComponentModel.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;

namespace Employment.Web.Mvc.Area.Example.ViewModels.DataAccess
{
    public class CaGenViewModel
    {
        [Bindable]
        public string SiteName { get; set; }
        [Bindable]
        public string AddressLine1 { get; set; }
        [Bindable]
        public string AddressLine2 { get; set; }
        [Bindable]
        public string AddressLine3 { get; set; }
        [Bindable]
        public string Contracts { get; set; }

        
    }
}