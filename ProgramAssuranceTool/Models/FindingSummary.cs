using System.Collections.Generic;

namespace ProgramAssuranceTool.Models
{
    public class FindingSummary
    {
        public List<FindingSummaryDetail> InScopeReview { get; set; }

        public List<FindingSummaryDetail> OutScopeReview{ get; set; }

        public List<FindingSummaryDetail> RecoveryReview { get; set; }

        public FindingSummary()
        {
            InScopeReview = new List<FindingSummaryDetail>();
            OutScopeReview = new List<FindingSummaryDetail>();
            RecoveryReview = new List<FindingSummaryDetail>();
        }
    }

    public class FindingSummaryDetail
    {
        public string Type { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public decimal ReviewCount { get; set; }
    }
}