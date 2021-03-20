using System;

namespace GameLogService.Model
{
	public class Record
	{
		public string Name { get; set; }
		public int Wins { get; set; }
		public int Losses { get; set; }

		public Record( string name)
		{
			Name = name;
		}
		public override string ToString()
		{
			return $"{Name}: ({Wins,2}-{Losses,2}) {Percent(),5}";
		}

		public string Percent()
		{
			if (TotalGames() == 0)
				return "0%";
			return $"{  Clip() * 100M:####0}%";
		}

		public decimal Clip()
		{
			if (TotalGames() == 0)
				return 0.000M;

			return Wins / (decimal)TotalGames();
		}

		public int TotalGames()
		{
			return Wins + Losses;
		}

		public string OverallResult()
		{
			if (TotalGames() == 0)
				return " ";
			if (Clip() > .5M)
				return "W";
			else if (Clip() == .5M)
				return "T";
			return "L";
		}

		public void Update(
			int myScore, 
			int hisScore)
		{
			if (myScore > hisScore)
			{
				Wins++;
				return;
			}
			Losses++;
		}
	}
}
