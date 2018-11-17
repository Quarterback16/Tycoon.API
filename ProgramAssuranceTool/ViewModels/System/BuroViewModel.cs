using System;

namespace ProgramAssuranceTool.ViewModels.System
{
	[Serializable]
	public class BuroViewModel
	{
		public int ReviewsRead { get; set; }
		public int ReviewsUpdated { get; set; }
		public int ValidationErrors { get; set; }
	}
}