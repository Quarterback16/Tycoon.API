using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace RosterLib
{

	public class PlayerProjectionGenerator
	{
		public PipeLine<PlayerGameProjectionMessage> pipeline { get; set; }

		public void Execute( NFLGame game )
		{
			if ( pipeline == null ) InitialiseThePipeLine();

			var msg = new PlayerGameProjectionMessage();
			msg.Game = game;
			msg.Game.PlayerGameMetrics = new List<PlayerGameMetrics>();
			pipeline.Execute( msg );

		}

		private void InitialiseThePipeLine()
		{
			pipeline = new PipeLine<PlayerGameProjectionMessage>();
			pipeline.Register( msg => new GetGamePrediction( msg ) );
			pipeline.Register( msg => new ClearGameMetrics( msg ) );
			pipeline.Register( msg => new PullMetricsFromPrediction( msg ) );
			pipeline.Register( msg => new SavePlayerGameMetrics( msg ) );
		}

	}



	public class PlayerGameProjectionMessage
	{
		public NFLPlayer Player { get; set; }
		public NFLGame Game { get; set; }
		public NFLResult Prediction { get; set; }
		public PlayerGameMetrics PlayerGameMetrics { get; set; }
		public IPlayerGameMetricsDao Dao { get; set; }
	}
}
