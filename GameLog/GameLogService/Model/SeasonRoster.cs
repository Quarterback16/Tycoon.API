using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLogService.Model
{
	public class SeasonRoster
	{
		public List<PlayerReportModel> PlayerLogs { get; }
		public PlayerContributions Contribution { get; }

		public int TotalPoints { get; set; }
		public LineUp[] LineUp { get; set; }

		public SeasonRoster()
		{
			PlayerLogs = new List<PlayerReportModel>();
			TotalPoints = 0;
			LineUp = new LineUp[17];
			for (int i = 0; i < 17; i++)
			{
				LineUp[i] = new LineUp();
			}
			Contribution = new PlayerContributions();
		}

		public void AddLog(
			PlayerReportModel model)
		{
			PlayerLogs.Add(model);
		}

		public string Points(
			int week)
		{
			var (rushingTouchdowns, runningBackSet) = TouchdownsForRushing(week);
			rushingTouchdowns += QbTouchdownsForRushing(week);
			var (passingTouchdowns, quarterback) = TouchdownsForPassing(week);
			var (receivingTouchdowns,receiverSet) = TouchdownsForReceiving(week);
			var (rbReceivingTouchdowns,catchingRbSet) = RbTouchdownsForReceiving(week);
			receivingTouchdowns += rbReceivingTouchdowns;
			if (receivingTouchdowns > 0
				&& runningBackSet.Count() < 2)
				runningBackSet.Add(
					catchingRbSet.At(0));
			var (fgs, pats, kicker) = Kicking(week);
			var tdp = MinOf(passingTouchdowns, receivingTouchdowns);
			var tds = rushingTouchdowns + tdp;
			var xp = MinOf(pats, tds);
			var points = (tds * 6) + (fgs * 3) + xp;
			TotalPoints += points;
			LineUp[week].QB = quarterback;
			LineUp[week].RB1 = runningBackSet.At(0);
			LineUp[week].RB2 = runningBackSet.At(1);
			LineUp[week].PR1 = receiverSet.At(0);
			LineUp[week].PR2 = receiverSet.At(1);
			LineUp[week].PR3 = receiverSet.At(2);
			LineUp[week].Kicker = kicker;
			Contribution.Add(LineUp[week]);
			return $@"TDp:{
				tdp
				} TDr:{
				rushingTouchdowns,2
				} TDs:{
				tds,2
				} FGs:{
				fgs,2
				} xp:{
				xp,2
				} PTS:{
				points,2
				} drops: {
				DroppedPasses(
					passingTouchdowns,
					receivingTouchdowns),2
				}  rb spots avail: {
				2-runningBackSet.Count(),2
				}";
		}

		private int DroppedPasses(
			int passingTouchdowns, 
			int receivingTouchdowns)
		{
			if (receivingTouchdowns >= passingTouchdowns)
				return 0;
			return (passingTouchdowns - receivingTouchdowns);
		}

		private int MinOf(
			int num1, 
			int num2)
		{
			if (num2 < num1)
				return num2;
			return num1;
		}

		private (int,int,Starter) Kicking(int week)
		{
			var kicker = new Starter();
			var fgs = 0;
			var pats = 0;
			var logsForWeek = GetLogsFor(
				week,
				"KK");
			var sortedLogs = logsForWeek.OrderByDescending(
				l => l.GameLog[0].FieldGoalsMade);
			var take1 = 0;
			foreach (var kk in sortedLogs)
			{
				fgs += kk.GameLog[0].FieldGoalsMade;
				pats += kk.GameLog[0].ExtraPointsMade;
				kicker = new Starter(
					kk.PlayerName,
					new GameStats 
					{
						FieldGoalsMade = kk.GameLog[0].FieldGoalsMade
					});
				take1++;
				if (take1 == 1)
					break;
			}
			return (fgs, pats, kicker);
		}

		private (int, PlayerSet) TouchdownsForRushing(
			int week )
		{
			var rushingTouchdowns = 0;
			var runningbacksUsed = new PlayerSet();
			var logsForWeek = GetLogsFor(
				week, 
				"RB");
			var sortedLogs = logsForWeek.OrderByDescending(
				l => l.GameLog[0].RushingTds);
			var take2 = 0;
			foreach (var rb in sortedLogs)
			{
				rushingTouchdowns += rb.GameLog[0].RushingTds;
				if (rb.GameLog[0].RushingTds > 0)
					runningbacksUsed.Add(
						new Starter(
							rb.PlayerName,
							new GameStats
							{
								RushingTds = rb.GameLog[0].RushingTds
							}));
				take2++;
				if (take2 == 2)
					break;
			}
			return (rushingTouchdowns, runningbacksUsed);
		}

		private List<PlayerReportModel> GetLogsFor(
			int week, 
			string position)
		{
			var thisWeek = new List<PlayerReportModel>();
			foreach (var model in PlayerLogs)
			{
				if (model.Position != position)
					continue;
				var thisPlayer = new PlayerReportModel(
					season: model.Season,
					playerName: model.PlayerName,
					position: model.Position);
				var thisLog = new List<GameStats>();
				foreach (var g in model.GameLog)
				{
					thisLog.Add(g);
				}
				foreach (var g in model.GameLog)
				{
					if (g.Week != week)
						continue;
					var thisWeeksGame = new List<GameStats>
					{
						g
					};
					thisLog  = thisWeeksGame;
					thisPlayer.GameLog = thisLog;
					thisWeek.Add(thisPlayer);
				}
			}
			return thisWeek;
		}

		private int QbTouchdownsForRushing(
			int week)
		{
			var rushingTRouchdowns = 0;
			var logsForWeek = GetLogsFor(
				week,
				"QB");
			var sortedLogs = logsForWeek.OrderByDescending(
				l => l.GameLog[0].PassingTds);
			var take1 = 0;
			foreach (var qb in sortedLogs)
			{
				rushingTRouchdowns += qb.GameLog[0].RushingTds;
				take1++;
				if (take1 == 1)
					break;
			}
			return rushingTRouchdowns;
		}

		private (int,Starter) TouchdownsForPassing(
			int week)
		{
			Starter quarterback = new Starter();
			var passingTouchdowns = 0;
			var logsForWeek = GetLogsFor(
				week,
				"QB");
			var sortedLogs = logsForWeek.OrderByDescending(
				l => l.GameLog[0].PassingTds);
			var take1 = 0;
			foreach (var qb in sortedLogs)
			{
				passingTouchdowns += qb.GameLog[0].PassingTds;
				quarterback = new Starter(
					qb.PlayerName,
					new GameStats
					{
						PassingTds = qb.GameLog[0].PassingTds
					});
				take1++;
				if (take1 == 1)
					break;
			}
			return (passingTouchdowns, quarterback);
		}

		private (int,PlayerSet) TouchdownsForReceiving(
			int week)
		{
			var receiverSet = new PlayerSet();
			var receivingTouchdowns = 0;
			var logsForWeek = GetLogsFor(
				week,
				"WR");
			logsForWeek.AddRange(
				GetLogsFor(
					week, 
					"TE"));
			var sortedLogs = logsForWeek.OrderByDescending(
				l => l.GameLog[0].ReceivingTds);
			var take3 = 0;
			foreach (var pr in sortedLogs)
			{
				receivingTouchdowns += pr.GameLog[0].ReceivingTds;
				receiverSet.Add(
					new Starter(
						pr.PlayerName,
						new GameStats
						{
							ReceivingTds = pr.GameLog[0].ReceivingTds
						}));
				take3++;
				if (take3 == 3)
					break;
			}
			return (receivingTouchdowns, receiverSet);
		}

		private (int,PlayerSet) RbTouchdownsForReceiving(
			int week)
		{
			var catchingRbSet = new PlayerSet();
			var receivingTouchdowns = 0;
			var logsForWeek = GetLogsFor(
				week,
				"RB");
			var sortedLogs = logsForWeek.OrderByDescending(
				l => l.GameLog[0].ReceivingTds);
			var take2 = 0;
			foreach (var pr in sortedLogs)
			{
				receivingTouchdowns += pr.GameLog[0].ReceivingTds;
				catchingRbSet.Add(
					new Starter(
						pr.PlayerName,
						new GameStats
						{
							ReceivingTds = pr.GameLog[0].ReceivingTds
						}));
				take2++;
				if (take2 == 2)
					break;
			}
			return (receivingTouchdowns, catchingRbSet);
		}

		public string ContributionsOut()
		{
			var sb = new StringBuilder();
			sb.AppendLine("Total Contributions:");

			var sortedContributions = Contribution.Contributions
				.OrderByDescending(c => c.Value.Scores());

			foreach (var item in sortedContributions)
			{
				sb.AppendLine($"   {item.Key,-17} ({item.Value.Scores(),2})");
			}
			return sb.ToString();
		}

		public int ContributionOf(
			string player)
		{
			if (Contribution.Contributions.ContainsKey(player))
				return Contribution.Contributions[player].Scores();
			return 0;
		}

		public string Lineup(
			int week)
		{
			return LineUp[week].ToString();
		}
	}
}
