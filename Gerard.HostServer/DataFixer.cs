using Gerard.Messages;

namespace Gerard.HostServer
{
	//TODO: Railway Oriented code to perform fix
	// 1. get the player
	// 2. check if data is missing
	// 3. Get Statleship data
	// 4. Update Tfl database

	public class DataFixer
	{
		private readonly ITflService TflService;

		public ILog Log { get; set; }

		public DataFixer(
			ITflService tflService,
			ILog logger)
		{
			Log = logger;
			TflService = tflService;
		}

		public bool ApplyFix( DataFixCommand dataFixCommand )
		{
			var result = false;
			var player = GetPlayer(dataFixCommand);
			if (player == null)
			{
				Log.Info($"  x Player Not Found {dataFixCommand} in TFL db");
				return false;
			}
			else
				Log.Info($"Player{dataFixCommand} equates to {player}");

			if (!player.IsMissingDob())
			{
				Log.Info($"  x Player DOB is present {dataFixCommand}:{player.DBirth} in TFL db");
				return false;
			}
			else
				Log.Info($"    Player {dataFixCommand} is missing DOB");

			var statPlayer = GetStatleshipPlayer(dataFixCommand);
			if (statPlayer == null)
			{
				Log.Info($"  x Could not find Player {dataFixCommand} at Stattleship");
				return false;
			}
			else if (statPlayer.BirthDate == new System.DateTime(1, 1, 1))
			{
				Log.Info($"  x Could not find a birthdate {dataFixCommand} at Stattleship");
				return false;
			}
			else
				Log.Info($"Birthdate for {dataFixCommand} is {statPlayer.BirthDate:yyyy-MM-dd}");

			PutDob(player, statPlayer);
			return result;
		}

		public bool PutDob(
			NFLPlayer player,
			Player statPlayer)
		{
			TflService.UpdateDob(
				player,
				statPlayer.BirthDate);
			Log.Info($"SUCCESS: {player} dob set to {statPlayer.BirthDate:yyyy-MM-dd}");
			return true;
		}

		public Player GetStatleshipPlayer(DataFixCommand dataFixCommand)
		{
			var query = new PlayerQuery(
				teamCode: dataFixCommand.TeamCode,
				firstName: dataFixCommand.FirstName,
				lastName: dataFixCommand.LastName);
			var sut = new PlayerQueryHandler();
			var player = sut.Handle(query);
			return player;
		}

		public NFLPlayer GetPlayer(DataFixCommand command)
		{
			return TflService.GetNflPlayer(
				command.FirstName,
				command.LastName);
		}
	}
}