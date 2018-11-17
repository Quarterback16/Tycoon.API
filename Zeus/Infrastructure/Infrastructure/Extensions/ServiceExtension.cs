using System.Collections.Generic;
using System.Globalization;
using System.ServiceModel;
using Employment.Esc.Shared.Contracts.Execution;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;

namespace Employment.Web.Mvc.Infrastructure.Extensions
{
    /// <summary>
    /// Validate wcf service response
    /// </summary>
    public static class ServiceExtension
    {
        /// <summary>
        /// Validate a wcf response <seealso cref="IResponseWithExecutionResult"/>
        /// </summary>
        /// <param name="response"></param>
        /// <exception cref="ServiceValidationException"></exception>
        public static void Validate(this IResponseWithExecutionResult response)
        {
            if (response.ExecutionResult != null && response.ExecutionResult.Status == ExecuteStatus.Failed)
            {
                var errors = new Dictionary<string, string>();

                foreach (var r in response.ExecutionResult.ExecuteMessages)
                {
                    var key = !string.IsNullOrEmpty(r.Help) ? r.Help : response.ExecutionResult.ExecuteMessages.IndexOf(r).ToString();

                    if (!errors.ContainsKey(key))
                    {
                        errors.Add(key, r.Text);
                    }
                }

                throw new ServiceValidationException(errors);
            }
        }

        /// <summary>
        /// Create a service validation exception from a fault exception
        /// </summary>
        /// <param name="vf"></param>
        /// <returns></returns>
        public static ServiceValidationException ToServiceValidationException(this FaultException<ValidationFault> vf)
        {
            var errors = new Dictionary<string, string>();

            foreach (var error in vf.Detail.Details)
            {
                errors.Add(!string.IsNullOrEmpty(error.Key) ? error.Key : vf.Detail.Details.IndexOf(error).ToString(), error.Message);
            }

            return new ServiceValidationException(errors);
        }

        /// <summary>
        /// Create a service validation exception from a fault exception
        /// </summary>
        /// <param name="fe"></param>
        /// <returns></returns>
        public static ServiceValidationException ToServiceValidationException(this FaultException fe)
        {
            return new ServiceValidationException(new Dictionary<string, string> { { fe.Code.Name, fe.Reason.ToString() } });
        }
    }
}
