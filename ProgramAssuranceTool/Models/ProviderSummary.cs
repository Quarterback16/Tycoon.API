namespace ProgramAssuranceTool.Models
{
    public class ProviderSummary
    {
        public string OrgCode { get; set; }

        public string ESACode { get; set; }

        public string SiteCode { get; set; }

        public string State { get; set; }

        public int RecoveryCount { get; set; }

        public int CompletedReviewCount { get; set; }

        public int TotalReviewCount { get; set; }

        public int ValidCount { get; set; }

        public int InvalidRecovery { get; set; }

        public int InvalidNoRecovery { get; set; }

        public int ValidAdminCount { get; set; }

        public int InvalidAdminCount { get; set; }
    }
}