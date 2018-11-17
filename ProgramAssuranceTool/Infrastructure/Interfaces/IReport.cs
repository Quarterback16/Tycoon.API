using ProgramAssuranceTool.Infrastructure.Services;

namespace ProgramAssuranceTool.Infrastructure.Interfaces
{
    /// <summary>
    /// Defines the methods and properties that are required for a Report.
    /// </summary>
    /// <remarks>
    /// All ActiveReport classes within Mvc should inherit this interface to registerable with the Container Provider, so they can be used with the <see cref="ReportService" /> create methods.
    /// </remarks>
    public interface IReport
    {

    }
}
