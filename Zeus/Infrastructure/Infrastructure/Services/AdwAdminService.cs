using System.Collections.Generic;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Models;
using System.Linq;
using System;
using Employment.Web.Mvc.Infrastructure.Types;
using System.Data.SqlClient;
using System.Data;
using Employment.Web.Mvc.Infrastructure.Models.AdwLookup;

namespace Employment.Web.Mvc.Infrastructure.Services
{
    /// <summary>
    /// Defines a service for accessing ADW data.
    /// </summary>
    public class AdwAdminService : AdwService, IAdwAdminService
    {
        private const string ConnectionName = "Db_ConnADW";


        /// <summary>
        /// Initializes a new instance of the <see cref="AdwService" /> class.
        /// </summary>
        /// <param name="sqlService">Sql service for interacting with a Sql database.</param>
        /// <param name="configurationManager">Configuration manager for interacting with the Web configuration.</param>
        /// <param name="client">Client for interacting with WCF services.</param>
        /// <param name="cacheService">Cache service for interacting with cached data.</param>
        public AdwAdminService(ISqlService sqlService, IConfigurationManager configurationManager, IClient client,  IRuntimeCacheService cacheService) : base(sqlService, configurationManager, client,  cacheService) { }


        /// <summary>
        /// Returns a list containing all the <see cref="CodeTypeModel"/> that meets the given criteria.
        /// </summary>
        /// <param name="startingTableType">Starting Table Type.</param>
        /// <param name="listType">Whether to use current codes only (default is all). Values: '' (all), 'C' current codes, 'E' ended codes.</param>
        /// <param name="exactLookup">Whether to do an exact lookup (default is true).</param>
        /// <param name="maxRows">Maximum rows to be returned. Default is 0 indicating unlimited.</param>
        /// <returns>A list of all the <see cref="CodeTypeModel"/> that meets the specified criteria.</returns>
        public IList<CodeTypeModel> GetListCodeTypes(string startingTableType, char listType, bool exactLookup, int maxRows = 0)
        {

            if (string.IsNullOrEmpty(startingTableType))
            {
                throw new ArgumentNullException("startingTableType", "startingTableType cannot be empty.");
            }


            IList<CodeTypeModel> results = Enumerable.Empty<CodeTypeModel>().ToList();

            var keyModel = new KeyModel(CacheType.Global, "ListCodeTypes").Add(startingTableType).Add(listType).Add(exactLookup);
            if (!CacheService.TryGet(keyModel, out results))
            {
                var sqlParameters = new List<SqlParameter>();
                // Retrieve all the Adw Code data for the specified starting table type.

                // PROC Name = up_ListCodeType
                // Parameters are: @StartingCodeType, @ListType, @ExactLookup, @MaxRows
                sqlParameters.Add(new SqlParameter { ParameterName = "@StartingCodeType", SqlDbType = System.Data.SqlDbType.VarChar, Value = startingTableType });
                if (listType != null && listType != '\0' && listType != 'A')
                {
                    sqlParameters.Add(new SqlParameter { ParameterName = "@ListType", SqlDbType = System.Data.SqlDbType.Char, Value = listType });
                }
                sqlParameters.Add(new SqlParameter { ParameterName = "@ExactLookup", SqlDbType = System.Data.SqlDbType.Char, Value = exactLookup ? 'y' : 'n' });
                sqlParameters.Add(new SqlParameter { ParameterName = "@MaxRows", SqlDbType = System.Data.SqlDbType.Int, Value = maxRows });

                results = SqlService.Execute<CodeTypeModel>(ConnectionName, "up_ListCodeType", sqlParameters, dataReader =>
                    {
                        var item = new CodeTypeModel();
                        item.CodeType = dataReader["code_type"].ToString();
                        item.LongDescription = dataReader["long_desc"].ToString();
                        item.ShortDescription = dataReader["short_desc"].ToString();

                        return item;
                    }).ToList();

                if (results.Any())
                {
                    CacheService.Set(keyModel, results);
                }
            }


            return results;

        }

        /// <summary>
        /// Returns a list containing all the <see cref="RelatedCodeTypeModel"/> that meets the given criteria.
        /// </summary>
        /// <param name="startingTableType">Starting Related Table Type.</param>
        /// <param name="listType">Whether to use current codes only (default is all). Values: '' (all), 'C' current codes, 'E' ended codes.</param>
        /// <param name="exactLookup">Whether to do an exact lookup (default is true).</param>
        /// <param name="maxRows">Maximum rows to be returned. Default is 0 indicating unlimited.</param>
        /// <returns>A list of all the <see cref="CodeTypeModel"/> that meets the specified criteria.</returns>
        public IList<RelatedCodeTypeModel> GetRelatedListCodeTypes(string startingTableType, char listType, bool exactLookup, int maxRows = 0)
        {




            IList<RelatedCodeTypeModel> results = Enumerable.Empty<RelatedCodeTypeModel>().ToList();

            var keyModel = new KeyModel(CacheType.Global, "RelatedListCodeTypes").Add(startingTableType).Add(listType).Add(exactLookup);
            if (!CacheService.TryGet(keyModel, out results))
            {
                var sqlParameters = new List<SqlParameter>();
                // Retrieve all the Adw Code data for the specified starting table type.

                // PROC Name = up_ListRelatedCodeType
                // Parameters are: @StartingRelType, @ListType, @ExactLookup, @MaxRows
                if (!string.IsNullOrEmpty(startingTableType))
                {
                    sqlParameters.Add(new SqlParameter { ParameterName = "@StartingRelType", SqlDbType = System.Data.SqlDbType.VarChar, Value = startingTableType });
                }
                if (listType != null && listType != '\0' && listType != 'A')
                {
                    sqlParameters.Add(new SqlParameter { ParameterName = "@ListType", SqlDbType = System.Data.SqlDbType.Char, Value = listType });
                }
                sqlParameters.Add(new SqlParameter { ParameterName = "@ExactLookup", SqlDbType = System.Data.SqlDbType.Char, Value = exactLookup ? 'y' : 'n' });
                sqlParameters.Add(new SqlParameter { ParameterName = "@MaxRows", SqlDbType = System.Data.SqlDbType.Int, Value = maxRows });

                results = SqlService.Execute<RelatedCodeTypeModel>(ConnectionName, "up_ListRelatedCodeType", sqlParameters, dataReader =>
                    {
                        var item = new RelatedCodeTypeModel();
                        item.Relationship = dataReader["relation_type_cd"].ToString();
                        item.SubType = dataReader["sub_code_type"].ToString();
                        item.SubDescription = dataReader["SubDesc"].ToString();
                        item.DomType = dataReader["dom_code_type"].ToString();
                        item.DomDescription = dataReader["DomDesc"].ToString();
                        return item;
                    }
                    ).ToList();

                if (results.Any())
                {
                    CacheService.Set(keyModel, results);
                }
            }


            return results;

        }


        /// <summary>
        /// Returns the List of the properties that match the specified criteria.
        /// </summary>
        /// <param name="startCodeType">The type of code.</param>
        /// <param name="startCode">The code.</param>
        /// <param name="startPropertyType">The type of property.</param>
        /// <param name="listType">Incates whether to use current codes only (default is true).</param>
        /// <param name="exactLookup">Exact lookup.</param>
        /// <param name="maxRows">Maximum number of rows to return. Default 0 indicates unlimited.</param>
        /// <returns>The property value.</returns>
        public IList<PropertyModel> GetPropertyList(string startCodeType, string startCode, string startPropertyType, char listType, bool exactLookup, int maxRows)
        {

            var key = new KeyModel(CacheType.Global, "PropertyList").Add(startCodeType).Add(startCode).Add(startPropertyType).Add(listType).Add(exactLookup).Add(maxRows);

            IList<PropertyModel> data;

            if (!CacheService.TryGet(key, out data))
            {
                // Retrieve all Adw property data for the specified code type
                var sqlParameters = new List<SqlParameter>();

                sqlParameters.Add(new SqlParameter { ParameterName = "@return_value", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.ReturnValue });
                sqlParameters.Add(new SqlParameter { ParameterName = "@CodeType", SqlDbType = SqlDbType.VarChar, Value = startCodeType });
                sqlParameters.Add(new SqlParameter { ParameterName = "@PropertyType", SqlDbType = SqlDbType.VarChar, Value = startPropertyType });
                if (listType != null && listType != '\0' && listType != 'A')
                {
                    sqlParameters.Add(new SqlParameter { ParameterName = "@ListType", SqlDbType = System.Data.SqlDbType.Char, Value = listType });
                }
                sqlParameters.Add(new SqlParameter { ParameterName = "@ExactLookup", SqlDbType = SqlDbType.Char, Value = exactLookup ? 'y' : 'n' });
                sqlParameters.Add(new SqlParameter { ParameterName = "@MaxRows", SqlDbType = SqlDbType.Int, Value = maxRows });
                sqlParameters.Add(new SqlParameter { ParameterName = "@Current_Date", SqlDbType = SqlDbType.DateTime, Value = UserService.DateTime });

                data = SqlService.Execute<PropertyModel>(ConnectionName, "up_ListProperty", sqlParameters, rdr =>
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


            return data;
        }

        /// <summary>
        /// Returns the List of the property types that match the specified criteria.
        /// </summary>
        /// <param name="startCodeType">The start code.</param>
        /// <param name="startCode">The start property code.</param> 
        /// <param name="listType">Indicates whether to use current codes only (default is true).</param>
        /// <param name="exactLookup">Exact lookup.</param>
        /// <param name="maxRows">Maximum number of rows to return. Default 0 indicates unlimited.</param>
        /// <returns>The list of property types.</returns>
        public IList<PropertyModel> GetPropertyTypeList(string startCode, string startProperty, char listType, bool exact = false, int max = 0)
        {

            var key = new KeyModel(CacheType.Global, "PropertyTypeList").Add(startCode).Add(startProperty).Add(listType).Add(exact).Add(max);

            IList<PropertyModel> data;

            if (!CacheService.TryGet(key, out data))
            {
                // Retrieve all Adw property data for the specified code type
                var sqlParameters = new List<SqlParameter>();

                sqlParameters.Add(new SqlParameter { ParameterName = "@return_value", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.ReturnValue });
                sqlParameters.Add(new SqlParameter { ParameterName = "@CodeType", SqlDbType = SqlDbType.VarChar, Value = startCode });
                sqlParameters.Add(new SqlParameter { ParameterName = "@PropertyType", SqlDbType = SqlDbType.VarChar, Value = startProperty });
                if (listType != null && listType != '\0' && listType != 'A')
                {
                    sqlParameters.Add(new SqlParameter { ParameterName = "@ListType", SqlDbType = System.Data.SqlDbType.Char, Value = listType });
                }
                sqlParameters.Add(new SqlParameter { ParameterName = "@ExactLookup", SqlDbType = SqlDbType.Char, Value = exact ? 'y' : 'n' });
                sqlParameters.Add(new SqlParameter { ParameterName = "@MaxRows", SqlDbType = SqlDbType.Int, Value = max });
                sqlParameters.Add(new SqlParameter { ParameterName = "@Current_Date", SqlDbType = SqlDbType.DateTime, Value = UserService.DateTime });

                data = SqlService.Execute<PropertyModel>(ConnectionName, "up_ListPropertyType", sqlParameters, rdr =>
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

            return data;
        }


        /// <summary>
        /// Returns a list that contains all the <see cref="DeltaModel"/> that meet given criteria.
        /// </summary>
        /// <param name="codeName">The Name of the code.</param>
        /// <param name="startCode">The starting of the code.</param>
        /// <param name="lastUpdateDate">Last update date.</param>
        /// <param name="lastUpdateVersion">Last update version.</param>
        /// <param name="rowPosition">Row position.</param>
        /// <param name="maxRows">Maximum rows to returned.</param>
        /// <param name="wsType">WS type.</param>
        /// <returns>The list of all the <see cref="DeltaModel"/> that meet the specified criteria.</returns>
        public IList<DeltaModel> GetDeltaList(string codeName, string startCode, DateTime lastUpdateDate, int lastUpdateVersion = 0, int rowPosition = 0, int maxRows = 0, bool wsType = false)
        {
            var keymodel = new KeyModel("DeltaList").Add(codeName).Add(startCode).Add(lastUpdateDate).Add(lastUpdateVersion).Add(rowPosition).Add(maxRows).Add(wsType);

            IList<DeltaModel> list;

            if(!CacheService.TryGet(keymodel, out list))
            {

                var sqlParameters = new List<SqlParameter>();

                var storedProc = "up_ListCodeDelta";

                sqlParameters.Add(new SqlParameter { ParameterName = "@Code_Type", SqlDbType = SqlDbType.VarChar , Value = codeName});
                sqlParameters.Add(new SqlParameter { ParameterName = "@StartingCode", SqlDbType = SqlDbType.VarChar , Value = startCode});
                sqlParameters.Add(new SqlParameter { ParameterName = "@LastUpdatedDate", SqlDbType = SqlDbType.DateTime , Value = lastUpdateDate});
                sqlParameters.Add(new SqlParameter { ParameterName = "@MaxRows", SqlDbType = SqlDbType.Int , Value = maxRows});

                list = SqlService.Execute<DeltaModel>(ConnectionName, storedProc, sqlParameters, dataReader =>
                    {
                        var model = new DeltaModel();

                        model.Code = dataReader["code"].ToString();                        
                        model.LongDescription = dataReader["long_desc"].ToString();
                        model.ShortDescription = dataReader["short_desc"].ToString();

                        if (!dataReader.IsDBNull(3))
                        {
                            model.CurrencyStartDate = (DateTime) dataReader[3];
                        }

                        if (!dataReader.IsDBNull(4))
                        {
                            model.CurrencyEndDate = (DateTime) dataReader[4];
                        }

                        return model;
                    }).ToList();

                if(!list.Any())
                {
                    CacheService.Set(keymodel, list);
                }
            }

            return list;
        }



        /// <summary>
        /// Returns a list that contains all the <see cref="CodeModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="codeType">The type of code.</param>
        /// <param name="startingCode">The code to start at.</param>
        /// <param name="currentCodesOnly">Whether to use current codes only (default is true).</param>
        /// <param name="exactLookup">Whether to do an exact lookup (default is true).</param>
        /// <returns>The list of all the <see cref="CodeModel" /> that meet the specified criteria.</returns>
        public IList<CodeModel> GetListCodes(string codeType, string startingCode, bool currentCodesOnly, bool exactLookup)
        {
            return GetListCodes(codeType, startingCode, currentCodesOnly, exactLookup, 0);
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
        public IList<CodeModel> GetListCodes(string codeType, string startingCode, bool currentCodesOnly, bool exactLookup, int maxRows)
        {
            return GetListCodesInternal(codeType, startingCode, currentCodesOnly, exactLookup, maxRows);
        }

        /// <summary>
        /// Returns a list that contains all the <see cref="CodeModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="codeType">The type of code.</param>
        /// <param name="startingCode">The code to start at.</param>
        /// <param name="currentCodesOnly">Whether to use current codes only (default is true).</param>
        /// <param name="exactLookup">Whether to do an exact lookup (default is true).</param>
        /// <param name="maxRows">The maximum number of rows to return (0 is all).</param>
        /// <param name="intOnly">Whether to return int types only.</param>
        /// <returns>The list of all the <see cref="CodeModel" /> that meet the specified criteria.</returns>
        public IList<CodeModel> GetListCodes(string codeType, string startingCode, bool currentCodesOnly, bool exactLookup, int maxRows, bool intOnly)
        {
            return GetListCodesInternal(codeType, startingCode, currentCodesOnly, exactLookup, maxRows, intOnly);
        }

        /// <summary>
        /// Returns a list that contains all the <see cref="RelatedCodeModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="relatedCodeType">The type of related code.</param>
        /// <param name="searchCode">The code to search for.</param>
        /// <param name="dominantSearch">Whether it is a dominant search.</param>
        /// <param name="currentCodesOnly">Whether to use current codes only (default is true).</param>
        /// <param name="exactLookup">Whether to do an exact lookup (default is true).</param>
        /// <returns>The list of all the <see cref="RelatedCodeModel" /> that meet the specified criteria.</returns>
        public IList<RelatedCodeModel> GetRelatedCodes(string relatedCodeType, string searchCode, bool dominantSearch, bool currentCodesOnly, bool exactLookup)
        {
            return GetRelatedCodes(relatedCodeType, searchCode, dominantSearch, currentCodesOnly, exactLookup, 0);
        }

        /// <summary>
        /// Returns a list that contains all the <see cref="RelatedCodeModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="relatedCodeType">The type of related code.</param>
        /// <param name="searchCode">The code to search for.</param>
        /// <param name="dominantSearch">Whether it is a dominant search.</param>
        /// <param name="currentCodesOnly">Whether to use current codes only (default is true).</param>
        /// <param name="exactLookup">Whether to do an exact lookup (default is true).</param>
        /// <param name="maxRows">The maximum number of rows to return (0 is all).</param>
        /// <returns>The list of all the <see cref="RelatedCodeModel" /> that meet the specified criteria.</returns>
        public IList<RelatedCodeModel> GetRelatedCodes(string relatedCodeType, string searchCode, bool dominantSearch, bool currentCodesOnly, bool exactLookup, int maxRows)
        {
            return GetRelatedCodesInternal(relatedCodeType, searchCode, dominantSearch, currentCodesOnly, exactLookup, maxRows);
        }
    }
}
