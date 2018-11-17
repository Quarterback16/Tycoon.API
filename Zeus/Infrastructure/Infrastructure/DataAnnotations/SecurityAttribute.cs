using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using System.IdentityModel.Claims;
using Employment.Web.Mvc.Infrastructure.Configuration;
using System.Collections.Generic;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to indicate the security requirements for accessing a Controller and/or Controller Actions.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class SecurityAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityAttribute" /> class.
        /// </summary>
        public SecurityAttribute()
        {
            AllowInProduction = true;
            AllowWindowsAuthentication = false;
        }

        /// <summary>
        /// User service.
        /// </summary>
        public IUserService UserService
        {
            get
            {
                var containerProvider = DependencyResolver.Current as IContainerProvider;

                return (containerProvider != null) ? containerProvider.GetService<IUserService>() : null;
            }
        }

        /// <summary>
        /// Configuration manager.
        /// </summary>
        public IConfigurationManager ConfigurationManager
        {
            get
            {
                var containerProvider = DependencyResolver.Current as IContainerProvider;

                return (containerProvider != null) ? containerProvider.GetService<IConfigurationManager>() : null;
            }
        }

        /// <summary>
        /// Whether access is allowed in production.
        /// </summary>
        /// <remarks>
        /// Defaults as <c>true</c>.
        /// </remarks>
        public bool AllowInProduction { get; set; }

        /// <summary>
        /// If specified, the user must be one of the specified users to have access.
        /// </summary>
        public new string[] Users
        {
            get
            {
                if (AllowAny)
                {
                    return null;
                }

                return users;
            }
            set
            {
                users = value;
            }
        }

        private string[] users; 

        /// <summary>
        /// If specified, the user must belong to at least one of the Roles to have access.
        /// </summary>
        public new string[] Roles
        {
            get
            {
                if (AllowAny)
                {
                    return null;
                }

                return roles;
            }
            set
            {
                roles = value;
            }
        }

        private string[] roles;

        /// <summary>
        /// If specified, the user must belong to at least one of the Organisation Codes to have access.
        /// </summary>
        public string[] OrganisationCodes
        {
            get
            {
                if (AllowAny)
                {
                    return null;
                }

                return organisationCodes;
            }
            set
            {
                organisationCodes = value;
            }
        }

        private string[] organisationCodes;

        /// <summary>
        /// If specified, the user must belong to at least one of the Contracts to have access.
        /// </summary>
        public string[] Contracts
        {
            get
            {
                if (AllowAny)
                {
                    return null;
                }

                return contracts;
            }
            set
            {
                contracts = value;
            }
        }

        private string[] contracts;

        /// <summary>
        /// If true, any authenticated user will be allowed and Roles, OrganisationCodes and Contracts will be null.
        /// </summary>
        public bool AllowAny { get; set; }

        /// <summary>
        /// Whether single sign on grants access
        /// </summary>
        public bool AllowWindowsAuthentication { get; set; }

        /// <summary>
        /// If Security settings are specified inside Security.Config, then supply the key.
        /// </summary>
        /// <param name="securityConfigKey">Security setting key.</param>
        public SecurityAttribute(string securityConfigKey = null)
        {
            if (!string.IsNullOrEmpty(securityConfigKey))
            {


                SecuritySettingsSection section = ConfigurationManager.GetSection<SecuritySettingsSection>("securitySettings");//as SecuritySection;
                if(section == null)
                {
                    throw new InvalidOperationException("Could not find securitySettings section in Web.Config");
                }
                if(section.SecurityTypes == null)
                {
                    throw new InvalidOperationException("Could not find securityTypes section in Security.Config");
                }
                var securitySubSection = section.SecurityTypes.Get(securityConfigKey);


                if (securitySubSection != null)
                {
                    bool allowInProduction = false;
                    if( bool.TryParse(securitySubSection.AllowInProduction, out allowInProduction))
                    {
                        AllowInProduction = allowInProduction;
                    }


                    if (securitySubSection.Roles != null && securitySubSection.Roles.Count > 0)
                    {
                        var rolesList = new List<string>() ;

                        for (int i = 0; i < securitySubSection.Roles.Count; i++)
                        {
                            rolesList.Add(securitySubSection.Roles[i].Name);
                        }

                        Roles = rolesList.ToArray();
                    }

                    if (securitySubSection.Contracts != null && securitySubSection.Contracts.Count > 0)
                    {
                        var contractsList = new List<string>();

                        for (int i = 0; i < securitySubSection.Contracts.Count; i++)
                        {
                            contractsList.Add(securitySubSection.Contracts[i].Name);
                        }

                        Contracts = contractsList.ToArray();
                    }

                    if (securitySubSection.Users != null && securitySubSection.Users.Count > 0)
                    {
                        var usersList = new List<string>();

                        for (int i = 0; i < securitySubSection.Users.Count; i++)
                        {
                            usersList.Add(securitySubSection.Users[i].Name);
                        }

                        Users = usersList.ToArray();
                    }

                    if (securitySubSection.OrgCodes != null && securitySubSection.OrgCodes.Count > 0)
                    {
                        var organisationCodesList = new List<string>();

                        for (int i = 0; i < securitySubSection.OrgCodes.Count; i++)
                        {
                            organisationCodesList.Add(securitySubSection.OrgCodes[i].Name);
                        }

                        OrganisationCodes = organisationCodesList.ToArray();
                    }
                }
                 
                 
            }
        }

        /// <summary>
        /// Called when authorization is required.
        /// </summary>
        /// <param name="httpContext">The HTTP context, which encapsulates all HTTP-specific information about an individual HTTP request.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="httpContext" /> parameter is null.</exception>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            var identity = httpContext.User.Identity as ClaimsIdentity;

            return IsAuthorized(identity);
        }

        /// <summary>
        /// Returns whether the user is authorized based on their claims.
        /// </summary>
        /// <returns>true if the user is authorized; otherwise, false.</returns>
        public bool IsAuthorized()
        {
            return IsAuthorized(UserService.Identity);
        }

        /// <summary>
        /// Returns whether the user is authorized based on their claims.
        /// </summary>
        /// <param name="identity">User claims identity.</param>
        /// <returns>true if the user is authorized; otherwise, false.</returns>
        public bool IsAuthorized(ClaimsIdentity identity)
        {
            if ((ConfigurationManager.AppSettings.Get("Environment").Equals("PROD", StringComparison.OrdinalIgnoreCase) ||
                ConfigurationManager.AppSettings.Get("Environment").Equals("PREPROD", StringComparison.OrdinalIgnoreCase))
                && !AllowInProduction)
            {
                return false;
            }

            if ((AllowWindowsAuthentication) && identity.AuthenticationMethodWindows())
            {
                return true;
            }

            return (identity != null && identity.IsAuthenticated && (AllowAny || (Users != null && Users.Contains(identity.Name)) || identity.IsInRole(Roles) || identity.IsInOrganisation(OrganisationCodes) || identity.IsInContract(Contracts)));
        }

        /// <summary>
        /// Returns whether the user is authorized based on their claims.
        /// </summary>
        /// <returns>true if the user is authorized; otherwise, false.</returns>
        internal static bool IsAuthorized(bool isPreprodOrProd, bool allowInProduction, bool allowWindowsAuthentication, bool allowAny,
            string[] users, string[] roles, string[] orgCodes, string[] contracts, ClaimsIdentity identity)
        {
            if (isPreprodOrProd && !allowInProduction)
            {
                return false;
            }

            if ((allowWindowsAuthentication) && identity.AuthenticationMethodWindows())
            {
                return true;
            }

            return (identity != null && identity.IsAuthenticated && (allowAny || (users != null && users.Contains(identity.Name)) || identity.IsInRole(roles) || identity.IsInOrganisation(orgCodes) || identity.IsInContract(contracts)));
        }

        /// <summary>
        /// Returns whether the user is authorized based on their claims.
        /// </summary>
        /// <returns>true if the user is authorized; otherwise, false.</returns>
        internal static bool IsAuthorized(bool isPreprodOrProd, bool allowInProduction, bool allowWindowsAuthentication, bool allowAny,
            string[] users, string[] roles, string[] orgCodes, string[] contracts, ClaimSubset identity)
        {
            if (isPreprodOrProd && !allowInProduction)
            {
                return false;
            }

            if ((allowWindowsAuthentication) && identity.AuthenticationMethodWindows)
            {
                return true;
            }

            return (identity != null && identity.IsAuthenticated && (allowAny || (users != null && users.Contains(identity.UserId)) || identity.IsInRole(roles) || identity.IsInOrganisation(orgCodes) || identity.IsInContract(contracts)));
        }



        /// <summary>
        /// Handle an unauthorized request.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        protected override void HandleUnauthorizedRequest(System.Web.Mvc.AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult();
            filterContext.HttpContext.HandleErrorStatusCode(((HttpStatusCodeResult)filterContext.Result).StatusCode);
        }
    }
}