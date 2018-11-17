using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RosterLib
{
	public class YahooCalculator
	{
		//  uses a pipeline to process a YahooProjectedPointsMessage
		public PipeLine<YahooProjectedPointsMessage> yahooPipeline { get; set; }

		public YahooProjectedPointsMessage Calculate( NFLPlayer p, NFLGame g )
		{
			if ( yahooPipeline == null ) InitialiseThePipeLine();

			var msg = new YahooProjectedPointsMessage();
			msg.Player = p;
			msg.Player.Points = 0;
			msg.Game = g;

			yahooPipeline.Execute( msg );
			return msg;
		}

		private void InitialiseThePipeLine()
		{
			yahooPipeline = new PipeLine<YahooProjectedPointsMessage>();
			yahooPipeline.Register( msg => new LoadPlayerGameMetric( msg ) );
			yahooPipeline.Register( msg => new AddYahooPassingPoints( msg ) );
		}
	}
}
