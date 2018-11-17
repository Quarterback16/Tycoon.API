﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RosterLib
{
	public class GoallineReport : IHtmlReport
	{
		#region  Properties

		public string FileOut { get; set; }

		public Dictionary<string, NflTeam> TeamList { get; set; }

		public string Season { get; set; }

		public string Week { get; set; }

		#endregion

		public void Render()
		{
			LoadTeams();

			var heading = Week == null ? "GL Scores Season " + Season : "Scores : Week " + Week;
			var str = new SimpleTableReport( heading );
			str.ColumnHeadings = true;
			str.DoRowNumbers = true;
			str.AddColumn( new ReportColumn( "Team", "TEAM", "{0,-20}" ) );
			str.AddColumn( new ReportColumn( "Total", "TOTAL", "{0}" ) );
			AddWeeklyColumns( str );

			str.LoadBody( BuildTable() );
			FileOut = Week == null ? string.Format( "{0}{1}//Scores//GLScores.htm", Utility.OutputDirectory(), Season )
				: string.Format( "{0}{1}//Scores//GLScores-{2}.htm", Utility.OutputDirectory(), Season, Week );
			str.RenderAsHtml( FileOut, true );
		}

		private static void AddWeeklyColumns( SimpleTableReport str )
		{
			for ( int i = 1; i < 18; i++ )
			{
				var fieldName = string.Format( "Wk{0:0#}", i );
				str.AddColumn( new ReportColumn( fieldName, fieldName, "{0}" ) );
			}
		}

		private DataTable BuildTable()
		{
			var dt = new DataTable();
			var cols = dt.Columns;
			cols.Add( "TEAM", typeof( String ) );
			cols.Add( "TOTAL", typeof( Int32 ) );
			AddWeeklyReportCols( cols );

			var ds = Week == null ? Utility.TflWs.ScoresDs( Season ) : Utility.TflWs.ScoresDs( Season, Week );

			var dt2 = ds.Tables[ 0 ];

			foreach ( DataRow dr in dt2.Rows )
				IncrementTeamwith( dr );

			foreach ( KeyValuePair<string, NflTeam> team in TeamList )
			{
				DataRow dr = dt.NewRow();
				dr[ "TEAM" ] = team.Value.Name;
				dr[ "TOTAL" ] = team.Value.TotTDs;
				AddWeeklyScorers( dr, team );
				dt.Rows.Add( dr );
			}
			dt.DefaultView.Sort = "TOTAL DESC";
			return dt;
		}

		private void AddWeeklyReportCols( DataColumnCollection cols )
		{
			for ( int i = 1; i < 18; i++ )
			{
				var fieldName = string.Format( "Wk{0:0#}", i );
				cols.Add( fieldName, typeof( String ) );
			}
		}

		private void AddWeeklyScorers( DataRow dr, KeyValuePair<string, NflTeam> team )
		{
			var theTeam = team.Value;
			IDictionaryEnumerator myEnumerator = theTeam.MetricsHt.GetEnumerator();
			while ( myEnumerator.MoveNext() )
			{
				var scorer = myEnumerator.Value;
				var field = string.Format( "Wk{0}", myEnumerator.Key );
				dr[ field ] = scorer;
			}
		}

		private void IncrementTeamwith( DataRow dr )
		{
			// use public Hashtable MetricsHt to store scorers
			var scoreType = dr[ "SCORE" ].ToString();
			var distance = dr[ "DISTANCE" ].ToString();
			var teamCode = dr[ "TEAM" ].ToString();
			var weekNo = System.Convert.ToInt32( dr[ "WEEK" ].ToString() );

			if ( TeamList.ContainsKey( teamCode ) )
			{
				var t = TeamList[ teamCode ];

				if ( scoreType.Equals( Constants.K_SCORE_TD_RUN ) && distance == "1" )
				{
					if ( weekNo < 18 )
					{
						t.TotTDr++;
						t.TotTDs++;
						AddGoallineScorer( dr, t );
					}
				}
			}

		}

		private static void AddGoallineScorer( DataRow dr, NflTeam t )
		{
			var scorer = dr[ "PLAYERID1" ].ToString();
			var player = new NFLPlayer( scorer );
			scorer = player.PlayerNameShort;
			var htKey = dr[ "WEEK" ].ToString();  //  data is stored 01
			if ( t.MetricsHt.ContainsKey( htKey ) )
				AppendScorer( t, scorer, htKey );
			else
				t.MetricsHt.Add( htKey, scorer );
		}

		private static void AppendScorer( NflTeam t, string scorer, string htKey )
		{
			var oldScorer = t.MetricsHt[ htKey ];
			t.MetricsHt[ htKey ] = oldScorer + "<br>" + scorer;
		}

		private void LoadTeams()
		{
			var ds = Utility.TflWs.TeamsDs( Season );
			DataTable dt = ds.Tables[ "Team" ];
			TeamList = new Dictionary<string, NflTeam>();

			foreach ( DataRow dr in dt.Rows )
			{
				var t = new NflTeam( dr[ "TEAMID" ].ToString(), Season,
												 Int32.Parse( dr[ "WINS" ].ToString() ),
												 dr[ "TEAMNAME" ].ToString() );
				t.MetricsHt = new Hashtable();
				TeamList.Add( t.TeamCode, t );
			}
		}
	}
}
