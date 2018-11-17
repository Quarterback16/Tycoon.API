using System.IO;
using System.Web.Mvc;
using DataDynamics.ActiveReports;

namespace Employment.Web.Mvc.Infrastructure.Interfaces
{
    /// <summary>
    /// Defines methods and properties that are required for a Report Service.
    /// </summary>
    public interface IReportService
    {
        /// <summary>
        /// Create the specified report.
        /// </summary>
        /// <returns>Instance of report.</returns>
        T Create<T>() where T : ActiveReport;

        /// <summary>
        /// Create the specified report and bind datasource.
        /// </summary>
        /// <param name="datasource">Data source to bind.</param>
        /// <returns>Instance of report.</returns>
        T Create<T>(object datasource) where T : ActiveReport;

        /// <summary>
        /// Convert report to PDF <see cref="FileStreamResult" />.
        /// </summary>
        /// <typeparam name="T">Type of report.</typeparam>
        /// <param name="report">Instance of report to convert.</param>
        /// <param name="name">Name of report.</param>
        /// <returns>The report as a PDF <see cref="FileStreamResult" />.</returns>
        FileStreamResult ToPdfResult<T>(T report, string name) where T : ActiveReport;

        /// <summary>
        /// Convert a report to Rtf
        /// </summary>
        /// <typeparam name="T">Type of report.</typeparam>
        /// <param name="report">Instance of report to convert.</param>
        /// <param name="name">Name of report.</param>
        /// <returns>The report as a Rtf file</returns>
        Stream ToRtfResult<T>(T report, string name) where T : ActiveReport;
    }
}
