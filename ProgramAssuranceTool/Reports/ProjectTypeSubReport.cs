using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DataDynamics.ActiveReports;
using DataDynamics.ActiveReports.Document;
using ProgramAssuranceTool.Helpers;

namespace ProgramAssuranceTool.Reports
{
    /// <summary>
    /// Summary description for ProjectTypeSubReport.
    /// </summary>
    public partial class ProjectTypeSubReport : DataDynamics.ActiveReports.ActiveReport
    {

	    private bool IsDownloadedAsCsv = false;

        public ProjectTypeSubReport()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

	    public ProjectTypeSubReport(bool isDownloadedAsCsv):this()
	    {
		    IsDownloadedAsCsv = isDownloadedAsCsv;
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
