using System.Collections.Generic;

namespace FreeAgentBrowser.Models
{
	public class MockPositionRepository : IPositionRepository
	{
		public IEnumerable<Position> AllPositions =>
			new List<Position>
			{
				new Position
				{
					PositionId = 1,
					PositionName = "QB",
					Description = "Quarterback",
				},
				new Position
				{
					PositionId = 2,
					PositionName = "RB",
					Description = "Running back",
				},
				new Position
				{
					PositionId = 3,
					PositionName = "WR",
					Description = "Wide Receiver",
				},
				new Position
				{
					PositionId = 4,
					PositionName = "TE",
					Description = "Tight End",
				},
				new Position
				{
					PositionId = 5,
					PositionName = "PK",
					Description = "Place Kicker",
				},
			};
	}
}
