using System;

namespace ProgramAssuranceTool.Models
{
    public class SiteVisit
    {
        public string OrgCode { get; set; }

        public string OrgName { get; set; }

        public string ESACode { get; set; }

        public string ESAName { get; set; }

        public string SiteCode { get; set; }

        public string SiteName { get; set; }

        public int ProjectID { get; set; }

        public string ProjectName { get; set; }

        public int JobSeekerID { get; set; }

        public string JobSeekerFamilyName { get; set; }

        public string JobSeekerFirstName { get; set; }

        public string JobSeekerName { get; set; }

        public int ClaimID { get; set; }

        public string ClaimType { get; set; }

        public string ClaimTypeDescription  { get; set; }

        public decimal ClaimAmount { get; set; }

        public DateTime ClaimCreationDate { get; set; }

        public string ContractType { get; set; }

        public int DaysOverdue { get; set; }

        public string AssessmentAction { get; set; }

        public string AssessmentOutcome { get; set; }

        public decimal FinalOutcome { get; set; }

        public DateTime LastUpdateDate { get; set; }

        public string RecoveryReason { get; set; }

        public string ReviewStatus { get; set; }

        public DateTime UploadDate { get; set; }
    }
}