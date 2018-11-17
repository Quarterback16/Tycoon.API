using System.Web.Mvc;
using ProgramAssuranceTool.Infrastructure.DataAnnotations;
using ProgramAssuranceTool.Infrastructure.Interfaces;

namespace ProgramAssuranceTool.Infrastructure.Registrations
{
	/// <summary>
	/// Represents a registration that is used to register global filters.
	/// </summary>
	[Order( 1 )]
	public class GlobalFilterRegistration : IRegistration
	{
		/// <summary>
		/// Register global filters.
		/// </summary>
		public void Register()
		{
			GlobalFilters.Filters.Add( new HandleAllErrorsAttribute() );
		}
	}
}