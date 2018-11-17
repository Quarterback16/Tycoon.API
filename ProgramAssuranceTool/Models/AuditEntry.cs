using System;
using System.ComponentModel.DataAnnotations;

namespace ProgramAssuranceTool.Models
{
	[Serializable]
	public class AuditEntity
	{
		[StringLength( 10 )]
		[ScaffoldColumn( false )]
		public string CreatedBy { get; set; }

		[ScaffoldColumn( false )]
		public DateTime? CreatedOn { get; set; }

		[StringLength( 10 )]
		[ScaffoldColumn( false )]
		public string UpdatedBy { get; set; }

		[ScaffoldColumn( false )]
		public DateTime? UpdatedOn { get; set; }

		[ScaffoldColumn( false )]
		public long? Version { get; set; }

	}
}