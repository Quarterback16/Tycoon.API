using System;
using System.Collections.Generic;

namespace RosterService
{
	public class RetroRosters : IRosterService
	{
		public readonly List<RosterEvent> RosterMoves;

		public Dictionary<string, List<string>> Roster;

		public RetroRosters(
			IEventStore eventStore)
		{
			Roster = new Dictionary<string, List<string>>();
			RosterMoves = (List<RosterEvent>)
				eventStore.Get<RosterEvent>("moves");
			CalculateRosters();
		}

		private void CalculateRosters()
		{
			foreach (var move in RosterMoves)
			{
				//if (move.Player == "Theotis Brown")
				//	Console.WriteLine(move.Player);
				var fteam = move.FantasyTeam;
				if (!Roster.ContainsKey(fteam))
					Roster.Add(fteam, new List<string>());
				var direction = move.Direction;
				if (direction.Equals("IN"))
				{
					Roster[fteam].Add(
						move.ToLine());
				}
				if (direction.Equals("OUT"))
				{
					foreach (var player in Roster[fteam])
					{
						if (player.Contains(move.Player))
						{
							Roster[fteam].Remove(player);
							break;
						}
					}
				}
			}
		}

		public string GetOwnerOf(
			string player,
			string noOwner = "FA")
		{
			var owner = noOwner;
			foreach (KeyValuePair<string, List<string>> fteam in Roster)
			{
				foreach (string p in fteam.Value)
				{
					if (p.Contains(player))
					{
						owner = fteam.Key;
						break;
					}
				}
			}
			return owner;
		}

		public int GetPriceOf(
			string player)
		{
			var price = 0;
			foreach (var move in RosterMoves)
			{
				var direction = move.Direction;
				if (direction.Equals("IN"))
				{
					if (move.Player.Equals(player))
						price = Int32.Parse(move.Price);
				}
			}
			return price;
		}

		public int GetIdOf(
			string player)
		{
			var id = 0;
			foreach (var move in RosterMoves)
			{
				if (move.Player.Equals(player))
					id = Int32.Parse(move.Number);
			}
			return id;
		}

		public List<string> GetRoster(string fteam)
		{
			return Roster[fteam];
		}
	}
}
