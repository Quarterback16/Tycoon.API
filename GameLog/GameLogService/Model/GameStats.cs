using System;

namespace GameLogService.Model
{
	public class GameStats
	{
		public int Week { get; set; }
		public int RushingTds { get; set; }
		public int PassingTds { get; set; }
		public int ReceivingTds { get; set; }
		public int FieldGoalsMade { get; set; }
		public int ExtraPointsMade { get; set; }


		public string KickerStats()
		{
			return $"Week:{Week:0#} {FieldGoalsMade}-{ExtraPointsMade}";
		}

		public override string ToString()
		{
			return $"Week:{Week:0#} {RushingTds}-{ReceivingTds}-{PassingTds}";
		}

		public int KickingPoints()
		{
			return (3 * FieldGoalsMade) + ExtraPointsMade;
		}
	}

}
