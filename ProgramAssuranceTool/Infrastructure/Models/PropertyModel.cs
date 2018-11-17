using System;
using ProgramAssuranceTool.Infrastructure.DataAnnotations;

namespace ProgramAssuranceTool.Infrastructure.Models
{
	/// <summary>
	/// Adw property model.
	/// </summary>
	public class PropertyModel
	{
		/// <summary>
		/// Adw property type code.
		/// </summary>
		[Alias( "property_type_code" )]
		public string PropertyType { get; set; }

		/// <summary>
		/// Adw code type.
		/// </summary>
		[Alias( "code_type" )]
		public string CodeType { get; set; }

		/// <summary>
		/// Adw code.
		/// </summary>
		[Alias( "code" )]
		public string Code { get; set; }

		/// <summary>
		/// Adw value.
		/// </summary>
		[Alias( "value_text" )]
		public string Value { get; set; }

		/// <summary>
		/// Currency start date time.
		/// </summary>
		[Alias( "currency_start_dt" )]
		public DateTime? StartDate { get; set; }

		/// <summary>
		/// Currency end date time.
		/// </summary>
		[Alias( "currency_end_dt" )]
		public DateTime? EndDate { get; set; }
	}
}