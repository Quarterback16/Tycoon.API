﻿namespace RosterLib
{
	public class DefensiveScorer
	{
		public string FileOut { get; set; }

		public void RenderDefensiveReports( NFLWeek week )
		{
			RenderDefensiveScoringReport( week );
			RenderTeamToDefendAgainst( week );
		}

		public void RenderTeamToDefendAgainst( NFLWeek week )
		{
			//  Team to beat report showing a jucy opponent for a particular defence
			var offset = -4;
			if ( week.WeekNo < 2 )
			{
				//  do the whole of last season
#if DEBUG
				Utility.Announce( "Looking back at the whole of last season" );
#endif
				offset = -Constants.K_WEEKS_IN_REGULAR_SEASON;
			}
			else
			{
#if DEBUG
				Utility.Announce( "Going back 4 games" );
#endif
			}

			var ttb = new TeamLister
			   {
					Heading = string.Format( "{0}\\defense\\Team To beat-{1:0#}", week.Season, week.WeekNo ),
					SubHeading = string.Format("last {0} weeks", offset)
			   };
			ICalculate ttbCalculator = 
				new DefensiveScoringCalculator( new NFLWeek( week.SeasonNo, week.WeekNo ), offset);
			FileOut = ttb.RenderTeamToBeat(ttbCalculator);
		}

		public void RenderDefensiveScoringReport( NFLWeek week )
		{
			var offset = -4;
			if ( week.WeekNo < 2 )
				//  do the whole of last season
				offset = - Constants.K_WEEKS_IN_REGULAR_SEASON;

			var tl = new TeamLister
			   {
			      Heading = string.Format("{0}\\defense\\Defensive Scoring-{1:0#}", week.Season, week.WeekNo ),
					SubHeading = string.Format( "last {0} weeks", offset )
			   };
			ICalculate myCalculator = new DefensiveScoringCalculator( week, offset );
			FileOut = tl.RenderTeams(myCalculator);
		}
	}
}
