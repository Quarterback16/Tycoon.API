
namespace Employment.Web.Mvc.Service.Interfaces.Common
{
    /// <summary>
    /// Model for phone number
    /// </summary>
    public class PhoneNumberModel
    {
        /// <summary>
        /// Area code
        /// </summary>
        public string AreaCode { get; set; }

        /// <summary>
        /// Phone number
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Silent number indicator
        /// </summary>
        public bool Silent{ get; set; }

        /// <summary>
        /// Type of this phone number, home, mobile, fax etc.
        /// </summary>
        public PhoneType Type { get; set; }

        /// <summary>
        /// Enum for adw phone type.
        /// </summary>
        public enum PhoneType
        {
            /// <summary>
            /// Home
            /// </summary> 
            Home,

            /// <summary>
            /// Mobile
            /// </summary>
            Mobile,

            /// <summary>
            /// Fax
            /// </summary>
            Fax,

            /// <summary>
            /// Work
            /// </summary>
            Work
        }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the subscriber status.
        /// </summary>
        /// <value>The subscriber status.</value>
        public string SubscriberStatus { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>The start date.</value>
        public System.DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [remove flag].
        /// </summary>
        /// <value><c>true</c> if [remove flag]; otherwise, <c>false</c>.</value>
        public bool RemoveFlag { get; set; }

        /// <summary>
        /// Gets or sets the seq num.
        /// </summary>
        /// <value>The seq num.</value>
        public virtual long SeqNum { get; set; }
        
        /// <summary>
        /// Gets or sets the phone id.
        /// </summary>
        /// <value>The phone id.</value>
        public virtual long PhoneId { get; set; }

        /// <summary>
        /// Gets or sets the integrity control number JPD.
        /// </summary>
        /// <value>The integrity control number JPD.</value>
        public long IntegrityControlNumberJPD { get; set; }

        public string IsMarketResearch { get; set; }

        public string IsPreferred { get; set; }

        public string IsSilentNumber { get; set; }
    }
}
