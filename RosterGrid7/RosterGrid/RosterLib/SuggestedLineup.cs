﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;

namespace RosterLib
{
	public class SuggestedLineup
	{
		public string LeagueId { get; set; }
		public FantasyLeague League { get; set; }

		/// <summary>
		///   The team we are suggesting a lineup for
		/// </summary>
		public string TeamCode { get; set; }

		public string OwnerCode { get; set; }

		public Hashtable RankMaster { get; set; }

		public string Season { get; set; }
		public int Week { get; set; }

		public NFLWeek NflWeek { get; set; }

		public IRatePlayers Scorer { get; set; }

		private List<NFLPlayer> _playerList;
		private readonly List<NFLPlayer> _usedPlayers;

		/// <summary>
		///   options to set after instantiation
		/// </summary>
		public bool IncludeFreeAgents { get; set; }

		public bool IncludeSpread { get; set; }
		public bool IncludeRatingModifier { get; set; }

		public SuggestedLineup(string leagueId, string ownerCode, string teamCode, string season, int week)
		{
#if DEBUG
			Utility.Announce(string.Format("Suggesting a lineup for {0} in league {1} team {2} - {3}:{4}",
			                               ownerCode, leagueId, teamCode, season, week));
#endif
			LeagueId = leagueId;
			League = new FantasyLeague(leagueId);
			TeamCode = teamCode;
			OwnerCode = ownerCode;
			Season = season;
			Week = week;
			NflWeek = new NFLWeek(Season, Week.ToString());
			if ( LeagueId.Equals( Constants.K_LEAGUE_Yahoo ) || LeagueId.Equals( Constants.K_LEAGUE_PerfectChallenge ) )
				Scorer = new EspnScorer(NflWeek);
			else
				Scorer = new GS4Scorer(NflWeek);

			_usedPlayers = new List<NFLPlayer>();
			RankMaster = new Hashtable();
			IncludeFreeAgents = false;
		}

		public void Render()
		{
			var str = new SimpleTableReport(string.Format("Suggested Lineup {0}:{1:#0} {2}", Season, Week, LeagueId));
			str.AddStyle(
				"#container { text-align: left; background-color: #ccc; margin: 0 auto; border: 1px solid #545454; padding:10px; font: 13px/19px Trebuchet MS, Georgia, Times New Roman, serif; }");
			str.AddStyle("#main { margin-left:1em; }");
			str.AddStyle("#dtStamp { font-size:0.8em; }");
			str.AddStyle(".end { clear: both; }");
			str.AddStyle(".gponame { color:white; background:black }");
			str.AddStyle(
				"label { display:block; float:left; width:130px; padding: 3px 5px; margin: 0px 0px 5px 0px; text-align:right; }");
			str.AddStyle(
				"value { display:block; float:left; width:100px; padding: 3px 5px; margin: 0px 0px 5px 0px; text-align:left; font-weight: bold; color:blue }");
			str.AddStyle(
				"#notes { float:right; height:auto; width:308px; font-size: 88%; background-color: #ffffe1; border: 1px solid #666666; padding: 5px; margin: 0px 0px 10px 10px; color:#666666 }");
			str.AddStyle(
				"div.notes H4 { background-image: url(images/icon_info.gif); background-repeat: no-repeat; background-position: top left; padding: 3px 0px 3px 27px; border-width: 0px 0px 1px 0px; border-style: solid; border-color: #666666; color: #666666; font-size: 110%;}");
			str.ColumnHeadings = true;
			str.DoRowNumbers = false;
			str.ShowElapsedTime = false;
			str.IsFooter = false;
			str.AddColumn(new ReportColumn("Slot", "SLOT", "{0:00}"));
			str.AddColumn(new ReportColumn("Player", "PLAYER", "{0}"));
			str.AddColumn(new ReportColumn("Team", "TEAM", "{0}"));
			str.AddColumn(new ReportColumn("Pos", "POS", "{0}"));
			str.AddColumn(new ReportColumn("RPoints", "PTS", "{0}"));
			str.AddColumn(new ReportColumn("Role", "ROLE", "{0}"));
			str.AddColumn(new ReportColumn("Game", "GAME", "{0}"));
			str.AddColumn(new ReportColumn("OppUnit", "OPPRATE", "{0}"));
			str.AddColumn(new ReportColumn("Spread", "SPREAD", "{0:##.#}"));
			str.AddColumn(new ReportColumn("Total", "TOTAL", "{0:##.#}"));
			str.AddColumn(new ReportColumn("Actual", "ACTUAL", "{0:##}"));
			str.LoadBody(BuildTable());
			str.SetSortOrder( "SLOT,PTS DESC" );
			str.RenderAsHtml(FileName(), true);
		}

		public string FileName()
		{
			return string.Format("{0}{1}//Lineups//{3}//Suggested-{4}-{2:00}.htm", 
				Utility.OutputDirectory(), Season, Week, LeagueId, OwnerCode);
		}

		private DataTable BuildTable()
		{
			var dt = new DataTable();
			var cols = dt.Columns;
			cols.Add("SLOT", typeof (String));
			cols.Add("PLAYER", typeof (String));
			cols.Add("TEAM", typeof (String));
			cols.Add("POS", typeof (String));
			cols.Add("PTS", typeof (Int16));
			cols.Add("ROLE", typeof (String));
			cols.Add("GAME", typeof (String));
			cols.Add("OPPRATE", typeof (String));
			cols.Add("SPREAD", typeof (Decimal));
			cols.Add("TOTAL", typeof (Decimal));
			cols.Add("ACTUAL", typeof (Int16));

			// starters
			foreach (var s in League.LineupSlots)
			{
				Utility.Announce(string.Format("  Picking a player for slot {0} - {1}, rank {2}",
				                                         s.SlotNumber, s.SlotCode, s.Rank));
				PickPlayer(s);
				AddPlayerLineWithPlayer( dt, false, s.PlayerSelected, s.SlotNumber);
			}
			//  blank line
			var blank = dt.NewRow();
			blank[ "SLOT" ] = 99;
			dt.Rows.Add(blank);

			if ( LeagueId != Constants.K_LEAGUE_PerfectChallenge )
			{
				var teamRow = Utility.TflWs.GetFTeamDr( Season, LeagueId, OwnerCode );
				_playerList = new List<NFLPlayer>();
				for ( var i = 65; i < 91; i++ ) //  from A-Z
				{
					var playerCode = string.Format( "PLAYER{0}", (char) i );
					var playerId = teamRow[ playerCode ].ToString().Trim();
					if (playerId.Length > 0)
						AddPlayerLine( dt, playerId, freeAgent: false );
				}
			}

			if ( ( LeagueId.Equals( Constants.K_LEAGUE_Yahoo ) || LeagueId.Equals( Constants.K_LEAGUE_PerfectChallenge ) )
				&& IncludeFreeAgents)
			{
#if ! DEBUG
				var blank2 = dt.NewRow();
				dt.Rows.Add(blank2);
				AddFreeAgents(dt, "1");

				var blank3 = dt.NewRow();
				dt.Rows.Add(blank3);
				AddFreeAgents(dt, "2");
				var blank4 = dt.NewRow();
				dt.Rows.Add(blank4);
				AddFreeAgents(dt, "3");
#endif
				var blank5 = dt.NewRow();
				dt.Rows.Add(blank5);
				AddFreeAgents(dt, "4");
			}
			return dt;
		}

		private void AddFreeAgents(DataTable dt, string catCode)
		{
			var ds = Utility.TflWs.GetOffensivePlayers(catCode);
			foreach ( var row in ds.Tables[ 0 ].Rows.Cast<DataRow>()
				.Where( row => row[ "POSDESC" ].ToString().IndexOf( "FB" ) < 0 ) )
				AddPlayerLine( dt, row[ "PLAYERID" ].ToString().Trim(), freeAgent: true );
		}

		private string ActualPoints(NFLPlayer p)
		{
			var nScore = Scorer.RatePlayer(p, NflWeek);
			return nScore.ToString();
		}

		private void AddPlayerLine(DataTable dt, string playerId, [Optional] bool freeAgent)
		{
			var p = new NFLPlayer(playerId, LeagueId);
			AddPlayerLineWithPlayer(dt, freeAgent, p);
		}

		private void AddPlayerLineWithPlayer(DataTable dt, bool freeAgent, NFLPlayer p, [Optional] int slotNumber)
		{
			if (!NotUsed( p ) || !IsAvailable( p )) return;

			NFLGame game = null;

			if (p.CurrTeam != null)
				game = p.CurrTeam.GameFor(Season, Week);

			var dr = dt.NewRow();
			if (freeAgent)
				dr["SLOT"] = "999";
			else
			{
				if (slotNumber > 0)
					dr["SLOT"] = string.Format( "{0:00}", slotNumber );
				else
					dr["SLOT"] = "Bench";
			}

			if ((p.CurrTeam != null) && (game != null))
			{
				var opponent = p.CurrTeam.OpponentFor(Season, Week);
				//  player actually available
				dr["PLAYER"] = p.Url(p.PlayerName, forceReport: false);
				dr["TEAM"] = p.CurrTeam.TeamCode;
				dr["ROLE"] = p.PlayerRole;
				dr["POS"] = p.PlayerPos;

				dr["PTS"] = RankPoints( p, game, opponent );
				dr["GAME"] = string.Format( "{0} {1} {2}",
				                           game.GameDay(), game.Hour, game.OpponentOut(p.CurrTeam.TeamCode));
//				dr["SPREAD"] = PlayerSpread(game.GetSpread(), game.IsHome(p.CurrTeam.TeamCode));
				dr[ "SPREAD" ] = game.GetSpread();
				dr[ "TOTAL" ] = game.Total;

				if (opponent != null)
					dr["OPPRATE"] = opponent.Ratings;

				if (game.Played())
					dr["ACTUAL"] = ActualPoints(p);

				_usedPlayers.Add(p);
			}
			dt.Rows.Add(dr);
		}

		public decimal PlayerSpread(decimal spread, bool isHome)
		{
			var modifier = 1.0M;

			if ( isHome )
			{
				if ( spread > 0 )
				{
					//  home win
					if (spread > 13M)
						// big win expected
						modifier = 1.4M;
					else if (spread > 9.5M)
						modifier = 1.3M;
					else if (spread > 6.5M)
						modifier = 1.2M;
					else if (spread > 2.5M)
						modifier = 1.1M;
				}
				else if ( spread < 0 )
				{
					//  home loss
					if (spread < -13M)
						// big los expected
						modifier = .6M;
					else if (spread < -9.5M)
						modifier = .7M;
					else if (spread < -6.5M)
						modifier = .8M;
					else if (spread < -2.5M)
						modifier = .9M;
				}
			}
			else
			{
				//  away
				if ( spread > 0 )
				{
					//  home win
					if ( spread > 13M )
					{
						// big win expected
						modifier = .6M;
					}
					else if ( spread > 9.5M )
					{
						modifier = .7M;
					}
					else if ( spread > 6.5M )
					{
						modifier = .8M;
					}
					else if ( spread > 2.5M )
					{
						modifier = .9M;
					}
				}
				else if ( spread < 0 )
				{
					//  away win
					if ( spread < -13 )
					{
						// big win expected
						modifier = 1.4M;
					}
					else if ( spread < -9.5M )
					{
						modifier = 1.3M;
					}
					else if ( spread < -6.5M )
					{
						modifier = 1.2M;
					}
					else if ( spread < -2.5M )
					{
						modifier = 1.1M;
					}
				}
			}
			return modifier;
		}

		//  fill the available slot with a player from the roster or maybe a free agent
		private void PickPlayer(LineupSlot slot)
		{
			_playerList = new List<NFLPlayer>();

			if ( LeagueId != Constants.K_LEAGUE_PerfectChallenge )
			{
				//  select eligible players from the fantasy roster
				var dr = Utility.TflWs.GetFTeamDr( Season, LeagueId, OwnerCode );
				//  add rostered players
				for ( var i = 65; i < 91; i++ ) //  from A-Z
				{
					var playerCode = string.Format( "PLAYER{0}", (char) i );
					AddPlayer( dr[ playerCode ].ToString().Trim(), slot.SlotType, freeAgent: false );
				}
			}

			if (( LeagueId.Equals(Constants.K_LEAGUE_Yahoo) || LeagueId.Equals( Constants.K_LEAGUE_PerfectChallenge ) )
				&& IncludeFreeAgents )
			{
				//  append to the roster all free agents starters available
				var cats = string.Empty;
				foreach (var sType in slot.SlotType)
				{
					var sCat = sType;
					if (sCat == "W" || sCat == "T") sCat = "3";
					if (cats.IndexOf(sCat) < 0)
						cats += sCat;
				}
				var ds = Utility.TflWs.GetOffensivePlayers(cats);
				foreach ( var row in ds.Tables[ 0 ].Rows.Cast<DataRow>()
					.Where( row => ! HaveAlreadyGotOne( row[ "PLAYERID" ].ToString().Trim() ) ) )
					AddPlayer( row[ "PLAYERID" ].ToString().Trim(), slot.SlotType, freeAgent: true );
			}

#if DEBUG
			Utility.Announce( string.Format( "{0} players to choose from for {1}", _playerList.Count, slot.SlotCode ) );
#endif
			// pick the best player from the list
			slot.PlayerSelected = ChooseBestPlayer(slot);
		}

		private NFLPlayer ChooseBestPlayer(LineupSlot slot)
		{
			var selectedPlayer = ChoosePlayerForSlot(slot.Rank);
			return selectedPlayer;
		}

		private NFLPlayer ChoosePlayerForSlot(int rank)
		{
#if DEBUG
			Utility.Announce("    Choosing from:");
#endif
			//  start with dummy player
			var selectedPlayer = new NFLPlayer("-", "**", "?", "*", "-", "*", null);
			var availablePlayers = new List<NFLPlayer>();
			foreach (var plyr in _playerList)
			{
				var game = plyr.CurrTeam.GameFor(Season, Week);
				var opponent = plyr.CurrTeam.OpponentFor(Season, Week);
				if (opponent != null)
				{
					//  work out projected points
					plyr.Points = RankPoints(plyr, game, opponent);

					if (LeagueId.Equals(Constants.K_LEAGUE_Gridstats_NFL1))
					{
						if (IsAvailable(plyr))
							availablePlayers.Add(plyr);
					}
					else
					{
						if (NotUsed(plyr) && IsAvailable(plyr))
							availablePlayers.Add(plyr);
					}
				}
				else
					plyr.Points = 0;
			}
			//  pick out the player depending on rank
			availablePlayers.Sort((p1, p2) => p1.Points.CompareTo(p2.Points)*-1);

			var selectionIndex = 0; //  always take best available
			if (LeagueId.Equals(Constants.K_LEAGUE_Gridstats_NFL1))
				selectionIndex = rank - 1;

			if (availablePlayers.Count > selectionIndex)
				selectedPlayer = availablePlayers[selectionIndex];


			foreach ( var p in availablePlayers )
				Utility.Announce( string.Format( "      {0,-16} who has {1,4:#0.#} points",
				                                 p.PlayerNameShort, p.Points ) );
			Utility.Announce(string.Format("   {0} has been selected with {1:#0.#} points",
			                                         selectedPlayer.PlayerNameShort, selectedPlayer.Points));
			Utility.Announce("   ------------------------------------------------");

			return selectedPlayer;
		}

		private bool HaveAlreadyGotOne(string playerCode)
		{
			return _playerList.Any(p => p.PlayerCode.Equals(playerCode));
		}

		//  rank points are used to sort players
		public decimal RankPoints( NFLPlayer player, NFLGame game, NflTeam opponent )
		{
			decimal points = 0;

			#region  short cut

			if (RankMaster.Contains(player.PlayerCode))
			{
				var rankedPlayer = (NFLPlayer) RankMaster[player.PlayerCode];
				return rankedPlayer.Points;
			}
			#endregion

			#region  Get the average output for the last 3 games

			//  Always start with the Average points in their last 3 games, if 0 and QB use 14
			if (game != null)
			{
				if (player.PlayerRole.Equals(Constants.K_ROLE_STARTER))
				{
					var avgPoints = AveragePoints(player);

					//  Rookies and injured players may not have played in the last 3 games
					if ( player.PlayerCat.Equals( "1" ) )
					{
						if (avgPoints.Equals( 0.0M )) avgPoints = 14.0M;  // avergage for a QB
					}
					if ( player.PlayerCat.Equals( "2" ) )
					{
						if ( avgPoints.Equals( 0.0M ) ) avgPoints = 8.0M;  // avergage for a RB
					}
					if ( player.PlayerCat.Equals( "3" ) )
					{
						if ( avgPoints.Equals( 0.0M ) ) avgPoints = 6.0M;  // avergage for a RB
					}
					else if ( player.PlayerCat.Equals( "4" ))
					{
						if (avgPoints.Equals( 0.0M )) avgPoints = 6.0M;  // avergage day for a PK						
					}
					points += avgPoints;
				}
			}

			#endregion

			#region  Consider the likely result

			decimal spreadModifier = 1;  //  no modification

			if ( IncludeSpread )
			{
				if (game != null) spreadModifier = PlayerSpread( game.GetSpread(), game.IsHome( player.CurrTeam.TeamCode ) ) / 1;

				points *= spreadModifier;
			}

			#endregion

			#region  factor in the quality of the opponent

			if ( IncludeRatingModifier )
			{
				if ( opponent != null )
				{
					var oppRating = player.OpponentRating( opponent.Ratings );
					var ratingModifier = RatingModifier( oppRating );
					points *= ratingModifier;
				}
			}

			#endregion

			player.Points = points;
			RankMaster.Add(player.PlayerCode, player); //  save points for later

#if DEBUG
			//  verbose audit trail of the calculations
			//Utility.Announce( string.Format(
			//   "{0,-16} has {1,4:#0.#} r pts- spr {2,4:#0.#} avgFP last 3 {3,4:##.#} rating mod {4,4:#0.#} {5}",
			//   player.PlayerNameShort, points, spreadModifier, avgPoints, ratingModifier, oppRating ) );
#endif
			return points;
		}

		public decimal AveragePoints(NFLPlayer p)
		{
			var prevWeek = NflWeek.PreviousWeek( NflWeek, false, regularSeasonGamesOnly:true );

			var nTotal = Scorer.RatePlayer(p, prevWeek);

			var twoWeeksAgo = prevWeek.PreviousWeek( prevWeek, false, regularSeasonGamesOnly: true );

			nTotal += Scorer.RatePlayer(p, twoWeeksAgo);

			var threeWeeksAgo = twoWeeksAgo.PreviousWeek( twoWeeksAgo, false, regularSeasonGamesOnly: true );

			nTotal += Scorer.RatePlayer(p, threeWeeksAgo);

#if DEBUG
//			Utility.Announce( string.Format( "Total points last three games {0}", nTotal ) );
#endif
			return nTotal/3;
		}

		public decimal RatingModifier(string rating)
		{
			var modifier = 1.0M;
			switch (rating)
			{
				case "A":
					modifier = 0.5M;
					break;
				case "B":
					modifier = 0.75M;
					break;
				case "D":
					modifier = 1.25M;
					break;
				case "E":
					modifier = 1.5M;
					break;
			}
			return modifier;
		}

		private void AddPlayer(string playerId, IEnumerable<string> slotTypes, [Optional] bool freeAgent)
		{
			if (playerId.Trim().Length <= 0) return;

			// determine eligible players
			var p = new NFLPlayer(playerId, LeagueId);

			if (!IsEligible( p, slotTypes )) return;

			if (freeAgent) p.PlayerName += "-?";
			_playerList.Add(p);
		}

		private bool NotUsed(NFLPlayer p)
		{
			return ! IsUsed(p);
		}

		private bool IsUsed(NFLPlayer p)
		{
			return _usedPlayers.Any(up => p.PlayerCode.Equals(up.PlayerCode));
		}

		private bool IsAvailable(NFLPlayer p)
		{
			return p.Owner.Equals("**") || p.Owner.Equals(TeamCode);
		}

		private bool IsEligible(NFLPlayer p, IEnumerable<string> slotTypes)
		{
			var isEligible = false;

			if (! IsAvailable(p))
				return false;

			if (p.PlayerRole.Equals(Constants.K_ROLE_BACKUP) ||
			    p.PlayerRole.Equals(Constants.K_ROLE_STARTER))
			{
				foreach (var slottype in slotTypes)
				{
					if (slottype.Equals("2"))
					{
						if (p.PlayerCat.Equals("2") && (p.PlayerPos.Contains("RB") || p.PlayerPos.Contains("HB")))
						{
							isEligible = true;
							break;
						}
						break;
					}
					if (slottype.Equals("T"))
					{
						if (p.PlayerCat.Equals("3") && p.PlayerPos.Contains("TE"))
						{
							isEligible = true;
							break;
						}
					}
					if (slottype.Equals("W"))
					{
						if (p.PlayerCat.Equals("3") && p.PlayerPos.Contains("WR"))
						{
							isEligible = true;
							break;
						}
					}
					if (p.PlayerCat.Equals(slottype))
					{
						isEligible = true;
						break;
					}
				}
			}
			return isEligible;
		}
	}
}