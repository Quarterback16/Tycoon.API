using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Web.Mvc;
using DataDynamics.ActiveReports;
using DataDynamics.ActiveReports.Export.Pdf;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Infrastructure.Extensions
{
    /// <summary>
    /// Extensions for <see cref="ActiveReport" /> and <see cref="IReport" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ReportExtension
    {
        /// <summary>
        /// Convert report to PDF <see cref="FileStreamResult" />.
        /// </summary>
        /// <typeparam name="T">Type of report.</typeparam>
        /// <param name="report">Instance of report to convert.</param>
        /// <param name="name">Name of report.</param>
        /// <returns>The report as a PDF <see cref="FileStreamResult" />.</returns>
        public static FileStreamResult ToPdfResult<T>(this T report, string name) where T : ActiveReport
        {
            report.Run();
            return report.Document.ToPdfResult(name);
        }

        /// <summary>
        /// Convert report document to PDF <see cref="FileStreamResult" />.
        /// </summary>
        /// <param name="reportDocument">Active Report document instance</param>
        /// <param name="name">Name of report</param>
        /// <returns>The report as a PDF <see cref="FileStreamResult" />.</returns>
        public static FileStreamResult ToPdfResult(this DataDynamics.ActiveReports.Document.Document reportDocument, string name)
        {
            // Do not close or dispose MemoryStream as the FileStreamResult disposes it after use
            var memoryStream = new MemoryStream();

            using (var exported = new PdfExport())
            {
                exported.Export(reportDocument, memoryStream);

                memoryStream.Flush();
                memoryStream.Seek(0, SeekOrigin.Begin);

                var fileStreamResult = new FileStreamResult(memoryStream, "application/pdf");

                fileStreamResult.FileDownloadName = string.Format("{0}.pdf", name);

                return fileStreamResult;
            }
        }
    }
}