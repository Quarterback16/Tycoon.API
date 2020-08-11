namespace RosterService
{
	public static class Utility
	{
		public static string FantasyTeamName(
			string abbr)
		{
			var name = "???";
			switch (abbr)
			{
				case "CD":
					name = "Colonna's DeLoreans";
					break;
				case "DD":
					name = "Drakesboro Duck Hunters";
					break;
				case "LL":
					name = "Lithuania Lightning";
					break;
				case "SR":
					name = "Shannahan's Revival";
					break;
				case "RR":
					name = "Roslin Rhinos";
					break;
				case "CG":
					name = "Chepstow Galaxy";
					break;
				case "SF":
					name = "Send on the Fridge";
					break;
				case "BR":
					name = "Black County Raiders";
					break;
				default:
					break;
			}
			return name;
		}
	}
}
