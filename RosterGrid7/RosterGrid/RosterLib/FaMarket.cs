using System;
using System.Collections;
using System.Data;
using System.Linq;

namespace RosterLib
{
	/// <summary>
	/// Summary description for FaMarket.  (Based on Victory Points report.)
	/// </summary>
	public class FaMarket
	{
		public string FileOut = "";

		private readonly string _mSeason;
		private readonly ArrayList _mTeamList;

		public FaMarket(string season)
		{
			_mSeason = season;
			var ds = Utility.TflWs.TeamsDs(season);
			var dt = ds.Tables["Team"];
			_mTeamList = new ArrayList();
			foreach ( var t in from DataRow dr in dt.Rows 
									 select dr["TEAMID"].ToString() into teamCode 
									 select new NflTeam( teamCode ) )
			{
				t.CountFaPoints(season);
				_mTeamList.Add(t);
			}
		}

		public void RenderAsHtml()
		{
			//  Use a simple table output to show the rankings
			var str = new SimpleTableReport
			                        	{
			                        		ReportHeader = "Market Analysis " + _mSeason,
			                        		ColumnHeadings = true,
			                        		DoRowNumbers = true
			                        	};
			str.AddColumn(new ReportColumn("Team", "TEAM", "{0,-20}"));
			str.AddColumn(new ReportColumn("FA Points", "FA", "{0,5}"));
			str.AddColumn(new ReportColumn("In", "IN", "{0,5}"));
			str.AddColumn(new ReportColumn("Got", "GOT", "{0}"));
			str.AddColumn(new ReportColumn("Out", "OUT", "{0,5}"));
			str.AddColumn(new ReportColumn("Lost", "LOST", "{0}"));
			str.AddColumn(new ReportColumn("Net", "NET", "{0}"));
			str.LoadBody(BuildTable());
			FileOut = string.Format( "{0}{1}\\FreeAgentMarket\\faMarket.htm",
			                         Utility.OutputDirectory(), _mSeason );
			str.RenderAsHtml( FileOut, true);
		}

		private DataTable BuildTable()
		{
			var dt = new DataTable();
			var cols = dt.Columns;
			cols.Add("TEAM", typeof (String));
			cols.Add("FA", typeof (Int32));
			cols.Add("IN", typeof (Int32));
			cols.Add("GOT", typeof (String));
			cols.Add("OUT", typeof (Int32));
			cols.Add("LOST", typeof (String));
			cols.Add("NET", typeof (Int32));

			foreach (NflTeam t in _mTeamList)
			{
				var dr = dt.NewRow();
				dr["TEAM"] = t.Name;
				dr["FA"] = t.FaPoints;
				dr["IN"] = t.PlayersIn;
				dr["GOT"] = t.PlayersGot;
				dr["OUT"] = t.PlayersOut;
				dr["LOST"] = t.PlayersLost;
				dr["NET"] = t.PlayersIn - t.PlayersOut;
				dt.Rows.Add(dr);
			}
			dt.DefaultView.Sort = "FA DESC";
			return dt;
		}
	}
}