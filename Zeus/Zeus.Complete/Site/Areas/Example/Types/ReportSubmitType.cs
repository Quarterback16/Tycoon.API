using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Employment.Web.Mvc.Area.Example.Types
{
    public class ReportSubmitType
    {
        /// <summary>
        /// Submity type for generating sample report.
        /// </summary>
        public const string GenerateSampleReport = "GenerateSample";
    }
    
    public class ReportTypes
    {
        public const string BasicPdf = "Basic Pdf";
        public const string BasicHtml = "Basic Html";
        public const string BasicRtf = "Basic Rtf";
        public const string BasicDocx = "Basic Docx";

        public const string ComplexPdf = "Complex Pdf";
        public const string ComplexHtml = "Complex Html";
        public const string ComplexRtf = "Complex Rtf";
        public const string ComplexDocx = "Complex Docx";

        //public const string Appointment = "Appointment";

        //public const string Notification = "Notification";

        //public const string JSCIReport = "JSCI Report";

        //public const string JSCIHistory = "JSCI History";

        //public const string SessionKeysuffix = "Template";
    }
}