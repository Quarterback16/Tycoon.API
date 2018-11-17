using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Employment.Esc.Notification.Contracts.DataContracts;
using Employment.Esc.Notification.Contracts.ServiceContracts;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Models;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Service.Interfaces.Notification;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;

namespace Employment.Web.Mvc.Service.Implementation.Notification
{
    public class NotificationService : Infrastructure.Services.Service, INotificationService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoticeboardService" /> class.
        /// </summary>
        /// <param name="client">Client for interacting with WCF services.</param>
        /// <param name="cacheService">Cache service for interacting with cached data.</param>
        public NotificationService(IClient client,  ICacheService cacheService) : base(client,  cacheService) { }

        /// <summary>
        /// Gets a list of Notification messages for a job seeker.
        /// </summary>
        /// <param name="jobseekerID">Requested Jobseeker ID.</param>
        /// <returns>A collection of <see cref="NotificationModel" /> for the jobseeker.</returns>
        public IEnumerable<NotificationModel> GetList(long jobseekerID)
        {
            var cacheKey = new KeyModel(CacheType.Global, "NotificationList").Add(jobseekerID.ToString());

            IEnumerable<NotificationModel> cacheValue = null;
            if (CacheService.TryGet(cacheKey, out cacheValue))
            {
                return cacheValue;
            }

            var request = new AllMessageRequest { JobseekerId = jobseekerID };

            ValidateRequest(request);

            try
            {
                var service = Client.Create<IDiaryNotification>("DiaryNotification.svc");

                var response = service.JobseekerGetAllMessages(request);

                ValidateResponse(response);

                var model = new List<NotificationModel>();

                if (response.SqlLists != null && response.SqlLists.Length > 0)
                {
                    var sqlModel = response.SqlLists.ToNotificationModelList();//   MappingEngine.Map<IEnumerable<NotificationModel>>(response.SqlLists);
                    model.AddRange(sqlModel);
                }
                if (response.MFLists != null && response.MFLists.Length > 0)
                {
                    var mfModel = response.MFLists.ToNotificationModelList();//MappingEngine.Map<IEnumerable<NotificationModel>>(response.MFLists);
                    model.AddRange(mfModel);
                }

                // TODO sort ?

                var i = 0;
                foreach (var item in model)
                {
                    item.ListItemID = i;
                    i++;
                    item.JobseekerID = jobseekerID;
                }

                CacheService.Set(cacheKey, model);

                return model;
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
        /// Get the Notification letter details.
        /// </summary>
        /// <param name="jobseekerID">The requested jobseekerID.</param>
        /// <param name="printRequestID">The requested print request ID.</param>
        /// <returns>The letter lines.</returns>
        public IEnumerable<string> GetNotificationLetterDetails(long jobseekerID, long printRequestID)
        {
            var request = new PrintRequestParamsRequest 
            { 
                JobseekerId = jobseekerID,
                PrintRequestId = printRequestID
            };

            ValidateRequest(request);

            try
            {
                var service = Client.Create<INotification>("Notification.svc");

                var response = service.PrintLetterLines(request);

                ValidateResponse(response);

                var letterModel = new List<string>();

                if (response.textLists != null)
                {
                    foreach (var item in response.textLists)
                    {
                        letterModel.Add(item.LetterLine);
                    }
                }

                return letterModel;
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
        /// Create Jobseeker SMS
        /// </summary>
        /// <param name="model"></param>
        /// <returns>The SMS message ID.</returns>
        public long CreateSMS(SMSModel model)
        {
            var request = model.ToSMSJNMMessageRequest();

            ValidateRequest(request);

            try
            {
                var service = Client.Create<IDiaryNotification>("DiaryNotification.svc");

                var response = service.CreateSMSMessage(request);

                ValidateResponse(response);

                return response.MessageId;
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
    }
}
