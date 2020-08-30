﻿using NLog;
using RosterLib.Interfaces;
using System;
using System.Collections;
using System.Data;
using System.Runtime.InteropServices;

namespace RosterLib
{
	public class RenderStatsToHtml : IRender
	{
		private readonly IWeekMaster WeekMaster;

		public string SubHeader { get; set; }
		public string Season { get; set; }
		public int Week { get; set; }
		public int WeeksToGoBack { get; set; }
		public bool RenderToCsv { get; set; }
		public bool LongStats { get; set; }
		public bool SupressZeros { get; set; }

		public bool ShowOpponent { get; set; }

		public string FileOut { get; set; }

		public Logger Logger { get; set; }

		public RenderStatsToHtml( IWeekMaster weekMasterIn )
		{
			WeekMaster = weekMasterIn;
			Logger = LogManager.GetCurrentClassLogger();
		}

		public string RenderData( 
			ArrayList playerList, 
			string sHead, 
			[Optional] string sortOrder,
			IRatePlayers scorer )
		{
			//  Output the list
			Utility.Announce( "PlayerListing " + sHead );
			var r = new SimpleTableReport
			{
				ReportHeader = sHead,
				DoRowNumbers = true
			};
			if ( !string.IsNullOrEmpty( SubHeader ) )
				r.SubHeader = SubHeader;

			var ds = LoadData( playerList, scorer );    //  <--action

			r.AddColumn( new ReportColumn( "Name", "NAME", "{0,-15}" ) );
			r.AddColumn( new ReportColumn( "Pos", "POS", "{0,9}" ) );
			r.AddColumn( new ReportColumn( "Role", "ROLE", "{0,9}" ) );
			r.AddColumn( new ReportColumn( "RookieYr", "ROOKIEYR", "{0,4}" ) );
			r.AddColumn( new ReportColumn( "Team", "CURRTEAM", "{0,2}" ) );
			r.AddColumn( new ReportColumn( "Age", "AGE", "{0,2}" ) );
			r.AddColumn( new ReportColumn( "Owner", "FT", "{0,2}" ) );
			if ( LongStats )
			{
				r.AddColumn( new ReportColumn( "Curr", "CURSCORES", "{0,5}" ) );
				r.AddColumn( new ReportColumn( "Tot", "SCORES", "{0,5}" ) );
				r.AddColumn( new ReportColumn( "Avg", "AVG", "{0:0.0}" ) );
				r.AddColumn( new ReportColumn( "Inj", "INJURY", "{0,5}" ) );
				r.AddColumn( new ReportColumn( "YDp", "YDP", "{0,5}" ) );
				r.AddColumn( new ReportColumn( "Tdp", "TDP", "{0,5}" ) );
				r.AddColumn( new ReportColumn( "YDr", "YDR", "{0,5}" ) );
				r.AddColumn( new ReportColumn( "Tdr", "TDR", "{0,5}" ) );
				r.AddColumn( new ReportColumn( "YDc", "YDC", "{0,5}" ) );
				r.AddColumn( new ReportColumn( "TDc", "TDC", "{0,5}" ) );
				r.AddColumn( new ReportColumn( "Fg", "Fg", "{0,5}" ) );
			}
			r.AddColumn( new ReportColumn( "Points", "POINTS", "{0,5}" ) );

			var dt = ds.Tables[ 0 ];
			dt.DefaultView.Sort = LongStats
			   ? ( string.IsNullOrEmpty( sortOrder ) ? "Points DESC" : sortOrder ) 
			                                         : "Points DESC";

			r.LoadBody( dt );
			if ( string.IsNullOrEmpty( FileOut ) )
				FileOut = $@"{
					Utility.OutputDirectory()
					}//{
					Utility.CurrentSeason()
					}//{
					sHead
					}.htm";
			r.RenderAsHtml(fileName: FileOut, persist: true );

			if ( RenderToCsv )
				r.RenderAsCsv( "Starters-" + sHead, Logger );
			return FileOut;
		}

		public DataSet LoadData(
			ArrayList plyrList,
			IRatePlayers scorer)
		{
			var ds = new DataSet();
			var dt = new DataTable();
			var cols = dt.Columns;
			cols.Add( "Name", typeof( String ) );
			cols.Add( "Pos", typeof( String ) );
			cols.Add( "Role", typeof( String ) );
			cols.Add( "RookieYr", typeof( String ) );
			cols.Add( "Age", typeof( String ) );
			cols.Add( "Currteam", typeof( String ) );
			cols.Add( "FT", typeof( String ) );

			if ( LongStats )
			{
				cols.Add( "CurScores", typeof( Int32 ) );
				cols.Add( "Scores", typeof( Int32 ) );
				cols.Add( "Avg", typeof( Decimal ) );
				cols.Add( "Injury", typeof( Int32 ) );
				cols.Add( "Tdp", typeof( Int32 ) );
				cols.Add( "YDp", typeof( Int32 ) );
				cols.Add( "Tdr", typeof( Int32 ) );
				cols.Add( "TDc", typeof( Int32 ) );
				cols.Add( "YDr", typeof( Int32 ) );
				cols.Add( "YDc", typeof( Int32 ) );
				cols.Add( "Fg", typeof( Int32 ) );
			}

			cols.Add( "Points", typeof( Decimal ) );
			cols.Add( "AgeRate", typeof( Decimal ) );
			cols.Add( "ADP", typeof( Int32 ) );

			if ( Season == null ) Season = Utility.CurrentSeason();

			foreach ( NFLPlayer p in plyrList )
			{
				if (p.TotStats == null)
					p.LoadPerformances(
						allGames: false,
						currSeasonOnly: true,
						whichSeason: Season); //  to get the stats

				//  rate the last whatevr weeks
				var theWeek = WeekMaster != null ?
				   WeekMaster.GetWeek(Season, Week) 
				   : new NFLWeek(
					   Int32.Parse(Season),
					   Week,
					   loadGames: true);

				var totPoints = 0M;
				for (var w = WeeksToGoBack; w > 0; w--)
				{
					if (scorer != null) scorer.RatePlayer(
						p,
						theWeek);
					totPoints += p.Points;
					theWeek = WeekMaster != null 
						? WeekMaster.PreviousWeek(theWeek)
					   : theWeek.PreviousWeek(
						   theWeek: theWeek,
						   loadgames: false,
						   regularSeasonGamesOnly: false);
				}

				if (totPoints <= 0 && SupressZeros)
					continue;

				DataRow dr = PopulatePlayerRow(dt, p, totPoints);
				dt.Rows.Add(dr);
			}
			ds.Tables.Add( dt );

			return ds;
		}

		private DataRow PopulatePlayerRow(
			DataTable dt,
			NFLPlayer p,
			decimal totPoints)
		{
			var dr = dt.NewRow();
			dr["Name"] = p.ProjectionLink(Season);

			dr["Pos"] = p.PlayerPos;
			dr["Role"] = p.RoleOut();
			dr["RookieYr"] = p.RookieYear + "-" + p.Drafted;
			dr["CurrTeam"] = p.TeamCode;
			dr["FT"] = p.Owner;
			dr["Age"] = p.PlayerAge(
				noQuestionMark: true);

			if (LongStats)
			{
				dr["CurSCORES"] = p.CurrScores;
				dr["SCORES"] = p.Scores;
				dr["Avg"] = p.ScoresPerYear();
				dr["INJURY"] = p.Injuries();
				dr["Tdp"] = p.TotStats.Tdp;
				dr["YDp"] = p.TotStats.YDp;
				dr["Tdr"] = p.TotStats.Tdr;
				dr["TDc"] = p.TotStats.Tdc;
				dr["YDr"] = p.TotStats.YDr;
				dr["YDc"] = p.TotStats.YDc;
				dr["Fg"] = p.TotStats.Fg;
			}

			dr["Points"] = totPoints;
			dr["AgeRate"] = p.AgeRating();
			dr["ADP"] = p.Adp;
			return dr;
		}

		public string AsDraftRound(int adp)
		{
			if (adp < 0) return string.Empty;
			var round = (adp / 12) + 1 ;
			int remainder;
			if (round == 1)
				remainder = adp;
			else
			    remainder = adp - ((round-1) * 12) + 1;
			var result = $"{round:#0}.{remainder:00}";
			return result;
		}

		public string RenderProjectedData(
			ArrayList playerList, 
			string sHead, 
			[Optional] string sortOrder,
		    IRatePlayers scorer, 
			IWeekMaster weekMaster,
			IAdpMaster adpMaster = null)
		{
			//  Output the list
			Utility.Announce("PlayerListing " + sHead);

			SimpleTableReport r = SetUpProjectionReport(
				sHead);

			var ds = LoadProjectedData(
				plyrList: playerList,
				scorer: scorer,
				weekMaster: weekMaster,
				adpMaster: adpMaster); 

			var dt = ds.Tables[0];
			dt.DefaultView.Sort = LongStats
			   ? (string.IsNullOrEmpty(sortOrder) 
			   ? "Points DESC" : sortOrder) : "Points DESC";

			r.LoadBody(dt); //  just assigns the data table
			FileOut = $@"{
				Utility.OutputDirectory()
				}//{
				Season
				}//Projections//{
				sHead
				}.htm";
			r.RenderAsHtml(
				fileName: FileOut, 
				persist: true);

			if (RenderToCsv)
				r.RenderAsCsv(
					"Starters-" + sHead, 
					Logger);
			return FileOut;
		}

		private SimpleTableReport SetUpProjectionReport(
			string sHead)
		{
			var r = new SimpleTableReport
			{
				ReportHeader = sHead,
				ReportFooter = Season,
				DoRowNumbers = true
			};

			if (!string.IsNullOrEmpty(SubHeader)) r.SubHeader = SubHeader;
			///////////////////////////////////////////////////////////////
			///////////////////////////////////////////////////////////////
			r.AddColumn(new ReportColumn("Name", "NAME", "{0,-15}"));
			r.AddColumn(new ReportColumn("Pos", "POS", "{0,9}"));
			r.AddColumn(new ReportColumn("Role", "ROLE", "{0,9}"));
			r.AddColumn(new ReportColumn("RookieYr", "ROOKIEYR", "{0,4}"));
			r.AddColumn(new ReportColumn("Team", "CURRTEAM", "{0,2}"));
			if (ShowOpponent)
			{
				r.AddColumn(new ReportColumn("Opp", "OPPONENT", "{0,2}"));
				r.AddColumn(new ReportColumn("Opp", "OPPRATE", "{0,2}"));
				r.AddColumn(new ReportColumn("Spread", "SPREAD", "{0,5}"));
			}
			r.AddColumn(new ReportColumn("Age", "AGE", "{0,2}"));
			r.AddColumn(new ReportColumn("Owner", "FT", "{0,2}"));
			if (LongStats)
			{
				r.AddColumn(new ReportColumn("cat", "CAT", "{0,5}"));
				r.AddColumn(new ReportColumn("scoremod", "SCOREMOD", "{0,5}"));
				r.AddColumn(new ReportColumn("seasons", "SEASONS", "{0,5}"));
				r.AddColumn(new ReportColumn("newteam", "NEWTEAM", "{0,5}"));
				r.AddColumn(new ReportColumn("Curr", "CURSCORES", "{0,5}"));
				r.AddColumn(new ReportColumn("Tot", "SCORES", "{0,5}"));
				r.AddColumn(new ReportColumn("Avg", "AVG", "{0:0.0}"));
				r.AddColumn(new ReportColumn("Inj", "INJURY", "{0,5}"));
				r.AddColumn(new ReportColumn("YDp", "YDP", "{0,5}"));
				r.AddColumn(new ReportColumn("Tdp", "TDP", "{0,5}"));
				r.AddColumn(new ReportColumn("YDr", "YDR", "{0,5}"));
				r.AddColumn(new ReportColumn("Tdr", "TDR", "{0,5}"));
				r.AddColumn(new ReportColumn("YDc", "YDC", "{0,5}"));
				r.AddColumn(new ReportColumn("TDc", "TDC", "{0,5}"));
				r.AddColumn(new ReportColumn("Fg", "Fg", "{0,5}"));
				r.AddColumn(new ReportColumn("newbie", "NEWBIE", "{0,5}"));
				r.AddColumn(new ReportColumn("AgeRate", "AgeRate", "{0,5}"));
			}
			r.AddColumn(new ReportColumn("Points", "POINTS", "{0,5}"));
			r.AddColumn(new ReportColumn("ADP", "ADP", "{0,5}"));
			return r;
		}

		public DataSet LoadProjectedData(
			ArrayList plyrList, 
			IRatePlayers scorer, 
			IWeekMaster weekMaster,
			IAdpMaster adpMaster = null)
		{
			var ds = new DataSet();
			var dt = new DataTable();
			DefineReportColumns(dt);

			if (Season == null)
				Season = Utility.CurrentSeason();

			var dao = new DbfPlayerGameMetricsDao();

			foreach (NFLPlayer p in plyrList)
			{
				if (p.PlayerName.Equals("Mark Ingram"))
					Console.WriteLine("TestPlayer check");

				var pgms = dao.GetSeason(Season, p.PlayerCode);

				var totPoints = 0M;
				foreach (PlayerGameMetrics pgm in pgms)
				{
					var nWeek = Int32.Parse(pgm.Week());
					var theWeek = weekMaster.GetWeek(
						season: Season,
						week: nWeek);

					//  We are only concerned with the first 4 weeks
					//if (nWeek > 4)
					//	continue;

					// if there is no scorer it just reads the stats, 
					// this is what we want
					if (scorer == null)
						p.Points = pgm.CalculateProjectedFantasyPoints(p);
					else
						scorer.RatePlayer(p, theWeek);

					if (p.TotStats == null)
						p.TotStats = new PlayerStats();
					p.TotStats.Tdp += pgm.ProjTDp;
					p.TotStats.YDp += pgm.ProjYDp;
					p.TotStats.Tdr += pgm.ProjTDr;
					p.TotStats.Tdc += pgm.ProjTDc;
					p.TotStats.YDr += pgm.ProjYDr;
					p.TotStats.YDc += pgm.ProjYDc;
					p.TotStats.Fg += pgm.ProjFG;
					totPoints += p.Points;
				}

				if (totPoints > 0 || !SupressZeros)
				{
					var dr = dt.NewRow();
					dr["Name"] = p.ProjectionLink(Season);
					dr["Pos"] = p.PlayerPos;
					dr["Role"] = p.RoleOut();
					dr["RookieYr"] = p.RookieYear;
					dr["CurrTeam"] = p.TeamCode;
					if (ShowOpponent)
					{
						dr["Opponent"] = p.Opponent;
						dr["Spread"] = p.PlayerSpread;
						dr["OppRate"] = p.OppRate;
					}
					if (p.Owner == null) p.LoadOwner();
					dr["FT"] = p.Owner;
					dr["Age"] = p.PlayerAge();

					if (LongStats)
					{
						dr["cat"] = p.PlayerCat;
						dr["scoremod"] = p.ScoreModifier();
						dr["seasons"] = p.NoOfSeasons();
						dr["newteam"] = p.IsNewbie() ? "*" : string.Empty;
						dr["CurSCORES"] = p.CurrScores;
						dr["SCORES"] = p.Scores;
						dr["Avg"] = p.ScoresPerYear();
						dr["INJURY"] = p.Injuries();
						dr["Tdp"] = p.TotStats.Tdp;
						dr["YDp"] = p.TotStats.YDp;
						dr["Tdr"] = p.TotStats.Tdr;
						dr["TDc"] = p.TotStats.Tdc;
						dr["YDr"] = p.TotStats.YDr;
						dr["YDc"] = p.TotStats.YDc;
						dr["Fg"] = p.TotStats.Fg;
						dr["Health"] = p.HealthRating();
						dr["newbie"] = 1.0M - p.NewbieModifier();
						dr["adjProj"] = AdjustedPoints(
							p, 
							totPoints);
						dr["AgeRate"] = p.AgeRating();
					}

					dr["Points"] = totPoints;
					if (!LongStats)
						dr["Points"] = (int) AdjustedPoints(
							p,
							totPoints);

					dr["Adp"] = AsDraftRound(p.Adp);
					if (adpMaster != null)
						dr["Adp"] = adpMaster.GetAdp(
							p.PlayerName);
					dt.Rows.Add(dr);
					//Logger.Trace($"{p.PlayerName:-20} {totPoints:0.0}");
				}
			}
			ds.Tables.Add(dt);
			return ds;
		}

		private static decimal AdjustedPoints(
			NFLPlayer p, 
			decimal totPoints)
		{
			var hr = p.HealthRating();
			var ar = p.AgeRating();
			var sm = p.ScoreModifier();
			var nm = p.NewbieModifier();
			if (p.PlayerName.Equals("Mark Ingram"))
				Console.WriteLine("TestPlayer check");
			var adjPoints = 
			    totPoints
				* hr
				* ar
				* sm
				* nm;
			return adjPoints;
		}

		private void DefineReportColumns(DataTable dt)
		{
			var cols = dt.Columns;
			cols.Add("Name", typeof(String));
			cols.Add("Pos", typeof(String));
			cols.Add("Role", typeof(String));
			cols.Add("RookieYr", typeof(String));
			cols.Add("Age", typeof(String));
			cols.Add("Currteam", typeof(String));
			if (ShowOpponent)
			{
				cols.Add("Opponent", typeof(String));
				cols.Add("Spread", typeof(String));
				cols.Add("OppRate", typeof(String));
			}
			cols.Add("FT", typeof(String));

			if (LongStats)
			{
				cols.Add("cat", typeof(String));
				cols.Add("scoremod", typeof(Decimal));
				cols.Add("Seasons", typeof(Int32));
				cols.Add("newteam", typeof(String));
				cols.Add("CurScores", typeof(Int32));
				cols.Add("Scores", typeof(Int32));
				cols.Add("Avg", typeof(Decimal));
				cols.Add("Injury", typeof(Int32));
				cols.Add("Tdp", typeof(Int32));
				cols.Add("YDp", typeof(Int32));
				cols.Add("Tdr", typeof(Int32));
				cols.Add("TDc", typeof(Int32));
				cols.Add("YDr", typeof(Int32));
				cols.Add("YDc", typeof(Int32));
				cols.Add("Fg", typeof(Int32));
				cols.Add("Health", typeof(Decimal));
				cols.Add("newbie", typeof(Decimal));
				cols.Add("AdjProj", typeof(Int32));
				cols.Add("AgeRate", typeof(Decimal));
			}

			cols.Add("Points", typeof(Decimal));
			cols.Add("Adp", typeof(string));
		}
	}
}