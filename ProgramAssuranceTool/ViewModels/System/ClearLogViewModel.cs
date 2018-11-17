using System;
using System.ComponentModel.DataAnnotations;
using ProgramAssuranceTool.Helpers;

namespace ProgramAssuranceTool.ViewModels.System
{
	[Serializable]
	public class ClearLogViewModel
	{

		[Display( Name = "Remove items Prior to (dd/mm/yyyy)" )]
		[DataType( DataType.Date )]
		[Required]
		[HtmlProperties( MaxLength = 10 )]
		[DisplayFormat( ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}" )]
		public DateTime PriorToDate { get; set; }

	}
}