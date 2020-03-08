using Newtonsoft.Json;
using System;
using TipIt.Interfaces;

namespace TipIt.Events
{
    public class AddTeamEvent : IEvent
    {
        [JsonProperty("event-type")]
        public string EventType { get; set; }  // basically the aggregate

        [JsonProperty("league-code")]
        public string LeagueCode { get; set; }

        [JsonProperty("team-id")]
        public string TeamId { get; set; }

        [JsonProperty("team-name")]
        public string TeamName { get; set; }

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
