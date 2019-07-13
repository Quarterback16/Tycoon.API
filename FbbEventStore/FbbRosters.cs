using System;
using System.Collections.Generic;

namespace FbbEventStore
{
	public class FbbRosters : IRosterMaster
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

		public string GetMlbTeam(string playerName)
		{
			var mlbTeam = "???";
			foreach (var move in RosterMoves)
			{
				if (move.Player.Equals(playerName))
				{
					mlbTeam = move.MlbTeam;
					break;
				}
			}
			return mlbTeam;
		}

		public string GetPosition(string playerName)
		{
			var position = "???";
			foreach (var move in RosterMoves)
			{
				if (move.Player.Equals(playerName))
				{
					position = move.Position;
					break;
				}
			}
			return position;
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

		public List<string> GetBatters(
			string fteam,
			DateTime? asOf)
		{
			return GetPlayers(
				isBatters: true, 
				fteam, 
				ref asOf);
		}

		public List<string> GetPitchers(
			string fteam,
			DateTime? asOf)
		{
			return GetPlayers(
				isBatters: false,
				fteam,
				ref asOf);
		}

		private List<string> GetPlayers(
			bool isBatters,
			string fteam, 
			ref DateTime? asOf)
		{
			if (asOf == null)
				asOf = DateTime.Now.Date;

			Roster.Clear();
			foreach (var move in RosterMoves)
			{
				if (string.IsNullOrEmpty(move.TransactionDate))
					move.TransactionDate = "2019-04-01";

				if (move.FantasyTeam != fteam)
					continue;
				if (DateTime.Parse(move.TransactionDate) > asOf)
					continue;

				if (!Roster.ContainsKey(fteam))
					Roster.Add(fteam, new List<string>());
				if (IsBatter(move.Position).Equals(isBatters))
				{
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
			return Roster[fteam];
		}

		public int JerseyNumber(
			string fteam,
			string playerName,
			bool isBatter)
		{
			var jerseyNumber = 0;
			Roster.Clear();
			foreach (var move in RosterMoves)
			{
				if (string.IsNullOrEmpty(move.TransactionDate))
					move.TransactionDate = "2019-04-01";

				if (move.FantasyTeam != fteam)
					continue;

				if (!Roster.ContainsKey(fteam))
					Roster.Add(fteam, new List<string>());
				if (isBatter)
				{
					if (IsBatter(move.Position))
					{
						var direction = move.Direction;
						if (direction.Equals("IN"))
						{
							jerseyNumber++;
							Roster[fteam].Add(move.Player);
							if (move.Player.Equals(playerName))
								return jerseyNumber;
						}
					}
				}
				else
				{
					if (!IsBatter(move.Position))
					{
						var direction = move.Direction;
						if (direction.Equals("IN"))
						{
							jerseyNumber++;
							Roster[fteam].Add(move.Player);
							if (move.Player.Equals(playerName))
								return jerseyNumber;
						}
					}
				}
			}
			return jerseyNumber;
		}

		private bool IsBatter(string position)
		{
			var isBatter = true;
			if (position.Equals("SP")
				|| position.Equals("RP"))
				isBatter = false;
			return isBatter;
		}

		public void DumpRoster(string fteam)
		{
			Console.WriteLine($"Roster for {fteam}");
			var roster = Roster[fteam];
			var i = 0;
			foreach (var item in roster)
			{
				i++;
				Console.WriteLine( $"{i,2} {item}");
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
