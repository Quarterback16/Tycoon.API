using System;
using System.Collections.Generic;

namespace FbbEventStore
{
	public interface IRosterMaster
	{
		string GetOwnerOf(string player);
		List<string> GetRoster(string fteam);

		List<string> GetBatters(
			string fteam, 
			DateTime? asOf);

		List<string> GetPitchers(
			string fteam,
			DateTime? asOf);

		int JerseyNumber(
			string fteam,
			string playerName,
			bool isBatter );

		string GetMlbTeam(
			string playerName);

		string GetPosition(
			string playerName);
	}
}