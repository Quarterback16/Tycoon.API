using System.Collections.Generic;
using System.ServiceModel;
using Employment.Esc.SmartClient.Provisioner.Contracts.ServiceContracts;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Service.Interfaces.Provisioner;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;

namespace Employment.Web.Mvc.Service.Implementation.Provisioner
{
    public class ProvisionerService : Infrastructure.Services.Service, IProvisionerService
    {
        public ProvisionerService(IClient client, ICacheService cacheService) : base(client,  cacheService) { }

        public bool EmulateAtNextLogon(ProvisionerModel model)
        {
            try
            {
                var errors = new Dictionary<string, string>();

                if (string.IsNullOrEmpty(model.UserId) || (model.UserId.Length <= 0) || (model.UserId.Length > 10))
                {
                    errors.Add("UserId","User Id is required and must be under 10 characters.");
                }
                if (string.IsNullOrEmpty(model.JobNumber) && (string.IsNullOrEmpty(model.Reason)))
                {
                    errors.Add("JobNumber","You must enter a job number or a reason.");
                }
                if (!string.IsNullOrEmpty(model.JobNumber) && model.JobNumber.Length > 20)
                {
                    errors.Add("JobNumber","The job number is too long.");
                }
                if (!string.IsNullOrEmpty(model.Reason) && (model.Reason.Length > 200))
                {
                    errors.Add("Reason","The reason is too long.");
                }

                if (errors.Count > 0)
                {
                    throw new ServiceValidationException(errors);
                }

                var service = Client.Create<ISmartClientProvisioner>("SmartClientProvisioner.svc");
                return service.EmulateAtNextLogon(model.UserId,model.JobNumber,model.Reason);
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