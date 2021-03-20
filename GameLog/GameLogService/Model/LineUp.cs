using System;
using System.Text;

namespace GameLogService.Model
{
	public class LineUp
	{
		public Starter QB { get; set; }
		public Starter RB1 { get; set; }
		public Starter RB2 { get; set; }
		public Starter PR1 { get; set; }
		public Starter PR2 { get; set; }
		public Starter PR3 { get; set; }
		public Starter Kicker { get; set; }

		public override string ToString()
		{
			return LineupAsString();
		}

		public string LineupAsString()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"        QB1: {QB}");
			sb.AppendLine($"        RB1: {RB1}");
			sb.AppendLine($"        RB2: {RB2}");
			sb.AppendLine($"        PR1: {PR1}");
			sb.AppendLine($"        PR2: {PR2}");
			sb.AppendLine($"        PR3: {PR3}");
			sb.AppendLine($"        KK1: {Kicker}");
			return sb.ToString();
		}
	}


}
