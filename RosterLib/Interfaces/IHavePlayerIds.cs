using RosterLib.Models;
using System.Collections.Generic;

namespace RosterLib.Interfaces
{
	public interface IHavePlayerIds
	{
		IEnumerable<PlayerId> GetAll();

		string PlayerType();
	}
}
