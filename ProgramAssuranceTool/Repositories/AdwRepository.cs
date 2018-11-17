using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Models;

namespace ProgramAssuranceTool.Repositories
{
	/// <summary>
	///   The systems interface with the SqlServer ADW data store
	/// </summary>
	[Serializable]
	public class AdwRepository : IAdwRepository
	{
		public string DbConnectionName { get; set; }

		public string DbConnectionString { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="AdwRepository"/> class.
		/// </summary>
		public AdwRepository()
		{
			DbConnectionName = "Db_ConnADW";
			DbConnectionString = ConfigurationManager.ConnectionStrings[ DbConnectionName ].ConnectionString;
		}

		#region IAdwRepository Members

		/// <summary>
		/// Determines whether the code is valid.
		/// </summary>
		/// <param name="listCode">The list code.</param>
		/// <param name="codeValue">The code value.</param>
		/// <returns>
		/// valid or not
		/// </returns>
		public bool IsCodeValid( string listCode, string codeValue )
		{
			if ( String.IsNullOrEmpty( listCode ) || String.IsNullOrEmpty( codeValue ) )
				return false;

			var desc = GetDescription( listCode, codeValue );
			return ( desc != null && desc.Trim().Length > 0 && desc != "Not Found" );
		}

		/// <summary>
		/// Gets the description of a particular code.
		/// </summary>
		/// <param name="listCode">The list code.</param>
		/// <param name="codeValue">The code value.</param>
		/// <param name="currentCodeOnly">Whether to use current codes only (default is true).</param>
		/// <returns>
		/// description (long)
		/// </returns>
		public string GetDescription( string listCode, string codeValue, bool currentCodeOnly = true )
		{
			if ( string.IsNullOrEmpty( codeValue ) )
				return string.Empty;

			if ( listCode.Equals( DataConstants.AdwListCodeForProjectTypes ) )
			{
				var items = ListCode( listCode );
				foreach ( var i in items.Where( i => i.Value.Equals( codeValue ) ) )
				{
					return i.Text;
				}
				return string.Empty;
			}
			var description = string.Empty;
			using ( var connection = new SqlConnection( DbConnectionString ) )
			{
				var sqlparams = new SqlParameter[ 4 ];
				sqlparams[ 0 ] = new SqlParameter( "@Code_Type", SqlDbType.VarChar ) { Value = listCode };
				sqlparams[ 1 ] = new SqlParameter( "@StartingCode", SqlDbType.VarChar ) { Value = codeValue };
				sqlparams[ 2 ] = new SqlParameter("@ExactLookup", SqlDbType.VarChar) { Value = "Y" };
				sqlparams[ 3 ] = new SqlParameter("@ListType", SqlDbType.Char) { Value = currentCodeOnly ? "c" : string.Empty };
				connection.Open();
				using ( var command = new SqlCommand( "up_ListCode", connection ) )
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlparams );
					using ( var rdr = command.ExecuteReader() )
					{
						while ( rdr.Read() )
							description = rdr[ "long_desc" ].ToString();

						rdr.Close();
					}
				}
				return description;
			}
		}

		/// <summary>
		/// Gets the short descriptionof a particular code.
		/// </summary>
		/// <param name="listCode">The list code.</param>
		/// <param name="codeValue">The code value.</param>
		/// <returns>
		/// description (short)
		/// </returns>
		public string GetShortDescription( string listCode, string codeValue )
		{
			if ( string.IsNullOrEmpty( codeValue ) )
				return string.Empty;
			string description = string.Empty;
			using ( var connection = new SqlConnection( DbConnectionString ) )
			{
				var sqlparams = new SqlParameter[ 3 ];
				sqlparams[ 0 ] = new SqlParameter( "@Code_Type", SqlDbType.VarChar ) { Value = listCode };
				sqlparams[ 1 ] = new SqlParameter( "@StartingCode", SqlDbType.VarChar ) { Value = codeValue };
				sqlparams[ 2 ] = new SqlParameter( "@ExactLookup", SqlDbType.VarChar ) { Value = "Y" };
				connection.Open();
				using ( var command = new SqlCommand( "up_ListCode", connection ) )
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlparams );
					using ( var rdr = command.ExecuteReader() )
					{
						while ( rdr.Read() )
							description = rdr[ "short_desc" ].ToString();

						rdr.Close();
					}
				}
				return description;
			}
		}

		/// <summary>
		/// Get all the ADW items for a particular list code
		/// </summary>
		/// <param name="adwCodeTable">The adw code table.</param>
		/// <param name="blankItem">if set to <c>true</c> add a blank item.</param>
		/// <param name="goLong">if set to <c>true</c> use the long description rather than the short description.</param>
		/// <returns>
		/// A collection of selection items
		/// </returns>
		public List<SelectListItem> ListCode( string adwCodeTable, bool blankItem, bool goLong )
		{
			var response = new List<SelectListItem>();

			using ( var connection = new SqlConnection( DbConnectionString ) )
			{
				var sqlparams = new SqlParameter[ 4 ];

				sqlparams[ 0 ] = new SqlParameter( "@Code_Type", SqlDbType.VarChar ) { Value = adwCodeTable };

				sqlparams[ 1 ] = new SqlParameter( "@ListType", SqlDbType.VarChar ) { Value = "C" };

				sqlparams[ 2 ] = new SqlParameter( "@ExactLookup", SqlDbType.Char ) { Value = "n" };

				sqlparams[ 3 ] = new SqlParameter( "@return_value", SqlDbType.Int ) { Direction = ParameterDirection.ReturnValue };

				connection.Open();
				using ( var command = new SqlCommand( "up_ListCode", connection ) )
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlparams );

					using ( var rdr = command.ExecuteReader() )
					{
						while ( rdr.Read() )
						{
							var pcl = new SelectListItem
								{
									Text = rdr[ WhichDescription( goLong ) ].ToString(),
									Value = rdr[ "code" ].ToString()
								};
							response.Add( pcl );
						}
						rdr.Close();
					}
					return response;
				}
			}
		}

		private static string WhichDescription( bool goLong )
		{
			return goLong ? "long_desc" : "short_desc";
		}

		/// <summary>
		/// Get all the ADW items for a particular list code
		/// </summary>
		/// <param name="adwCodeTable">The adw code table.</param>
		/// <returns>
		/// List of Adw Items
		/// </returns>
		public List<AdwItem> ListAdwCode( string adwCodeTable )
		{
			var response = new List<AdwItem>();

			using ( var connection = new SqlConnection( DbConnectionString ) )
			{
				var sqlparams = new SqlParameter[ 4 ];

				sqlparams[ 0 ] = new SqlParameter( "@Code_Type", SqlDbType.VarChar ) { Value = adwCodeTable };

				sqlparams[ 1 ] = new SqlParameter( "@ListType", SqlDbType.VarChar ) { Value = "C" };

				sqlparams[ 2 ] = new SqlParameter( "@ExactLookup", SqlDbType.Char ) { Value = "n" };

				sqlparams[ 3 ] = new SqlParameter( "@return_value", SqlDbType.Int ) { Direction = ParameterDirection.ReturnValue };

				connection.Open();
				using ( var command = new SqlCommand( "up_ListCode", connection ) )
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlparams );

					using ( var rdr = command.ExecuteReader() )
					{
						while ( rdr.Read() )
						{
							var pcl = new AdwItem
							{
								Description = rdr[ "long_desc" ].ToString(),
								Code = rdr[ "code" ].ToString()
							};
							response.Add( pcl );
						}
						rdr.Close();
					}
					return response;
				}
			}
		}

		/// <summary>
		/// Get all the ADW items for a particular list code
		/// </summary>
		/// <param name="adwCodeTable">The adw code table.</param>
		/// <returns>
		/// A collection of selection items
		/// </returns>
		public List<SelectListItem> ListCode( string adwCodeTable )
		{
			var response = new List<SelectListItem>();

			using ( var connection = new SqlConnection( DbConnectionString ) )
			{
				var sqlparams = new SqlParameter[ 4 ];

				sqlparams[ 0 ] = new SqlParameter( "@Code_Type", SqlDbType.VarChar ) { Value = adwCodeTable };

				sqlparams[ 1 ] = new SqlParameter( "@ListType", SqlDbType.VarChar ) { Value = "C" };

				sqlparams[ 2 ] = new SqlParameter( "@ExactLookup", SqlDbType.Char ) { Value = "n" };

				sqlparams[ 3 ] = new SqlParameter( "@return_value", SqlDbType.Int ) { Direction = ParameterDirection.ReturnValue };

				connection.Open();
				using ( var command = new SqlCommand( "up_ListCode", connection ) )
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlparams );

					using ( var rdr = command.ExecuteReader() )
					{
						while ( rdr.Read() )
						{
							var pcl = new SelectListItem
								{
									Text = rdr[ "long_desc" ].ToString(),
									Value = rdr[ "code" ].ToString()
								};
							response.Add( pcl );
						}
						rdr.Close();
					}
					return response;
				}
			}
		}

		/// <summary>
		/// Get all the ADW items for a particular related list code
		/// </summary>
		/// <param name="adwRelatedCodeTable">The adw related code table.</param>
		/// <param name="adwSearchCode">The adw search code.</param>
		/// <param name="blankItem">if set to <c>true</c> add a blank item.</param>
		/// <param name="exactMatch">The exact match option.</param>
		/// <returns>
		/// A collection of selection items
		/// </returns>
		public List<SelectListItem> ListRelatedCode( string adwRelatedCodeTable, string adwSearchCode, bool blankItem, string exactMatch )
		{
			var response = new List<SelectListItem>();
			if ( blankItem )
				response.Add( new SelectListItem
					{
						Value = " ",
						Text = " "
					} );

			using ( var connection = new SqlConnection( DbConnectionString ) )
			{
				var sqlparams = new SqlParameter[ 6 ];

				sqlparams[ 0 ] = new SqlParameter( "@RelationshipType", SqlDbType.VarChar ) { Value = adwRelatedCodeTable };

				sqlparams[ 1 ] = new SqlParameter( "@ListType", SqlDbType.VarChar ) { Value = "C" };

				sqlparams[ 2 ] = new SqlParameter( "@SearchType", SqlDbType.VarChar ) { Value = "d" };

				sqlparams[ 3 ] = new SqlParameter( "@SearchCode", SqlDbType.VarChar ) { Value = adwSearchCode };

				sqlparams[ 4 ] = new SqlParameter( "@ExactLookup", SqlDbType.VarChar ) { Value = exactMatch };

				sqlparams[ 5 ] = new SqlParameter( "@return_value", SqlDbType.Int ) { Direction = ParameterDirection.ReturnValue };

				connection.Open();
				using ( var command = new SqlCommand( "up_ListRelatedCode", connection ) )
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddRange( sqlparams );

					using ( var rdr = command.ExecuteReader() )
					{
						while ( rdr.Read() )
						{
							var pcl = new SelectListItem
								{
									Value = rdr[ "sub_code" ].ToString(),
									Text = rdr[ "sub_long_desc" ].ToString()
								};
							response.Add( pcl );
						}
						rdr.Close();
					}

					return response;
				}
			}
		}

		/// <summary>
		/// Get all the ADW items for a particular list code
		/// </summary>
		/// <param name="adwCodeTable">The adw code table.</param>
		/// <param name="blank">if set to <c>true</c> [blank].</param>
		/// <param name="goLong">if set to <c>true</c> [go long].</param>
		/// <returns>
		/// an array of code descriptions
		/// </returns>
		public string[] ListCodeArray( string adwCodeTable, bool blank, bool goLong )
		{
			var values = ListCode( adwCodeTable, blank, goLong );
			var myArray = new string[ values.Count ];
			var counter = 0;
			foreach ( var v in values )
			{
				myArray[ counter ] = v.Text;
				counter++;
			}
			return myArray;
		}

		/// <summary>
		/// Get all the ADW items for a particular list code
		/// </summary>
		/// <param name="adwCodeTable">The adw code table.</param>
		/// <param name="blank">if set to <c>true</c> adds a blank item.</param>
		/// <param name="goLong">if set to <c>true</c> uses long description.</param>
		/// <returns>
		/// a dictionary of codes and descriptions
		/// </returns>
		public IDictionary<string, string> ListCodeDictionary( string adwCodeTable, bool blank, bool goLong )
		{
			var values = ListCode( adwCodeTable, blank, goLong );
			return values.ToDictionary( v => v.Value, v => v.Text );
		}

		#endregion IAdwRepository Members

		#region Orgs and Sites

		/// <summary>
		/// Gets the name of the org.
		/// </summary>
		/// <param name="orgCode">The org code.</param>
		/// <returns>
		/// Org name
		/// </returns>
		public string GetOrgName( string orgCode )
		{
			//  try the static data first
			var orgName = GetOrg( orgCode );
			if (string.IsNullOrEmpty( orgName ))
				return orgName;
			orgName = GetDescription( DataConstants.AdwListCodeForOrgCodes, orgCode );
			if ( string.IsNullOrEmpty( orgName ) )
				orgName = orgCode + "?";
			return orgName;
		}

		/// <summary>
		/// Gets the name of the site.
		/// </summary>
		/// <param name="siteCode">The site code.</param>
		/// <returns></returns>
		public string GetSiteName( string siteCode )
		{
			var siteName = GetDescription( DataConstants.AdwListCodeForSiteCodes, siteCode );
			if ( string.IsNullOrEmpty( siteName ) )
				siteName = siteCode + "?";
			return siteName;
		}

		/// <summary>
		/// Finds the org.
		/// </summary>
		/// <param name="searchText">The search text.</param>
		/// <param name="maxResults">The maximum results.</param>
		/// <returns></returns>
		internal List<AdwItem> FindOrg( string searchText, int maxResults )
		{
			var result = from o in AdwOrgList
							 where o.Code.IndexOf( searchText, StringComparison.OrdinalIgnoreCase ) >= 0
							 orderby o.Code
							 select o;

			return result.Take( maxResults ).ToList();
		}

		internal string GetOrg( string orgCode )
		{
			var result = from o in AdwOrgList
							 where o.Code.Equals( orgCode )
							 select o;


			var firstOrDefault = result.FirstOrDefault();
			return firstOrDefault != null 
				? firstOrDefault.Description : string.Format( "Could not find Name for {0}", orgCode );
		}

		private static List<AdwItem> _adwOrgList;

		internal List<AdwItem> AdwOrgList
		{
			get
			{
				if ( _adwOrgList == null || _adwOrgList.Count < 1 )
					_adwOrgList = ListAdwCode( DataConstants.AdwListCodeForOrgCodes );
				return _adwOrgList;
			}
		}

		// to improve the performance we are using this static variable to keep and therefore populate the OrgList once
		private static List<SelectListItem> _orgList;

		internal List<SelectListItem> OrgList
		{
			get
			{
				if ( _orgList == null || _orgList.Count < 1 )
					_orgList = ListCode( DataConstants.AdwListCodeForOrgCodes );
				return _orgList;
			}
		}

		internal List<SelectListItem> LookupOrg( string searchText, int maxResults )
		{
			var result = from o in OrgList
							 where o.Value.IndexOf( searchText, StringComparison.OrdinalIgnoreCase ) >= 0
							 orderby o.Value
							 select o;

			return result.Take( maxResults ).ToList();
		}

		// to improve the performance we are using this static variable to keep and therefore populate the ESAList once
		private static List<SelectListItem> _esaList;

		internal List<SelectListItem> ESAList
		{
			get
			{
				if ( _esaList == null || _esaList.Count < 1 )
					_esaList = ListCode( DataConstants.AdwListCodeForEsaCodes );
				return _esaList;
			}
		}

		/// <summary>
		/// Retrieve ESA list that match searchText and OrgCode
		/// </summary>
		/// <param name="searchText"></param>
		/// <param name="orgCode"></param>
		/// <param name="maxResults"></param>
		/// <returns></returns>
		internal List<SelectListItem> LookupESA( string searchText, string orgCode, int maxResults )
		{
			var result = from o in ESAList
							 where o.Value.IndexOf( searchText, StringComparison.OrdinalIgnoreCase ) >= 0
							 orderby o.Value
							 select o;

			return result.Take( maxResults ).ToList();
		}

		// to improve the performance we are using this static variable to keep and therefore populate the SiteList once
		private static List<SelectListItem> _siteList;

		internal List<SelectListItem> SiteList
		{
			get
			{
				if ( _siteList == null || _siteList.Count < 1 )
					_siteList = ListCode( DataConstants.AdwListCodeForSiteCodes );
				return _siteList;
			}
		}

		internal List<SelectListItem> LookupSite( string searchText, string orgCode, int maxResults )
		{
			var result = from o in SiteList
							 where o.Value.IndexOf( searchText, StringComparison.OrdinalIgnoreCase ) >= 0
							 orderby o.Value
							 select o;

			return result.Take( maxResults ).ToList();
		}

		// to improve the performance we are using this static variable to keep and therefore populate the SiteList once
		private static List<SelectListItem> _claimTypeList;

		// we are using a short list of claim types now not the whole list
		internal List<SelectListItem> ClaimTypeList
		{
			get
			{
				if ( _claimTypeList == null || _claimTypeList.Count < 1 )
				   _claimTypeList = ListRelatedCode( DataConstants.AdwRelatedListCodeForPatClaimTypes, "IND", false, "N" );
				return _claimTypeList;
			}
		}

		/// <summary>
		/// Lookups the type of the claim.
		/// </summary>
		/// <param name="searchText">The search text.</param>
		/// <param name="claimType">Type of the claim.</param>
		/// <param name="maxResults">The maximum results.</param>
		/// <returns></returns>
		public List<SelectListItem> LookupClaimType( string searchText, string claimType, int maxResults )
		{
			var result = from o in ClaimTypeList
							 where o.Value.IndexOf( searchText, StringComparison.OrdinalIgnoreCase ) >= 0
							 orderby o.Value
							 select o;

			return result.Take( maxResults ).ToList();
		}

		#endregion Orgs and Sites
	}
}