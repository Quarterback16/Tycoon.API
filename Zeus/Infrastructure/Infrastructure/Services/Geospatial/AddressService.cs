using System.Collections.Generic;
using System.Linq;

using Department.AddressValidation;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Interfaces.Geospatial;
using Employment.Web.Mvc.Infrastructure.Mappers;
using Employment.Web.Mvc.Infrastructure.Models.Geospatial;

namespace Employment.Web.Mvc.Infrastructure.Services.Geospatial
{
    /// <summary>
    /// The implementation of the Address Service.
    /// </summary>
    public class AddressService : Service, IAddressService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddressService"/> class.
        /// </summary>
        /// <param name="client">Client for interacting with WCF services.</param>
        /// <param name="cacheService">Cache service for interacting with cached data.</param>
        public AddressService(IClient client,  ICacheService cacheService) 
            : base (client,  cacheService)
        {
        }

        /// <summary>
        /// Validates the address using Geocoding.
        /// </summary>
        /// <param name="address">The address to be validated.</param>
        /// <returns>Matching addresses with reliability values.</returns>
        public IEnumerable<AddressModel> Validate(AddressModel address)
        {
            var query = new Address(address.Line1, address.Line2, address.Line3, address.Locality, address.State,
                                    address.Postcode);

            var addressValidator = new AddressValidator();
            var result = addressValidator.ValidateAddress(query, DatabaseType.PAF);
            return (result).ToAddressModel();
        }

        /// <summary>
        /// Validates the address using Geocoding.
        /// </summary>
        /// <param name="address">The address to be validated.</param>
        /// <param name="returnLatLongInfo">Whether to return lat/long info.</param>
        /// <returns>Matching addresses with reliability values.</returns>
        public IEnumerable<AddressModel> Validate(string address, bool returnLatLongInfo = false)
        {
            var addressValidator = new AddressValidator();
            var result = addressValidator.ValidateAddress(address, returnLatLongInfo ? DatabaseType.GNAF : DatabaseType.PAF);
            return (result).ToAddressModel();
        }

        /// <summary>
        /// Gets extended attributes associated with the address. Extended attributes are things such as
        /// region names and other data associated with an address stored in the Geocoding system.
        /// </summary>
        /// <param name="address">The addresses.</param>
        /// <returns>The Address with its attributes.</returns>
        public AddressModel GetAttributes(AddressModel address)
        {
            var result = GetAttributes(new[] {address});
            return result.FirstOrDefault();
        }

        /// <summary>
        /// Gets extended attributes associated with the addresses. Extended attributes are things such as
        /// region names and other data associated with an address stored in the Geocoding system.
        /// </summary>
        /// <param name="addresses">The addresses.</param>
        /// <returns>Adresses with their attributes.</returns>
        public IEnumerable<AddressModel> GetAttributes(IEnumerable<AddressModel> addresses)
        {
            var query = addresses.Select(address => new Address(address.Line1, address.Line2, address.Line3, address.Locality, address.State, address.Postcode)).ToArray();
            var addressValidator = new AddressValidator();
            var result = addressValidator.AssignRegionsToAddresses(query);
            return (result).ToAddressModel();
        }
    }
}
