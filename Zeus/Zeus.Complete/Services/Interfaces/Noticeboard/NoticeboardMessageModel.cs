using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Employment.Web.Mvc.Service.Interfaces.Noticeboard
{
    /// <summary>
    /// Noticeboard message model class.
    /// </summary>
    public class NoticeboardMessageModel
    {
        /// <summary>
        /// Message ID
        /// </summary>
        public int MessageID { get; set; }

        /// <summary>
        /// Message type
        /// </summary>
        public string MessageType { get; set; }

        /// <summary>
        /// Message date
        /// </summary>
        public DateTime MessageDate { get; set; }

        /// <summary>
        /// Message start date
        /// </summary>
        public DateTime MessageStartDate { get; set; }

        /// <summary>
        /// Message end date
        /// </summary>
        public DateTime MessageEndDate { get; set; }

        /// <summary>
        /// Number of days from message start date to today's date
        /// </summary>
        public int MessageDaysDisplayed { get; set; }

        /// <summary>
        /// Number of days from today's date to message end date
        /// </summary>
        public int MessageDaysToGo { get; set; }

        /// <summary>
        /// Message jobseeker's ID
        /// </summary>
        public long JobseekerID { get; set; }

        /// <summary>
        /// Message jobseeker's first name
        /// </summary>
        public string JobseekerFirstName { get; set; }

        /// <summary>
        /// Message jobseeker's last name
        /// </summary>
        public string JobseekerLastName { get; set; }

        /// <summary>
        /// Message managed by user ID
        /// </summary>
        public string ManagedBy { get; set; }

        /// <summary>
        /// ADW table to use for <see cref="MessageReasonCode" />
        /// </summary>
        public string MessageReasonADWTable { get; set; }

        /// <summary>
        /// Message reason code
        /// </summary>
        public string MessageReasonCode { get; set; }

        /// <summary>
        /// Message reason description (using MessageReasonADWTable)
        /// </summary>
        public string MessageReasonDescription { get; set; }

        /// <summary>
        /// Message content
        /// </summary>
        public string MessageContent { get; set; }

        /// <summary>
        /// <see cref="MessageContent" /> split into comma delimited parts
        /// </summary>
        public List<string> MessageContentParts { get; set; }

        /// <summary>
        /// Appointment ID
        /// </summary>
        public long AppointmentID { get; set; }

        /// <summary>
        /// Appointment date
        /// </summary>
        public DateTime? AppointmentDate { get; set; }

        /// <summary>
        /// Appointment result code
        /// </summary>
        public string AppointmentResultCode { get; set; }

        /// <summary>
        /// Appointment reason code
        /// </summary>
        public string AppointmentReasonCode { get; set; }
        
        /// <summary>
        /// Appointment consultant ID
        /// </summary>
        public string AppointmentConsultantID { get; set; }

        /// <summary>
        /// Appointment consultant name
        /// </summary>
        public string AppointmentConsultantText { get; set; }
    }
}
