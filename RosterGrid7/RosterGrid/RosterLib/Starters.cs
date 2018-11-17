using System;

namespace RosterLib
{
	public class Starters
	{
		public bool PlayoffsOnly { get; set; }

		public bool WriteProjections { get; set; }

		public PlayerLister Lister { get; set; }

		public Starters()
		{
			Lister = new PlayerLister();
		}

		public void AllStarters( [System.Runtime.InteropServices.Optional] string fantasyLeague )
		{
			Utility.Announce( "Starters..." );
			RenderStarters( Constants.K_QUARTERBACK_CAT, "QB", fantasyLeague );
			RenderStarters( Constants.K_RECEIVER_CAT, "WR", fantasyLeague );
			RenderStarters( Constants.K_RECEIVER_CAT, "TE", fantasyLeague );
			RenderStarters( Constants.K_RUNNINGBACK_CAT, "RB", fantasyLeague );
			RenderStarters( Constants.K_KICKER_CAT, "K", fantasyLeague );
		}

		public string RenderStarters( string cat, string sPos, [System.Runtime.InteropServices.Optional] string fantasyLeague )
		{
			Lister.SortOrder = Int32.Parse(Utility.CurrentWeek()) > 0 ? "POINTS DESC" : "CURSCORES DESC";
			PlayoffsOnly = PlayoffsOnly;

			var theWeek = new NFLWeek( Int32.Parse( Utility.CurrentSeason() ), Int32.Parse( Utility.CurrentWeek() ), false );
			var gs = new GS4Scorer( theWeek );
			Lister.RenderToCsv = true;
			Lister.SetScorer( gs );
			Lister.StartersOnly = true;
			Lister.Collect( cat, sPos, fantasyLeague );

			var fileOut = Lister.Render( string.Format( "{1}-Starters-{0}", sPos, fantasyLeague ) );

			if (WriteProjections)
				WritePlayerProjections();

			Lister.Clear();

			return fileOut;
		}

		public void WritePlayerProjections()
		{
			foreach (NFLPlayer p in Lister.PlayerList)
				p.PlayerProjection(Utility.CurrentSeason());
		}

	}
}
