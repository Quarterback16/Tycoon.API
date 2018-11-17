using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.Extensions
{

    internal class ClaimSubset
    {
        public string UserId;
        public string Site;
        public string Org;
        public bool IsAuthenticated;
        public bool AuthenticationMethodWindows;
        public HashSet<string> Roles;
        public HashSet<string> Contracts;

        public bool IsInRole(string[] roles)
        {
            if (roles != null)
            {
                foreach (var role in roles)
                {
                    if (Roles.Contains(role))
                    {
                        return true;
                    }
                } 
            }
            return false;
        }

        public bool IsInOrganisation(string[] orgCodes)
        {
            if (orgCodes != null)
            {
                foreach (var code in orgCodes)
                {
                    if (string.Equals(code, Org, StringComparison.Ordinal))
                    {
                        return true;
                    }
                } 
            }
            return false;
        }

        public bool IsInContract(string[] contracts)
        {
            if (contracts != null)
            {
                foreach (var contract in contracts)
                {
                    if (Contracts.Contains(contract))
                    {
                        return true;
                    }
                } 
            }
            return false;
        }
    }

    /// <summary>
    /// Extensions for <see cref="IClaimsIdentity" />.
    /// </summary>
    public static class ClaimsIdentityExtension
    {
        internal static ClaimSubset ToClaimSubset(this ClaimsIdentity identity)
        {
            ClaimSubset claims = new ClaimSubset();
            claims.UserId = identity.Username();
            claims.Org = identity.OrganisationCode();
            claims.Site = identity.SiteCode();
            claims.Roles = identity.GetRolesHash();
            claims.Contracts = identity.GetContractsHash();
            claims.AuthenticationMethodWindows = identity.AuthenticationMethodWindows();
            claims.IsAuthenticated = identity.IsAuthenticated;

            return claims;
        }


        /// <summary>
        /// Returns whether the user is in any of the specified roles.
        /// </summary>
        /// <param name="identity">An instance of <see cref="IClaimsIdentity" />.</param>
        /// <param name="roles">Roles to check.</param>
        /// <returns>true if user has any of the specified roles; otherwise, false.</returns>
        public static bool IsInRole(this ClaimsIdentity identity, IEnumerable<string> roles)
        {
            if (roles == null || roles.Count() == 0)
                return false;

            var rolesHash = identity.GetRolesHash();
            foreach (var role in roles)
            {
                if (rolesHash.Contains(role))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Returns whether the user is in any of the specified organisation codes.
        /// </summary>
        /// <param name="identity">An instance of <see cref="IClaimsIdentity" />.</param>
        /// <param name="organisationCodes">Organisation codes to check.</param>
        /// <returns>true if user has any of the specified organisation codes; otherwise, false.</returns>
        public static bool IsInOrganisation(this ClaimsIdentity identity, IEnumerable<string> organisationCodes)
        {
            return organisationCodes != null && organisationCodes.Intersect(identity.GetOrganisationCodes()).Any();
        }

        /// <summary>
        /// Returns whether the user is in any of the specified site codes.
        /// </summary>
        /// <param name="identity">An instance of <see cref="IClaimsIdentity" />.</param>
        /// <param name="siteCodes">Site codes to check.</param>
        /// <returns>true if user has any of the specified site codes; otherwise, false.</returns>
        public static bool IsInSite(this ClaimsIdentity identity, IEnumerable<string> siteCodes)
        {
            return siteCodes != null && siteCodes.Intersect(identity.GetSiteCodes()).Any();
        }

        /// <summary>
        /// Returns whether the user is in any of the specified contracts.
        /// </summary>
        /// <param name="identity">An instance of <see cref="IClaimsIdentity" />.</param>
        /// <param name="contracts">Contracts to check.</param>
        /// <returns>true if user has any of the specified contracts; otherwise, false.</returns>
        public static bool IsInContract(this ClaimsIdentity identity, IEnumerable<string> contracts)
        {
            return contracts != null && contracts.Intersect(identity.GetContracts()).Any();
        }

        /// <summary>
        /// Get the user roles.
        /// </summary>
        /// <param name="identity">An instance of <see cref="IClaimsIdentity" />.</param>
        /// <returns>A collection of roles.</returns>
        public static IEnumerable<string> GetRoles(this ClaimsIdentity identity)
        {
            return GetClaims(identity, new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ClaimType.RolesNation, ClaimType.RolesBase, ClaimType.RolesGeneral, ClaimType.RolesReporting });
        }


        /// <summary>
        /// Get the user roles.
        /// </summary>
        /// <param name="identity">An instance of <see cref="IClaimsIdentity" />.</param>
        /// <returns>A collection of roles.</returns>
        internal static HashSet<string> GetRolesHash(this ClaimsIdentity identity)
        {
            return GetClaimsHash(identity, new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ClaimType.RolesNation, ClaimType.RolesBase, ClaimType.RolesGeneral, ClaimType.RolesReporting });
        }

        /// <summary>
        /// Get the user nation roles.
        /// </summary>
        /// <param name="identity">An instance of <see cref="IClaimsIdentity" />.</param>
        /// <returns>A collection of nation roles.</returns>
        public static IEnumerable<string> GetNationRoles(this ClaimsIdentity identity)
        {
            return GetClaims(identity,  ClaimType.RolesNation );
        }

        /// <summary>
        /// Get the user general roles.
        /// </summary>
        /// <param name="identity">An instance of <see cref="IClaimsIdentity" />.</param>
        /// <returns>A collection of roles.</returns>
        public static IEnumerable<string> GetGeneralRoles(this ClaimsIdentity identity)
        {
            return GetClaims(identity,  ClaimType.RolesGeneral );
        }

        /// <summary>
        /// Get the user organisation codes.
        /// </summary>
        /// <param name="identity">An instance of <see cref="IClaimsIdentity" />.</param>
        /// <returns>A collection of organisation codes.</returns>
        public static IEnumerable<string> GetOrganisationCodes(this ClaimsIdentity identity)
        {
            return GetClaims(identity,  ClaimType.Organisation );
        }

        /// <summary>
        /// Get the user site codes.
        /// </summary>
        /// <param name="identity">An instance of <see cref="IClaimsIdentity" />.</param>
        /// <returns>A collection of site codes.</returns>
        public static IEnumerable<string> GetSiteCodes(this ClaimsIdentity identity)
        {
            return GetClaims(identity,  ClaimType.Site );
        }

        /// <summary>
        /// Get the user contracts.
        /// </summary>
        /// <param name="identity">An instance of <see cref="IClaimsIdentity" />.</param>
        /// <returns>A collection of contracts.</returns>
        public static IEnumerable<string> GetContracts(this ClaimsIdentity identity)
        {
            return GetClaims(identity,  ClaimType.Contracts );
        }
        internal static HashSet<string> GetContractsHash(this ClaimsIdentity identity)
        {
            return GetClaimsHash(identity, new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ClaimType.Contracts });
        }
        /// <summary>
        /// Get the username.
        /// </summary>
        /// <param name="identity">An instance of <see cref="IClaimsIdentity" />.</param>
        /// <returns>A username.</returns>
        public static IEnumerable<string> GetUsername(this ClaimsIdentity identity)
        {
            return GetClaims(identity,  ClaimType.Name );
        }
        /// <summary>
        /// Get the username.
        /// </summary>
        /// <param name="identity">An instance of <see cref="IClaimsIdentity" />.</param>
        /// <returns>A username.</returns>
        internal static string Username(this ClaimsIdentity identity)
        {
            return GetFirstClaim(identity,  ClaimType.Name );
        }


		        /// <summary>
        /// Get the user organisation codes.
        /// </summary>
        /// <param name="identity">An instance of <see cref="IClaimsIdentity" />.</param>
        /// <returns>A collection of organisation codes.</returns>
        internal static string OrganisationCode(this ClaimsIdentity identity)
        {
            return GetFirstClaim(identity,  ClaimType.Organisation );
        }

        /// <summary>
        /// Get the user site codes.
        /// </summary>
        /// <param name="identity">An instance of <see cref="IClaimsIdentity" />.</param>
        /// <returns>A collection of site codes.</returns>
        internal static string SiteCode(this ClaimsIdentity identity)
        {
            return GetFirstClaim(identity,  ClaimType.Site );
        }
		
        /// <summary>
        /// Get the first name.
        /// </summary>
        /// <param name="identity">An instance of <see cref="IClaimsIdentity" />.</param>
        /// <returns>A first name.</returns>
        internal static string FirstName(this ClaimsIdentity identity)
        {
            return GetFirstClaim(identity,  ClaimType.GivenName );
        }
        /// <summary>
        /// Get the first name.
        /// </summary>
        /// <param name="identity">An instance of <see cref="IClaimsIdentity" />.</param>
        /// <returns>A first name.</returns>
        public static IEnumerable<string> GetFirstName(this ClaimsIdentity identity)
        {
            return GetClaims(identity,  ClaimType.GivenName );
        }
        /// <summary>
        /// Get the last name.
        /// </summary>
        /// <param name="identity">An instance of <see cref="IClaimsIdentity" />.</param>
        /// <returns>A last name.</returns>
        internal static string LastName(this ClaimsIdentity identity)
        {
            return GetFirstClaim(identity,  ClaimType.Surname );
        }
        /// <summary>
        /// Get the last name.
        /// </summary>
        /// <param name="identity">An instance of <see cref="IClaimsIdentity" />.</param>
        /// <returns>A last name.</returns>
        public static IEnumerable<string> GetLastName(this ClaimsIdentity identity)
        {
            return GetClaims(identity,  ClaimType.Surname );
        }

        /// <summary>
        /// Get the last logon date and time.
        /// </summary>
        /// <param name="identity">An instance of <see cref="IClaimsIdentity" />.</param>
        /// <returns>The last logon date and time.</returns>
        public static IEnumerable<string> GetLastLogon(this ClaimsIdentity identity)
        {
            return GetClaims(identity,  ClaimType.LastLogon );
        }

        /// <summary>
        /// From the claim set, if authentication method windows, it is an internal single sign on
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static bool AuthenticationMethodWindows(this ClaimsIdentity identity)
        {
            var authenticationClaim = GetFirstClaim(identity, ClaimType.AuthenticationMethod);
            if (authenticationClaim == null)
                return false;

            return authenticationClaim.Equals("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/windows",StringComparison.Ordinal);
        }
		/// <summary>
        /// Get the claims for a specified set of claim types.
        /// </summary>
        /// <param name="identity">An instance of <see cref="IClaimsIdentity" />.</param>
        /// <param name="claimTypes">A collection of claim types.</param>
        /// <returns>The claims for the specified claim types.</returns>
        public static IEnumerable<string> GetClaims(ClaimsIdentity identity, IEnumerable<string> claimTypes)
        {
            return (identity != null) ? identity.Claims.Where(c => claimTypes.Contains(c.Type)).Select(c => c.Value) : Enumerable.Empty<string>();
        }

        /// <summary>
        /// Get the claims for a specified set of claim types.
        /// </summary>
        /// <param name="identity">An instance of <see cref="IClaimsIdentity" />.</param>
        /// <param name="claimTypes">A collection of claim types.</param>
        /// <returns>The claims for the specified claim types.</returns>
        public static List<string> GetClaims(ClaimsIdentity identity, HashSet<string> claimTypes)
        {
	        if (identity != null)
	        {
				List<string> results = new List<string>();
		        foreach (var claim in identity.Claims)
		        {
                    if (claimTypes.Contains(claim.Type))
			        {
						results.Add(claim.Value);
			        }
		        }
		        return results;
	        }
	        else
	        {
		        return new List<string>(0);
	        }
        }

        /// <summary>
        /// Get the claims for a specified set of claim types.
        /// </summary>
        /// <param name="identity">An instance of <see cref="IClaimsIdentity" />.</param>
        /// <param name="claimTypes">A collection of claim types.</param>
        /// <returns>The claims for the specified claim types.</returns>
        public static HashSet<string> GetClaimsHash(ClaimsIdentity identity, HashSet<string> claimTypes)
        {
            if (identity != null)
            {
                HashSet<string> results = new HashSet<string>(StringComparer.Ordinal);
                foreach (var claim in identity.Claims)
                {
                    if (claimTypes.Contains(claim.Type))
                    {
                        results.Add(claim.Value);
                    }
                }
                return results;
            }
            else
            {
                return new HashSet<string>();
            }
        }
		        
		/// <summary>
        /// Get the claims for a specified set of claim types.
        /// </summary>
        /// <param name="identity">An instance of <see cref="IClaimsIdentity" />.</param>
        /// <param name="claimType">A claim type.</param>
        /// <returns>The claims for the specified claim types.</returns>
        public static IEnumerable<string> GetClaims(ClaimsIdentity identity, string claimType)
        {
	        if (identity != null)
	        {
				List<string> results = new List<string>();
		        foreach (var claim in identity.Claims)
		        {
                    if (string.Equals(claimType, claim.Type, StringComparison.Ordinal))
			        {
						results.Add(claim.Value);
			        }
		        }
		        return results;
	        }
	        else
	        {
		        return Enumerable.Empty<string>();
	        }
        }


		/// <summary>
		/// Get the claims for a specified set of claim types.
		/// </summary>
		/// <param name="identity">An instance of <see cref="IClaimsIdentity"/>.</param>
		/// <param name="claimType">A claim type.</param>
		/// <returns>
		/// The claims for the specified claim types.
		/// </returns>
        internal static string GetFirstClaim(ClaimsIdentity identity, string claimType)
        {
	        if (identity != null)
	        {
		        foreach (var claim in identity.Claims)
		        {
                    if (string.Equals(claimType, claim.Type, StringComparison.Ordinal))
			        {
						return claim.Value;
			        }
		        }
	        }
	        return null;
        }
    }
}
