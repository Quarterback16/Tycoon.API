using System;
using ProgramAssuranceTool.Infrastructure.DataAnnotations;

namespace ProgramAssuranceTool.Infrastructure.Models
{
	/// <summary>
	/// Related adw code model.
	/// </summary>
	public class RelatedCodeModel
	{
		/// <summary>
		/// Related code
		/// </summary>
		[Alias( "relation_type_cd" )]
		public string RelatedCode { get; set; }

		/// <summary>
		/// Whether the code was retrieved with a dominant search.
		/// </summary>
		public bool Dominant { get; set; }

		/// <summary>
		/// Dominant code
		/// </summary>
		[Alias( "dom_code" )]
		public string DominantCode { get; set; }

		/// <summary>
		/// Dominant long description
		/// </summary>
		[Alias( "dom_long_desc" )]
		public string DominantDescription { get; set; }

		/// <summary>
		/// Dominant short description
		/// </summary>
		[Alias( "dom_short_desc" )]
		public string DominantShortDescription { get; set; }

		/// <summary>
		/// Subordinate code
		/// </summary>
		[Alias( "sub_code" )]
		public string SubordinateCode { get; set; }

		/// <summary>
		/// Subordinate long description
		/// </summary>
		[Alias( "sub_long_desc" )]
		public string SubordinateDescription { get; set; }

		/// <summary>
		/// Subordinate short description
		/// </summary>
		[Alias( "sub_short_desc" )]
		public string SubordinateShortDescription { get; set; }

		/// <summary>
		/// Currency Start date time
		/// </summary>
		[Alias( "currency_start_dt" )]
		public DateTime? StartDate { get; set; }

		/// <summary>
		/// Currency end date time
		/// </summary>
		[Alias( "currency_end_dt" )]
		public DateTime? EndDate { get; set; }

		/// <summary>
		/// Position
		/// </summary>
		[Alias( "id" )]
		public int Position { get; set; }

		internal object ToCodeModel()
		{
			throw new NotImplementedException();
		}
	}
}