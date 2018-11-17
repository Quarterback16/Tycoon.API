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
using Employment.Web.Mvc.Infrastructure.Mappers;
using Employment.Web.Mvc.Infrastructure.Models;
using Employment.Web.Mvc.Infrastructure.Types;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;

namespace Employment.Web.Mvc.Infrastructure.Services
{
    /// <summary>
    /// Defines a service for accessing ADW data.
    /// </summary>
    public class AdwService : Service, IAdwService
    {
        /// <summary>
        /// Sql service for interacting with a Sql database.
        /// </summary>
        protected readonly ISqlService SqlService;

        /// <summary>
        /// Configuration manager for interacting with the Web configuration.
        /// </summary>
        protected readonly IConfigurationManager ConfigurationManager;

        private const string connectionName = "Db_ConnADW";

        /// <summary>
        /// Initializes a new instance of the <see cref="AdwService" /> class.
        /// </summary>
        /// <param name="sqlService">Sql service for interacting with a Sql database.</param>
        /// <param name="configurationManager">Configuration manager for interacting with the Web configuration.</param>
        /// <param name="client">Client for interacting with WCF services.</param>
        /// <param name="cacheService">Cache service for interacting with cached data.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sqlService" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="configurationManager" /> is <c>null</c>.</exception>
        public AdwService(ISqlService sqlService, IConfigurationManager configurationManager, IClient client,  IRuntimeCacheService cacheService) : base(client,  cacheService)
        {
            if (sqlService == null)
            {
                throw new ArgumentNullException("sqlService");
            }

            SqlService = sqlService;

            if (configurationManager == null)
            {
                throw new ArgumentNullException("configurationManager");
            }

            ConfigurationManager = configurationManager;
        }

        /// <summary>
        /// Returns the value of the property that matches the specified criteria.
        /// </summary>
        /// <param name="codeType">The type of code.</param>
        /// <param name="code">The code.</param>
        /// <param name="propertyType">The type of property.</param>
        /// <returns>The property value.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="codeType"/> is <c>null</c> or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="code"/> is <c>null</c> or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="propertyType"/> is <c>null</c> or empty.</exception>
        public string GetPropertyValue(string codeType, string code, string propertyType)
        {
            return GetPropertyValue(codeType, code, propertyType, true);
        }

        /// <summary>
        /// Returns the value of the property that matches the specified criteria.
        /// </summary>
        /// <param name="codeType">The type of code.</param>
        /// <param name="code">The code.</param>
        /// <param name="propertyType">The type of property.</param>
        /// <param name="currentCodesOnly">Whether to use current codes only (default is true).</param>
        /// <returns>The property value.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="codeType"/> is <c>null</c> or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="code"/> is <c>null</c> or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="propertyType"/> is <c>null</c> or empty.</exception>
        public string GetPropertyValue(string codeType, string code, string propertyType, bool currentCodesOnly)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException("code");
            }

            if (string.IsNullOrEmpty(propertyType))
            {
                throw new ArgumentNullException("propertyType");
            }

            var property = GetPropertiesInternal(codeType, propertyType, currentCodesOnly).FirstOrDefault(m => m.Code == code);

            return property != null ? property.Value : string.Empty;
        }

        /// <summary>
        /// Returns the property that matches the specified criteria.
        /// </summary>
        /// <param name="codeType">The type of code.</param>
        /// <param name="propertyType">The type of property.</param>
        /// <param name="currentCodesOnly">Whether to use current codes only (default is true).</param>
        /// <returns>The properties.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="codeType"/> is <c>null</c> or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="propertyType"/> is <c>null</c> or empty.</exception>
        internal virtual IList<PropertyModel> GetPropertiesInternal(string codeType, string propertyType, bool currentCodesOnly)
        {
            if (string.IsNullOrEmpty(codeType))
            {
                throw new ArgumentNullException("codeType");
            }

            var key = new KeyModel(CacheType.Global, "ListProperties").Add(codeType);

            IList<PropertyModel> data;

            if (!CacheService.TryGet(key, out data))
            {
                // Retrieve all Adw property data for the specified code type
                var sqlParameters = new List<SqlParameter>();

                sqlParameters.Add(new SqlParameter { ParameterName = "@return_value", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.ReturnValue });
                sqlParameters.Add(new SqlParameter { ParameterName = "@CodeType", SqlDbType = SqlDbType.VarChar, Value = codeType });
                sqlParameters.Add(new SqlParameter { ParameterName = "@PropertyType", SqlDbType = SqlDbType.VarChar, Value = string.Empty });
                sqlParameters.Add(new SqlParameter { ParameterName = "@ListType", SqlDbType = SqlDbType.Char, Value = string.Empty });
                sqlParameters.Add(new SqlParameter { ParameterName = "@ExactLookup", SqlDbType = SqlDbType.Char, Value = 'y' });
                sqlParameters.Add(new SqlParameter { ParameterName = "@MaxRows", SqlDbType = SqlDbType.Int, Value = 0 });
                sqlParameters.Add(new SqlParameter { ParameterName = "@Current_Date", SqlDbType = SqlDbType.DateTime, Value = UserService.DateTime });

                data = SqlService.Execute<PropertyModel>(connectionName, "up_ListPropertyType", sqlParameters, rdr =>
                {
                    var property = new PropertyModel();
                    property.PropertyType = rdr[0].ToString();
                    property.CodeType = rdr[1].ToString();
                    property.Code = rdr[2].ToString();
                    property.StartDate = (DateTime)rdr[3];
                    if (!rdr.IsDBNull(4))
                    {
                        property.EndDate = (DateTime)rdr[4];
                    }
                    property.Value = rdr[5].ToString();
                    return property;
                }).ToList();

                if (data.Any())
                {
                    // Successful so add all Adw property data for the specified code type to the cache
                    CacheService.Set(key, data);
                }
            }

            // Filter for current codes only
            if (currentCodesOnly)
            {
                var userDate = UserService.DateTime.Date;
                // Filters date range based on user context
                data = data.Where(d => d.StartDate.HasValue && d.StartDate.Value.Date <= userDate && (!d.EndDate.HasValue || d.EndDate.Value.Date > userDate)).ToList();
            }

            // Filter to include only those of the specified property type
            if (!string.IsNullOrEmpty(propertyType))
            {
                data = data.Where(d => d.PropertyType == propertyType).ToList();
            }

            return data;
        }

        /// <summary>
        /// Returns the long description of the list code that matches the specified criteria.
        /// </summary>
        /// <param name="codeType">The type of code.</param>
        /// <param name="code">The code.</param>
        /// <returns>The long description of the list code that matches the specified criteria; otherwise, an empty string.</returns>
        public string GetListCodeDescription(string codeType, string code)
        {
            return GetListCodeDescription(codeType, code, true);
        }

        /// <summary>
        /// Returns the long description of the list code that matches the specified criteria.
        /// </summary>
        /// <param name="codeType">The type of code.</param>
        /// <param name="code">The code.</param>
        /// <param name="currentCodesOnly">Whether to use current codes only (default is true).</param>
        /// <returns>The long description of the list code that matches the specified criteria; otherwise, an empty string.</returns>
        public string GetListCodeDescription(string codeType, string code, bool currentCodesOnly)
        {
            var listCode = GetListCode(codeType, code, currentCodesOnly);

            return (listCode != null) ? listCode.Description : string.Empty;
        }

        /// <summary>
        /// Returns the short description of the list code that matches the specified criteria.
        /// </summary>
        /// <param name="codeType">The type of code.</param>
        /// <param name="code">The code.</param>
        /// <returns>The short description of the list code that matches the specified criteria; otherwise, an empty string.</returns>
        public string GetListCodeDescriptionShort(string codeType, string code)
        {
            return GetListCodeDescriptionShort(codeType, code, true);
        }

        /// <summary>
        /// Returns the short description of the list code that matches the specified criteria.
        /// </summary>
        /// <param name="codeType">The type of code.</param>
        /// <param name="code">The code.</param>
        /// <param name="currentCodesOnly">Whether to use current codes only (default is true).</param>
        /// <returns>The short description of the list code that matches the specified criteria; otherwise, an empty string.</returns>
        public string GetListCodeDescriptionShort(string codeType, string code, bool currentCodesOnly)
        {
            var listCode = GetListCode(codeType, code, currentCodesOnly);

            return (listCode != null) ? listCode.ShortDescription : string.Empty;
        }

        /// <summary>
        /// Returns a <see cref="CodeModel" /> that matches the specified criteria.
        /// </summary>
        /// <param name="codeType">The type of code.</param>
        /// <param name="code">The code.</param>
        /// <returns>The <see cref="CodeModel" /> that matches the specified criteria; otherwise, null.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="code"/> is <c>null</c> or empty.</exception>
        public CodeModel GetListCode(string codeType, string code)
        {
            return GetListCode(codeType, code, true);
        }

        /// <summary>
        /// Returns a <see cref="CodeModel" /> that matches the specified criteria.
        /// </summary>
        /// <param name="codeType">The type of code.</param>
        /// <param name="code">The code.</param>
        /// <param name="currentCodesOnly">Whether to use current codes only (default is true).</param>
        /// <returns>The <see cref="CodeModel" /> that matches the specified criteria; otherwise, null.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="code"/> is <c>null</c> or empty.</exception>
        public CodeModel GetListCode(string codeType, string code, bool currentCodesOnly)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException("code");
            }

            return GetListCodesInternal(codeType, code, currentCodesOnly).FirstOrDefault(d => d.Code == code);
        }

        /// <summary>
        /// Returns a list that contains all the <see cref="CodeModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="codeType">The type of code.</param>
        /// <returns>The list of all the <see cref="CodeModel" /> that meet the specified criteria.</returns>
        public IList<CodeModel> GetListCodes(string codeType)
        {
            return GetListCodes(codeType, true);
        }

        /// <summary>
        /// Returns a list that contains all the <see cref="CodeModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="codeType">The type of code.</param>
        /// <param name="currentCodesOnly">Whether to use current codes only (default is true).</param>
        /// <returns>The list of all the <see cref="CodeModel" /> that meet the specified criteria.</returns>
        public IList<CodeModel> GetListCodes(string codeType, bool currentCodesOnly)
        {
            return GetListCodesInternal(codeType, string.Empty, currentCodesOnly);
        }

        /// <summary>
        /// Returns a list that contains all the <see cref="CodeModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="codeType">The type of code.</param>
        /// <param name="startingCode">The code to start at.</param>
        /// <returns>The list of all the <see cref="CodeModel" /> that meet the specified criteria.</returns>
        public IList<CodeModel> GetListCodes(string codeType, string startingCode)
        {
            return GetListCodes(codeType, startingCode, true);
        }

        /// <summary>
        /// Returns a list that contains all the <see cref="CodeModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="codeType">The type of code.</param>
        /// <param name="startingCode">The code to start at.</param>
        /// <param name="currentCodesOnly">Whether to use current codes only (default is true).</param>
        /// <returns>The list of all the <see cref="CodeModel" /> that meet the specified criteria.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="startingCode"/> is <c>null</c> or empty.</exception>
        public IList<CodeModel> GetListCodes(string codeType, string startingCode, bool currentCodesOnly)
        {
            if (string.IsNullOrEmpty(startingCode))
            {
                throw new ArgumentNullException("startingCode");
            }

            return GetListCodesInternal(codeType, startingCode, currentCodesOnly);
        }

        /// <summary>
        /// Returns a list that contains all the <see cref="CodeModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="codeType">The type of code.</param>
        /// <param name="startingCode">The code to start at.</param>
        /// <param name="currentCodesOnly">Whether to use current codes only (default is true).</param>
        /// <returns>The list of all the <see cref="CodeModel" /> that meet the specified criteria.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="codeType"/> is <c>null</c> or empty or is equal to USR (user details should not be accessed in this way).</exception>
        internal IList<CodeModel> GetListCodesInternal(string codeType, string startingCode, bool currentCodesOnly)
        {
            return GetListCodesInternal(codeType, startingCode, currentCodesOnly, null, null);
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
        internal virtual IList<CodeModel> GetListCodesInternal(string codeType, string startingCode, bool currentCodesOnly, bool? exactLookup, int? maxRows)
        {
            return GetListCodesInternal(codeType, startingCode, currentCodesOnly, exactLookup, maxRows, null);
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
        internal virtual IList<CodeModel> GetListCodesInternal(string codeType, string startingCode, bool currentCodesOnly, bool? exactLookup, int? maxRows, bool? intOnly)
        {
            if (string.IsNullOrEmpty(codeType))
            {
                throw new ArgumentNullException("codeType");
            }

            if (System.String.Compare(codeType, "USR", System.StringComparison.OrdinalIgnoreCase) == 0)
            {
                throw new ArgumentOutOfRangeException("User details should not be retrieved in this way.");
            }

            var key = new KeyModel(CacheType.Global, "ListCodes").Add(codeType).Add(startingCode).Add(currentCodesOnly).Add(!string.IsNullOrEmpty(startingCode) || (exactLookup.HasValue && exactLookup.Value)).Add(maxRows).Add(intOnly);

            IList<CodeModel> data;

            if (!CacheService.TryGet(key, out data))
            {
                var storedProc = "up_ListCode";
                // Retrieve all Adw list code data for the specified code type
                var sqlParameters = new List<SqlParameter>();

                sqlParameters.Add(new SqlParameter { ParameterName = "@return_value", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.ReturnValue });
                sqlParameters.Add(new SqlParameter { ParameterName = "@Code_Type", SqlDbType = SqlDbType.Char, Value = codeType });
                sqlParameters.Add(new SqlParameter { ParameterName = "@StartingCode", SqlDbType = SqlDbType.Char, Value = !string.IsNullOrEmpty(startingCode) ? startingCode : string.Empty });
                sqlParameters.Add(new SqlParameter { ParameterName = "@ListType", SqlDbType = SqlDbType.Char, Value = currentCodesOnly ? "c" : string.Empty });
                sqlParameters.Add(new SqlParameter { ParameterName = "@ExactLookup", SqlDbType = SqlDbType.Char, Value = !string.IsNullOrEmpty(startingCode) || (exactLookup.HasValue && exactLookup.Value) ? "y" : string.Empty });
                sqlParameters.Add(new SqlParameter { ParameterName = "@MaxRows", SqlDbType = SqlDbType.Int, Value = maxRows.HasValue ? maxRows : 0 });
                sqlParameters.Add(new SqlParameter { ParameterName = "@Current_Date", SqlDbType = SqlDbType.DateTime, Value = UserService.DateTime });
                if(intOnly != null && intOnly.Value)
                {
                    storedProc = "up_ListCodeInt"; 
                }

                data = SqlService.Execute<CodeModel>(connectionName, storedProc, sqlParameters, rdr =>
                {
                    var listItem = new CodeModel();
                    listItem.Code = rdr[0].ToString();
                    listItem.ShortDescription = rdr[1].ToString();
                    listItem.Description = rdr[2].ToString();                    
                    if (intOnly != null && intOnly.Value)
                    {
                        listItem.CurrencyStart = (long)rdr[3];
                        listItem.CurrencyEnd = (long)rdr[4];
                    }
                    else
                    {
                        listItem.StartDate = (DateTime)rdr[3];
                        if (!rdr.IsDBNull(4))
                            listItem.EndDate = (DateTime)rdr[4];
                    }

                    return listItem;
                }
                ).ToList();

                if (data.Any())
                {
                    // Successful so add all Adw list code data for the specified code type to the cache
                    CacheService.Set(key, data);
                }
            }

            return data;
        }

        /// <summary>
        /// Returns the long description of the related code that matches the specified criteria.
        /// </summary>
        /// <param name="relatedCodeType">The type of related code.</param>
        /// <param name="searchCode">The code to search for.</param>
        /// <param name="code">The code.</param>
        /// <returns>The long description of the related code that matches the specified criteria; otherwise, an empty string.</returns>
        public string GetRelatedCodeDescription(string relatedCodeType, string searchCode, string code)
        {
            return GetRelatedCodeDescription(relatedCodeType, searchCode, code, true);
        }

        /// <summary>
        /// Returns the long description of the related code that matches the specified criteria.
        /// </summary>
        /// <param name="relatedCodeType">The type of related code.</param>
        /// <param name="searchCode">The code to search for.</param>
        /// <param name="code">The code.</param>
        /// <param name="dominantSearch">Whether it is a dominant search (default is true).</param>
        /// <returns>The long description of the related code that matches the specified criteria; otherwise, an empty string.</returns>
        public string GetRelatedCodeDescription(string relatedCodeType, string searchCode, string code, bool dominantSearch)
        {
            return GetRelatedCodeDescription(relatedCodeType, searchCode, code, dominantSearch, true);
        }

        /// <summary>
        /// Returns the long description of the related code that matches the specified criteria.
        /// </summary>
        /// <param name="relatedCodeType">The type of related code.</param>
        /// <param name="searchCode">The code to search for.</param>
        /// <param name="code">The code.</param>
        /// <param name="dominantSearch">Whether it is a dominant search (default is true).</param>
        /// <param name="currentCodesOnly">Whether to use current codes only (default is true).</param>
        /// <returns>The long description of the related code that matches the specified criteria; otherwise, an empty string.</returns>
        public string GetRelatedCodeDescription(string relatedCodeType, string searchCode, string code, bool dominantSearch, bool currentCodesOnly)
        {
            var relatedCode = GetRelatedCode(relatedCodeType, searchCode, code, dominantSearch, currentCodesOnly).ToCodeModel();

            return (relatedCode != null) ? relatedCode.Description : string.Empty;
        }

        /// <summary>
        /// Returns the short description of the related code that matches the specified criteria.
        /// </summary>
        /// <param name="relatedCodeType">The type of related code.</param>
        /// <param name="searchCode">The code to search for.</param>
        /// <param name="code">The code.</param>
        /// <returns>The short description of the related code that matches the specified criteria; otherwise, an empty string.</returns>
        public string GetRelatedCodeDescriptionShort(string relatedCodeType, string searchCode, string code)
        {
            return GetRelatedCodeDescriptionShort(relatedCodeType, searchCode, code, true);
        }

        /// <summary>
        /// Returns the short description of the related code that matches the specified criteria.
        /// </summary>
        /// <param name="relatedCodeType">The type of related code.</param>
        /// <param name="searchCode">The code to search for.</param>
        /// <param name="code">The code.</param>
        /// <param name="dominantSearch">Whether it is a dominant search (default is true).</param>
        /// <returns>The short description of the related code that matches the specified criteria; otherwise, an empty string.</returns>
        public string GetRelatedCodeDescriptionShort(string relatedCodeType, string searchCode, string code, bool dominantSearch)
        {
            return GetRelatedCodeDescriptionShort(relatedCodeType, searchCode, code, dominantSearch, true);
        }

        /// <summary>
        /// Returns the short description of the related code that matches the specified criteria.
        /// </summary>
        /// <param name="relatedCodeType">The type of related code.</param>
        /// <param name="searchCode">The code to search for.</param>
        /// <param name="code">The code.</param>
        /// <param name="dominantSearch">Whether it is a dominant search (default is true).</param>
        /// <param name="currentCodesOnly">Whether to use current codes only (default is true).</param>
        /// <returns>The short description of the related code that matches the specified criteria; otherwise, an empty string.</returns>
        public string GetRelatedCodeDescriptionShort(string relatedCodeType, string searchCode, string code, bool dominantSearch, bool currentCodesOnly)
        {
            var relatedCode = GetRelatedCode(relatedCodeType, searchCode, code, dominantSearch, currentCodesOnly).ToCodeModel();

            return (relatedCode != null) ? relatedCode.ShortDescription : string.Empty;
        }

        /// <summary>
        /// Returns a <see cref="RelatedCodeModel" /> that matches the specified criteria using a dominant search.
        /// </summary>
        /// <param name="relatedCodeType">The type of related code.</param>
        /// <param name="searchCode">The code to search for.</param>
        /// <param name="code">The code.</param>
        /// <returns>The <see cref="RelatedCodeModel" /> that matches the specified criteria; otherwise, null.</returns>
        public RelatedCodeModel GetRelatedCode(string relatedCodeType, string searchCode, string code)
        {
            return GetRelatedCode(relatedCodeType, searchCode, code, true);
        }

        /// <summary>
        /// Returns a <see cref="RelatedCodeModel" /> that matches the specified criteria.
        /// </summary>
        /// <param name="relatedCodeType">The type of related code.</param>
        /// <param name="searchCode">The code to search for.</param>
        /// <param name="code">The code.</param>
        /// <param name="dominantSearch">Whether it is a dominant search (default is true).</param>
        /// <returns>The <see cref="RelatedCodeModel" /> that matches the specified criteria; otherwise, null.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="code"/> is <c>null</c> or empty.</exception>
        public RelatedCodeModel GetRelatedCode(string relatedCodeType, string searchCode, string code, bool dominantSearch)
        {
            return GetRelatedCode(relatedCodeType, searchCode, code, dominantSearch, true);
        }

        /// <summary>
        /// Returns a <see cref="RelatedCodeModel" /> that matches the specified criteria.
        /// </summary>
        /// <param name="relatedCodeType">The type of related code.</param>
        /// <param name="searchCode">The code to search for.</param>
        /// <param name="code">The code.</param>
        /// <param name="dominantSearch">Whether it is a dominant search (default is true).</param>
        /// <param name="currentCodesOnly">Whether to use current codes only (default is true).</param>
        /// <returns>The <see cref="RelatedCodeModel" /> that matches the specified criteria; otherwise, null.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="code"/> is <c>null</c> or empty.</exception>
        public RelatedCodeModel GetRelatedCode(string relatedCodeType, string searchCode, string code, bool dominantSearch, bool currentCodesOnly)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException("code");
            }

            return GetRelatedCodesInternal(relatedCodeType, searchCode, dominantSearch, currentCodesOnly).FirstOrDefault(d => dominantSearch ? d.SubordinateCode == code : d.DominantCode == code);
        }
        
        /// <summary>
        /// Returns a list that contains all the <see cref="RelatedCodeModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="relatedCodeType">The type of related code.</param>
        /// <returns>The list of all the <see cref="RelatedCodeModel" /> that meet the specified criteria.</returns>
        public IList<RelatedCodeModel> GetRelatedCodes(string relatedCodeType)
        {
            return GetRelatedCodes(relatedCodeType, true);
        }

        /// <summary>
        /// Returns a list that contains all the <see cref="RelatedCodeModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="relatedCodeType">The type of related code.</param>
        /// <param name="currentCodesOnly">Whether to use current codes only (default is true).</param>
        /// <returns>The list of all the <see cref="RelatedCodeModel" /> that meet the specified criteria.</returns>
        public IList<RelatedCodeModel> GetRelatedCodes(string relatedCodeType, bool currentCodesOnly)
        {
            return GetRelatedCodesInternal(relatedCodeType, string.Empty, true, currentCodesOnly);
        }

        /// <summary>
        /// Returns a list that contains all the <see cref="RelatedCodeModel" /> that meet the specified criteria using a dominant search.
        /// </summary>
        /// <param name="relatedCodeType">The type of related code.</param>
        /// <param name="searchCode">The code to search for.</param>
        /// <returns>The list of all the <see cref="RelatedCodeModel" /> that meet the specified criteria.</returns>
        public IList<RelatedCodeModel> GetRelatedCodes(string relatedCodeType, string searchCode)
        {
            return GetRelatedCodes(relatedCodeType, searchCode, true, true);
        }

        /// <summary>
        /// Returns a list that contains all the <see cref="RelatedCodeModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="relatedCodeType">The type of related code.</param>
        /// <param name="searchCode">The code to search for.</param>
        /// <param name="dominantSearch">Whether it is a dominant search (default is true).</param>
        /// <returns>The list of all the <see cref="RelatedCodeModel" /> that meet the specified criteria.</returns>
        public IList<RelatedCodeModel> GetRelatedCodes(string relatedCodeType, string searchCode, bool dominantSearch)
        {
            return GetRelatedCodes(relatedCodeType, searchCode, dominantSearch, true);
        }

        /// <summary>
        /// Returns a list that contains all the <see cref="RelatedCodeModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="relatedCodeType">The type of related code.</param>
        /// <param name="searchCode">The code to search for.</param>
        /// <param name="dominantSearch">Whether it is a dominant search.</param>
        /// <param name="currentCodesOnly">Whether to use current codes only (default is true).</param>
        /// <returns>The list of all the <see cref="RelatedCodeModel" /> that meet the specified criteria.</returns>
        public IList<RelatedCodeModel> GetRelatedCodes(string relatedCodeType, string searchCode, bool dominantSearch, bool currentCodesOnly)
        {
            return GetRelatedCodesInternal(relatedCodeType, searchCode, dominantSearch, currentCodesOnly);
        }

        /// <summary>
        /// Returns a list that contains all the <see cref="RelatedCodeModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="relatedCodeType">The type of related code.</param>
        /// <param name="searchCode">The code to search for.</param>
        /// <param name="dominantSearch">Whether it is a dominant search.</param>
        /// <param name="currentCodesOnly">Whether to use current codes only (default is true).</param>
        /// <returns>The list of all the <see cref="RelatedCodeModel" /> that meet the specified criteria.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="relatedCodeType"/> is <c>null</c> or empty or is equal to UOFH or USOR (user details should not be retrieved in this way).</exception>
        internal IList<RelatedCodeModel> GetRelatedCodesInternal(string relatedCodeType, string searchCode, bool dominantSearch, bool currentCodesOnly)
        {
            return GetRelatedCodesInternal(relatedCodeType, searchCode, dominantSearch, currentCodesOnly, null, null);
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
        internal virtual IList<RelatedCodeModel> GetRelatedCodesInternal(string relatedCodeType, string searchCode, bool dominantSearch, bool currentCodesOnly, bool? exactLookup, int? maxRows)
        {
            if (string.IsNullOrEmpty(relatedCodeType))
            {
                throw new ArgumentNullException("relatedCodeType");
            }

            if ((System.String.Compare(relatedCodeType, "UOFH", System.StringComparison.OrdinalIgnoreCase) == 0) || (System.String.Compare(relatedCodeType, "USOR", System.StringComparison.OrdinalIgnoreCase) == 0))
            {
                throw new ArgumentOutOfRangeException("User details should not be retrieved in this way.");
            }

            var key = new KeyModel(CacheType.Global, "RelatedCodes").Add(relatedCodeType).Add(searchCode).Add(dominantSearch).Add(currentCodesOnly).Add(!string.IsNullOrEmpty(searchCode) || (exactLookup.HasValue && exactLookup.Value)).Add(maxRows);

            IList<RelatedCodeModel> data;

            if (!CacheService.TryGet(key, out data))
            {
                // Retrieve all Adw related code data for the specified related code type{
                var sqlParameters = new List<SqlParameter>();

                sqlParameters.Add(new SqlParameter { ParameterName = "@return_value", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.ReturnValue });
                sqlParameters.Add(new SqlParameter { ParameterName = "@RelationshipType", SqlDbType = SqlDbType.VarChar, Value = relatedCodeType });
                sqlParameters.Add(new SqlParameter { ParameterName = "@SearchCode", SqlDbType = SqlDbType.VarChar, Value = !string.IsNullOrEmpty(searchCode) ? searchCode : string.Empty });
                sqlParameters.Add(new SqlParameter { ParameterName = "@SearchType", SqlDbType = SqlDbType.Char, Value = dominantSearch ? "d" : "s" });
                sqlParameters.Add(new SqlParameter { ParameterName = "@ListType", SqlDbType = SqlDbType.Char, Value = currentCodesOnly ? "c" : string.Empty });
                sqlParameters.Add(new SqlParameter { ParameterName = "@ExactLookup", SqlDbType = SqlDbType.Char, Value = !string.IsNullOrEmpty(searchCode) || (exactLookup.HasValue && exactLookup.Value) ? "y" : string.Empty });
                sqlParameters.Add(new SqlParameter { ParameterName = "@MaxRows", SqlDbType = SqlDbType.Int, Value = maxRows.HasValue ? maxRows.Value : 0 });
                sqlParameters.Add(new SqlParameter { ParameterName = "@RowPosition", SqlDbType = SqlDbType.Int, Value = 0 });
                sqlParameters.Add(new SqlParameter { ParameterName = "@Current_Date", SqlDbType = SqlDbType.DateTime, Value = UserService.DateTime });

                data = SqlService.Execute<RelatedCodeModel>(connectionName, "up_ListRelatedCode", sqlParameters, rdr =>
                {
                    var listItem = new RelatedCodeModel();
                    listItem.RelatedCode = rdr[0].ToString();
                    listItem.SubordinateCode = rdr[1].ToString();
                    listItem.DominantCode = rdr[2].ToString();
                    listItem.StartDate = (DateTime)rdr[3];
                    if (!rdr.IsDBNull(4))
                    {
                        listItem.EndDate = (DateTime)rdr[4];
                    }
                    listItem.SubordinateShortDescription = rdr[5].ToString();
                    listItem.SubordinateDescription = rdr[6].ToString();
                    listItem.DominantShortDescription = rdr[7].ToString();
                    listItem.DominantDescription = rdr[8].ToString();
                    listItem.Position = (int)rdr[9];
                    return listItem;
                }).ToList();

                if (data.Any())
                {
                    // Successful so add all Adw related code data for the specified related code type to the cache
                    CacheService.Set(key, data);
                }
            }

            if (data.Any())
            {
                // Mark whether the search was dominant
                data.ForEach(d => d.Dominant = dominantSearch);
            }

            return data;
        }

        /// <summary>
        /// Returns a list that contains all the <see cref="UserModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="organisationCode">The organisation code the users are in.</param>
        /// <returns>The list of all the <see cref="UserModel" /> that meet the specified criteria.</returns>
        public IList<UserModel> GetUsersInOrganisation(string organisationCode)
        {
            return GetUsersInOrganisation(organisationCode, string.Empty, string.Empty);
        }

        /// <summary>
        /// Returns a list that contains all the <see cref="UserModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="organisationCode">The organisation code the users are in.</param>
        /// <param name="userIDPattern">A user ID pattern.</param>
        /// <param name="namePattern">A name pattern.</param>
        /// <returns>The list of all the <see cref="UserModel" /> that meet the specified criteria.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="organisationCode"/> is <c>null</c> or empty.</exception>
        public virtual IList<UserModel> GetUsersInOrganisation(string organisationCode, string userIDPattern, string namePattern)
        {
            if (string.IsNullOrEmpty(organisationCode))
            {
                throw new ArgumentNullException("organisationCode");
            }

            var key = new KeyModel(CacheType.Global, "UsersInOrganisation").Add(organisationCode).Add(userIDPattern).Add(namePattern);

            List<UserModel> data;

            if (!CacheService.TryGet(key, out data))
            {
                try
                {
                    var service = Client.Create<IAdwActiveDirectory>("AdwActiveDirectory.svc");

                    var response = service.ListUsersByOrg(new ListUsersByOrgRequest
                    {
                        OrgCode = organisationCode,
                        UserIdPattern = userIDPattern,
                        NamePattern = namePattern
                    });

                    ValidateResponse(response);

                    data = (response.Users != null) ? response.Users.ToListUserModel() : new List<UserModel>();

                    CacheService.Set(key, data);
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

            return data;
        }

        /// <summary>
        /// Returns a list that contains all the <see cref="UserModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="organisationCode">The organisation code the users are in.</param>
        /// <param name="role">The role the users are in.</param>
        /// <returns>The list of all the <see cref="UserModel" /> that meet the specified criteria.</returns>
        public IList<UserModel> GetUsersInOrganisationWithRole(string organisationCode, string role)
        {
            return GetUsersInOrganisationWithRole(organisationCode, role, string.Empty, string.Empty);
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
        public virtual IList<UserModel> GetUsersInOrganisationWithRole(string organisationCode, string role, string userIDPattern, string namePattern)
        {
            if (string.IsNullOrEmpty(organisationCode))
            {
                throw new ArgumentNullException("organisationCode");
            }

            if (string.IsNullOrEmpty(role))
            {
                throw new ArgumentNullException("role");
            }

            var key = new KeyModel(CacheType.Global, "UsersInOrganisationWithRole").Add(organisationCode).Add(role).Add(userIDPattern).Add(namePattern);

            List<UserModel> data;

            if (!CacheService.TryGet(key, out data))
            {
                try
                {
                    var service = Client.Create<IAdwActiveDirectory>("AdwActiveDirectory.svc");

                    var response = service.ListUsersByOrgWithRole(new ListUsersByOrgWithRoleRequest
                    {
                        OrgCode = organisationCode,
                        Role = role,
                        UserIdPattern = userIDPattern,
                        NamePattern = namePattern
                    });

                    ValidateResponse(response);

                    data = (response.Users != null) ? response.Users.ToListUserModel() : new List<UserModel>();

                    CacheService.Set(key, data);
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

            return data;
        }

        /// <summary>
        /// Returns a list that contains all the <see cref="UserModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="siteCode">The site code the users are in.</param>
        /// <returns>The list of all the <see cref="UserModel" /> that meet the specified criteria.</returns>
        public IList<UserModel> GetUsersInSite(string siteCode)
        {
            return GetUsersInSite(siteCode, string.Empty, string.Empty);
        }

        /// <summary>
        /// Returns a list that contains all the <see cref="UserModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="siteCode">The site code the users are in.</param>
        /// <param name="userIDPattern">A user ID pattern.</param>
        /// <param name="namePattern">A name pattern.</param>
        /// <returns>The list of all the <see cref="UserModel" /> that meet the specified criteria.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="siteCode"/> is <c>null</c> or empty.</exception>
        public virtual IList<UserModel> GetUsersInSite(string siteCode, string userIDPattern, string namePattern)
        {
            if (string.IsNullOrEmpty(siteCode))
            {
                throw new ArgumentNullException("siteCode");
            }

            var key = new KeyModel(CacheType.Global, "UsersInSite").Add(siteCode).Add(userIDPattern).Add(namePattern);

            List<UserModel> data;

            if (!CacheService.TryGet(key, out data))
            {
                try
                {
                    var service = Client.Create<IAdwActiveDirectory>("AdwActiveDirectory.svc");

                    var response = service.ListUsersBySite(new ListUsersBySiteRequest
                    {
                        SiteCode = siteCode,
                        UserIdPattern = userIDPattern,
                        NamePattern = namePattern
                    });

                    ValidateResponse(response);

                    data = (response.Users != null) ? response.Users.ToListUserModel() : new List<UserModel>();

                    CacheService.Set(key, data);
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

            return data;
        }

        /// <summary>
        /// Returns a list that contains all the <see cref="UserModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="siteCode">The site code the users are in.</param>
        /// <param name="role">The role the users are in.</param>
        /// <returns>The list of all the <see cref="UserModel" /> that meet the specified criteria.</returns>
        public IList<UserModel> GetUsersInSiteWithRole(string siteCode, string role)
        {
            return GetUsersInSiteWithRole(siteCode, role, string.Empty, string.Empty);
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
        public virtual IList<UserModel> GetUsersInSiteWithRole(string siteCode, string role, string userIDPattern, string namePattern)
        {
            if (string.IsNullOrEmpty(siteCode))
            {
                throw new ArgumentNullException("siteCode");
            }

            if (string.IsNullOrEmpty(role))
            {
                throw new ArgumentNullException("role");
            }

            var key = new KeyModel(CacheType.Global, "UsersInSiteWithRole").Add(siteCode).Add(role).Add(userIDPattern).Add(namePattern);

            List<UserModel> data;

            if (!CacheService.TryGet(key, out data))
            {
                try
                {
                    var service = Client.Create<IAdwActiveDirectory>("AdwActiveDirectory.svc");

                    var response = service.ListUsersBySiteWithRole(new ListUsersBySiteWithRoleRequest
                    {
                        SiteCode = siteCode,
                        Role = role,
                        UserIdPattern = userIDPattern,
                        NamePattern = namePattern
                    });

                    ValidateResponse(response);

                    data = (response.Users != null) ? response.Users.ToListUserModel() : new List<UserModel>();

                    CacheService.Set(key, data);
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

            return data;
        }

        /// <summary>
        /// Gets the name of user based on the User ID supplied.
        /// </summary>
        /// <param name="userId">Ensure that this User ID has (_D).</param>
        /// <returns> <see cref="UserModel"/> User Model if response is received otherwise <c>null</c>.</returns>
        public UserModel GetUserDetails(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                try
                {
                    UserModel userModel = null;
                    var key = new KeyModel(CacheType.Global, "UserDetailsOf").Add(userId);

                    // Check if exists in Cache.
                    if (!CacheService.TryGet(key, out userModel))
                    {


                        var service = Client.Create<IAdwActiveDirectory>("AdwActiveDirectory.svc");
                        var response = service.GetUserIdNameDetails(new GetUserIdNameDetailsRequest
                            {
                                UserId = userId
                            });

                        ValidateResponse(response);
                        var data = response.Users;

                        if (data != null)
                        {
                            var userDetails = data.FirstOrDefault();
                            if (userDetails != null)
                            {
                                userModel = userDetails.ToListUserModel();

                                // Add in Cache.
                                CacheService.Set(key, userModel);

                                return userModel;
                            }

                        }
                    }
                    else
                    {
                        return userModel;
                    }
                }
                catch (FaultException<ValidationFault> validationFault)
                {
                    throw validationFault.ToServiceValidationException();
                }
                catch (FaultException faultException)
                {
                    throw faultException.ToServiceValidationException();
                }
                
            }
            return new UserModel();
        }
    }
}