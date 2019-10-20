using RosterLib.Interfaces;
using System.Collections.Generic;

namespace RosterLib
{
   public class HotListReporter : RosterGridReport
   {
      private readonly IWeekMaster WeekMaster;

      public PlayerLister PlayerLister { get; set; }

      public bool FreeAgentsOnly { get; set; }

      public bool StartersOnly { get; set; }

      public string League { get; set; }

      public NFLRosterGrid RosterGrid { get; set; }

      public List<HotListConfig> Configs { get; set; }

      public List<RosterGridLeague> Leagues { get; set; }

      public HotListReporter( IKeepTheTime timekeeper ) : base( timekeeper )
		{
         WeekMaster = new WeekMaster();
         Name = "Hot Lists";
         Configs = new List<HotListConfig>();
#if ! DEBUG2
         Configs.Add(new HotListConfig { Category = Constants.K_QUARTERBACK_CAT, Position = "QB", FreeAgents = true, Starters = true });
         Configs.Add(new HotListConfig { Category = Constants.K_RUNNINGBACK_CAT, Position = "RB", FreeAgents = false, Starters = false });
         Configs.Add(new HotListConfig { Category = Constants.K_RECEIVER_CAT, Position = "WR", FreeAgents = true, Starters = true });
         Configs.Add(new HotListConfig { Category = Constants.K_RECEIVER_CAT, Position = "TE", FreeAgents = true, Starters = true });
#endif
         Configs.Add(new HotListConfig { Category = Constants.K_KICKER_CAT, Position = "PK", FreeAgents = true, Starters = true });
		Leagues = new List<RosterGridLeague>
		{
			new RosterGridLeague
			{
				Id = Constants.K_LEAGUE_Gridstats_NFL1,
				Name = "Gridstats GS1"
			}
		};
#if !DEBUG
         Leagues.Add( new RosterGridLeague { Id = Constants.K_LEAGUE_Yahoo, Name = "Spitzys League" } );
         Leagues.Add( new RosterGridLeague { Id = Constants.K_LEAGUE_Rants_n_Raves, Name = "ESPN" } );
#endif
		}

      public override void RenderAsHtml()
      {
         foreach (var league in Leagues)
         {
            League = league.Id;
            foreach ( var rpt in Configs )
               GenerateReport( rpt );
         }
      }

      private void GenerateReport(HotListConfig rpt)
      {
         HotList(
			 rpt.Category,
			 rpt.Position,
			 rpt.FreeAgents,
			 rpt.Starters);
      }

      public void HotList(
		  string catCode,
		  string position,
		  bool freeAgentsOnly,
		  bool startersOnly)
      {
         var gs = new GS4Scorer(Utility.CurrentNFLWeek());
         PlayerLister = new PlayerLister
         {
            CatCode = catCode,
            Position = position,
            FantasyLeague = League,
            Season = Utility.CurrentSeason(),
            FreeAgentsOnly = freeAgentsOnly,
            StartersOnly = startersOnly,
            Week = Utility.PreviousWeek(),
            WeekMaster = WeekMaster,
            WeeksToGoBack = 4
         };
         PlayerLister.SetScorer(gs);
         PlayerLister.Load();
         PlayerLister.SubHeader = $"{PlayerLister.Position} HotList for {PlayerLister.FantasyLeague}";
         PlayerLister.FileOut = $"HotLists//HotList-{PlayerLister.FantasyLeague}-{PlayerLister.Position}";
         PlayerLister.Render(PlayerLister.FileOut);
      }
   }

   public class HotListConfig
   {
      public string Category { get; set; }

      public string Position { get; set; }

      public bool FreeAgents { get; set; }

      public bool Starters { get; set; }
   }
}