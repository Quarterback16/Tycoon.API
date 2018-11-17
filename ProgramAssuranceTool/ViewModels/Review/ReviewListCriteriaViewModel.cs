using System;

namespace ProgramAssuranceTool.ViewModels.Review
{
	[Serializable]
	public class ReviewListCriteriaViewModel
	{
		public int ProjectId { get; set; }
		public string ReviewId { get; set; }
		public string UploadId { get; set; }
		public string ClaimId { get; set; }
		public string JobseekerId { get; set; }
		public string ActivityId { get; set; }
		public string AssessmentCode { get; set; }
		public string OutcomeCode { get; set; }
		public string AssessmentAction { get; set; }
		public string RecoveryReason { get; set; }
		public string SiteCode { get; set; }
		public string EsaCode { get; set; }
		public string OrgCode { get; set; }
		public string StateCode { get; set; }
	}
}