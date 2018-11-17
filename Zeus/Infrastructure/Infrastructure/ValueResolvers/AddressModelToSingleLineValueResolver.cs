using Employment.Web.Mvc.Infrastructure.Models.Geospatial;

namespace Employment.Web.Mvc.Infrastructure.ValueResolvers
{
    /// <summary>
    /// Defines a value resolver that returns a single line address based on a <see cref="AddressModel" />.
    /// </summary>
    public static class AddressModelToSingleLineValueResolver 
    {
        /// <summary>
        /// Resolves a single line address from a <see cref="AddressModel" />..
        /// </summary>
        /// <returns>A single line address.</returns>
        public static string Resolve(AddressModel source) {
            if (string.IsNullOrWhiteSpace(source.Line1)) {
                return string.Empty;
            }

            if (string.IsNullOrWhiteSpace(source.Line2)) {
                return string.Format("{0} {1} {2} {3}", source.Line1, source.Locality, source.State, source.Postcode);
            }

            if (string.IsNullOrWhiteSpace(source.Line3)) {
                return string.Format("{0} {1} {2} {3} {4}", source.Line1, source.Line2, source.Locality, source.State, source.Postcode);
            }

            if (source.ReturnLatLongDetails)
            {
                return string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8}", source.Line1, source.Line2, source.Line3, source.Locality, source.State, source.Postcode, source.Latitude, source.Longitude, source.Reliability);
            }
            
            return string.Format("{0} {1} {2} {3} {4} {5}", source.Line1, source.Line2, source.Line3, source.Locality, source.State, source.Postcode);
        }
    } 
}


