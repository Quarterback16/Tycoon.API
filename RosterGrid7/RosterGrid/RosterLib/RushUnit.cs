using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace RosterLib
{
	/// <summary>
	///   A rush unit can have an Aceback and an R2, or a committee of 2
	///   this adjusts the workload in the projections
	/// </summary>
	public class RushUnit
	{
		public List<NFLPlayer> Runners { get; set; }

		public NFLPlayer GoalLineBack { get; set; }
		public NFLPlayer AceBack { get; set; }
		public NFLPlayer R1 { get; set; }
		public NFLPlayer R2 { get; set; }

		public int nR1 { get; set; }
		public int nR2 { get; set; }
		public string Committee { get; set; }
		public List<NFLPlayer> Starters { get; set; }
		public string TeamCode { get; set; }

		public bool IsAceBack { get; set; }

		public RushUnit()
		{
			Runners = new List<NFLPlayer>();
			Starters = new List<NFLPlayer>();
			nR2 = 0;
			IsAceBack = true;
		}

		public void Add( NFLPlayer player )
		{
			Runners.Add( player );
		}

		public void Load( string teamCode )
		{
			TeamCode = teamCode;
			var ds = Utility.TflWs.GetTeamPlayers( teamCode, Constants.K_RUNNINGBACK_CAT );
			var dt = ds.Tables[ "player" ];
			if ( dt.Rows.Count != 0 )
				foreach (DataRow dr in dt.Rows)
				{
					if ( ! dr["POSDESC"].ToString().Trim().Contains("FB") )
					   Add(new NFLPlayer(dr["PLAYERID"].ToString()));
				}
			SetGoalLineBack();
			SetAceBack();
			if ( AceBack != null )
				SetBackup();
#if DEBUG
			DumpUnit();
#endif
		}

		private void SetBackup()
		{
			//  can only have one backup, any others need to be given roles of R for reserve
			foreach ( var p in Runners )
			{
				if ( p.IsBackup() && !p.IsFullback() )
				{
					R2 = p;
					nR2++;
#if DEBUG
					//Utility.Announce( string.Format( "Setting Backup to {0}", p.PlayerName ) );
#endif
				}
			}
		}

		private void SetGoalLineBack()
		{
			foreach ( var p in Runners )
			{
				if ( p.IsShortYardageBack() )
				{
					GoalLineBack = p;
					break;
				}
			}
		}

		private void SetAceBack()
		{
			var nStarters = 0;
			Committee = string.Empty;

			foreach ( var p in Runners )
			{
				if ( p.IsStarter() && ! p.IsFullback() )
				{
					nStarters++;
					nR1++;
					AceBack = p;
					if ( R1 == null )
						R1 = p;

					Committee += p.PlayerNameShort + " + ";
					Starters.Add(p);
#if DEBUG
					//Utility.Announce( string.Format( "Setting Ace to {0}", p.PlayerName ) );
#endif
				}
			}
			if ( nStarters != 1 )
			{
				AceBack = null;
				IsAceBack = false;
			}
		}

		public bool HasIntegrityError()
		{
			if ( ( AceBack == null ) && ( R1 == null ) && ( R2 == null ) )
				return true;
			else
			{
				if ( nR2 > 1 )   //  zero is okay
				{
#if DEBUG
					Utility.Announce( string.Format( "{1} is Too many R2 for {0}", TeamCode, nR2 ) );
#endif
					return true;
				}
				return false;
			}
		}

		public void DumpUnit()
		{
			var starters = ( AceBack == null || string.IsNullOrEmpty( AceBack.ToString() ) ) ? Committee : AceBack.ToString() ;
			DumpPlayer( "R1", starters, nR1 );
			DumpPlayer( "R2", R2 == null ? string.Empty : R2.PlayerNameShort, nR2 );

			Utility.Announce( string.Format( "Ace back : {0}", AceBack ) );
			Utility.Announce( string.Format( "R2       : {0}", R2 ) );
			Utility.Announce( string.Format( "Goaline  : {0}", GoalLineBack ) );
			foreach ( var runner in Runners )
			   Utility.Announce( string.Format( "{0,-25} : {1} : {2}", runner.PlayerName.PadRight(25), runner.PlayerRole, runner.PlayerPos ) );				
		}

		private void DumpPlayer( string pos, string player, int count )
		{
			var plyrName = ( player == null ) ? "none" : player;
			Utility.Announce( string.Format( "{2} ({1}): {0}", plyrName, count, pos ) );
		}


		public void LoadCarries( string season, string week )
		{
			var totRushes = 0;
			foreach (var runner in Runners)
			{
				//  remove SH designation
				runner.PlayerPos = runner.PlayerPos.Replace(",SH", "");
				string carries = Utility.TflWs.PlayerStats( 
					Constants.K_STATCODE_RUSHING_CARRIES, season, week, runner.PlayerCode);
				runner.TotStats = new PlayerStats();
				int rushes = 0;
				if ( ! int.TryParse(carries, out rushes ) )
					rushes = 0;

				runner.TotStats.Rushes = rushes;
				totRushes += rushes;
			}
			if (totRushes > 0) //  not bye wk
			{
				var compareByCarries = new Comparison<NFLPlayer>(CompareTeamsByCarries);
				Runners.Sort(compareByCarries);
				//  look for SH back
				var sh = GetShortYardageBack(season, week, TeamCode);
				foreach (var runner in Runners)
				{
					if (runner.PlayerCode.Equals(sh))
						runner.PlayerPos += ",SH";
				}
				DumpUnitByCarries(totRushes);

				foreach (var runner in Runners)
					Utility.TflWs.StorePlayerRoleAndPos(runner.PlayerRole, runner.PlayerPos, runner.PlayerCode);
			}
			else
				Utility.Announce(string.Format("{0}:{1} is a bye week for {2}", season, week, TeamCode ));
		}

		public string GetShortYardageBack(string season, string week, string teamCode)
		{
			var sh = "???";
			var ds = Utility.TflWs.PenaltyScores(season, week, teamCode );
			if (ds != null)
			{
				var dt = ds.Tables[0];
				foreach (DataRow dr in dt.Rows )
				{
					sh = dr["PLAYERID1"].ToString();
				}
			}
			return sh;
		}


		private static int CompareTeamsByCarries(NFLPlayer x, NFLPlayer y)
		{
			if (x == null)
			{
				if (y == null)
					return 0;
				return -1;
			}
			return y == null ? 1 : y.TotStats.Rushes.CompareTo(x.TotStats.Rushes);
		}

		public void DumpUnitByCarries( int totRushes )
		{
			foreach (var runner in Runners)
			{
				var load = Utility.Percent(runner.TotStats.Rushes, totRushes);

				//  Returned fron the dead
				if ( load > 0 && ( runner.PlayerRole == Constants.K_ROLE_INJURED  || runner.PlayerRole == Constants.K_ROLE_SUSPENDED ) )
					runner.PlayerRole = Constants.K_ROLE_DEEP_RESERVE;

				if (runner.PlayerRole != Constants.K_ROLE_INJURED && runner.PlayerRole != Constants.K_ROLE_SUSPENDED)
				{
					runner.PlayerRole = Constants.K_ROLE_DEEP_RESERVE;
					if (load > 0.0M)
						runner.PlayerRole = Constants.K_ROLE_RESERVE;
					if (load > 10.0M)
						runner.PlayerRole = Constants.K_ROLE_BACKUP;
					if (load > 30.0M)
						runner.PlayerRole = Constants.K_ROLE_STARTER;
				}

				Utility.Announce(string.Format("{0,-25} : {1} : {2,3} : {3,5:##0.0}% : {4} : {5}",
					runner.PlayerName.PadRight(25), runner.PlayerRole, runner.TotStats.Rushes, 
					load, runner.PlayerRole, runner.PlayerPos
					));
			}
		}
	}
}
