using System.Collections.Generic;
using Employment.Web.Mvc.Infrastructure.Models;

namespace Employment.Web.Mvc.Infrastructure.Interfaces
{
    /// <summary>
    /// Defines the methods and properties that are required for an Adw Service.
    /// </summary>
    public interface IAdwService
    {
        /// <summary>
        /// Returns the value of the property that matches the specified criteria.
        /// </summary>
        /// <param name="codeType">The type of code.</param>
        /// <param name="code">The code.</param>
        /// <param name="propertyType">The type of property.</param>
        /// <returns>The property value.</returns>
        string GetPropertyValue(string codeType, string code, string propertyType);

        /// <summary>
        /// Returns the value of the property that matches the specified criteria.
        /// </summary>
        /// <param name="codeType">The type of code.</param>
        /// <param name="code">The code.</param>
        /// <param name="propertyType">The type of property.</param>
        /// <param name="currentCodesOnly">Whether to use current codes only (default is true).</param>
        /// <returns>The property value.</returns>
        string GetPropertyValue(string codeType, string code, string propertyType, bool currentCodesOnly);

        /// <summary>
        /// Returns the long description of the list code that matches the specified criteria.
        /// </summary>
        /// <param name="codeType">The type of code.</param>
        /// <param name="code">The code.</param>
        /// <returns>The the long description of the list code that matches the specified criteria; otherwise, an empty string.</returns>
        string GetListCodeDescription(string codeType, string code);

        /// <summary>
        /// Returns the long description of the list code that matches the specified criteria.
        /// </summary>
        /// <param name="codeType">The type of code.</param>
        /// <param name="code">The code.</param>
        /// <param name="currentCodesOnly">Whether to use current codes only (default is true).</param>
        /// <returns>The the long description of the list code that matches the specified criteria; otherwise, an empty string.</returns>
        string GetListCodeDescription(string codeType, string code, bool currentCodesOnly);

        /// <summary>
        /// Returns the short description of the list code that matches the specified criteria.
        /// </summary>
        /// <param name="codeType">The type of code.</param>
        /// <param name="code">The code.</param>
        /// <returns>The short description of the list code that matches the specified criteria; otherwise, an empty string.</returns>
        string GetListCodeDescriptionShort(string codeType, string code);

        /// <summary>
        /// Returns the short description of the list code that matches the specified criteria.
        /// </summary>
        /// <param name="codeType">The type of code.</param>
        /// <param name="code">The code.</param>
        /// <param name="currentCodesOnly">Whether to use current codes only (default is true).</param>
        /// <returns>The short description of the list code that matches the specified criteria; otherwise, an empty string.</returns>
        string GetListCodeDescriptionShort(string codeType, string code, bool currentCodesOnly);

        /// <summary>
        /// Returns a <see cref="CodeModel" /> that matches the specified criteria.
        /// </summary>
        /// <param name="codeType">The type of code.</param>
        /// <param name="code">The code.</param>
        /// <returns>The <see cref="CodeModel" /> that matches the specified criteria; otherwise, null.</returns>
        CodeModel GetListCode(string codeType, string code);

        /// <summary>
        /// Returns a <see cref="CodeModel" /> that matches the specified criteria.
        /// </summary>
        /// <param name="codeType">The type of code.</param>
        /// <param name="code">The code.</param>
        /// <param name="currentCodesOnly">Whether to use current codes only (default is true).</param>
        /// <returns>The <see cref="CodeModel" /> that matches the specified criteria; otherwise, null.</returns>
        CodeModel GetListCode(string codeType, string code, bool currentCodesOnly);

        /// <summary>
        /// Returns a list that contains all the <see cref="CodeModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="codeType">The type of code.</param>
        /// <returns>The list of all the <see cref="CodeModel" /> that meet the specified criteria.</returns>
        IList<CodeModel> GetListCodes(string codeType);

        /// <summary>
        /// Returns a list that contains all the <see cref="CodeModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="codeType">The type of code.</param>
        /// <param name="currentCodesOnly">Whether to use current codes only (default is true).</param>
        /// <returns>The list of all the <see cref="CodeModel" /> that meet the specified criteria.</returns>
        IList<CodeModel> GetListCodes(string codeType, bool currentCodesOnly);

        /// <summary>
        /// Returns a list that contains all the <see cref="CodeModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="codeType">The type of code.</param>
        /// <param name="startingCode">The code to start at.</param>
        /// <returns>The list of all the <see cref="CodeModel" /> that meet the specified criteria.</returns>
        IList<CodeModel> GetListCodes(string codeType, string startingCode);

        /// <summary>
        /// Returns a list that contains all the <see cref="CodeModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="codeType">The type of code.</param>
        /// <param name="startingCode">The code to start at.</param>
        /// <param name="currentCodesOnly">Whether to use current codes only (default is true).</param>
        /// <returns>The list of all the <see cref="CodeModel" /> that meet the specified criteria.</returns>
        IList<CodeModel> GetListCodes(string codeType, string startingCode, bool currentCodesOnly);

        /// <summary>
        /// Returns the long description of the related code that matches the specified criteria.
        /// </summary>
        /// <param name="relatedCodeType">The type of related code.</param>
        /// <param name="searchCode">The code to search for.</param>
        /// <param name="code">The code.</param>
        /// <returns>The long description of the related code that matches the specified criteria; otherwise, an empty string.</returns>
        string GetRelatedCodeDescription(string relatedCodeType, string searchCode, string code);

        /// <summary>
        /// Returns the long description of the related code that matches the specified criteria.
        /// </summary>
        /// <param name="relatedCodeType">The type of related code.</param>
        /// <param name="searchCode">The code to search for.</param>
        /// <param name="code">The code.</param>
        /// <param name="dominantSearch">Whether it is a dominant search (default is true).</param>
        /// <returns>The long description of the related code that matches the specified criteria; otherwise, an empty string.</returns>
        string GetRelatedCodeDescription(string relatedCodeType, string searchCode, string code, bool dominantSearch);

        /// <summary>
        /// Returns the long description of the related code that matches the specified criteria.
        /// </summary>
        /// <param name="relatedCodeType">The type of related code.</param>
        /// <param name="searchCode">The code to search for.</param>
        /// <param name="code">The code.</param>
        /// <param name="dominantSearch">Whether it is a dominant search (default is true).</param>
        /// <param name="currentCodesOnly">Whether to use current codes only (default is true).</param>
        /// <returns>The long description of the related code that matches the specified criteria; otherwise, an empty string.</returns>
        string GetRelatedCodeDescription(string relatedCodeType, string searchCode, string code, bool dominantSearch, bool currentCodesOnly);

        /// <summary>
        /// Returns the short description of the related code that matches the specified criteria.
        /// </summary>
        /// <param name="relatedCodeType">The type of related code.</param>
        /// <param name="searchCode">The code to search for.</param>
        /// <param name="code">The code.</param>
        /// <returns>The short description of the related code that matches the specified criteria; otherwise, an empty string.</returns>
        string GetRelatedCodeDescriptionShort(string relatedCodeType, string searchCode, string code);

        /// <summary>
        /// Returns the short description of the related code that matches the specified criteria.
        /// </summary>
        /// <param name="relatedCodeType">The type of related code.</param>
        /// <param name="searchCode">The code to search for.</param>
        /// <param name="code">The code.</param>
        /// <param name="dominantSearch">Whether it is a dominant search (default is true).</param>
        /// <returns>The short description of the related code that matches the specified criteria; otherwise, an empty string.</returns>
        string GetRelatedCodeDescriptionShort(string relatedCodeType, string searchCode, string code, bool dominantSearch);

        /// <summary>
        /// Returns the short description of the related code that matches the specified criteria.
        /// </summary>
        /// <param name="relatedCodeType">The type of related code.</param>
        /// <param name="searchCode">The code to search for.</param>
        /// <param name="code">The code.</param>
        /// <param name="dominantSearch">Whether it is a dominant search (default is true).</param>
        /// <param name="currentCodesOnly">Whether to use current codes only (default is true).</param>
        /// <returns>The short description of the related code that matches the specified criteria; otherwise, an empty string.</returns>
        string GetRelatedCodeDescriptionShort(string relatedCodeType, string searchCode, string code, bool dominantSearch, bool currentCodesOnly);

        /// <summary>
        /// Returns a <see cref="RelatedCodeModel" /> that matches the specified criteria using a dominant search.
        /// </summary>
        /// <param name="relatedCodeType">The type of related code.</param>
        /// <param name="searchCode">The code to search for.</param>
        /// <param name="code">The code.</param>
        /// <returns>The <see cref="RelatedCodeModel" /> that matches the specified criteria; otherwise, null.</returns>
        RelatedCodeModel GetRelatedCode(string relatedCodeType, string searchCode, string code);

        /// <summary>
        /// Returns a <see cref="RelatedCodeModel" /> that matches the specified criteria.
        /// </summary>
        /// <param name="relatedCodeType">The type of related code.</param>
        /// <param name="searchCode">The code to search for.</param>
        /// <param name="dominantSearch">Whether it is a dominant search (default is true).</param>
        /// <param name="code">The code.</param>
        /// <returns>The <see cref="RelatedCodeModel" /> that matches the specified criteria; otherwise, null.</returns>
        RelatedCodeModel GetRelatedCode(string relatedCodeType, string searchCode, string code, bool dominantSearch);

        /// <summary>
        /// Returns a <see cref="RelatedCodeModel" /> that matches the specified criteria.
        /// </summary>
        /// <param name="relatedCodeType">The type of related code.</param>
        /// <param name="searchCode">The code to search for.</param>
        /// <param name="dominantSearch">Whether it is a dominant search (default is true).</param>
        /// <param name="code">The code.</param>
        /// <param name="currentCodesOnly">Whether to use current codes only (default is true).</param>
        /// <returns>The <see cref="RelatedCodeModel" /> that matches the specified criteria; otherwise, null.</returns>
        RelatedCodeModel GetRelatedCode(string relatedCodeType, string searchCode, string code, bool dominantSearch, bool currentCodesOnly);

        /// <summary>
        /// Returns a list that contains all the <see cref="RelatedCodeModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="relatedCodeType">The type of related code.</param>
        /// <returns>The list of all the <see cref="RelatedCodeModel" /> that meet the specified criteria.</returns>
        IList<RelatedCodeModel> GetRelatedCodes(string relatedCodeType);

        /// <summary>
        /// Returns a list that contains all the <see cref="RelatedCodeModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="relatedCodeType">The type of related code.</param>
        /// <param name="currentCodesOnly">Whether to use current codes only (default is true).</param>
        /// <returns>The list of all the <see cref="RelatedCodeModel" /> that meet the specified criteria.</returns>
        IList<RelatedCodeModel> GetRelatedCodes(string relatedCodeType, bool currentCodesOnly);

        /// <summary>
        /// Returns a list that contains all the <see cref="RelatedCodeModel" /> that meet the specified criteria using a dominant search.
        /// </summary>
        /// <param name="relatedCodeType">The type of related code.</param>
        /// <param name="searchCode">The code to search for.</param>
        /// <returns>The list of all the <see cref="RelatedCodeModel" /> that meet the specified criteria.</returns>
        IList<RelatedCodeModel> GetRelatedCodes(string relatedCodeType, string searchCode);

        /// <summary>
        /// Returns a list that contains all the <see cref="RelatedCodeModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="relatedCodeType">The type of related code.</param>
        /// <param name="searchCode">The code to search for.</param>
        /// <param name="dominantSearch">Whether it is a dominant search (default is true).</param>
        /// <returns>The list of all the <see cref="RelatedCodeModel" /> that meet the specified criteria.</returns>
        IList<RelatedCodeModel> GetRelatedCodes(string relatedCodeType, string searchCode, bool dominantSearch);

        /// <summary>
        /// Returns a list that contains all the <see cref="RelatedCodeModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="relatedCodeType">The type of related code.</param>
        /// <param name="searchCode">The code to search for.</param>
        /// <param name="dominantSearch">Whether it is a dominant search.</param>
        /// <param name="currentCodesOnly">Whether to use current codes only (default is true).</param>
        /// <returns>The list of all the <see cref="RelatedCodeModel" /> that meet the specified criteria.</returns>
        IList<RelatedCodeModel> GetRelatedCodes(string relatedCodeType, string searchCode, bool dominantSearch, bool currentCodesOnly);

        /// <summary>
        /// Returns a list that contains all the <see cref="UserModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="organisationCode">The organisation code the users are in.</param>
        /// <returns>The list of all the <see cref="UserModel" /> that meet the specified criteria.</returns>
        IList<UserModel> GetUsersInOrganisation(string organisationCode);

        /// <summary>
        /// Returns a list that contains all the <see cref="UserModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="organisationCode">The organisation code the users are in.</param>
        /// <param name="userIDPattern">A user ID pattern.</param>
        /// <param name="namePattern">A name pattern.</param>
        /// <returns>The list of all the <see cref="UserModel" /> that meet the specified criteria.</returns>
        IList<UserModel> GetUsersInOrganisation(string organisationCode, string userIDPattern, string namePattern);

        /// <summary>
        /// Returns a list that contains all the <see cref="UserModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="organisationCode">The organisation code the users are in.</param>
        /// <param name="role">The role the users are in.</param>
        /// <returns>The list of all the <see cref="UserModel" /> that meet the specified criteria.</returns>
        IList<UserModel> GetUsersInOrganisationWithRole(string organisationCode, string role);

        /// <summary>
        /// Returns a list that contains all the <see cref="UserModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="organisationCode">The organisation code the users are in.</param>
        /// <param name="role">The role the users are in.</param>
        /// <param name="userIDPattern">A user ID pattern.</param>
        /// <param name="namePattern">A name pattern.</param>
        /// <returns>The list of all the <see cref="UserModel" /> that meet the specified criteria.</returns>
        IList<UserModel> GetUsersInOrganisationWithRole(string organisationCode, string role, string userIDPattern, string namePattern);

        /// <summary>
        /// Returns a list that contains all the <see cref="UserModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="siteCode">The site code the users are in.</param>
        /// <returns>The list of all the <see cref="UserModel" /> that meet the specified criteria.</returns>
        IList<UserModel> GetUsersInSite(string siteCode);

        /// <summary>
        /// Returns a list that contains all the <see cref="UserModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="siteCode">The site code the users are in.</param>
        /// <param name="userIDPattern">A user ID pattern.</param>
        /// <param name="namePattern">A name pattern.</param>
        /// <returns>The list of all the <see cref="UserModel" /> that meet the specified criteria.</returns>
        IList<UserModel> GetUsersInSite(string siteCode, string userIDPattern, string namePattern);

        /// <summary>
        /// Returns a list that contains all the <see cref="UserModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="siteCode">The site code the users are in.</param>
        /// <param name="role">The role the users are in.</param>
        /// <returns>The list of all the <see cref="UserModel" /> that meet the specified criteria.</returns>
        IList<UserModel> GetUsersInSiteWithRole(string siteCode, string role);

        /// <summary>
        /// Returns a list that contains all the <see cref="UserModel" /> that meet the specified criteria.
        /// </summary>
        /// <param name="siteCode">The site code the users are in.</param>
        /// <param name="role">The role the users are in.</param>
        /// <param name="userIDPattern">A user ID pattern.</param>
        /// <param name="namePattern">A name pattern.</param>
        /// <returns>The list of all the <see cref="UserModel" /> that meet the specified criteria.</returns>
        IList<UserModel> GetUsersInSiteWithRole(string siteCode, string role, string userIDPattern, string namePattern);


        /// <summary>
        /// Gets the name of user based on the User ID supplied.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <returns>User Name.</returns>
        UserModel GetUserDetails(string userId);
    }
}
