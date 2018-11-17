using System;
using System.Reflection;
using System.Web.Mvc;
using Employment.Esc.Framework.Reporting;
using Employment.Web.Mvc.Area.Example.ViewModels.Report;
using Employment.Web.Mvc.Infrastructure.Controllers;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.ViewModels;


namespace Employment.Web.Mvc.Area.Example.Controllers
{
    /// <summary>
    /// Defines the Report controller.
    /// </summary>
    [Security(AllowAny = true)]
    public class ReportController : InfrastructureController
    {

        // Replace "YourService" with whichever service you need from "Service.Interfaces"
        // protected readonly IYourService YourService;
/*
        protected readonly IHtmlReportService HtmlReportService;
*/
        protected readonly IDocumentReportService DocumentReportService;

  
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultController" /> class.
        /// </summary>
        /// <param name="documentReportService">The document report service.</param>
        /// <param name="userService">User service for retrieving user data.</param>
        /// <param name="adwService">Adw service for retrieving ADW data.</param>
        /// <exception cref="System.ArgumentNullException">documentReportService</exception>
        ///// <param name="yourService">Your service for interacting with your data.</param>
        public ReportController(IDocumentReportService documentReportService, IUserService userService, IAdwService adwService)
            : base(userService, adwService)
        {

            if (documentReportService == null)
            {
                throw new ArgumentNullException("documentReportService");
            }
            else
            {
                DocumentReportService = documentReportService;
            }
        }

        /// <summary>
        /// Index action.
        /// </summary>
        [Menu("Report")]
        public ActionResult Index()
        {
            var simpleModel = new ContentViewModel().AddTitle("Report examples");
            simpleModel.AddParagraph("This area contains examples of generating reports using the Employment.Esc.Framework.Reporting.dll.");

            return View(simpleModel);
        }

        /// <summary>
        /// Get Action to display other examples of Reports.
        /// </summary>
        /// <returns></returns>
        [Menu("Report Examples", ParentAction = "Index")]
        public ActionResult ReportTypes()
        {
            var model = new ReportTypesViewModel();

            return View(model);
        }

        private static string[] _randomLongStrings = new string[] { 

"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris quis ex bibendum velit feugiat pharetra. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nulla auctor libero vitae condimentum sagittis. Sed dignissim mollis purus nec eleifend. In ornare faucibus ante sed pellentesque. Nunc ac libero vitae dui faucibus auctor. Praesent non luctus sem. Cras mattis, nibh eget interdum molestie, est eros commodo tortor, vitae dignissim nunc sapien a nulla. Praesent id neque et nisi consectetur sodales. Cras vitae lacinia tellus. Nunc eget augue quis arcu suscipit cursus. In libero quam, faucibus in luctus vitae, pharetra quis ipsum. Nullam felis ligula, iaculis ac neque ac, placerat hendrerit nisl. Lorem ipsum dolor sit amet, consectetur adipiscing elit."
,
"Morbi eget scelerisque libero. Maecenas hendrerit libero tellus, sit amet dictum mauris bibendum eu. Aenean pulvinar mauris a blandit bibendum. Donec eu neque lobortis, porta tellus id, rhoncus est. Cras bibendum fringilla felis, sit amet tincidunt est porttitor tincidunt. Mauris non lacinia nisi. Etiam at lacus ornare, iaculis velit vel, elementum tellus. Maecenas ex augue, tempor eget nunc eget, porttitor fringilla neque."
,
"Proin faucibus dapibus eros, nec imperdiet neque viverra nec. Nulla pellentesque leo ac mauris suscipit malesuada. Etiam placerat nec nulla aliquam posuere. Maecenas efficitur maximus nunc vitae ornare. Duis sed tellus ut lectus mattis blandit at eget nisl. Donec dolor mauris, dignissim id ultrices a, scelerisque scelerisque sapien. Nunc eget est ac magna pretium sodales. Ut accumsan neque ac risus suscipit, in viverra massa condimentum."
,
"Morbi dictum tortor libero. Nunc at justo ullamcorper, aliquet ipsum ac, tempus tortor. Aliquam tellus arcu, semper et semper ac, accumsan eget leo. Nulla accumsan volutpat dignissim. Vestibulum vitae hendrerit augue, ac tincidunt nulla. Nullam finibus orci non ultricies imperdiet. Mauris iaculis pellentesque ullamcorper. Curabitur pellentesque aliquet urna nec luctus. Nam diam dolor, eleifend a sodales sit amet, sodales in metus. Ut id mattis leo, vitae posuere mi. Donec eget dui tellus. Nunc a nibh gravida, ornare ligula id, luctus dui. Mauris arcu mi, porttitor eget odio vitae, pulvinar egestas nunc. Etiam quis diam dictum, varius ipsum id, ultrices nisl. Vestibulum faucibus, dui quis aliquet aliquam, lectus turpis pellentesque quam, ut tincidunt tellus risus ac erat."
,
"Praesent consectetur augue hendrerit nisi luctus, et ornare sem fermentum. Vivamus malesuada consectetur pretium. In sit amet erat nisi. Donec ultricies velit risus, ac ultricies ante eleifend in. Sed id laoreet ligula. Ut molestie euismod cursus. Duis venenatis egestas tellus, non vehicula eros blandit id. Fusce rhoncus magna vel mauris consequat, molestie tempus tellus feugiat. Maecenas tempus sem velit, eu tincidunt mauris commodo sed. Donec pretium aliquam tortor vel tempus. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. In eu lectus augue. Praesent ullamcorper, nisl tincidunt fermentum pharetra, tortor felis feugiat arcu, cursus aliquet massa arcu et ante. Duis a rutrum dui. Nam sed justo in dui consectetur placerat ut vitae nisi. "
 };
        private static string[] _randomShortStrings = new string[] { "Lorem ipsum dolor sit", "Quisque rhoncus mi odio, vitae", "Fusce sodales consectetur", "Aenean vestibulum libero orci, vestibulum", "Ut fermentum orci risus", "Proin nec est" };
        private string RandomShortString()
        {
            Random random = new Random();
            var rand = random.Next(0,5);
            return _randomShortStrings[rand];
        }
        private string RandomLongString()
        {
            Random random = new Random();
            var rand = random.Next(0, 4);
            return _randomLongStrings[rand];
        }
        private string RandomNumber()
        {
            Random random = new Random();
            var rand = random.NextDouble();
            return ""+rand;
        }

        private IDocumentRender createBasicReport()
        {
            var doc = this.DocumentReportService.CreateDocument();
            doc.AddHeading1(RandomShortString());
            doc.AddParagraph(RandomLongString());
            doc.AddHeading2(RandomShortString());
            doc.AddParagraph(RandomLongString());
            doc.AddHeading3(RandomShortString());
            var img = getImageData("sample.png");
            doc.AddImage(img, ImageFormat.PNG, "A demo image");
            return doc;
        }

        private IDocumentRender createComplexReport()
        {
            //get image bytes
            var img = getImageData("sample.png");
            var img2 = getImageData("aust-gov.jpg");

            //create renderer
            var doc = this.DocumentReportService.CreateDocument();

            //add header and footer
            doc.AddHeaderImage(img2, ImageFormat.JPG, "A demo header image",TextAlign.Center);
            doc.AddFooter("Some footer text");

            //page 1
            doc.AddHeading1("A heading 1");
            doc.AddParagraph(RandomLongString());
            doc.AddParagraph(RandomLongString());
            doc.AddHeading2("A heading 2");
            doc.AddParagraph(RandomLongString());
            doc.AddHeading3("A heading 3");
            doc.AddParagraph(RandomLongString());
            doc.AddPageBreak();

            //page 2
            doc.AddHeading2("Second page - left, right center text");
            doc.AddParagraph(RandomLongString());
            doc.AddParagraph(RandomLongString(), TextAlign.Right);
            doc.AddParagraph(RandomLongString(), TextAlign.Center);
            doc.AddPageBreak();

            //page 3
            doc.AddHeading2("A PNG image");
            doc.AddImage(img, ImageFormat.PNG, "A demo png image");
            doc.AddHeading2("A JPG image");
            doc.AddImage(img2, ImageFormat.JPG, "A demo jpeg image");
            doc.AddPageBreak();


            //page 4
            //add a table
            doc.AddHeading2("A table");
            var table = doc.AddTable();
            table.AddColumn("test column 1");
            table.AddColumn("test column 2");
            table.AddColumn("test column 3");

            for (int i = 0; i < 5; i++)
            {
                var row = table.AddRow();
                row.AddCell(RandomShortString());
                row.AddCell(RandomNumber());
                row.AddCell(RandomLongString());
            }

            //add a list
            var list = doc.AddList();
            list.AddListItem(RandomShortString());
            list.AddListItem(RandomShortString());
            list.AddListItem(RandomShortString());

            //add a numbered list
            var listNumbered = doc.AddNumberedList();
            listNumbered.AddListItem(RandomShortString());
            listNumbered.AddListItem(RandomShortString());
            listNumbered.AddListItem(RandomShortString());


            return doc;
        }


        /// <summary>
        /// Allows user to download Pdf copy of report.
        /// </summary>
        /// <param name="reportTypesViewModel"> model. </param>
        /// <returns>Pdf file.</returns>
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
                    extension = ".pdf";
                    appType = "application/pdf";
                    break;
                case Employment.Web.Mvc.Area.Example.Types.ReportTypes.BasicRtf:
                    fileContents = createBasicReport().Save(ExportFormat.RTF);
                    extension = ".rtf";
                    appType = "application/rtf";
                    break;
                case Employment.Web.Mvc.Area.Example.Types.ReportTypes.BasicHtml:
                    fileContents = createBasicReport().Save(ExportFormat.HTML);
                    extension = ".html";
                    appType = "text/html";
                    break;
                case Employment.Web.Mvc.Area.Example.Types.ReportTypes.BasicDocx:
                    fileContents = createBasicReport().Save(ExportFormat.DOCX);
                    extension = ".docx";
                    appType = "application/msword";
                    break;
                case Employment.Web.Mvc.Area.Example.Types.ReportTypes.ComplexPdf:
                    fileContents = createComplexReport().Save(ExportFormat.PDF);
                    extension = ".pdf";
                    appType = "application/pdf";
                    break;
                case Employment.Web.Mvc.Area.Example.Types.ReportTypes.ComplexRtf:
                    fileContents = createComplexReport().Save(ExportFormat.RTF);
                    extension = ".rtf";
                    appType = "application/rtf";
                    break;
                case Employment.Web.Mvc.Area.Example.Types.ReportTypes.ComplexHtml:
                    fileContents = createComplexReport().Save(ExportFormat.HTML);
                    extension = ".html";
                    appType = "text/html";
                    break;
                case Employment.Web.Mvc.Area.Example.Types.ReportTypes.ComplexDocx:
                    fileContents = createComplexReport().Save(ExportFormat.DOCX);
                    extension = ".docx";
                    appType = "application/msword";
                    break;
            }

            string fileDownloadName = string.IsNullOrEmpty(reportSelected) ? "Report" : string.Format("{0}", reportSelected);
            return File(fileContents, appType, fileDownloadName + extension);
        }

/*
        /// <summary>
        /// Demonstrates Smart Autocomplete.
        /// </summary>
        /// <returns></returns>
        [Menu("Smart Autocomplete Example")]
        public ActionResult NewAutocomplete()
        {
            var autocompleteModel = new SmartAutocompleteViewModel();

            return View(autocompleteModel);
        }



        /// <summary>
        /// Renders AJAX selection view
        /// </summary>
        /// <param name="input">The user supplied text</param>
        /// <param name="pageIndex">Current page number</param>
        /// <param name="pageSize">page size</param>
        /// <returns>the next page data</returns>
        [AjaxOnly]
        public JsonResult GetData(string input, int pageIndex, int pageSize)
        {
            //pageIndex = --pageIndex < 0 ? 0 : pageIndex;
            var data = SampleData.FillDictionary();
            if (!string.IsNullOrEmpty(input))
            {
                data = data.Where(x => x.Text.ToLower().Contains(input.ToLower())).ToList();
                //data = data.Where(x => x.ToLower().Contains(input.ToLower()));
            }

            data = data.Skip(pageIndex * pageSize).Take(pageSize);

            return new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = data };//new {result = data}
        }


        /// <summary>
        /// Responsible for displaying Helper View.
        /// </summary>
        /// <returns></returns>
        [Menu("Helper functions", ParentAction = "Index")]
        public ActionResult Helpers()
        {
            var model = new HelperMethodsViewModel();

            return View(model);
        }

        /// <summary>
        /// Responsible for displaying Layout View used for Report templates.
        /// </summary>
        /// <returns></returns>
        [Menu("Layout View", ParentAction = "Index")]
        public ActionResult MockLayout()
        {
            var model = new LayoutViewModel();

            return View(model);
        }

        /// <summary>
        /// Displays Stylesheet used in layout page.
        /// </summary>
        /// <returns></returns>
        [Menu("Stylesheet", ParentAction = "Index")]
        public ActionResult StyleView()
        {
            var model = new StylesheetViewModel();

            return View(model);
        }


        /// <summary>
        /// Initializes new instance of BasicsViewModel.
        /// </summary>
        /// <returns></returns>
        [Menu("Basics", ParentAction = "Index")]
        public ActionResult Basics()
        {
            var model = new BasicsViewModel();

            return View(model);
        }


        /// <summary>
        /// Get Action to display other examples of Reports.
        /// </summary>
        /// <returns></returns>
        [Menu("Other Report Examples", ParentAction = "Index")]
        public ActionResult ReportTypes()
        {
            var model = new ReportTypesViewModel();

            return View(model);
        }

        /// <summary>
        /// Updates the ReportSelected property.
        /// </summary>
        /// <param name="model">ReportTypesViewModel</param>
        /// <returns>View</returns>
        [HttpPost]
        [ButtonHandler]
        public ActionResult ReportExampleSelectionChanged(ReportTypesViewModel reportTypesViewModel)
        {

            if (ModelState.IsValid)
            {
                switch (reportTypesViewModel.VariousReportExamples)
                {
                    case Employment.Web.Mvc.Area.Example.Types.ReportTypes.Appointment:
                        reportTypesViewModel.ReportSelected = Types.ReportTypes.Appointment;
                        break;
                    case Employment.Web.Mvc.Area.Example.Types.ReportTypes.JSCIHistory:
                        reportTypesViewModel.ReportSelected = Types.ReportTypes.JSCIHistory;
                        break;
                    case Employment.Web.Mvc.Area.Example.Types.ReportTypes.JSCIReport:
                        reportTypesViewModel.ReportSelected = Types.ReportTypes.JSCIReport;
                        break;
                }
            }

            return View(reportTypesViewModel);
        }


        /// <summary>
        /// Allows user to download Pdf copy of report.
        /// </summary>
        /// <param name="reportTypesViewModel"> model. </param>
        /// <returns>Pdf file.</returns>
        [HttpPost]
        [ButtonHandler]
        public ActionResult DownloadReport(ReportTypesViewModel reportTypesViewModel)
        {
            var reportSelected = reportTypesViewModel.VariousReportExamples;
            string parseResult = string.Empty;

            // Try to get from session.
            KeyModel templateResultKey = new KeyModel(CacheType.User, reportSelected + Employment.Web.Mvc.Area.Example.Types.ReportTypes.SessionKeysuffix);
            if (!UserService.Session.TryGet(templateResultKey, out parseResult))
            {
                parseResult = GetParsedReport(reportSelected);
            }

            var fileContents = ConvertToPdf(parseResult);
            string fileDownloadName = string.IsNullOrEmpty(reportSelected)
                                          ? "Report"
                                          : string.Format("{0}", reportSelected);
            const string pdfExtension = ".pdf";
            return File(fileContents, "application/pdf", fileDownloadName + pdfExtension);
        }

        /// <summary>
        /// Displays report based on the Selected type.
        /// </summary>
        /// <param name="reportSelected"> </param>
        /// <returns>Report in new tab.</returns>
        public ActionResult ShowReport(string reportSelected)
        {
            string parseResult = string.Empty;
            parseResult = GetParsedReport(reportSelected);
            KeyModel templateResultKey = new KeyModel(CacheType.User, reportSelected + Employment.Web.Mvc.Area.Example.Types.ReportTypes.SessionKeysuffix);
            UserService.Session.Set(templateResultKey, parseResult);

            if (string.IsNullOrEmpty(parseResult))
            {
                string errorMessage = "Error has occurred during report creation.";
                AddErrorMessage(errorMessage);
                return RedirectToAction("CustomError", "Error", new { Area = "" });
            }

            return Content(parseResult);

        }


        /// <summary>
        /// Redirects to Error Page in new tab.
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewReport()
        {
            string errorMessage = "Error has occurred during report creation.";
            AddErrorMessage(errorMessage);
            return RedirectToAction("CustomError", "Error", new { Area = "" });
        }


        /// <summary>
        /// Parses the Report according to its type.
        /// </summary>
        /// <param name="reportSelected"></param>
        /// <returns></returns>
        private string GetParsedReport(string reportSelected)
        {
            string parseResult = string.Empty;
            switch (reportSelected)
            {
                case Employment.Web.Mvc.Area.Example.Types.ReportTypes.Appointment:
                    parseResult = GetAppointmentSlipReport();
                    break;
                case Employment.Web.Mvc.Area.Example.Types.ReportTypes.JSCIReport:
                    parseResult = GetJSCIReport();
                    break;
                case Types.ReportTypes.JSCIHistory:
                    parseResult = GetJSCIHistoryReport();
                    break;
            }
            return parseResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetJSCIHistoryReport()
        {
            var historyReportModel = PopulateJSCIHistoryModel();
            return HtmlReportService.ParseMainTemplate(ObtainTemplateContent, historyReportModel);
        }

        /// <summary>
        /// Searches for selected report type in Manifest Resources and calls RazorMachine to get Content (HTML markup).
        /// </summary>
        /// <param name="reportSelected">Selected Report.</param>
        /// <returns>string containing HTML markup.</returns>
        private string ObtainTemplateContent(string reportSelected)
        {
            string templateContent = string.Empty;
            foreach (var manifestResourceName in listOfEmbeddedManifestResources)
            {
                if (manifestResourceName.ToLower().Contains(string.Format("{0}.", reportSelected.ToLower())))
                {
                    System.IO.Stream stream =
                        currentAssembly.GetManifestResourceStream(manifestResourceName);
                    if (stream != null)
                    {
                        byte[] buffer = new byte[stream.Length];
                        stream.Read(buffer, 0, buffer.Length);
                        templateContent = Encoding.UTF8.GetString(buffer);
                        break;
                    }
                }
            }
            return templateContent;
        }


        /// <summary>
        /// Calls HtmlToPdfConverterService to Get Pdf file.
        /// </summary>
        /// <param name="parseResult">Html</param>
        /// <returns>Byte array</returns>
        private byte[] ConvertToPdf(string parseResult)
        {
            return HtmlToPdfConverterService.ConvertToPdf(parseResult, string.Empty, includeDefaultFooter: true, useDefaultMargins: true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetAppointmentSlipReport()
        {
            string parseResult = null;
            //string rootOperatorPath = "/Example";
            //var templateServiceForAppointmentSlip =
            //    new HtmlReportService<AppointmentSlipModel>(ObtainTemplateContent, rootOperatorPath);
            //// Populate Model
            AppointmentSlipModel appointmentSlipModel = PopulateAppointmentSlipModel();
            //parseResult = templateServiceForAppointmentSlip.ParseMainTemplate(appointmentSlipModel, templateContent);
            //return parseResult;
            return HtmlReportService.ParseMainTemplate<AppointmentSlipModel>(ObtainTemplateContent, appointmentSlipModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetJSCIReport()
        {
            string parseResult;
            // Populate JSCI Report Model
            JSCIReportModel reportModel = PopulateJSCIReportModel();
            //templateServiceForJsciReport.RegisterSubTemplates("~/Templates/JSCI SubReport", "JSCI SubReport");
            return HtmlReportService.ParseMainTemplate<JSCIReportModel>(ObtainTemplateContent, reportModel);
        }

        /// <summary>
        /// Generates a report and displays it in new tab.
        /// </summary>
        /// <returns>
        /// HTML markup of Report.
        /// </returns>
        public ActionResult GenerateReport()
        {
            string parseResult = string.Empty;
            foreach (var manifestResourceName in Assembly.GetExecutingAssembly().GetManifestResourceNames())
            {
                System.IO.Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(manifestResourceName);
                if (stream != null)
                {
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);
                    string templateContent = Encoding.UTF8.GetString(buffer);
                    if (templateContent.Length > 0)
                    {
                        if (manifestResourceName.Contains("JSCI Report"))
                        {
                            parseResult = GetJSCIReport();

                        }
                        //else if (manifestResourceName.Contains("Appointment"))
                        //{
                        //    parseResult = GetAppointmentSlipReport(templateContent);
                        //}
                    }
                }
            }
            return Content(parseResult);
        }




        #region HELPER METHODS


        private static AppointmentSlipModel PopulateAppointmentSlipModel()
        {
            AppointmentSlipModel model = new AppointmentSlipModel()
                                             {
                                                 StyleTemplateName = "CustomStyles",
                                                 ParentTemplateName = "Appointment",
                                                 PageTitle = "Appointment Slip",
                                                 JSKId = "123456789",
                                                 AppointmentDescription = @"You are required to attend an appointment with us, The Salvation Army Employment Plus, as your
                                                                            Remote Jobs and Communities Program provider. The purpose of this appointment is to discuss
                                                                            your employment and personal development needs and the activities you might participate in to
                                                                            address these needs. We will also negotiate an Individual Participation Plan with you.",
                                                 LetterDate = new DateTime(2013, 04, 17),
                                                 ReceiverAddress = "Private Bag 9 \nBROOME WA 6725",
                                                 ReceiverName = "Bertha McCarthy",
                                                 AppointmentDetails = new AppointmentDetails
                                                 {
                                                     AppointmentDateTime = new DateTime(2013, 07, 1, 9, 0, 0),
                                                     ContactName = "Mr John",
                                                     Format = "Face-2-Face",
                                                     Location = "Ian - Line 1 \nLine 2 \nLine 3\nBelconnen",
                                                     Phone = "0411 145 777"

                                                 },
                                                 AustGovImageData = GetImageData("aust-gov.jpg"),
                                                 PrintStyleTemplateName = "Employment.Web.Mvc.Area.Example.Views.Templates.PrintMediaStyles",
                                                 ScreenStyleTemplateName = "Employment.Web.Mvc.Area.Example.Views.Templates.ScreenMediaStyles"

                                             };

            return model;
        }


        /// <summary>
        /// Instantiates JSCIHistoryModel with appropriate values.
        /// </summary>
        /// <returns></returns>
        public static JSCIHistoryModel PopulateJSCIHistoryModel()
        {

            List<HistoryRecord> historyRecords = new List<HistoryRecord>();
            historyRecords.Add(new HistoryRecord
            {
                AssessmentDate = DateTime.Now.AddDays((new Random().Next(0, 20))),
                CreatedBy = UserType.ESL_JSC,
                JCA = "Special needs completed, Personal factors  completed, Disability completed",
                Result = "Stream 1-3",
                Status = StatusType.Active,
                UpdatedBy = UserType.BFL767
            });
            historyRecords.Add(new HistoryRecord
            {
                AssessmentDate = DateTime.Now.AddDays((new Random().Next(5, 20))),
                CreatedBy = UserType.BFL767,
                JCA = "Special needs completed, Personal factors completed, Disability completed",
                Result = "Vocational Rehabilitation Service",
                Status = StatusType.Inactive,
                UpdatedBy = UserType.ESL_JSC
            });
            historyRecords.Add(new HistoryRecord
            {
                AssessmentDate = DateTime.Now.AddDays((new Random().Next(10, 20))),
                CreatedBy = UserType.BFL767,
                JCA = "None",
                Result = "Job Network (Highly Disadvantaged)",
                Status = StatusType.Active,
                UpdatedBy = UserType.BMB
            });
            historyRecords.Add(new HistoryRecord
            {
                AssessmentDate = DateTime.Now.AddDays((new Random().Next(15, 28))),
                CreatedBy = UserType.BFL767,
                JCA = "Disability completed",
                Result = "DES - Disability Management Service",
                Status = StatusType.Inactive,
                UpdatedBy = UserType.NOE707
            });
            historyRecords.Add(new HistoryRecord
            {
                AssessmentDate = DateTime.Now.AddDays((new Random().Next(20, 28))),
                CreatedBy = UserType.ESL_JSC,
                JCA = "Special needs completed, Personal factors completed, Disability completed ",
                Result = "None",
                Status = StatusType.Active,
                UpdatedBy = UserType.BMB
            });
            historyRecords.Add(new HistoryRecord
            {
                AssessmentDate = DateTime.Now.AddDays((new Random().Next(25, 28))),
                CreatedBy = UserType.BMB,
                JCA = "Special needs completed, Personal factors completed, Disability completed",
                Result = "DES - Disability Management Service",
                Status = StatusType.Active,
                UpdatedBy = UserType.BFL767
            });

            JSCIHistoryModel model = new JSCIHistoryModel
            {
                AustGovImageData = GetImageData("aust-gov.jpg"),
                Name = "Jsci History",
                JskID = "1234567890",
                HistoryRecords = historyRecords,
                PageTitle = "History JSCI",
                ParentTemplateName = "Employment.Web.Mvc.Area.Example.Views.Templates.JSCI History"
            };


            return model;
        }

        /// <summary>
        /// Instantiates JSCIReportModel with appropriate values.
        /// </summary>
        /// <returns></returns>
        public static JSCIReportModel PopulateJSCIReportModel()
        {
            List<QuestionAndResponse> questionAndAnswers = new List<QuestionAndResponse>(10);
            questionAndAnswers.Add(new QuestionAndResponse { QuestionText = @"What have you MOSTLY been doing in the LAST TWO YEARS?", ResponseRecorded = @"Caring" });

            questionAndAnswers.Add(new QuestionAndResponse
            {
                QuestionText = @"Have you done any paid work at all in the last
two years?"
            ,
                ResponseRecorded = @"No"
            });

            questionAndAnswers.Add(new QuestionAndResponse
            {
                QuestionText = @"What is the highest level of schooling you have
COMPLETED?"
            ,
                ResponseRecorded = @"Year 10"
            });

            questionAndAnswers.Add(new QuestionAndResponse
            {
                QuestionText = @"Have you COMPLETED any other
qualification(s)?"
            ,
                ResponseRecorded = @"Yes"
            });

            questionAndAnswers.Add(new QuestionAndResponse
            {
                QuestionText = @"Have you done any paid work at all in the last
two years?"
            ,
                ResponseRecorded = @"No"
            });

            questionAndAnswers.Add(new QuestionAndResponse
            {
                QuestionText = @"What is the highest level of schooling you have
COMPLETED?"
           ,
                ResponseRecorded = @"Year 10"
            });

            questionAndAnswers.Add(new QuestionAndResponse
            {
                QuestionText = @"Have you COMPLETED any other
qualification(s)?"
            ,
                ResponseRecorded = @"Yes"
            });

            questionAndAnswers.Add(new QuestionAndResponse
            {
                QuestionText = @"Have you done any paid work at all in the last
two years?"
          ,
                ResponseRecorded = @"No"
            });

            questionAndAnswers.Add(new QuestionAndResponse
            {
                QuestionText = @"What is the highest level of schooling you have
COMPLETED?"
            ,
                ResponseRecorded = @"Year 10"
            });

            questionAndAnswers.Add(new QuestionAndResponse
            {
                QuestionText = @"Have you COMPLETED any other
qualification(s)?"
            ,
                ResponseRecorded = @"Yes"
            });

            questionAndAnswers.Add(new QuestionAndResponse
            {
                QuestionText = @"Have you done any paid work at all in the last
two years?"
            ,
                ResponseRecorded = @"No"
            });

            questionAndAnswers.Add(new QuestionAndResponse
            {
                QuestionText = @"What is the highest level of schooling you have
COMPLETED?"
            ,
                ResponseRecorded = @"Year 10"
            });

            questionAndAnswers.Add(new QuestionAndResponse
            {
                QuestionText = @"Have you COMPLETED any other
qualification(s)?"
           ,
                ResponseRecorded = @"Yes"
            });

            questionAndAnswers.Add(new QuestionAndResponse
            {
                QuestionText = @"Have you done any paid work at all in the last
two years?"
           ,
                ResponseRecorded = @"No"
            });

            questionAndAnswers.Add(new QuestionAndResponse
            {
                QuestionText = @"What is the highest level of schooling you have
COMPLETED?"
            ,
                ResponseRecorded = @"Year 10"
            });

            questionAndAnswers.Add(new QuestionAndResponse
            {
                QuestionText = @"Have you COMPLETED any other
qualification(s)?"
          ,
                ResponseRecorded = @"Yes"
            });

            questionAndAnswers.Add(new QuestionAndResponse
            {
                QuestionText = @"Have you done any paid work at all in the last
two years?"
            ,
                ResponseRecorded = @"No"
            });

            questionAndAnswers.Add(new QuestionAndResponse
            {
                QuestionText = @"What is the highest level of schooling you have
COMPLETED?"
           ,
                ResponseRecorded = @"Year 10"
            });

            questionAndAnswers.Add(new QuestionAndResponse
            {
                QuestionText = @"Have you COMPLETED any other
qualification(s)?"
           ,
                ResponseRecorded = @"Yes"
            });



            JSCIReportModel model = new JSCIReportModel
            {
                BranchDetails = new BranchDetails { EntityName = "Centrelink", BranchName = "BANKSTOWN", BranchAddress = new AddressModel { streetAddress = "Level 4 2-14 Meredith Street", suburb = "BANKSTOWN", state = "NSW", postcode = "2200" }, Telephone = "(02) 92051228" },
                Heading = "JSCI Report",
                Name = "MS CARMEL AYYAD",
                JSKId = "6468361609",
                DOB = new DateTime(1959, 8, 16),
                Address = new AddressModel { streetAddress = "60 JAMES ST", suburb = "PUNCHBOWL", state = "NSW", postcode = "2196" },
                Telephone = "(02) 87254906",
                ReportDescription = @"This report shows information recorded about you for the purpose of determining the most
                                appropriate employment assistance for you. Your Assessor should have discussed these
                                changes with you before they were recorded. As a result of these changes, the level of
                                employment assistance you receive may change. Your Assessor can provide you with more
                                information about this.
                                Please check that the information in the report is correct. If you find any information that is
                                incorrect, please tell your Assessor so that the information can be corrected.
                                Your Assessor is unable to change your address, telephone number or date of birth. You will
                                need to contact Centrelink by telephone or in person to have these details corrected.",
                ReasonForJSCI = "Auto Processing",

                QuestionAnswers = questionAndAnswers,

                Act = @"Your personal information is protected by law under the Social Security (Administration) Act 1999
                            and the Privacy Act 1988.
                            This information is provided to Centrelink and the Department of Education, Employment and
                            Workplace Relations and where relevant, third parties. The information you provide on this form
                            will be used to decide what services are the most suitable for you. This may include referral to
                            other services or a programs provided by an Australian Government Department or by a
                            contracted service provider.
                            This is to give you the appropriate services to help you find employment.",
                Declaration = "I, CARMEL AYYAD declare that this information is correct.",

                FeedbackText = @"Your Assessor is required to ensure that only relevant and necessary information is
                        collected and that it is kept confidential. If you have any concerns about the service you
                        have received, please talk to your Assessor, or telephone the Customer Service Line on
                        1800 805 260.",
                ReportPrintDate = DateTime.Now.Date,
                JSCICreationDate = new DateTime(2013, 02, 19),
                PageTitle = "JSCI REPORT",
                StyleTemplateName = "Employment.Web.Mvc.Area.Example.Views.Templates.CustomStyles",
                AustGovImageData = GetImageData("aust-gov.jpg"),
                ParentTemplateName = "Employment.Web.Mvc.Area.Example.Views.Templates.Jsci Report",
                ScreenStyleTemplateName = "Employment.Web.Mvc.Area.Example.Views.Templates.ScreenMediaStyles",
                PrintStyleTemplateName = "Employment.Web.Mvc.Area.Example.Views.Templates.PrintMediaStyles"
            };

            return model;
        }
*/
        private static byte[] getImageData(string imageNameWithExtension)
        {
            var imageStreamFromAssembly =
                Assembly.GetExecutingAssembly().GetManifestResourceStream("Employment.Web.Mvc.Area.Example.Content.Images." + imageNameWithExtension);
            byte[] byteArrayFromImageStream = new byte[0];
            if (imageStreamFromAssembly != null)
            {
                byteArrayFromImageStream = new byte[imageStreamFromAssembly.Length];
                imageStreamFromAssembly.Read(byteArrayFromImageStream, 0, byteArrayFromImageStream.Length);

            }
            return byteArrayFromImageStream;
        }





    }
}
