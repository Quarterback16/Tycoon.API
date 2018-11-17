using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace RosterLib
{
	public class PassUnit
	{
		public string TeamCode { get; set; }

		public List<NFLPlayer> Quarterbacks { get; set; }
		public List<NFLPlayer> Receivers { get; set; }

		public NFLPlayer Q1 { get; set; }
		public NFLPlayer Q2 { get; set; }

		public int nQ1 { get; set; }
		public int nQ2 { get; set; }

		public NFLPlayer W1 { get; set; }
		public NFLPlayer W2 { get; set; }
		public NFLPlayer W3 { get; set; }

		public int nW1 { get; set; }
		public int nW2 { get; set; }
		public int nW3 { get; set; }

		public NFLPlayer TE { get; set; }
		public int nTE { get; set; }

		public PassUnit()
		{
			Quarterbacks = new List<NFLPlayer>();
			Receivers = new List<NFLPlayer>();
			nQ1 = 0;
			nQ2 = 0;
			nW1 = 0;
			nW2 = 0;
			nW3 = 0;
			nTE = 0;
		}

		public void DumpUnit()
		{
			DumpPlayer( "Q1", Q1, nQ1 );
			DumpPlayer( "Q2", Q2, nQ2 );
			DumpPlayer( "W1", W1, nW1 );
			DumpPlayer( "W2", W2, nW2 );
			DumpPlayer( "W3", W3, nW3 );
			DumpPlayer( "TE", TE, nTE );
			foreach ( var receiver in Receivers )
			{
				Utility.Announce( string.Format( "{0,-25} : {1} : {2}", 
					receiver.PlayerName.PadRight(25), receiver.PlayerRole, receiver.PlayerPos ) );
			}
		}

		private void DumpPlayer( string pos, NFLPlayer player, int count )
		{
			var plyrName = ( player == null ) ? "none" : player.PlayerName;
			Utility.Announce( string.Format( "{2} ({1}): {0}", plyrName, count, pos ) );
		}

		public void Load( string teamCode )
		{
			TeamCode = teamCode;
			LoadPlayers( teamCode, Constants.K_QUARTERBACK_CAT );
			LoadPlayers( teamCode, Constants.K_RECEIVER_CAT );
			SetQbRoles();
			SetReceiverRoles();
#if DEBUG
			DumpUnit();
#endif
		}

		private void SetQbRoles()
		{
			//  can only have one backup, any others need to be given roles of R for reserve
			foreach ( var p in Quarterbacks )
			{
#if DEBUG
				//Utility.Announce(string.Format("   Examining QB {0}", p.PlayerName));
#endif
				if ( p.IsQuarterback() )
				{
					if ( p.IsStarter() )
					{
						Q1 = p;
						nQ1++;
#if DEBUG
						//Utility.Announce( string.Format( "      Setting Starter to {0}", p.PlayerName ) );
#endif
					}
					if ( p.IsBackup() )
					{
						Q2 = p;
						nQ2++;
#if DEBUG
						//Utility.Announce( string.Format( "       Setting Backup to {0}", p.PlayerName ) );
#endif
					}
				}
			}
		}

		private void SetReceiverRoles()
		{
			//  can only have one backup, any others need to be given roles of R for reserve
			foreach ( var p in Receivers )
			{
#if DEBUG
				//Utility.Announce(string.Format("   Examining Rec {0}", p.PlayerName));
#endif
				if ( p.HasPos( "W1" ) )
				{
					if ( p.IsStarter() )
					{
						W1 = p;
						nW1++;
#if DEBUG
						//Utility.Announce( string.Format( "      Setting W1 to {0}", p.PlayerName ) );
#endif
					}
				}

				if ( p.HasPos( "W2" ) )
				{
					if ( p.IsStarter() )
					{
						W2 = p;
						nW2++;
#if DEBUG
						//Utility.Announce( string.Format( "      Setting W2 to {0}", p.PlayerName ) );
#endif
					}
				}

				if ( p.HasPos( "W3" ) )
				{
					if ( p.IsStarter() )
					{
						W3 = p;
						nW3++;
#if DEBUG
						//Utility.Announce( string.Format( "      Setting W3 to {0}", p.PlayerName ) );
#endif
					}
				}

				if ( p.HasPos( "TE" ) )
				{
					if ( p.IsStarter() )
					{
						TE = p;
						nTE++;
#if DEBUG
						//Utility.Announce( string.Format( "      Setting TE to {0}", p.PlayerName ) );
#endif
					}
				}
			}
		}

		private void LoadPlayers( string teamCode, string playerCat )
		{
			var ds = Utility.TflWs.GetTeamPlayers( teamCode, playerCat );
			var dt = ds.Tables[ "player" ];
			if ( dt.Rows.Count != 0 )
				foreach ( DataRow dr in dt.Rows )
					Add( new NFLPlayer( dr[ "PLAYERID" ].ToString() ), playerCat );
		}

		public void Add( NFLPlayer player, string playerCat )
		{
			if ( playerCat == Constants.K_QUARTERBACK_CAT )
				Quarterbacks.Add( player );
			else
				Receivers.Add( player );
		}

		public bool HasIntegrityError()
		{
			var errors = 0;
			if ( Q1 == null )
			{
				errors = AddError( errors, "Q1" );
			}
			else if ( W1 == null )
			{
				errors = AddError( errors, "W1" );
			}
			else if ( W2 == null )
			{
				errors = AddError( errors, "W2" );
			}
			else if ( TE == null )
			{
				errors = AddError( errors, "TE" );
			}

			errors += CheckCount( nQ1, "Q1", 1 );  // must v a QB
			errors += CheckCount( nW1, "W1", 1 );  // must v a W1
			errors += CheckCount( nW2, "W2", 1 );  //  must v a W2
			errors += CheckCount( nTE, "TE", 2 );  //  some teams start 2 TEs

#if DEBUG
			Utility.Announce(string.Format("{0} errors", errors ));
#endif
			return ( errors > 0 );
		}

		private int AddError( int errors, string pos )
		{
			Utility.Announce( string.Format( "No {1} for {0}", TeamCode, pos ) );
			errors++;
			return errors;
		}

      private int CheckCount( int theCount, string thePos, int max )
      {
			if ( theCount > max )
			{
				Utility.Announce( string.Format( "{1} is Too many {2} for {0}", TeamCode, theCount, thePos ) );
				return 1;
			}
			return 0;
      }

		public void AnalyseWideouts(string season, string week )
		{
			var totYdc = 0;
			foreach (var p in Receivers)
			{
				if (p.IsWideout())
				{
					//  remove designation2
					p.PlayerPos = p.PlayerPos.Replace(",W1", "");
					p.PlayerPos = p.PlayerPos.Replace(",W2", "");
					p.PlayerPos = p.PlayerPos.Replace(",W3", "");
					//  how many yards
					string recYardage = Utility.TflWs.PlayerStats(
						Constants.K_STATCODE_RECEPTION_YARDS, season, week, p.PlayerCode);
					p.TotStats = new PlayerStats();
					int recYds = 0;
					if (!int.TryParse(recYardage, out recYds))
						recYds = 0;

					p.TotStats.YDc = recYds;
					totYdc += recYds;
				}
			}
			if (totYdc > 0) //  not bye wk
			{
				var compareByYdc = new Comparison<NFLPlayer>(ComparePlayersByYdc);

				Receivers.Sort(compareByYdc);

				DumpUnitByYardage(totYdc);

				//foreach (var runner in Runners)
				//	Utility.TflWs.StorePlayerRoleAndPos(runner.PlayerRole, runner.PlayerPos, runner.PlayerCode);
			}
			else
				Utility.Announce(string.Format("{0}:{1} is a bye week for {2}", season, week, TeamCode));
		}

		public void AnalyseQuarterbacks(string season, string week)
		{
			var totYdp = 0;
			foreach (var p in Quarterbacks)
			{
				if (p.IsQuarterback())
				{
					//  how many yards
					string passYardage = Utility.TflWs.PlayerStats(
						Constants.K_STATCODE_PASSING_YARDS, season, week, p.PlayerCode);
					p.TotStats = new PlayerStats();
					int passYds = 0;
					if (!int.TryParse(passYardage, out passYds))
						passYds = 0;

					p.TotStats.YDp = passYds;
					totYdp += passYds;
				}

			}
			if (totYdp > 0) //  not bye wk
			{
				var compareByYdp = new Comparison<NFLPlayer>(ComparePlayersByYdp);

				Quarterbacks.Sort(compareByYdp);

				DumpUnitByPassingYardage(totYdp);
			}
			else
				Utility.Announce(string.Format("{0}:{1} is a bye week for {2}", season, week, TeamCode));
		}

		public void AnalyseTightends(string season, string week)
		{
			var totYdc = 0;
			foreach (var p in Receivers)
			{
				if (p.IsTightEnd())
				{
					//  how many yards
					string recYardage = Utility.TflWs.PlayerStats(
						Constants.K_STATCODE_RECEPTION_YARDS, season, week, p.PlayerCode);
					p.TotStats = new PlayerStats();
					int recYds = 0;
					if (!int.TryParse(recYardage, out recYds))
						recYds = 0;

					p.TotStats.YDc = recYds;
					totYdc += recYds;
				}
			}
			if (totYdc > 0) //  not bye wk
			{
				var compareByYdc = new Comparison<NFLPlayer>(ComparePlayersByYdc);

				Receivers.Sort(compareByYdc);

				DumpTightendsByYardage(totYdc);

				//foreach (var runner in Runners)
				//	Utility.TflWs.StorePlayerRoleAndPos(runner.PlayerRole, runner.PlayerPos, runner.PlayerCode);
			}
			else
				Utility.Announce(string.Format("{0}:{1} is a bye week for {2}", season, week, TeamCode));
		}

		public void DumpTightendsByYardage(int tot)
		{
			foreach (var p in Receivers)
			{
				if (p.IsTightEnd())
				{
					var load = Utility.Percent(p.TotStats.YDc, tot);

					//  Returned fron the dead
					if (load > 0 && (p.PlayerRole == Constants.K_ROLE_INJURED || p.PlayerRole == Constants.K_ROLE_SUSPENDED))
						p.PlayerRole = Constants.K_ROLE_DEEP_RESERVE;

					if (p.PlayerRole != Constants.K_ROLE_INJURED && p.PlayerRole != Constants.K_ROLE_SUSPENDED)
					{
						p.PlayerRole = Constants.K_ROLE_RESERVE;
						if (load > 0)
							p.PlayerRole = Constants.K_ROLE_BACKUP;

						if (load > 40)
						{
							p.PlayerRole = Constants.K_ROLE_STARTER;
						}
					}

					Utility.Announce(string.Format("{0,-25} : {1} : {2,3} : {3,5:##0.0}% : {4} : {5}",
						p.PlayerName.PadRight(25), p.PlayerRole, p.TotStats.YDc,
						load, p.PlayerRole, p.PlayerPos
						));

					Utility.TflWs.StorePlayerRoleAndPos(p.PlayerRole, p.PlayerPos, p.PlayerCode);
				}
			}
		}

		public void DumpUnitByYardage(int tot)
		{
			var wideCnt = 0;
			foreach (var p in Receivers)
			{
				if (p.IsWideout())
				{
					var load = Utility.Percent(p.TotStats.YDc, tot);

					//  Returned fron the dead
					if (load > 0 && (p.PlayerRole == Constants.K_ROLE_INJURED || p.PlayerRole == Constants.K_ROLE_SUSPENDED))
						p.PlayerRole = Constants.K_ROLE_DEEP_RESERVE;

					if (p.PlayerRole != Constants.K_ROLE_INJURED && p.PlayerRole != Constants.K_ROLE_SUSPENDED)
					{
						p.PlayerRole = Constants.K_ROLE_RESERVE;
						if (load > 0)
						{
							p.PlayerRole = Constants.K_ROLE_BACKUP;
							if (load > 10)
							{
								wideCnt++;
								if (wideCnt < 4)
								{
									p.PlayerPos += string.Format(",W{0}", wideCnt);
									p.PlayerRole = Constants.K_ROLE_STARTER;
								}
							}
						}
					}

					Utility.Announce(string.Format("{0,-25} : {1} : {2,3} : {3,5:##0.0}% : {4} : {5}",
						p.PlayerName.PadRight(25), p.PlayerRole, p.TotStats.YDc,
						load, p.PlayerRole, p.PlayerPos
						));

					Utility.TflWs.StorePlayerRoleAndPos(p.PlayerRole, p.PlayerPos, p.PlayerCode);
				}
			}
		}

		private static int ComparePlayersByYdc(NFLPlayer x, NFLPlayer y)
		{
			if (x == null)
			{
				if (y == null)
					return 0;
				return -1;
			}
			return y == null ? 1 : y.TotStats.YDc.CompareTo(x.TotStats.YDc);
		}

		private static int ComparePlayersByYdp(NFLPlayer x, NFLPlayer y)
		{
			if (x == null)
			{
				if (y == null)
					return 0;
				return -1;
			}
			return y == null ? 1 : y.TotStats.YDp.CompareTo(x.TotStats.YDp);
		}

		public void DumpUnitByPassingYardage(int tot)
		{
			foreach (var p in Quarterbacks)
			{
				if (p.IsQuarterback())
				{
					var load = Utility.Percent(p.TotStats.YDp, tot);

					//  Returned fron the dead
					if (load > 0 && (p.PlayerRole == Constants.K_ROLE_INJURED || p.PlayerRole == Constants.K_ROLE_SUSPENDED))
						p.PlayerRole = Constants.K_ROLE_RESERVE;

					if (p.PlayerRole != Constants.K_ROLE_INJURED && p.PlayerRole != Constants.K_ROLE_SUSPENDED)
					{
						p.PlayerRole = Constants.K_ROLE_BACKUP;
						if (load > 0)
						{
							p.PlayerRole = Constants.K_ROLE_BACKUP;
							if (load > 20)
							{
								p.PlayerRole = Constants.K_ROLE_STARTER;
							}
						}
					}

					Utility.Announce(string.Format("{0,-25} : {1} : {2,3} : {3,5:##0.0}% : {4} : {5}",
						p.PlayerName.PadRight(25), p.PlayerRole, p.TotStats.YDc,
						load, p.PlayerRole, p.PlayerPos
						));

					Utility.TflWs.StorePlayerRoleAndPos(p.PlayerRole, p.PlayerPos, p.PlayerCode);
				}
			}
		}
	}
}
