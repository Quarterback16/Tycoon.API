using System;
using System.ComponentModel;
using System.Web.Mvc;
using System.Web.Routing;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Properties;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to indicate a required parameter. If not supplied, a redirect to the specified action will occur.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class RequiredParameterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// User service.
        /// </summary>
        private IUserService UserService
        {
            get
            {
                var containerProvider = DependencyResolver.Current as IContainerProvider;

                return containerProvider != null ? containerProvider.GetService<IUserService>() : null;
            }
        }

        /// <summary>
        /// The type ID of this attribute.
        /// </summary>
        /// <remarks>
        /// When <see cref="AttributeUsageAttribute.AllowMultiple" /> is true, <see cref="TypeId" /> must be overriden to return the current instance so <see cref="TypeDescriptor" /> can properly return multiple attributes of the same type.
        /// </remarks>
        public override object TypeId { get { return this; } }

        /// <summary>
        /// The name of the parameter that is required.
        /// </summary>
        private readonly string parameterName;

        /// <summary>
        /// The controller action.
        /// </summary>
        private readonly string action;

        /// <summary>
        /// The controller (defaults to controller in current context).
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// The area (defaults to area in current context).
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// The name of the route.
        /// </summary>
        public string RouteName { get; set; }

        ///// <summary>
        ///// Whether the parameter is a activity ID and linked to <see cref="ISessionService.ActivityID" />.
        ///// </summary>
        ///// <remarks>
        ///// If the parameter is supplied, <see cref="ISessionService.ActivityID" /> will be updated with the supplied value. 
        ///// If the parameter is not supplied and <see cref="ISessionService.ActivityID" /> is populated, the value of <see cref="ISessionService.ActivityID" /> will be used.
        ///// If the parameter is not supplied and <see cref="ISessionService.ActivityID" /> is empty, the redirect will occur.
        ///// </remarks>
        //public bool IsActivityID
        //{
        //    get
        //    {
        //        return ContextType.HasValue && ContextType.Value == Types.ContextType.Activity;
        //    }
        //    set
        //    {
        //        if (value)
        //        {
        //            if (ContextType.HasValue)
        //            {
        //                throw new InvalidOperationException(DataAnnotationsResources.RequiredParameterAttribute_Invalid);
        //            }

        //            ContextType = Types.ContextType.Activity;
        //        }
        //    }
        //}

        ///// <summary>
        ///// Whether the parameter is a appointment ID and linked to <see cref="ISessionService.AppointmentID" />.
        ///// </summary>
        ///// <remarks>
        ///// If the parameter is supplied, <see cref="ISessionService.AppointmentID" /> will be updated with the supplied value. 
        ///// If the parameter is not supplied and <see cref="ISessionService.AppointmentID" /> is populated, the value of <see cref="ISessionService.AppointmentID" /> will be used.
        ///// If the parameter is not supplied and <see cref="ISessionService.AppointmentID" /> is empty, the redirect will occur.
        ///// </remarks>
        //public bool IsAppointmentID
        //{
        //    get
        //    {
        //        return ContextType.HasValue && ContextType.Value == Types.ContextType.Appointment;
        //    }
        //    set
        //    {
        //        if (value)
        //        {
        //            if (ContextType.HasValue)
        //            {
        //                throw new InvalidOperationException(DataAnnotationsResources.RequiredParameterAttribute_Invalid);
        //            }

        //            ContextType = Types.ContextType.Appointment;
        //        }
        //    }
        //}

        ///// <summary>
        ///// Whether the parameter is a contract ID and linked to <see cref="ISessionService.ContractID" />.
        ///// </summary>
        ///// <remarks>
        ///// If the parameter is supplied, <see cref="ISessionService.ContractID" /> will be updated with the supplied value. 
        ///// If the parameter is not supplied and <see cref="ISessionService.ContractID" /> is populated, the value of <see cref="ISessionService.ContractID" /> will be used.
        ///// If the parameter is not supplied and <see cref="ISessionService.ContractID" /> is empty, the redirect will occur.
        ///// </remarks>
        //public bool IsContractID
        //{
        //    get
        //    {
        //        return ContextType.HasValue && ContextType.Value == Types.ContextType.Contract;
        //    }
        //    set
        //    {
        //        if (value)
        //        {
        //            if (ContextType.HasValue)
        //            {
        //                throw new InvalidOperationException(DataAnnotationsResources.RequiredParameterAttribute_Invalid);
        //            }

        //            ContextType = Types.ContextType.Contract;
        //        }
        //    }
        //}

        ///// <summary>
        ///// Whether the parameter is a Centrelink reference number and linked to <see cref="ISessionService.CRN" />.
        ///// </summary>
        ///// <remarks>
        ///// If the parameter is supplied, <see cref="ISessionService.CRN" /> will be updated with the supplied value. 
        ///// If the parameter is not supplied and <see cref="ISessionService.CRN" /> is populated, the value of <see cref="ISessionService.CRN" /> will be used.
        ///// If the parameter is not supplied and <see cref="ISessionService.CRN" /> is empty, the redirect will occur.
        ///// </remarks>
        //public bool IsCRN
        //{
        //    get
        //    {
        //        return ContextType.HasValue && ContextType.Value == Types.ContextType.CRN;
        //    }
        //    set
        //    {
        //        if (value)
        //        {
        //            if (ContextType.HasValue)
        //            {
        //                throw new InvalidOperationException(DataAnnotationsResources.RequiredParameterAttribute_Invalid);
        //            }

        //            ContextType = Types.ContextType.CRN;
        //        }
        //    }
        //}

        ///// <summary>
        ///// Whether the parameter is a employer ID and linked to <see cref="ISessionService.EmployerID" />.
        ///// </summary>
        ///// <remarks>
        ///// If the parameter is supplied, <see cref="ISessionService.EmployerID" /> will be updated with the supplied value. 
        ///// If the parameter is not supplied and <see cref="ISessionService.EmployerID" /> is populated, the value of <see cref="ISessionService.EmployerID" /> will be used.
        ///// If the parameter is not supplied and <see cref="ISessionService.EmployerID" /> is empty, the redirect will occur.
        ///// </remarks>
        //public bool IsEmployerID
        //{
        //    get
        //    {
        //        return ContextType.HasValue && ContextType.Value == Types.ContextType.Employer;
        //    }
        //    set
        //    {
        //        if (value)
        //        {
        //            if (ContextType.HasValue)
        //            {
        //                throw new InvalidOperationException(DataAnnotationsResources.RequiredParameterAttribute_Invalid);
        //            }

        //            ContextType = Types.ContextType.Employer;
        //        }
        //    }
        //}

        ///// <summary>
        ///// Whether the parameter is a job seeker ID and linked to <see cref="ISessionService.JobSeekerID" />.
        ///// </summary>
        ///// <remarks>
        ///// If the parameter is supplied, <see cref="ISessionService.JobSeekerID" /> will be updated with the supplied value. 
        ///// If the parameter is not supplied and <see cref="ISessionService.JobSeekerID" /> is populated, the value of <see cref="ISessionService.JobSeekerID" /> will be used.
        ///// If the parameter is not supplied and <see cref="ISessionService.JobSeekerID" /> is empty, the redirect will occur.
        ///// </remarks>
        //public bool IsJobSeekerID
        //{
        //    get
        //    {
        //        return ContextType.HasValue && ContextType.Value == Types.ContextType.JobSeeker;
        //    }
        //    set
        //    {
        //        if (value)
        //        {
        //            if (ContextType.HasValue)
        //            {
        //                throw new InvalidOperationException(DataAnnotationsResources.RequiredParameterAttribute_Invalid);
        //            }

        //            ContextType = Types.ContextType.JobSeeker;
        //        }
        //    }
        //}

        ///// <summary>
        ///// Whether the parameter is a override ID and linked to <see cref="ISessionService.OverrideID" />.
        ///// </summary>
        ///// <remarks>
        ///// If the parameter is supplied, <see cref="ISessionService.OverrideID" /> will be updated with the supplied value. 
        ///// If the parameter is not supplied and <see cref="ISessionService.OverrideID" /> is populated, the value of <see cref="ISessionService.OverrideID" /> will be used.
        ///// If the parameter is not supplied and <see cref="ISessionService.OverrideID" /> is empty, the redirect will occur.
        ///// </remarks>
        //public bool IsOverrideID
        //{
        //    get
        //    {
        //        return ContextType.HasValue && ContextType.Value == Types.ContextType.Override;
        //    }
        //    set
        //    {
        //        if (value)
        //        {
        //            if (ContextType.HasValue)
        //            {
        //                throw new InvalidOperationException(DataAnnotationsResources.RequiredParameterAttribute_Invalid);
        //            }

        //            ContextType = Types.ContextType.Override;
        //        }
        //    }
        //}

        ///// <summary>
        ///// Whether the parameter is a payment ID and linked to <see cref="ISessionService.PaymentID" />.
        ///// </summary>
        ///// <remarks>
        ///// If the parameter is supplied, <see cref="ISessionService.PaymentID" /> will be updated with the supplied value. 
        ///// If the parameter is not supplied and <see cref="ISessionService.PaymentID" /> is populated, the value of <see cref="ISessionService.PaymentID" /> will be used.
        ///// If the parameter is not supplied and <see cref="ISessionService.PaymentID" /> is empty, the redirect will occur.
        ///// </remarks>
        //public bool IsPaymentID
        //{
        //    get
        //    {
        //        return ContextType.HasValue && ContextType.Value == Types.ContextType.Payment;
        //    }
        //    set
        //    {
        //        if (value)
        //        {
        //            if (ContextType.HasValue)
        //            {
        //                throw new InvalidOperationException(DataAnnotationsResources.RequiredParameterAttribute_Invalid);
        //            }

        //            ContextType = Types.ContextType.Payment;
        //        }
        //    }
        //}

        ///// <summary>
        ///// Whether the parameter is a provider ID and linked to <see cref="ISessionService.ProviderID" />.
        ///// </summary>
        ///// <remarks>
        ///// If the parameter is supplied, <see cref="ISessionService.ProviderID" /> will be updated with the supplied value. 
        ///// If the parameter is not supplied and <see cref="ISessionService.ProviderID" /> is populated, the value of <see cref="ISessionService.ProviderID" /> will be used.
        ///// If the parameter is not supplied and <see cref="ISessionService.ProviderID" /> is empty, the redirect will occur.
        ///// </remarks>
        //public bool IsProviderID
        //{
        //    get
        //    {
        //        return ContextType.HasValue && ContextType.Value == Types.ContextType.Provider;
        //    }
        //    set
        //    {
        //        if (value)
        //        {
        //            if (ContextType.HasValue)
        //            {
        //                throw new InvalidOperationException(DataAnnotationsResources.RequiredParameterAttribute_Invalid);
        //            }

        //            ContextType = Types.ContextType.Provider;
        //        }
        //    }
        //}

        ///// <summary>
        ///// Whether the parameter is a vacancy ID and linked to <see cref="ISessionService.VacancyID" />.
        ///// </summary>
        ///// <remarks>
        ///// If the parameter is supplied, <see cref="ISessionService.VacancyID" /> will be updated with the supplied value. 
        ///// If the parameter is not supplied and <see cref="ISessionService.VacancyID" /> is populated, the value of <see cref="ISessionService.VacancyID" /> will be used.
        ///// If the parameter is not supplied and <see cref="ISessionService.VacancyID" /> is empty, the redirect will occur.
        ///// </remarks>
        //public bool IsVacancyID
        //{
        //    get
        //    {
        //        return ContextType.HasValue && ContextType.Value == Types.ContextType.Vacancy;
        //    }
        //    set
        //    {
        //        if (value)
        //        {
        //            if (ContextType.HasValue)
        //            {
        //                throw new InvalidOperationException(DataAnnotationsResources.RequiredParameterAttribute_Invalid);
        //            }

        //            ContextType = Types.ContextType.Vacancy;
        //        }
        //    }
        //}

        ///// <summary>
        ///// Whether the parameter is a context type that is linked to a context ID.
        ///// </summary>
        ///// <remarks>
        ///// If the parameter is supplied, the linked context ID will be updated with the supplied value. 
        ///// If the parameter is not supplied and the linked context ID is populated, the value of the linked context ID will be used.
        ///// If the parameter is not supplied and the linked context ID is empty, the redirect will occur.
        ///// </remarks>
        //private ContextType? ContextType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiredParameterAttribute" /> class.
        /// </summary>
        /// <param name="parameterName">The name of the parameter that is required.</param>
        /// <param name="action">The action to redirect to if the required parameter is not supplied.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="parameterName" /> is null or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="action" /> is null or empty.</exception>
        public RequiredParameterAttribute(string parameterName, string action)
        {
            if (string.IsNullOrEmpty(parameterName))
            {
                throw new ArgumentNullException("parameterName");
            }

            if (string.IsNullOrEmpty(action))
            {
                throw new ArgumentNullException("action");
            }

            this.parameterName = parameterName;
            this.action = action;
        }

        /// <summary>
        /// Called before the action is executed to check that the required parameters have been supplied.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Default Controller to current controller if not supplied
            if (string.IsNullOrEmpty(Controller))
            {
                Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            }

            // Default Area to current Area if not supplied
            if (string.IsNullOrEmpty(Area))
            {
                Area = filterContext.RouteData.GetArea();
            }

            object parameterValue = null;

            if (filterContext.ActionParameters.ContainsKey(parameterName) && filterContext.ActionParameters[parameterName] != null)
            {
                parameterValue = filterContext.ActionParameters[parameterName];
            }

            //if (ContextType.HasValue)
            //{
            //    string idAsString = parameterValue != null ? parameterValue.ToString() : string.Empty;

            //    long idAsLong;
            //    long.TryParse(idAsString, out idAsLong);

            //    switch (ContextType)
            //    {
            //        // Long based context ID's
            //        case Types.ContextType.Activity:
            //        case Types.ContextType.Appointment:
            //        case Types.ContextType.Employer:
            //        case Types.ContextType.JobSeeker:
            //        case Types.ContextType.Override:
            //        case Types.ContextType.Payment:
            //        case Types.ContextType.Provider:
            //        case Types.ContextType.Vacancy:

            //            var contextIDAsLong = UserService.Session.GetContextID<long>(ContextType.Value);

            //            if (idAsLong > 0)
            //            {
            //                // Update ID in session if a value was supplied
            //                UserService.Session.SetContextID(ContextType.Value, idAsLong);
            //            }
            //            else if (contextIDAsLong > 0)
            //            {
            //                // Use the ID in session if there is one and no value was supplied
            //                filterContext.ActionParameters[parameterName] = parameterValue = contextIDAsLong;
            //            }

            //            break;

            //        // String based context ID's
            //        case Types.ContextType.Contract:
            //        case Types.ContextType.CRN:

            //            var contextIDAsString = UserService.Session.GetContextID<string>(ContextType.Value);

            //            if (!string.IsNullOrEmpty(idAsString))
            //            {
            //                // Update ID in session if a value was supplied
            //                UserService.Session.SetContextID(ContextType.Value, idAsString);
            //            }
            //            else if (!string.IsNullOrEmpty(contextIDAsString))
            //            {
            //                // Use the ID in session if there is one and no value was supplied
            //                filterContext.ActionParameters[parameterName] = parameterValue = contextIDAsString;
            //            }

            //            break;
            //    }
            //}

            // If required parameter is not supplied then set result to redirect to specified action
            if (parameterValue == null)
            {
                var route = new RouteValueDictionary { { "Action", action }, { "Controller", Controller }, { "Area", Area } };

                filterContext.Result = !string.IsNullOrEmpty(RouteName) ? new RedirectToRouteResult(RouteName, route) : new RedirectToRouteResult(route);
            }
        }
    }
}
