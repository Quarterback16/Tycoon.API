using System;
using Newtonsoft.Json;

namespace RosterService
{
	public class RosterEvent : IEvent
	{
		[JsonProperty("trans")]
		public string Direction { get; set; }

		[JsonProperty("player")]
		public string Player { get; set; }

		//[JsonProperty("date")]
		//public string TransactionDate { get; set; }

		[JsonProperty("fteam")]
		public string FantasyTeam { get; set; }

		[JsonProperty("team")]
		public string ProTeam { get; set; }

		[JsonProperty("pos")]
		public string Position { get; set; }

		[JsonProperty("price")]
		public string Price { get; set; }

		[JsonProperty("num")]
		public string Number { get; set; }

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

		internal string ToLine()
		{
			return $"{Number,3}  {Position} {Player,-20} ({ProTeam}) {Price,3}";
		}

		public DateTimeOffset TimeStamp
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}

	}
}
