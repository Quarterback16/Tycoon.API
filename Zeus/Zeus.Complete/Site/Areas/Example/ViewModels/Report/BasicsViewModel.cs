using System.ComponentModel.DataAnnotations;
using Employment.Web.Mvc.Area.Example.Types;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using DisplayNameAttribute = System.ComponentModel.DisplayNameAttribute;
namespace Employment.Web.Mvc.Area.Example.ViewModels.Report
{
    [DisplayName("Generating Reports - Basics")]
    [Group("Initial Setup", Order = 1)]
    [Group("Model Class", Order = 2)]
    [Group("Template (View in CSHTML)", Order = 3)]
    [Group("Controller Logic", Order = 4)]
    [Group("ViewModel", Order = 5)]
    [Button("Generate Report", "ViewModel", Order = 1, Cancel = false, SubmitType = ReportSubmitType.GenerateSampleReport)]
    [Link("Generate Report (Cancel)", Order = 3, Cancel = true)]
    [Link("Cancel", ActionForDependencyType.Visible, "CanAction", ComparisonType.EqualTo, false, SkipClientSideValidation = true, Parameters = new string[] { "JobseekerID" }, Action = "Details", Order = 4, PassOnNull = true, Cancel = true)]
    [Link("Generate Report Link", ActionForDependencyType.Visible, "CanAction", ComparisonType.EqualTo, true, OpensInNewTab = true, Action = "GenerateReport", Order = 2)]
    [Link("Generate Report Link", ActionForDependencyType.Visible, "CanAction", ComparisonType.EqualTo, false, OpensInNewTab = true, Action = "GenerateReport", Order = 3)]
    [Link("Generate Report Link", OpensInNewTab = true, Action = "GenerateReport", Order = 4)]
    [Link("Generate Report Link", OpensInNewTab = true, Action = "GenerateReport", Order = 5)]
    [Link("Generate Report Link", OpensInNewTab = true, Action = "GenerateReport", Order = 6)]
    [Link("Generate Report Link", OpensInNewTab = true, Action = "GenerateReport", Order = 7)]
    [Link("Generate Report Link", OpensInNewTab = true, Action = "GenerateReport", Order = 8)]
    public class BasicsViewModel
    {
        [Hidden]
        public bool CanAction { get; set; }

        #region Group: Initial Setup

        /// <summary>
        /// Content that displays steps for Initial Setup.
        /// </summary>
        [Display(GroupName = "Initial Setup", Order = 1)]
        public ContentViewModel InitialSetup
        {
            get
            {
                return new ContentViewModel()
                    .AddParagraph("")
                    .AddTitle("Steps for Initial setup:")
                    .AddUnderlinedText("This is underlined text.")
                    .AddPreformatted(
                   @"
1. Get the latest of Rhea.Complete.sln including web/dependencies folder.

2. You will use HtmlReportService defined in Infrastructure. HtmlToPdfConverterService can be used to generate PDF from html reports.
                     
3. Now we are ready to add models, templates (views) and controller logic.

4. All models need to implement IBaseReportTemplate interface.

5. Create a folder called 'Templates' under Views in your project and add CSHTML views in it. 

6. PROJECT teams may wish to add their own custom styles for report. You may have upto 3 templates for styles for various media types such as, screen, print and common ones (that apply to both).
    * Please ensure that you name your style views unique, e.g.: Employer.CustomStyles.cshtml, Employer.PrintMediaStyles.cshtml etc.                                      
                    ");

                /* Other instructions
                After loading the 'Reporting Tool' solution you can either reference that project
    (adding Project Reference) to your project 
                    or
                 * 
                 * 2. Add reference to following assemblies in your project. 
        | Xipton.Razor.dll (located at \Web\Dependencies\RazorEngine) 
        | ReportingTool.RazorMachine (located at \Web\Dependencies\RazorEngine) 
                */
            }
        }

        #endregion


        #region Group: Model Class

        /// <summary>
        /// Content that displays Model class.
        /// </summary>
        [Display(GroupName = "Model Class", Order = 1)]
        public ContentViewModel ModelDescription
        {
            get
            {
                return new ContentViewModel()
                    .AddTitle("Model class")
                    .AddPreformatted(
                   @"/// <summary>
    /// Model for JSCI Report.
    /// </summary>
    public class JSCIReportModel : IBaseReportTemplate
    {

       
         
        public string Name { get; set; }

        public string JSKId { get; set; }

        public DateTime DOB { get; set; }

        public AddressModel Address { get; set; }

        public string Telephone { get; set; }

        public string Heading { get; set; }

        public BranchDetails BranchDetails { get; set; }

        public string ReportDescription { get; set; }

        public string ReasonForJSCI { get; set; }

        public List<QuestionAndResponse> QuestionAnswers { get; set; }

        /// <summary>
        /// Privacy Act statement.
        /// </summary>
        public string Act { get; set; }

        public string Declaration { get; set; }

        public string FeedbackText { get; set; }

        public DateTime ReportPrintDate { get; set; }

        public DateTime JSCICreationDate { get; set; }


        #region IBaseTemplate Members

        /// <summary>
        /// Template name which defines styles for @media print. The value is not mandatory.
        /// </summary>
        public string PrintStyleTemplateName { get; set; }


        /// <summary>
        /// Template name which defines styles for @media screen. The value is not mandatory.
        /// </summary>
        public string ScreenStyleTemplateName { get; set; }

        /// <summary>
        ///  REQUIRED for 'Title' of report.
        /// </summary>
        public string PageTitle
        {
            get{    return 'JSCI Report';  }
            set{}
        } 

        /// <summary>
        /// Name of the template that contains common styles to be applied for both @media screen and @media print.
        /// </summary>
        public string StyleTemplateName { get; set; }

        #endregion
    }

    /// <summary>
    /// Question and Response
    /// </summary>
    public class QuestionAndResponse
    {
        public string QuestionText { get; set; }

        public string ResponseRecorded { get; set; }
    }

    /// <summary>
    /// Branch Details
    /// </summary>
    public class BranchDetails
    {
        public string EntityName { get; set; }

        public string BranchName { get; set; }

        public AddressModel BranchAddress { get; set; }

        public string Telephone { get; set; }
    }

    /// <summary>
    /// Address class.
    /// </summary>
    public class AddressModel
    {
        public string streetAddress;

        public string postcode;

        public string state;

        public string suburb;
    }
                    
                    ");
            }
        }

        #endregion


        #region Group: Template (View in CSHTML)

        /// <summary>
        /// Content that displays Template (View in CSHTML).
        /// </summary>
        [Display(GroupName = "Template (View in CSHTML)", Order = 1)]
        public ContentViewModel TemplateDescription
        {
            get
            {
                return new ContentViewModel()
                    .AddTitle("Template views:")
                    .AddParagraph("Views need to be added in Templates folder. They can then be made as 'Embedded Resource'. Note some of the helper functions used (such as @AddBreak(), @RenderImage(), @GetLabel(),  @GetTextBox() etc.")
                    .AddSubTitle("JSCI Report.cshtml")
                    .AddPreformatted(
                   @"



<div class='outer'>
    <div class='letterheadcentre'>
        
        @{
            if(Model.AustGovImageData != null)
            {
                @RenderImage(Model.AustGovImageData, 'Australian Government Logo.');
            }

        }

            <strong>@Model.BranchDetails.EntityName  @Model.BranchDetails.BranchName</strong>
            <br>
            @Model.BranchDetails.BranchAddress.streetAddress
            <br>
            @Model.BranchDetails.BranchAddress.suburb @Model.BranchDetails.BranchAddress.state @Model.BranchDetails.BranchAddress.postcode
            <br>
            @Model.BranchDetails.Telephone
            <br>
        </div>
        <br>
        @GetLabel('Name:', 'textBox Name')
        @GetTextBox(Model.Name, 'textBoxName', 'Signature', true) @*read-only*@
        @GetLabel('Date:', 'textBoxSignature')           
        @GetTextBox(string.Empty, 'textBoxSignature', false) @*editable*@
        
        <h1 class='H1Italic'>
            @Model.Heading **</h1>
        
        <br />
        <br />
        <p>
            @Model.ReportDescription</p>
        <p>
            <div class='fullrow'>
                <!--this div becomes the container for the relative positioning-->
                <div class='bindlabeltodata'>
                    <!--this div keeps the label and data divs together for smaller screen sizes-->
                    <span class='Label'>Reason for conducting the JSCI:</span> <span class='data'>Auto Processing</span>
                </div>
                <!--end of label to data div-->
            </div>
        </p>

   @Include('JSCI SubReport')   /* This is how you will reference sub template. */
        <br />

        <p>
            @Model.Act</p>
        <p>
            I, @Model.Name declare that this information is correct.</p>
        <br />
        <div class='row'>
        @GetTextBox(string.Empty, 'textBoxSignature', 'Signature', false, 'Signature')
        
        @GetTextBox(DateTime.Now.Day.ToString(), 'textBoxDay', 'Date', false, 'Day')
        /
        @GetTextBox(DateTime.Now.Month.ToString(), 'textBoxMonth', 'Date', false, 'Month')
        /
        @GetTextBox(DateTime.Now.Year.ToString(), 'textBoxYear', 'Date', false, 'Year')
        
 
    </div> 
        <p>
            @Model.FeedbackText
            <br />
            @Model.ReportPrintDate
            <br />
            @Model.JSCICreationDate </p>
        
        <p>** -->  This is not the illustration of actual report but instead it has been attempted to cover various scenarios encountered in different reports.</p>
    </div>


                   ");
            }
        }

        [Display(Name = "Sub Template", GroupName = "Template (View in CSHTML)", Order = 2)]
        public ContentViewModel SubTemplateContent
        {
            get
            {
                return new ContentViewModel()
                .AddTitle("Sub Template for syles")
                .AddStrongText("Please ensure to add '{}' onto first line for any style sub-template. This resolves any character encoding issues.")
                .AddPreformatted("" +
                                 @"
{}/* Add braces to fix Encoding problems. */
@*Customised styles for this report.*@ 
h1 
{ 
    font-weight: normal;  
    width: 75%; 
} 
                ");



                ;
            }
        }

        #endregion


        #region Group: Controller Logic

        /// <summary>
        /// Content that displays Login inside controller.
        /// </summary>
        [Display(GroupName = "Controller Logic", Order = 1)]
        public ContentViewModel ControllerContent
        {
            get
            {
                return new ContentViewModel()
                    .AddTitle("Logic to be added in Controller. Refer to ReportController in Example Area.");
            }
        }

        #endregion


        #region ViewModel Content

        /// <summary>
        /// Displays crucial content in ViewModel.
        /// </summary>
        [Display(GroupName = "ViewModel", Order = 1)]
        public ContentViewModel ViewModelContent
        {
            get
            {
                return new ContentViewModel()
                    .AddTitle("ViewModel")
                    .AddParagraph("Make sure to add Link with relevant action that generates the report. Ensure the property 'OpensInNewTab' is set to True.")
                    .AddPreformatted(@"
 [Link('Generate Report;, OpensInNewTab = true, Action = 'GenerateReport', Order = 2)]

                                       ")

                    ;
            }
        }

        #endregion
    }
}