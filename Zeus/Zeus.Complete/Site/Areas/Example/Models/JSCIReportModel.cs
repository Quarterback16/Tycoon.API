using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Area.Example.Models
{

    /// <summary>
    /// Model for JSCI Report.
    /// </summary>
    public class JSCIReportModel : IBaseReportTemplate
    {

        public string ParentTemplateName { get; set; }
         
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


        public byte[] AustGovImageData { get; set; }


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
            get{    return "JSCI Report";  }
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
}
