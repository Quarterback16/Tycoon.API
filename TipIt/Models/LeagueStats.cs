namespace TipIt.Models
{
    public class LeagueStats
    {
        public string League { get; set; }
        public int TotalGames { get; set; }
        public int TotalPoints { get; set; }
        public int TeamScoreAverage { get; set; }
        public int TeamScoreMode { get; set; }

        public override string ToString()
        {
            return $@"{
                League
                } TG:{
                TotalGames
                } TP:{
                TotalPoints
                } TA:{
                TeamScoreAverage
                } TM:{
                TeamScoreMode
                }";
        }
    }
}
