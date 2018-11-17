using System.ComponentModel.DataAnnotations;


namespace ProgramAssuranceTool.Models
{
	public class PatQuestion : AuditEntity
	{
		[Key]
		[Display( Name = "Project Question ID" )]
		public int ProjectQuestionId { get; set; }

		[Display( Name = "Project ID" )]
		public int ProjectId { get; set; }

		public string Type { get; set; }
		public string Text { get; set; }
		public string AnswerColumn { get; set; }

		public override string ToString()
		{
			return string.Format( "{0} - {1} - {2} ", Type, Text, AnswerColumn );
		}

	}
}