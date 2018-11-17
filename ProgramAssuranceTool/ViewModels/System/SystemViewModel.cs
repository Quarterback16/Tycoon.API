using System;
using System.ComponentModel.DataAnnotations;


namespace ProgramAssuranceTool.ViewModels.System
{
	public class SystemViewModel
	{
		[Display( Name = "System Name" )]
		public string SystemName { get; set; }
		[Display( Name = "Current Date" )]
		public DateTime CurrentTime { get; set; }

	}
}