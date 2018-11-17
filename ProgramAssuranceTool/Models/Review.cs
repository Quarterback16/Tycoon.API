using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Repositories;

namespace ProgramAssuranceTool.Models
{
	[Serializable]
	public class Review : AuditEntity, IValidatableObject
	{
		private readonly IAdwRepository _adwRepository;

		public Review( IAdwRepository adwRepository )
		{
			_adwRepository = adwRepository;
		}

		public Review() : this( new AdwRepository() )
		{
		}

		[Key]
		[Display( Name = "Review ID" )]
		public int ReviewId { get; set; }

		[Display( Name = "Upload ID" )]
		public int UploadId { get; set; }

		[Display( Name = "Project ID" )]
		public int ProjectId { get; set; }

		[Display( Name = "Org Code" )]
		public string OrgCode { get; set; }

		[Display( Name = "ESA Code" )]
		public string ESACode { get; set; }

		[Display( Name = "Site Code" )]
		public string SiteCode { get; set; }

		[Display( Name = "State Code" )]
		public string StateCode { get; set; }

		[Display( Name = "Managed By" )]
		public string ManagedBy { get; set; }

		[Display( Name = "Claim ID" )]
		public long ClaimId { get; set; }

		[Display( Name = "Jobseeker ID" )]
		public long JobseekerId { get; set; }

		[Display( Name = "Activity ID" )]
		public long ActivityId { get; set; }

		[Display( Name = "Claim Sequence Number" )]
		public int ClaimSequenceNumber { get; set; }

		[Display( Name = "Claim amount" )]
		public decimal ClaimAmount { get; set; }

		[Display( Name = "Claim Recovery amount" )]
		public decimal ClaimRecoveryAmount { get; set; }

		[Display( Name = "Additional Review" )]
		public bool IsAdditionalReview { get; set; }

		[Display( Name = "Out of Scope" )]
		public bool IsOutOfScope { get; set; }

		[Display( Name = "Partial Recovery" )]
		public bool PartialRecovery { get; set; }

		[Display( Name = "Comments" )]
		[StringLength( 3000 )]
		[AllowHtml]
		public string Comments { get; set; }

		[Display( Name = "Site Name" )]
		public string SiteName { get; set; }

		[Display( Name = "Organisation Name" )]
		public string OrgName { get; set; }

		#region  New 2014 Fields

		[Display( Name = "Final Outcome" )]
		[UIHint( "AdwDropdownList" )]
		[AdwCodeList( DataConstants.AdwListCodeForOutcomeCodes, true, false )]
		public string OutcomeCode { get; set; }

		[Display( Name = "Assessment Outcome" )]
		[UIHint( "AdwDropdownList" )]
		[AdwCodeList( DataConstants.AdwListCodeForAssessmentCodes, true, false )]
		public string AssessmentCode { get; set; }

		[Display( Name = "Recovery Reason" )]
		[UIHint( "AdwDropdownList" )]
		[AdwCodeList( DataConstants.AdwListCodeForRecoveryReasonCodes, true, false )]
		public string RecoveryReason { get; set; }

		[Display( Name = "Assessment Action" )]
		[UIHint( "AdwDropdownList" )]
		[AdwCodeList( DataConstants.AdwListCodeForAssessmentActionCodes, true, false )]
		public string AssessmentAction { get; set; }

		[Display( Name = "Assessment Date" )]
		public DateTime AssessmentDate { get; set; }

		[Display( Name = "Assessment Action Date" )]
		public DateTime AssessmentActionDate { get; set; }

		[Display( Name = "Recovery Reason Date" )]
		public DateTime RecoveryReasonDate { get; set; }

		[Display( Name = "Final Outcome Date" )]
		public DateTime FinalOutcomeDate { get; set; }

		[Display( Name = "Job Seeker Given Name" )]
		public string JobSeekerGivenName { get; set; }

		[Display( Name = "Job Seeker Surname" )]
		public string JobSeekerSurname { get; set; }

		[Display( Name = "Contract Type" )]
		public string ContractType { get; set; }

		[Display( Name = "Claim Type" )]
		public string ClaimType { get; set; }

		[Display( Name = "Claim Creation Date" )]
		public DateTime ClaimCreationDate { get; set; }

		[Display( Name = "Auto Special Claim Flag" )]
		public bool AutoSpecialClaim { get; set; }

		[Display( Name = "Manual Special Claim Flag" )]
		public bool ManualSpecialClaim { get; set; }

		[Display(Name = "Is CheckList Completed")]
		public bool IsCheckListCompleted { get; set; }

		#endregion

		public IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
		{
			if (!string.IsNullOrEmpty(AssessmentCode))
			{
				if (string.IsNullOrEmpty(_adwRepository.GetDescription(
					DataConstants.AdwListCodeForAssessmentCodes, AssessmentCode, false)))	// check regardless of the adw end date status and must be exists
					yield return new ValidationResult("Assessment outcome is not valid", new[] { "AssessmentCode" });
			}

			if (!string.IsNullOrEmpty(RecoveryReason))
			{
				if (string.IsNullOrEmpty(_adwRepository.GetDescription(
					DataConstants.AdwListCodeForRecoveryReasonCodes, RecoveryReason, false)))	// check regardless of the adw end date status and must be exists
					yield return new ValidationResult("Recovery reason is not valid", new[] { "RecoveryReason" });
			}

			if (!string.IsNullOrEmpty(AssessmentAction))
			{
				if (string.IsNullOrEmpty(_adwRepository.GetDescription(
					DataConstants.AdwListCodeForAssessmentActionCodes, AssessmentAction, false)))	// check regardless of the adw end date status and must be exists
					yield return new ValidationResult("Assessment action is not valid", new[] { "AssessmentAction" });
			}

			if (!string.IsNullOrEmpty(OutcomeCode))
			{
				if (string.IsNullOrEmpty( _adwRepository.GetDescription(
					DataConstants.AdwListCodeForOutcomeCodes, OutcomeCode, false)))	// check regardless of the adw end date status and must be exists
					yield return new ValidationResult( "Final outcome is not valid", new[] {"OutcomeCode"} );
			}

			if ( ClaimRecoveryAmount > ClaimAmount )
				yield return new ValidationResult( "Recovery amount cannot be greater than the amount of the claim", new[] { "ClaimRecoveryAmount" } );

			if ( ClaimRecoveryAmount < 0 )
				yield return new ValidationResult( "Recovery amount cannot be negative", new[] { "ClaimRecoveryAmount" } );

		}

		public bool HasOutcome()
		{
			return !string.IsNullOrEmpty( OutcomeCode );
		}

		public bool Converted()
		{
			return UpdatedBy.Equals( DataConstants.ConversionUserId );
		}

		public string Status()
		{
			return HasOutcome() ? "Completed" : "In Progress";
		}

		public bool CanEdit( PatUser user, string resourceSet )
		{
			var canEdit = false;
			if ( user.IsAdministrator() )
				canEdit = true;
			else
			{
				if ( user.IsInAnyOfTheseGroups( resourceSet ) )
					canEdit = true;
			}
			return canEdit;
		}

		public bool CanDelete( PatUser user )
		{
			return user.IsAdministrator();
		}

		public override string ToString()
		{
			return string.Format( "{0}-{1}-{2} locn {3}-{4}-{5} outcome [{6}]",
										 ReviewId, UploadId, ProjectId, OrgCode, ESACode, SiteCode, OutcomeCode );
		}

		public string Quarter()
		{
			var reviewDate = ClaimCreationDate == new DateTime( 1, 1, 1 )
									  ? new DateTime( 2011, 7, 1 )
									  : ClaimCreationDate;

			var quarter = AppHelper.QuarterFor( reviewDate );
			return quarter;			
		}
	}
}