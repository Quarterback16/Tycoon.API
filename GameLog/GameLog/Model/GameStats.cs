namespace GameLog.Model
{
	public class GameStats
	{
		public int Week { get; set; }
		public int RushingTds { get; set; }
		public int PassingTds { get; set; }
		public int ReceivingTds { get; set; }

		public override string ToString()
		{
			return $"Week:{Week:0#} {RushingTds}-{PassingTds}-{ReceivingTds}";
		}
	}

}
