using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using Elmah;
using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Infrastructure.Interfaces;
using ProgramAssuranceTool.Infrastructure.Models;
using ProgramAssuranceTool.Infrastructure.Types;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Models;

namespace ProgramAssuranceTool.Repositories
{
	/// <summary>
	///   The class responsible for interfacing with the User data
	/// </summary>
	public class UserRepository : IUserRepository
	{
		/// <summary>
		/// Gets or sets the audit service.
		/// </summary>
		/// <value>
		/// The audit service.
		/// </value>
		public IAuditService AuditService { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="UserRepository"/> class.
		/// </summary>
		public UserRepository()
		{
			AuditService = null;  //  no auditing
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UserRepository"/> class.
		/// </summary>
		/// <param name="auditService">The audit service.</param>
		public UserRepository( IAuditService auditService )
		{
			AuditService = auditService;
		}

		/// <summary>
		/// Gets the program assurance users.
		/// </summary>
		/// <returns>
		/// list of users
		/// </returns>
		public List<PatUser> GetProgramAssuranceUsers()
		{
			var users = new List<PatUser>();

			users.AddRange( GetAllPAUsersByGroupPaths( GetProgramAssuranceGroups() ) );

			var q = ( from u in users select u ).Distinct( new PatUserComparer() );

			var sortedList = q.OrderBy( x => x.FullName ).ToList();
			return sortedList;
		}

		/// <summary>
		/// Counts the program assurance users.
		/// </summary>
		/// <returns>the number of users</returns>
		public int CountProgramAssuranceUsers()
		{
			var users = GetProgramAssuranceUsers();
			return users.Count;
		}

		/// <summary>
		/// Gets the program assurance users for a particular state code.
		/// </summary>
		/// <param name="stateCode">The state code.</param>
		/// <param name="cacheService">The cache service.</param>
		/// <returns>
		/// list of users
		/// </returns>
		public List<PatUser> GetProgramAssuranceUsers( string stateCode, ICacheService cacheService )
		{
			var users = GetPatUsers( cacheService );

			var q = users.Where( paUser => paUser.InState( stateCode ) ).ToList();

			return q.ToList();
		}

		/// <summary>
		/// Gets the program assurance user ids.
		/// </summary>
		/// <param name="stateCode">The state code.</param>
		/// <param name="cacheService">The cache service.</param>
		/// <returns>list of userIds</returns>
		public List<string> GetProgramAssuranceUserIds( string stateCode, ICacheService cacheService )
		{
			var users = GetProgramAssuranceUsers( stateCode, cacheService );

			return users.Select( user => user.LoginName ).ToList();
		}

		/// <summary>
		/// Gets the program assurance groups.
		/// </summary>
		/// <returns>list of groups</returns>
		public List<String> GetProgramAssuranceGroups()
		{
			return GetProgramAssuranceGroups( LdapGroupQueryPath );
		}

		/// <summary>
		/// Gets the program assurance groups.
		/// </summary>
		/// <param name="ldapQueryPath">The LDAP query path.</param>
		/// <returns>list of groups</returns>
		public List<String> GetProgramAssuranceGroups( string ldapQueryPath )
		{
			var groups = new List<String>();

			var adminGroupName = GroupNameFor( CommonConstants.PaamGroupAdministrators );
			var adminGroup = string.Format( ldapQueryPath, adminGroupName );
			groups.Add( adminGroup );

			var noGroupName = GroupNameFor( CommonConstants.PaamGroupNo );
			var noGroup = string.Format( ldapQueryPath, noGroupName );
			groups.Add( noGroup );

			var nswGroupName = GroupNameFor( CommonConstants.PaamGroupNsw );
			var nswGroup = string.Format( ldapQueryPath, nswGroupName );
			groups.Add( nswGroup );

			var ntGroupName = GroupNameFor( CommonConstants.PaamGroupNt );
			var ntGroup = string.Format( ldapQueryPath, ntGroupName );
			groups.Add( ntGroup );

			var qldGroupName = GroupNameFor( CommonConstants.PaamGroupQld );
			var qldGroup = string.Format( ldapQueryPath, qldGroupName );
			groups.Add( qldGroup );

			var saGroupName = GroupNameFor( CommonConstants.PaamGroupSa );
			var saGroup = string.Format( ldapQueryPath, saGroupName );
			groups.Add( saGroup );

			var tasGroupName = GroupNameFor( CommonConstants.PaamGroupTas );
			var tasGroup = string.Format( ldapQueryPath, tasGroupName );
			groups.Add( tasGroup );

			var vicGroupName = GroupNameFor( CommonConstants.PaamGroupVic );
			var vicGroup = string.Format( ldapQueryPath, vicGroupName );
			groups.Add( vicGroup );

			var waGroupName = GroupNameFor( CommonConstants.PaamGroupWa );
			var waGroup = string.Format( ldapQueryPath, waGroupName );
			groups.Add( waGroup );

			return groups;
		}

		private static string GroupNameFor( string baseGroupName )
		{
			var suffix = string.Empty;
			var env = AppHelper.Environment().ToUpper();
			if ( env.Equals( "LOCAL" ) || env.Equals( "DEV" ) || env.Equals( "DEVFIX" ) )
				suffix = "_DEV";
			else if ( env.Equals( "TEST" ) || env.Equals( "TESTFIX" ) )
				suffix = "_TEST";
			else if ( env.Equals( "PRE" ) || env.Equals( "PREPROD" ) )
				suffix = "_PRE";
			var groupName = baseGroupName + suffix;
			return groupName;
		}

		/// <summary>
		/// Gets the program assurance user dropdown list.
		/// </summary>
		/// <param name="cacheService">The cache service.</param>
		/// <returns>
		/// a list of user selection items
		/// </returns>
		public IEnumerable<SelectListItem> GetProgramAssuranceDropdownList( ICacheService cacheService )
		{
			var users = GetPatUsers( cacheService );
			var results = from user in users
							  select new SelectListItem { Selected = false, Text = user.FullName, Value = user.LoginName };
			return results.AsEnumerable();
		}

		/// <summary>
		/// Gets the LDAP group query path.
		/// </summary>
		/// <value>
		/// The LDAP group query path.
		/// </value>
		public String LdapGroupQueryPath
		{
			get { return ConfigurationManager.AppSettings[ CommonConstants.LdapGroupQueryPath ]; }
		}

		/// <summary>
		///   Retrieve Users by list of Group paths
		/// </summary>
		/// <param name="groupPaths">The group paths.</param>
		/// <returns>list of users</returns>
		public List<PatUser> GetAllPAUsersByGroupPaths( List<string> groupPaths )
		{
			var users = new List<PatUser>();
			foreach ( var groupPath in groupPaths )
			{
				try
				{
					using ( var deGroup = new DirectoryEntry( groupPath ) )
					{
						var count = deGroup.Properties[ "member" ].Count;
						for ( var i = 0; i < count; i++ )
						{
							var pathnavigate = groupPath.Split( "CN".ToCharArray() );
							var respath = pathnavigate[ 0 ];
							var objpath = deGroup.Properties[ "member" ][ i ].ToString();
							var path = respath + objpath;

							using ( var deUser = new DirectoryEntry( path ) )
								users.Add( PatUser.GetUser( deUser ) );
						}
					}
				}
				catch (DirectoryServicesCOMException)
				{
					ErrorLog.GetDefault( null ).Log( new Error( new System.ApplicationException( string.Format( "AD Error accessing {0}", groupPath  ) ) ) );
				}
			}
			return users;
		}

		/// <summary>
		/// Gets the program assurance admin users.
		/// </summary>
		/// <param name="cacheService">The cache service.</param>
		/// <returns></returns>
		public List<PatUser> GetProgramAssuranceAdminUsers( ICacheService cacheService )
		{
			var users = GetPatUsers( cacheService );
			var admins = users.Where( patUser => patUser.IsAdministrator() ).ToList();
			return admins;
		}

		/// <summary>
		/// Gets the pat users from the Cache.
		/// </summary>
		/// <param name="cacheService">The cache service.</param>
		/// <returns></returns>
		public List<PatUser> GetPatUsers( ICacheService cacheService )
		{
			var cacheKey = new KeyModel( CacheType.Global, "UserList" );
			List<PatUser> users;
			if ( !cacheService.TryGet( cacheKey, out users ) )
			{
				//  do it the slow way
				users = GetProgramAssuranceUsers();
				cacheService.Set( cacheKey, users );
				if ( AuditService != null )
					AuditService.AuditActivity( string.Format( "Loaded {0} users into Cache", users.Count ), "CACHE" );
			}
			return users;
		}

		/// <summary>
		///   Turn a Windows Identity into a PAT user.
		/// </summary>
		/// <param name="user">windows identity.</param>
		/// <returns>a PAT user</returns>
		public PatUser PatUserFor( WindowsIdentity user )
		{
			return new PatUser( user );
		}
	}
}