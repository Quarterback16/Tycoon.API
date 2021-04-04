using System;
using TipIt.Events;
using TipIt.Helpers;

namespace TipIt.Models
{
    public class Game
    {
        public string League { get; set; }
        public int Round { get; set; }
        public DateTime GameDate { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }

        public PredictedResult PredictedResult { get; set; }

        public Game(ScheduleEvent e)
        {
            League = e.LeagueCode;
            Round = e.Round;
            GameDate = e.GameDate;
            HomeTeam = e.HomeTeam;
            AwayTeam = e.AwayTeam;
        }

        public Game(ResultEvent e)
        {
            League = e.LeagueCode;
            Round = e.Round;
            GameDate = e.GameDate;
            HomeTeam = e.HomeTeam;
            AwayTeam = e.AwayTeam;
            HomeScore = e.HomeScore;
            AwayScore = e.AwayScore;
        }

		public string Result()
		{
            return $"{AwayScore} - {HomeScore}";
		}

		public override string ToString()
        {
            return $@"{
                League
                } Round {
                Round
                } : {
                GameDate.ToString("yyyy-MM-dd HH:mm")
                } {
                StringUtils.StringOfSize(4,AwayTeam)
                } {AwayScore} @ {
                StringUtils.StringOfSize(4, HomeTeam)
                } {HomeScore}";
        }

        internal bool WinFor(string teamCode)
        {
            if (HomeTeam.Equals(teamCode))
            {
                if (HomeScore > AwayScore)
                    return true;
            }
            if (AwayTeam.Equals(teamCode))
                if (AwayScore > HomeScore)
                    return true;
            return false;
        }

        internal bool LossFor(string teamCode)
        {
            if (HomeTeam.Equals(teamCode))
            {
                if (AwayScore > HomeScore)
                    return true;
            }
            if (AwayTeam.Equals(teamCode))
                if (HomeScore > AwayScore)
                    return true;
            return false;
        }

		internal int WinningMargin()
		{
            return Math.Abs(HomeScore - AwayScore);
		}

		internal string GameLine(
            string teamCode)
		{
            var line = $@"{League} Rd {Round,2} {GameDate.ToString("yyyy-MM-dd")} {
                ResultFor(teamCode)
                } {ScoreFor(teamCode),2} - {ScoreAgin(teamCode),2}";
            return line;
		}

		private string ScoreAgin(string teamCode)
		{
            if (IsHomeTeam(teamCode))
                return $"{AwayScore}";
            return $"{HomeScore}";
		}

        private string ScoreFor(string teamCode)
        {
            if (IsHomeTeam(teamCode))
                return $"{HomeScore}";
            return $"{AwayScore}";
        }

        private bool IsHomeTeam(string teamCode)
		{
			return HomeTeam.Equals(teamCode);
		}

		private string ResultFor(
            string teamCode)
		{
            if (WinFor(teamCode))
                return "W";
            else if (LossFor(teamCode))
                return "L";
            return "T";
        }

		internal bool Involves(
            string teamCode)
		{
            if (HomeTeam.Equals(teamCode))
                return true;
            if (AwayTeam.Equals(teamCode))
                return true;
            return false;
        }

		internal string GameResultShort(
            string teamCode)
		{
            var shortResult = "T  ";
            if (WinFor(teamCode))
                shortResult = $"+{WinningMargin(),2}";
            else if (LossFor(teamCode))
                shortResult = $"-{WinningMargin(),2}";

            return $"R{Round,2} {shortResult}";
        }

		internal bool HasBeenPlayed()
		{
            return (AwayScore + HomeScore) > 0;
		}
	}
}
