using System.Collections.Generic;

namespace GameLogService.Model
{
	public class PlayerContributions
	{
		public Dictionary<string,GameStats> Contributions { get; }

		public PlayerContributions()
		{
			Contributions = new Dictionary<string, GameStats>();
		}

		public void Add( 
			Starter player,
			GameStats stats)
		{
			if (!Contributions.ContainsKey(player.Name))
			{
				Contributions.Add(player.Name, stats);
				return;
			}
			Contributions[player.Name].RushingTds += stats.RushingTds;
			Contributions[player.Name].PassingTds += stats.PassingTds;
			Contributions[player.Name].ReceivingTds += stats.ReceivingTds;
			Contributions[player.Name].FieldGoalsMade += stats.FieldGoalsMade;
			Contributions[player.Name].ExtraPointsMade += stats.ExtraPointsMade;
		}

		internal void Add(
			LineUp lineUp)
		{
			Add(
				lineUp.QB, 
				lineUp.QB.Stats);
			Add(
				lineUp.RB1,
				lineUp.RB1.Stats);
			Add(
				lineUp.RB2,
				lineUp.RB2.Stats);
			Add(
				lineUp.PR1,
				lineUp.PR1.Stats);
			Add(
				lineUp.PR2,
				lineUp.PR2.Stats);
			Add(
				lineUp.PR3,
				lineUp.PR3.Stats);
			Add(
				lineUp.Kicker,
				lineUp.Kicker.Stats);
		}
	}
}
