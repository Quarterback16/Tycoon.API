using GameLogService;

using System;

namespace GameLog
{
	class Program
	{
		static void Main()
		{
			// enter year
			var season = "1984";
			// enter player name
			var playerName = "Joe Montana";
			// scrape it
			var service = new GameLogService.GameLogService();

			service.GameLogFor(
				season, 
				playerName);

			Console.ReadLine();
		}
	}
}
