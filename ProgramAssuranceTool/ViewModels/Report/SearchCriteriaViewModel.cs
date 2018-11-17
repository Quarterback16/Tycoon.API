using ProgramAssuranceTool.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProgramAssuranceTool.ViewModels.Report
{
    public class SearchCriteriaViewModel : IValidatableObject
    {
        [Display(Name = "Search type")]
        public bool IsAdvanceSearchType { get; set; }

        [Display(Name = "Organisation")]
        public string OrgCode { get; set; }

        [Display(Name = "ESA Code")]
        public string ESACode { get; set; }

        [Display(Name = "Contract Type")]
        [UIHint("AdwDropdownList")]
        [AdwCodeList(DataConstants.AdwListCodeForProjectContracts, true, false)]
        public string ContractType { get; set; }

        [Display(Name = "Site Code")]
        public string SiteCode { get; set; }

        [Display(Name = "Sample Date from (dd/mm/yyyy)")]
        [DataType(DataType.Date)]
        public DateTime? UploadDateFrom { get; set; }

		  [Display(Name = "Sample Date to (dd/mm/yyyy)")]
        [DataType(DataType.Date)]
        public DateTime? UploadDateTo { get; set; }

        [Display(Name = "Project ID")]
        public string ProjectID { get; set; }

        [Display(Name = "Project Type")]
        [UIHint("AdwDropdownList")]
        [AdwCodeList(DataConstants.AdwListCodeForProjectTypes, true, false)]
        public string ProjectType { get; set; }

        [Display(Name = "Sort Column")]
        public string SortColumn { get; set; }

        [Display(Name = "Sort By")]
        public string SortBy { get; set; }

		  public int TotalRecords { get; set; }

	    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // validate mandatory field
            if (!IsAdvanceSearchType)
            {
                // must have org code
                if (string.IsNullOrWhiteSpace(OrgCode))
                {
	                yield return new ValidationResult("Organisation is mandatory, Please specify Organisation.", new[] {"OrgCode"});
                }
            }
            else
            {
                // must have either project id or project type
                if (string.IsNullOrWhiteSpace(ProjectID) && string.IsNullOrWhiteSpace(ProjectType))
                {
	                yield return new ValidationResult("Project ID or Project Type is mandatory, Please specify Project ID or Project Type", new[] {"ProjectID", "ProjectType"});
                }

            }

            if (UploadDateFrom.HasValue && UploadDateTo.HasValue && UploadDateFrom.Value > UploadDateTo.Value)
            {
					yield return new ValidationResult("Sample Date To must be greater than or equal to the Sample Date From.", new[] { "UploadDateTo" });
            }
        }
    }
}