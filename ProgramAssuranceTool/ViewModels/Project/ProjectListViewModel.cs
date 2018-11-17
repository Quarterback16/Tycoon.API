using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ProgramAssuranceTool.Infrastructure.DataAnnotations;

namespace ProgramAssuranceTool.ViewModels.Project
{
	public class ProjectListViewModel : IValidatableObject
	{
		[Display(Name = "Uploads From (dd/mm/yyyy)")]
		[DataType(DataType.Date)]
		[ValidDate(ErrorMessage = "Date format must be dd/mm/yyyy")]
		public DateTime? UploadFrom { get; set; }

		[Display(Name = "Uploads To (dd/mm/yyyy)")]
		[DataType(DataType.Date)]
		[ValidDate(ErrorMessage = "Date format must be dd/mm/yyyy")]
		public DateTime? UploadTo { get; set; }

		public string SavedSettings { get; set; }


		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (UploadFrom.HasValue)
			{
				if (UploadTo.HasValue)
				{
					if (UploadTo.Value < UploadFrom.Value)
					{
						yield return new ValidationResult("Upload To date cannot be before the Upload From date", new[] {"UploadTo"});
					}

					if (UploadFrom > DateTime.Now)
					{
						yield return new ValidationResult("Upload From date cannot be in the future", new[] {"UploadFrom"});
					}

					if (UploadTo > DateTime.Now)
					{
						yield return new ValidationResult("Upload To date cannot be in the future", new[] {"UploadTo"});
					}
				}
				else
				{
					yield return new ValidationResult("Upload To date cannot be blank", new[] {"UploadTo"});
				}
			}
			else
			{
				if (UploadTo.HasValue)
				{
					yield return new ValidationResult("Upload From date cannot be blank", new[] {"UploadFrom"});
				}
			}
		}
	}
}