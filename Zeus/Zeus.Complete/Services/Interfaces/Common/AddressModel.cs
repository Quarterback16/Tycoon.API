namespace Employment.Web.Mvc.Service.Interfaces.Common
{
    /// <summary>
    /// A model representing address information
    /// </summary>
    public class AddressModel
    {
        /// <summary>
        /// First line of address
        /// </summary>
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Second line of address
        /// </summary>
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Thrid line of address
        /// </summary>
        public string AddressLine3 { get; set; }

        /// <summary>
        /// Suburb
        /// </summary>
        public string Suburb { get; set; }

        /// <summary>
        /// State
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Postcode
        /// </summary>
        public string Postcode { get; set; }

        /// <summary>
        /// Community code
        /// </summary>
        public string Community { get; set; }

        /// <summary>
        /// The type of this address, residential, postal , etc
        /// </summary>
        public AddressType Type { get; set; }

        /// <summary>
        /// enum for adw table _ address type
        /// </summary>
        public enum AddressType
        {
            /// <summary>
            /// Postal
            /// </summary>
            Postal,

            /// <summary>
            /// Residential
            /// </summary>
            Residential,
            /// <summary>
            /// RJCP Nominee
            /// </summary>
            RJCPNominee,
            /// <summary>
            /// DHS Nominee
            /// </summary>
            DHSNominee

        }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>The start date.</value>
        public System.DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the seq num.
        /// </summary>
        /// <value>The seq num.</value>
        public long SeqNum { get; set; }

        /// <summary>
        /// Gets or sets the seq num.
        /// </summary>
        /// <value>The seq num.</value>
        public long IntegrityControlNumberJSA { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [remove flag].
        /// </summary>
        /// <value><c>true</c> if [remove flag]; otherwise, <c>false</c>.</value>
        public bool RemoveFlag { get; set; }
    }
}
