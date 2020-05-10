using FreeAgentBrowser.Models;
using System.Collections.Generic;

namespace FreeAgentBrowser.ViewModels
{
	public class PlayerListViewModel
	{
		public IEnumerable<Player> Players { get; set; }
		public string CurrentPosition { get; set; }
	}
}
