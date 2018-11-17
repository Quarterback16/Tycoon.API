using System;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using log4net;
using log4net.Config;
using TFLLib;
using RosterLib;

[assembly: XmlConfigurator(Watch = true)]

namespace RosterGrid
{
	/// <summary>
	/// Summary description for Roster Grid
	/// Uses the TFL database to generate a Grid sheet for a particular 
	/// position, a big Roster web page and Team Cards for each team.
	/// Compile and test in Visual Studio, deploy to c:\public
	///    There are 3 files to deploy the exe, the config and the tflLib.DLL
	/// </summary>
	public class RosterGrid
	{
		private static ElapsedTimer _et;

		#region  Statics

		public static TextWriterTraceListener Twtl;
		public static TraceSwitch Ts;

		public static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		#endregion

		#region  Accessors

		public string DataDrive { get; set; }

		#endregion

		#region  PROCEDURE DIVISION

		/// <summary>
		/// The main entry point for the application.   START HERE.
		/// Options are set in the Options region.
		/// </summary>
		[STAThread]
		public static void Main()
		{
			try
			{
				var dataDrive = Utility.PrimaryDrive();

                //if ( Utility.HostName() == Constants.K_WORK_MACHINE )
                //    dataDrive = Constants.K_WORK_DRIVE;

//				Utility.TflWs = new DataLibrarian(dataDrive);
				Utility.TflWs = new DataLibrarian(
					 Utility.NflConnectionString(),
					 Utility.TflConnectionString(),
					 Utility.CtlConnectionString()
					 );

				Utility.Announce( string.Format( "RosterGrid ver. 3.05 - 4-SEP-13 .NET {0}", Utility.DotNetVersion() ) );
				Utility.Announce( string.Format( "Using Drive {0}", dataDrive ) );
				Utility.Announce( string.Format( "Output directory:{0}", Utility.OutputDirectory()) );
				Utility.Announce( string.Format( "NFL Connection String:{0}", Utility.NflConnectionString() ) );
				Utility.Announce( string.Format( "TFL Connection String:{0}", Utility.TflConnectionString() ) );
				Utility.Announce( string.Format( "CTL Connection String:{0}", Utility.CtlConnectionString() ) );

				#region  trace file

				Utility.WriteLog("Roster Grid begins");
				Utility.WriteLog("Data source is " + Utility.TflWs.ConStr);

				#endregion

				_et = new ElapsedTimer();
				_et.Start(DateTime.Now);

				DisplayReportSettings();

				#region  Roster stuff

				//  Pre-req for performance and stats reports
				ExecuteStep( GenerateYahooXml, Config.GenerateYahooXml );
				ExecuteStep( GenerateStatsXml, Config.GenerateStatsXml );
				ExecuteStep( AllStats, Config.GenerateStatsXml );

				Utility.LoadUnits();

				ExecuteStep( OldRosterReport, Config.OldFormat);

				ExecuteStep( Utility.ExperiencePoints, Config.DoExperience);

				ExecuteStep (NewRosterReport, Config.NewFormat);

				ExecuteStep( OffensiveLineReport, Config.DoOffensiveLine);
				ExecuteStep( Starters, Config.DoStarters);

				ExecuteStep( Returners, Config.DoReturners);

				ExecuteStep( GenerateDepthCharts, Config.DoDepthCharts );

				#endregion

				#region  GridStats specific stuff

				ExecuteStep(GridStatsReport, Config.DoGridStatsReport);
				ExecuteStep(GridStatsRankings, Config.DoGs4WrRanks);
				ExecuteStep(HotLists, Config.DoHotLists);

				#endregion

				ExecuteStep(SuggestedLineups, Config.DoSuggestedLineups);
				ExecuteStep(ProjectedLineups, Config.DoProjectedLineups);

				#region  ESPN stuff

				ExecuteStep(EspnReport, Config.DoEspn);

				#endregion

				#region  Rankings

				ExecuteStep(Rankings, Config.DoRankings);
				ExecuteStep(VictoryPoints, Config.DoVictoryPoints);
				ExecuteStep(NflukRatings, Config.DoNflukRatings);

				#endregion

				#region  Reports

				ExecuteStep(CurrentScorers, Config.DoCurrentScorers);
				ExecuteStep(LineupCards, Config.DoLineupCards);
				ExecuteStep(FreeAgentReport, Config.DoFreeAgents);
				ExecuteStep(RunUnitReports, Config.DoUnitReports);
				ExecuteStep(RunUnitsByWeek, Config.DoUnitsByWeek);
				ExecuteStep(StarRatings, Config.DoStarRatings);
				ExecuteStep(GridStatsPerformance, Config.DoGsPerformance);
				ExecuteStep( EspnPerformance, Config.DoEspnPerformance);
				ExecuteStep( YahooProjections, Config.DoYahooProjections);
				ExecuteStep( FpProjections, Config.DoFpProjections );
				ExecuteStep(BalanceReport, Config.DoBalanceReport);
				ExecuteStep(DefensiveScoring, Config.DoDefensiveScoring);
				ExecuteStep(NextWeeksMatchups, Config.DoMatchups);

				#endregion

				#region  Best Bets

				if (Config.DoPlays())
				{
					BestBets(Utility.CurrentSeason(), Int32.Parse(Utility.CurrentWeek()));
					BestBets(Utility.CurrentSeason(), Int32.Parse(Utility.CurrentWeek()) + 1);
				}
				if (Config.DoBackTest())
				{
					var kenny = new NFLGambler(7.50D);
					kenny.BackTestSchemes();
				}

				#endregion

				#region  Pre-season stuff

				ExecuteStep( FaMarket, Config.DoFaMarket);

				ExecuteStep( StrengthOfSched, Config.DoStrengthOfSchedule);

				//SaveAppointments(); //  do once a year done last 13.5.2010, 27.4.2009, 14.7.2008, 
				//                           28.5.2007, 24.5.2006, 10.8.2005

				//  In preseason - generate the initial number ratings
				//DanGordan dan = new DanGordan();
				//dan.GenerateNumberRatings("2006");

				#endregion

				#region  Post Season stuff

				ExecuteStep(FrequencyTables, Config.DoFrequencyTables);

				#endregion

				ExecuteStep( StatsGrids, Config.DoStatsGrids );

				#region  Test Area

				#region  Miller tipping

				//var mt = new MillerTips();
				//mt.TipSeason( "2010" );

				#endregion

				#region  Hillen tipping

				ExecuteStep( HillenTipping, Config.DoHillenTips );

				#endregion

				#region  Nibble Rankings

				//var nr = new NibbleRanker( "2010" );
				//nr.RefreshRatings( 1 );

				#endregion

				#region Dan Gordon Testing

				//GordanRanks gr = new GordanRanks("2006");

				//DanGordan dan = new DanGordan();
				//dan.ProjectLine("2006", "01");
				//dan.ProjectLine("2006", "02");
				//dan.ProjectLine("2006", "03");
				//dan.ProjectLine("2006", "04");
				//dan.ProjectLine("2006", "05");
				//dan.ProjectLine("2006", "06");
				//dan.ProjectLine("2006", "07");
				//dan.ProjectLine("2006", "08");
				//dan.ProjectLine("2006", "10");
				//dan.ProjectLine("2006", "11");
				//dan.ProjectLine("2006", "12");
				//dan.ProjectLine("2006", "13");            
				//dan.ProjectLine("2006", "14");
				//dan.ProjectLine("2006", "15");
				//dan.ProjectLine("2006", "16");
				//dan.ProjectLine("2006", "17");
				//dan.ProjectLine("2006", "18"); 

				//TippingReport tr = new TippingReport();
				//tr.Render();

				#endregion

				//  refresh scoring totals for a game
				//var testGame = new NFLGame("2009:01-A");
				//testGame.RefreshTotals();

				//  Game projection
				//var testGame = new NFLGame("2010:01-A");
				//testGame.WriteProjection();

				#region Defensive reports testing

				//  Best scoring defenses Yahoo scoring
				//TeamLister tl = new TeamLister();
				//tl.Heading = string.Format( "defense\\Defensive Scoring {0}-{1:0#}", CurrentSeason(), Int32.Parse( CurrentWeek() ) );
				//ICalculate myCalculator = new DefensiveScoringCalculator( new NFLWeek( 2010, 5 ), -5	);
				//tl.RenderTeams( myCalculator );

				//  Team to beat report showing a jucy oppenet for a particular defence
				//TeamLister ttb = new TeamLister();
				//ttb.Heading = string.Format( "defense\\Team To beat {0}-{1:0#}", CurrentSeason(), Int32.Parse( CurrentWeek() ) );
				//ICalculate myCalculator = new DefensiveScoringCalculator( new NFLWeek( 2010, 5 ), -5	);
				//ttb.RenderTeamToBeat( myCalculator );

				#endregion

				#region  Suggested Lineup testing

				//SuggestedLineup r = new SuggestedLineup(leagueId: K_LEAGUE_Yahoo,
				//                                      ownerCode: K_OWNER_SteveColonna,
				//                                      teamCode: "BB",  // team code for the owner
				//                                      season: Utility.CurrentSeason(), week: 16);
				//r.IncludeFreeAgents = true;
				//r.IncludeSpread = true;
				//r.IncludeAverage = true;
				//r.IncludeRatingModifier = true;
				//r.Render();

				// Grid stats oppponent 
				//SuggestedLineup r1 = new SuggestedLineup(K_LEAGUE_Gridstats_NFL1,
				//                                         "B015", "PP",
				//                                         Utility.CurrentSeason(), 12);
				//r1.IncludeSpread = true;
				//r1.IncludeRatingModifier = true;
				//r1.Render();

				//  suggest the gridstats lineup
				//SuggestedLineup gr = new SuggestedLineup(K_LEAGUE_Gridstats_NFL1,
				//                                         K_OWNER_SteveColonna,
				//                                         "CC",  // team code for the owner
				//                                        Utility.CurrentSeason(), 16);
				//gr.IncludeSpread = true;
				//gr.IncludeRatingModifier = true;
				//gr.Render();

				////  suggest the projected gridstats lineup
				//ProjectedLineup r2 = new ProjectedLineup( K_LEAGUE_Gridstats_NFL1, 
				//                                      K_OWNER_SteveColonna, 
				//                                      Utility.CurrentSeason(), 6 );

				//r2.Render();

				#endregion

				#region   Gridstats performances

				//GenericGridStatsPerformance( K_RECEIVER_CAT, "TE" );

				#endregion

				#region  ESPN/Yahoo performances

				////////////////////////////////////////////////////////////////
				//  best ESPN/Yahoo performances in a particular week and season
				////////////////////////////////////////////////////////////////
				//BatchEspnPerformance( K_QUARTERBACK_CAT, 7, "QB" );
				//BatchEspnPerformance( K_RECEIVER_CAT, 7, "WR"  );
				//BatchEspnPerformance( K_RECEIVER_CAT, 7, "TE"  );
				//BatchEspnPerformance( K_RUNNINGBACK_CAT, 7, "RB"  );
				//BatchEspnPerformance( K_KICKER_CAT, 13, "PK"  );
				/////////////////////////////////////////////////////////////////

				#endregion

				#region  ESPN/Yahoo projections

				////////////////////////////////////////////////////////////////
				//  ESPN/Yahoo projections for a particular week and season
				////////////////////////////////////////////////////////////////
				//ProjectYahooPerformance( K_QUARTERBACK_CAT, 15, "QB" );
				//ProjectYahooPerformance( K_RECEIVER_CAT, 15, "WR"  );
				//ProjectYahooPerformance( K_RECEIVER_CAT, 15, "TE"  );
				//ProjectYahooPerformance( K_RUNNINGBACK_CAT, 15, "RB"  );
				//ProjectYahooPerformance( K_KICKER_CAT, 15, "PK"  );
				//ProjectYahooPerformance( K_QUARTERBACK_CAT, 16, "QB" );
				//ProjectYahooPerformance( K_RECEIVER_CAT, 16, "WR"  );
				//ProjectYahooPerformance( K_RECEIVER_CAT, 16, "TE"  );
				//ProjectYahooPerformance( K_RUNNINGBACK_CAT, 16, "RB"  );
				//ProjectYahooPerformance( K_KICKER_CAT, 16, "PK"  );
				/////////////////////////////////////////////////////////////////

				#endregion

				#region  Player Report Testing

				//var p = new NFLPlayer( "MCCLDE02" );
				//p.PlayerReport( forceIt : true );

				#endregion

				#region Unit Report Testing

				//RunUnitReports();

				//var team = new NflTeam("SL");
				//team.DumpLineup("2010", "16");
				//team.Lineup("2010", "16").DumpKeyPlayers();

				//var s = new NflSeason( "2010", false, false );
				//s.TestLineups( "17" );
				//var s = new NflSeason("2011");
				//var teamCount = s.NumberOfTeams();

				#endregion

				#region  Best Bets testing

				//BestBets( "2010", 7 );

				#endregion

				#region  Return List

				//ReturnersReport( K_LEAGUE_Gridstats_NFL1, "2010", 9 );
				//ReturnersReport( K_LEAGUE_Gridstats_NFL1, "2009", 21 );

				#endregion

				#region  Frequency Tables

				//var po = new PlayerOutput();
				//po.PlayerType = "QBs Scores - PManning";
				//po.SinglePlayer = "MANNPE01";
				//po.ScoreType = K_SCORE_TD_PASS;
				//po.ScoreSlot = "2";
				//po.wRange.startWeek = new NFLWeek( 2010, 01 );
				//po.wRange.endWeek = new NFLWeek( 2010, 11 );
				//po.Scorer = new GS4Scorer( CurrentNFLWeek() );
				//po.Scorer.ScoresOnly = true;
				//po.Load();
				//po.Render( po.PlayerType );

				//FrequencyTables();  //  all of them

				#endregion

				#region Score Counts

				//TODO:  Why no Punt returns???
				//ScoreCounts( "2010" );

				#endregion

				#region Data Integrity

				//var checker = new DataIntegrityChecker();
				//checker.Season = "2010";
				//checker.Week = 17;
				//checker.CheckStats();
				//checker.CheckScores();

				#endregion

				#region  Charting

				//var chart = new BarChart();
				//chart.Title = "2007 Sales";
				//chart.Plot();

				#endregion

				#region  NextId

				//string nextId = Utility.TflWs.NextId("Steve", "Colonna");

				#endregion

				#region  Cleanup

//#if ! DEBUG
				if (Masters.Sm != null)
				{
					Utility.Announce(Masters.Sm.StatsMessage());
					Masters.Sm.Dump2Xml();
				}

				if (Masters.Tm != null)
				{
					Utility.Announce(Masters.Tm.StatsMessage());
					Masters.Tm.Dump2Xml();
				}
				if (Masters.Gm != null)
				{
					Utility.Announce(Masters.Gm.StatsMessage());
					Masters.Gm.HomeAwayRatio();
					Masters.Gm.Dump2Xml();
				}
				if (Masters.Epm != null)
				{
					Utility.Announce(Masters.Epm.StatsMessage());
				}

				if (Masters.Pm != null)
				{
					Utility.Announce(Masters.Pm.StatsMessage());
					//Pm.InjuryRatio();
					//Pm.Dump2Xml();
				}
//#endif

				_et.Stop(DateTime.Now);
				Utility.Announce("-----------------------------------------------------");
				Utility.Announce("Process completed OK - Elapsed time: " + _et.TimeOut());
				Utility.Announce("-----------------------------------------------------");

				//SystemSounds.Asterisk.Play();

#if DEBUG
				Console.ReadLine();
#endif
				if (Twtl != null) Twtl.Flush();

				#endregion
			}

#if DEBUG
			finally
			{
				if (Twtl != null) Twtl.Flush();
			}
#else
			catch (ArgumentOutOfRangeException ex)
			{
				Utility.WriteLog(ex.Message);
					 Utility.WriteLog(GetStackTrace());
					 Utility.WriteLog(GetStackTraceWithMethods());
				if ( Twtl != null) Twtl.Flush();
			}
#endif
		}

		private static void GenerateDepthCharts()
		{
			var currentSeason = Utility.CurrentSeason();
			var season = new NflSeason( currentSeason );
			foreach ( var team in season.TeamList )
			{
				var dcr = new DepthChartReport( currentSeason, team.TeamCode );
				dcr.Execute();
			}

		}

		private static void HillenTipping()
		{
			var m = new HillenMaster( "Hillen", "Hillen.xml" );
			m.Calculate( Utility.CurrentSeason(), Utility.CurrentWeek() );
			m.Dump2Xml();
			var r = new HillenTips( Utility.CurrentSeason(), Utility.CurrentWeek() );
			r.Render();
			var r2 = new HillenTips( Utility.CurrentSeason(), Utility.PreviousWeek().ToString() );
			r2.Render();			
		}

		private static void AllStats()
		{
			var season = Utility.CurrentSeason();
			StatGridFor( season, "Sacks" );
			StatGridFor( season, "YDr" );
			StatGridFor( season, "YDp" );
			StatGridFor( season, "TDp" );
			StatGridFor( season, "TDr" );
			StatGridFor( season, "TDrAllowed" );
			StatGridFor( season, "Sacks" );
			StatGridFor( season, "SacksAllowed" );
			StatGridFor( season, "YDrAllowed" );
			StatGridFor( season, "YDpAllowed" );
			StatGridFor( season, "TDpAllowed" );
			StatGridFor( season, "INTs" );
			StatGridFor( season, "INTsThrown" );
		}

		private static void StatGridFor( string season, string statType )
		{
			var sg = new StatGrid( season, statType );
			sg.Render();
		}

		private static void GenerateStatsXml()
		{
			var sm = new StatMaster( "Stats", "stats.xml" );
			sm.Calculate( Utility.CurrentSeason(), Utility.CurrentWeek() );
			sm.Dump2Xml();
		}

		private static void GenerateYahooXml()
		{
			var m = new YahooMaster( "Yahoo", "YahooOutput.xml" );
			m.Calculate( Utility.CurrentSeason(), Utility.CurrentWeek() );
			m.Dump2Xml();
		}

		#endregion

		private static void ScoreCounts(string season)
		{
			var sc = new ScoreCount(season);
			sc.RenderToHtml();
		}

		private static void FrequencyTables()
		{
			SeasonFreqTableGs4("Kickers", Constants.K_SCORE_FIELD_GOAL, "1", scoresOnly: false);
			SeasonFreqTableGs4("QBs Scores", Constants.K_SCORE_TD_PASS, "2", scoresOnly: true);
			SeasonFreqTableGs4("RBs Scores", Constants.K_SCORE_TD_RUN, "1", scoresOnly: true);
			SeasonFreqTableGs4("WRs Scores", Constants.K_SCORE_TD_PASS, "1", scoresOnly: true);
		}

		private static void SeasonFreqTableGs4(string playerType, string scoreType, string scoreSlot,
		                                       [System.Runtime.InteropServices.Optional] bool scoresOnly)
		{
			var po = new PlayerOutput
			         	{
			         		PlayerType = playerType,
			         		ScoreType = scoreType,
			         		ScoreSlot = scoreSlot,
			         		wRange = {startWeek = new NFLWeek(2010, 1), endWeek = Utility.CurrentNFLWeek()},
			         		Scorer = new GS4Scorer(Utility.CurrentNFLWeek()) {ScoresOnly = scoresOnly}
			         	};
			po.Load();
			po.Render(po.PlayerType, po.wRange.startWeek.Season );
		}

		private static void Returners()
		{
			ReturnersReport(Constants.K_LEAGUE_Gridstats_NFL1, Utility.CurrentSeason(), Int32.Parse(Utility.CurrentWeek()));
		}

		private static void ReturnersReport(string fantasyLeague, string season, int week)
		{
#if DEBUG
			Utility.Announce("Loading Returners...");
#endif
			var pl = new PlayerLister()
			         	{
			         		SortOrder = week > 0 ? "POINTS DESC" : "CURSCORES DESC",
			         		Season = season
			         	};

			var scorer = new ReturnerScorer(new NFLWeek(Int32.Parse(season), week));
			pl.SetScorer(scorer);
			pl.Week = week;
			pl.WeeksToGoBack = week;
			pl.Collect( "*", "KR", fantasyLeague );
			pl.RenderReturners( season );
		}

		public static void WeeklyEspnPerformance(
			string catCode, YahooMaster master, int week, string leagueId, [System.Runtime.InteropServices.Optional] string sPos)
		{
			var pl = new PlayerLister {WeeksToGoBack = 1};
			//  dont want too much data
			var currentWeek =
				new NFLWeek(Int32.Parse(Utility.CurrentSeason()), weekIn: week, loadGames: false);
			var gs = new EspnScorer( currentWeek ) {Master = master};

			pl.SetScorer(gs);
			pl.SetFormat("annual");
			pl.AllWeeks = false; //  just the regular saeason
			pl.Season = currentWeek.Season;
			pl.RenderToCsv = false;
			pl.Week = week;
			pl.Collect( catCode, sPos: sPos, fantasyLeague: leagueId );

			var targetFile = string.Format( "{2}//Performance//{3}-Yahoo {1} Performance upto Week {0}", currentWeek.WeekNo, sPos, Utility.CurrentSeason() );
			pl.Render(targetFile);
		}

		private static void SuggestedLineups()
		{
			var r = new SuggestedLineup(Constants.K_LEAGUE_Yahoo,
												 Constants.KOwnerSteveColonna, "BB",
			                            Utility.CurrentSeason(),
			                            Int32.Parse(Utility.CurrentWeek()))
			        	{IncludeFreeAgents = true, IncludeRatingModifier = true, IncludeSpread = true };
			r.Render();

			var r2 = new SuggestedLineup(Constants.K_LEAGUE_Gridstats_NFL1,
			                             Constants.KOwnerSteveColonna, "CC",
			                             Utility.CurrentSeason(),
			                             Int32.Parse(Utility.CurrentWeek())) {IncludeSpread = true, IncludeRatingModifier = true};
			r2.Render();
		}

		private static void ProjectedLineups()
		{
			var r = new ProjectedLineup(Constants.K_LEAGUE_Yahoo,
																 Constants.KOwnerSteveColonna,
			                                        Utility.CurrentSeason(), Utility.UpcomingWeek());
			r.Render();

			var r2 = new ProjectedLineup(Constants.K_LEAGUE_Gridstats_NFL1,
																  Constants.KOwnerSteveColonna,
			                                         Utility.CurrentSeason(), Utility.UpcomingWeek());
			r2.Render();
		}

		private static void OldRosterReport()
		{
			Config.ReportType = "Old";

			if ( Utility.CurrWeek > 0 )
			{
				Utility.CurrentLeague = Constants.K_LEAGUE_Yahoo;
				OldRosterGrid();
			}

			Utility.CurrentLeague = Constants.K_LEAGUE_Gridstats_NFL1;
			OldRosterGrid();
		}

		private static void NewRosterReport()
		{
			//  This is the new all singing all dancing Report
			//  Set up the reporting environment

			var rr = new NFLRosterReport( Utility.CurrentSeason() );
			Config.ReportType = "New";

			if (Config.DoRoster())
			{
				rr.CurrentRoster(); //  just opens a HTML file
				rr.TimeTaken = _et.TimeOut();
				rr.Render(); //  bulk of the work is here in TeamListOut() which is ultimately team.TeamDivContents  
				_et.Stop(DateTime.Now);
				//rr.XmlOutput();  //  this takes too long as it gets all games for all players  
				//rr.RosterExperience();
				//Utility.Announce("Roster Grid Report - finished");
			}
			else Utility.Announce("skipping Roster");

			if (Config.DoKickers()) rr.KickerProjection();
			if (Config.DoProjections())
			{
				//if (CurrentWeek() == "00")
				//{
				//   rr.SeasonProjection("Tdp");
				//   rr.SeasonProjection("Tdr"); //  Generate Season Tdr projection
				//   rr.ProjectionList.Add("Fg"); //  this will cause the Fg rankings to be produced
				//   rr.DumpProjections();
				//}
				rr.SeasonProjection( "Spread", Utility.CurrentSeason(), Utility.CurrentWeek(), DateTime.Now );
			}
			if (Config.DoTeamcards()) rr.TeamCards(); //  spit out the Team cards too 

			if (Config.DoPlayerReports()) rr.PlayerReports(); //  spit out a file for every player

			if (Config.DoPlayerCsv()) rr.DumpPlayersToCsv(); //  writes a file that can be imported into ListPro or Excel
		}

		private static void OffensiveLineReport()
		{
			var olr = new OffLineReport("2008");
			olr.RenderAsHTML();
		}

		#endregion

		private static void ExecuteStep(Action stepMethod, Func<bool> goNogoMethod)
		{
			Utility.ExecuteStep( stepMethod, goNogoMethod );
		}

		#region  Rankings

		public static void CurrentScorers()
		{
			Utility.Announce("Current Scorers ...");
			var suffix = "_Wk" + Utility.CurrentWeek();
			ScoreReport("Scorers QB" + suffix, "1");
			ScoreReport("Scorers RB" + suffix, "2");
			ScoreReport("Scorers PR" + suffix, "3");
			ScoreReport("Scorers PK" + suffix, "4");
		}

		private static void ScoreReport(string header, string category)
		{
			//Utility.Announce( "ScoreReport " + header );
			var r = new SimpleTableReport {ReportHeader = header, DoRowNumbers = true};
			var ds = Utility.TflWs.GetCurrentScoring(category);
			//  AddFteams
			ds = AddFteams(ds);
			r.AddColumn(new ReportColumn("Given", "FIRSTNAME", "{0,-10}"));
			r.AddColumn(new ReportColumn("Surname", "SURNAME", "{0,-15}"));
			r.AddColumn(new ReportColumn("RookieYr", "ROOKIEYR", "{0,4}"));
			r.AddColumn(new ReportColumn("Team", "CURRTEAM", "{0,2}"));
			r.AddColumn(new ReportColumn("Scores", "CURSCORES", "{0,5}"));
			r.AddColumn(new ReportColumn("GS4", "COLLEGE", "{0,-2}"));
			r.LoadBody(ds.Tables[0]);
			r.RenderAsHtml(Utility.OutputDirectory() + header + ".htm", true);
		}

		private static DataSet AddFteams(DataSet ds)
		{
			for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
			{
				var playerId = ds.Tables[0].Rows[i]["PLAYERID"].ToString();
				var teamCode = Utility.TflWs.GetStatus(playerId, "GS", Utility.CurrentSeason());
				ds.Tables[0].Rows[i]["COLLEGE"] = teamCode; //  Not reporting on College
			}
			return ds;
		}

		public static void BalanceReport()
		{
			Utility.Announce("BalanceReport...");
			var br = new BalanceReport(Utility.LastSeason());
			br.Render();
		}

		public static void GridStatsPerformance()
		{
			Utility.Announce("GridStatsPerformance...");
			var master = new GridStatsMaster( "GridStats", "GridStatsOutput.xml" );
			GenericGridStatsPerformance( Constants.K_QUARTERBACK_CAT, "QB", master );
			GenericGridStatsPerformance( Constants.K_RUNNINGBACK_CAT, "RB", master );
			GenericGridStatsPerformance( Constants.K_RECEIVER_CAT, "WR", master );
			GenericGridStatsPerformance( Constants.K_RECEIVER_CAT, "TE", master );
			GenericGridStatsPerformance( Constants.K_KICKER_CAT, "Kicker", master );
		}

		public static void GenericGridStatsPerformance(string catCode, string sPos, GridStatsMaster master )
		{
			var pr = new PerformanceReport(Utility.CurrentSeason(), Int32.Parse(Utility.CurrentWeek()))
			         	{PlayerLister = {OnesAndTwosOnly = true}};
			
			var currentWeek = new NFLWeek( Int32.Parse( Utility.CurrentSeason() ), Int32.Parse( Utility.CurrentWeek() ), false );
			var gs = new GS4Scorer( currentWeek ) { ScoresOnly = true, Master = master };
			pr.Scorer = gs;
			pr.Render( catCode, sPos, Constants.K_LEAGUE_Gridstats_NFL1, startersOnly: false );
		}

		public static void YahooProjections()
		{
			Utility.Announce("YahooProjections...");
			var yp = new YahooProjector();
			yp.AllProjections(  Utility.CurrentNFLWeek() );
		}

		public static void FpProjections()
		{
			Utility.Announce( "FP-Projections..." );
			//  Regenerate the projections based on latest roles
			var w = new NFLWeek( Utility.CurrentSeason(), Utility.CurrentWeek() );
			var sut = new PlayerProjectionGenerator();
			for ( int i = 0; i < w.GameList().Count; i++ )
			{
				var game = (NFLGame) w.GameList()[ i ];
				sut.Execute( game );
			}
			//  do full list
			var dao = new DbfPlayerGameMetricsDao();  
			var scorer = new YahooProjectionScorer();  
			var fpr = new FantasyProjectionReport(  Utility.CurrentSeason(), Utility.CurrentWeek(), dao, scorer );
			fpr.RenderAll();
		}

		public static void EspnPerformance()
		{
			Utility.Announce("EspnPerformance...");
			var prevWeek = Utility.PreviousWeek();

			var master = new YahooMaster( "Yahoo", "YahooOutput.xml" );
			BatchAllPositions( prevWeek, master, Constants.K_LEAGUE_50_Dollar_Challenge );
			BatchAllPositions( prevWeek, master, Constants.K_LEAGUE_Yahoo );
			BatchAllPositions( prevWeek, master, Constants.K_LEAGUE_Rants_n_Raves );
		}

		private static void BatchAllPositions( int prevWeek, YahooMaster master, string leagueId )
		{
			BatchEspnPerformance( Constants.K_QUARTERBACK_CAT, prevWeek, "QB", master, leagueId );
			BatchEspnPerformance( Constants.K_RECEIVER_CAT, prevWeek, "WR", master, leagueId );
			BatchEspnPerformance( Constants.K_RECEIVER_CAT, prevWeek, "TE", master, leagueId );
			BatchEspnPerformance( Constants.K_RUNNINGBACK_CAT, prevWeek, "RB", master, leagueId );
			BatchEspnPerformance( Constants.K_KICKER_CAT, prevWeek, "PK", master, leagueId );
		}

		private static void BatchEspnPerformance( string catCode, int week, string sPos, YahooMaster master, string leagueId )
		{
         //  2 reports
			GenericEspnPerformance( catCode, master, leagueId, sPos );
			WeeklyEspnPerformance( catCode, master, week, leagueId, sPos );
		}

		public static void Starters()
		{
			var s = new Starters();
			s.AllStarters( Constants.K_LEAGUE_Gridstats_NFL1 );
		}

		public static void GenericEspnPerformance(
			string catCode, YahooMaster master, string leagueId, string sPos )
		{
			var pl = new PlayerLister();

         var season = Utility.CurrentSeason();
         var week = Utility.CurrentWeek();
			var currentWeek = new NFLWeek(Int32.Parse(season), Int32.Parse(week), false);
			var gs = new EspnScorer( currentWeek ) {Master = master};

			pl.SetScorer(gs);
			pl.StartersOnly = true; // backups dont factoer in Yahoo
			pl.SetFormat("weekly");  //  as opposed to "annual"
			pl.AllWeeks = false; //  just the regular saeason
			pl.Season = currentWeek.Season;
			if (string.IsNullOrEmpty(sPos) || sPos.Equals("*") ) sPos = "All";

			pl.Collect( catCode, sPos, leagueId );
			pl.FileOut = string.Format( "{4}{2}//Performance//{3}-Yahoo {1} Performance upto Week {0}.htm", currentWeek.WeekNo, sPos, Utility.CurrentSeason(), leagueId, Utility.OutputDirectory() );
			pl.Render();
		}

		public static void RunUnitsByWeek()
		{
			Utility.Announce("RunUnitsByWeek");
			RunUnitsByWeek("PO");
			RunUnitsByWeek("RO");
			RunUnitsByWeek("PP");
			RunUnitsByWeek("PR");
			RunUnitsByWeek("RD");
			RunUnitsByWeek("PD");
			RunUnitsByWeek("FG");
		}

		public static void RunUnitsByWeek(string unitCode)
		{
			var ul = new UnitLister(unitCode);
			ul.Render(string.Format("Unit Report {1} week {0}", Utility.CurrentWeek(), unitCode));
		}

		public static void Rankings()
		{
			Utility.Announce("Rankings");
			//string suffix = " " + CurrentSeason() + "-" + CurrentWeek();
			const string suffix = "";
			RankReport("QB ranks" + suffix, "1", true);
			RankReport("RB ranks" + suffix, "2", true);
			RankReport("PR ranks" + suffix, "3", true);
			RankReport("PK ranks" + suffix, "4", true);
			RankReport("QB ranks all" + suffix, "1", false);
			RankReport("RB ranks all" + suffix, "2", false);
			RankReport("PR ranks all" + suffix, "3", false);
			RankReport("PK ranks all" + suffix, "4", false);
		}

		private static void RankReport(string header, string category, bool bCurrOnly)
		{
			var sSubHead = (bCurrOnly) ? " - Active" : " - All time";
			var sHead = header + sSubHead;
			Utility.Announce("RankReport " + sHead);
			var r = new SimpleTableReport {ReportHeader = sHead, ReportFooter = "", DoRowNumbers = true};
			var ds = Utility.TflWs.GetScoring(category, bCurrOnly, Int32.Parse(Utility.CurrentSeason()));
			r.AddColumn(new ReportColumn("Given", "FIRSTNAME", "{0,-10}"));
			r.AddColumn(new ReportColumn("Surname", "SURNAME", "{0,-15}"));
			r.AddColumn(new ReportColumn("RookieYr", "ROOKIEYR", "{0,4}"));
			r.AddColumn(new ReportColumn("Team", "CURRTEAM", "{0,2}"));
			r.AddColumn(new ReportColumn("Scores", "SCORES", "{0,5}"));
			r.AddColumn(new ReportColumn("Average", "OUTPUT", "{0:0.0}"));
			r.AddColumn(new ReportColumn("Rating", "CURRATING", "{0,5}"));
			r.LoadBody(ds.Tables[0]);
			r.RenderAsHtml(Utility.OutputDirectory() + header + ".htm", true);
		}

		public static void HotLists()
		{
			var hl = new HotListReporter();
			hl.RunAllHotlists();
			hl.League = Constants.K_LEAGUE_50_Dollar_Challenge;
			hl.RunAllHotlists();
			hl.League = Constants.K_LEAGUE_Yahoo;
			hl.RunAllHotlists();
		}

		public static void StatsGrids()
		{
			var statMaster = new StatMaster( "Stats", "stats.xml" );
			statMaster.Calculate( Utility.CurrentSeason(), 
				string.Format( "{0:0#}", Utility.CurrentNFLWeek().WeekNo - 1 ) );
			statMaster.Dump2Xml();
			var season = new NflSeason( Utility.CurrentSeason() );
			StatGridFor( season, "YDr", statMaster );
			StatGridFor( season, "YDp", statMaster );
			StatGridFor( season, "TDp", statMaster );
			StatGridFor( season, "TDr", statMaster );
			StatGridFor( season, "Sacks", statMaster );
			StatGridFor( season, "SacksAllowed", statMaster );
			StatGridFor( season, "YDrAllowed", statMaster );
			StatGridFor( season, "YDpAllowed", statMaster );
			StatGridFor( season, "TDpAllowed", statMaster );
			StatGridFor( season, "INTs", statMaster );
			StatGridFor( season, "INTsThrown", statMaster );
		}

		private static void StatGridFor( NflSeason season, string statType, StatMaster statMaster )
		{
			var sg = new StatGrid( season, statType, statMaster );
			sg.Render();
		}

		private static void Gs4WrRankings()
		{
			//  Create a player Listing
			var pl = new PlayerLister(Constants.K_RECEIVER_CAT, true);
			//  Create a rating system
			var theWeek = new NFLWeek(Int32.Parse(Utility.CurrentSeason()), Utility.WeeksAgo(4), false);
			var gs = new GS4Scorer(theWeek);
			pl.SetScorer(gs);
			pl.Render("Gridstats Receivers Last 4 weeks");
		}

		private static void StarRatings()
		{
			StarRatings(Constants.K_QUARTERBACK_CAT);
			StarRatings(Constants.K_RUNNINGBACK_CAT);
			StarRatings(Constants.K_RECEIVER_CAT);
		}

		/// <summary>
		///   A Grid Stats report than just counts the *BIG* games
		/// </summary>
		/// <param name="playerCategory"></param>
		private static void StarRatings(string playerCategory)
		{
			var lastWeek = Convert.ToInt32(Utility.CurrentWeek()) - 1;
			if (lastWeek > 0)
			{
				PlayerLister pl = new PlayerLister(playerCategory, faOnly: false,
				                                   fantasyLeague: Constants.K_LEAGUE_Gridstats_NFL1);
					//  All players (not just Free agents)
				var theWeek = new NFLWeek(Int32.Parse(Utility.CurrentSeason()), lastWeek, false);
				var scorer = new StarScorer {Week = theWeek, Season = Utility.CurrentSeason()};
				pl.SetScorer(scorer);
				pl.Week = theWeek.WeekNo;
				pl.Season = theWeek.Season;
				pl.WeeksToGoBack = pl.Week;
				pl.SubHeader = string.Format("{0} Big Games", Utility.MainPos(playerCategory));

				var header = string.Format("stars\\Stars-{2}-{0}-{1:0#}", scorer.Season, scorer.Week.WeekNo,
				                           Utility.MainPos(playerCategory));
				Utility.Announce(string.Format("  Rendering - {0}", header));
				pl.Render(header);
			}
		}

		public static void GridStatsRankings()
		{
			var theWeek = Int32.Parse(Utility.CurrentWeek());
			Gs4WrRankings();
			GridStatsQbRankings(theWeek);
			GridStatsRbRankings(theWeek);
			GridStatsPrRankings(theWeek);
			GridStatsKickerRankings(theWeek);
		}

		public static void GridStatsQbRankings(int week)
		{
			GridStatRanking(Constants.K_QUARTERBACK_CAT, "QB", week);
		}

		private static void GridStatRanking(string theCat, string thePos, int week)
		{
			var pl = new PlayerLister();
			var theWeek = new NFLWeek(Int32.Parse(Utility.CurrentSeason()), week, false);
			var gs = new GS4Scorer(theWeek);
			pl.SetScorer(gs);
			pl.Collect( theCat, thePos, Constants.K_LEAGUE_Gridstats_NFL1 );
			pl.Render(string.Format("GS {1}s week {0}", week, thePos));
		}

		public static void GridStatsRbRankings(int week)
		{
			GridStatRanking(Constants.K_RUNNINGBACK_CAT, "RB", week);
		}

		public static void GridStatsPrRankings(int week)
		{
			GridStatRanking(Constants.K_RECEIVER_CAT, "Receivers", week);
		}

		public static void GridStatsKickerRankings(int week)
		{
			GridStatRanking(Constants.K_KICKER_CAT, "Kickers", week);
		}

		private static void NflukRatings()
		{
			DoNflukRatings(new QuarterbackCategory());
			DoNflukRatings(new RunningbackCategory());
			DoNflukRatings(new ReceiverCategory());
			DoNflukRatings(new DefensiveTeamCategory());
			DoNflukRatings(new KickerCategory());
		}

		private static void DefensiveScoring()
		{
			var ds = new DefensiveScorer();
			ds.RenderDefensiveReports( Utility.CurrentNFLWeek() );
		}

		private static void DoNflukRatings(PlayerPos pp)
		{
			if (pp.Category.Equals(Constants.K_DEFENSIVETEAM_CAT))
			{
				var nflUkTeams = new NFLUKTeamScorer();
				var tl = new TeamLister();
				tl.SetScorer(nflUkTeams);
				tl.Render(string.Format("NflUk.com {2} rankings {0} week {1:0#} ",
				                        Utility.CurrentSeason(), Int32.Parse(Utility.CurrentWeek()) + 1, pp.Name));
			}
			else
			{
				var nflUkTeams = new NFLUKPlayerScorer();
				//  Create a player Listing
				var pl = new PlayerLister();
				//  Create a rating system
				pl.SetScorer(nflUkTeams);
				pl.Collect( pp.Category, "*", Constants.K_LEAGUE_Yahoo );
				pl.Render(string.Format("NflUk.com {2} rankings {0} week {1:0#} ",
				                        Utility.CurrentSeason(), Int32.Parse(Utility.CurrentWeek()) + 1, pp.Name));
			}
		}

		#endregion

		#region  Routines

		public static void RunUnitReports()
		{
			Utility.Announce("RunUnitReports...");
			using (var ur = new UnitReport())
			{
				ur.Render();
			}
			//  Also do the individual Unit reports
		}

		public static void OldRosterGrid()
		{
			Utility.Announce("Roster Grid Report - Quarterbacks...");
			var rg = new NFLRosterGrid( Constants.K_QUARTERBACK_CAT ) {Focus = "QB"};
			rg.CurrentRoster();
			Utility.Announce( "Roster Grid Report - QB Injuries..." );
			rg.CurrentInjuries();

			Utility.Announce("Roster Grid Report - Running backs...");
			rg = new NFLRosterGrid( Constants.K_RUNNINGBACK_CAT ) {Focus = "RB"}; //  Running backs
			rg.CurrentRoster();
			Utility.Announce( "Roster Grid Report - RB Injuries..." );
			rg.CurrentInjuries();

			Utility.Announce("Roster Grid Report - Receivers...");
			rg = new NFLRosterGrid( Constants.K_RECEIVER_CAT ) {Focus = "WR"};
			rg.CurrentRoster();
			Utility.Announce( "Roster Grid Report - PR Injuries..." );
			rg.CurrentInjuries();

			Utility.Announce("Roster Grid Report - Tight Ends...");
			rg = new NFLRosterGrid( Constants.K_RECEIVER_CAT ) { Focus = "TE" };
			rg.CurrentRoster();

			Utility.Announce("Roster Grid Report - Kickers...");
			var krg = new NFLRosterGrid( Constants.K_KICKER_CAT ) { Focus = "K" };
			krg.CurrentRoster();
			Utility.Announce( "Roster Grid Report - PK Injuries..." );
			krg.CurrentInjuries();
		}

		private static void FreeAgentReport()
		{
			Utility.Announce("FreeAgentReport...");
			var far = new FreeAgentReport();
			far.Render();
		}

		public static void GridStatsReport()
		{
			GridStatsReport(Constants.K_LEAGUE_Gridstats_NFL1, "GS1 - Great Britain");
			if ( Utility.CurrentWeek() != "0" )
				GridStatsReport(Constants.K_LEAGUE_Yahoo, "Yahoo League");
		}

		private static void GridStatsReport(string leagueCode, string leagueName)
		{
			//Utility.Announce(string.Format("GridStatsReport {0}...", leagueName));

			//  Create a GridStats League
			var gridStatsLeague = new GridStatsLeague( 
				leagueName, leagueCode, Utility.CurrentSeason(), Int32.Parse( Utility.CurrentWeek() ) );
			//  Tell it to produce the Roster Report
			gridStatsLeague.RosterReport();
			//  Do this weeks ratings too
			gridStatsLeague.GameRatings(Utility.CurrentNFLWeek(), "O001"); //  for the upcoming week
		}

		public static void EspnReport()
		{
			//  Create a GridStats League
			var espnLeague = new EspnLeague("Yahoo - Bretts Retirement", "YH", Utility.CurrentSeason(), Int32.Parse( Utility.CurrentWeek() ) );
			//  Tell it to produce the Roster Report
			espnLeague.RosterReport();
			//  Do this weeks ratings too
			espnLeague.GameRatings(Utility.CurrentNFLWeek(), "O001"); //  for the upcoming week
		}

		public static void StrengthOfSched()
		{
			Utility.Announce("Strength of Schedule for " + Utility.CurrentSeason() + "...");
			var sos = new StrengthOfSchedule(Utility.CurrentSeason());
			sos.RenderAsHtml();
		}

		public static void VictoryPoints()
		{
			Utility.Announce("Victory Points for " + Utility.CurrentSeason() + "...");
			var vp = new VictoryPoints(Utility.CurrentSeason());
			vp.RenderAsHtml();
		}

		public static void FaMarket()
		{
			Utility.Announce("Free Agent Market Analysis for " + Utility.CurrentSeason() + "...");
			var fa = new FaMarket( Utility.CurrentSeason() );
			fa.RenderAsHtml();
		}

		public static void FaMarket(string seasonIn)
		{
			Utility.Announce("Free Agent Market Analysis for " + seasonIn + "...");
			var fa = new FaMarket(seasonIn);
			fa.RenderAsHtml();
		}

		public static void LineupCards()
		{
			Utility.Announce("Lineup Cards for " + Utility.CurrentSeason() + "...");
			var ls = new LineupSlate(Utility.CurrentSeason());
			ls.RenderAsHtml();
		}

		/// <summary>
		///  A routine to print match up reports for the upcoming week.
		/// </summary>
		public static void NextWeeksMatchups()
		{
			Utility.Announce("NextWeeksMatchups...");
			var currentWeek = Utility.CurrentNFLWeek();
			currentWeek.RenderMatchups();
		}

		/// <summary>
		///   A routine to put the upcoming NFL schedule into Outlook.
		///   Run this once in the pre-season.
		/// </summary>
		//public static void SaveAppointments()
		//{
		//    for ( int w = 1; w < 18; w++ )
		//    {
		//        NFLWeek week = new NFLWeek( Int32.Parse( CurrentSeason() ), w );
		//        week.SaveAppointments();
		//    }
		//}
		/// <summary>
		///   A routine to examine the upcoming week for potential propositions.
		///   Prereqs:  none.
		/// </summary>
		public static void BestBets(string seasonIn, int weekIn)
		{
			if (weekIn > 0)
			{
				Utility.Announce("Best Bets " + seasonIn + ":" + weekIn + "...");
				var thisWeek = new NFLWeek(Int32.Parse(seasonIn), weekIn);
				thisWeek.RenderBestBets();
			}
		}

		#endregion

		#region   Helper functions

		private static void DisplayReportSettings()
		{
#if DEBUG
			Utility.WriteLog("Debug mode");
#else
				Utility.WriteLog("Release mode");
#endif
			var exeAssembly = Assembly.GetExecutingAssembly();
			var name = exeAssembly.GetName();
			var version = name.Version;

			Utility.Announce(string.Format("Version is {0}", version));
			Utility.Announce(string.Format("Current Season is {0}", Utility.CurrentSeason()));
			Utility.Announce(string.Format("Current Week   is {0}", Utility.CurrentWeek()));
			WriteFlag("Old Grids           ", Config.OldFormat());
			WriteFlag("New Roster          ", Config.NewFormat());
			WriteFlag("Offensive Line      ", Config.DoOffensiveLine());
			WriteFlag("Rankings            ", Config.DoRankings());
			WriteFlag("Current Scorers     ", Config.DoCurrentScorers());
			WriteFlag("Team Cards          ", Config.DoTeamcards());
			WriteFlag("Projections         ", Config.DoProjections());
			WriteFlag("Kickers             ", Config.DoKickers());
			WriteFlag("Experience Points   ", Config.DoExperience());
			WriteFlag("Victory Points      ", Config.DoVictoryPoints());
			WriteFlag("Free Agent Market   ", Config.DoFaMarket());
			WriteFlag("Strength of Schedule", Config.DoStrengthOfSchedule());
			WriteFlag("Lineup Cards        ", Config.DoLineupCards());
			WriteFlag("Team Metrics        ", Config.DoTeamMetrics());
			WriteFlag("GS4 Roster          ", Config.DoGridStatsReport());
			WriteFlag("GS4 WR list         ", Config.DoGs4WrRanks());
			WriteFlag("Hot Lists           ", Config.DoHotLists());
			WriteFlag("Suggest Lineups     ", Config.DoSuggestedLineups());
			WriteFlag("Matchups            ", Config.DoMatchups());
			WriteFlag("UnitReports         ", Config.DoUnitReports());
			WriteFlag("Free Agents         ", Config.DoFreeAgents());
			WriteFlag("Star Ratings        ", Config.DoStarRatings());
			WriteFlag("GS Performance      ", Config.DoGsPerformance());
			WriteFlag("GridStats           ", Config.DoGridStatsReport());
			WriteFlag("ESPN Performance    ", Config.DoEspnPerformance());
			WriteFlag("Balance Report      ", Config.DoBalanceReport());
			WriteFlag("Best Bets           ", Config.DoPlays());
			WriteFlag("Back testing        ", Config.DoBackTest());
			WriteFlag("Player Reports      ", Config.DoPlayerReports());
			WriteFlag("Player CSV          ", Config.DoPlayerCsv());
			WriteFlag("Player Reports      ", Config.DoDefensiveScoring());
			WriteFlag("ESPN                ", Config.DoEspn());
			WriteFlag("Starters            ", Config.DoStarters());
			WriteFlag("Returners           ", Config.DoReturners());
			WriteFlag("All Games           ", Config.AllGames);
			WriteFlag("Units By Week       ", Config.DoUnitsByWeek());
         WriteFlag("StatsGrids          ", Config.DoStatsGrids());
			WriteFlag("Hillen Tips         ", Config.DoHillenTips() );
			WriteFlag("Depth Charts        ", Config.DoDepthCharts() );
			WriteFlag("FP Projections      ", Config.DoFpProjections() );
		}

		private static void WriteFlag(string flagName, bool flag)
		{
			Utility.Announce(flagName + " " + ((flag) ? "ON" : "OFF"));
		}

		protected static string GetStackTrace()
		{
			var result = new StringBuilder();
			// Create a call stack trace with file information. 
			var trace = new StackTrace(true);
			var frameCount = trace.FrameCount;
			for (int n = 0; n < frameCount; ++n)
			{
				StackFrame frame = trace.GetFrame(n);
				result.Append(frame.ToString());
			}
			result.Append(trace.GetFrame(1).GetMethod().DeclaringType.ToString());
			return result.ToString();
		}

		protected static string GetStackTraceWithMethods()
		{
			var result = new StringBuilder();
			var trace = new StackTrace(true);
			var frameCount = trace.FrameCount;
			for (int n = 0; n < frameCount; ++n)
			{
				//StringBuilder frameString = new StringBuilder(); 
				var frame = trace.GetFrame(n);

				var lineNumber = frame.GetFileLineNumber();
				var fileName = frame.GetFileName();
				var methodBase = frame.GetMethod();
				var methodName = methodBase.Name;
				var paramInfos = methodBase.GetParameters();
				result.AppendFormat("{0} - line {1}, {2}",
				                    fileName,
				                    lineNumber,
				                    methodName);
				if (paramInfos.Length == 0)
				{
					// No parameters for this method; display 
					// empty parentheses. 
					result.Append("()\n");
				}
				else
				{
					// Iterate over parameters, displaying each parameter's 
					// type and name.
					result.Append("(");
					int count = paramInfos.Length;
					for (int i = 0; i < count; ++i)
					{
						Type paramType = paramInfos[i].ParameterType;
						result.AppendFormat("{0} {1}",
						                    paramType,
						                    paramInfos[i].Name);
						if (i < count - 1)
							result.Append(",");
					}
					result.Append(")\n");
				}
			}
			return result.ToString();
		}

		#endregion
	}
}