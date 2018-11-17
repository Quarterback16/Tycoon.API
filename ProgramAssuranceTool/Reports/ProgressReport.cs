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
    /// Summary description for ProgressReport .
    /// </summary>
    public partial class ProgressReport : DataDynamics.ActiveReports.ActiveReport, IReport
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

        public ProgressReport()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        private void ProgressReport_ReportStart(object sender, EventArgs e)
        {
            var serviceProviderDs = new XMLDataSource();
            serviceProviderDs.NodeList = (System.Xml.XmlNodeList)((XMLDataSource)this.DataSource).Field("Progress", true);
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
