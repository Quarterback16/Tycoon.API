using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Employment.Web.Mvc.Area.Example.Types;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;

namespace Employment.Web.Mvc.Area.Example.ViewModels.Report
{
    [DisplayName("Other Report Examples")]
    [Group("Reports", Order = 1)]
    [Button("Download", "Reports",  SubmitType = "DownloadReport", ResultsInDownload = true,  Cancel = false, Clear = false, Order = 1)]
    [Serializable]
    public class ReportTypesViewModel
    {
        [Infrastructure.DataAnnotations.Bindable]
        [Display(Name = "Select one of the Report Type and click 'Download'", GroupName = "Reports", Order = 1)]
        [Selection(SelectionType.Single, new[] { ReportTypes.BasicPdf, ReportTypes.BasicRtf, ReportTypes.BasicHtml, ReportTypes.BasicDocx, ReportTypes.ComplexPdf, ReportTypes.ComplexRtf, ReportTypes.ComplexHtml, ReportTypes.ComplexDocx }, new[] { ReportTypes.BasicPdf, ReportTypes.BasicRtf, ReportTypes.BasicHtml, ReportTypes.BasicDocx, ReportTypes.ComplexPdf, ReportTypes.ComplexRtf, ReportTypes.ComplexHtml, ReportTypes.ComplexDocx })]
        [Required(ErrorMessage = "Please select Report to show.")]
        public string VariousReportExamples { get; set; }


        [Display(GroupName = "Creating Reports")]
        public ContentViewModel Overview
        {
            get
            {
                return new ContentViewModel()
                    .AddParagraph("The reporting system supports generating HTML, PDF, RTF and DOCX reports.")
                    .AddParagraph("Rather than generate HTML and convert to PDF, the reports are now entirely generated in code. There is no design view.")
                    .AddParagraph("Supported formatting includes headers, footers, images, tables, lists.")
                     .AddParagraph("The different output formats will never be identical - eg the HTML version of a report won't be identical to the DOCX version, although it is as close as we can make it.")
                   .AddPreformatted(@"

        private IDocumentRender createBasicReport()
        {
            var doc = this.DocumentReportService.CreateDocument();
            doc.AddHeading1(""Heading 1 level"");
            doc.AddParagraph(""This is some sample text. This is some sample text. "");
            doc.AddHeading2(""Heading 2 level""); 
            doc.AddParagraph(""This is some more sample text. This is some more sample text."");
            doc.AddHeading3(""Heading 3 level"");
            var img = getImageData(""sample.png"");
            doc.AddImage(img, DocumentImageFormat.PNG, ""A demo image"");
            return doc;
        }

        [HttpPost]
        [ButtonHandler]
        public ActionResult DownloadReport(ReportTypesViewModel reportTypesViewModel)
        {
            //Don't ever put generated reports into the session or into the cache.
            string reportSelected = reportTypesViewModel.VariousReportExamples;

            byte[] fileContents = null;
            string extension = null;
            string appType = null;
            switch (reportSelected)
            {
                case Employment.Web.Mvc.Area.Example.Types.ReportTypes.BasicPdf:
                    fileContents = createBasicReport().Save(ExportFormat.PDF);
                    extension = "".pdf"";
                    appType = ""application/pdf"";
                    break;
                case Employment.Web.Mvc.Area.Example.Types.ReportTypes.BasicRtf:
                    fileContents = createBasicReport().Save(ExportFormat.RTF);
                    extension = "".rtf"";
                    appType = ""application/rtf"";
                    break;
                case Employment.Web.Mvc.Area.Example.Types.ReportTypes.BasicHtml:
                    fileContents = createBasicReport().Save(ExportFormat.HTML);
                    extension = "".html"";
                    appType = ""text/html"";
                    break;
                case Employment.Web.Mvc.Area.Example.Types.ReportTypes.BasicDocx:
                    fileContents = createBasicReport().Save(ExportFormat.DOCX);
                    extension = "".docx"";
                    appType = ""application/msword"";
                    break;
                ...
            }

            string fileDownloadName = string.IsNullOrEmpty(reportSelected) ? ""Report"" : string.Format(""{0}"", reportSelected);
            return File(fileContents, appType, fileDownloadName + extension);
        }

")
                    ;
            }
        }
    }
}