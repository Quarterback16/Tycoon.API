using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Infrastructure.Models
{
    /// <summary>
    /// Menu model for <see cref="SecurityAttribute" />.
    /// </summary>
    [Serializable]
    public class SecurityModel
    {
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
        public bool AllowInProduction { get; set; }

        /// <summary>
        /// If true, any authenticated user will be allowed and Roles, OrganisationCodes and Contracts will be null.
        /// </summary>
        public bool AllowAny { get; set; }

        /// <summary>
        /// Whether single sign on grants access
        /// </summary>
        public bool AllowWindowsAuthentication { get; set; }

        /// <summary>
        /// If specified, the user must be one of the specified users to have access.
        /// </summary>
        public string[] Users { get; set; }

        /// <summary>
        /// If specified, the user must belong to at least one of the Roles to have access.
        /// </summary>
        public string[] Roles { get; set; }

        /// <summary>
        /// If specified, the user must belong to at least one of the Organisation Codes to have access.
        /// </summary>
        public string[] OrganisationCodes { get; set; }

        /// <summary>
        /// If specified, the user must belong to at least one of the Contracts to have access.
        /// </summary>
        public string[] Contracts { get; set; }

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
            var security = new SecurityAttribute
                          {
                              AllowAny = AllowAny,
                              AllowInProduction = AllowInProduction,
                              AllowWindowsAuthentication = AllowWindowsAuthentication,
                              Contracts = Contracts,
                              OrganisationCodes = OrganisationCodes,
                              Roles = Roles,
                              Users = Users
                          };


            return security.IsAuthorized(identity);
        }

        /// <summary>
        /// Returns whether the user is authorized based on their claims.
        /// </summary>
        /// <param name="identity">User claims identity.</param>
        /// <param name="isPreprodOrProd">if set to <c>true</c> then environment is preprod or prod.</param>
        /// <returns>
        /// true if the user is authorized; otherwise, false.
        /// </returns>
        internal bool IsAuthorized(ClaimsIdentity identity,bool isPreprodOrProd)
        {
            return SecurityAttribute.IsAuthorized(isPreprodOrProd,AllowInProduction, AllowWindowsAuthentication, AllowAny, Users, Roles, OrganisationCodes, Contracts,  identity);
        }


        /// <summary>
        /// Returns whether the user is authorized based on their claims.
        /// </summary>
        /// <param name="identity">User claims identity.</param>
        /// <param name="isPreprodOrProd">if set to <c>true</c> then environment is preprod or prod.</param>
        /// <returns>
        /// true if the user is authorized; otherwise, false.
        /// </returns>
        internal bool IsAuthorized(ClaimSubset identity, bool isPreprodOrProd)
        {
            return SecurityAttribute.IsAuthorized(isPreprodOrProd, AllowInProduction, AllowWindowsAuthentication, AllowAny, Users, Roles, OrganisationCodes, Contracts, identity);
        }

        /// <summary>
        /// Override equals for custom comparison.
        /// </summary>
        /// <param name="obj">Other object.</param>
        /// <returns><c>true</c> if the objects are equal; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            var model = obj as SecurityModel;

            if (model == null)
            {
                return false;
            }

            var contractsMatch = (model.Contracts == null && Contracts == null) || (model.Contracts != null && Contracts != null && model.Contracts.SequenceEqual(Contracts));
            var rolesMatch = (model.Roles == null && Roles == null) || (model.Roles != null && Roles != null && model.Roles.SequenceEqual(Roles));
            var organisationCodesMatch = (model.OrganisationCodes == null && OrganisationCodes == null) || (model.OrganisationCodes != null && OrganisationCodes != null && model.OrganisationCodes.SequenceEqual(OrganisationCodes));
            var usersMatch = (model.Users == null && Users == null) || (model.Users != null && Users != null && model.Users.SequenceEqual(Users));

            return (contractsMatch && rolesMatch && organisationCodesMatch && usersMatch &&
                model.AllowInProduction == AllowInProduction &&
                model.AllowAny == AllowAny &&
                model.AllowWindowsAuthentication == AllowWindowsAuthentication);
        }

        /// <summary>
        /// Override GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = AllowInProduction.GetHashCode();
                hashCode = (hashCode * 397) ^ (Users != null ? Users.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Roles != null ? Roles.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (OrganisationCodes != null ? OrganisationCodes.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Contracts != null ? Contracts.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}