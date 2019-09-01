using System;

namespace RosterLib
{
   /// <summary>
   ///   Calculates Yahoo points based on projections
   /// </summary>
   public class YahooProjectionScorer : IRatePlayers
	{
		public NFLWeek Week { get; set; }
		public bool ScoresOnly { get; set; }
		public string Name { get; set; }
		public XmlCache Master { get; set; }
		public bool AnnounceIt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public decimal RatePlayer(
			NFLPlayer plyr,
			NFLWeek week,
			bool takeCache = true)
		{
			// Points for Scores and points for stats
			if ( week.WeekNo.Equals( 0 ) ) return 0;

			Week = week;  //  set the global week, other wise u will get the same week all the time
			plyr.Points = 0;  //  start from scratch

			#region  Passing

			//  4 pts for a Tdp
			plyr.Points += plyr.ProjectedTDp * 4;
#if DEBUG
			Utility.Announce( $"{plyr.PlayerName} has {plyr.Points} points for Tdp" );
#endif
			//  1 pt / 25 YDp
			int ptsForYDp = (int) plyr.ProjectedYDp / 25;
			plyr.Points += ptsForYDp;
#if DEBUG
			Utility.Announce( $"{plyr.PlayerName} has {ptsForYDp} points for YDp" );
#endif

			#endregion

			#region  Catching

			//  6 pts for a TD catch
			decimal ptsForTDcatches = 6 * plyr.ProjectedTDc;
			plyr.Points += ptsForTDcatches;
#if DEBUG
			Utility.Announce( string.Format( "{0} has {1} points for TD catches", plyr.PlayerName, ptsForTDcatches ) );
#endif
			//  1 pt / 10 yds
			int ptsForYDs = (int) plyr.ProjectedYDc / 10;
			plyr.Points += ptsForYDs;
#if DEBUG
			Utility.Announce( string.Format( "{0} has {1} points for YDc", plyr.PlayerName, ptsForYDs ) );
#endif
			#endregion

			#region  Running

			//  6 points for TD run
			decimal ptsForTDruns = 6 * plyr.ProjectedTDr;
			plyr.Points += ptsForTDruns;
#if DEBUG
			Utility.Announce( string.Format( "{0} has {1} points for TD runs", plyr.PlayerName, ptsForTDruns ) );
#endif

			//  1 pt / 10 yds
			int ptsForYDr = (int) plyr.ProjectedYDr / 10;
			plyr.Points += ptsForYDr;
#if DEBUG
			Utility.Announce( string.Format( "{0} has {1} points for YDr", plyr.PlayerName, ptsForYDr ) );
#endif

			#endregion

			#region  Kicking

			plyr.Points += 3 * plyr.ProjectedFg;

			plyr.Points += plyr.ProjectedPat;

			#endregion

#if DEBUG
			Utility.Announce( $"{plyr.PlayerName} has {plyr.Points} in week {week.Season}:{week.Week}" );
#endif

			return plyr.Points;
		}

	}
}
