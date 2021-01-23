using System.Collections.Generic;

namespace GameLogService.Model
{
	public class PlayerReportModel
	{
		public string Season { get; set; }
		public string PlayerName { get; set; }
		public string Position { get; set; }
		public List<GameStats> GameLog { get; set; }

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
	}
}
