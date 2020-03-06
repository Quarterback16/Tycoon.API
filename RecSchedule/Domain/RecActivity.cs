namespace RecSchedule.Domain
{
	public class RecActivity
	{
		public string Name { get; set; }
		public string Description { get; set; }

		public RecActivity()
		{
			Name = string.Empty;
			Description = "free";
		}
	}
}
