using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Employment.Esc.Noticeboard.Contracts.DataContracts;
using Employment.Esc.Noticeboard.Contracts.ServiceContracts;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Service.Interfaces.Noticeboard;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using System.Text;
using Employment.Web.Mvc.Infrastructure.Models;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Service.Implementation.Noticeboard
{
    /// <summary>
    /// Defines a service for interacting with Noticeboard messages
    /// </summary>
    public class NoticeboardService : Infrastructure.Services.Service, INoticeboardService
    {
        protected readonly IAdwService AdwService;
       
        /// <summary>
        /// Noticeboard Service to Get Messages and its count
        /// </summary>
        /// <param name="adwService"></param>
        /// <param name="client"></param>
        /// <param name="cacheService"></param>
        public NoticeboardService(IAdwService adwService, IClient client,  ICacheService cacheService)
            : base(client,  cacheService)         
        {
            if (adwService == null)
            {
                throw new ArgumentNullException("adwService");
            }

            AdwService = adwService;
                         
        }

        /// <summary>
        /// Gets summary details for all noticeboard message types.
        /// </summary>
        /// <param name="model">A <see cref="SummaryModel" /> with request data.</param>
        /// <returns>A <see cref="SummaryModel" /> with request and response data.</returns>
        public SummaryModel GetSummary(SummaryModel model)
        {
            var currentDateTime = UserService.DateTime;

            var request = model.ToMessageCountsGetRequest();//    MappingEngine.Map<MessageCountsGetRequest>(model);

            request.RequestDT = currentDateTime.ToString("dMMMyyyy HH:mm:ss");
            //request.SiteCode = UserService.SiteCode;

            ValidateRequest(request);

            try
            {
                var service = Client.Create<INoticeboardMessages>("NoticeboardMessages.svc");

                var response = service.MessageCountsGet(request);

                var summaryModel = response.ToSummaryModel();//   MappingEngine.Map<SummaryModel>(response);

                summaryModel.SearchCriteriaSiteCode = request.SiteCode;
                summaryModel.SearchCriteriaJobseekerID = model.SearchCriteriaJobseekerID;
                summaryModel.SearchCriteriaCreatedAfterDate = model.SearchCriteriaCreatedAfterDate;
                summaryModel.SearchCriteriaManagedBy = model.SearchCriteriaManagedBy;

                return summaryModel;
            }
            catch (FaultException<ValidationFault> vf)
            {
                throw vf.ToServiceValidationException();
            }
            catch (FaultException fe)
            {
                throw fe.ToServiceValidationException();
            }
        }

        /// <summary>
        /// Gets a list of noticeboard messages or a list of appointments (for message types of NAATDY or NARDUE).
        /// </summary>
        /// <param name="model">A <see cref="ListModel" /> with request data.</param>
        /// <returns>A <see cref="ListModel" /> with request and response data.</returns>
        public ListModel GetList(ListModel model)
        {
            // TODO set up mainframe paging

            if (model.SearchCriteriaMessageTypes.Contains("NAATDY") ||
                model.SearchCriteriaMessageTypes.Contains("NARDUE"))
            {
                return GetAppointmentList(model);
            }
            else
            {
                return GetMessagesList(model);
            }
        }

        /// <summary>
        /// Gets a list of appointments (for message types of NAATDY or NARDUE).
        /// </summary>
        /// <param name="model">A <see cref="ListModel" /> with request data.</param>
        /// <returns>A <see cref="ListModel" /> with request and response data.</returns>
        private ListModel GetAppointmentList(ListModel model)
        {
            var currentDateTime = UserService.DateTime;

            var request = model.ToMessageAppointmentsListRequest();//   MappingEngine.Map<MessageAppointmentsListRequest>(model);

            request.RequestDT = currentDateTime.ToString("dMMMyyyy HH:mm:ss");
            //request.SiteCode = UserService.SiteCode;

            ValidateRequest(request);

            try
            {
                var service = Client.Create<INoticeboardMessages>("NoticeboardMessages.svc");

                var response = service.MessageAppointmentsList(request);

                var nextRecordID = -1;
                var nextRecordDateTime = DateTime.MinValue;

                if (response != null && response.MessageAppointments != null && response.MessageAppointments.Count > Constants.MaxNumRecords)
                {
                    nextRecordID = response.MessageAppointments[Constants.MaxNumRecords - 1].AppointmentId;
                    DateTime.TryParse(response.MessageAppointments[Constants.MaxNumRecords - 1].StartDateTime, out nextRecordDateTime);
                    response.MessageAppointments.RemoveRange(Constants.MaxNumRecords, response.MessageAppointments.Count - Constants.MaxNumRecords);
                }

                var listModel = response.ToListModel(); //MappingEngine.Map<ListModel>(response);

                listModel.SearchCriteriaSiteCode = request.SiteCode;
                listModel.SearchCriteriaMessageTypes = model.SearchCriteriaMessageTypes;
                listModel.SearchCriteriaJobseekerID = model.SearchCriteriaJobseekerID;
                listModel.SearchCriteriaCreatedAfterDate = model.SearchCriteriaCreatedAfterDate;
                listModel.SearchCriteriaManagedBy = model.SearchCriteriaManagedBy;
                listModel.SearchCriteriaNextRecordID = nextRecordID;
                listModel.SearchCriteriaNextRecordDateTime = nextRecordDateTime;

                return listModel;
            }
            catch (FaultException<ValidationFault> vf)
            {
                throw vf.ToServiceValidationException();
            }
            catch (FaultException fe)
            {
                throw fe.ToServiceValidationException();
            }
        }

        /// <summary>
        /// Gets a list of noticeboard messages.
        /// </summary>
        /// <param name="model">A <see cref="ListModel" /> with request data.</param>
        /// <returns>A <see cref="ListModel" /> with request and response data.</returns>
        private ListModel GetMessagesList(ListModel model)
        {
            var currentDateTime = UserService.DateTime;

            var request = model.ToMessagesListRequest();//   MappingEngine.Map<MessagesListRequest>(model);

            request.RequestDT = currentDateTime.ToString("dMMMyyyy HH:mm:ss");
            //request.SiteCode = UserService.SiteCode;

            ValidateRequest(request);

            try
            {
                var service = Client.Create<INoticeboardMessages>("NoticeboardMessages.svc");

                var response = service.MessagesList(request);

                var nextRecordID = -1;
                var nextRecordDateTime = DateTime.MinValue;

                if (response != null && response.Messages != null && response.Messages.Count > Constants.MaxNumRecords)
                {
                    nextRecordID = response.Messages[Constants.MaxNumRecords - 1].MessageId;
                    DateTime.TryParse(response.Messages[Constants.MaxNumRecords - 1].MessageDate, out nextRecordDateTime);
                    response.Messages.RemoveRange(Constants.MaxNumRecords, response.Messages.Count - Constants.MaxNumRecords);
                }

                var listModel = response.ToListModel();//    MappingEngine.Map<ListModel>(response);

                listModel.SearchCriteriaSiteCode = request.SiteCode;
                listModel.SearchCriteriaMessageTypes = model.SearchCriteriaMessageTypes;
                listModel.SearchCriteriaJobseekerID = model.SearchCriteriaJobseekerID;
                listModel.SearchCriteriaCreatedAfterDate = model.SearchCriteriaCreatedAfterDate;
                listModel.SearchCriteriaManagedBy = model.SearchCriteriaManagedBy;
                listModel.SearchCriteriaNextRecordID = nextRecordID;
                listModel.SearchCriteriaNextRecordDateTime = nextRecordDateTime;

                foreach (var message in listModel.MessagesList)
                {
                    message.MessageDaysDisplayed = (currentDateTime.Date.Subtract(message.MessageStartDate.Date)).Days;
                    message.MessageDaysToGo = message.MessageEndDate.Date.Subtract(currentDateTime.Date).Days - 1;
                }

                return listModel;
            }
            catch (FaultException<ValidationFault> vf)
            {
                throw vf.ToServiceValidationException();
            }
            catch (FaultException fe)
            {
                throw fe.ToServiceValidationException();
            }
        }

        /// <summary>
        /// Gets a list of noticeboard messages from "MNB"
        /// </summary>
        /// <param name="siteCode">Users site code.</param>
        /// <param name="dateTime">Users date time.</param>
        /// <returns>Noticeboard messages.</returns>
        public IEnumerable<MiniNoticeboardModel> GetSpecificMessages(string siteCode, DateTime dateTime)
        {
            IEnumerable<MiniNoticeboardModel> messages;
            var keymodel = new KeyModel("GetSpecificMessages").Add(siteCode).Add(dateTime.Date);
            if (!CacheService.TryGet(keymodel, out messages))
            {
                var request = new MessageCountsGetRequest
                {
                    RequestDT = dateTime.ToString("dMMMyyyy HH:mm:ss"),
                    SiteCode = siteCode,
                    MessageTypes = ConvertMessageTypes(GetMessageTypesFromMNB())
                };
                ValidateRequest(request);

                try
                {
                    var service = Client.Create<INoticeboardMessages>("NoticeboardMessages.svc");

                    var response = service.MessageCountsGet(request);

                    var miniNoticeboardModel = GetDescfromAdw(response.MessageCounts).ToList();

                    CacheService.Set(keymodel, miniNoticeboardModel, new TimeSpan(0, 0, 5, 0));

                    return miniNoticeboardModel;
                }
                catch (FaultException<ValidationFault> vf)
                {
                    throw vf.ToServiceValidationException();
                }
                catch (FaultException fe)
                {
                    throw fe.ToServiceValidationException();
                }
            }
            return messages;
        }
            

        #region Request MiniNoticeboard

        private List<string> GetMessageTypesFromMNB()
        {
            var messageTypes = new List<string>();
            var itemsMNB = AdwService.GetListCodes("MNB");
            foreach (var i in itemsMNB)
            {
                var msgtypes = AdwService.GetRelatedCodes("UMGT", i.Code, true);
                foreach (var m in msgtypes)
                {
                    messageTypes.Add(m.SubordinateCode);
                }
            }
            return messageTypes;
        }

        private string ConvertMessageTypes(List<string> messageTypes)
        {
            if (messageTypes == null) return string.Empty;

            var sb = new StringBuilder();

            foreach (var messageType in messageTypes)
            {
                if (!String.IsNullOrEmpty(messageType))
                {
                    if (!String.IsNullOrEmpty(sb.ToString()))
                    {
                        sb.Append(",");
                    }
                    sb.Append(messageType);
                }
            }

            return sb.ToString();
        }

        #endregion

        #region Response MiniNoticeboard

        private IEnumerable<MiniNoticeboardModel> GetDescfromAdw(List<MessageCountData> messageCountData)
        {
            var list = new List<MiniNoticeboardModel>();
            var listMNB = AdwService.GetListCodes("MNB");
            var codes = AdwService.GetListCodes("MG1");

            foreach (var i in listMNB)
            {
                list.Add(new MiniNoticeboardModel
                {
                    Description = codes.First(d => d.Code == i.Code).Description,
                    MessageCounts = GetMessageCount(i.Code, ConvertMessageCounts(messageCountData)),
                    MessageGroupLevel1Code = i.Code
                });
            }
            return list;
        }

        private List<string> GetMessageTypes(string messageGroupLevel1Code)
        {
            var relCodes = AdwService.GetRelatedCodes("UMGT", messageGroupLevel1Code);

            var list = new List<string>();
            foreach (var relCode in relCodes)
            {
                list.Add(relCode.SubordinateCode);
            }
            return list;
        }

        private int GetMessageCount(string messageGroupLevel1Code, Dictionary<string, int> messageCounts)
        {
            var count = 0;
            var messageTypes = GetMessageTypes(messageGroupLevel1Code);

            foreach (var messageType in messageTypes)
            {
                if (messageCounts.ContainsKey(messageType))
                {
                    count += messageCounts[messageType];
                }
            }

            return count;
        }

        private Dictionary<string, int> ConvertMessageCounts(List<MessageCountData> messageCountData)
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

        #endregion

        //public void ClearSpecificMessagesCache(string siteCode, DateTime adateTime)
        //{
        //    CacheService.Remove(string.Format("{0}:{1}", siteCode, adateTime.Date));
        //}
    }
}
