using System;
using ProgramAssuranceTool.Helpers;

namespace ProgramAssuranceTool.ViewModels.Sample
{
	public class UploadReviewListViewModel
	{
		public Models.Upload Upload { get; set; }
		public Models.Project Project { get; set; }
		public int Margin { get; set; }
		public int Width { get; set; }
		public bool CanEdit { get; set; }
	}
}