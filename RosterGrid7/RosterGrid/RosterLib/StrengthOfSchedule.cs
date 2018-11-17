using System;
using System.Collections;
using System.Data;

namespace RosterLib
{
	/// <summary>
	///   An ranking of teams by "Victory Points".  VP are gained by beating a team 
	///   who has beaten other teams.  The number of VP is equal to the Current M_wins 
	///   for the opponent defeated.
	/// </summary>
	public class StrengthOfSchedule
	{
		private readonly ArrayList _teamList;
		private readonly string _season;

		private const string KFieldTeam = "TEAM";
		private const string KFieldSos = "SOS";
		private const string KFieldExpWins = "EXPWINS";
		private const string KFieldExpLosses = "EXPLOSSES";
		private const string KFieldWins = "WINS";
		private const string KFieldLosses = "LOSSES";
		private const string KFieldVariance = "VAR";

		public StrengthOfSchedule(string season)
		{
			//  Part 1 - Get the Teams for the season
			_season = season;
			var ds = Utility.TflWs.TeamsDs(season);
			var dt = ds.Tables["Team"];
			_teamList = new ArrayList();
			//  Part 2 - Iterate through the teams
			foreach (DataRow dr in dt.Rows)
			{
				var t = new NflTeam(dr["TEAMID"].ToString(), season,
				                    Int32.Parse(dr["WINS"].ToString()),
				                    dr["TEAMNAME"].ToString());
				t.StrengthOfSchedule();
				_teamList.Add(t);
				//break;  //  Only need one team to start with
			}
		}

		/// <summary>
		/// Renders the object as a simple HTML report.
		/// </summary>
		public void RenderAsHtml()
		{
			var str = new SimpleTableReport("Strength of Schedule") {ColumnHeadings = true, DoRowNumbers = true};
			str.AddColumn(new ReportColumn("Team", KFieldTeam, "{0,-20}"));
			str.AddColumn(new ReportColumn("SoS", KFieldSos, "{0}"));
			str.AddColumn(new ReportColumn("Exp W", KFieldExpWins, "{0}"));
			str.AddColumn(new ReportColumn("Exp L", KFieldExpLosses, "{0}"));
			str.AddColumn(new ReportColumn("Prev W", KFieldWins, "{0}"));
			str.AddColumn(new ReportColumn("Prev L", KFieldLosses, "{0}"));
			str.AddColumn(new ReportColumn("Var", KFieldVariance, "{0}"));
			str.LoadBody(BuildTable());
			str.RenderAsHtml(string.Format("{0}\\{1}\\StrengthOfSchedule.htm", Utility.OutputDirectory(), _season), true);
		}

		private DataTable BuildTable()
		{
			var dt = new DataTable();
			var cols = dt.Columns;
			cols.Add(KFieldTeam, typeof (String));
			cols.Add(KFieldSos, typeof (String));
			cols.Add(KFieldExpWins, typeof (Int32));
			cols.Add(KFieldExpLosses, typeof (Int32));
			cols.Add(KFieldWins, typeof (Int32));
			cols.Add(KFieldLosses, typeof (Int32));
			cols.Add(KFieldVariance, typeof (Int32));
			foreach (NflTeam t in _teamList)
			{
				var sos = string.Format("{0:0.##0}", t.SoS);
				var dr = dt.NewRow();
				dr[KFieldTeam] = t.Name;
				dr[KFieldSos] = sos;
				dr[KFieldExpWins] = t.ExpWins;
				dr[KFieldExpLosses] = t.ExpLosses;
				dr[KFieldWins] = t.Wins;
				dr[KFieldLosses] = t.Losses;
				dr[KFieldVariance] = t.ExpWins - t.Wins;
				dt.Rows.Add(dr);
			}
			dt.DefaultView.Sort = KFieldSos + " ASC";
			return dt;
		}
	}
}