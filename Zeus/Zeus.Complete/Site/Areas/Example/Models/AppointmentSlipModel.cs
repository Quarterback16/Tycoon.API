using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Area.Example.Models
{
    public class AppointmentSlipModel : IBaseReportTemplate
    {
        public DateTime LetterDate { get; set; }

        public string JSKId { get; set; }

        public string ReceiverName { get; set; }

        public string ReceiverAddress { get; set; }

        public string AppointmentDescription { get; set; }

        public AppointmentDetails AppointmentDetails { get; set; }

        public byte[] AustGovImageData { get; set; }



        #region IBaseReportTemplate Members

        /// <summary>
        /// Template name which defines styles for @media print. The value is not mandatory.
        /// </summary>
        public string PrintStyleTemplateName { get; set; }


        /// <summary>
        /// Template name which defines styles for @media screen. The value is not mandatory.
        /// </summary>
        public string ScreenStyleTemplateName { get; set; }

        public string PageTitle
        {
            get;
            set;
        }

        public string StyleTemplateName
        {
            get;
            set;
        }

        public string ParentTemplateName { get; set; }
        #endregion
    }


    public class AppointmentDetails
    {
        // public DateTime Time { get; set; }

        public DateTime AppointmentDateTime { get; set; }

        public string Location { get; set; }

        public string Phone { get; set; }

        public string ContactName { get; set; }

        public string Format { get; set; }




    }
}