using System;
using System.Collections.Generic;
using System.Text;
using Employment.Esc.Noticeboard.Contracts.DataContracts;
using Employment.Web.Mvc.Infrastructure.TypeConverters;
using Employment.Web.Mvc.Service.Interfaces.Noticeboard;

namespace Employment.Web.Mvc.Service.Implementation.Noticeboard
{
    /// <summary>
    /// Represents a mapper that is used to map between the Noticeboard domain models and the Noticeboard WCF DataContracts.
    /// </summary>
    public static class NoticeboardMapper
    {
        /// <summary>
        /// Convert to the message counts get request.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        public static MessageCountsGetRequest ToMessageCountsGetRequest(this SummaryModel src)
        {
            var dest = new MessageCountsGetRequest();
            dest.SiteCode = src.SearchCriteriaSiteCode;
            dest.JobseekerId = src.SearchCriteriaJobseekerID;
            dest.CreatedAfterDT = src.SearchCriteriaCreatedAfterDate.ToString("dMMMyyyy HH:mm:ss");
            dest.ManagedBy = src.SearchCriteriaManagedBy;
            return dest;
        }

        /// <summary>
        /// Convert to the summary model.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        public static SummaryModel ToSummaryModel(this MessageCountsGetResponse src)
        {
            var dest = new SummaryModel();
            dest.ManagedByUsers = src.ManagedByUsers;
            dest.MessageCounts = ConvertMessageCounts(src.MessageCounts);
            return dest;
        }

        /// <summary>
        /// Convert to the messages list request.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        public static MessagesListRequest ToMessagesListRequest(this ListModel src)
        {
            var dest = new MessagesListRequest();
            dest.MessageType = ConvertMessageTypes(src.SearchCriteriaMessageTypes);
            dest.SiteCode = src.SearchCriteriaSiteCode;
            dest.JobseekerId = src.SearchCriteriaJobseekerID;
            dest.CreatedAfterDT = src.SearchCriteriaCreatedAfterDate.ToString("dMMMyyyy HH:mm:ss");
            dest.ManagedBy = src.SearchCriteriaManagedBy;
            dest.BookmarkID = (int) (src.SearchCriteriaNextRecordID > 0 ? src.SearchCriteriaNextRecordID : 0); //TODO: why does wcf service have int32?
            dest.BookmarkDT = src.SearchCriteriaNextRecordID > 0 ? src.SearchCriteriaNextRecordDateTime.ToString("dMMMyyyy HH:mm:ss") : string.Empty;
            dest.GetManagedByUsers = false;
            return dest;
        }


        /// <summary>
        /// Convert to the list model.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        public static ListModel ToListModel(this MessagesListResponse src)
        {
            var dest = new ListModel();
            var list = new List<NoticeboardMessageModel>();
            if (src.Messages != null)
            {
                foreach (var s in src.Messages)
                {
                    list.Add(s.ToNoticeboardMessageModel());
                }
            }
            dest.MessagesList = list;
            dest.MessageCounts = ConvertMessageCounts(src.MessageCounts);
            return dest;
        }

        /// <summary>
        /// Convert to the message appointments list request.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        public static MessageAppointmentsListRequest ToMessageAppointmentsListRequest(this ListModel src)
        {
            var dest = new MessageAppointmentsListRequest();
            dest.MessageType = ConvertMessageTypes(src.SearchCriteriaMessageTypes);
            dest.SiteCode = src.SearchCriteriaSiteCode;
            dest.JobseekerId = src.SearchCriteriaJobseekerID;
            dest.CreatedAfterDT = src.SearchCriteriaCreatedAfterDate.ToString("dMMMyyyy HH:mm:ss");
            dest.ManagedBy = src.SearchCriteriaManagedBy;
            dest.BookmarkID = (int)(src.SearchCriteriaNextRecordID > 0 ? src.SearchCriteriaNextRecordID : 0);//TODO: why does wcf service have int32?
            dest.BookmarkDT = src.SearchCriteriaNextRecordID > 0 ? src.SearchCriteriaNextRecordDateTime.ToString("dMMMyyyy HH:mm:ss") : string.Empty;
            dest.GetManagedByUsers = false;
            return dest;
        }

        /// <summary>
        /// Convert to the list model.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        public static ListModel ToListModel(this MessageAppointmentsListResponse src)
        {
            var dest = new ListModel();
            var list = new List<NoticeboardAppointmentModel>();
            if (src.MessageAppointments != null)
            {
                foreach (var s in src.MessageAppointments)
                {
                    list.Add(s.ToNoticeboardAppointmentModel());
                }
            }
            dest.AppointmentsList = list;

            dest.MessageCounts = ConvertMessageCounts(src.MessageCounts);
            return dest;
        }

        /// <summary>
        /// Convert to the noticeboard message model.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        public static NoticeboardMessageModel ToNoticeboardMessageModel(this MessageData src)
        {
            var dest = new NoticeboardMessageModel();
            dest.MessageID = src.MessageId;
            dest.MessageType = src.MessageType;
            dest.MessageDate = DateTimeTypeConverter.Convert(src.MessageDate);
            dest.MessageStartDate = DateTimeTypeConverter.Convert(src.MessageStartDate);
            dest.MessageEndDate = DateTimeTypeConverter.Convert(src.MessageEndDate);
            dest.MessageContent = src.MessageContent;
            dest.MessageContentParts = new List<string>(src.MessageContent.Split(','));
            dest.JobseekerID = src.JobseekerId;
            dest.JobseekerFirstName = src.FirstName;
            dest.JobseekerLastName = src.LastName;
            dest.ManagedBy = src.ManagedBy;
            dest.MessageReasonADWTable = src.MessageReasonAdwTable;
            dest.MessageReasonCode = src.MessageReason;
            dest.MessageReasonDescription = src.MessageReasonDescription;
            dest.AppointmentID = src.AppointmentID;
            dest.AppointmentDate = NullDateTimeTypeConverter.Convert(src.AppointmentDate);
            dest.AppointmentResultCode = src.AppointmentResult;
            dest.AppointmentReasonCode = src.AppointmentReason;
            dest.AppointmentConsultantID = src.ConsultantID;
            dest.AppointmentConsultantText = src.ConsultantText;
            return dest;
        }

        /// <summary>
        /// Convert to the noticeboard appointment model.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        public static NoticeboardAppointmentModel ToNoticeboardAppointmentModel(this MessageAppointmentData src)
        {
            var dest = new NoticeboardAppointmentModel();
            dest.AppointmentID = src.AppointmentId;
            dest.JobseekerID = src.JobseekerId;
            dest.JobseekerFirstName = src.FirstName;
            dest.JobseekerLastName = src.LastName;
            dest.StartDateTime = DateTimeTypeConverter.Convert(src.StartDateTime);
            dest.EndDateTime = DateTimeTypeConverter.Convert(src.EndDateTime);
            dest.Breach = src.Breach;
            dest.ConsultantID = src.ConsultantID;
            dest.ConsultantText = src.ConsultantText;
            dest.CreationDate = DateTimeTypeConverter.Convert(src.CreatedOnDateTime);
            dest.CreationSiteCode = src.CreationSite;
            dest.EligibilityCode = src.EligibilityCode;
            dest.InterpreterCode = src.InterpreterCode;
            dest.Location = src.Location;
            dest.ReasonCode = src.ReasonCode;
            dest.ResultCode = src.ResultCode;
            dest.ResultReason = src.ResultReason;
            dest.SessionTypeCode = src.SessionType;
            dest.SpecialNeedsIndicator = src.SpecialNeedsIndicator;
            dest.SpecialRequirements = src.SpecialRequirements;
            return dest;
        }

        
        private static string ConvertMessageTypes(List<string> messageTypes)
        {
            if (messageTypes == null) return string.Empty;

            StringBuilder sb = new StringBuilder();
            foreach (var messageType in messageTypes)
            {
                if (!String.IsNullOrEmpty(messageType))
                {
                    if (sb.Length <= 0)
                    {
                        sb.Append(',');
                    }
                    sb.Append(messageType);
                }
            }

            return sb.ToString();
        }

        private static Dictionary<string, int> ConvertMessageCounts(List<MessageCountData> messageCountData)
        {
            var dictionary = new Dictionary<string, int>();
            if (messageCountData == null) return dictionary;

            foreach (var item in messageCountData)
            {
                if (!dictionary.ContainsKey(item.MessageType))
                {
                    dictionary.Add(item.MessageType, item.Count);
                }
            }

            return dictionary;
        }

    }
}
