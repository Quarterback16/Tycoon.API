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
    /// Summary description for FindingSummarySubReport.
    /// </summary>
    public partial class FindingSummarySubReport : DataDynamics.ActiveReports.ActiveReport
    {
	    public bool IsDownloadedAsCsv = false;

        public FindingSummarySubReport()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

		  public FindingSummarySubReport(bool isDownloadedAsCsv):this()
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
