using System.ComponentModel;

namespace ProgramAssuranceTool.Infrastructure.Types
{
	/// <summary>
	/// Represents an enum which defines the history type.
	/// </summary>
	public enum HistoryType
	{
		/// <summary>
		/// Activity history.
		/// </summary>
		[Description( "activity" )]
		Activity,

		/// <summary>
		/// Contract history.
		/// </summary>
		[Description( "contract" )]
		Contract,

		/// <summary>
		/// Employer history.
		/// </summary>
		[Description( "employer" )]
		Employer,

		/// <summary>
		/// Job seeker history.
		/// </summary>
		[Description( "job seeker" )]
		JobSeeker,

		/// <summary>
		/// Provider context.
		/// </summary>
		[Description( "provider" )]
		Provider,

		/// <summary>
		/// Vacancy history.
		/// </summary>
		[Description( "vacancy" )]
		Vacancy
	}
}