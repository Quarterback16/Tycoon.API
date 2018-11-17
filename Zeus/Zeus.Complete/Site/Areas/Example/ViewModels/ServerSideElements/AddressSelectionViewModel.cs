using System.ComponentModel.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.ViewModels.Geospatial;
using Employment.Web.Mvc.Infrastructure.ViewModels;

namespace Employment.Web.Mvc.Area.Example.ViewModels.ServerSideElements
{
    [Group("Address selection")]
    [Button("Submit")]
    [Button("Clear", Clear=true)]
    public class AddressSelectionViewModel
    {
        public AddressSelectionViewModel()
        {

            ResidentialAddress = new Infrastructure.ViewModels.Geospatial.AddressViewModel {AddressType = "Residential", ReturnLatLongDetails = true};
            PostalAddress = new Infrastructure.ViewModels.Geospatial.AddressViewModel { AddressType = "Postal" };

            AddressInfo = new ContentViewModel().AddTitle("Address Details")
                                                .AddSubTitle("Latitude, Longitude and Locality details are returned if ")
                                                .AddLineBreak()
                                                .AddPreformatted("ReturnLatLong is set to true");
        }
        /*
        // Ian's autocomplete address control.
        //To pass parameters to ajax call use [AjaxSelection("ValidateAddress",Controller = "Address" ,Parameters = new string[]{"State","Postcode"})]
        [AjaxSelection("ValidateAddress", Controller = "Address")]
        [Bindable()]
        [Required(ErrorMessage = "7 characters of Address are required")]
        public string Address { get; set; }
         */

        


        [Bindable]
        public Employment.Web.Mvc.Infrastructure.ViewModels.Geospatial.AddressViewModel ResidentialAddress { get; set; }

        [Bindable]
        public Employment.Web.Mvc.Infrastructure.ViewModels.Geospatial.AddressViewModel PostalAddress { get; set; }


        [Bindable]
        [Display(GroupName = "Address selection")]
        public ContentViewModel AddressInfo { get; set; }



    }
}