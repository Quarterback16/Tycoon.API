using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Models;
using Employment.Web.Mvc.Infrastructure.Extensions;
using System;

namespace Employment.Web.Mvc.Infrastructure.Services
{
    /// <summary>
    /// Defines a service for accessing User data.
    /// </summary>
    public class UserService : IUserService
    {
        /// <summary>
        /// Session service for interacting with the session.
        /// </summary>
        private readonly ISessionService SessionService;

        /// <summary>
        /// Configuration manager for interacting with the web.config.
        /// </summary>
        private IConfigurationManager ConfigurationManager
        {
            get
            {
                var containerProvider = DependencyResolver.Current as IContainerProvider;

                return containerProvider != null ? containerProvider.GetService<IConfigurationManager>() : null;
            }
        }

        /// <summary>
        /// History service for interacting with the history.
        /// </summary>
        public IHistoryService History
        {
            get
            {
                var containerProvider = DependencyResolver.Current as IContainerProvider;

                return containerProvider != null ? containerProvider.GetService<IHistoryService>() : null;
            }
        }

        /// <summary>
        /// Dashboard service for interacting with a user's dashboard preferences
        /// </summary>
        public IDashboardService Dashboard 
        {
            get
            {
                var containerProvider = DependencyResolver.Current as IContainerProvider;

                return containerProvider != null ? containerProvider.GetService<IDashboardService>() : null;
            }
        }

        /// <summary>
        /// The user session.
        /// </summary>
        public ISessionService Session { get { return SessionService; } }

        private ClaimsIdentity identity;

        /// <summary>
        /// The user claims identity.
        /// </summary>
        public ClaimsIdentity Identity
        {
            get { return identity ?? Thread.CurrentPrincipal.Identity as ClaimsIdentity; }
            set { identity = value; }
        }

        /// <summary>
        /// Whether the user is authenticated.
        /// </summary>
        public bool IsAuthenticated { get { return Identity.IsAuthenticated; } }

        /// <summary>
        /// The current date time context of the user.
        /// No applications are to use the public setter apart from Administration
        /// </summary>
        public DateTime DateTime
        {
            get
            {
                if (ConfigurationManager.AppSettings.Get("Environment").Equals("PROD", StringComparison.OrdinalIgnoreCase))
                {
                    return DateTime.Now;
                }

                var cookie = HttpContext.Current.Request.Cookies.Get("UserDateTime");

                if (cookie != null)
                {
                    DateTime datevalue;

                    return DateTime.TryParse(cookie.Value, out datevalue) ? datevalue : DateTime.Now;
                }

                return DateTime.Now;
            }
            set
            {
                // Cookie will expire with end of session
                HttpContext.Current.Response.Cookies.Set(new HttpCookie("UserDateTime", value.ToString("o")));
            }
        }

        /// <summary>
        /// Whether to use the high contrast display mode.
        /// </summary>
        public bool UseHighContrast
        {
            get
            {
                bool useHighContrast = false;
                var cookie = HttpContext.Current.Request.Cookies.Get("UserHighContrast");

                if (cookie != null)
                {
                    bool.TryParse(cookie.Value, out useHighContrast);
                }

                return useHighContrast;
            }
            set
            {
                var cookie = new HttpCookie("UserHighContrast", value.ToString());

                // Cookie does not expire with end of session
                cookie.Expires = DateTime.Now.AddYears(100);

                HttpContext.Current.Response.Cookies.Set(cookie);
            }
        }

        /// <summary>
        /// Roles that the user belongs to.
        /// </summary>
        public IEnumerable<string> Roles
        {
            get { return Identity.GetRoles(); }
        }

        /// <summary>
        /// General roles that the user belongs to.
        /// </summary>
        public IEnumerable<string> GeneralRoles 
        {
            get { return Identity.GetGeneralRoles(); }
        }

        /// <summary>
        /// Organisation codes that the user belongs to.
        /// </summary>
        public IEnumerable<string> OrganisationCodes
        {
            get { return Identity.GetOrganisationCodes(); }
        }

        /// <summary>
        /// Organisation code that the user belongs to.
        /// </summary>
        public string OrganisationCode
        {
            get { return Identity.OrganisationCode(); }
        }

        /// <summary>
        /// Site codes that the user belongs to.
        /// </summary>
        public IEnumerable<string> SiteCodes
        {
            get { return Identity.GetSiteCodes(); }
        }

        /// <summary>
        /// Site code that the user belongs to.
        /// </summary>
        public string SiteCode
        {
            get { return Identity.SiteCode(); }
        }

        /// <summary>
        /// Contracts that the user belongs to.
        /// </summary>
        public IEnumerable<string> Contracts
        {
            get { return Identity.GetContracts(); }
        }

        /// <summary>
        /// The username.
        /// </summary>
        public string Username
        {
            get { return Identity.Username(); }
        }

        /// <summary>
        /// Users first name.
        /// </summary>
        public string FirstName
        {
            get { return Identity.FirstName(); }
        }

        /// <summary>
        /// Users last name.
        /// </summary>
        public string LastName
        {
            get { return Identity.LastName(); }
        }

        /// <summary>
        /// Users last logon date and time.
        /// </summary>
        public DateTime? LastLogon
        {
            get
            {
                var lastLogon = Identity.GetLastLogon().FirstOrDefault();

                DateTime dt;
                if (!string.IsNullOrEmpty(lastLogon) && DateTime.TryParse(lastLogon, out dt))
                {
                    return dt;
                }

                return null;
            }
        }

        /// <summary>
        /// Returns whether the user is in any of the specified roles.
        /// </summary>
        /// <param name="roles">Roles to check.</param>
        /// <returns>true if user has any of the specified roles; otherwise, false.</returns>
        public bool IsInRole(IEnumerable<string> roles)
        {
            return Identity.IsInRole(roles);
        }

        /// <summary>
        /// Returns whether the user is in any of the specified organisation codes.
        /// </summary>
        /// <param name="organisationCodes">Organisation codes to check.</param>
        /// <returns>true if user has any of the specified organisation codes; otherwise, false.</returns>
        public bool IsInOrganisation(IEnumerable<string> organisationCodes)
        {
            return Identity.IsInOrganisation(organisationCodes);
        }

        /// <summary>
        /// Returns whether the user is in any of the specified site codes.
        /// </summary>
        /// <param name="siteCodes">Site codes to check.</param>
        /// <returns>true if user has any of the specified site codes; otherwise, false.</returns>
        public bool IsInSite(IEnumerable<string> siteCodes)
        {
            return Identity.IsInSite(siteCodes);
        }

        /// <summary>
        /// Returns whether the user is in any of the specified contracts.
        /// </summary>
        /// <param name="contracts">Contracts to check.</param>
        /// <returns>true if user has any of the specified contracts; otherwise, false.</returns>
        public bool IsInContract(IEnumerable<string> contracts)
        {
            return Identity.IsInContract(contracts);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService" /> class.
        /// </summary>
        /// <param name="sessionService">Session service for interacting with the session.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sessionService"/> is <c>null</c>.</exception>
        public UserService(ISessionService sessionService)
        {
            if (sessionService == null)
            {
                throw new ArgumentNullException("sessionService");
            }

            SessionService = sessionService;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService" /> class.
        /// </summary>
        /// <param name="sessionService">Session service for interacting with the session.</param>
        /// <param name="claimsIdentity">Claims identity.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sessionService"/> is <c>null</c>.</exception>
        internal UserService(ISessionService sessionService, ClaimsIdentity claimsIdentity)
        {
            if (sessionService == null)
            {
                throw new ArgumentNullException("sessionService");
            }

            if (claimsIdentity == null)
            {
                throw new ArgumentNullException("claimsIdentity");
            }

            SessionService = sessionService;
            identity = claimsIdentity;
        }
    }
}
