using System.IO;
using System.Web.Mvc;
using DataDynamics.ActiveReports;
using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Infrastructure.Extensions;
using ProgramAssuranceTool.Infrastructure.Interfaces;

namespace ProgramAssuranceTool.Infrastructure.Services
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
				report.Parameters.Add(new Parameter { DefaultValue = CommonConstants.Pdf, Key = "DownloadType", PromptUser = false });

            return report.ToPdfResult(name);
        }

        /// <summary>
        /// Convert report to Rtf <see cref="FileStreamResult" />.
        /// </summary>
        /// <typeparam name="T">Type of report.</typeparam>
        /// <param name="report">Instance of report to convert.</param>
        /// <param name="name">Name of report.</param>
        /// <returns>The report as a Rtf<see cref="FileStreamResult" />.</returns>
        public FileStreamResult ToRtfResult<T>(T report, string name) where T : ActiveReport
        {
            report.PageSettings.DefaultPaperSize = true;
            report.PageSettings.DefaultPaperSource = true;
				report.Parameters.Add(new Parameter { DefaultValue = CommonConstants.Word, Key = "DownloadType", PromptUser = false });

            return report.ToRtfResult(name);
        }

        /// <summary>
        /// Convert report to Rtf <see cref="FileStreamResult" />.
        /// </summary>
        /// <typeparam name="T">Type of report.</typeparam>
        /// <param name="report">Instance of report to convert.</param>
        /// <param name="name">Name of report.</param>
        /// <returns>The report as a Rtf<see cref="FileStreamResult" />.</returns>
        public FileStreamResult ToTextResult<T>(T report, string name) where T : ActiveReport
        {
            report.PageSettings.DefaultPaperSize = true;
            report.PageSettings.DefaultPaperSource = true;
	        report.Parameters.Add(new Parameter { DefaultValue = CommonConstants.Csv, Key = "DownloadType", PromptUser = false});

            return report.ToCsvResult(name);
        }
    }
}