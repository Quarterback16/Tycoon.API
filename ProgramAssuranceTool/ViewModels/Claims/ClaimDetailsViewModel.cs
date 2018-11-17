using System;

namespace ProgramAssuranceTool.ViewModels.Claims
{
	public class ClaimDetailsViewModel
	{
		public long ClaimId { get; set; }
		public int ClaimSequenceNumber { get; set; }
		public string ClaimType { get; set; }
		public string ContractType { get; set; }

		public DateTime ClaimCreationDate { get; set; }
		public bool AutoSpecial { get; set; }
		public bool ManualSpecial { get; set; }

		public string ErrorMessage { get; set; }
	}
}