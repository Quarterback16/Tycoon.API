using System;
using System.Collections.Generic;

namespace GameLogService.Model
{
	public class PlayerReportModel
	{
		public string Season { get; set; }
		public string PlayerName { get; set; }
		public string Position { get; set; }
		public List<GameStats> GameLog { get; set; }

		public PlayerReportModel()
		{
		}

		public PlayerReportModel(
			string season,
			string playerName,
			string position)
		{
			Season = season;
			PlayerName = playerName;
			Position = position;

			if (PlayerName.Equals("Gary Anderson"))
				Position = "RB";
		}

		public bool IsKicker()
		{
			if (GameLog != null)
			{
				foreach (var game in GameLog)
				{
					if (game.FieldGoalsMade > 0
						|| game.ExtraPointsMade > 0)
						return true;
				}
			}
			return false;
		}

		public override string ToString()
		{
			return $"{PlayerName} games:{GameLogCout()}";
		}

		private int GameLogCout()
		{
			if (GameLog == null)
				return 0;
			return GameLog.Count;
		}
	}
}
