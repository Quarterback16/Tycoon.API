using System.Collections.Generic;

namespace GameLogService.Model
{
	public class PlayerReportModel
	{
		public string Season { get; set; }
		public string PlayerName { get; set; }
		public List<GameStats> GameLog { get; set; }
	}
}
