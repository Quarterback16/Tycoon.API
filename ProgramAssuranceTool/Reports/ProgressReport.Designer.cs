namespace ProgramAssuranceTool.Reports
{
    /// <summary>
    /// Summary description for ProgressReport .
    /// </summary>
    partial class ProgressReport 
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ProgressReport));
			DataDynamics.ActiveReports.DataSources.XMLDataSource xmlDataSource1 = new DataDynamics.ActiveReports.DataSources.XMLDataSource();
			this.pageHeader = new DataDynamics.ActiveReports.PageHeader();
			this.label1 = new DataDynamics.ActiveReports.Label();
			this.label2 = new DataDynamics.ActiveReports.Label();
			this.label3 = new DataDynamics.ActiveReports.Label();
			this.label4 = new DataDynamics.ActiveReports.Label();
			this.label5 = new DataDynamics.ActiveReports.Label();
			this.line1 = new DataDynamics.ActiveReports.Line();
			this.label6 = new DataDynamics.ActiveReports.Label();
			this.label7 = new DataDynamics.ActiveReports.Label();
			this.label14 = new DataDynamics.ActiveReports.Label();
			this.PrintedBy = new DataDynamics.ActiveReports.TextBox();
			this.detail = new DataDynamics.ActiveReports.Detail();
			this.SampleName = new DataDynamics.ActiveReports.TextBox();
			this.textbox2 = new DataDynamics.ActiveReports.TextBox();
			this.InProgressReviewCount = new DataDynamics.ActiveReports.TextBox();
			this.CompletedReviewCount = new DataDynamics.ActiveReports.TextBox();
			this.TotalReviewCount = new DataDynamics.ActiveReports.TextBox();
			this.PercentCompleted = new DataDynamics.ActiveReports.TextBox();
			this.LastUpdateDate = new DataDynamics.ActiveReports.TextBox();
			this.pageFooter = new DataDynamics.ActiveReports.PageFooter();
			this.reportInfo1 = new DataDynamics.ActiveReports.ReportInfo();
			this.line2 = new DataDynamics.ActiveReports.Line();
			((System.ComponentModel.ISupportInitialize)(this.label1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label5)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label6)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label7)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label14)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.PrintedBy)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.SampleName)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.textbox2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.InProgressReviewCount)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.CompletedReviewCount)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.TotalReviewCount)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.PercentCompleted)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.LastUpdateDate)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			// 
			// pageHeader
			// 
			this.pageHeader.Controls.AddRange(new DataDynamics.ActiveReports.ARControl[] {
            this.label1,
            this.label2,
            this.label3,
            this.label4,
            this.label5,
            this.line1,
            this.label6,
            this.label7,
            this.label14,
            this.PrintedBy});
			this.pageHeader.Height = 1.84325F;
			this.pageHeader.Name = "pageHeader";
			// 
			// label1
			// 
			this.label1.Height = 0.312F;
			this.label1.HyperLink = null;
			this.label1.Left = 2F;
			this.label1.Name = "label1";
			this.label1.Style = "font-size: 18pt; font-weight: bold; text-align: center; ddo-char-set: 1";
			this.label1.Text = "Progress Report\r\n";
			this.label1.Top = 0F;
			this.label1.Width = 4.375F;
			// 
			// label2
			// 
			this.label2.Height = 0.8750001F;
			this.label2.HyperLink = null;
			this.label2.Left = 0.06200006F;
			this.label2.Name = "label2";
			this.label2.Style = "font-weight: bold; vertical-align: bottom";
			this.label2.Text = "Sample Name";
			this.label2.Top = 0.8750001F;
			this.label2.Width = 3.125F;
			// 
			// label3
			// 
			this.label3.Height = 0.8750001F;
			this.label3.HyperLink = null;
			this.label3.Left = 3.25F;
			this.label3.Name = "label3";
			this.label3.Style = "font-weight: bold; vertical-align: bottom";
			this.label3.Text = "Project Type";
			this.label3.Top = 0.8750001F;
			this.label3.Width = 0.688F;
			// 
			// label4
			// 
			this.label4.Height = 0.8750001F;
			this.label4.HyperLink = null;
			this.label4.Left = 4F;
			this.label4.Name = "label4";
			this.label4.Style = "font-weight: bold; vertical-align: bottom";
			this.label4.Text = "In Progress Reviews";
			this.label4.Top = 0.8750001F;
			this.label4.Width = 0.688F;
			// 
			// label5
			// 
			this.label5.Height = 0.8750001F;
			this.label5.HyperLink = null;
			this.label5.Left = 4.75F;
			this.label5.Name = "label5";
			this.label5.Style = "font-weight: bold; text-align: left; vertical-align: bottom";
			this.label5.Text = "Completed Reviews";
			this.label5.Top = 0.8750001F;
			this.label5.Width = 0.813F;
			// 
			// line1
			// 
			this.line1.Height = 0F;
			this.line1.Left = 0F;
			this.line1.LineWeight = 1F;
			this.line1.Name = "line1";
			this.line1.Top = 1.77F;
			this.line1.Width = 8.25F;
			this.line1.X1 = 0F;
			this.line1.X2 = 8.25F;
			this.line1.Y1 = 1.77F;
			this.line1.Y2 = 1.77F;
			// 
			// label6
			// 
			this.label6.Height = 0.8750001F;
			this.label6.HyperLink = null;
			this.label6.Left = 5.625F;
			this.label6.Name = "label6";
			this.label6.Style = "font-weight: bold; text-align: left; vertical-align: bottom";
			this.label6.Text = "Total Number of Reviews";
			this.label6.Top = 0.8750001F;
			this.label6.Width = 0.813F;
			// 
			// label7
			// 
			this.label7.Height = 0.8750001F;
			this.label7.HyperLink = null;
			this.label7.Left = 6.5F;
			this.label7.Name = "label7";
			this.label7.Style = "font-weight: bold; text-align: left; vertical-align: bottom";
			this.label7.Text = "% Completed";
			this.label7.Top = 0.8750001F;
			this.label7.Width = 0.813F;
			// 
			// label14
			// 
			this.label14.Height = 0.8750001F;
			this.label14.HyperLink = null;
			this.label14.Left = 7.375F;
			this.label14.Name = "label14";
			this.label14.Style = "font-weight: bold; text-align: left; vertical-align: bottom";
			this.label14.Text = "Last Update Date";
			this.label14.Top = 0.8750001F;
			this.label14.Width = 0.813F;
			// 
			// PrintedBy
			// 
			this.PrintedBy.Height = 0.2F;
			this.PrintedBy.Left = 5.188F;
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
			this.detail.Controls.AddRange(new DataDynamics.ActiveReports.ARControl[] {
            this.SampleName,
            this.textbox2,
            this.InProgressReviewCount,
            this.CompletedReviewCount,
            this.TotalReviewCount,
            this.PercentCompleted,
            this.LastUpdateDate});
			this.detail.Height = 0.275F;
			this.detail.Name = "detail";
			this.detail.Format += new System.EventHandler(this.detail_Format);
			// 
			// SampleName
			// 
			this.SampleName.DataField = "SampleName";
			this.SampleName.Height = 0.2F;
			this.SampleName.Left = 0.06200004F;
			this.SampleName.Name = "SampleName";
			this.SampleName.Style = "text-align: left";
			this.SampleName.Text = "SampleName";
			this.SampleName.Top = 0F;
			this.SampleName.Width = 3.125F;
			// 
			// textbox2
			// 
			this.textbox2.DataField = "ProjectType";
			this.textbox2.Height = 0.2F;
			this.textbox2.Left = 3.25F;
			this.textbox2.Name = "textbox2";
			this.textbox2.Style = "text-align: left";
			this.textbox2.Text = "ProjectType";
			this.textbox2.Top = 0F;
			this.textbox2.Width = 0.688F;
			// 
			// InProgressReviewCount
			// 
			this.InProgressReviewCount.DataField = "InProgressReviewCount";
			this.InProgressReviewCount.Height = 0.2F;
			this.InProgressReviewCount.Left = 4F;
			this.InProgressReviewCount.Name = "InProgressReviewCount";
			this.InProgressReviewCount.OutputFormat = resources.GetString("InProgressReviewCount.OutputFormat");
			this.InProgressReviewCount.Style = "text-align: right";
			this.InProgressReviewCount.Text = "InProgressReviewCount";
			this.InProgressReviewCount.Top = 0F;
			this.InProgressReviewCount.Width = 0.6880001F;
			// 
			// CompletedReviewCount
			// 
			this.CompletedReviewCount.DataField = "CompletedReviewCount";
			this.CompletedReviewCount.Height = 0.2F;
			this.CompletedReviewCount.Left = 4.75F;
			this.CompletedReviewCount.Name = "CompletedReviewCount";
			this.CompletedReviewCount.OutputFormat = resources.GetString("CompletedReviewCount.OutputFormat");
			this.CompletedReviewCount.Style = "text-align: right";
			this.CompletedReviewCount.Text = "CompletedReviewCount";
			this.CompletedReviewCount.Top = 0F;
			this.CompletedReviewCount.Width = 0.8130002F;
			// 
			// TotalReviewCount
			// 
			this.TotalReviewCount.DataField = "TotalReviewCount";
			this.TotalReviewCount.Height = 0.2F;
			this.TotalReviewCount.Left = 5.625F;
			this.TotalReviewCount.Name = "TotalReviewCount";
			this.TotalReviewCount.OutputFormat = resources.GetString("TotalReviewCount.OutputFormat");
			this.TotalReviewCount.Style = "text-align: right";
			this.TotalReviewCount.Text = "TotalReviewCount";
			this.TotalReviewCount.Top = 0F;
			this.TotalReviewCount.Width = 0.813F;
			// 
			// PercentCompleted
			// 
			this.PercentCompleted.DataField = "PercentCompleted";
			this.PercentCompleted.Height = 0.2F;
			this.PercentCompleted.Left = 6.5F;
			this.PercentCompleted.Name = "PercentCompleted";
			this.PercentCompleted.OutputFormat = resources.GetString("PercentCompleted.OutputFormat");
			this.PercentCompleted.Style = "text-align: right";
			this.PercentCompleted.Text = "PercentCompleted";
			this.PercentCompleted.Top = 0F;
			this.PercentCompleted.Width = 0.813F;
			// 
			// LastUpdateDate
			// 
			this.LastUpdateDate.DataField = "LastUpdateDate";
			this.LastUpdateDate.Height = 0.2F;
			this.LastUpdateDate.Left = 7.375F;
			this.LastUpdateDate.Name = "LastUpdateDate";
			this.LastUpdateDate.OutputFormat = resources.GetString("LastUpdateDate.OutputFormat");
			this.LastUpdateDate.Style = "text-align: left";
			this.LastUpdateDate.Text = "LastUpdateDate";
			this.LastUpdateDate.Top = 0F;
			this.LastUpdateDate.Width = 0.813F;
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
			this.reportInfo1.Left = 5.001F;
			this.reportInfo1.Name = "reportInfo1";
			this.reportInfo1.Style = "text-align: right";
			this.reportInfo1.Top = 0.062F;
			this.reportInfo1.Width = 3.187F;
			// 
			// line2
			// 
			this.line2.Height = 0F;
			this.line2.Left = 0F;
			this.line2.LineWeight = 1F;
			this.line2.Name = "line2";
			this.line2.Top = 0F;
			this.line2.Width = 8.25F;
			this.line2.X1 = 0F;
			this.line2.X2 = 8.25F;
			this.line2.Y1 = 0F;
			this.line2.Y2 = 0F;
			// 
			// ProgressReport
			// 
			this.MasterReport = false;
			xmlDataSource1.FileURL = "C:\\Userdata\\Source\\ESC\\Release\\WebSites\\ProgramAssuranceTool\\ProgramAssuranceTool" +
    "\\Content\\Xml\\ComplianceRiskIndicatorReport.xml";
			xmlDataSource1.RecordsetPattern = "ArrayOfComplianceRiskIndicator/ComplianceRiskIndicator";
			this.DataSource = xmlDataSource1;
			this.PageSettings.PaperHeight = 11F;
			this.PageSettings.PaperWidth = 8.5F;
			this.PrintWidth = 8.27F;
			this.Script = "\r\n";
			this.Sections.Add(this.pageHeader);
			this.Sections.Add(this.detail);
			this.Sections.Add(this.pageFooter);
			this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" +
            "l; font-size: 10pt; color: Black; ddo-char-set: 186", "Normal"));
			this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"));
			this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" +
            "lic", "Heading2", "Normal"));
			this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"));
			this.ReportStart += new System.EventHandler(this.ProgressReport_ReportStart);
			((System.ComponentModel.ISupportInitialize)(this.label1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label5)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label6)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label7)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label14)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.PrintedBy)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.SampleName)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.textbox2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.InProgressReviewCount)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.CompletedReviewCount)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.TotalReviewCount)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.PercentCompleted)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.LastUpdateDate)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private DataDynamics.ActiveReports.Label label1;
        private DataDynamics.ActiveReports.ReportInfo reportInfo1;
        private DataDynamics.ActiveReports.Label label2;
        private DataDynamics.ActiveReports.Label label3;
        private DataDynamics.ActiveReports.Label label4;
        private DataDynamics.ActiveReports.Line line1;
        private DataDynamics.ActiveReports.TextBox SampleName;
        private DataDynamics.ActiveReports.TextBox textbox2;
        private DataDynamics.ActiveReports.TextBox InProgressReviewCount;
        private DataDynamics.ActiveReports.TextBox CompletedReviewCount;
        private DataDynamics.ActiveReports.TextBox TotalReviewCount;
        private DataDynamics.ActiveReports.TextBox PercentCompleted;
        private DataDynamics.ActiveReports.Label label14;
        private DataDynamics.ActiveReports.TextBox LastUpdateDate;
        private DataDynamics.ActiveReports.Line line2;
        private DataDynamics.ActiveReports.TextBox PrintedBy;
        private DataDynamics.ActiveReports.Label label5;
        private DataDynamics.ActiveReports.Label label6;
		  private DataDynamics.ActiveReports.Label label7;
    }
}
