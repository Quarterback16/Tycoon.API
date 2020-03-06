using System;
using System.Collections.Generic;

namespace RecSchedule
{
	class Program
	{
		static void Main(string[] args)
		{
#if !DEBUG
			var scheduleDate = DateTime.Now.AddDays(1);
#endif
			var scheduleDate = new DateTime(2020,1,6);
			var scheduleDateOut = scheduleDate.ToString("yyyy-MM-dd");
			if (args.Length > 0)
				scheduleDateOut = args[0].ToString();

			Console.WriteLine($"Rec Schedule : {scheduleDateOut}");

			//  Get the date (Monday)  the week is starting on
			var weekStart = DateTime.Parse(scheduleDateOut);

			//  load sessions available
			LoadSessions(weekStart);
			//  for each session 
			//    allocate an activity
		}

		private static List<Domain.RecSession> LoadSessions(
			DateTime weekStart)
		{
			var sessionList = new List<Domain.RecSession>
			{
				new Domain.RecSession
				{
					SessionDate = weekStart,
					SessionType = Domain.SessionType.Casual,
					StartTime = "1930",
					Activity = new Domain.RecActivity()
				},
				new Domain.RecSession
				{
					SessionDate = weekStart.AddDays(2),
					SessionType = Domain.SessionType.Casual,
					StartTime = "1930",
					Activity = new Domain.RecActivity()
				},
				new Domain.RecSession
				{
					SessionDate = weekStart.AddDays(4),
					SessionType = Domain.SessionType.Casual,
					StartTime = "1930",
					Activity = new Domain.RecActivity()
				}
			};
			return sessionList;
		}
	}


}
