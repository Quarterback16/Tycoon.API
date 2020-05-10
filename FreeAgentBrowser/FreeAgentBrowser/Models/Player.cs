namespace FreeAgentBrowser.Models
{
	public class Player
	{
		public int Id { get; set; }
		public int Number { get; set; }
		public string Name { get; set; }
		public string TeamCode { get; set; }

		public string ImageThumbnailUrl { get; set; }

		// make Position a category to map better to the Pie example
		public Position Position { get; set; }

		// this is highly volatile may be better as a calculation
		//public int Value { get; set; }
	}
}
