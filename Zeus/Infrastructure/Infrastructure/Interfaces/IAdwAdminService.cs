using System.Collections.Generic;
using System.Linq;
using Employment.Web.Mvc.Infrastructure.Models;
using System;
using Employment.Web.Mvc.Infrastructure.Models.AdwLookup;

namespace Employment.Web.Mvc.Infrastructure.Interfaces
{
    /// <summary>
    /// Defines the methods and properties that are required for an Adw Service.
    /// </summary>
    public interface IAdwAdminService : IAdwService
    {

        /// <summary>
        /// Returns a list containing all the <see cref="CodeTypeModel"/> that meets the given criteria.
        /// </summary>
        /// <param name="startingTableType">Starting Table Type.</param>
        /// <param name="listType">Whether to use current codes only (default is all). Values: '' (all), 'C' current codes, 'E' ended codes.</param>
        /// <param name="exactLookup">Whether to do an exact lookup (default is true).</param>
        /// <param name="maxRows">Maximum rows to be returned. Default is 0 indicating unlimited.</param>
        /// <returns>A list of all the <see cref="CodeTypeModel"/> that meets the specified criteria.</returns>
        IList<CodeTypeModel> GetListCodeTypes(string startingTableType, char listType, bool exactLookup, int maxRows = 0);


        /// <summary>
        /// Returns a list containing all the <see cref="RelatedCodeTypeModel"/> that meets the given criteria.
        /// </summary>
        /// <param name="startingTableType">Starting Table Type.</param>
        /// <param name="listType">Whether to use current codes only (default is all). Values: '' (all), 'C' current codes, 'E' ended codes.</param>
        /// <param name="exactLookup">Whether to do an exact lookup (default is true).</param>
        /// <param name="maxRows">Maximum rows to be returned. Default is 0 indicating unlimited.</param>
        /// <returns>A list of all the <see cref="CodeTypeModel"/> that meets the specified criteria.</returns>
        IList<RelatedCodeTypeModel> GetRelatedListCodeTypes(string startingTableType, char listType, bool exactLookup, int maxRows = 0);

        /// <summary>
        /// Returns the List of the properties that match the specified criteria.
        /// </summary>
        /// <param name="startCodeType">The type of code.</param>
        /// <param name="startCode">The code.</param>
        /// <param name="startPropertyType">The type of property.</param>
        /// <param name="listType">Indicates whether to use current codes only (default is true).</param>
        /// <param name="exactLookup">Exact lookup.</param>
        /// <param name="maxRows">Maximum number of rows to return. Default 0 indicates unlimited.</param>
        /// <returns>The list of property values.</returns>
        IList<PropertyModel> GetPropertyList(string startCodeType, string startCode, string startPropertyType, char listType, bool exactLookup, int maxRows);

        /// <summary>
        /// Returns a list that contains all the <see cref="CodeModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="codeType">The type of code.</param>
        /// <param name="startingCode">The code to start at.</param>
        /// <param name="currentCodesOnly">Whether to use current codes only (default is true).</param>
        /// <param name="exactLookup">Whether to do an exact lookup (default is true).</param>
        /// <returns>The list of all the <see cref="CodeModel" /> that meet the specified criteria.</returns>
        IList<CodeModel> GetListCodes(string codeType, string startingCode, bool currentCodesOnly, bool exactLookup);

        /// <summary>
        /// Returns a list that contains all the <see cref="CodeModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="codeType">The type of code.</param>
        /// <param name="startingCode">The code to start at.</param>
        /// <param name="currentCodesOnly">Whether to use current codes only (default is true).</param>
        /// <param name="exactLookup">Whether to do an exact lookup (default is true).</param>
        /// <param name="maxRows">The maximum number of rows to return (0 is all).</param>
        /// <returns>The list of all the <see cref="CodeModel" /> that meet the specified criteria.</returns>
        IList<CodeModel> GetListCodes(string codeType, string startingCode, bool currentCodesOnly, bool exactLookup, int maxRows);

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
        IList<CodeModel> GetListCodes(string codeType, string startingCode, bool currentCodesOnly, bool exactLookup, int maxRows, bool intOnly);

        /// <summary>
        /// Returns a list that contains all the <see cref="RelatedCodeModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="relatedCodeType">The type of related code.</param>
        /// <param name="searchCode">The code to search for.</param>
        /// <param name="dominantSearch">Whether it is a dominant search.</param>
        /// <param name="currentCodesOnly">Whether to use current codes only (default is true).</param>
        /// <param name="exactLookup">Whether to do an exact lookup (default is true).</param>
        /// <returns>The list of all the <see cref="RelatedCodeModel" /> that meet the specified criteria.</returns>
        IList<RelatedCodeModel> GetRelatedCodes(string relatedCodeType, string searchCode, bool dominantSearch, bool currentCodesOnly, bool exactLookup);

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
        IList<RelatedCodeModel> GetRelatedCodes(string relatedCodeType, string searchCode, bool dominantSearch, bool currentCodesOnly, bool exactLookup, int maxRows);


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
        IList<DeltaModel> GetDeltaList(string codeName, string startCode, DateTime lastUpdateDate, int lastUpdateVersion = 0, int rowPosition = 0, int maxRows = 0, bool wsType = false);


        /// <summary>
        /// Returns the List of the property types that match the specified criteria.
        /// </summary>
        /// <param name="startCode">The start code.</param>
        /// <param name="startProperty">The start property code.</param> 
        /// <param name="listType">Indicates whether to use current codes only (default is true).</param>
        /// <param name="exact">Exact lookup.</param>
        /// <param name="max">Maximum number of rows to return. Default 0 indicates unlimited.</param>
        /// <returns>The list of property types.</returns>
        IList<PropertyModel> GetPropertyTypeList(string startCode, string startProperty, char listType, bool exact = false, int max = 0);
    }
}
