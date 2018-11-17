namespace RosterLib.RosterGridReports
{
	public interface IWinOrLose
	{
		NFLGame Game { get; set; }
		bool Home { get; set; }

		bool IsWinner { get; set; }

		decimal Margin { get; set; }
		NflTeam Team { get; set; }
	}
}