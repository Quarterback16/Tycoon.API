using System;
using System.ComponentModel.DataAnnotations;

namespace ProgramAssuranceTool.Models
{
	public class PatControl : AuditEntity
	{
		[Key]
		public int ControlId { get; set; }
		public bool SystemAvailable { get; set; }
		public int ProjectCount { get; set; }
		public int SampleCount { get; set; }
		public int ReviewCount { get; set; }
		public decimal ProjectCompletion { get; set; }
		public decimal TotalComplianceIndicator { get; set; }
		public DateTime LastBatchRun { get; set; }
		public DateTime LastComplianceRun { get; set; }
	}
}