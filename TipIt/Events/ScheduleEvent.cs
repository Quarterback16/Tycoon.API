using System;
using Newtonsoft.Json;
using TipIt.Interfaces;

namespace TipIt.Events
{
    public class ScheduleEvent : IEvent
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
    }
}
