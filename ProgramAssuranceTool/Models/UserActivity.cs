using System.ComponentModel.DataAnnotations;

namespace ProgramAssuranceTool.Models
{
	public class UserActivity : AuditEntity
	{
		[Key]
		public int ActivityId { get; set; }

		[StringLength( 60 )]
		public string Activity { get; set; }

		[StringLength( 10 )]
		public string UserId { get; set; }
	}
}
