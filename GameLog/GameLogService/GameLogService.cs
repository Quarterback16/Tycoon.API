using GameLogService.Model;

namespace GameLogService
{
	public class GameLogService
	{
		private GameStatsRepository _repo;

		public GameLogService()
		{
			_repo = new GameStatsRepository();
		}

		public void GameLogFor(
			string season, 
			string playerName)
		{
			var model = new PlayerReportModel
			{
				Season = season,
				PlayerName = playerName
			};
			model.GameLog = _repo.GetGameStats(
				model);

			_repo.SendToConsole(
				model);
		}
	}
}
