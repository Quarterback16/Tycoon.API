namespace ProgramAssuranceTool.ViewModels.Project
{
	public class ProjectDetailsViewModel
	{
		public Models.Project Project { get; set; }
		public string ResourceSet { get; set; }
		public string UserId { get; set; }
		public bool UserIsAdmin { get; set; }
		public int ContractCount { get; set; }
		public int SampleCount { get; set; }
		public int ReviewCount { get; set; }
	}
}