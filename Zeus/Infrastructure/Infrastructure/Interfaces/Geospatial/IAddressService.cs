using System.Collections.Generic;
using Employment.Web.Mvc.Infrastructure.Models.Geospatial;

namespace Employment.Web.Mvc.Infrastructure.Interfaces.Geospatial
{
    /// <summary>
    /// Defines the methods and properties that are required for an Address Service.
    /// </summary>
    public interface IAddressService
    {
        /// <summary>
        /// Validates the address using Geocoding.
        /// </summary>
        /// <param name="address">The address to be validated.</param>
        /// <returns>Matching addresses with reliability values.</returns>
        IEnumerable<AddressModel> Validate(AddressModel address);

        /// <summary>
        /// Validates the address using Geocoding.
        /// </summary>
        /// <param name="address">The address to be validated.</param>
        /// <param name="returnLatLongInfo">Whether to return lat/long info.</param>
        /// <returns>Matching addresses with reliability values.</returns>
        IEnumerable<AddressModel> Validate(string address, bool returnLatLongInfo = false);

        /// <summary>
        /// Gets extended attributes associated with the address. Extended attributes are things such as
        /// region names and other data associated with an address stored in the Geocoding system.
        /// </summary>
        /// <param name="address">The addresses.</param>
        /// <returns>The Address with its attributes.</returns>
        AddressModel GetAttributes(AddressModel address);

        /// <summary>
        /// Gets extended attributes associated with the addresses. Extended attributes are things such as
        /// region names and other data associated with an address stored in the Geocoding system.
        /// </summary>
        /// <param name="addresses">The addresses.</param>
        /// <returns>Addresses with their attributes.</returns>
        IEnumerable<AddressModel> GetAttributes(IEnumerable<AddressModel> addresses);
    }
}
