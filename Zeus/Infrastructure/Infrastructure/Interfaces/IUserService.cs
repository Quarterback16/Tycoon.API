using System.Collections.Generic;
using System.IdentityModel.Claims;
using System;
using System.Security.Claims;

namespace Employment.Web.Mvc.Infrastructure.Interfaces
{
    /// <summary>
    /// Defines methods and properties that are required for a User Service.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// The user session.
        /// </summary>
        ISessionService Session { get; }

        /// <summary>
        /// The user history.
        /// </summary>
        IHistoryService History { get; }

        /// <summary>
        /// The user's dsahboard preferences
        /// </summary>
        IDashboardService Dashboard { get; }
        
        /// <summary>
        /// The user claims identity.
        /// </summary>
        ClaimsIdentity Identity { get; }

        /// <summary>
        /// Whether the user is authenticated.
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// The current date time context of the user.
        /// </summary>
        DateTime DateTime { get; set; }

        /// <summary>
        /// All roles that the user belongs to.
        /// </summary>
        IEnumerable<string> Roles { get; }

        /// <summary>
        /// Roles that the user belongs to.
        /// </summary>
        IEnumerable<string> GeneralRoles { get; }

        /// <summary>
        /// Organisation codes that the user belongs to.
        /// </summary>
        IEnumerable<string> OrganisationCodes { get; }

        /// <summary>
        /// Organisation code that the user belongs to.
        /// </summary>
        string OrganisationCode { get; }

        /// <summary>
        /// Site codes that the user belongs to.
        /// </summary>
        IEnumerable<string> SiteCodes { get; }

        /// <summary>
        /// Site code that the user belongs to.
        /// </summary>
        string SiteCode { get; }
        
        /// <summary>
        /// Contracts that the user belongs to.
        /// </summary>
        IEnumerable<string> Contracts { get; }

        /// <summary>
        /// The username.
        /// </summary>
        string Username { get; }

        /// <summary>
        /// Users first name.
        /// </summary>
        string FirstName { get; }

        /// <summary>
        /// Users last name.
        /// </summary>
        string LastName { get; }

        /// <summary>
        /// Users last logon date and time.
        /// </summary>
        DateTime? LastLogon { get; }

        /// <summary>
        /// Whether to use the high contrast display mode.
        /// </summary>
        bool UseHighContrast { get; set; }

        /// <summary>
        /// Returns whether the user is in any of the specified roles.
        /// </summary>
        /// <param name="roles">Roles to check.</param>
        /// <returns>true if user has any of the specified roles; otherwise, false.</returns>
        bool IsInRole(IEnumerable<string> roles);

        /// <summary>
        /// Returns whether the user is in any of the specified organisation codes.
        /// </summary>
        /// <param name="organisationCodes">Organisation codes to check.</param>
        /// <returns>true if user has any of the specified organisation codes; otherwise, false.</returns>
        bool IsInOrganisation(IEnumerable<string> organisationCodes);

        /// <summary>
        /// Returns whether the user is in any of the specified site codes.
        /// </summary>
        /// <param name="siteCodes">Site codes to check.</param>
        /// <returns>true if user has any of the specified site codes; otherwise, false.</returns>
        bool IsInSite(IEnumerable<string> siteCodes);

        /// <summary>
        /// Returns whether the user is in any of the specified contracts.
        /// </summary>
        /// <param name="contracts">Contracts to check.</param>
        /// <returns>true if user has any of the specified contracts; otherwise, false.</returns>
        bool IsInContract(IEnumerable<string> contracts);
    }
}
