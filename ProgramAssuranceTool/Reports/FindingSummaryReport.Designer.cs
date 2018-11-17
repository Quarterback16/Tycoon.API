namespace ProgramAssuranceTool.Reports
{
    /// <summary>
    /// Summary description for FindingSummaryReport .
    /// </summary>
    partial class FindingSummaryReport
    {
        private DataDynamics.ActiveReports.PageHeader pageHeader;
        private DataDynamics.ActiveReports.Detail detail;
        private DataDynamics.ActiveReports.PageFooter pageFooter;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            base.Dispose(disposing);
        }

        #region ActiveReport Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FindingSummaryReport));
            DataDynamics.ActiveReports.DataSources.XMLDataSource xmlDataSource1 = new DataDynamics.ActiveReports.DataSources.XMLDataSource();
            this.pageHeader = new DataDynamics.ActiveReports.PageHeader();
            this.label1 = new DataDynamics.ActiveReports.Label();
            this.line1 = new DataDynamics.ActiveReports.Line();
            this.PrintedBy = new DataDynamics.ActiveReports.TextBox();
            this.detail = new DataDynamics.ActiveReports.Detail();
            this.pageFooter = new DataDynamics.ActiveReports.PageFooter();
            this.reportInfo1 = new DataDynamics.ActiveReports.ReportInfo();
            this.line2 = new DataDynamics.ActiveReports.Line();
            this.groupHeader1 = new DataDynamics.ActiveReports.GroupHeader();
            this.InScopeSubReport = new DataDynamics.ActiveReports.SubReport();
            this.label15 = new DataDynamics.ActiveReports.Label();
            this.label16 = new DataDynamics.ActiveReports.Label();
            this.label6 = new DataDynamics.ActiveReports.Label();
            this.line3 = new DataDynamics.ActiveReports.Line();
            this.groupFooter1 = new DataDynamics.ActiveReports.GroupFooter();
            this.groupHeader2 = new DataDynamics.ActiveReports.GroupHeader();
            this.OutScopeSubReport = new DataDynamics.ActiveReports.SubReport();
            this.label2 = new DataDynamics.ActiveReports.Label();
            this.label3 = new DataDynamics.ActiveReports.Label();
            this.label4 = new DataDynamics.ActiveReports.Label();
            this.line4 = new DataDynamics.ActiveReports.Line();
            this.groupFooter2 = new DataDynamics.ActiveReports.GroupFooter();
            this.groupHeader3 = new DataDynamics.ActiveReports.GroupHeader();
            this.RecoverySubReport = new DataDynamics.ActiveReports.SubReport();
            this.label5 = new DataDynamics.ActiveReports.Label();
            this.label7 = new DataDynamics.ActiveReports.Label();
            this.label8 = new DataDynamics.ActiveReports.Label();
            this.line5 = new DataDynamics.ActiveReports.Line();
            this.groupFooter3 = new DataDynamics.ActiveReports.GroupFooter();
            ((System.ComponentModel.ISupportInitialize)(this.label1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PrintedBy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label15)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label16)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Controls.AddRange(new DataDynamics.ActiveReports.ARControl[] {
            this.label1,
            this.line1,
            this.PrintedBy});
            this.pageHeader.Height = 0.7390834F;
            this.pageHeader.Name = "pageHeader";
            // 
            // label1
            // 
            this.label1.Height = 0.312F;
            this.label1.HyperLink = null;
            this.label1.Left = 1.875F;
            this.label1.Name = "label1";
            this.label1.Style = "font-size: 18pt; font-weight: bold; text-align: center; ddo-char-set: 1";
            this.label1.Text = "Finding Summary Report";
            this.label1.Top = 0F;
            this.label1.Width = 4.375F;
            // 
            // line1
            // 
            this.line1.Height = 0F;
            this.line1.Left = 0.062F;
            this.line1.LineWeight = 1F;
            this.line1.Name = "line1";
            this.line1.Top = 0.687F;
            this.line1.Width = 8.138F;
            this.line1.X1 = 0.062F;
            this.line1.X2 = 8.2F;
            this.line1.Y1 = 0.687F;
            this.line1.Y2 = 0.687F;
            // 
            // PrintedBy
            // 
            this.PrintedBy.Height = 0.2F;
            this.PrintedBy.Left = 5.187F;
            this.PrintedBy.Name = "PrintedBy";
            this.PrintedBy.OutputFormat = resources.GetString("PrintedBy.OutputFormat");
            this.PrintedBy.Style = "text-align: right";
            this.PrintedBy.Text = "Printed By: XXXXXX";
            this.PrintedBy.Top = 0.375F;
            this.PrintedBy.Width = 3F;
            // 
            // detail
            // 
            this.detail.ColumnSpacing = 0F;
            this.detail.Height = 0.05625001F;
            this.detail.Name = "detail";
            this.detail.Visible = false;
            // 
            // pageFooter
            // 
            this.pageFooter.Controls.AddRange(new DataDynamics.ActiveReports.ARControl[] {
            this.reportInfo1,
            this.line2});
            this.pageFooter.Height = 0.3645833F;
            this.pageFooter.Name = "pageFooter";
            // 
            // reportInfo1
            // 
            this.reportInfo1.FormatString = "Page {PageNumber} of {PageCount} on {RunDateTime}";
            this.reportInfo1.Height = 0.2F;
            this.reportInfo1.Left = 4.999001F;
            this.reportInfo1.Name = "reportInfo1";
            this.reportInfo1.Style = "text-align: right";
            this.reportInfo1.Top = 0.06200003F;
            this.reportInfo1.Width = 3.187F;
            // 
            // line2
            // 
            this.line2.Height = 0F;
            this.line2.Left = 0.062F;
            this.line2.LineWeight = 1F;
            this.line2.Name = "line2";
            this.line2.Top = 0F;
            this.line2.Width = 10.938F;
            this.line2.X1 = 0.062F;
            this.line2.X2 = 11F;
            this.line2.Y1 = 0F;
            this.line2.Y2 = 0F;
            // 
            // groupHeader1
            // 
            this.groupHeader1.Controls.AddRange(new DataDynamics.ActiveReports.ARControl[] {
            this.InScopeSubReport,
            this.label15,
            this.label16,
            this.label6,
            this.line3});
            this.groupHeader1.Height = 1.145333F;
            this.groupHeader1.Name = "groupHeader1";
            // 
            // InScopeSubReport
            // 
            this.InScopeSubReport.CloseBorder = false;
            this.InScopeSubReport.Height = 0.312F;
            this.InScopeSubReport.Left = 0.06200001F;
            this.InScopeSubReport.Name = "InScopeSubReport";
            this.InScopeSubReport.Report = null;
            this.InScopeSubReport.ReportName = "InScopeSubReport";
            this.InScopeSubReport.Top = 0.7500001F;
            this.InScopeSubReport.Width = 6.063001F;
            // 
            // label15
            // 
            this.label15.Height = 0.251F;
            this.label15.HyperLink = null;
            this.label15.Left = 5.125F;
            this.label15.Name = "label15";
            this.label15.Style = "font-weight: bold; vertical-align: bottom";
            this.label15.Text = "Review Count";
            this.label15.Top = 0.436F;
            this.label15.Width = 1F;
            // 
            // label16
            // 
            this.label16.Height = 0.251F;
            this.label16.HyperLink = null;
            this.label16.Left = 0.06200001F;
            this.label16.Name = "label16";
            this.label16.Style = "font-weight: bold; text-align: left; vertical-align: bottom";
            this.label16.Text = "Description";
            this.label16.Top = 0.437F;
            this.label16.Width = 5F;
            // 
            // label6
            // 
            this.label6.Height = 0.3750001F;
            this.label6.HyperLink = null;
            this.label6.Left = 0.06200001F;
            this.label6.Name = "label6";
            this.label6.Style = "font-size: 11pt; font-weight: bold; vertical-align: bottom; ddo-char-set: 1";
            this.label6.Text = "In-Scope Reviews";
            this.label6.Top = 0F;
            this.label6.Width = 2.938F;
            // 
            // line3
            // 
            this.line3.Height = 0F;
            this.line3.Left = 0.06150064F;
            this.line3.LineWeight = 1F;
            this.line3.Name = "line3";
            this.line3.Top = 0.687F;
            this.line3.Width = 6.0885F;
            this.line3.X1 = 6.15F;
            this.line3.X2 = 0.06150064F;
            this.line3.Y1 = 0.687F;
            this.line3.Y2 = 0.687F;
            // 
            // groupFooter1
            // 
            this.groupFooter1.Height = 0.03125F;
            this.groupFooter1.Name = "groupFooter1";
            this.groupFooter1.Visible = false;
            // 
            // groupHeader2
            // 
            this.groupHeader2.Controls.AddRange(new DataDynamics.ActiveReports.ARControl[] {
            this.OutScopeSubReport,
            this.label2,
            this.label3,
            this.label4,
            this.line4});
            this.groupHeader2.Height = 1.187F;
            this.groupHeader2.Name = "groupHeader2";
            // 
            // OutScopeSubReport
            // 
            this.OutScopeSubReport.CloseBorder = false;
            this.OutScopeSubReport.Height = 0.312F;
            this.OutScopeSubReport.Left = 0.06200001F;
            this.OutScopeSubReport.Name = "OutScopeSubReport";
            this.OutScopeSubReport.Report = null;
            this.OutScopeSubReport.ReportName = "OutScopeSubReport";
            this.OutScopeSubReport.Top = 0.812F;
            this.OutScopeSubReport.Width = 6.063001F;
            // 
            // label2
            // 
            this.label2.Height = 0.251F;
            this.label2.HyperLink = null;
            this.label2.Left = 5.125F;
            this.label2.Name = "label2";
            this.label2.Style = "font-weight: bold; vertical-align: bottom";
            this.label2.Text = "Review Count";
            this.label2.Top = 0.4980001F;
            this.label2.Width = 1F;
            // 
            // label3
            // 
            this.label3.Height = 0.251F;
            this.label3.HyperLink = null;
            this.label3.Left = 0.06200001F;
            this.label3.Name = "label3";
            this.label3.Style = "font-weight: bold; text-align: left; vertical-align: bottom";
            this.label3.Text = "Description";
            this.label3.Top = 0.499F;
            this.label3.Width = 5F;
            // 
            // label4
            // 
            this.label4.Height = 0.3750001F;
            this.label4.HyperLink = null;
            this.label4.Left = 0.06200001F;
            this.label4.Name = "label4";
            this.label4.Style = "font-size: 11pt; font-weight: bold; vertical-align: bottom; ddo-char-set: 1";
            this.label4.Text = "Out-Scope Reviews";
            this.label4.Top = 0.06200004F;
            this.label4.Width = 2.938F;
            // 
            // line4
            // 
            this.line4.Height = 0F;
            this.line4.Left = 0.03650102F;
            this.line4.LineWeight = 1F;
            this.line4.Name = "line4";
            this.line4.Top = 0.7500001F;
            this.line4.Width = 6.113499F;
            this.line4.X1 = 6.15F;
            this.line4.X2 = 0.03650102F;
            this.line4.Y1 = 0.7500001F;
            this.line4.Y2 = 0.7500001F;
            // 
            // groupFooter2
            // 
            this.groupFooter2.Height = 0.05208333F;
            this.groupFooter2.Name = "groupFooter2";
            this.groupFooter2.Visible = false;
            // 
            // groupHeader3
            // 
            this.groupHeader3.Controls.AddRange(new DataDynamics.ActiveReports.ARControl[] {
            this.RecoverySubReport,
            this.label5,
            this.label7,
            this.label8,
            this.line5});
            this.groupHeader3.Height = 1.187F;
            this.groupHeader3.Name = "groupHeader3";
            // 
            // RecoverySubReport
            // 
            this.RecoverySubReport.CloseBorder = false;
            this.RecoverySubReport.Height = 0.312F;
            this.RecoverySubReport.Left = 0.06200001F;
            this.RecoverySubReport.Name = "RecoverySubReport";
            this.RecoverySubReport.Report = null;
            this.RecoverySubReport.ReportName = "RecoverySubReport";
            this.RecoverySubReport.Top = 0.812F;
            this.RecoverySubReport.Width = 6.063001F;
            // 
            // label5
            // 
            this.label5.Height = 0.251F;
            this.label5.HyperLink = null;
            this.label5.Left = 5.125F;
            this.label5.Name = "label5";
            this.label5.Style = "font-weight: bold; vertical-align: bottom";
            this.label5.Text = "Review Count";
            this.label5.Top = 0.4980001F;
            this.label5.Width = 1F;
            // 
            // label7
            // 
            this.label7.Height = 0.251F;
            this.label7.HyperLink = null;
            this.label7.Left = 0.06200001F;
            this.label7.Name = "label7";
            this.label7.Style = "font-weight: bold; text-align: left; vertical-align: bottom";
            this.label7.Text = "Description";
            this.label7.Top = 0.499F;
            this.label7.Width = 5F;
            // 
            // label8
            // 
            this.label8.Height = 0.3750001F;
            this.label8.HyperLink = null;
            this.label8.Left = 0.06200001F;
            this.label8.Name = "label8";
            this.label8.Style = "font-size: 11pt; font-weight: bold; vertical-align: bottom; ddo-char-set: 1";
            this.label8.Text = "Recovery";
            this.label8.Top = 0.06200004F;
            this.label8.Width = 2.938F;
            // 
            // line5
            // 
            this.line5.Height = 0F;
            this.line5.Left = 0.06850061F;
            this.line5.LineWeight = 1F;
            this.line5.Name = "line5";
            this.line5.Top = 0.7500001F;
            this.line5.Width = 6.0815F;
            this.line5.X1 = 6.15F;
            this.line5.X2 = 0.06850061F;
            this.line5.Y1 = 0.7500001F;
            this.line5.Y2 = 0.7500001F;
            // 
            // groupFooter3
            // 
            this.groupFooter3.Height = 0.04166667F;
            this.groupFooter3.Name = "groupFooter3";
            this.groupFooter3.Visible = false;
            // 
            // FindingSummaryReport
            // 
            this.MasterReport = false;
            xmlDataSource1.FileURL = "C:\\Userdata\\Source\\ESC\\Release\\WebSites\\ProgramAssuranceTool\\ProgramAssuranceTool" +
    "\\Content\\Xml\\ComplianceRiskIndicatorReport.xml";
            xmlDataSource1.RecordsetPattern = "ArrayOfComplianceRiskIndicator/ComplianceRiskIndicator";
            this.DataSource = xmlDataSource1;
            this.PageSettings.PaperHeight = 11F;
            this.PageSettings.PaperWidth = 8.5F;
            this.PrintWidth = 8.312F;
            this.Sections.Add(this.pageHeader);
            this.Sections.Add(this.groupHeader1);
            this.Sections.Add(this.groupHeader2);
            this.Sections.Add(this.groupHeader3);
            this.Sections.Add(this.detail);
            this.Sections.Add(this.groupFooter3);
            this.Sections.Add(this.groupFooter2);
            this.Sections.Add(this.groupFooter1);
            this.Sections.Add(this.pageFooter);
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" +
            "l; font-size: 10pt; color: Black; ddo-char-set: 186", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" +
            "lic", "Heading2", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"));
            this.ReportStart += new System.EventHandler(this.FindingSummaryReport_ReportStart);
            ((System.ComponentModel.ISupportInitialize)(this.label1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PrintedBy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label15)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label16)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private DataDynamics.ActiveReports.Label label1;
        private DataDynamics.ActiveReports.ReportInfo reportInfo1;
        private DataDynamics.ActiveReports.Line line1;
        private DataDynamics.ActiveReports.Line line2;
        private DataDynamics.ActiveReports.TextBox PrintedBy;
        private DataDynamics.ActiveReports.GroupHeader groupHeader1;
        private DataDynamics.ActiveReports.SubReport InScopeSubReport;
        private DataDynamics.ActiveReports.GroupFooter groupFooter1;
        private DataDynamics.ActiveReports.GroupHeader groupHeader2;
        private DataDynamics.ActiveReports.GroupFooter groupFooter2;
        private DataDynamics.ActiveReports.GroupHeader groupHeader3;
        private DataDynamics.ActiveReports.GroupFooter groupFooter3;
        private DataDynamics.ActiveReports.Label label15;
        private DataDynamics.ActiveReports.Label label16;
        private DataDynamics.ActiveReports.Label label6;
        private DataDynamics.ActiveReports.SubReport OutScopeSubReport;
        private DataDynamics.ActiveReports.Label label2;
        private DataDynamics.ActiveReports.Label label3;
        private DataDynamics.ActiveReports.Label label4;
        private DataDynamics.ActiveReports.SubReport RecoverySubReport;
        private DataDynamics.ActiveReports.Label label5;
        private DataDynamics.ActiveReports.Label label7;
        private DataDynamics.ActiveReports.Label label8;
        private DataDynamics.ActiveReports.Line line3;
        private DataDynamics.ActiveReports.Line line4;
        private DataDynamics.ActiveReports.Line line5;
    }
}
