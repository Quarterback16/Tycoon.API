using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Employment.Web.Mvc.Service.Interfaces.Noticeboard
{
    /// <summary>
    /// Get summary model class.
    /// </summary>
    [Serializable]
    public class SummaryModel
    {
        /// <summary>
        /// Search criteria - messages for site code 
        /// </summary>
        public string SearchCriteriaSiteCode { get; set; }

        /// <summary>
        /// Search criteria - messages for jobseeker ID
        /// </summary>
        public long SearchCriteriaJobseekerID { get; set; }

        /// <summary>
        /// Search criteria - messages created after date
        /// </summary>
        public DateTime SearchCriteriaCreatedAfterDate { get; set; }

        /// <summary>
        /// Search criteria - messages managed by user ID
        /// </summary>
        public string SearchCriteriaManagedBy { get; set; }

        /// <summary>
        /// Managed by user ID values for all noticeboard messages for search criteria
        /// </summary>
        public List<string> ManagedByUsers { get; set; }

        /// <summary>
        /// Total counts of all noticeboard messages for search criteria 
        /// </summary>
        public Dictionary<string, int> MessageCounts { get; set; }
    }
}
