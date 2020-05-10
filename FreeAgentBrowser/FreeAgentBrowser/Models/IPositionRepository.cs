using System.Collections.Generic;

namespace FreeAgentBrowser.Models
{
	public interface IPositionRepository
	{
		IEnumerable<Position> AllPositions { get; }
	}
}
