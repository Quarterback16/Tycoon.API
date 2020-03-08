using System;
using Newtonsoft.Json;
using TipIt.Interfaces;

namespace TipIt.Events
{
    public class GamePlayedEvent : IEvent
    {
        [JsonProperty("event-type")]
        public string EventType { get; set; }  // basically the aggregate

        [JsonProperty("h-team")]
        public string HomeTeam { get; set; }

        [JsonProperty("h-score")]
        public int HomeScore { get; set; }

        [JsonProperty("a-team")]
        public string AwayTeam { get; set; }

        [JsonProperty("a-score")]
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

    }
}
