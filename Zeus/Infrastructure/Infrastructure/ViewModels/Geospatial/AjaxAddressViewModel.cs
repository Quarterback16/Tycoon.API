using Employment.Web.Mvc.Infrastructure.Models.Geospatial;
using Employment.Web.Mvc.Infrastructure.Types.Geospatial;

namespace Employment.Web.Mvc.Infrastructure.ViewModels.Geospatial
{
    /// <summary>
    /// The Ajax View Model for <see cref="AddressModel"/>.
    /// </summary>
    public class AjaxAddressViewModel
    {
        /// <summary>
        /// Gets or sets the full address details as a single line.
        /// </summary>
        /// <value>
        /// The full address details as a single line.
        /// </value>
        public string SingleLineAddress { get; set; }

        /// <summary>
        /// Gets or sets the first line of the Address.
        /// </summary>
        /// <value>
        /// The first line of the Address.
        /// </value>
        public string Line1 { get; set; }

        /// <summary>
        /// Gets or sets the second line of the Address.
        /// </summary>
        /// <value>
        /// The second line of the Address.
        /// </value>
        public string Line2 { get; set; }

        /// <summary>
        /// Gets or sets the third line of the Address.
        /// </summary>
        /// <value>
        /// The third line of the Address.
        /// </value>
        public string Line3 { get; set; }

        /// <summary>
        /// Gets or sets the locality of the Address. The locality is the town, suburb or other locality name of the Address.
        /// </summary>
        /// <value>
        /// The locality of the Address.
        /// </value>
        public string Locality { get; set; }

        /// <summary>
        /// Whether to return Latitude, longitude (co-ordinate) information along with regions and Confidence level.
        /// </summary> 
        public bool ReturnLatLongDetails { get; set; }

        /// <summary>
        /// Latitude data.
        /// </summary> 
        public string Latitude { get; set; }

        /// <summary>
        /// Longitude data.
        /// </summary> 
        public string Longitude { get; set; }

        /// <summary>
        /// Gets or sets the state the Address is contained in.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the post code of the Address.
        /// </summary>
        /// <value>
        /// The post code.
        /// </value>
        public string Postcode { get; set; }

        /// <summary>
        /// Gets or sets the reliability of the Address.
        /// </summary>
        /// <value>
        /// The reliability of the Address.
        /// </value>
        public AddressReliability Reliability { get; set; }
    }
}
