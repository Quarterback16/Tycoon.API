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
    public partial class ProjectTypeReport : DataDynamics.ActiveReports.ActiveReport, IReport
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

        public ProjectTypeReport()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        private void ProjectTypeReport_ReportStart(object sender, EventArgs e)
        {
            this.PrintedBy.Text = string.Format("Printed By: {0}", HttpContext.Current.User.Identity.Name);

            var ds1 = new XMLDataSource();
            ds1.NodeList = (System.Xml.XmlNodeList)((XMLDataSource)this.DataSource).Field("ProjectByOrganisation/ProjectTypeDetail", true);
				this.ProjectByOrgSubReport.Report = new ProjectTypeSubReport(IsDownloadedAsCsv);
            this.ProjectByOrgSubReport.Report.DataSource = ds1;

            var ds2 = new XMLDataSource();
            ds2.NodeList = (System.Xml.XmlNodeList)((XMLDataSource)this.DataSource).Field("ProjectByType/ProjectTypeDetail", true);
				this.ProjectByTypeSubReport.Report = new ProjectTypeSubReport(IsDownloadedAsCsv);
            this.ProjectByTypeSubReport.Report.DataSource = ds2;

            var ds3 = new XMLDataSource();
            ds3.NodeList = (System.Xml.XmlNodeList)((XMLDataSource)this.DataSource).Field("ProjectByESA/ProjectTypeDetail", true);
				this.ProjectByESASubReport.Report = new ProjectTypeSubReport(IsDownloadedAsCsv);
            this.ProjectByESASubReport.Report.DataSource = ds3;

            var ds4 = new XMLDataSource();
            ds4.NodeList = (System.Xml.XmlNodeList)((XMLDataSource)this.DataSource).Field("ProjectByState/ProjectTypeDetail", true);
				this.ProjectByStateSubReport.Report = new ProjectTypeSubReport(IsDownloadedAsCsv);
            this.ProjectByStateSubReport.Report.DataSource = ds4;

            var ds5 = new XMLDataSource();
            ds5.NodeList = (System.Xml.XmlNodeList)((XMLDataSource)this.DataSource).Field("ProjectByNational/ProjectTypeDetail", true);
				this.ProjectByNationalSubReport.Report = new ProjectTypeSubReport(IsDownloadedAsCsv);
            this.ProjectByNationalSubReport.Report.DataSource = ds5;

        }

    }
}
