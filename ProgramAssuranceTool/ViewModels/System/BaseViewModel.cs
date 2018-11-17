using System;

namespace ProgramAssuranceTool.ViewModels
{
	[Serializable]
	public class BaseViewModel
	{
		public int ProjectId { get; set; }
		public string ProjectName { get; set; }
	}
}