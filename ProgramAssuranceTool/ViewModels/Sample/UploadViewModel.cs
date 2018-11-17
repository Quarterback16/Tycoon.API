using System.ComponentModel.DataAnnotations;

namespace ProgramAssuranceTool.ViewModels.Sample
{
	public class UploadViewModel : BaseViewModel
	{
		[Display( Name = "Source File" )]
		public string SourceFile { get; set; }

		public bool IncludesOutcomes { get; set; }
		public bool AdditionalReview { get; set; }
		public bool OutOfScope { get; set; }
		public bool IsRandom { get; set; }
		public bool IsNational { get; set; }
		public bool IsAdminUser { get; set; }

	}
}