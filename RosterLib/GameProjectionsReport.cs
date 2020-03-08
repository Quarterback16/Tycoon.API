using RosterLib.Interfaces;

namespace RosterLib
{
	public class GameProjectionsReport : RosterGridReport
	{
		public NflSeason NflSeason { get; private set; }

		public GameProjectionsReport( IKeepTheTime timekeeper ) : base( timekeeper )
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
					System.Console.WriteLine($"WriteProjection for {game.GameName()}");
				else
					game.WriteProjection();
#if DEBUG2
				if ( game.WeekNo > 1 ) break;
#endif
			}
		}
	}
}