using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RosterLib
{
	/// <summary>
	///   This is a "Filter"
	/// </summary>
	public class LoadPlayerGameMetric
	{
		public LoadPlayerGameMetric( YahooProjectedPointsMessage input )
		{
			Process( input, new DbfPlayerGameMetricsDao() );
		}

		private void Process( YahooProjectedPointsMessage input, IPlayerGameMetricsDao dao )
		{
			input.PlayerGameMetrics = dao.Get( input.Player.PlayerCode, input.Game.GameKey() );
		}
	}
}
