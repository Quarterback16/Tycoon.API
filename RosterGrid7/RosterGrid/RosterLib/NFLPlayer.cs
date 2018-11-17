using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Runtime.InteropServices;
using TFLLib;

namespace RosterLib
{
	/// <summary>
	/// Summary description for NFLPlayer.
	/// </summary>
	public class NFLPlayer : IComparable
	{
		#region  Constants

		public const string K_ROLE_STARTER = "S";
		public const string K_ROLE_BACKUP = "B";
		public const string K_ROLE_RESERVE = "R";
		public const string K_ROLE_INJURED = "I";
		public const string K_ROLE_SUSPENDED = "X";

		public const String K_FANTASY_POSITIONS = "QB,RB,HB,FL,WR,TE,PK,K";
		public const String K_RUSHING_POSITIONS = "RB,HB";

		#endregion

		public string FirstName { get; set; }
		public string Surname { get; set; }
		public string JerseyNo { get; set; }
		public int HeightFeet { get; set; }
		public int HeightInches { get; set; }
		public int Weight { get; set; }
		public string DBirth { get; set; }
		public string College { get; set; }
		public DateTime StartDate { get; set; }

		public string PlayerName;
		public string PlayerNameShort;
		public string PlayerCode;
		public string PlayerRole;
		public string PlayerPos;
		public string PlayerCat;
		public string RookieYear;
		//public string howGot;    //  D1, FA  etc

		public string Owner;

		private string _drafted;
		private string _lastSeason = "";
		public int Scores;
		public int CurrScores;
		private string _teamLastYear;
		private PlayerPos _myCat;

		#region  Projections

		public List<NFLGame> CurrentGames { get; set; }

		#endregion

		public string LoWeek;
		public string HiWeek;

		//  the players list of games played (potentially) 
		//  each game is encapsulated as an NFLPerformance object
		public ArrayList PerformanceList;

		public PlayerStats TotStats;

		#region  Constructors

		public NFLPlayer(string nameIn, string codeIn, string roleIn, string strRookieYrIn,
		                 string strPlayerPos, string strPlayerCat, NflTeam teamIn)
		{
			//RosterLib.Utility.Announce( "NFLPlayer: Instantiating new Player " + nameIn );
			PlayerName = nameIn;
			PlayerCode = codeIn;
			PlayerRole = roleIn;
			PlayerPos = strPlayerPos.Trim();
			PlayerCat = strPlayerCat;
			RookieYear = strRookieYrIn;

			FillPlayer();
			if (RookieYear == Utility.CurrentSeason()) PlayerName += "*";
			Owner = "**";
			LoadOwner();
			PerformanceList = new ArrayList();
			TotStats = new PlayerStats();
			Drafted = Utility.TflWs.Drafted(codeIn);
			_teamLastYear = Utility.TflWs.PlayedFor(PlayerCode, Int32.Parse(Utility.CurrentSeason()) - 1, 17);
			CurrTeam = teamIn;
		}

		public NFLPlayer(string nameIn, string codeIn, string roleIn, string strRookieYrIn,
		                 string strPlayerPos, string dBirthIn, int scoresIn, int currScoresIn, string strCatIn,
		                 NflTeam teamIn)
		{
			RosterLib.Utility.Announce("2. Instantiating new Player " + nameIn);

			PlayerName = nameIn;
			PlayerCode = codeIn;
			PlayerRole = roleIn;
			PlayerPos = strPlayerPos.Trim();
			PlayerCat = strCatIn;
			RookieYear = strRookieYrIn;
			DBirth = dBirthIn;
			Scores = scoresIn;
			CurrScores = currScoresIn;

			FillPlayer();

			CurrTeam = teamIn;
			if (RookieYear == Utility.CurrentSeason()) PlayerName += "*";
			Owner = "**";
			LoadOwner();
			PerformanceList = new ArrayList();
			_teamLastYear = Utility.TflWs.PlayedFor(PlayerCode, Int32.Parse(Utility.CurrentSeason()) - 1, 17);
			TotStats = new PlayerStats();
			Drafted = Utility.TflWs.Drafted(codeIn).Trim();
		}


		public NFLPlayer(DataRow dr, [Optional] string fantasyLeague)
		{
			LoadPlayer(dr);
			if (! string.IsNullOrEmpty(fantasyLeague))
				LoadOwner(fantasyLeague);
		}

		public NFLPlayer( string playerId )
		{
			if ( playerId.Trim().Length > 0 )
			{
				PlayerCode = playerId;
				FillPlayer();
				TotStats = new PlayerStats();
				_teamLastYear = Utility.TflWs.PlayedFor( PlayerCode, Int32.Parse( Utility.CurrentSeason() ) - 1, 17 );
				CurrTeam = new NflTeam( TeamCode );
			}
			else
				Utility.Announce( "NFLPlayer: Unable to instantiate " + playerId );			
		}

		public NFLPlayer(string playerId, [Optional] string fantasyLeague)
		{
			if (playerId.Trim().Length > 0)
			{
				PlayerCode = playerId;
				FillPlayer();
				TotStats = new PlayerStats();
				LoadOwner(fantasyLeague);
				_teamLastYear = Utility.TflWs.PlayedFor(PlayerCode, Int32.Parse(Utility.CurrentSeason()) - 1, 17);
				CurrTeam = new NflTeam(TeamCode);
			}
			else
				Utility.Announce("NFLPlayer: Unable to instantiate " + playerId);
		}

		public NFLPlayer(XmlNode node)
		{
#if DEBUG
			Utility.Announce( "Instantiating new Player via xml" );		
#endif
			if (node.Attributes != null)
			{
				PlayerCode = node.Attributes["id"].Value;
				PlayerName = node.Attributes["name"].Value;
				RookieYear = node.Attributes["rookie-year"].Value;
				PlayerPos = node.Attributes["pos"].Value;
				PlayerRole = node.Attributes["role"].Value;
				StarRating = node.Attributes["star"].Value;
				Bio = node.Attributes["bio"].Value;
				PlayerCat = node.Attributes["cat"].Value;
				DBirth = node.Attributes["dob"].Value;
				Owner = node.Attributes["owner"].Value;
				JerseyNo = node.Attributes["jersey"].Value;
				Injury = node.Attributes["INJURY"].Value;
				var team = node.Attributes["currteam"].Value;
				CurrTeam = Masters.Tm.GetTeam( Utility.CurrentSeason(), team );
			}
			if (node.Attributes != null)
			{
				Scores = Int32.Parse(node.Attributes["Scores"].Value);
				CurrScores = Int32.Parse(node.Attributes["currscores"].Value);
			}
			if (RookieYear == Utility.CurrentSeason()) PlayerName += "*";
#if DEBUG
			Utility.Announce( string.Format( "Instantiated {0} via xml", PlayerName ) );
#endif
		}

		public NFLPlayer()
		{
		}

		#endregion

		#region  Accessors

		public string TeamCode
		{
			get
			{
				if (CurrTeam == null)
				{
					Utility.Announce(string.Format("NFLPlayer.TeamCode Player {0} {1} has a null CurrTeam", PlayerName,
					                                         PlayerCode));
					return "??";
				}
				return CurrTeam.TeamCode;
			}
		}

		public decimal Points { get; set; }

		public string Bio { get; set; }

		public string Opponent { get; set; }

		public string OppRate { get; set; }

		public string PlayerSpread { get; set; }

		public string LineupPos { get; set; }

		public bool IsRetired { get; set; }

		public string LastSeason
		{
			get { return _lastSeason; }
			set { _lastSeason = value; }
		}

		public string Drafted
		{
			get
			{
				if (_drafted == null) _drafted = Utility.TflWs.Drafted(PlayerCode);
				return _drafted;
			}
			set { _drafted = value; }
		}

		public decimal ExperiencePoints { get; set; }

		public bool GamesLoaded { get; set; }

		public bool EpDone { get; set; }

		public NflTeam CurrTeam { get; set; }

		public string TeamLastYear
		{
			get
			{
				if (_teamLastYear == null)
					_teamLastYear = Utility.TflWs.PlayedFor(PlayerCode, Int32.Parse(Utility.CurrentSeason()) - 1, 17);
				return _teamLastYear;
			}
			set { _teamLastYear = value; }
		}

		public string StarRating { get; set; }

		public int ProjectedTDp { get; set; }

		public int ProjectedTDr { get; set; }

		public int ProjectedTDc { get; set; }

		public int ProjectedYDp { get; set; }

		public int ProjectedYDr { get; set; }

		public int ProjectedYDc { get; set; }

		public int ProjectedFg { get; set; }

		public int ProjectedPat { get; set; }

		public int ProjectedReceptions { get; set; }

		private void SetLastSeason()
		{
			LastSeason = Utility.TflWs.Retired(PlayerCode);
		}

		public string Status()
		{
			string statusOut;

			if (TeamCode == "??")
			{
				//  workout if the guy has retired yet
				SetLastSeason();
				IsRetired = (LastSeason.Length > 0);
				statusOut = IsRetired ? "Retired" : "Free Agent";
			}
			else
			{
				//  return the Name of the Team
				if (CurrTeam == null) CurrTeam = new NflTeam(TeamCode);
				statusOut = CurrTeam.NameOut();
			}
			return statusOut;
		}

		public string Unit()
		{
			string unitOut = "";

			if (PlayerCat != null)
			{
				if (PlayerCat == "1")
					unitOut = "PO";
				else if (PlayerCat == "2")
					unitOut = "RO";
				else if (PlayerCat == "3")
					unitOut = "PO";
				else if (PlayerCat == "4")
					unitOut = "KK";
				else if (PlayerCat == "5")
					unitOut = "RD";
				else if (PlayerCat == "6")
					unitOut = "PD";
				else if (PlayerCat == "7")
					unitOut = "PP";

				//  Pass Rushes are a subset of Category 5
				if (PlayerPos.IndexOf("DE") >= 0)
					unitOut = "PR";
				else if (PlayerPos.IndexOf("DT") >= 0)
					unitOut = "PR";
			}
			return unitOut;
		}

		public string UnitRating()
		{
			string myUnitRating = "?";

			switch (Unit())
			{
				case "PO":
					myUnitRating = CurrTeam.Ratings.Substring(0, 1);
					break;
				case "RO":
					myUnitRating = CurrTeam.Ratings.Substring(1, 1);
					break;
				case "PP":
					myUnitRating = CurrTeam.Ratings.Substring(2, 1);
					break;
				case "PR":
					myUnitRating = CurrTeam.Ratings.Substring(3, 1);
					break;
				case "RD":
					myUnitRating = CurrTeam.Ratings.Substring(4, 1);
					break;
				case "PD":
					myUnitRating = CurrTeam.Ratings.Substring(5, 1);
					break;
			}

			return myUnitRating;
		}

		public string OpponentRating(string ratings)
		{
			var myOpponentRating = "?";

			if (ratings.Length.Equals( 6 ))
			{
				switch (Unit())
				{
					case "PO":
						myOpponentRating = ratings.Substring( 5, 1 ); // PD
						break;
					case "RO":
						myOpponentRating = ratings.Substring( 4, 1 ); //  RD
						break;
					case "PP":
						myOpponentRating = ratings.Substring( 3, 1 ); //  PR
						break;
					case "PR":
						myOpponentRating = ratings.Substring( 2, 1 ); // PP
						break;
					case "RD":
						myOpponentRating = ratings.Substring( 1, 1 );
						break;
					case "PD":
						myOpponentRating = ratings.Substring( 0, 1 );
						break;
				}
			}
			else
				Utility.Announce( "Cant get opponent ratings for " + PlayerName + ": " + ratings );

			return myOpponentRating;
		}

		#endregion

		public DataSet LastScores(string scoreType, int weekFrom, int weekTo, string season, string id)
		{
#if DEBUG
			//Utility.Announce( string.Format( "Getting last Scores {0} for {1}s season {2} from:{3} to:{4}", 
			//   Utility.ScoreTypeOut( scoreType ), PlayerName, season, weekTo, weekFrom ) );
#endif
			var ds = Utility.TflWs.GetScoresForWeeks(scoreType, PlayerCode, season, weekTo, weekFrom, id);

#if DEBUG
			//Utility.DumpDataSet( ds );
#endif
			return ds;
		}

		public DataSet LastStats(string statType, int weekFrom, int weekTo, string season)
		{
			//  Get the last 4 weeks of stats for this player
#if DEBUG
			//Utility.Announce( string.Format( "NFLPlayer.LastStats -Getting {2}:{3}-{4} {0} stats for {1}", 
			//   Utility.StatTypeOut( statType ), PlayerName, season, weekFrom, weekTo ) );
#endif
			var ds = Utility.TflWs.GetStatsForWeeks( PlayerCode, season, weekTo, weekFrom, statType );
#if DEBUG
			//Utility.DumpDataSet( ds );
#endif
			return ds;
		}

		private void FillPlayer()
		{
			var ds = Utility.TflWs.GetPlayer(PlayerCode);
			var dt = ds.Tables["player"];
			if (dt.Rows.Count == 1)
				LoadPlayer(dt.Rows[0]);
		}

		public override string ToString()
		{
			return PlayerName;
		}

		public bool IsStar()
		{
			if (StarRating == null)
				return false;

			return StarRating.Trim().Length > 0;
		}

		public void LoadPlayer(DataRow dr)
		{
			if (dr.RowState == DataRowState.Deleted) return;

			PlayerCode = dr["PLAYERID"].ToString();
			PlayerName = dr["firstname"].ToString().Trim() + " " + dr["surname"].ToString().Trim();
			var firstName = dr["firstname"].ToString().Trim();
			if (firstName.Length > 0)
				PlayerNameShort = firstName.Substring(0, 1) + dr["surname"].ToString().Trim();
			else
				PlayerNameShort = dr["surname"].ToString().Trim();

			RookieYear = dr["rookieyr"].ToString();
			PlayerPos = dr["posdesc"].ToString().Trim();
			if ( PlayerPos == "K" ) PlayerPos = "PK";
			PlayerRole = dr["ROLE"].ToString();
			PlayerCat = dr["CATEGORY"].ToString();
			Injury = dr["INJURY"].ToString();
			StarRating = dr["STAR"].ToString();
			Bio = FixBio(dr["BIO"].ToString());
			JerseyNo = dr["JERSEY"].ToString();
			var d = Convert.ToDateTime(dr["DOB"].ToString());
			DBirth = d.ToShortDateString();
			Scores = Int32.Parse(dr["Scores"].ToString());
			CurrScores = Int32.Parse(dr["curscores"].ToString());
			CurrTeam = new NflTeam( dr["CURRTEAM"].ToString() );
			if (RookieYear == Utility.CurrentSeason()) PlayerName += "*";
			TotStats = new PlayerStats();
		}

		/// <summary>
		/// Fixes the bio by removing the "ì" characters );.
		/// Replace() does not apear to work
		/// </summary>
		/// <param name="bioIn">The bio stored in the memo file.</param>
		/// <returns>fixed string</returns>
		private string FixBio(string bioIn)
		{
			if (bioIn.Length > 0)
			{
				StringBuilder sb = new StringBuilder();
				for (int i = 0; i < bioIn.Length; i++)
				{
					char c = bioIn[i];
					int n = c;
					//RosterLib.Utility.Announce( string.Format("Character {0} is {1}", c, n ));
					if ((n == 236) || (n == 10))
					{
						//  skip wierd clipper characters
					}
					else
					{
						sb.Append(c);
					}
				}
				Bio = sb.ToString();
			}
			return Bio;
		}

		public void SetLoWeek(string loWeekIn)
		{
			LoWeek = loWeekIn;
		}

		public void SetHiWeek(string hiWeekIn)
		{
			HiWeek = hiWeekIn;
		}

		public void LoadSeason(string season)
		{
			if (season == null) season = Utility.CurrentSeason();

			var ds = Utility.TflWs.GetSeason(CurrTeam.TeamCode, season);
			var dt = ds.Tables[0];
			if (CurrentGames == null) CurrentGames = new List<NFLGame>();
			CurrentGames.Clear();
			foreach ( var game in from DataRow dr in dt.Rows select new NFLGame( dr ) )
			{
				game.LoadPrediction("unit");
				CurrentGames.Add( game );
			}
		}

		public void LoadPerformances(bool allGames, bool currSeasonOnly, string whichSeason)
		{
			if (String.IsNullOrEmpty( PlayerName )) return;
			Utility.Announce( string.Format( "NFLPlayer.LoadPerformances allGames={0}", allGames ) );
			if (GamesLoaded) return;
			Utility.Announce( string.Format( "NFLPlayer.LoadPerformances GamesLoaded {0}", GamesLoaded ) );
			var nYearToGoTo = allGames ? Int32.Parse(RookieYear) : Int32.Parse(whichSeason);
			Utility.Announce(string.Format("NFLPlayer.LoadPerformances Loading from {0} to {1}",
			                                    Utility.CurrentSeason(), nYearToGoTo));

			for (var s = Int32.Parse(Utility.CurrentSeason()); s >= nYearToGoTo; s--)
			{
#if DEBUG
				Utility.Announce( string.Format( "NFLPlayer.LoadPerformances doing Season {0}", s ) );
#endif
				for (var w = Constants.K_WEEKS_IN_A_SEASON; w > 0; w--)
				{
					// What team did he play for?
					var teamCode = Utility.TflWs.PlayedFor(PlayerCode, s, w);
#if DEBUG
					Utility.Announce(
						string.Format( "NFLPlayer.LoadPerformances: Season:{0}  Week:{1}  Played for {2}",
										  s, w, teamCode ) );
#endif
					if (( teamCode.Length <= 0 ) || ( teamCode.Equals( "??" ) )) continue;

					if (PerformanceList == null) PerformanceList = new ArrayList();
					var perf = new NflPerformance(s, w, teamCode, this);
					PerformanceList.Add(perf);
				}
			}

			var nGames = (PerformanceList == null) ? 0 : PerformanceList.Count;
#if DEBUG
					Utility.Announce(string.Format( "NFLPlayer.LoadPerformances  {0} had {1} games added from {2} to {3}",
					                                         PlayerName, nGames, nYearToGoTo, Utility.CurrentSeason()));
#endif
			TallyPerformance(true, currSeasonOnly, whichSeason);
			GamesLoaded = true; // dont want to do this multiple times
#if DEBUG
				Utility.Announce("Games already loaded for " + PlayerName);
#endif
		}

		#region Total Performance

		public void TallyPerformance(bool allGames, bool currSeasonOnly, string whichSeason)
		{
			if (!string.IsNullOrEmpty(PlayerName))
			{
				//RosterLib.Utility.Announce(string.Format("NFLPlayer.TallyPerformance for {0}", PlayerName));
				if (PerformanceList != null)
				{
					if (TotStats == null) TotStats = new PlayerStats();
					bool tally = true;
					TotStats.Zeroise();
					foreach (NflPerformance p in PerformanceList)
					{
						//RosterLib.Utility.Announce( string.Format( "Doing {0}:{1}", p.Season, p.Week ) );
						if (currSeasonOnly) tally = p.Season.ToString() == whichSeason ? true : false;
						if (tally)
						{
							//RosterLib.Utility.Announce(string.Format("   Tallying {0}:{1}", p.Season, p.Week));

							TotStats.Tdp += p.PerfStats.Tdp;
							TotStats.Tdr += p.PerfStats.Tdr;
							TotStats.Tdc += p.PerfStats.Tdc;
							TotStats.Fg += p.PerfStats.Fg;
							TotStats.Pat += p.PerfStats.Pat;
							TotStats.PatPass += p.PerfStats.PatPass;
							TotStats.PatCatch += p.PerfStats.PatCatch;
							TotStats.PatRun += p.PerfStats.PatRun;
							TotStats.KickRet += p.PerfStats.KickRet;
							TotStats.FumRet += p.PerfStats.FumRet;
							TotStats.IntRet += p.PerfStats.IntRet;
							TotStats.Safety += p.PerfStats.Safety;
							//  Stats
							TotStats.YDr += p.PerfStats.YDr;
							TotStats.Rushes += p.PerfStats.Rushes;
							TotStats.Completions += p.PerfStats.Completions;
							TotStats.PassAtts += p.PerfStats.PassAtts;
							TotStats.YDp += p.PerfStats.YDp;
							TotStats.PassInt += p.PerfStats.PassInt;
							TotStats.Catches += p.PerfStats.Catches;
							TotStats.YDc += p.PerfStats.YDc;
							TotStats.Ints += p.PerfStats.Ints;
							TotStats.Sacks += p.PerfStats.Sacks;
						}
					}
				}
			}
			//RosterLib.Utility.Announce(string.Format("NFLPlayer.TallyPerformance for {0} done", PlayerName));		
		}

		#endregion

		/// <summary>
		///  Works out how many seasons the player played for.
		///  Has to factor in the season the player retired.
		/// </summary>
		/// <returns></returns>
		public int NoOfSeasons()
		{
			var nSeasons = 1;
			SetLastSeason();
			var lastYr = IsRetired ? LastSeason : Utility.CurrentSeason();

			if (RookieYear == null) return 0;

			try
			{
				nSeasons = Int32.Parse(lastYr) - Int32.Parse(RookieYear) + (IsRetired ? 1 : 0);
			}
			catch (FormatException ex)
			{
				Utility.Announce(string.Format("{1} Cant format lastYr={0}", lastYr, ex.Message));
			}

			return nSeasons;
		}

		public int DraftRound()
		{
			SetDraftRound();
			var round = Drafted.Substring(0, 1) == "D" ? Int32.Parse(Drafted.Substring(1, 1)) : 0;
			return round;
		}

		public bool IsInPrime()
		{
			if ((NoOfSeasons() > 3) && (NoOfSeasons() < 9))
				return true;

			return false;
		}

		public string Seasons()
		{
			return string.Format("({0})", NoOfSeasons());
		}

		public bool IsRookie()
		{
			return (RookieYear == Utility.CurrentSeason());
		}

		public bool IsInjured()
		{
			return (PlayerRole == "I");
		}

		public bool IsSuspended()
		{
			return (PlayerRole == "X");
		}

		public bool IsStarter()
		{
			return (PlayerRole == Constants.K_ROLE_STARTER);
		}

		public bool IsOneOrTwo()
		{
			return ( IsStarter() || IsBackup() );
		}

		public string RoleOut()
		{
			string roleOut = "Unknown role";

			switch (PlayerRole)
			{
				case Constants.K_ROLE_STARTER:
					roleOut = "Starter";
					break;
				case Constants.K_ROLE_BACKUP:
					roleOut = "BackUp";
					break;
				case Constants.K_ROLE_RESERVE:
					roleOut = "Reserve";
					break;
				case Constants.K_ROLE_DEEP_RESERVE:
					roleOut = "Deep Reserve";
					break;

				case Constants.K_ROLE_INJURED:
					roleOut = "Injured";
					break;

				case Constants.K_ROLE_SUSPENDED:
					roleOut = "Suspended";
					break;

				case Constants.K_ROLE_HOLDOUT:
					roleOut = "Holdout";
					break;
			}
			return roleOut;
		}

		public bool IsBackup()
		{
			return (PlayerRole == "B");
		}

		public bool IsFreeAgent()
		{
			return (Owner == "**");
		}

		public bool IsPlayoffBound()
		{
//			const string poTeams = "NE,BR,HT,DB,PS,CI,NJ,NG,GB,NO,SF,AF,DL";
//			const string poTeams = "AC,AF,BB,BR,CH,CI,CL,CP,DB,DC.DL,GB,HT,IC,JJ,KC,MD,MV,NE,NG,NJ,NO,OR,PE,PS,SD,SF,SL,SS,TB,TT,WR";  // everyone
			var isPlayoffBound = CurrTeam.IsPlayoffBound;  //  set in TEAM.DBF

			return isPlayoffBound;
		}

		public bool IsNewbie()
		{
			return (CurrTeam.TeamCode != TeamLastYear);
		}

		public bool IsActive()
		{
			return (CurrTeam.TeamCode != "??");
		}

		public void LoadOwner([Optional] string fantasyLeague)
		{
			var theLeague = (string.IsNullOrEmpty(fantasyLeague)) ? Utility.CurrentLeague : fantasyLeague;
			Owner = Utility.TflWs.GetStatus(PlayerCode, theLeague, Utility.CurrentSeason());
		}

		#region Old Grid report

		public string PlayerBox(bool isBold)
		{
			var s = "\n";
			var nameOut = Owner == "**"
			                 	? HtmlLib.Centre(HtmlLib.Font("Verdana", PlayerName, "-1"))
			                 	: HtmlLib.Font("Verdana", PlayerName + " - " + Owner, "-1");
			if ( PlayerRole.Equals( "I" ) || PlayerRole.Equals( "H" ) || PlayerRole.Equals( "X" ) )
			{
				nameOut = HtmlLib.Strikeout( nameOut );
				isBold = false;
			}
			if (isBold) nameOut = HtmlLib.Bold(nameOut);
			if (IsItalic()) nameOut = HtmlLib.Italics(nameOut);

			s += HtmlLib.TableOpen(
				"BORDER=1 WIDTH=144 CELLSPACING=0 CELLPADDING=0" + " BGCOLOR=" + SetColour("RED") +
				" BORDERCOLOR=" + SetBorderColour()) +
			     HtmlLib.TableRowOpen() + "\n" +
			     HtmlLib.TableData(nameOut, SetColour("RED"), "ALIGN=CENTER") + "\n" +
			     HtmlLib.TableRowClose() + "\n" +
			     HtmlLib.TableClose() + "\n";
			return s;
		}

		private string SetBorderColour()
		{
			string theColour;
			switch (PlayerRole)
			{
				case "S":
					theColour = "BLACK";
					break;
				case "B":
					theColour = "BLUE";
					break;
				default:
					theColour = "GRAY";
					break;
			}
			return theColour;
		}

		public bool IsFantasyPlayer()
		{
			bool isFantasyPlayer = false;
			if (PlayerPos != null)
			{
				string[] pos = PlayerPos.Trim().Split(',');
				for (int i = 0; i < pos.Length; i++)
				{
					if (Contains(pos[i], K_FANTASY_POSITIONS))
					{
						isFantasyPlayer = true;
						break;
					}
				}
			}
			return isFantasyPlayer;
		}

		public decimal ScoresPerYear()
		{
			if (NoOfSeasons() > 0)
				return (Decimal.Parse(Scores.ToString())/Decimal.Parse(NoOfSeasons().ToString()));

			return 0.0M;
		}

		public string SetColour(string takenColour)
		{
			string theColour = "WHITE";
			if (IsFantasyPlayer())
			{
				if (Owner == "CC" || Owner == "BB")
					theColour = "YELLOW";
				else
					theColour = Owner == "**" ? "LIME" : takenColour;
			}
			return theColour;
		}

		public bool Contains(string subString, string mainString)
		{
			var nSpot = mainString.IndexOf(subString.Trim());
			return !(nSpot == -1);
		}

		public bool IsItalic()
		{
			bool bItalic = false;
			if (PlayerPos != null)
			{
				if (PlayerPos.IndexOf("FB") > -1)
					bItalic = true;
				else
				{
					if (PlayerPos.IndexOf("TE") > -1)
						bItalic = true;
					else if (PlayerPos.IndexOf("P") > -1)
						bItalic = true;
				}
			}
			return bItalic;
		}

		#endregion

		#region  Player Reports

		public void DumpHistory()
		{
			var pr = new PlayerReport(this);
			pr.Render();
		}

		public void PlayerReport([Optional] bool forceIt)
		{
			if (forceIt)
			{
				var pr = new PlayerReport(PlayerCode);
				pr.Render();
			}
			else
			{
				if (!IsPlayerReport())
				{
					var pr = new PlayerReport(PlayerCode);
					pr.Render();
				}
			}
		}

		public bool IsPlayerReport()
		{
			var reportFileName = string.Format(@"{1}\players\{0}.htm", PlayerCode, Utility.OutputDirectory());
			var exists = File.Exists(reportFileName);

			//if ( exists ) 
			//   RosterLib.Utility.Announce( string.Format( "A player report for {0} already exists", PlayerName ) );

			return exists;
		}

		#endregion

		public void PlayerProjection(string season)
		{
			var r = new PlayerProjection(PlayerCode, season);
			r.Render();
		}

		public string Url(string text, [Optional] bool forceReport)
		{
			var url = text;
			if (forceReport) PlayerReport(forceIt:true);
			if ( IsPlayerReport() )
			{
				var reportFileName = string.Format( @"{1}\players\{0}.htm", PlayerCode, Utility.OutputDirectory() );
				url = string.Format( "<a href =\"file:///{0}\">{1}</a>", reportFileName, text );
			}
			return url;
		}

		public string PlayerRow(bool addAvg)
		{
			//RosterLib.Utility.Announce( "NFLPlayer.PlayerRow() " );
			string nameOut = PlayerName;
			if (PlayerRole == "S") nameOut = HtmlLib.Bold(nameOut);
			if (IsItalic()) nameOut = HtmlLib.Italics(nameOut);
			if (Config.DoPlayerReports())
				if (! IsPlayerReport()) PlayerReport();

			string s = string.Format("{0}\n", HtmlLib.TableRowOpen(" BGCOLOR=" + SetColour("PINK")));
			if (_teamLastYear == null)
				_teamLastYear = Utility.TflWs.PlayedFor(PlayerCode, Int32.Parse(Utility.CurrentSeason()) - 1, 17);

			//  Name
			s += HtmlLib.TableData(NewPlayerFlag() + JerseyNo + " " + Url(nameOut)) + "\n";
			//  *
			s += HtmlLib.TableData(StarOut()) + "\n";
			//  Pos
			s += HtmlLib.TableData(PlayerPos + " - " + PlayerRole) + "\n";
			//  Age
			s += HtmlLib.TableData(Age(DBirth) + "&nbsp;" + Seasons()) + "\n";
			if (Config.ShowEp())
			{
				ExperiencePoints = Masters.Epm.GetEp(PlayerCode);
				s += HtmlLib.TableData(string.Format("{0}", Convert.ToInt32(ExperiencePoints))) + "\n";
			}
			//  Scores
			s += HtmlLib.TableData(string.Format("{0}-{1}", CurrScores, Scores)) + "\n";

			//  Performance stats
			LoadPerformances(Config.AllGames, false, WhichSeason());
			TallyPerformance(true, true, WhichSeason());
			if (TotStats != null)
			{
				//  Career Stats
				s += HtmlLib.TableData(TotStats.Stat1(PlayerCat, addAvg)) + "\n";
				s += HtmlLib.TableData(TotStats.Stat2(PlayerCat)) + "\n";
			}
			s += HtmlLib.TableData(Owner) + "\n";
			s += HtmlLib.TableRowClose() + "\n";
			return s;
		}

		private static string WhichSeason()
		{
			if (Int32.Parse(Utility.CurrentWeek()) < 2)
				return Utility.LastSeason();
			return Utility.CurrentSeason();
		}

		private string StarOut()
		{
			string star = HtmlLib.Spaces(1);
			if (IsStar()) star = HtmlLib.FixedImage(Utility.OutputDirectory(), "star.jpg");
			return star;
		}

		public string PlayerHeaderRow(string perfCol1, string perfCol2)
		{
			if (_myCat == null)
			{
				var cf = new CategoryFactory();
				_myCat = cf.CreatePos(PlayerCat, this);
			}

			var s = string.Format("{0}\n", HtmlLib.TableRowOpen());
			s += HtmlLib.TableHeader("Name") + "\n";
			s += HtmlLib.TableHeader("*") + "\n";
			s += HtmlLib.TableHeader("Pos") + "\n";
			s += HtmlLib.TableHeader("Age") + "\n";
			if (Config.ShowEp())
				s += HtmlLib.TableHeader("EP") + "\n";
			s += HtmlLib.TableHeader("Scores") + "\n";
			if ( _myCat.Category != null )
			{
				if (_myCat.Category.Trim().Length > 0)
				{
					s += HtmlLib.TableHeader(_myCat.PerfCol1()) + "\n";
					s += HtmlLib.TableHeader(_myCat.PerfCol2()) + "\n";
					s += HtmlLib.TableHeader("FT") + "\n";
					s += HtmlLib.TableRowClose() + "\n";
				}
			}
			return s;
		}

		public string CatRow(string cat)
		{
			string s = string.Format("{0}\n", HtmlLib.TableRowOpen(" BGCOLOR=WHITE"));
			s += HtmlLib.TableData(Utility.CategoryOut(cat), "WHITE", "COLSPAN='8' ALIGN='CENTER'");
			s += HtmlLib.TableRowClose() + "\n";
			return s;
		}

		public string PlayerOut()
		{
			return Url(JerseyNo + " " + PlayerName);
		}

		public string TeamRating(string season)
		{
			return Utility.TflWs.GetRatingsFor(TeamCode, season);
		}

		private string NewPlayerFlag()
		{
			if (_teamLastYear == CurrTeam.TeamCode)
				return "&nbsp;";
			return ">";
		}

		public string PlayerAge()
		{
			string ageOut = Age(DBirth);
			if (ageOut == "??")
				//  estimate
				ageOut = string.Format("{0}", 22 + NoOfSeasons());

			return ageOut;
		}

		private string Age(string dob)
		{
			var age = string.Empty;
			
			if (! string.IsNullOrEmpty(PlayerName) )
			{
				//RosterLib.Utility.Announce( string.Format( "{0} was born on {1}", PlayerName, dob ) );
				if ((dob == "30/12/1899") || (dob == null))
					age = string.Format("{0}?", NoOfSeasons() + 23);
				else
				{
					var ts = DateTime.Now - Convert.ToDateTime(dob);
					var nAge = (ts.Days/365);
					age = string.Format("{0}", nAge);
				}
			}
			return age;
		}

		/// <summary>
		/// Calculates the EP for the player based on the unit performance.
		/// </summary>
		public void CalculateEp(string season)
		{
			if (EpDone)
				Utility.Announce(string.Format(" EP for {0} calculated as {1}", PlayerName, ExperiencePoints));
			else
			{
#if DEBUG
				Utility.Announce(string.Format("Calculating EP for {0}", PlayerName));
#endif
				if (PerformanceList == null) LoadPerformances(Config.AllGames, false, season);
				ExperiencePoints = 0;
				if (PerformanceList != null)
					foreach (NflPerformance g in PerformanceList)
						if (g.Game != null)
						{
							var theGame = Masters.Gm.GetGame(g.Game.GameKey());
							if (theGame == null)
							{
								g.Game.TallyMetrics(String.Empty);
								Masters.Gm.AddGame(g.Game);
							}
							else
								g.Game = theGame;
							var ep = g.Game.ExperiencePoints(this, g.TeamCode);
#if DEBUG
							if ( ep > 0 )
								Utility.Announce(string.Format("  {2} got {0} EP for {1}", 
									ep, g.Game.GameName(), PlayerNameShort ) );
#endif
							ExperiencePoints += ep;
							//  add to the teams count too
							CurrTeam.ExperiencePoints += ep;
							//  record on the performance too
							g.ExperiencePoints = ep;
						}
				EpDone = true;
			}
		}

		#region Player Questions?

		public bool IsOffence()
		{
			if ((PlayerCat == "1") || (PlayerCat == "2") || (PlayerCat == "3") || (PlayerCat == "4") ||
			    (PlayerCat == "7"))
				return true;
			return false;
		}

		public bool IsFantasyOffence()
		{
			if ((PlayerCat == "1") || (PlayerCat == "2") || (PlayerCat == "3") || (PlayerCat == "4"))
				return true;
			return false;
		}

		public bool IsDefence()
		{
			if ((PlayerCat == RosterLib.Constants.K_LINEMAN_CAT) || (PlayerCat == RosterLib.Constants.K_DEFENSIVEBACK_CAT))
				return true;
			return false;
		}

		public bool IsKicker()
		{
			return PlayerCat == RosterLib.Constants.K_KICKER_CAT ? true : false;
		}

		public bool IsTe()
		{
			return (PlayerCat == RosterLib.Constants.K_RECEIVER_CAT) && Contains("TE", PlayerPos) ? true : false;
		}

		public bool IsReturner()
		{
			return Contains("PR", PlayerPos) || Contains("KR", PlayerPos) ? true : false;
		}

		public bool HasSpecRole( string specRole )
		{
			return Contains( specRole, PlayerPos );
		}

		public bool IsRusher()
		{
			bool isRusher = false;
			string[] pos = PlayerPos.Trim().Split(',');
			for (int i = 0; i < pos.Length; i++)
			{
				if (Contains(pos[i], K_RUSHING_POSITIONS))
				{
					isRusher = true;
					break;
				}
			}
			return isRusher;
		}

		public bool IsInUnit(string unitCode)
		{
			return (Unit() == unitCode);
		}

		public bool IsAtHome(NFLWeek week)
		{
			bool atHome = false;

			NFLGame game = week.GameFor(TeamCode);

			if (game == null)
				RosterLib.Utility.Announce(string.Format("  no game found for {0} in week {1}", TeamCode, week.WeekKey()));
			else
				atHome = game.HomeTeam.Equals(TeamCode);

			//if ( atHome ) 
			//   RosterLib.Utility.Announce( string.Format( "  {0} is at home vs {1}", PlayerNameShort, game.AwayNflTeam.NameOut() ) );

			return atHome;
		}

		#endregion

		/// <summary>
		/// Determines the value of the player depending on a few variables.
		/// Used by the FA Market Analysis.
		/// D1 * 3, D2 * 2
		/// </summary>
		/// <returns>integer no of points</returns>
		public int Value()
		{
			var pts = 1;
			if ((PlayerCat == null) || (IsRetired))
				pts = 0;
			else
			{
				if (IsStarter())
					pts = 4;
				else if (IsBackup())
					pts = 2;

				// modify for category
				if (PlayerCat == "1") pts *= 4;
				if (PlayerPos.Trim().Equals("P")) pts /= 2;
				if (PlayerCat == "2") pts *= 3;
				if (PlayerCat == "3") pts *= 2;
				if (PlayerCat == "4") pts *= 1;
				if (PlayerCat == "5") pts *= 3;
				if (PlayerCat == "6") pts *= 3;
				if (PlayerCat == "7") pts *= 3;

				//  bonus for being mature
				if (IsInPrime()) pts *= 2;

				//  Rookie values
				if (IsRookie())
				{
					//  What round?
					SetDraftRound();

					switch (Drafted)
					{
						case "D1":
							pts *= 3;
							break;
						case "D2":
							pts *= 2;
							break;
						default:
							break;
					}
				}
			}

			return pts;
		}

		public void SetDraftRound()
		{
			if (Drafted == null)
				Drafted = Utility.TflWs.Drafted(PlayerCode).Substring(0, 2);
			else
			{
				if (Drafted.Length == 0)
					Drafted = Utility.TflWs.Drafted(PlayerCode).Substring(0, 2);
			}
		}

		public string PlayerHeader()
		{
			return string.Format("{0}{1}{2}\n", HtmlLib.TableOpen("cellpadding='0' cellspacing='0'"), PlayerRow(false),
			                     HtmlLib.TableClose());
		}

		public string PlayerDiv()
		{
			string s = "";
			if (Config.ShowEp()) return s; //  dont show games AND EP

			int pCount = 1;
			s = HtmlLib.DivOpen("class='he5i'") + "\n"
			    + HtmlLib.TableOpen("class='info' cellpadding='0' cellspacing='0' border='0'")
			    + "\n";
			if (Config.ShowPerformance)
			{
				if (PerformanceList == null) LoadPerformances(Config.AllGames, true, Utility.CurrentSeason());
				if (PerformanceList != null)
				{
					foreach (NflPerformance p in PerformanceList)
					{
						if (pCount == 1)
						{
							s += p.PerfHeaders();
							pCount++;
						}
						s += p.PerfRow();
					}
				}
			}
			s += HtmlLib.TableClose() + "\n" + HtmlLib.DivClose();
			return s;
		}

		public void ProjectNextWeek()
		{
			if (IsStarter())
			{
				if (_myCat == null)
				{
					if (! string.IsNullOrEmpty(PlayerCat))
					{
						CategoryFactory cf = new CategoryFactory();
						_myCat = cf.CreatePos(PlayerCat, this);
					}
				}
				if ((_myCat != null) && _myCat.Category.Trim().Length > 1)
					_myCat.ProjectNextWeek(this);
			}
		}

		/// <summary>
		///    Stores a projected metric on the players record.
		/// </summary>
		/// <param name="nProjected"></param>
		public void StoreProjection(int nProjected)
		{
			DataLibrarian.StoreProjection(nProjected, PlayerCode);
		}

		public void Save()
		{
			if (Utility.TflWs.PlayerExists(FirstName, Surname, College))
				Utility.Announce(
					string.Format("No Save: Player {0}, {1}, out of {2} exists", FirstName, Surname, College));
			else
			{
				string nextId = Utility.TflWs.NextId(FirstName, Surname);

				if (!string.IsNullOrEmpty(nextId))
				{
					PlayerPos = ValidatePosDesc(PlayerPos);
					PlayerCat = Utility.CategoryFor(PlayerPos);

					Utility.TflWs.StorePlayer(nextId, FirstName, Surname, TeamLastYear, Constants.K_ROLE_STARTER,
					                          HeightFeet, HeightInches,
					                          Weight, College, RookieYear, PlayerPos, PlayerCat, DBirth);
				}
			}
		}

		public string ValidatePosDesc(string candidatePos)
		{
			string pos = candidatePos.Trim();
			string testPos = pos + ",";
			string validPositions =
				"C,CD,S,DB,DE,DL,NT,DT,LB,P,WR,FB,HB,K,FS,PK,RB,G,OT,T,HB,ILB,OLB,LDE,RDE,LDT,RDT,LE,LG,LILB,RILB,LOLB,LT,MLB,MIKE,MILB,OG,OL,QB,RCB,LCB,RDT,RE,RG,RILB,RLB,ROLB,S,SS,TE,WILB,WILL,WLB,WR,";

			if (validPositions.IndexOf(pos) < 0)
				pos = "  ";
			return pos;
		}

		public decimal PointsForWeek(NFLWeek week, IRatePlayers rater)
		{
			return rater.RatePlayer(this, week);
		}

		public string Injuries()
		{
			int nInjuries = 0;
			if (! string.IsNullOrEmpty(Injury))
			{
				nInjuries = Int32.Parse(Injury);
				if (IsInjured())
					nInjuries++;
			}
			return nInjuries.ToString();
		}

		public string Injury { get; set; }

		public string GameFor(string season, int week)
		{
			return "v TT Mon";
		}

		public string ProjectionLink( string season )
		{
			var url = string.Format( "<a href =\"file:///{3}{1}//PlayerProjections/{0}.htm\">{2}</a>", 
				PlayerCode, season, PlayerName, Utility.OutputDirectory() );
			return url;
		}

		public string StatsFor( NFLGame g, string teamInFocus )
		{
			var stats = string.Empty;

			if ( PlayerRole == Constants.K_ROLE_STARTER )
			{
				switch ( PlayerCat )
				{
					case Constants.K_QUARTERBACK_CAT:
						if ( g.IsHome( teamInFocus ) )
							stats = string.Format( "{0}({1})", g.ProjectedHomeYdp, g.ProjectedHomeTdp );
						else
							stats = string.Format( "{0}({1})", g.ProjectedAwayYdp, g.ProjectedAwayTdp );

						break;
					case Constants.K_RUNNINGBACK_CAT:
						//TODO:
						if ( g.IsHome( teamInFocus ) )
							stats = string.Format( "{0}({1})", g.ProjectedHomeYdp, g.ProjectedHomeTdp );
						else
							stats = string.Format( "{0}({1})", g.ProjectedAwayYdp, g.ProjectedAwayTdp );
						break;
					case Constants.K_RECEIVER_CAT:
						//TODO:
						if ( g.IsHome( teamInFocus ) )
							stats = string.Format( "{0}({1})", g.ProjectedHomeYdp, g.ProjectedHomeTdp );
						else
							stats = string.Format( "{0}({1})", g.ProjectedAwayYdp, g.ProjectedAwayTdp );
						break;
					case Constants.K_KICKER_CAT:
						//TODO:
						if ( g.IsHome( teamInFocus ) )
							stats = string.Format( "{0}({1})", g.ProjectedHomeYdp, g.ProjectedHomeTdp );
						else
							stats = string.Format( "{0}({1})", g.ProjectedAwayYdp, g.ProjectedAwayTdp );
						break;
				}
			}
			return stats;
		}

		public bool IsShortYardageBack()
		{
			var npoint = PlayerPos.IndexOf( "SH" );
			return ( npoint > -1 );
		}

		internal bool IsFullback()
		{
			var npoint = PlayerPos.IndexOf( "FB" );
			return ( npoint > -1 );
		}

		internal bool IsQuarterback()
		{
			var npoint = PlayerPos.IndexOf( "QB" );
			return ( npoint > -1 );
		}

		internal bool HasPos( string pos )
		{
			var npoint = PlayerPos.IndexOf( pos );
			return ( npoint > -1 );
		}

		internal void LoadProjections( PlayerGameMetrics pgm )
		{
			ProjectedFg = pgm.ProjFG;
			ProjectedPat = pgm.ProjPat;
			ProjectedTDc = pgm.ProjTDc;
			ProjectedTDp = pgm.ProjTDp;
			ProjectedTDr = pgm.ProjTDr;
			ProjectedYDc = pgm.ProjYDc;
			ProjectedYDp = pgm.ProjYDp;
			ProjectedYDr = pgm.ProjYDr;
		}

		public int Compare(object x, object y)
		{
			var player1 = (NFLPlayer) x;
			var player2 = (NFLPlayer) y;
			if (player1.TotStats.Rushes > player2.TotStats.Rushes)
				return 1;
			else
				return 0;
		}

		public int CompareTo(object obj)
		{
			var player2 = (NFLPlayer) obj;
			if ( this.TotStats.Rushes > player2.TotStats.Rushes)
				return 1;
			else
				return 0;
		}

		internal bool IsWideout()
		{
			return PlayerPos.Contains("WR");
		}

		internal bool IsTightEnd()
		{
			return PlayerPos.Contains("TE");
		}
	}
}