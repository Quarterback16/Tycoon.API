using System;

namespace ProgramAssuranceTool.Models
{
    public class Progress
    {
        public string SampleName{ get; set; }

        public string ProjectType { get; set; }

        public int InProgressReviewCount { get; set; }

        public int CompletedReviewCount { get; set; }

        public int TotalReviewCount { get; set; }

        public decimal PercentCompleted { get; set; }

        public DateTime LastUpdateDate { get; set; }

    }
}