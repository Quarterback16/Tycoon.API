using System.Collections.Generic;

namespace RosterService
{
	public interface IRosterService
	{
		string GetOwnerOf(
			string player);

		List<string> GetRoster(
			string fteam);

	}
}
