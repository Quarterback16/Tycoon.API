using NLog;
using RosterLib.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace RosterLib
{
	public class TeamRanker : IRankTeams
	{
		public string GameScope { get; set; }
		public IBreakdown Breakdowns { get; set; }

		public Hashtable RatingsHt { get; set; }

		public string FileOut { get; set; }

		public Logger Logger { get; set; }

		public string Week { get; set; }

		public IKeepTheTime TimeKeeper { get; set; }

		public static DateTime LastDateRanked { get; set; }

		public bool ForceReRank { get; set; }

		#region Constructors
		public TeamRanker( IKeepTheTime timeKeeper )
		{
			TimeKeeper = timeKeeper;
			RatingsHt = new Hashtable();
			Week = TimeKeeper.PreviousWeek();
			FileOut = $@"{Utility.OutputDirectory()}\\{
				TimeKeeper.CurrentSeason()
				}\\Metrics\\MetricTable-{Week:0#}.htm";
			Logger = LogManager.GetCurrentClassLogger();
			Breakdowns = new PreStyleBreakdown();
		}
		#endregion

		public void RankTeams( 
			DateTime when,
			string sortColumn = "YDr" )
		{
			if ( ForceReRank )
				Logger.Trace( "  Ranking forced" );
			else
			{
				if ( HaveAlreadyRated( when ) )
				{
					Logger.Trace(
						$"  Have already got Ratings for {when:d}" );
					LoadRatings( when );
					return;
				}
				else
					Logger.Trace( "  Generating ratings for {0:d}", when );
			}
#if DEBUG
			var stopwatch = new Stopwatch();
			stopwatch.Start();
#endif
			//  what season are we in
			var season = TimeKeeper.CurrentSeason( when );
			//  Get the teams for that season
			var teamList = new List<NflTeam>();
			LoadTeams( teamList, season, when );

			var metricTable = LoadMetrix(
				teamList: teamList,
				when: when);

			Rank( metricTable, "YDr DESC", UnitRatings.UnitRating.Ro );
#if !DEBUG2
			Rank(
				metricTable: metricTable,
				orderBy: "YDp DESC",
				unitrating: UnitRatings.UnitRating.Po);
			Rank( metricTable, "SAKa ASC", UnitRatings.UnitRating.Pp );
			Rank( metricTable, "SAK DESC", UnitRatings.UnitRating.Pr );
			Rank( metricTable, "YDra ASC", UnitRatings.UnitRating.Rd );
			Rank( metricTable, "IntRatio DESC", UnitRatings.UnitRating.Pd );
#endif
			LastDateRanked = when;
			DumpRatingsHt( when );
			UpdateMetricsWithRatings( metricTable );
			DumpMetricTable( metricTable, when, sortColumn );
			WriteRatings( metricTable, when );
			UpdateRatings( metricTable, season );
			//TODO:  Update the team table?
#if DEBUG
			Utility.StopTheWatch(
				stopwatch,
				$"Ranking teams : {when:d}");
#endif
		}

		public bool HaveAlreadyRated( DateTime when )
		{
			// get ratings
			var theSunday = TimeKeeper.GetSundayFor( when );
			var ds = Utility.TflWs.GetUnitRatings( when );
			return ds.Tables[ 0 ].Rows.Count > 0;
		}

		private void LoadRatings( DateTime when )
		{
			// load the ratings into RatingsHt for a particular date
			var ds = Utility.TflWs.GetUnitRatings( when );
			var dt = ds.Tables[ "uratings" ];
			foreach ( DataRow dr in dt.Rows )
			{
				RatingsHt.Add(
					dr[ "TEAMCODE" ].ToString(), 
					new UnitRatings( dr[ "RATINGS" ].ToString() ) );
			}
			LastDateRanked = when;
		}

		private static void UpdateRatings(
			DataTable dt,
			string season)
		{
			foreach ( DataRow dr in dt.Rows )
			{
				var po = dr[ "RYDp" ].ToString();
				var ro = dr[ "RYDr" ].ToString();
				var pp = dr[ "RSAKa" ].ToString();
				var pr = dr[ "RSAK" ].ToString();
				var rd = dr[ "RYDra" ].ToString();
				var pd = dr[ "RIntRatio" ].ToString();
				var ratings = string.Format( "{0}{1}{2}{3}{4}{5}", po, ro, pp, pr, rd, pd );
				var teamCode = dr[ "TEAM" ].ToString();
				Utility.TflWs.UpdateRatings( season, teamCode, ratings );
			}
		}

		private void WriteRatings( DataTable dt, DateTime when )
		{
			foreach ( DataRow dr in dt.Rows )
			{
				var po = dr[ "RYDp" ].ToString();
				var ro = dr[ "RYDr" ].ToString();
				var pp = dr[ "RSAKa" ].ToString();
				var pr = dr[ "RSAK" ].ToString();
				var rd = dr[ "RYDra" ].ToString();
				var pd = dr[ "RIntRatio" ].ToString();
				var ratings = string.Format( "{0}{1}{2}{3}{4}{5}", po, ro, pp, pr, rd, pd );
				var teamCode = dr[ "TEAM" ].ToString();

				var theSunday = TimeKeeper.GetSundayFor( when );
				Logger.Trace(
					"  Saving URATINGS for Sunday {0:d}",
					theSunday);
				Utility.TflWs.SaveUnitRatings( ratings, theSunday, teamCode );
			}
		}

		private void UpdateMetricsWithRatings( DataTable dt )
		{
			foreach ( DataRow dr in dt.Rows )
			{
				var teamCode = dr[ "TEAM" ].ToString();
				var poRank = GetRank(
					teamCode,
					UnitRatings.UnitRating.Po);
				dr["RYDp"] = poRank;
				var roRank = GetRank(
					teamCode,
					UnitRatings.UnitRating.Ro);
				dr["RYDr"] = roRank;
				var ppRank = GetRank(
					teamCode,
					UnitRatings.UnitRating.Pp);
				dr["RSAKa"] = ppRank;
				var prRank = GetRank(
					teamCode,
					UnitRatings.UnitRating.Pr);
				dr[ "RSAK" ] = prRank;
				var rdRank = GetRank(
					teamCode,
					UnitRatings.UnitRating.Rd);
				dr[ "RYDra" ] = rdRank;
				var pdRank = GetRank(
					teamCode,
					UnitRatings.UnitRating.Pd);
				dr["RIntRatio"] = pdRank; 
				dr["RPTS"] = Utility.RatingPts(
					$"{poRank}{roRank}{ppRank}{prRank}{rdRank}{pdRank}");
				dr.AcceptChanges();
			}
		}

		private string GetRank(
			string teamCode,
			UnitRatings.UnitRating unitRating)
		{
			var ur = ( UnitRatings ) RatingsHt[ teamCode ];
			return ur.RatingFor( unitRating );
		}

		private void DumpRatingsHt( DateTime when )
		{
			var myEnumerator = RatingsHt.GetEnumerator();
			var i = 0;
			Utility.Announce( string.Format( "\t-INDEX-\t-KEY-\t-VALUE-  Ranked at {0}", when.ToShortDateString() ) );
			while ( myEnumerator.MoveNext() )
			{
				Utility.Announce( $@"\t[{
					i++
					}]:\t{
					myEnumerator.Key
					}\t{
					myEnumerator.Value
					}\t{
					Utility.RatingPts(myEnumerator.Value.ToString())
					}" );
			}
		}

		private void Rank(
			DataTable metricTable,
			string orderBy,
			UnitRatings.UnitRating unitrating)
		{
			var rank = 0;
			metricTable.DefaultView.Sort = orderBy;
			foreach ( DataRowView drv in metricTable.DefaultView )
			{
				rank++;
				StoreRank(
					teamCode: drv["TEAM"].ToString(),
					rank: rank,
					unitRating: unitrating);
			}
		}

		private void StoreRank(
			string teamCode,
			int rank,
			UnitRatings.UnitRating unitRating)
		{
			var rating = RatingsFor( rank );
			UpdateRating( teamCode, unitRating, rating );
		}

		public static string RatingsFor( int rank )
		{
			if ( rank < 4 )
				return "A";
			if ( rank < 10 )
				return "B";
			if ( rank < 24 )
				return "C";
			return rank < 30 ? "D" : "E";
		}

		private void UpdateRating( string teamCode, UnitRatings.UnitRating unitRating, string rating )
		{
			var ur = new UnitRatings();

			if ( RatingsHt.ContainsKey( teamCode ) )
				ur = ( UnitRatings ) RatingsHt[ teamCode ];
			else
				RatingsHt.Add( teamCode, ur );

			switch ( unitRating )
			{
				case UnitRatings.UnitRating.Po:
					ur.PassOffence = rating;
					break;

				case UnitRatings.UnitRating.Ro:
					ur.RushOffence = rating;
					break;

				case UnitRatings.UnitRating.Pp:
					ur.PassProtection = rating;
					break;

				case UnitRatings.UnitRating.Pr:
					ur.PassRush = rating;
					break;

				case UnitRatings.UnitRating.Rd:
					ur.RunDefence = rating;
					break;

				case UnitRatings.UnitRating.Pd:
					ur.PassDefence = rating;
					break;
			}
			RatingsHt[ teamCode ] = ur;
		}

		public void LoadTeams(
			ICollection<NflTeam> teamList,
			string season,
			DateTime focusDate)
		{
			var teamDs = Utility.TflWs.TeamsDs( season );
			if ( teamDs.Tables[ 0 ].Rows.Count <= 0 ) return;

			var dt = teamDs.Tables[ "team" ];
			foreach ( DataRow dr in dt.Rows )
			{
				var teamCode = dr[ "TEAMID" ].ToString();

				Logger.Trace( $"      Tallying {teamCode}" );
				TallyTeam(
					teamList,
					season,
					focusDate,
					teamCode);
#if DEBUG2
				break;
#endif
			}
		}

		public void TallyTeam( 
			ICollection<NflTeam> teamList, 
			string season, 
			DateTime focusDate, 
			string teamCode )
		{
			var team = new NflTeam( teamCode );  //  simple code constructor
			if ( TimeKeeper.IsItRegularSeason() )
			{
				team.LoadGames( team.TeamCode, season );
				GameScope = GameScopeFromGameList(
					team.GameList);
			}
			else
			{
				GameScope = $@"Regular season Games {
					Int32.Parse(season)-1
					}";
				team.LoadPreviousRegularSeasonGames(
					team.TeamCode,
					season,
					focusDate);
			}
			team.TallyStats(Breakdowns);
			teamList.Add( team );
			var breakdownKey = $"{team.TeamCode}-Q";
			var breakdownFile = $@"{Utility.OutputDirectory()}\\{
							TimeKeeper.CurrentSeason()
							}\\Metrics\\breakdowns\\{breakdownKey}.htm";
			Breakdowns.Dump( 
				breakdownKey, 
				breakdownFile);
		}

		private string GameScopeFromGameList(ArrayList games)
		{
			string from = "";
			string to = "";
			for ( int i = 0; i < games.Count; i++ )
			{
				var g = ( NFLGame ) games[ i ];
				if ( string.IsNullOrEmpty( from ) ) from = g.GameDate.ToShortDateString();
				to = g.GameDate.ToShortDateString();
			}
			return $"Games From: {from} to {to}";
		}

		private DataTable LoadMetrix(
			IEnumerable<NflTeam> teamList,
			DateTime when)
		{
			var metricTable = BuildTable();
			foreach ( var nflTeam in teamList )
			{
				var dr = metricTable.NewRow();
				dr[ "TEAM" ] = nflTeam.TeamCode;
				//  Average gain per pass is a better indicator 
				dr["YDp"] = GainPerPassAttempt(
					nflTeam.TotYdp,
					nflTeam.TotPasses);
				dr[ "YDr" ] = nflTeam.TotYdr;
				dr[ "SAKa" ] = nflTeam.TotSacksAllowed;
				dr[ "SAK" ] = nflTeam.TotSacks;
				dr[ "YDra" ] = nflTeam.TotYdrAllowed;
				dr[ "TDpa" ] = nflTeam.TotTDpAllowed;
				dr[ "INT" ] = nflTeam.TotIntercepts;
				dr[ "IntRatio" ] = InterceptionRatio( 
					nflTeam.TotIntercepts, 
					nflTeam.TotTDpAllowed );
				//dr[ "RPTS" ] = nflTeam.RatingPts();
				metricTable.Rows.Add( dr );
			}
			//DumpMetricTable( metricTable, when );
			return metricTable;
		}

		private decimal GainPerPassAttempt(
			int totYdp, 
			int totPasses)
		{
			if (totPasses.Equals(0))
				return 0.0M;
			return FormatDecimal(
				totYdp,
				totPasses);
		}

		public static decimal InterceptionRatio(
			int interceptions,
			int touchDownPassesAllowed)
		{
			if (touchDownPassesAllowed.Equals(0)) return interceptions;
			return FormatDecimal(
				interceptions, 
				touchDownPassesAllowed);
		}

		private static decimal FormatDecimal(
			int quotient,
			int divisor)
		{
			var rawDecimal = (decimal)quotient / divisor;
			var formatted = rawDecimal.ToString("0.##");
			return Decimal.Parse(formatted);
		}

		private void DumpMetricTable(
			DataTable dt,
			DateTime when,
			string sortColumn = "YDr")
		{
			var st =
				new SimpleTableReport( 
					$@"Team Metrics at {
						when.ToShortDateString()
						} {GameScope}" )
				{ ColumnHeadings = true };
			st.AddColumn( new ReportColumn( "Team", "TEAM", "{0,-20}" ) );
			st.AddColumn( new ReportColumn( "YDp/A", "YDp", "{0:0.00}" ) );
			st.AddColumn( new ReportColumn( "RYDp", "RYDp", "{0}" ) );
			st.AddColumn( new ReportColumn( "YDr", "YDr", "{0}" ) );
			st.AddColumn( new ReportColumn( "RYDr", "RYDr", "{0}" ) );
			st.AddColumn( new ReportColumn( "SAKa", "SAKa", "{0}" ) );
			st.AddColumn( new ReportColumn( "RSAKa", "RSAKa", "{0}" ) );
			st.AddColumn( new ReportColumn( "SAK", "SAK", "{0}" ) );
			st.AddColumn( new ReportColumn( "RSAK", "RSAK", "{0}" ) );
			st.AddColumn( new ReportColumn( "YDra", "YDra", "{0}" ) );
			st.AddColumn( new ReportColumn( "RYDra", "RYDra", "{0}" ) );
			st.AddColumn( new ReportColumn( "INT", "INT", "{0}" ) );
			st.AddColumn( new ReportColumn( "TDpa", "TDpa", "{0}" ) );
			st.AddColumn( new ReportColumn( "IntRatio", "IntRatio", "{0}" ) );
			st.AddColumn( new ReportColumn( "RIntRatio", "RIntRatio", "{0}" ) );
			st.AddColumn( new ReportColumn( "RPoints", "RPTS", "{0}" ) );
			dt.DefaultView.Sort = $"{sortColumn} DESC";
			st.LoadBody( dt );
			st.RenderAsHtml( FileOut, true );
		}

		private static DataTable BuildTable()
		{
			var dt = new DataTable();
			var cols = dt.Columns;
			cols.Add( "TEAM", typeof( String ) );
			cols.Add( "YDp", typeof( Decimal ) );
			cols.Add( "RYdp", typeof( String ) );
			cols.Add( "YDr", typeof( Int32 ) );
			cols.Add( "RYDr", typeof( String ) );
			cols.Add( "SAKa", typeof( Decimal ) );
			cols.Add( "RSAKa", typeof( String ) );
			cols.Add( "SAK", typeof( Decimal ) );
			cols.Add( "RSAK", typeof( String ) );
			cols.Add( "YDra", typeof( Int32 ) );
			cols.Add( "RYDra", typeof( String ) );
			cols.Add( "TDpa", typeof( Int32 ) );
			cols.Add( "RTDpa", typeof( String ) );
			cols.Add( "INT", typeof( Int32 ) );
			cols.Add( "IntRatio", typeof( Decimal ) );
			cols.Add( "RIntRatio", typeof( String ) );
			cols.Add( "RPTS", typeof( String ) );
			return dt;
		}
	}
}