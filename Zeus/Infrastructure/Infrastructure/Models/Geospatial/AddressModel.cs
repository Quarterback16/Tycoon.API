using System.Collections.Generic;
using Employment.Web.Mvc.Infrastructure.Types.Geospatial;

namespace Employment.Web.Mvc.Infrastructure.Models.Geospatial
{
    /// <summary>
    /// A model that represents an Address.
    /// </summary>
    public class AddressModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddressModel"/> class.
        /// </summary>
        public AddressModel()
        {
            Attributes = new List<AttributeModel>();
        }

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

        /// <summary>
        /// The Latitude of the Address
        /// </summary>
        public string Latitude { get; set; }

        /// <summary>
        /// The Longitude of the Address
        /// </summary>
        public string Longitude { get; set; }

        /// <summary>
        /// Gets or sets the attributes associated with the Address.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        public List<AttributeModel> Attributes { get; set; }

        /// <summary>
        /// Whether to return Latitude, longitude (co-ordinate) information.
        /// </summary>
        public bool ReturnLatLongDetails { get; set; }
    }
}
