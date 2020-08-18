using System.Collections.Generic;

namespace RosterService
{
	public interface IRosterService
	{
		string GetOwnerOf(
			string player,
			string noOwner);

		List<string> GetRoster(
			string fteam);

		int GetPriceOf(
			string player);

		int GetIdOf(
			string player);
	}
}
