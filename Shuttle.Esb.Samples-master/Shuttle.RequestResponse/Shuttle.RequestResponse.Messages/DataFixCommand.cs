namespace Shuttle.RequestResponse.Messages
{
	public class DataFixCommand
	{
		public string TeamCode { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }

		public override string ToString()
		{
			return $"DataFix {FirstName} {LastName} ({TeamCode})";
		}
	}
}
