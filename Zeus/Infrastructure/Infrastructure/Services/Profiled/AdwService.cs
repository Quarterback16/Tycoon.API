using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel;

using Employment.Esc.Adw.Contracts.DataContracts;
using Employment.Esc.Adw.Contracts.ServiceContracts;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Models;
using Employment.Web.Mvc.Infrastructure.Types;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using StackExchange.Profiling;

namespace Employment.Web.Mvc.Infrastructure.Services.Profiled
{
    /// <summary>
    /// Defines a service for accessing ADW data.
    /// </summary>
    /// <remarks>
    /// Profiled version of <see cref="Services.AdwService" />.
    /// </remarks>
    public class AdwService : Services.AdwService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdwService" /> class.
        /// </summary>
        /// <param name="sqlService">Sql service for interacting with a Sql database.</param>
        /// <param name="configurationManager">Configuration manager for interacting with the Web configuration.</param>
        /// <param name="client">Client for interacting with WCF services.</param>
        /// <param name="cacheService">Cache service for interacting with cached data.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sqlService" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="configurationManager" /> is <c>null</c>.</exception>
        public AdwService(ISqlService sqlService, IConfigurationManager configurationManager, IClient client, IRuntimeCacheService cacheService) : base(sqlService, configurationManager, client,  cacheService) { }

        /// <summary>
        /// Returns the property that matches the specified criteria.
        /// </summary>
        /// <param name="codeType">The type of code.</param>
        /// <param name="propertyType">The type of property.</param>
        /// <param name="currentCodesOnly">Whether to use current codes only (default is true).</param>
        /// <returns>The properties.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="codeType"/> is <c>null</c> or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="propertyType"/> is <c>null</c> or empty.</exception>
        internal override IList<PropertyModel> GetPropertiesInternal(string codeType, string propertyType, bool currentCodesOnly)
        {
            if (string.IsNullOrEmpty(codeType))
            {
                throw new ArgumentNullException("codeType");
            }

            using (MiniProfiler.Current.Step(string.Format("AdwService.GetProperties ({0})", codeType)))
            {
                return base.GetPropertiesInternal(codeType, propertyType, currentCodesOnly);
            }
        }
        
        /// <summary>
        /// Returns a list that contains all the <see cref="CodeModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="codeType">The type of code.</param>
        /// <param name="startingCode">The code to start at.</param>
        /// <param name="currentCodesOnly">Whether to use current codes only (default is true).</param>
        /// <param name="exactLookup">Whether to do an exact lookup (default is true).</param>
        /// <param name="maxRows">The maximum number of rows to return (0 is all).</param>
        /// <returns>The list of all the <see cref="CodeModel" /> that meet the specified criteria.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="codeType"/> is <c>null</c> or empty or is equal to USR (user details should not be accessed in this way).</exception>
        internal override IList<CodeModel> GetListCodesInternal(string codeType, string startingCode, bool currentCodesOnly, bool? exactLookup, int? maxRows)
        {
            if (string.IsNullOrEmpty(codeType))
            {
                throw new ArgumentNullException("codeType");
            }

            if (string.Compare(codeType, "USR", true) == 0)
            {
                throw new ArgumentOutOfRangeException("User details should not be retrieved in this way.");
            }

            using (MiniProfiler.Current.Step(string.Format("AdwService.GetListCodes ({0})", codeType)))
            {
                return base.GetListCodesInternal(codeType, startingCode, currentCodesOnly, exactLookup, maxRows);
            }
        }

        /// <summary>
        /// Returns a list that contains all the <see cref="CodeModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="relatedCodeType">The type of related code.</param>
        /// <param name="searchCode">The code to search for.</param>
        /// <param name="dominantSearch">Whether it is a dominant search.</param>
        /// <param name="currentCodesOnly">Whether to use current codes only (default is true).</param>
        /// <param name="exactLookup">Whether to do an exact lookup (default is true).</param>
        /// <param name="maxRows">The maximum number of rows to return (0 is all).</param>
        /// <returns>The list of all the <see cref="CodeModel" /> that meet the specified criteria.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="relatedCodeType"/> is <c>null</c> or empty or is equal to UOFH or USOR (user details should not be retrieved in this way).</exception>
        internal override IList<RelatedCodeModel> GetRelatedCodesInternal(string relatedCodeType, string searchCode, bool dominantSearch, bool currentCodesOnly, bool? exactLookup, int? maxRows)
        {
            if (string.IsNullOrEmpty(relatedCodeType))
            {
                throw new ArgumentNullException("relatedCodeType");
            }

            if ((string.Compare(relatedCodeType, "UOFH", true) == 0) || (string.Compare(relatedCodeType, "USOR", true) == 0))
            {
                throw new ArgumentOutOfRangeException("User details should not be retrieved in this way.");
            }

            using (MiniProfiler.Current.Step(string.Format("AdwService.GetRelatedCodes ({0})", relatedCodeType)))
            {
                return base.GetRelatedCodesInternal(relatedCodeType, searchCode, dominantSearch, currentCodesOnly, exactLookup, maxRows);
            }
        }
        
        /// <summary>
        /// Returns a list that contains all the <see cref="UserModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="organisationCode">The organisation code the users are in.</param>
        /// <param name="userIDPattern">A user ID pattern.</param>
        /// <param name="namePattern">A name pattern.</param>
        /// <returns>The list of all the <see cref="UserModel" /> that meet the specified criteria.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="organisationCode"/> is <c>null</c> or empty.</exception>
        public override IList<UserModel> GetUsersInOrganisation(string organisationCode, string userIDPattern, string namePattern)
        {
            if (string.IsNullOrEmpty(organisationCode))
            {
                throw new ArgumentNullException("organisationCode");
            }

            using (MiniProfiler.Current.Step(string.Format("AdwService.GetUsersInOrganisation ({0})", organisationCode)))
            {
                return base.GetUsersInOrganisation(organisationCode, userIDPattern, namePattern);
            }
        }

        /// <summary>
        /// Returns a list that contains all the <see cref="UserModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="organisationCode">The organisation code the users are in.</param>
        /// <param name="role">The role the users are in.</param>
        /// <param name="userIDPattern">A user ID pattern.</param>
        /// <param name="namePattern">A name pattern.</param>
        /// <returns>The list of all the <see cref="UserModel" /> that meet the specified criteria.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="organisationCode"/> is <c>null</c> or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="role"/> is <c>null</c> or empty.</exception>
        public override IList<UserModel> GetUsersInOrganisationWithRole(string organisationCode, string role, string userIDPattern, string namePattern)
        {
            if (string.IsNullOrEmpty(organisationCode))
            {
                throw new ArgumentNullException("organisationCode");
            }

            if (string.IsNullOrEmpty(role))
            {
                throw new ArgumentNullException("role");
            }

            using (MiniProfiler.Current.Step(string.Format("AdwService.GetUsersInOrganisationWithRole ({0}, {1})", organisationCode, role)))
            {
                return base.GetUsersInOrganisationWithRole(organisationCode, role, userIDPattern, namePattern);
            }
        }

        /// <summary>
        /// Returns a list that contains all the <see cref="UserModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="siteCode">The site code the users are in.</param>
        /// <param name="userIDPattern">A user ID pattern.</param>
        /// <param name="namePattern">A name pattern.</param>
        /// <returns>The list of all the <see cref="UserModel" /> that meet the specified criteria.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="siteCode"/> is <c>null</c> or empty.</exception>
        public override IList<UserModel> GetUsersInSite(string siteCode, string userIDPattern, string namePattern)
        {
            if (string.IsNullOrEmpty(siteCode))
            {
                throw new ArgumentNullException("siteCode");
            }

            using (MiniProfiler.Current.Step(string.Format("AdwService.GetUsersInSite ({0})", siteCode)))
            {
                return base.GetUsersInSite(siteCode, userIDPattern, namePattern);
            }
        }

        /// <summary>
        /// Returns a list that contains all the <see cref="UserModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="siteCode">The site code the users are in.</param>
        /// <param name="role">The role the users are in.</param>
        /// <param name="userIDPattern">A user ID pattern.</param>
        /// <param name="namePattern">A name pattern.</param>
        /// <returns>The list of all the <see cref="UserModel" /> that meet the specified criteria.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="siteCode"/> is <c>null</c> or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="role"/> is <c>null</c> or empty.</exception>
        public override IList<UserModel> GetUsersInSiteWithRole(string siteCode, string role, string userIDPattern, string namePattern)
        {
            if (string.IsNullOrEmpty(siteCode))
            {
                throw new ArgumentNullException("siteCode");
            }

            if (string.IsNullOrEmpty(role))
            {
                throw new ArgumentNullException("role");
            }

            using (MiniProfiler.Current.Step(string.Format("AdwService.GetUsersInSiteWithRole ({0}, {1})", siteCode, role)))
            {
                return base.GetUsersInSiteWithRole(siteCode, role, userIDPattern, namePattern);
            }
        }
    }
}