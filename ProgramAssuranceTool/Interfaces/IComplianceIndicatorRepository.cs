using ProgramAssuranceTool.Models;

namespace ProgramAssuranceTool.Interfaces
{
	public interface IComplianceIndicatorRepository
	{
		/// <summary>
		/// Updates the Compliance indicator.
		/// </summary>
		/// <param name="entity">The CI data.</param>
		void Update( ComplianceIndicator entity );
	}
}
