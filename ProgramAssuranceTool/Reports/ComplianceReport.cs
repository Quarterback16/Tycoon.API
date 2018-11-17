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
    public partial class ComplianceReport : DataDynamics.ActiveReports.ActiveReport, IReport
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

        public ComplianceReport()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

			  // this is to set the orientation & paper kind
				this.PageSettings.Orientation = PageOrientation.Landscape;
			
				this.PageSettings.Margins.Left = 0.196F;
				this.PageSettings.Margins.Right = 0.196F;
        }

        private void ComplianceReport_ReportStart(object sender, EventArgs e)
        {
            var serviceProviderDs = new XMLDataSource();
            serviceProviderDs.NodeList = (System.Xml.XmlNodeList)((XMLDataSource)this.DataSource).Field("ComplianceRiskIndicator", true);
            this.DataSource = serviceProviderDs;

            this.PrintedBy.Text = string.Format("Printed By: {0}", HttpContext.Current.User.Identity.Name);
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
