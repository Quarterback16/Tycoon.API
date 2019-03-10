using System;
using Newtonsoft.Json;

namespace HsEventStore
{
    public class HsEvent
    {
        [JsonProperty("event-type")]
        public string EventType { get; set; }  // basically the aggregate

        [JsonProperty("deck-name")]
        public string DeckName { get; set; }

        [JsonProperty("direction")]
        public string Direction { get; set; }

        [JsonProperty("date-of-effect")]
        public DateTime DateOfEffect { get; set; }
    }
}
