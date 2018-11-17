using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProgramAssuranceTool.Models
{
	public class ReviewFinding : AuditEntity, IValidatableObject
	{

		[Key]
		[Display( Name = "Review Finding ID" )]
		public int Id { get; set; }

		[Display( Name = "Review ID" )]
		public int ReviewId { get; set; }

		[Display( Name = "Finding Code Type" )]
		public int FindingCodeType { get; set; }

		[Display( Name = "Upload ID" )]
		public int UploadId { get; set; }

		[Display( Name = "Project ID" )]
		public int ProjectId { get; set; }

		[Display( Name = "Finding Code" )]
		public string FindingCode { get; set; }

		public IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
		{
			throw new NotImplementedException();
		}
	}
}