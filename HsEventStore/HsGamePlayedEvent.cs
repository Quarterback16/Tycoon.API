using System;
using Newtonsoft.Json;

namespace HsEventStore
{
	public class HsGamePlayedEvent : IEvent
	{
		[JsonProperty("event-type")]
		public string EventType { get; set; }  // basically the aggregate

		[JsonProperty("date-played")]
		public DateTime DatePlayed { get; set; }

		[JsonProperty("h-deck")]
		public string HomeDeck { get; set; }

		[JsonProperty("o-deck")]
		public string OpponentDeck { get; set; }

		[JsonProperty("opp")]
		public string Opponent { get; set; }
		
		[JsonProperty("result")]
		public string Result { get; set; }

		[JsonProperty("rank")]
		public int Rank { get; set; }

		[JsonProperty("run")]
		public int Run { get; set; }

		[JsonProperty("notes")]
		public string Notes { get; set; }

		public Guid Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public int Version { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public DateTimeOffset TimeStamp { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
	}
}
