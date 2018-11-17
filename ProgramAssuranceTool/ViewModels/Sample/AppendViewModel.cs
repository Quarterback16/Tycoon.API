using System.ComponentModel.DataAnnotations;

namespace ProgramAssuranceTool.ViewModels.Sample
{
	public class AppendViewModel
	{
		public int ProjectId { get; set; }
		public int UploadId { get; set; }
		public string ProjectName { get; set; }

		public string UploadName { get; set; }

		[Display( Name = "Source File" )]
		public string SourceFile { get; set; }

		public bool IncludesOutcomes { get; set; }
		public bool AdditionalReview { get; set; }
		public bool OutOfScope { get; set; }
	}
}