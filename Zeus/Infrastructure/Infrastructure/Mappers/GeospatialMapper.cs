using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Department.AddressValidation;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Models.Geospatial;
using Employment.Web.Mvc.Infrastructure.TypeConverters;
using Employment.Web.Mvc.Infrastructure.Types.Geospatial;
using Employment.Web.Mvc.Infrastructure.ValueResolvers;
using Employment.Web.Mvc.Infrastructure.ViewModels.Geospatial;

namespace Employment.Web.Mvc.Infrastructure.Mappers
{
    /// <summary>
    /// Creates mappings between Geospatial model and service types.
    /// </summary>
    public static class GeospatialMapper
    {

        /// <summary>
        /// Convert to the attribute model.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        public static AttributeModel ToAttributeModel(this Region src)
        {
            AttributeModel dest = new AttributeModel();
            dest.Code = src.Code;
            dest.Name = src.Classification;
            dest.Value = src.Name;
            return dest;
        }

        /// <summary>
        /// Convert to the address model.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        public static AddressModel ToAddressModel(this Address src)
        {
            var dest = new AddressModel();
            dest.Attributes = new List<AttributeModel>();
            if (src.Regions != null)
            {
                foreach (var reg in src.Regions)
                {
                    dest.Attributes.Add(reg.ToAttributeModel());
                }
            }
            dest.Reliability = (AddressReliability)src.ReliabilityLevel.Level;
            dest.Line1 = src.AddressLine1;
            dest.Line2 = src.AddressLine2;
            dest.Line3 = src.AddressLine3;
            dest.Locality = src.Suburb;
            return dest;
        }

        /// <summary>
        /// Convert to the address model.
        /// </summary>
        /// <param name="srcList">The source list.</param>
        /// <returns></returns>
        public static IEnumerable<AddressModel> ToAddressModel(this IEnumerable<Address> srcList)
        {
            var destList = new List<AddressModel>();
            if (srcList != null)
            {
                foreach (var src in srcList)
                {
                    var dest = new AddressModel();
                    dest.Attributes = new List<AttributeModel>();
                    if (src.Regions != null)
                    {
                        foreach (var reg in src.Regions)
                        {
                            dest.Attributes.Add(reg.ToAttributeModel());
                        }
                    }
                    dest.Reliability = (AddressReliability) src.ReliabilityLevel.Level;
                    dest.Line1 = src.AddressLine1;
                    dest.Line2 = src.AddressLine2;
                    dest.Line3 = src.AddressLine3;
                    dest.Locality = src.Suburb;

                    dest.Latitude = src.Latitute;
                    dest.Longitude = src.Longitude;
                    dest.ReturnLatLongDetails = src.Latitute == null && src.Longitude == null ? false : true;
                    dest.State = src.State;
                    dest.Postcode = src.Postcode;
                    
                    destList.Add(dest);
                }
            }
            return destList as IEnumerable<AddressModel>;
        }


        /// <summary>
        /// Convert to the ajax address view model list.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        public static IEnumerable<AjaxAddressViewModel> ToAjaxAddressViewModelList(this IEnumerable<AddressModel> src) {
            var dest = new List<AjaxAddressViewModel>();
            if (src != null) {
                foreach (var s in src) {
                    dest.Add(s.ToAjaxAddressViewModel());
                }
            }
            return dest as IEnumerable<AjaxAddressViewModel>;
        }



        /// <summary>
        /// Convert to the ajax address view model.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        public static AjaxAddressViewModel ToAjaxAddressViewModel(this AddressModel src) {
            var dest = new AjaxAddressViewModel();
            dest.SingleLineAddress = AddressModelToSingleLineValueResolver.Resolve(src);
            dest.Line1 = src.Line1;

            // Mapping rest of the elements
            dest.Reliability = src.Reliability; 
            dest.Line2 = src.Line2;
            dest.Line3 = src.Line3;
            dest.Locality = src.Locality;
            dest.State = src.State;
            dest.Postcode = src.Postcode; 

            // Mapping Lat and long
            dest.Latitude = src.Latitude;
            dest.Longitude = src.Longitude;
            dest.ReturnLatLongDetails = src.ReturnLatLongDetails;            

            return dest;
        }

        /// <summary>
        /// Convert to the attribute model.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        public static AddressViewModel ToAttributeModel(this AddressModel src) {
            var dest = new AddressViewModel();
            dest.Line1 = src.Line1;
            dest.Line2 = src.Line2;
            dest.Line3 = src.Line3;
            dest.Locality = src.Locality;
            dest.Postcode = src.Postcode;
            dest.Reliability = src.Reliability;
            dest.State = src.State;
           return dest;
        }
    }
}
