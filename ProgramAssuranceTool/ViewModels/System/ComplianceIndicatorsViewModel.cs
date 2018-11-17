using System;

namespace ProgramAssuranceTool.ViewModels.System
{
	[Serializable]
	public class ComplianceIndicatorsViewModel
	{
		public int ProjectsRead { get; set; }
		public int CompletedProjects { get; set; }
		public int SamplesRead { get; set; }
		public int ReviewsRead { get; set; }
		public int ReviewsCompleted { get; set; }
		public decimal TotalCompliancePoints { get; set; }
		public int IndicatorsGenerated { get; set; }
	}
}