using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Employment.Web.Mvc.Service.Interfaces.Noticeboard
{
    /// <summary>
    /// Noticeboard messages service
    /// </summary>
    public interface INoticeboardService
    {
        /// <summary>
        /// Gets summary details for all noticeboard message types.
        /// </summary>
        /// <param name="model">A <see cref="SummaryModel" /> with request data.</param>
        /// <returns>A <see cref="SummaryModel" /> with request and response data.</returns>
        SummaryModel GetSummary(SummaryModel model);

        /// <summary>
        /// Gets a list of noticeboard messages or a list of appointments (for message types of NAATDY or NARDUE).
        /// </summary>
        /// <param name="model">A <see cref="ListModel" /> with request data.</param>
        /// <returns>A <see cref="ListModel" /> with request and response data.</returns>
        ListModel GetList(ListModel model);

        /// <summary>
        /// Get summary details only for specified type (From Adw Table)
        /// </summary>
        /// <param name="siteCode">Users site code.</param>
        /// <param name="dateTime">Users date time.</param>
        /// <returns>Noticeboard messages.</returns>
        IEnumerable<MiniNoticeboardModel> GetSpecificMessages(string siteCode, DateTime dateTime);

        //void ClearSpecificMessagesCache(string siteCode, DateTime adateTime);
        
    }
}
