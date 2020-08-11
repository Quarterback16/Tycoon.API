﻿using RosterLib.Interfaces;
using System;
using System.Collections.Generic;

namespace RosterLib
{
	public class PlayerCsv : RosterGridReport
	{
		public string LeagueCode { get; set; }

		public PlayerLister Lister { get; set; }

		public List<StarterConfig> Configs { get; set; }

		public bool DoProjections { get; set; }

		public IAdpMaster AdpMaster { get; set; }

		public PlayerCsv(
			IKeepTheTime timekeeper,
			IAdpMaster adpMaster) : base(timekeeper)
		{
			Name = "Players CSV";
			SetLastRunDate();
			AdpMaster = adpMaster;
			Lister = new PlayerLister();
			Configs = new List<StarterConfig>
		 {
			new StarterConfig
			{
				Category = Constants.K_QUARTERBACK_CAT,
				Position = "QB"
			},
#if !DEBUG2
			new StarterConfig
			{
				Category = Constants.K_RUNNINGBACK_CAT,
				Position = "RB"
			},
			new StarterConfig
			{
				Category = Constants.K_RECEIVER_CAT,
				Position = "WR"
			},
			new StarterConfig
			{
				Category = Constants.K_RECEIVER_CAT,
				Position = "TE"
			},
			new StarterConfig
			{
				Category = Constants.K_KICKER_CAT,
				Position = "K"
			}
#endif
			};
		}

		public override void RenderAsHtml()
		{
			Console.WriteLine(
				$"Projections = {DoProjections}");
			RenderPlayerCsv();
		}

		public string RenderPlayerCsv()
		{
			Lister.SortOrder = "POINTS DESC";

			var nWeek = int.Parse(Utility.CurrentWeek());
			if (nWeek == 0) nWeek = 1;

			var theWeek = new NFLWeek(
				int.Parse(Utility.CurrentSeason()),
				nWeek, 
				loadGames: false);

			var weekMaster = new WeekMaster();

			Lister.RenderToCsv = true;
			Lister.StartersOnly = true;
			if (!DoProjections)
			{
				var scorer = new YahooScorer(theWeek);
				Lister.SetScorer(scorer);
			}

			foreach (var sc in Configs)
			{
				Lister.Collect(
					sc.Category, 
					sc.Position,
					fantasyLeague: string.Empty);
			}

			Lister.Folder = "Starters";
			Lister.LongStats = true;
			Lister.RenderToHtml = false;
			Lister.FantasyLeague = "GS";

			var fileOut = DoProjections ?
			   Lister.RenderProjection(
				   "PlayerCsv", 
				   weekMaster,
				   AdpMaster)
			   : Lister.Render("PlayerCsv");

			Lister.Clear();

			return fileOut;
		}
	}
}