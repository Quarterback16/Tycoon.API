using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ProgramAssuranceTool.Helpers;

namespace ProgramAssuranceTool.Models
{
	public class UserSetting : AuditEntity
	{
		[Key]
		public int Id { get; set; }

		[StringLength( 50 )]
		public string UserId { get; set; }

		[StringLength( 50 )]
		public string Name { get; set; }

		[StringLength( 10 )]
		public string SerialiseAs { get; set; }

		[StringLength( 250 )]
		public string Value { get; set; }

		public string SettingKey()
		{
			return Name == CommonConstants.ReviewListColumn ? Value : Name;
		}

		public IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
		{
			if ( !string.IsNullOrEmpty( SerialiseAs ) )
			{
				if ( ! ( SerialiseAs.Equals( "string" ) ||
							SerialiseAs.Equals( "numeric" ) ||
							SerialiseAs.Equals( "date" ) ||
							SerialiseAs.Equals( "currency" ) ) )
					yield return new ValidationResult( "SerialiseAs is not valid", new[] { "SerialiseAs" } );
			}
		}
	}
}