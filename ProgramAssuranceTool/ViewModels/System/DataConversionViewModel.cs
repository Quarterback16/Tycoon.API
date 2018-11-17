using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ProgramAssuranceTool.Models;

namespace ProgramAssuranceTool.ViewModels.System
{
	[Serializable]
	public class DataConversionViewModel
	{
		[Display( Name = "Report Only (No data changes)" )]
		public bool ReportOnly { get; set; }

		public PatUser ConversionUser { get; set; }

		[Display( Name = "System Name" )]
		public string SystemName { get; set; }
		[Display( Name = "Current Date" )]
		public DateTime CurrentTime { get; set; }

		public int ErrorCount { get; set; }

		public int ProjectNameIssues { get; set; }
		public int NumberOfProjectNameUpdates { get; set; }
		public List<ProjectNameError> ProjectNameErrors { get; set; }

		public int ProjectTypeIssues { get; set; }
		public int ProjectTypesUpdated { get; set; }
		public List<ProjectTypeError> ProjectTypeErrors { get; set; }

		public int ContractTypeIssues { get; set; }
		public List<ContractTypeError> ContractTypeErrors { get; set; }

		public int ResourceUpdates { get; set; }
		public List<ResourceError> ResourceErrors { get; set; }

      public int OutcomeFieldsChecked  { get; set; }
      public int NoOutcomes  { get; set; }
		public int OutcomeFieldsConverted { get; set; }
		public int OutcomeFieldIssues { get; set; }
		public List<OutcomeFieldError> OutcomeFieldErrors { get; set; }

		public int AssessmentFieldsChecked { get; set; }
		public int NoAssessment { get; set; }
		public int AssessmentFieldsConverted { get; set; }
		public int AssessmentFieldIssues { get; set; }
		public List<AssessmentFieldError> AssessmentFieldErrors { get; set; }

		public int UploadNameIssues { get; set; }
		public int UploadRecordsUpdated { get; set; }
		public List<UploadNameError> UploadNameErrors { get; set; }

		public int FindingsChecked { get; set; }
		public int NoFindings { get; set; }
		public int RecoveryReasonsConverted { get; set; }
		public int RecoveryReasonIssues { get; set; }
		public List<RecoveryReasonError> RecoveryReasonErrors { get; set; }

		public int ReviewsChecked { get; set; }
		public int AssessmentActionIssues { get; set; }
		public int AssessmentActionsConverted { get; set; }
		public int AANoOutcomes { get; set; }
		public int ValidOutcomes { get; set; }
		public List<AssessmentActionError> AssessmentActionErrors { get; set; }

		public int UploadRecords { get; set; }
		public int RandomFlagsSet { get; set; }
		public List<RandomFlagError> RandomFlagErrors { get; set; }

		public int ComplianceIndicatorRecordsUpdated { get; set; }

		public int ClaimsChecked { get; set; }
		public int ClaimDataFoundInSql { get; set; }
		public int ClaimDataFoundOnMainframe { get; set; }
		public int ClaimFieldsIssues { get; set; }
		public int ClaimFieldsConverted { get; set; }
		public int NoClaims { get; set; }
		public List<ClaimFieldsError> ClaimFieldsErrors { get; set; }

		public int JobseekersNotFound { get; set; }
		public int JobseekersFoundInSql { get; set; }

		public string TimeElapsed { get; set; }
	}

	public class ClaimFieldsError
	{
		public string ReviewId { get; set; }
		public string ClaimId { get; set; }
		public string ClaimType { get; set; }
		public string ManualSpecialFlag { get; set; }
		public string AutoSpecialFlag { get; set; }
		public string ClaimCreationDate { get; set; }
		public string ConversionStatus { get; set; }
	}

	public class RandomFlagError
	{
		public int ProjectId { get; set; }
		public string NatOrTargeted { get; set; }
		public int UploadId { get; set; }
		public string OldFlag { get; set; }
		public string NewFlag { get; set; }
		public string ConversionStatus { get; set; }
	}

	public class AssessmentActionError
	{
		public string ReviewId { get; set; }
		public string OutcomeCode { get; set; }
		public string ClaimId { get; set; }
		public string ClaimType { get; set; }
		public string ClaimAmount { get; set; }
		public string ClaimRecoveryAmount { get; set; }
		public string AssessmentAction { get; set; }
		public string ConversionStatus { get; set; }
	}

	public class RecoveryReasonError
	{
		public int ProjectId { get; set; }
		public int UploadId { get; set; }
		public int ReviewId { get; set; }
		public string OldFindingsCode { get; set; }
		public string NewRecoveryReason { get; set; }
		public string ReasonDate { get; set; }
		public string ConversionStatus { get; set; }
	}

	public class AssessmentFieldError
	{
		public int ProjectId { get; set; }
		public int UploadId { get; set; }
		public int ReviewId { get; set; }
		public string OldAssessmentCode { get; set; }
		public string NewAssessmentCode { get; set; }
		public string AssessmentDate { get; set; }
		public string ConversionStatus { get; set; }
	}

	public class OutcomeFieldError
	{
		public int ProjectId { get; set; }
		public int UploadId { get; set; }
		public int ReviewId { get; set; }
		public string OldOutcome { get; set; }
		public string NewOutcome { get; set; }
		public string OutcomeDate { get; set; }
		public string ConversionStatus { get; set; }
	}

	public class UploadNameError
	{
		public int ProjectId { get; set; }
		public int UploadId { get; set; }
		public string OldName { get; set; }
		public string NewName { get; set; }
		public string ConversionStatus { get; set; }
	}

	public class ResourceError
	{
		public int ProjectId { get; set; }
		public string OldResources { get; set; }
		public string NewResources { get; set; }
		public string ConversionStatus { get; set; }
	}

	public class ContractTypeError
	{
		public int ProjectId { get; set; }
		public string OldType { get; set; }
		public string NewType { get; set; }
		public string ConversionStatus { get; set; }
	}

	public class ProjectTypeError
	{
		public int ProjectId { get; set; }
		public string OldType { get; set; }
		public string NewType { get; set; }
		public string ConversionStatus { get; set; }
	}

	public class ProjectNameError
	{
		public int ProjectId { get; set; }
		public string OldName { get; set; }
		public string NewName { get; set; }
		public string ConversionStatus { get; set; }
	}
}