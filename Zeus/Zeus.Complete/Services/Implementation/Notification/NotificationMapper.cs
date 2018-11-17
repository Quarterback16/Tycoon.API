using System;
using System.Collections.Generic;
using System.Text;
using Employment.Esc.Notification.Contracts.DataContracts;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.TypeConverters;
using Employment.Web.Mvc.Service.Interfaces.Notification;

namespace Employment.Web.Mvc.Service.Implementation.Notification
{
    /// <summary>
    /// Represents a mapper that is used to map between the Notification domain models and the Notification WCF DataContracts.
    /// </summary>
    public static class NotificationMapper
    {
        /// <summary>
        /// Convert to the notification model list.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        public static List<NotificationModel> ToNotificationModelList(this requestedItem[] src)
        {
            List<NotificationModel> dest = new List<NotificationModel>();
            if (src != null)
            {
                foreach (var s in src)
                {
                    dest.Add(s.ToNotificationModel());
                }
            }
            return dest;
        }

        /// <summary>
        /// Convert to the notification model.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        public static NotificationModel ToNotificationModel(this requestedItem src)
        {
            var dest = new NotificationModel();
            dest.MessageSource = "MF";
            dest.MessageTypeCode = src.messageType;
            dest.StatusCode = src.statusCode;
            dest.DateCreated = src.createDate;
            dest.DateSent = src.sendDate;
            dest.UserID = src.createUserId;
            dest.PrintRequestID = src.printRequestId;
            return dest;
        }


        /// <summary>
        /// Convert to the notification model list.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        public static List<NotificationModel> ToNotificationModelList(this sqlList[] src)
        {
            List<NotificationModel> dest = new List<NotificationModel>();
            if (src != null)
            {
                foreach (var s in src)
                {
                    dest.Add(s.ToNotificationModel());
                }
            }
            return dest;
        }

        /// <summary>
        /// Convert to the notification model.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        public static NotificationModel ToNotificationModel(this sqlList src)
        {
            var dest = new NotificationModel();
            dest.MessageSource = src.messageSource;
            dest.MessageID = src.messageId;
            dest.MessageTypeCode = src.messageType;
            dest.StatusCode = "Sent";
            dest.DateCreated = DateTimeTypeConverter.Convert(src.createDate);
            dest.DateSent = DateTimeTypeConverter.Convert(src.messageDate);
            dest.UserID = src.userId;
            dest.AppointmentDate = src.appDate;
            dest.AppointmentTime = src.appTime;
            dest.AppointmentDay = src.appDay;
            dest.AppointmentAddress = src.appAddress;
            dest.AppointmentContactName = src.appContactName;
            dest.AppointmentContactPhone = src.appContactPhone;
            dest.JobseekerEmail = src.jsEmail;
            dest.JobseekerMobilePhone = src.jsSMS;
            dest.Subject = src.subject;
            dest.DeliveryMethod = src.deliveryMethod;
            dest.Message = src.message;
            dest.TimeStamp = src.timeStamp;
            return dest;
        }




        /// <summary>
        /// Convert to the SMSJNM message request.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        public static SMSJNMMessageRequest ToSMSJNMMessageRequest(this SMSModel src)
        {
            var dest = new SMSJNMMessageRequest();
            dest.jobseekerId = src.JobseekerID;
            dest.subjectArea = src.ContractType;
            dest.smsPhone = src.Phone;
            dest.smsMessage = src.Message;
            return dest;
        }




    }
}
