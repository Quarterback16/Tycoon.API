using System.Collections.Generic;
using MvcJqGrid;
using Players.Models;

namespace Players.Services
{
	public class TflService
	{
		public List<Player> GetPlayers( GridSettings gridSettings )
		{
			var list = new List<Player>
				{
					new Player
						{
							PlayerId = 1,
							FirstName = "Joe",
							Surname = "Montana"
						},
					new Player
						{
							PlayerId = 2,
							FirstName = "Jerry",
							Surname = "Rice"
						},
				};
			return list;
		}

		public int CountPlayers( GridSettings gridSettings )
		{
			return 2;
		}
	}
}