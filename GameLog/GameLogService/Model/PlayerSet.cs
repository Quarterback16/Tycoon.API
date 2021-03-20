using System;
using System.Collections.Generic;

namespace GameLogService.Model
{
	public class PlayerSet
	{
		public List<Starter> Players { get; }
		public PlayerSet()
		{
			Players = new List<Starter>();
		}
		public void Add(Starter anotherPlayer)
		{
			Players.Add(anotherPlayer);
		}

		internal Starter At(int index)
		{
			if (index > Count() - 1)
				return new Starter();
			if (string.IsNullOrEmpty(Players[index].Name))
				return new Starter();
			return Players[index];
		}

		internal int Count()
		{
			return Players.Count;
		}

		internal int Scorers()
		{
			var scorers = 0;
			foreach (var player in Players)
			{
				if (player.Stats.Scores() > 0)
					scorers++;
			}
			return scorers;
		}
	}
}
