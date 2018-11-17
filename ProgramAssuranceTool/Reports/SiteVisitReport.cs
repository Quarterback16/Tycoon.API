using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Printing;
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
    /// Summary description for ComplianceReport.
    /// </summary>
    public partial class SiteVisitReport : DataDynamics.ActiveReports.ActiveReport, IReport
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

        public SiteVisitReport()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

				// this is to set the orientation & paper kind
				this.PageSettings.Orientation = PageOrientation.Landscape;

				// this is to set the paper size, but somehow it doesn't take any effect.
				this.PageSettings.PaperKind = PaperKind.A3;

				// this is to set the paper size, and this seems to work 
				this.Document.Printer.PaperKind = PaperKind.A3;
        }

        private void SiteVisitReport_ReportStart(object sender, EventArgs e)
        {
            this.PrintedBy.Text = string.Format("Printed By: {0}", HttpContext.Current.User.Identity.Name);

            var ds = new XMLDataSource();
            ds.NodeList = (System.Xml.XmlNodeList)((XMLDataSource)this.DataSource).Field("SiteVisit", true);
            this.DataSource = ds;
        }

		  private void detail_Format(object sender, EventArgs e)
		  {
			  if (IsDownloadedAsCsv)
			  {
				  AppHelper.AddDoubleQuoteToText(detail.Controls);
			  }

		  }

    }
}
