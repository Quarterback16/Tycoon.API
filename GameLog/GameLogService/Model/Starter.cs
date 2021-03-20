namespace GameLogService.Model
{
	public class Starter
	{
		public string Name { get; set; }
		public GameStats Stats { get; set; }

		public Starter()
		{
			Name = "open";
			Stats = new GameStats();
		}

		public Starter(
			string name,
			GameStats stats)
		{
			Name = name;
			Stats = stats;
		}
		public override string ToString()
		{
			return $"{Name,-17} ({Stats.Scores()})";
		}
	}
}
