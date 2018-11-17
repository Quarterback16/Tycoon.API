﻿using System;
using System.Collections;
using System.Data;
using System.Runtime.InteropServices;

namespace RosterLib
{
	public class RenderStatsToHtml : IRender
	{
		public string SubHeader { get; set; }
		public string Season { get; set; }
		public int Week { get; set; }
		public int WeeksToGoBack { get; set; }
		public bool RenderToCsv { get; set; }
		public bool LongStats { get; set; }
		public bool SupressZeros { get; set; }

		public string FileOut { get; set; }

		public string RenderData(ArrayList playerList, string sHead, [Optional] string sortOrder, IRatePlayers scorer)
		{
			//  Output the list
			Utility.Announce("PlayerListing " + sHead);
			var r = new SimpleTableReport {ReportHeader = sHead, DoRowNumbers = true};
			if (! string.IsNullOrEmpty(SubHeader)) r.SubHeader = SubHeader;

			var ds = LoadData(playerList, scorer);

			r.AddColumn(new ReportColumn("Name", "NAME", "{0,-15}"));
			r.AddColumn(new ReportColumn("Pos", "POS", "{0,9}"));
			r.AddColumn(new ReportColumn("Role", "ROLE", "{0,9}"));
			r.AddColumn(new ReportColumn("RookieYr", "ROOKIEYR", "{0,4}"));
			r.AddColumn(new ReportColumn("Team", "CURRTEAM", "{0,2}"));
			r.AddColumn(new ReportColumn("Age", "AGE", "{0,2}"));
			r.AddColumn(new ReportColumn("Owner", "FT", "{0,2}"));
			if (LongStats)
			{
				r.AddColumn(new ReportColumn("Curr", "CURSCORES", "{0,5}"));
				r.AddColumn(new ReportColumn("Tot", "SCORES", "{0,5}"));
				r.AddColumn(new ReportColumn("Avg", "AVG", "{0:0.#0}"));
				r.AddColumn(new ReportColumn("Inj", "INJURY", "{0,5}"));
				r.AddColumn(new ReportColumn("YDp", "YDP", "{0,5}"));
				r.AddColumn(new ReportColumn("Tdp", "TDP", "{0,5}"));
				r.AddColumn(new ReportColumn("YDr", "YDR", "{0,5}"));
				r.AddColumn(new ReportColumn("Tdr", "TDR", "{0,5}"));
				r.AddColumn(new ReportColumn("YDc", "YDC", "{0,5}"));
				r.AddColumn(new ReportColumn("TDc", "TDC", "{0,5}"));
				r.AddColumn(new ReportColumn("Fg", "Fg", "{0,5}"));
			}
			r.AddColumn(new ReportColumn("Points", "POINTS", "{0,5}"));

			var dt = ds.Tables[0];
			dt.DefaultView.Sort = LongStats 
				? (string.IsNullOrEmpty(sortOrder) ? "Points DESC" : sortOrder) : "Points DESC";

			r.LoadBody(dt); //  just assigns the data table
			if ( string.IsNullOrEmpty( FileOut ) )
			   FileOut = string.Format("{0}{2}\\{1}.htm", 
					Utility.OutputDirectory(), sHead, Utility.CurrentSeason() );
			r.RenderAsHtml( FileOut, true);

			if (RenderToCsv)
				r.RenderAsCsv("Starters-" + sHead);
			return FileOut;
		}

		public DataSet LoadData(ArrayList plyrList, IRatePlayers scorer)
		{
			var ds = new DataSet();
			var dt = new DataTable();
			var cols = dt.Columns;
			cols.Add("Name", typeof (String));
			cols.Add("Pos", typeof (String));
			cols.Add("Role", typeof (String));
			cols.Add("RookieYr", typeof (String));
			cols.Add("Age", typeof (String));
			cols.Add("Currteam", typeof (String));
			cols.Add("FT", typeof (String));

			if (LongStats)
			{
				cols.Add("CurScores", typeof (Int32));
				cols.Add("Scores", typeof (Int32));
				cols.Add("Avg", typeof (Decimal));
				cols.Add("Injury", typeof (Int32));
				cols.Add("Tdp", typeof (Int32));
				cols.Add("YDp", typeof (Int32));
				cols.Add("Tdr", typeof (Int32));
				cols.Add("TDc", typeof (Int32));
				cols.Add("YDr", typeof (Int32));
				cols.Add("YDc", typeof (Int32));
				cols.Add("Fg", typeof (Int32));
			}

			cols.Add("Points", typeof (Decimal));

			if (Season == null) Season = Utility.CurrentSeason();

			foreach (NFLPlayer p in plyrList)
			{
				if (p.TotStats == null) p.LoadPerformances(false, true, Season); //  to get the stats

				//  rate the last whatevr weeks
				var theWeek = new NFLWeek(Int32.Parse(Season), Week, false); // start here
				var totPoints = 0M;
				for (var w = WeeksToGoBack; w > 0; w--)
				{
					if (scorer != null) scorer.RatePlayer(p, theWeek);
					totPoints += p.Points;
					theWeek = theWeek.PreviousWeek(theWeek,false,false);
				}

				if (totPoints > 0 || ! SupressZeros)
				{
					var dr = dt.NewRow();
					if ( RenderToCsv )
						dr[ "Name" ] = p.PlayerName;
					else
						dr[ "Name" ] = p.ProjectionLink( Season );

					dr["Pos"] = p.PlayerPos;
					dr["Role"] = p.RoleOut();
					dr["RookieYr"] = p.RookieYear;
					dr["CurrTeam"] = p.TeamCode;
					dr["FT"] = p.Owner;
					dr["Age"] = p.PlayerAge();

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
					dt.Rows.Add(dr);
				}
			}
			ds.Tables.Add(dt);

			return ds;
		}

		public string RenderProjectedData(ArrayList playerList, string sHead, [Optional] string sortOrder, 
			IRatePlayers scorer )
		{
			//  Output the list
			Utility.Announce("PlayerListing " + sHead);
			var r = new SimpleTableReport {ReportHeader = sHead, ReportFooter = Season, DoRowNumbers = true};
			if (!string.IsNullOrEmpty(SubHeader)) r.SubHeader = SubHeader;

			var ds = LoadProjectedData(playerList, scorer);

			r.AddColumn(new ReportColumn("Name", "NAME", "{0,-15}"));
			r.AddColumn(new ReportColumn("Pos", "POS", "{0,9}"));
			r.AddColumn(new ReportColumn("Role", "ROLE", "{0,9}"));
			r.AddColumn(new ReportColumn("RookieYr", "ROOKIEYR", "{0,4}"));
			r.AddColumn(new ReportColumn("Team", "CURRTEAM", "{0,2}"));
			r.AddColumn(new ReportColumn("Opp", "OPPONENT", "{0,2}"));
			r.AddColumn(new ReportColumn("Opp", "OPPRATE", "{0,2}"));
			r.AddColumn(new ReportColumn("Spread", "SPREAD", "{0,5}"));
			r.AddColumn(new ReportColumn("Age", "AGE", "{0,2}"));
			r.AddColumn(new ReportColumn("Owner", "FT", "{0,2}"));
			if (LongStats)
			{
				r.AddColumn(new ReportColumn("Curr", "CURSCORES", "{0,5}"));
				r.AddColumn(new ReportColumn("Tot", "SCORES", "{0,5}"));
				r.AddColumn(new ReportColumn("Avg", "AVG", "{0:0.#0}"));
				r.AddColumn(new ReportColumn("Inj", "INJURY", "{0,5}"));
				r.AddColumn(new ReportColumn("YDp", "YDP", "{0,5}"));
				r.AddColumn(new ReportColumn("Tdp", "TDP", "{0,5}"));
				r.AddColumn(new ReportColumn("YDr", "YDR", "{0,5}"));
				r.AddColumn(new ReportColumn("Tdr", "TDR", "{0,5}"));
				r.AddColumn(new ReportColumn("YDc", "YDC", "{0,5}"));
				r.AddColumn(new ReportColumn("TDc", "TDC", "{0,5}"));
				r.AddColumn(new ReportColumn("Fg", "Fg", "{0,5}"));
			}
			r.AddColumn(new ReportColumn("Points", "POINTS", "{0,5}"));

			var dt = ds.Tables[0];
			dt.DefaultView.Sort = LongStats 
				? (string.IsNullOrEmpty(sortOrder) ? "Points DESC" : sortOrder) : "Points DESC";

			r.LoadBody(dt); //  just assigns the data table
			FileOut = string.Format("{0}{1}.htm", Utility.OutputDirectory(), sHead);
			r.RenderAsHtml( FileOut, true);

			if (RenderToCsv)
				r.RenderAsCsv("Starters-" + sHead);
			return FileOut;
		}

		public DataSet LoadProjectedData(ArrayList plyrList, IRatePlayers scorer)
		{
			var ds = new DataSet();
			var dt = new DataTable();
			var cols = dt.Columns;
			cols.Add("Name", typeof (String));
			cols.Add("Pos", typeof (String));
			cols.Add("Role", typeof (String));
			cols.Add("RookieYr", typeof (String));
			cols.Add("Age", typeof (String));
			cols.Add("Currteam", typeof (String));
			cols.Add("Opponent", typeof (String));
			cols.Add("Spread", typeof (String));
			cols.Add("OppRate", typeof (String));
			cols.Add("FT", typeof (String));

			if (LongStats)
			{
				cols.Add("CurScores", typeof (Int32));
				cols.Add("Scores", typeof (Int32));
				cols.Add("Avg", typeof (Decimal));
				cols.Add("Injury", typeof (Int32));
				cols.Add("Tdp", typeof (Int32));
				cols.Add("YDp", typeof (Int32));
				cols.Add("Tdr", typeof (Int32));
				cols.Add("TDc", typeof (Int32));
				cols.Add("YDr", typeof (Int32));
				cols.Add("YDc", typeof (Int32));
				cols.Add("Fg", typeof (Int32));
			}

			cols.Add("Points", typeof (Decimal));

			if (Season == null) Season = Utility.CurrentSeason();

			foreach (NFLPlayer p in plyrList)
			{
				if (p.TotStats == null) p.LoadPerformances(false, true, Season); //  to get the stats

				//  rate the last whatevr weeks
				var theWeek = new NFLWeek(Int32.Parse(Season), Week, false); // start here
				var totPoints = 0M;
				if (scorer != null) scorer.RatePlayer(p, theWeek);
				totPoints += p.Points;

				if (totPoints > 0 || !SupressZeros)
				{
					var dr = dt.NewRow();
					dr["Name"] = p.PlayerName;
					dr["Pos"] = p.PlayerPos;
					dr["Role"] = p.RoleOut();
					dr["RookieYr"] = p.RookieYear;
					dr["CurrTeam"] = p.TeamCode;
					dr["Opponent"] = p.Opponent;
					dr["Spread"] = p.PlayerSpread;
					dr["OppRate"] = p.OppRate;
					dr["FT"] = p.Owner;
					dr["Age"] = p.PlayerAge();

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
					dt.Rows.Add(dr);
				}
			}
			ds.Tables.Add(dt);

			return ds;
		}
	}
}