using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FreeAgentBrowser.Models
{
	public class PlayerRepository : IPlayerRepository
	{
		private readonly AppDbContext _appDbContext;

		public PlayerRepository(
			AppDbContext appDbContext)
		{
			_appDbContext = appDbContext;
		}

		public IEnumerable<Player> AllPlayers
		{
			get
			{
				return _appDbContext.Players
					.Include(
					  p => p.Position);
			}
		}

		public Player GetPlayerById(
			int playerId)
		{
			return _appDbContext.Players
				.FirstOrDefault(
					p => p.Id == playerId);
		}
	}
}
