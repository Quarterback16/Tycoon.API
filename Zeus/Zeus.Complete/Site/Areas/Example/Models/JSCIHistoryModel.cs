using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Area.Example.Models
{
    public class JSCIHistoryModel : IBaseReportTemplate
    {
        public string Name { get; set; }

        public byte[] AustGovImageData { get; set; }


        public string JskID { get; set; }


        public List<HistoryRecord> HistoryRecords { get; set; }

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


    public class HistoryRecord
    {
        public DateTime AssessmentDate { get; set; }

        public UserType CreatedBy { get; set; }

        public UserType UpdatedBy { get; set; }

        public string Result { get; set; }

        public StatusType Status { get; set; }

        public string JCA { get; set; }

    }


    public enum UserType
    {
        ESL_JSC,
        BFL767,
        BMB,
        NOE707
    }


    public enum StatusType
    {
        Active,
        Inactive
    }
}

 
