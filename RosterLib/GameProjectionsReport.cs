#define DEBUG2
using RosterLib.Interfaces;
using System;

namespace RosterLib
{
	public class GameProjectionsReport : RosterGridReport
	{
		public NflSeason NflSeason { get; private set; }

		public GameProjectionsReport( 
			IKeepTheTime timekeeper ) : base( timekeeper )
		{
			Name = "Game Projections Report";
			TimeKeeper = timekeeper;
		}

		public override void RenderAsHtml(
			bool structOnly = false)
		{
			StructOnly = structOnly;
			NflSeason = new NflSeason( 
				TimeKeeper.CurrentSeason(), 
				loadGames: true, 
				loadDivisions: false );
			foreach ( var game in NflSeason.GameList )
			{
				if (game.Played())
					continue;

				if (StructOnly)
					Console.WriteLine(
						$"WriteProjection for {game.GameName()}");
				else
				{
					TraceIt($"Writing projections for {game}");
					game.WriteProjection();
				}
#if DEBUG2
				if ( game.WeekNo > 1 ) break;
#endif
			}
		}

		public void TraceIt(string message)
		{
			Logger.Trace("   " + message);
#if DEBUG
			Console.WriteLine(message);
#endif
		}
	}
}