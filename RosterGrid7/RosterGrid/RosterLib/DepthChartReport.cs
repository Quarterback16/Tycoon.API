using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RosterLib
{
	public class DepthChartReport 
	{
		public string TeamCode { get; set; }
		public string Season { get; set; }
		public string FileOut { get; set; }
		public NflTeam NflTeam { get; set; }
		public int PlayerCount { get; set; }
		public List<String> Errors { get; set; }

		public DepthChartReport()
		{
			Season = "2013";
			TeamCode = "SF";
			Errors = new List<String>();
		}

		public DepthChartReport( string season, string teamCode )
		{
			Season = season;
			TeamCode = teamCode;
			Errors = new List<String>();
		}

		public void Execute()
		{
			var PreReport = new SimplePreReport();
			PreReport.ReportType = "Depth Chart";
			PreReport.Folder = "DepthCharts";
			PreReport.Season = Season;
			PreReport.InstanceName = TeamCode;
			PreReport.Body = GenerateBody();
			PreReport.RenderHtml();
			FileOut = PreReport.FileOut;
			DumpErrors();
		}

		private string GenerateBody()
		{
			var bodyOut = new StringBuilder();
			NflTeam = new NflTeam( TeamCode );
			NflTeam.LoadTeam();
			PlayerCount = NflTeam.PlayerList.Count;
#if DEBUG
			Utility.Announce( string.Format( "   {0} Roster Count : {1}", TeamCode, PlayerCount ) ); 
#endif
			bodyOut.AppendLine( NflTeam.RatingsOut() );
			bodyOut.AppendLine();
			AppendPlayerLine( bodyOut, "QB" );
			AppendPlayerLine( bodyOut, "Q2" );
			AppendPlayerLine( bodyOut, "RB" );
			AppendPlayerLine( bodyOut, "R2" );
			AppendPlayerLine( bodyOut, "RR" );
			AppendPlayerLine( bodyOut, "3D" );
			AppendPlayerLine( bodyOut, "SH" );
			AppendPlayerLine( bodyOut, "FB" );
			AppendPlayerLine( bodyOut, "W1" );
			AppendPlayerLine( bodyOut, "W2" );
			AppendPlayerLine( bodyOut, "W3" );
			AppendPlayerLine( bodyOut, "TE" );
			AppendPlayerLine( bodyOut, "T2" );
			AppendPlayerLine( bodyOut, "PR" );
			AppendPlayerLine( bodyOut, "KR" );
			AppendPlayerLine( bodyOut, "PK" );
			AppendPlayerLine( bodyOut, "INJ" );
			AppendPlayerLine( bodyOut, "SUS" );
			AppendPlayerLine( bodyOut, "HO" );
			return bodyOut.ToString();
		}

		private void AppendPlayerLine( StringBuilder bodyOut, string posDesc )
		{
			bodyOut.AppendLine( PlayerLine( posDesc ) );
			bodyOut.AppendLine();
		}

		private string PlayerLine( string posDesc )
		{
			List<NFLPlayer> playerList = GetPlayers( posDesc );
			if ( playerList.Count == 1 )
			{
				var player = playerList[ 0 ];
				if ( posDesc == "INJ" )
					return string.Format( "{2}: {0}-{1} {4} ({3})", player.JerseyNo, player.PlayerName, posDesc, player.Owner, player.PlayerPos );
				else
					return string.Format( "{2}: {0}-{1} ({3})", player.JerseyNo, player.PlayerName, posDesc, player.Owner );
			}
			else if ( playerList.Count == 0 )
				return string.Format( "{0}: ", posDesc );
			else
			{
				var playerKeys = GetPlayerKeys( playerList, posDesc );
				return string.Format( "{1}: {0}", playerKeys, posDesc );
			}
		}

		private object GetPlayerKeys( List<NFLPlayer> playerList, string posDesc )
		{
			var playerKeys = new StringBuilder();
			foreach ( var player in playerList )
			{
				if ( posDesc == "INJ" )
					playerKeys.Append( string.Format( "{0}-{1} {3} ({2}), ", player.JerseyNo, player.PlayerName, player.Owner, player.PlayerPos ) );
				else
				{
					var injStatus = string.Empty;
					if ( player.PlayerRole == Constants.K_ROLE_INJURED )
						injStatus = "-INJ";
					playerKeys.Append( string.Format( "{0}-{1}{3} ({2}), ", player.JerseyNo, player.PlayerName, player.Owner, injStatus ) );
				}
			};
			var playersOut = playerKeys.ToString();
			if ( playersOut.Length > 0 )
				playersOut = playersOut.Substring( 0, playersOut.Length - 2 ); 
			return playersOut;
		}

		private List<NFLPlayer> GetPlayers( string posDesc )
		{
			var desiredRole = "S";
			if ( posDesc == "R2" || posDesc == "Q2" || posDesc == "T2" ) 
				desiredRole = "B";
			if ( posDesc == "RR"  )
				desiredRole = "R";
			if ( posDesc == "INJ" ) desiredRole = "I";
			if ( posDesc == "SUS" ) desiredRole = "X";
			if ( posDesc == "HO" ) desiredRole = "H";
			List<NFLPlayer> playerList = new List<NFLPlayer>();
			foreach ( var p in NflTeam.PlayerList )
			{
				var player = (NFLPlayer) p;
				if ( HasPos( posDesc, player, desiredRole ) && RoleMatches( posDesc, desiredRole, player ) )
					playerList.Add( player );
			}
			ApplyRules( posDesc, playerList );
			return playerList;
		}

		/// <summary>
		///   Will apply rules oon Roles
		/// </summary>
		/// <param name="posDesc"></param>
		/// <param name="playerList"></param>
		private void ApplyRules( string posDesc, List<NFLPlayer> playerList )
		{
			//  missing spots rules:-
			//  must have Q1, R1, W1, W2, TE, PK
			if ( posDesc == "Q1" || posDesc == "R1" || posDesc == "W1" || posDesc == "W2" || posDesc == "TE" || posDesc == "PK" )
			{
				if ( playerList.Count == 0 )
				{
					var errMsg = string.Format( "No one found for {0} - {1}", posDesc, TeamCode );
					Utility.Announce( errMsg );
					Errors.Add( errMsg );
				}
			}
			//  only 1 rules
			if ( posDesc == "Q1" || posDesc == "W1" || posDesc == "W2" || posDesc == "PK" )
			{
				if ( ActiveCount( playerList ) > 1 )
				{
					var errMsg = string.Format( "Too many found for {0} - {1} - {2}",
						posDesc, GetPlayerKeys( playerList, posDesc ), TeamCode );
					Utility.Announce( errMsg );
					Errors.Add( errMsg );
				}
			}

		}

		private int ActiveCount( List<NFLPlayer> playerList )
		{
			var actives = 0;
			foreach ( var p in playerList )
			{
				if ( ! p.IsInjured() && ! p.IsSuspended() ) actives++;
			}
			return actives;
		}

		private static bool RoleMatches( string posDesc, string desiredRole, NFLPlayer player )
		{
			if ( posDesc == "SH" ) return true;
			if ( posDesc == "3D" ) return true;
			if ( posDesc == "W1" ) return true;
			if ( posDesc == "W2" ) return true;
			if ( posDesc == "W3" ) return true;
			if ( posDesc == "PR" ) return true;
			if ( posDesc == "KR" ) return true;
			if ( desiredRole == "I" ) return true;
			if ( desiredRole == "X" ) return true;
			if ( desiredRole == "H" ) return true;
			return player.PlayerRole.Equals( desiredRole );
		}

		private static bool HasPos( string posDesc, NFLPlayer player, string desiredRole )
		{
			if ( desiredRole == "I" || desiredRole == "X" || desiredRole == "H" )
				return ( player.PlayerRole == desiredRole );

			if ( posDesc == "RB" || posDesc == "R2" )
			{
				return player.Contains( "RB", player.PlayerPos ) || player.Contains( "HB", player.PlayerPos );
			}
			else if ( posDesc == "SH" )
			{
				return player.Contains( "SH", player.PlayerPos );
			}
			else if ( posDesc == "RR" )
			{
				return player.Contains( "RB", player.PlayerPos ) || player.Contains( "HB", player.PlayerPos );
			}
			else if ( posDesc == "3D" )
			{
				return player.Contains( "3D", player.PlayerPos );
			}
			else if ( posDesc == "PR" )
			{
				return player.Contains( "PR", player.PlayerPos );
			}
			else if ( posDesc == "KR" )
			{
				return player.Contains( "KR", player.PlayerPos );
			}
			else if ( posDesc == "Q2" )
			{
				return player.Contains( "QB", player.PlayerPos );
			}
			else if ( posDesc == "T2" )
			{
				return player.Contains( "TE", player.PlayerPos );
			}
			else if ( posDesc == "WR" )
			{
				return player.Contains( "WR", player.PlayerPos ) || player.Contains( "SE", player.PlayerPos ) || player.Contains( "FL", player.PlayerPos );
			}
			else if ( posDesc == "PK" )
			{
				return player.Contains( "PK", player.PlayerPos ) || ( player.Contains( "K", player.PlayerPos ) && player.PlayerCat == "4" );
			}
			return player.Contains( posDesc, player.PlayerPos );
		}

		public bool HasIntegrityError()
		{
			return Errors.Count() > 0;
		}

		public void DumpErrors()
		{
			foreach ( var err in Errors )
			{
				Utility.Announce( err );				
			}
		}
	}
}
