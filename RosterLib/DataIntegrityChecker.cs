using System;
using System.Data;

namespace RosterLib
{
	public class DataIntegrityChecker
	{
		public string Season { get; set; }

		public int Week { get; set; }

		public NFLWeek NflWeek { get; set; }

		public int ScoresChecked { get; set; }

		public int StatsChecked { get; set; }

		public int GamesChecked { get; set; }

		public int LineupsChecked { get; set; }

		public int Errors { get; set; }

		public int Warnings { get; set; }

		public DataIntegrityChecker()
		{

		}

		public DataIntegrityChecker(
			string season,
			int week)
		{
			Season = season;
			Week = week;
		}

		public void CheckScores()
		{
			if (string.IsNullOrEmpty(Season))
				Utility.Announce("Please specify the Season and optionally the Week");
			else
			{
				if (Week > 0)
					CheckScoresForWeek(Week);
				{
					for (int w = 1; w < 18; w++)
					{
						CheckScoresForWeek(w);
					}
				}
			}
			ReportResults(ScoresChecked, "Scores");
		}

		private void CheckScoresForWeek(int week)
		{
			NflWeek = new NFLWeek(Season, week);
			var scoreFactory = new ScoreFactory();

			var ds = Utility.TflWs.ScoresDs(Season, String.Format("{0:0#}", week));
			var dt = ds.Tables["score"];
			foreach (DataRow dr in dt.Rows)
			{
				ScoresChecked++;
				var score = scoreFactory.CreateScore(dr["SCORE"].ToString());
				if (score != null)
				{
					score.Load(dr);
#if DEBUG
					//score.Dump();
#endif
					if (score.IsValid())
					{
					}
					else
						Error(score);
				}
			}
		}

		public void CheckLineups()
		{
			if (string.IsNullOrEmpty(Season))
				Utility.Announce("Please specify the Season and Week");
			else
			{
				if (Week > 0)
					CheckLineupsForWeek(Week);
				else
				{
					for (int w = 1; w < 18; w++)
					{
						CheckLineupsForWeek(w);
					}
				}
			}
			ReportResults(StatsChecked, "Lineups");
		}

		private void CheckLineupsForWeek(int week)
		{
			NflWeek = new NFLWeek(
				Season,
				week,
				loadGames: true);

			foreach (var game in NflWeek.GamesList())
			{
				if (!game.Played())
					continue;

				CheckTeam(week, game, game.HomeTeam);
				CheckTeam(week, game, game.AwayTeam);

				LineupsChecked++;
			}
			ReportResults(LineupsChecked, "Lineups");
		}

		private void CheckTeam(
			int week, 
			NFLGame game,
			string teamCode)
		{
			var ds = Utility.TflWs.GetLineup(
				teamCode: teamCode,
				season: Season,
				week: week);
			if (ds.Tables[0].Rows.Count == 0)
			{
				GameError($"No Lineup for {teamCode}");
			}
		}

		public void CheckGames()
		{
			if (string.IsNullOrEmpty(Season))
				Utility.Announce("Please specify the Season and Week");
			else
			{
				if (Week > 0)
					CheckGamesForWeek(Week);
				else
				{
					for (int w = 1; w < 18; w++)
					{
						CheckGamesForWeek(w);
					}
				}
			}
			ReportResults(StatsChecked, "Stats");
		}

		private void CheckGamesForWeek(int week)
		{
			NflWeek = new NFLWeek(
				Season, 
				week,
				loadGames: true);

			foreach (var game in NflWeek.GamesList())
			{
				if (!game.Played())
					continue;

				var ds = Utility.TflWs.GetGameStats(
					gameCode: game.GameCode,
					season: Season,
					week: $"{week:0#}");
				if ( ds.Tables[0].Rows.Count == 0)
				{
					GameError("No Stats");
				}
				GamesChecked++;
			}
			ReportResults(GamesChecked, "Games");
		}

		public void CheckStats()
		{
			if (string.IsNullOrEmpty(Season))
				Utility.Announce("Please specify the Season and Week");
			else
			{
				if (Week > 0)
					CheckStatsForWeek(Week);
				else
				{
					for (int w = 1; w < 18; w++)
					{
						CheckStatsForWeek(w);
					}
				}
			}
			ReportResults(StatsChecked, "Stats");
		}

		private void CheckStatsForWeek(int week)
		{
			NflWeek = new NFLWeek(Season, week);
			var statFactory = new StatFactory();

			var ds = Utility.TflWs.PlayerStatsDs(
				season: Season,
				week: String.Format("{0:0#}", week));
			var dt = ds.Tables["stat"];
			foreach (DataRow dr in dt.Rows)
			{
				StatsChecked++;
				var stat = statFactory.CreateStat(dr["STAT"].ToString());
				if (stat != null)
				{
					stat.Load(dr);
#if DEBUG
					//stat.Dump();
#endif
					if (stat.IsValid())
					{
						if (!stat.IsReasonable())
							Warn($"{stat.Name} {stat.Quantity} not valid", stat);
					}
					else
						Error(stat);
				}
			}
		}

		private void ReportResults(int thingCheckedCount, string thingChecked)
		{
			if (Week > 0)
				Utility.Announce($"Week {Season}:{Week} {thingChecked} Checked {thingCheckedCount} Errors {Errors} Warnings {Warnings}");
			else
				Utility.Announce($"Season {Season} {thingChecked} Checked {thingCheckedCount} Errors {Errors} Warnings {Warnings}");
		}

		private void Warn(string msg, IStat stat)
		{
			Warnings++;
			stat.Dump();
			Utility.Announce($"          Warning : {NflWeek.WeekKey(":")} - {msg} - {stat.PlayerId}");
		}

		private void Error(IStat stat)
		{
			Errors++;
			stat.Dump();
			Utility.Announce($"          Error : {NflWeek.WeekKey(":")} - {stat.Error} - {stat.PlayerId}");
		}

		private void Error(IScore score)
		{
			Errors++;
			score.Dump();
			Utility.Announce($"          Error : {NflWeek.WeekKey(":")} - {score.Error} - {score.PlayerId1}");
		}

		private void GameError(string msg)
		{
			Errors++;
			Utility.Announce($"          Error : {NflWeek.WeekKey(":")} - {msg}");
		}

	}
}