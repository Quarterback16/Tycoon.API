using System.Collections.Generic;
using System.Web.Mvc;
using ProgramAssuranceTool.Models;

namespace ProgramAssuranceTool.Interfaces
{
	public interface IAdwRepository
	{
		/// <summary>
		///   Get all the ADW items for a particular list code
		/// </summary>
		/// <param name="adwCodeTable">The adw code table.</param>
		/// <returns>List of Adw Items</returns>
		List<AdwItem> ListAdwCode( string adwCodeTable );

		/// <summary>
		///   Get all the ADW items for a particular list code
		/// </summary>
		/// <param name="adwCodeTable">The adw code table.</param>
		/// <returns>A collection of selection items</returns>
		List<SelectListItem> ListCode( string adwCodeTable );

		/// <summary>
		///   Get all the ADW items for a particular list code
		/// </summary>
		/// <param name="adwCodeTable">The adw code table.</param>
		/// <param name="blankItem">if set to <c>true</c> add a blank item.</param>
		/// <param name="goLong">if set to <c>true</c> use the long description rather than the short description.</param>
		/// <returns>A collection of selection items</returns>
		List<SelectListItem> ListCode( string adwCodeTable, bool blankItem, bool goLong );

		/// <summary>
		///   Get all the ADW items for a particular related list code
		/// </summary>
		/// <param name="adwRelatedCodeTable">The adw related code table.</param>
		/// <param name="adwSearchCode">The adw search code.</param>
		/// <param name="blankItem">if set to <c>true</c> add a blank item.</param>
		/// <param name="exactMatch">The exact match option.</param>
		/// <returns>A collection of selection items</returns>
		List<SelectListItem> ListRelatedCode( string adwRelatedCodeTable, string adwSearchCode, bool blankItem, string exactMatch );

		/// <summary>
		/// Gets the description of a particular code.
		/// </summary>
		/// <param name="listCode">The list code.</param>
		/// <param name="codeValue">The code value.</param>
		/// <param name="currentCodeOnly">Whether to use current codes only (default is true).</param>
		/// <returns>description (long)</returns>
		string GetDescription( string listCode, string codeValue, bool currentCodeOnly = true );

		/// <summary>
		/// Gets the short descriptionof a particular code.
		/// </summary>
		/// <param name="listCode">The list code.</param>
		/// <param name="codeValue">The code value.</param>
		/// <returns>description (short)</returns>
		string GetShortDescription( string listCode, string codeValue );

		/// <summary>
		/// Determines whether the code is valid.
		/// </summary>
		/// <param name="listCode">The list code.</param>
		/// <param name="codeValue">The code value.</param>
		/// <returns>valid or not</returns>
		bool IsCodeValid( string listCode, string codeValue );

		/// <summary>
		///   Get all the ADW items for a particular list code
		/// </summary>
		/// <param name="adwCodeTable">The adw code table.</param>
		/// <param name="blank">if set to <c>true</c> [blank].</param>
		/// <param name="goLong">if set to <c>true</c> [go long].</param>
		/// <returns>an array of code descriptions</returns>
		string[] ListCodeArray( string adwCodeTable, bool blank, bool goLong );

		/// <summary>
		///   Get all the ADW items for a particular list code
		/// </summary>
		/// <param name="adwCodeTable">The adw code table.</param>
		/// <param name="blank">if set to <c>true</c> adds a blank item.</param>
		/// <param name="goLong">if set to <c>true</c> uses long description.</param>
		/// <returns>a dictionary of codes and descriptions</returns>
		IDictionary<string, string> ListCodeDictionary( string adwCodeTable, bool blank, bool goLong );

		/// <summary>
		/// Gets the name of the org.
		/// </summary>
		/// <param name="orgCode">The org code.</param>
		/// <returns>Org name</returns>
		string GetOrgName( string orgCode );

		/// <summary>
		/// Lookups the type of the claim.
		/// </summary>
		/// <param name="codeToSearch">The code to search.</param>
		/// <param name="esaCode">The esa code.</param>
		/// <param name="maxResults">The maximum results.</param>
		/// <returns></returns>
		List<SelectListItem> LookupClaimType( string codeToSearch, string esaCode, int maxResults );
	}
}
