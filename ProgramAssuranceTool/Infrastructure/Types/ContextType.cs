using System.ComponentModel;

namespace ProgramAssuranceTool.Infrastructure.Types
{
	/// <summary>
	/// Represents an enum which defines the context type.
	/// </summary>
	public enum ContextType
	{
		/// <summary>
		/// Activity context.
		/// </summary>
		[Description( "activity" )]
		Activity,

		/// <summary>
		/// Appointment context.
		/// </summary>
		[Description( "appointment" )]
		Appointment,

		/// <summary>
		/// Contract context.
		/// </summary>
		[Description( "contract" )]
		Contract,

		/// <summary>
		/// Centrelink reference number context.
		/// </summary>
		[Description( "Centrelink Reference Number" )]
		CRN,

		/// <summary>
		/// Employer context.
		/// </summary>
		[Description( "employer" )]
		Employer,

		/// <summary>
		/// Job seeker context.
		/// </summary>
		[Description( "job seeker" )]
		JobSeeker,

		/// <summary>
		/// Override context.
		/// </summary>
		[Description( "activity" )]
		Override,

		/// <summary>
		/// Payment context.
		/// </summary>
		[Description( "payment" )]
		Payment,

		/// <summary>
		/// Provider context.
		/// </summary>
		[Description( "provider" )]
		Provider,

		/// <summary>
		/// Vacancy context.
		/// </summary>
		[Description( "vacancy" )]
		Vacancy
	}
}