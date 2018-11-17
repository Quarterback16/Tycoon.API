using System.ComponentModel.DataAnnotations;

namespace ProgramAssuranceTool.ViewModels.Project
{
	public class UploadQuestionsViewModel : BaseViewModel
	{
		[Display( Name = "Source File" )]
		public string SourceFile { get; set; }

		[ScaffoldColumn( true )]
		public string ServerFile { get; set; }
	}
}