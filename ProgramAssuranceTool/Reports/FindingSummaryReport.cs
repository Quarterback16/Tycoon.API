using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DataDynamics.ActiveReports;
using DataDynamics.ActiveReports.DataSources;
using DataDynamics.ActiveReports.Document;
using ProgramAssuranceTool.Helpers;
using ProgramAssuranceTool.Infrastructure.Interfaces;
using System.Web;
using System.Web.WebPages;

namespace ProgramAssuranceTool.Reports
{
    /// <summary>
    /// Summary description for FindingSummaryReport .
    /// </summary>
    public partial class FindingSummaryReport : DataDynamics.ActiveReports.ActiveReport, IReport
    {
		 private bool IsDownloadedAsCsv
		 {
			 get
			 {
				 var isDownloadedAsCsv = false;

				 if (this.Parameters["DownloadType"] != null)
				 {
					 isDownloadedAsCsv = (this.Parameters["DownloadType"].Value == CommonConstants.Csv);
				 }

				 return isDownloadedAsCsv;
			 }
		 }

        public FindingSummaryReport()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        private void FindingSummaryReport_ReportStart(object sender, EventArgs e)
        {
            this.PrintedBy.Text = string.Format("Printed By: {0}", HttpContext.Current.User.Identity.Name);

            var ds1 = new XMLDataSource();
            ds1.NodeList = (System.Xml.XmlNodeList)((XMLDataSource)this.DataSource).Field("InScopeReview/FindingSummaryDetail", true);
            this.InScopeSubReport.Report = new FindingSummarySubReport(IsDownloadedAsCsv);
            this.InScopeSubReport.Report.DataSource = ds1;

            var ds2 = new XMLDataSource();
            ds2.NodeList = (System.Xml.XmlNodeList)((XMLDataSource)this.DataSource).Field("OutScopeReview/FindingSummaryDetail", true);
				this.OutScopeSubReport.Report = new FindingSummarySubReport(IsDownloadedAsCsv);
            this.OutScopeSubReport.Report.DataSource = ds2;

            var ds3 = new XMLDataSource();
            ds3.NodeList = (System.Xml.XmlNodeList)((XMLDataSource)this.DataSource).Field("RecoveryReview/FindingSummaryDetail", true);
				this.RecoverySubReport.Report = new FindingSummarySubReport(IsDownloadedAsCsv);
            this.RecoverySubReport.Report.DataSource = ds3;

        }

    }
}
