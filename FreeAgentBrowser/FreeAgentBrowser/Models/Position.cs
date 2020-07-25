using System.Collections.Generic;

namespace FreeAgentBrowser.Models
{
	public class Position
	{
		public int PositionId { get; set; }
		public string PositionName { get; set; }
		public string Description { get; set; }
		public List<Player> Players { get; set; }

		public override string ToString()
		{
			return Description;
		}
	}

}
