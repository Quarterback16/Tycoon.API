
using System;

namespace Employment.Web.Mvc.Area.Example.Service.Interfaces
{

    /// <summary>
    /// Sort Model class.
    /// </summary>
    /// <remarks>
    /// This is for demonstration purpose only. Service models must exist in Service Project.
    /// </remarks>
    public class SortModel
    {

        /// <summary>
        /// Sorting ID.
        /// </summary>
        public long? SortingID { get; set; }


        /// <summary>
        /// Address property.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Date Value.
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Time Value.
        /// </summary>
        public DateTime? Time { get; set; }

        /// <summary>
        /// Phone number.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Duration Property.
        /// </summary>
        public int? Duration { get; set; }

        /// <summary>
        /// Multi-line Text.
        /// </summary>
        public string LargeTextArea { get; set; }

        /// <summary>
        /// Password Value.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Image URL.
        /// </summary>
        public string ImageUrl { get; set; }


        /// <summary>
        /// Html Content.
        /// </summary>
        public string HtmlContent { get; set; }
    }
}