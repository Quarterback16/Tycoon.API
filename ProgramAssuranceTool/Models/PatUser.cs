using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.DirectoryServices;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Interfaces;

namespace ProgramAssuranceTool.Models
{

	public class PatUser
	{
		public String FirstName { get; set; }
		public String LastName { get; set; }

		[DisplayName( "User ID" )]
		public String LoginName { get; set; }

		[DisplayName( "Email Address" )]
		public String EmailAddress { get; set; }

		[DisplayName( "Full Name" )]
		public String FullName { get; set; }

		public IEnumerable<String> MemberOf { get; set; }

		public PatUser()
		{
		}

		public override string ToString()
		{
			return FullName;
		}

		/// <summary>
		///   Construct a PAT user given just the user ID eg SC0779
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="patService">dependancy for debugging</param>
		public PatUser( string userId, IPatService patService )
		{
#if DEBUG
			var stopwatch = new Stopwatch();
			stopwatch.Start();
#endif
			//  formulate the query
			var query = string.Format(
				ConfigurationManager.AppSettings[ CommonConstants.LdapUserQueryPath ], userId );
			//  get the directory entry
			var directoryUser = new DirectoryEntry( query );
			SetUserUp( directoryUser );
#if DEBUG
			var elapsed = AppHelper.StopTheWatch( stopwatch );
			patService.SaveActivity( string.Format( "User Id {1} construction took {0}", elapsed, userId ), "PATUSER" );

#endif
		}

		/// <summary>
		///   Construct a PAT user given just the user ID eg SC0779
		/// </summary>
		/// <param name="userId"></param>
		public PatUser( string userId )
		{
			//  formulate the query
			var query = string.Format(
				ConfigurationManager.AppSettings[ CommonConstants.LdapUserQueryPath ], userId );
			//  get the directory entry
			var directoryUser = new DirectoryEntry( query );
			SetUserUp( directoryUser );
		}

		public PatUser( IIdentity user )
		{
			try
			{
				LoginName = user.Name.RemoveDomain();
				var ldapPath = ConfigurationManager.AppSettings[ CommonConstants.LdapUserQueryPath ];
				var query = string.Format( ldapPath, LoginName );

				var directoryUser = new DirectoryEntry( query );
				SetUserUp( directoryUser );
			}
			catch (Exception ex)
			{
				Elmah.ErrorLog.GetDefault( null ).Log( new Elmah.Error( ex ) );
				throw;
			}
		}

		public PatUser( IIdentity user, IPatService patService )
		{
			try
			{
#if DEBUG
				var stopwatch = new Stopwatch();
				stopwatch.Start();
#endif
				LoginName = user.Name.RemoveDomain();
				var ldapPath = ConfigurationManager.AppSettings[ CommonConstants.LdapUserQueryPath ];
				var query = string.Format( ldapPath, LoginName );

				var directoryUser = new DirectoryEntry( query );
				SetUserUp( directoryUser );
#if DEBUG
				var elapsed = AppHelper.StopTheWatch( stopwatch );
				patService.SaveActivity( string.Format( "User Id {1} construction took {0}", elapsed, user.Name ), "PATUSER" );

#endif
			}
			catch ( Exception ex )
			{
				Elmah.ErrorLog.GetDefault( null ).Log( new Elmah.Error( ex ) );
				throw;
			}

		}

		private PatUser(DirectoryEntry directoryUser)
		{
			SetUserUp( directoryUser );
		}

		private void SetUserUp( DirectoryEntry directoryUser )
		{
			FirstName = GetProperty( directoryUser, ADProperties.FIRSTNAME );
			LastName = GetProperty( directoryUser, ADProperties.LASTNAME );
			FullName = string.Format( "{0} {1}", FirstName.Trim(), LastName.Trim() );
			LoginName = ExtractUserName( GetProperty( directoryUser, ADProperties.LOGINNAME ) );
			EmailAddress = GetProperty( directoryUser, ADProperties.EMAILADDRESS );
			MemberOf = GetProperties( directoryUser, ADProperties.MEMBEROF );
			//  get rid of any NON Program assurance groups
			var paMemberOf = MemberOf.Where( IsAProgramAssuranceGroup ).ToList();
			MemberOf = paMemberOf;
		}

		public void DumpUser()
		{
			Console.WriteLine( "Environment  : {0}", AppHelper.Environment() );
			Console.WriteLine( "FirstName    : {0}", FirstName );
			Console.WriteLine( "LastName     : {0}", LastName );
			Console.WriteLine( "FullName     : {0}", FullName );
			Console.WriteLine( "EmailAddress : {0}", EmailAddress );
			Console.WriteLine( "Administrator: {0}", IsAdmin() ? "Yes" : "No" );
			foreach ( var grp in MemberOf )
			{
				Console.WriteLine( "   Member Of : {0}", grp );				
			}
		}

		private static bool IsAProgramAssuranceGroup( string grp )
		{
			grp = grp.ToUpper();
			if ( grp.StartsWith( CommonConstants.PaamGroupAdministrators ) )
				return true;
			if ( grp.StartsWith( CommonConstants.PaamGroupGeneral ) )
				return true;
			if ( grp.StartsWith( CommonConstants.PaamGroupNo ) )
				return true;
			if ( grp.StartsWith( CommonConstants.PaamGroupAct ) )
				return true;
			if ( grp.StartsWith( CommonConstants.PaamGroupNsw ) )
				return true;
			if ( grp.StartsWith( CommonConstants.PaamGroupNt ) )
				return true;
			if ( grp.StartsWith( CommonConstants.PaamGroupQld ) )
				return true;
			if ( grp.StartsWith( CommonConstants.PaamGroupSa ) )
				return true;
			if ( grp.StartsWith( CommonConstants.PaamGroupTas ) )
				return true;
			if ( grp.StartsWith( CommonConstants.PaamGroupVic ) )
				return true;
			if ( grp.StartsWith( CommonConstants.PaamGroupWa ) )
				return true;
			return false;
		}

		private static string ExtractUserName( string path )
		{
			var userPath = path.Split( new[] { '\\' } );
			return userPath[ userPath.Length - 1 ];
		}

		private String GetProperty( DirectoryEntry userDetail, String propertyName )
		{
			string property;
			try
			{
				property =  userDetail != null && userDetail.Properties.Contains( propertyName ) ? userDetail.Properties[ propertyName ][ 0 ].ToString() : string.Empty;
			}
			catch (DirectoryServicesCOMException ex)
			{
				property = string.Format( "{2}:{0} - not found in Active Directory ({1})", propertyName, ex.Message, LoginName );
			}
			catch ( COMException ex )
			{
				property = string.Format( "{2}:{0} - not found in Active Directory ({1})", propertyName, ex.Message, LoginName );
			}
			return property;
		}

		private static IEnumerable<string> GetProperties( DirectoryEntry userDetail, String propertyName )
		{
			try
			{
				return ( from string Group in userDetail.Properties[ propertyName ]
							select Group.Split( ',' ) into splits
							select splits[ 0 ].Split( '=' ) into final
							select final[ 1 ] ).ToList();
			}
			catch ( DirectoryServicesCOMException )
			{
				return new List<string>();
			}
			catch ( COMException )
			{
				return new List<string>();  // bad data
			}
		}

		public static PatUser GetUser( DirectoryEntry directoryUser )
		{
			return new PatUser( directoryUser );
		}

		public bool IsAdministrator()
		{
			return IsAdmin();
		}

		public string ResourceSet()
		{
			var resources = string.Empty;
			if ( IsNo() ) resources += CommonConstants.ResourceCode_NO + ",";
			if ( IsNsw() ) resources += CommonConstants.ResourceCode_NSWACT + ",";
			if ( IsQld() ) resources += CommonConstants.ResourceCode_QLD + ",";
			if ( IsVic() ) resources += CommonConstants.ResourceCode_VIC + ",";
			if ( IsSa() ) resources += CommonConstants.ResourceCode_SA + ",";
			if ( IsWa() ) resources += CommonConstants.ResourceCode_WA + ",";
			if ( IsTas() ) resources += CommonConstants.ResourceCode_TAS + ",";
			if ( IsNt() ) resources += CommonConstants.ResourceCode_NT + ",";
			return resources;
		}

		private bool IsInGroup( string groupName )
		{
			if (AppHelper.IsTestEnvironment())
				groupName += "_TEST";
			else if (AppHelper.IsDevEnvironment())
				groupName += "_DEV";
			else if ( AppHelper.IsPreProdEnvironment() )
				groupName += "_PRE";
			var groupNameLong = string.Format( "{1}{0}", groupName, CommonConstants.Domain );
			var isInGroup = MemberOf.Any( grp => grp.ToUpper().Equals( groupName ) || grp.ToUpper().Equals( ( groupNameLong ) ) );
			return isInGroup;
		}

		public bool IsInAnyOfTheseGroups( string resources )
		{
			var resource = resources.Split( ',' );
			return resource.Select( GetGroupNameFromResource )
				.Where( groupName => !string.IsNullOrEmpty( groupName ) ).Any( IsInGroup );
		}

		private static string GetGroupNameFromResource( string resource )
		{
			var groupName = string.Empty;
			switch ( resource )
			{
				//  The C# switch statement requires that every case is a compile-time constant.
				case "NO":
					groupName = CommonConstants.PaamGroupNo;
					break;

				case "NSWACT":
					groupName = CommonConstants.PaamGroupNsw;
					break;

				case "QLD":
					groupName = CommonConstants.PaamGroupQld;
					break;

				case "VIC":
					groupName = CommonConstants.PaamGroupVic;
					break;

				case "SA":
					groupName = CommonConstants.PaamGroupSa;
					break;

				case "WA":
					groupName = CommonConstants.PaamGroupWa;
					break;

				case "TAS":
					groupName = CommonConstants.PaamGroupTas;
					break;

				case "NT":
					groupName = CommonConstants.PaamGroupNt;
					break;

			}
			return groupName;
		}

		public bool IsGeneral()
		{
			return IsInGroup( CommonConstants.PaamGroupGeneral );
		}

		public bool IsAdmin()
		{
			return IsInGroup( CommonConstants.PaamGroupAdministrators );
		}

		public bool IsNo()
		{
			return IsInGroup( CommonConstants.PaamGroupNo );
		}

		public bool IsNsw()
		{
			return IsInGroup( CommonConstants.PaamGroupNsw );
		}

		public bool IsAct()
		{
			return IsInGroup( CommonConstants.PaamGroupAct );
		}

		public bool IsNt()
		{
			return IsInGroup( CommonConstants.PaamGroupNt );
		}

		public bool IsQld()
		{
			return IsInGroup( CommonConstants.PaamGroupQld );
		}

		public bool IsSa()
		{
			return IsInGroup( CommonConstants.PaamGroupSa );
		}

		public bool IsTas()
		{
			return IsInGroup( CommonConstants.PaamGroupTas );
		}

		public bool IsVic()
		{
			return IsInGroup( CommonConstants.PaamGroupVic );
		}

		public bool IsWa()
		{
			return IsInGroup( CommonConstants.PaamGroupWa );
		}

		public bool InState( string stateCode )
		{
			if ( stateCode.Equals( DataConstants.StateNewSouthWales ) )
				return IsNsw();

			if ( stateCode.Equals( DataConstants.StateVictoria ) )
				return IsVic();

			if ( stateCode.Equals( DataConstants.StateQueensland ) )
				return IsQld();

			if ( stateCode.Equals( DataConstants.StateAustralianCapitalTerritory ) )
				return IsAct();

			if ( stateCode.Equals( DataConstants.StateWesternAustralia ) )
				return IsWa();

			if ( stateCode.Equals( DataConstants.StateSouthAustralia ) )
				return IsSa();

			if ( stateCode.Equals( DataConstants.StateTasmania ) )
				return IsTas();

			return stateCode.Equals( DataConstants.StateNorthernTerritory ) && IsNt();
		}

		public string UserFormated()
		{
			return string.Format( "{0}:{1}{2}",
			                      LoginName, FullName, IsAdminString( IsAdministrator() ) );
		}

		public string IsAdminString( bool isAdministrator )
		{
			return isAdministrator ? "  ADMINISTRATOR" : string.Empty;
		}
	}

	public class PatUserComparer : IEqualityComparer<PatUser>
	{
		public bool Equals( PatUser x, PatUser y )
		{
			if ( ReferenceEquals( x, y ) ) return true;

			if ( ReferenceEquals( x, null ) || ReferenceEquals( y, null ) )
				return false;

			return x.LoginName == y.LoginName;
		}

		public int GetHashCode( PatUser x )
		{
			return ReferenceEquals( x, null ) ? 0 : ( x.LoginName ?? "" ).GetHashCode();
		}
	}


}