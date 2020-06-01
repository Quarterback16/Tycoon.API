using System;
using System.Collections.Generic;
using System.Text;
using TipIt.Helpers;
using TipIt.Implementations;
using TipIt.Interfaces;
using TipIt.Models;

namespace TipIt.TippingStrategies
{
	public class NibbleTipster : BaseTipster, ITipster
	{
		public Dictionary<string,NibbleRating> Ratings { get; set; }

		public decimal AverageScore { get; set; }
		public int MaxScore { get; set; }
		public int MinScore { get; set; }

		public int HomeFieldAdvantage { get; set; }

		public NibbleTipster(
			TippingContext context) : base(context)
		{
			Ratings = new Dictionary<string, NibbleRating>();
		}

		public string ShowTips(
			string league,
			int round)
		{
			AverageScore = Context.AverageScore(
				leagueCode: league);
			HomeFieldAdvantage = Context.HomeFieldAdvantage(
				leagueCode: league);
			MaxScore = Context.MaxScore(league);
			MinScore = Context.MinScore(league);
			RateResults(league);
			Tip(league, round);
			return Output();
		}

		private void Tip(
			string league, 
			int round)
		{
			Predictions.Clear();
			var sched = Context.LeagueSchedule[league][round];
			foreach (var game in sched)
			{
				var prediction = Tip(game);

				Predictions.Add(prediction);
			}
		}


		public string RateResults(
			string leagueCode)
		{
			foreach (var item in Context.LeaguePastResults[leagueCode])
            {
                var games = item.Value;
                foreach (var g in games)
                {
					var gameRating = RateGame(g);
					AdjustTeam(
						g.HomeTeam,
						gameRating.HomeRating);
					AdjustTeam(
						g.AwayTeam,
						gameRating.AwayRating);
				}
			}
			return DumpRatings();
        }

		public string DumpRatings()
		{
			var sb = new StringBuilder();
			foreach (KeyValuePair<string,NibbleRating> pair in Ratings)
			{
				sb.AppendLine(
					$@"{
						StringUtils.StringOfSize(4,pair.Key)
						} {
						pair.Value
						}");
			}
			return sb.ToString();
		}

		private void AdjustTeam(
			string team, 
			NibbleRating rating)
		{
			if (!Ratings.ContainsKey(team))
				Ratings.Add(team, new NibbleRating());
			Ratings[team].Offence += rating.Offence;
			Ratings[team].Defence += rating.Defence;
		}

		public NibbleGameRating RateGame(
			Game game, int averageScore)
		{
			AverageScore = averageScore;
			return RateGame(game);
		}

		public NibbleGameRating RateGame(
			Game g)
		{
			var adjustment = new NibbleGameRating();
			if (!Ratings.ContainsKey(g.HomeTeam))
				Ratings.Add(g.HomeTeam, new NibbleRating());
			if (!Ratings.ContainsKey(g.AwayTeam))
				Ratings.Add(g.AwayTeam, new NibbleRating());

			var projHome = Ratings[g.HomeTeam].Offence 
				+ Ratings[g.AwayTeam].Defence;
			projHome = (projHome / 2) + (int) AverageScore;
			projHome += HomeFieldAdvantage;
			var projAway = Ratings[g.AwayTeam].Offence
				+ Ratings[g.HomeTeam].Defence;
			projAway = (projAway / 2) + (int) AverageScore;

			adjustment.HomeRating.Offence = (int)
				(g.HomeScore - projHome) / FudgeFactor();
			adjustment.HomeRating.Defence = (int)
				(g.AwayScore - projAway) / FudgeFactor();
			adjustment.AwayRating.Offence = (int)
				(g.AwayScore - projAway) / FudgeFactor();
			adjustment.AwayRating.Defence = (int)
				(g.HomeScore - projHome) / FudgeFactor();
			return adjustment;
		}

		private int FudgeFactor()
		{
			return 4;
		}

		public PredictedResult Tip(
			Game g)
		{
			var result = new PredictedResult(g);

			var homeOff = Ratings[g.HomeTeam].Offence;
			var homeDef = Ratings[g.HomeTeam].Defence;
			var awayOff = Ratings[g.AwayTeam].Offence;
			var awayDef = Ratings[g.AwayTeam].Defence;

			var homeScore = AverageScore + ((homeOff + awayDef) / 2);
			var awayScore = AverageScore + ((awayOff + homeDef) / 2);

			result.HomeScore = MaxMin((int)homeScore);
			result.AwayScore = MaxMin((int)awayScore);

			return result;
		}

		private int MaxMin(int score)
		{
			if (score > MaxScore)
				score = MaxScore;
			if (score < MinScore)
				score = MinScore;
			return score;
		}

	}

	public class NibbleRating
	{
		public int Offence { get; set; }
		public int Defence { get; set; }

		public NibbleRating()
		{
			Offence = 0;
			Defence = 0;
		}

		public override string ToString()
		{
			return $@"Off:{
				StringUtils.PadLeft(4, Offence.ToString())
				} Def:{
				StringUtils.PadLeft(4, Defence.ToString())
				}";
		}
	}

	public class NibbleGameRating
	{
		public NibbleRating HomeRating { get; set; }
		public NibbleRating AwayRating { get; set; }

		public NibbleGameRating()
		{
			HomeRating = new NibbleRating();
			AwayRating = new NibbleRating();
		}
		public override string ToString()
		{
			return $"Home:{HomeRating} Away:{AwayRating}";
		}
	}
}
