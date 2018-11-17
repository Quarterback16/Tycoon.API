using System;
using System.Collections.Generic;

namespace ProgramAssuranceTool.ViewModels.System
{
	[Serializable]
	public class DiskSpaceViewModel
	{
		public List<DiskDiagnostic> Disks { get; set; }
	}

	public class DiskDiagnostic
	{
		public string Name { get; set; }
		public string AvailableFreeSpace { get; set; }
		public string PercentFreeSpace { get; set; }
		public string SpaceUsed { get; set; }
		public string DriveType { get; set; }
		public bool IsAvailable { get; set; }
	}
}