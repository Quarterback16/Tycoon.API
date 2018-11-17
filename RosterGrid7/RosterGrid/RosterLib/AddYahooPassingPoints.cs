using System;


namespace RosterLib
{
	/// <summary>
	///  filter for calculating yahoo points
	/// </summary>
	public class AddYahooPassingPoints
	{
		public AddYahooPassingPoints( YahooProjectedPointsMessage input )
		{
			Process( input );
		}

		private void Process( YahooProjectedPointsMessage input )
		{
			input.Player.Points += input.PlayerGameMetrics.ProjTDp * 4;
			var yardagePts = Math.Floor( (decimal) input.PlayerGameMetrics.ProjYDp / 25 );
			input.Player.Points += yardagePts;
			//TODO:  -1 for Interceptions
			//TODO:  +2 per PAT pass
		}

	}
}
