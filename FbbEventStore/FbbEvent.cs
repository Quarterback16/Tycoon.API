using System;
using Newtonsoft.Json;

namespace FbbEventStore
{
	public class FbbEvent : IEvent
	{
		[JsonProperty("trans")]
		public string Direction { get; set; }  

		[JsonProperty("player")]
		public string Player { get; set; }

		[JsonProperty("date")]
		public string TransactionDate { get; set; }

		[JsonProperty("fteam")]
		public string FantasyTeam { get; set; }

		[JsonProperty("team")]
		public string MlbTeam { get; set; }

		[JsonProperty("pos")]
		public string Position { get; set; }

		public Guid Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public int Version { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public DateTimeOffset TimeStamp { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

	}
}
