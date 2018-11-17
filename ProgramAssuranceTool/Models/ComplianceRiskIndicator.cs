namespace ProgramAssuranceTool.Models
{
    public class ComplianceRiskIndicator
    {
        public string OrgCode { get; set; }

        public string ESACode { get; set; }

        public string SiteCode { get; set; }

		  public string ProjectType { get; set; }

        public decimal TotalCompliancePoint { get; set; }

        public decimal ComplianceIndicator { get; set; }

        public int InProgressReviewCount { get; set; }

        public int CompletedReviewCount { get; set; }

        public int TotalReviewCount { get; set; }

        public decimal TotalRecoveryAmount { get; set; }

        public int ValidCount { get; set; }

        public int ValidAdminCount { get; set; }

        public int InvalidAdminCount { get; set; }

        public int InvalidRecovery { get; set; }

        public int InvalidNoRecovery { get; set; }

    }
}