using System;

namespace Employment.Web.Mvc.Service.Interfaces.Common
{
    /// <summary>
    /// Details about details item
    /// </summary>
    public abstract class UpdateDetailsModel
    {
        /// <summary>
        /// The date the item was updated
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// The user that updated the item
        /// </summary>
        public string UpdateUser { get; set; }
    }
}
