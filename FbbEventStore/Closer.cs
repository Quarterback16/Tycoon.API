namespace FbbEventStore
{
	public class Closer
	{
		public Closer(string v1, string v2, string v3)
		{
			Name = v1;
			Team = v2;
			Hold = v3;
		}

		public string Name { get; set; }
		public string Team { get; set; }
		public string Hold { get; set; }

		public override string ToString()
		{
			return $"{Name}, {Team} : {Hold}";
		}
	}
}
