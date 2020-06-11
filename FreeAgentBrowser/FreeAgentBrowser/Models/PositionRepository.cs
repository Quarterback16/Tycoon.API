using System.Collections.Generic;

namespace FreeAgentBrowser.Models
{
	public class PositionRepository : IPositionRepository
	{
		private readonly AppDbContext _appDbContext;

		public PositionRepository(
			AppDbContext appDbContext)
		{
			_appDbContext = appDbContext;
		}

		public IEnumerable<Position> AllPositions => _appDbContext.Positions;
	}
}
