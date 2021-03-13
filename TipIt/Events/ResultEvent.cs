using Newtonsoft.Json;
using System;
using TipIt.Interfaces;

namespace TipIt.Events
{
    public class ResultEvent : IEvent
    {
        [JsonProperty("EventType")]
        public string EventType { get; set; }  // basically the aggregate

        [JsonProperty("League")]
        public string LeagueCode { get; set; }

        [JsonProperty("Round")]
        public int Round { get; set; }

        [JsonProperty("GameDate")]
        public DateTime GameDate { get; set; }

        [JsonProperty("HomeTeam")]
        public string HomeTeam { get; set; }

        [JsonProperty("AwayTeam")]
        public string AwayTeam { get; set; }

        [JsonProperty("HomeScore")]
        public int HomeScore { get; set; }

        [JsonProperty("AwayScore")]
        public int AwayScore { get; set; }

        public Guid Id
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
        public int Version
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
        public DateTimeOffset TimeStamp
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override string ToString()
		{
            return $@"{
                LeagueCode
                } R{Round,2} {GameDate.ToString("yyyy-MM-dd")} {
                HomeTeam
                } {
                HomeScore,2
                } {
                AwayTeam
                } {
                AwayScore,2
                }";
		}
    }
}
