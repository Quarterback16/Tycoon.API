using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RosterLib
{
	public interface IPlayerGameMetricsDao
	{
		PlayerGameMetrics Get( string playerCode, string gameCode );
		List<PlayerGameMetrics> GetWeek( string season, string week );
		void Save( PlayerGameMetrics pgm );
		void ClearGame( string gameKey );
	}
}
