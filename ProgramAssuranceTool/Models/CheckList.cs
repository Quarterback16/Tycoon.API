using System.Web.Mvc;
using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Repositories;
using System;
using System.ComponentModel.DataAnnotations;

namespace ProgramAssuranceTool.Models
{
	[Serializable]
	public class CheckList : AuditEntity
	{
		private readonly IAdwRepository _adwRepository;

		public CheckList(IAdwRepository adwRepository)
		{
			_adwRepository = adwRepository;
		}

		public CheckList()
			: this(new AdwRepository())
		{
		}

		[Display(Name = "CheckList ID")]
		public int CheckListID { get; set; }

		[Display(Name = "Review ID")]
		public int ReviewID { get; set; }

		[Display(Name = "Status")]
		public string Status { get; set; }

		[Display(Name = "Upload ID")]
		public int UploadID { get; set; }

		[Display(Name = "Project ID")]
		public int ProjectID { get; set; }

		[Display(Name = "Project Name")]
		public string ProjectName { get; set; }

		[Display(Name = "Claim ID")]
		public long ClaimID { get; set; }

		[Display(Name = "Claim Sequence Number")]
		public int ClaimSequenceNumber { get; set; }

		[Display(Name = "Claim Type")]
		public string ClaimType { get; set; }

		[Display(Name = "Claim Type Description")]
		public string ClaimTypeDescription { get; set; }

		[Display(Name = "Claim Amount ($)")]
		public decimal ClaimAmount { get; set; }

		[Display(Name = "Claim Start Date (dd/mm/yyyy)")]
		[DataType(DataType.Date)]
		public DateTime ClaimStartDate { get; set; }

		[Display(Name = "Claim End Date (dd/mm/yyyy)")]
		[DataType(DataType.Date)]
		public DateTime ClaimEndDate { get; set; }

		[Display(Name = "Claim is a")]
		public string ClaimIs { get; set; }

		[Display(Name = "Job Seeker ID")]
		public string JobSeekerID { get; set; }

		[Display(Name = "Job Seeker Name")]
		public string JobSeekerName { get; set; }

		[Display(Name = "Assessor Name")]
		public string AssessorName { get; set; }

		[Display(Name = "Assessment Date (dd/mm/yyyy)")]
		[DataType(DataType.Date)]
		public new DateTime UpdatedOn { get; set; }

		[Display(Name = "Is the claim a duplicate for an overlapping period?")]
		[UIHint("AdwDropdownList")]
		[AdwCodeList(DataConstants.AdwListCodeForYesNo, true, false)]
		public string IsClaimDuplicateOverlapping { get; set; }

		[Display(Name = "Is the claim included in the Deed 'Non-payable Outcome' list?")]
		[UIHint("AdwDropdownList")]
		[AdwCodeList(DataConstants.AdwListCodeForYesNo, true, false)]
		public string IsClaimIncludedInDeedNonPayableOutcomeList { get; set; }

		[Display(Name = "Does the documentary evidence meet the requirement as specified in the relevant Documentary Evidence for Payment Guidelines?")]
		[UIHint("AdwDropdownList")]
		[AdwCodeList(DataConstants.AdwListCodeForYesNo, true, false)]
		public string DoesDocEvidenceMeetGuidelineRequirement { get; set; }

		[Display(Name = "Is the documentary evidence consistent with evidence provided in ESS at the time of Manual or Auto Special Claim?")]
		[UIHint("AdwDropdownList")]
		[AdwCodeList(DataConstants.AdwListCodeForYesNo, true, false)]
		public string IsDocEvidenceConsistentWithESS { get; set; }

		[Display(Name = "Is the documentary evidence sufficient to support payment type?")]
		[UIHint("AdwDropdownList")]
		[AdwCodeList(DataConstants.AdwListCodeForYesNo, true, false)]
		public string IsDocEvidenceSufficientToSupportPaymentType { get; set; }

		[Display(Name = "Comments:")]
		[StringLength(3000)]
		[HtmlProperties(MaxLength = 3000)]
		[DataType(DataType.MultilineText)]
		[AllowHtml]
		public string Comment { get; set; }

		[Display(Name = "CheckList Status")]
		public bool IsCheckListCompleted { get; set; }

		public string PreviousButtonEnabled { get; set; }

		public string NextButtonEnabled { get; set; }

		public string SaveButtonEnabled { get; set; }

		public bool CanEdit { get; set; }

	}
}