using System.Collections.Generic;
using System.Web.Mvc;

namespace ProgramAssuranceTool.ViewModels.Project
{
	public class ProjectCreateViewModel
	{
		public Models.Project Project { get; set; }
		public IEnumerable<SelectListItem> Coordinators { get; set; }

		public int OriginalProjectId { get; set; }
		public string OriginalProjectName { get; set; }

		public bool UserIsAdministrator { get; set; }
	}
}