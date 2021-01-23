using System;

namespace TimeConverter
{
	class Program
	{
		static void Main(string[] args)
		{
			var timeCode = "8 AM PDT";
			var part = timeCode.Split();
			var len = part.Length;
			var timeZone = part[len - 1];
			var hour = Int32.Parse(part[0]);
			if (part[1].ToLower().Equals("pm"))
				hour += 12;
			if (timeZone.ToUpper() == "PDT")
			{
				hour = hour - 7 + 9;
			}
		}
	}
}
