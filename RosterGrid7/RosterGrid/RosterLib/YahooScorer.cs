using System;
using System.Data;

namespace RosterLib
{
	/// <summary>
	/// Applys the Yahoo scoring rules to a player after the stats have been recorded.
	/// </summary>
	public class YahooScorer : IRatePlayers
	{
		public YahooScorer( NFLWeek week )
		{
			Name = "Yahoo Scorer";
			Week = week;
		}

		#region IRatePlayers Members

		public bool ScoresOnly { get; set; }

		public Decimal RatePlayer(NFLPlayer plyr, NFLWeek week )
		{
			// Points for Scores and points for stats
			if (week.WeekNo.Equals(0)) return 0;

			Week = week;  //  set the global week, other wise u will get the same week all the time
			plyr.Points = 0;  //  start from scratch

			#region  Passing

			//  4 pts for a Tdp
            plyr.Points += PointsFor(plyr, 4, RosterLib.Constants.K_SCORE_TD_PASS, id: "2");
#if DEBUG
			RosterLib.Utility.Announce( string.Format( "{0} has {1} points for Tdp", plyr.PlayerName, plyr.Points ) );
#endif
			//  2 pts for a PAT pass
            int ptsForPATpasses = PointsFor(plyr, 2, RosterLib.Constants.K_SCORE_PAT_PASS, id: "2");
			plyr.Points += ptsForPATpasses;
#if DEBUG
			RosterLib.Utility.Announce( string.Format( "{0} has {1} points for PAT passes", plyr.PlayerName, ptsForPATpasses ) );
#endif
			//  1 pt / 25 YDp
			int ptsForYDp = PointsForStats( plyr:  plyr, increment : 1, forStatType : RosterLib.Constants.K_STATCODE_PASSING_YARDS, divisor :25.0M );
			plyr.Points += ptsForYDp;
#if DEBUG
			RosterLib.Utility.Announce( string.Format( "{0} has {1} points for YDp", plyr.PlayerName, ptsForYDp ) );
#endif
			//  -2 pts for an Interception
            int ptsForInts = PointsForStats(plyr: plyr, increment: -2, forStatType: RosterLib.Constants.K_STATCODE_INTERCEPTIONS_THROWN, divisor: 1.0M);
			plyr.Points += ptsForInts;
#if DEBUG
			RosterLib.Utility.Announce( string.Format( "{0} has {1} points for Interceptions", plyr.PlayerName, ptsForInts ) );
#endif		   
			#endregion

			#region  Catching

			//  6 pts for a TD catch
			int ptsForTDcatches = PointsFor( plyr, 6, RosterLib.Constants.K_SCORE_TD_PASS, id :"1" );
			plyr.Points += ptsForTDcatches;
#if DEBUG
			RosterLib.Utility.Announce( string.Format( "{0} has {1} points for TD catches", plyr.PlayerName, ptsForTDcatches ) );
#endif
			//  2 points for a 2 point conversion
			var ptsForPATcatches = PointsFor( plyr, 2, RosterLib.Constants.K_SCORE_PAT_PASS, id :"1" );
			plyr.Points += ptsForPATcatches;
#if DEBUG
			RosterLib.Utility.Announce( string.Format( "{0} has {1} points for PAT catches", plyr.PlayerName, ptsForPATcatches ) );
#endif
		
			//  1 pt / 10 yds
			int ptsForYDs = PointsForStats( plyr: plyr, increment : 1, forStatType : RosterLib.Constants.K_STATCODE_RECEPTION_YARDS, divisor : 10.0M );
			plyr.Points += ptsForYDs;
#if DEBUG
			RosterLib.Utility.Announce( string.Format( "{0} has {1} points for YDc", plyr.PlayerName, ptsForYDs ) );
#endif		   
			#endregion

			#region  Running

			//  6 points for TD run
			var ptsForTDruns = PointsFor( plyr, 6, RosterLib.Constants.K_SCORE_TD_RUN, id :"1" );
			plyr.Points += ptsForTDruns;
#if DEBUG
			RosterLib.Utility.Announce( string.Format( "{0} has {1} points for TD runs", plyr.PlayerName, ptsForTDruns ) );
#endif

			int ptsForPaTruns = PointsFor( plyr, 2, RosterLib.Constants.K_SCORE_PAT_RUN, id :"1" );
			plyr.Points += ptsForPaTruns;
#if DEBUG
			RosterLib.Utility.Announce( string.Format( "{0} has {1} points for PAT runs", plyr.PlayerName, ptsForPaTruns ) );
#endif
		
			//  1 pt / 10 yds
			int ptsForYDr = PointsForStats( plyr: plyr, increment : 1, forStatType : RosterLib.Constants.K_STATCODE_RUSHING_YARDS, divisor : 10.0M );
			plyr.Points += ptsForYDr;
#if DEBUG
			RosterLib.Utility.Announce( string.Format( "{0} has {1} points for YDc", plyr.PlayerName, ptsForYDr ) );
#endif
			
			#endregion
			
			#region  Kicking

			plyr.Points += PointsFor( plyr, 3, RosterLib.Constants.K_SCORE_FIELD_GOAL, "1" );	
			
			plyr.Points += PointsFor( plyr, 1, RosterLib.Constants.K_SCORE_PAT, "1" );	

			#endregion

			//RosterLib.Utility.Announce( string.Format( "{0} has {1} in week {2}:{3}", 
			//                      plyr.PlayerName, plyr.Points, week.Season, week.Week ) );

			return plyr.Points;
		}

		public string Name { get; set; }

		public XmlCache Master
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public NFLWeek Week { get; set; }

		#endregion

		private int PointsForStats( NFLPlayer plyr, int increment, string forStatType, decimal divisor)
		{
			var qty = 0.0M;
			var points = 0;

			var ds = plyr.LastStats( forStatType, Week.WeekNo, Week.WeekNo, Week.Season );
			foreach (DataRow dr in ds.Tables[0].Rows)
			{
				qty = Decimal.Parse(dr["QTY"].ToString());
				var pts = Math.Floor(qty / divisor);
				points = Convert.ToInt32(pts) * increment;
			}

			switch (forStatType)
			{
				case Constants.K_STATCODE_PASSING_YARDS:
					plyr.ProjectedYDp = Convert.ToInt32(qty);
					break;

				case Constants.K_STATCODE_PASSES_CAUGHT:
					plyr.ProjectedReceptions = Convert.ToInt32(qty);
					break;

				case Constants.K_STATCODE_RUSHING_YARDS:
					plyr.ProjectedYDr = Convert.ToInt32(qty);
					break;

				case Constants.K_STATCODE_INTERCEPTIONS_THROWN:
					break;

				case Constants.K_STATCODE_RECEPTION_YARDS:
					break;

				default:
					Utility.Announce(string.Format("Unknown stat type {0}", forStatType));
					break;
			}
			return points;
		}

		private int PointsFor(NFLPlayer plyr, int increment, string forScoreType, string id )
		{
			DataSet ds = plyr.LastScores(forScoreType, Week.WeekNo, Week.WeekNo, Week.Season, id);
			int nScores = ds.Tables[0].Rows.Count;
			int points = nScores * increment;

			switch (forScoreType)
			{
				case RosterLib.Constants.K_SCORE_TD_PASS:
					if (plyr.PlayerCat.Equals( RosterLib.Constants.K_QUARTERBACK_CAT ) && ( id == "2") )
						plyr.ProjectedTDp = nScores;
					else
						plyr.ProjectedTDc = nScores;
					break;

				case RosterLib.Constants.K_SCORE_TD_RUN:
					plyr.ProjectedTDr = nScores;
					break;

				case RosterLib.Constants.K_SCORE_FIELD_GOAL:
					plyr.ProjectedFg = nScores;
					points = ScoreFG( table : ds.Tables[0] );
					break;

				case RosterLib.Constants.K_SCORE_PAT_PASS:
					break;

				case RosterLib.Constants.K_SCORE_PAT:
					break;

				case RosterLib.Constants.K_SCORE_PAT_RUN:
					break;

				default:
					RosterLib.Utility.Announce(string.Format("Unknown score type {0}", forScoreType));
					break;
			}
			return points;
		}

		private static int ScoreFG( DataTable table )
		{
			var pts = 0;
			foreach (DataRow dr in table.Rows)
			{
				if ( Int32.Parse( dr["DISTANCE"].ToString() ) > 49 )
					pts += 5;
				else if ( Int32.Parse( dr["DISTANCE"].ToString() ) > 39 )
					pts += 4;
				else
					pts += 3;
			}
			return pts;
		}
	}
}

