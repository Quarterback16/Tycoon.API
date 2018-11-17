using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.Interfaces
{
    /// <summary>
    /// Defines the methods and properties that are required for a Workflow.
    /// </summary>
    public interface IWorkflow
    {
        /// <summary>
        /// Indication type.
        /// </summary>
        IndicatorType? Indicator { get; set; }
    }
}
