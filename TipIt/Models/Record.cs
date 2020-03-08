using System;
using TipIt.Helpers;

namespace TipIt.Models
{
    public class Record
    {
		public Record(string name)
		{
			Name = name;
		}

		public string Name { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }

		public decimal EasyPoints { get; set; }

		public override string ToString()
		{
			return $@"{
				StringUtils.StringOfSize(4,Name)
				}: ({Wins,2}-{Losses,2}-{Draws,2}) {Percent(),5} {
				EasyPoints
				}";
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

			return ((Wins * 2) + Draws) / ((decimal)TotalGames()*2);
		}

		public int TotalGames()
		{
			return Wins + Losses + Draws;
		}

	}
}
