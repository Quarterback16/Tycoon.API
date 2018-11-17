using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Employment.Web.Mvc.Service.Interfaces.Noticeboard
{
    /// <summary>
    /// Get list model class.
    /// </summary>
    public class ListModel
    {
        public ListModel()
        {
            SearchCriteriaNextRecordID = -1;
            SearchCriteriaNextRecordDateTime = DateTime.MinValue;
        }

        /// <summary>
        /// Search criteria - message types
        /// </summary>
        public List<string> SearchCriteriaMessageTypes { get; set; }

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
        /// Search criteria - next record ID
        /// </summary>
        public long SearchCriteriaNextRecordID { get; set; }

        /// <summary>
        /// Search criteria - next record datetime
        /// </summary>
        public DateTime SearchCriteriaNextRecordDateTime { get; set; }

        /// <summary>
        /// List of appointments (when <see cref="SearchCriteriaMessageTypes" /> is NAATDY or NARDUE, otherwise null)
        /// </summary>
        public IEnumerable<NoticeboardAppointmentModel> AppointmentsList { get; set; }

        /// <summary>
        /// List of messages (when <see cref="SearchCriteriaMessageTypes" /> is not NAATDY or NARDUE, otherwise null)
        /// </summary>
        public IEnumerable<NoticeboardMessageModel> MessagesList { get; set; }

        /// <summary>
        /// Total counts of all noticeboard messages for given search criteria
        /// </summary>
        public Dictionary<string, int> MessageCounts { get; set; }
    }
}
