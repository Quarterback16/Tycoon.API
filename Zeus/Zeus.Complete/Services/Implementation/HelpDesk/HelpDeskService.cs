using System;
using System.ServiceModel;
using Employment.Esc.HelpDeskNotification.Contracts.DataContracts;
using Employment.Esc.HelpDeskNotification.Contracts.ServiceContracts;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Service.Interfaces.HelpDesk;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;

namespace Employment.Web.Mvc.Service.Implementation.HelpDesk
{
    /// <summary>
    /// Help Desk Notification Service
    /// </summary>
    public class HelpDeskService : Infrastructure.Services.Service, IHelpDeskService
    {
        /// <summary>
        /// The Adw service interface <see cref="IAdwService"/>
        /// </summary>
        protected readonly IAdwService AdwService;

        /// <summary>
        /// Initializes a new instance of the <see cref="HelpDeskService" /> class.
        /// </summary>
        /// <param name="adwService">Adw service for retrieving ADW data.</param>
        /// <param name="client">Client for interacting with WCF services.</param>
        /// <param name="cacheService">Cache service for interacting with cached data.</param>
        public HelpDeskService(IAdwService adwService, IClient client,  ICacheService cacheService) : base(client,  cacheService)
        {
            if (adwService == null)
            {
                throw new ArgumentNullException("adwService");
            }

            AdwService = adwService;
        }

        /// <summary>
        /// Insert a Help desk notification
        /// </summary>
        /// <param name="model"></param>
        /// <returns>ID of the record inserted</returns>
        public int Create(HelpDeskModel model)
        {
            var request = model.ToInsHelpDeskNotificationRequest();
            ValidateRequest(request);

            try
            {
                var ret = AdwService.GetRelatedCodes("SINF", model.Subject).ToCodeModelList();

                request.subjectArea = ret[0].Code;

                string lapCode = AdwService.GetRelatedCodeDescription("SULA", request.subjectArea, "TODO");//TODO:Fix
                if (!string.IsNullOrEmpty(lapCode))
                {
                    request.description = string.Format("{0}: {1}  ", lapCode, model.Description);
                }
                else
                {
                    request.description = string.Format("{0}  ", model.Description);
                }


                var service = Client.Create<IHelpDeskNotification>("HelpDeskNotification.svc");
                request.userType = "Remote Services Client";
                request.callTakenBy = "Remote Services Web Form";
                request.status = "New";
                request.module = "HelpDeskService";
                request.priority = "1-Super User";
                request.summary = "a";
                request.subjectArea = request.subjectArea;
                var response = service.Insert(request);
                // Only responses with IResponseWithExecutionResult need to call ValidateResponse
                ValidateResponse(response);

                return response.requestID;
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