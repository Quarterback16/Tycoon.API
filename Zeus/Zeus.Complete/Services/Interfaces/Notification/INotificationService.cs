using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Employment.Web.Mvc.Service.Interfaces.Notification
{
    /// <summary>
    /// Notification messages service
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Gets a list of Notification messages for a job seeker.
        /// </summary>
        /// <param name="jobseekerID">Requested Jobseeker ID.</param>
        /// <returns>A collection of <see cref="NotificationModel" /> for the jobseeker.</returns>
        IEnumerable<NotificationModel> GetList(long jobseekerID);

        /// <summary>
        /// Get the Notification letter details.
        /// </summary>
        /// <param name="jobseekerID">The requested jobseekerID.</param>
        /// <param name="printRequestID">The requested print request ID.</param>
        /// <returns>The letter lines.</returns>
        IEnumerable<string> GetNotificationLetterDetails(long jobseekerID, long printRequestID);

        long CreateSMS(SMSModel model);
    }
}

