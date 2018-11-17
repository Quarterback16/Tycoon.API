using System.Collections.Generic;

namespace Tycoon.Api.Models
{
	public class PlayerDto
	{
		public string PlayerId { get; set; }
		public string Name { get; set; }

		public int FantasyPoints { get; set; }

		public ICollection<TeamServedDto> TeamsServed { get; set; } 
			= new List<TeamServedDto>();
	}
}
