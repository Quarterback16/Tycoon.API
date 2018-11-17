using System.IO;
using System.Web.Mvc;
using DataDynamics.ActiveReports;
using DataDynamics.ActiveReports.Export.Html;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Infrastructure.Services
{
    /// <summary>
    /// Defines a service for working with reports.
    /// </summary>
    public class ReportService : IReportService
    {
        /// <summary>
        /// Create the specified report.
        /// </summary>
        /// <returns>Instance of report.</returns>
        public T Create<T>() where T : ActiveReport
        {
            return DependencyResolver.Current.GetService<T>();
        }

        /// <summary>
        /// Create the specified report and bind datasource.
        /// </summary>
        /// <param name="datasource">Data source to bind.</param>
        /// <returns>Instance of report.</returns>
        public T Create<T>(object datasource) where T : ActiveReport
        {
            var report = DependencyResolver.Current.GetService<T>();

            report.DataSource = datasource;

            return report;
        }

        /// <summary>
        /// Convert report to PDF <see cref="FileStreamResult" />.
        /// </summary>
        /// <typeparam name="T">Type of report.</typeparam>
        /// <param name="report">Instance of report to convert.</param>
        /// <param name="name">Name of report.</param>
        /// <returns>The report as a PDF <see cref="FileStreamResult" />.</returns>
        public FileStreamResult ToPdfResult<T>(T report, string name) where T : ActiveReport
        {
            report.PageSettings.DefaultPaperSize = true;
            report.PageSettings.DefaultPaperSource = true;

            return report.ToPdfResult(name);
        }

        /// <summary>
        /// Convert report to Rtf <see cref="FileStreamResult" />.
        /// </summary>
        /// <typeparam name="T">Type of report.</typeparam>
        /// <param name="report">Instance of report to convert.</param>
        /// <param name="name">Name of report.</param>
        /// <returns>The report as a Rtf<see cref="FileStreamResult" />.</returns>
        public Stream ToRtfResult<T>(T report, string name) where T : ActiveReport
        {
            var rtf = new DataDynamics.ActiveReports.Export.Rtf.RtfExport();
            var ms = new MemoryStream();

            report.PageSettings.DefaultPaperSize = true;
            report.PageSettings.DefaultPaperSource = true;

            report.Run();
            rtf.Export(report.Document, ms);
            
            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }
    }
}