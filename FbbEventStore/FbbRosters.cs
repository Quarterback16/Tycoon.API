using System;
using System.Collections.Generic;

namespace FbbEventStore
{
	public class FbbRosters
	{
		public readonly List<FbbEvent> RosterMoves;

		public Dictionary<string, List<string>> Roster;

		public FbbRosters(IEventStore eventStore)
		{
			Roster = new Dictionary<string, List<string>>();
			RosterMoves = (List<FbbEvent>) 
				eventStore.Get<FbbEvent>("moves");
			CalculateRosters();
		}

		private void CalculateRosters()
		{
			foreach (var move in RosterMoves)
			{
				var fteam = move.FantasyTeam;
				if (!Roster.ContainsKey(fteam))
					Roster.Add(fteam, new List<string>());
				var direction = move.Direction;
				if (direction.Equals("IN"))
				{
					Roster[fteam].Add(move.Player);
				}
				if (direction.Equals("OUT"))
				{
					Roster[fteam].Remove(move.Player);
				}
			}
		}

		public string GetOwnerOf(string player)
		{
			var owner = "FA";
			foreach (KeyValuePair<string,List<string>> fteam in Roster)
			{
				foreach (string p in fteam.Value)
				{
					if (p.Equals(player))
					{
						owner = fteam.Key;
						break;
					}
				}
			}
			return owner;
		}

		public List<string> GetRoster(string fteam)
		{
			return Roster[fteam];
		}

		public void DumpRoster(string fteam)
		{
			Console.WriteLine($"Roster for {fteam}");
			var roster = Roster[fteam];
			foreach (var item in roster)
			{
				Console.WriteLine(item);
			}
		}

		public List<string> GetOwnersOf(string[] plyr)
		{
			var result = new List<string>();
			foreach (var item in plyr)
			{
				var owner = GetOwnerOf(item);
				result.Add($"{item,-25} is owned by {owner}");
			}
			return result;
		}
	}
}
