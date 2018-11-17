namespace RosterLib
{
	public class HotListReporter
	{
		public PlayerLister  PlayerLister { get; set; }

		public bool FreeAgentsOnly { get; set; }
		public bool StartersOnly { get; set; }
		public string League { get; set; }

		public HotListReporter()
		{
			FreeAgentsOnly = true;
			StartersOnly = true;
			League = Constants.K_LEAGUE_Gridstats_NFL1;
		}

		public void RunAllHotlists()
		{
			HotList( "1", "QB", true, true );
			HotList( "2", "RB", false, false );  //  need more info as these guys are getting scarce
			HotList( "3", "WR", true, true );
			HotList( "3", "TE", true, true );
			HotList( "4", "PK", true, true );			
		}

		public void HotList( string catCode, string position, bool freeAgentsOnly, bool startersOnly )
		{
			var gs = new GS4Scorer( Utility.CurrentNFLWeek() );
			PlayerLister = new PlayerLister
			{
				CatCode = catCode,
				Position = position,
				FantasyLeague = League,
				Season = Utility.CurrentSeason(),
				FreeAgentsOnly = freeAgentsOnly,
				StartersOnly = startersOnly,
				Week = Utility.CurrentNFLWeek().WeekNo
			};
			PlayerLister.SetScorer( gs );
			PlayerLister.Load();
			PlayerLister.SubHeader = string.Format( "{1} HotList for {0}", 
				PlayerLister.FantasyLeague, PlayerLister.Position );
			PlayerLister.FileOut = string.Format( "HotLists//HotList-{0}-{1}",
				PlayerLister.FantasyLeague, PlayerLister.Position );
			PlayerLister.Render( PlayerLister.FileOut );
		}
	}
}
