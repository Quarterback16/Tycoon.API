using Gerard.Messages;
using RosterLib.Interfaces;
using System;
using System.Diagnostics;

namespace RosterLib
{
	public class MissingPlayerDataChecker 
	{
		public ISend Sender { get; set; }

		public IRunStorer MyRunStorer { get; set; }
		public Stopwatch Stopwatch { get; set; }

		public MissingPlayerDataChecker(ISend sender)
		{
			Sender = sender;
			MyRunStorer = new DbfRunStorer();
			Stopwatch = new Stopwatch();
			Stopwatch.Start();
		}

		public void CheckPlayers( string teamCode )
		{
			var team = new NflTeam(teamCode);
			team.LoadTeam();
			foreach (NFLPlayer plyr in team.PlayerList)
			{
				if (plyr.IsMissingDob())
				{
					Console.WriteLine($"{plyr.PlayerNameShort} has no dob");
					Sender.Send(
						new DataFixCommand
						{
							FirstName = plyr.FirstName,
							LastName = plyr.Surname,
							TeamCode = teamCode
						});
				}
			}
			StoreRun("MissingPlayerDataChecker");
		}

		private void StoreRun(string theName)
		{
			var runTime = Utility.StopTheWatch(
				Stopwatch,
				$"Finished: {theName}");

			MyRunStorer.StoreRun(
				theName,
				runTime);
		}
	}
}
