using RosterLib.Interfaces;
using System.Collections.Generic;
using System.Text;
using NLog;
using System;

namespace RosterLib.RosterGridReports
{
	public class FantasyScorecardReport : RosterGridReport
	{
		public List<ScoreCard> ScoreCards { get; set; }

		public IPlayerGameMetricsDao PgmDao { get; set; }

		public List<string> PlayerIds { get; set; }

		public IHavePlayerIds PlayerList { get; set; }

		public string PlayerType { get; set; }

		public string Week { get; set; }

		public ScoreCard TotalScoreCard { get; set; }

		public FantasyScorecardReport(
			IKeepTheTime timekeeper,
			IPlayerGameMetricsDao pgmDao,
			IHavePlayerIds playerList) : base(timekeeper)
		{
			Name = "Fantasy Scorecard";
			Season = timekeeper.CurrentSeason();
			Week = timekeeper.Week;
			PlayerIds = new List<string>();
			PgmDao = pgmDao;
			PlayerList = playerList;
			ScoreCards = new List<ScoreCard>();
			TotalScoreCard = new ScoreCard();
		}

		public FantasyScorecardReport(
			IKeepTheTime timekeeper,
			IPlayerGameMetricsDao pgmDao) : base(timekeeper)
		{
			Name = "Fantasy Scorecard";
			Season = timekeeper.CurrentSeason();
			Week = timekeeper.Week;
			PlayerIds = new List<string>();
			PgmDao = pgmDao;
			ScoreCards = new List<ScoreCard>();
			TotalScoreCard = new ScoreCard();
		}

		public override void RenderAsHtml()
		{
			if (string.IsNullOrEmpty(PlayerList.PlayerType()))
				RenderAll(Constants.K_LEAGUE_Gridstats_NFL1);
			else
				RenderOne();
		}

		private void RenderAll(string leagueId)
		{
			var playerLister = new PlayerLister
			{
				StartersOnly = true
			};
			for (int i = 1; i < 6; i++)
			{			
				playerLister.Collect(
					catCode: CatForReport(i),
					sPos: PosForReport(i),
					fantasyLeague: leagueId);
				playerLister.Position = PosForReport(i);
				PlayerList = playerLister;
				RenderOne();
				playerLister.Clear();
			}
		}

		private string CatForReport(int report)
		{
			switch (report)
			{
				case 1:
					return "1";
				case 2:
					return "2";
				case 3:
					return "3";
				case 4:
					return "4";
				case 5:
					return "3";

				default:
					break;
			}
			return "?";
		}

		private string PosForReport(int cat)
		{
			switch (cat)
			{
				case 1:
					return "QB";
				case 2:
					return "RB";
				case 3:
					return "WR";
				case 4:
					return "PK";
				case 5:
					return "TE";

				default:
					break;
			}
			return "?";
		}

		private void RenderOne()
		{
			PlayerType = PlayerList.PlayerType();
			RenderPreReport();
			Logger.Info($"rendered {FileOut}");
		}

		private void RenderPreReport()
		{
			TallyStats();
			var PreReport = new SimplePreReport
			{
				ReportType = Name,
				Folder = "Scorecards",
				Season = Season,
				InstanceName = $"{PlayerType}-Week-{Week:0#}",
				Body = GenerateBody()
			};
			PreReport.RenderHtml();
			FileOut = PreReport.FileOut;
		}

		private void TallyStats()
		{
			var pList = PlayerList.GetAll();
			foreach (var plyr in pList)
			{
				var player = new NFLPlayer(plyr.Id);
				var scorecard = new ScoreCard
				{
					Player = player
				};
				var season = PgmDao.GetSeason(
					Season,
					player.PlayerCode);

				if (Week == "00")
					TallyPreviousActuals(
						scorecard,
						player);

				TallyPredictions(
					scorecard,
					player,
					season);

				if (Int32.Parse(Week) > 0)
				{
					TallyActuals(scorecard, player, season);
				}
				ScoreCards.Add(scorecard);
			}
		}

		private void TallyActuals(
			ScoreCard scorecard,
			NFLPlayer player,
			List<PlayerGameMetrics> season)
		{
			foreach (var pgm in season)
			{
				scorecard.TotalPoints += pgm.CalculateActualFantasyPoints(
					player);
			}
			for (int w = 1; w < 17; w++)
			{
				scorecard.Week[w-1] = Decimal.Parse(
					TotalFor(w, season, player));
			}
		}

		private void TallyPredictions(
			ScoreCard scorecard,
			NFLPlayer player,
			List<PlayerGameMetrics> season)
		{
			scorecard.TotalPoints = TotalPredictonFor(
				season,
				player);
			for (int w = 1; w < 17; w++)
			{
				scorecard.Week[w] 
					= PredictonFor(w, season, player) 
					  * player.HealthRating()
					  * player.AgeRating();
				TotalScoreCard.Week[w] += scorecard.Week[w];
				TotalScoreCard.TotalPoints += scorecard.Week[w];
			}
		}

		private static int CompareByTotalPoints(
			ScoreCard x,
			ScoreCard y)
		{
			if (x == null)
			{
				if (y == null)
					return 0;
				return -1;
			}
			return y == null ? 1 : y.TotalPoints.CompareTo(x.TotalPoints);
		}

		private string GenerateBody()
		{
			var compareByPts = new Comparison<ScoreCard>(
				CompareByTotalPoints);

			ScoreCards.Sort(compareByPts);
			var pList = PlayerList.GetAll();
			var sb = new StringBuilder();
			AppendHeader(sb);
			foreach (var card in ScoreCards)
			{
				var player = card.Player;
				var season = PgmDao.GetSeason(
					Season,
					player.PlayerCode);

				GeneratePredictions(
					sb, 
					player, 
					season);
				if (Week == "00")
					OutputPriors(
						sb,
						card);
				if (Int32.Parse(Week) > 0)
				{
					GenerateActuals(sb, player, season);
					VarianceLine(sb, player, season);
				}
				sb.AppendLine();
			}
			OutputTotalPredictions(sb);
			if (Week == "00")
				OutputPriors(
					sb,
					TotalScoreCard);
			return sb.ToString();
		}

		private void OutputTotalPredictions(StringBuilder sb)
		{
			sb.Append($"TOTALS        PRED ");
			sb.Append($"{TotalScoreCard.TotalPoints,11:0.0}");
			for (int w = 1; w < 17; w++)
				sb.Append($"{TotalScoreCard.Week[w],9:0.0}");
			sb.AppendLine();
		}

		private void OutputPriors(
			StringBuilder sb,
			ScoreCard card)
		{
			sb.Append(new string(' ',19));
			sb.Append($"{card.PreviousTotalPoints,11:0.0}");
			for (int w = 1; w < 17; w++)
				sb.Append($"{card.LastYearWeek[w],9:0.0}");
			sb.AppendLine();
		}

		private void VarianceLine(
			StringBuilder sb,
			NFLPlayer player,
			List<PlayerGameMetrics> season)
		{
			sb.Append(new String(' ', 19));
//			sb.Append(VarianceFor(season, player));
			sb.Append(new String(' ', 11));
			for (int w = 1; w <= Int32.Parse(Week); w++)
				sb.Append(VarianceFor(w, season, player));
			sb.AppendLine();
		}

		private string TotalFor(
			int week,
			List<PlayerGameMetrics> season,
			NFLPlayer player)
		{
			var Pts = 0.0M;
			foreach (var pgm in season)
			{
				if (pgm.Week().Equals($"{week:0#}"))
				{
					Pts = pgm.CalculateActualFantasyPoints(
						player);
				}
			}
			return $"{Pts,9:0.0}";
		}

		private void TallyPreviousActuals(
			ScoreCard scorecard,
			NFLPlayer player)
		{
			var season = PgmDao.GetSeason(
				PreviousSeason(Season),
				player.PlayerCode);
			scorecard.PreviousTotalPoints = 
				decimal.Parse(TotalFor(season, player));
			for (int w = 1; w < 17; w++)
			{
				var actualForTheWeek = decimal.Parse(
					TotalFor(
						w, 
						season, 
						player));
				scorecard.LastYearWeek[w] = actualForTheWeek;
				TotalScoreCard.LastYearWeek[w] += actualForTheWeek;
				TotalScoreCard.PreviousTotalPoints += actualForTheWeek;
			}
		}

		private string PreviousSeason(
			string season)
		{
			var year = Int32.Parse(season);
			var previousYear = year - 1;
			return previousYear.ToString();
		}

		private void GenerateActuals(
			StringBuilder sb,
			NFLPlayer player,
			List<PlayerGameMetrics> season)
		{
			sb.Append(new String(' ',19));
			sb.Append(TotalFor(season, player));
			for (int w = 1; w <= Int32.Parse(Week); w++)
			{
				sb.Append(TotalFor(w, season, player));
			}
			sb.AppendLine();
		}

		private string VarianceFor(
			int week,
			List<PlayerGameMetrics> season,
			NFLPlayer player)
		{
			var variance = 0.0M;
			foreach (var pgm in season)
			{
				if (pgm.Week().Equals($"{week:0#}"))
				{
					decimal predictedPts = pgm.CalculateProjectedFantasyPoints(
						player);
					decimal Pts = pgm.CalculateActualFantasyPoints(
						player);
					variance = Pts - predictedPts;
				}
			}
			var strVar = variance.ToString("+0.0;-0.0");
			return $"{strVar,9}";
		}

		private string VarianceFor(
			List<PlayerGameMetrics> season,
			NFLPlayer player)
		{
			var predicted = 0.0M;
			var Pts = 0.0M;
			foreach (var pgm in season)
			{
				predicted += pgm.CalculateProjectedFantasyPoints(
					player);
				Pts += pgm.CalculateActualFantasyPoints(
					player);
			}
			var variance = Pts - predicted;
			return $"{variance,11:+0.0}";
		}

		private string TotalFor(
			List<PlayerGameMetrics> season,
			NFLPlayer player)
		{
			var Pts = 0.0M;
			foreach (var pgm in season)
			{
				Pts += pgm.CalculateActualFantasyPoints(
					player);
			}
			return $"{Pts,11}";
		}

		private void GeneratePredictions(
			StringBuilder sb,
			NFLPlayer player,
			List<PlayerGameMetrics> season)
		{
			sb.Append($"{GetPlayerNameShort(player),-15} {GetTeamCode(player)}");
			sb.Append($"{TotalPredictonFor(season, player),11:0.0}");
			for (int w = 1; w < 17; w++)
				sb.Append($"{PredictonFor(w, season, player),9:0.0}");
			sb.AppendLine();
		}

		private static string GetPlayerNameShort(
			NFLPlayer player)
		{
			var name = player.PlayerNameShort;
			if (player.IsRookie())
				name += "*";
			return name;
		}

		private static string GetTeamCode(NFLPlayer player)
		{
			var teamCode = player.TeamCode;
			if (player.IsNewbie())
				teamCode += "*";
			else
				teamCode += " ";
			return teamCode;
		}

		private decimal TotalPredictonFor(
			List<PlayerGameMetrics> season,
			NFLPlayer player)
		{
			var predictedPts = 0.0M;
			foreach (var pgm in season)
			{
				predictedPts += PredictPlayer(
					player,
					pgm);
			}
			return predictedPts;
		}

		private void AppendHeader(StringBuilder sb)
		{
			sb.Append( "Player                -- total --");
			for (int w = 1; w < 17; w++)
				sb.Append($"-- {w:0#} -- ");
			sb.AppendLine();
		}

		private decimal PredictonFor(
			int week, 
			List<PlayerGameMetrics> season,
			NFLPlayer player)
		{
			var predictedPts = 0.0M;
			foreach (var pgm in season)
			{
				if (pgm.Week().Equals($"{week:0#}"))
				{
					predictedPts = PredictPlayer(player, pgm);
				}
			}
			return predictedPts;
		}

		private static decimal PredictPlayer(
			NFLPlayer player, 
			PlayerGameMetrics pgm)
		{
			var newbieModifier = player.NewbieModifier();
			if (newbieModifier != 1.0M)
				newbieModifier = 1.0M - newbieModifier;
			var healthModifier = player.HealthRating();
			var ageModifier = player.AgeRating();
			var predictedPts = pgm.CalculateProjectedFantasyPoints(
				player)
				* newbieModifier
				* healthModifier
				* ageModifier;
			return predictedPts;
		}
	}

	public class ScoreCard
	{
		public NFLPlayer Player { get; set; }
		public decimal TotalPoints { get; set; }
		public decimal PreviousTotalPoints { get; set; }

		public decimal[] LastYearWeek { get; set; }
		public decimal[] Week { get; set; }

		public ScoreCard()
		{
			LastYearWeek = new decimal[17];
			Week = new decimal[17];
		}

		public override string ToString()
		{
			return $"{Player.PlayerNameShort}:{TotalPoints,5:##0.0}";
		}
	}
}
