using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Employment.Web.Mvc.Service.Interfaces.Notification
{
    /// <summary>
    /// Notification model class.
    /// </summary>
    public class NotificationModel
    {
        public int ListItemID { get; set; }

        public long JobseekerID { get; set; }

        public string MessageSource { get; set; }

        public long MessageID { get; set; }

        public string MessageTypeCode { get; set; }

        public string StatusCode { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateSent { get; set; }

        public string UserID { get; set; }

        public long PrintRequestID { get; set; }

        public string AppointmentDate { get; set; }

        public string AppointmentTime { get; set; }

        public string AppointmentDay { get; set; }

        public string AppointmentAddress { get; set; }

        public string AppointmentContactName { get; set; }

        public string AppointmentContactPhone { get; set; }

        public string JobseekerEmail { get; set; }

        public string JobseekerMobilePhone { get; set; }

        public string Subject { get; set; }

        public int DeliveryMethod { get; set; }

        public string Message { get; set; }

        public long TimeStamp { get; set; }
    }
}
