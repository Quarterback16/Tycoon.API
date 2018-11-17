using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProgramAssuranceTool.Models
{
	public class ComplianceIndicator : AuditEntity, IValidatableObject
	{
		[Key]
		[Display( Name = "Compliance IndicatorID" )]
		public int ComplianceIndicatorId { get; set; }

		public string Programme { get; set; }
		public string SubjectTypeCode { get; set; }
		public string Subject { get; set; }
		public string EsaCode { get; set; }
		public string Quarter { get; set; }
		public decimal Value { get; set; }

		public IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
		{
			if ( string.IsNullOrEmpty( SubjectTypeCode ) )
				yield return new ValidationResult( "SubjectType Code is mandatory", new[] { "SubjectTypeCode" } );
			if ( string.IsNullOrEmpty( Subject ) )
				yield return new ValidationResult( "Subject is mandatory", new[] { "Subject" } );
			if ( string.IsNullOrEmpty( EsaCode ) )
				yield return new ValidationResult( "ESA Code is mandatory", new[] { "EsaCode" } );
			if ( string.IsNullOrEmpty( Quarter ) )
				yield return new ValidationResult( "Quarter is mandatory", new[] { "Quarter" } );
		}
	}
}