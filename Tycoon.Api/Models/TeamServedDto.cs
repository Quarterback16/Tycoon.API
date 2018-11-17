using System;

namespace Tycoon.Api.Models
{
	public class TeamServedDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime From { get; set; }
		public DateTime To { get; set; }
	}
}
