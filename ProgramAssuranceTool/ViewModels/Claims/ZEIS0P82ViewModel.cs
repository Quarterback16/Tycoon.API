using System;

namespace ProgramAssuranceTool.ViewModels.Claims
{
	public class ZEIS0P82ViewModel
	{
		public long ClaimId { get; set; }
		public int ClaimSequenceNumber { get; set; }
		public string ClaimStatusCode { get; set; }
		public DateTime ClaimStartDate { get; set; }
		public DateTime ClaimEndDate { get; set; }
		public string Stream { get; set; }
		public DateTime PlacementDate { get; set; }
		public DateTime RefPlacementComDate { get; set; }
		public DateTime RefPlacementEndDate { get; set; }
		public DateTime ReferralDate { get; set; }
		public string RefOutcomeCode { get; set; }
		public int SiteSequenceNumber { get; set; }
		public string SiteCode { get; set; }
		public string SiteSpecialistType { get; set; }
		public string EmployerName { get; set; }
		public long JobseekerId { get; set; }
		public string LMRCode { get; set; }
		public string RegStatusCode { get; set; }
		public long JobId { get; set; }
		public string JobTitleText { get; set; }
		public string JobSiteCd { get; set; }
		public DateTime JobCreationDate { get; set; }
		public string JobHoursTxt { get; set; }
		public string JobDescription { get; set; }
		public string JobTenureCd { get; set; }
		public string RefOutcomeDescription { get; set; }
		public string JobTenureDescription { get; set; }
		public string ErrorMessage { get; set; }
	}
}
