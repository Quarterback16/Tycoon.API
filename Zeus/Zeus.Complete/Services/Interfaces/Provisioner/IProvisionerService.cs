using System;

namespace Employment.Web.Mvc.Service.Interfaces.Provisioner
{
    public interface IProvisionerService
    {
        Boolean EmulateAtNextLogon(ProvisionerModel model);
    }
}