namespace ProgramAssuranceTool.Reports
{
    /// <summary>
    /// Summary description for ComplianceReport.
    /// </summary>
    partial class ComplianceReport
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ComplianceReport));
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
			this.label8 = new DataDynamics.ActiveReports.Label();
			this.label9 = new DataDynamics.ActiveReports.Label();
			this.label10 = new DataDynamics.ActiveReports.Label();
			this.label11 = new DataDynamics.ActiveReports.Label();
			this.label12 = new DataDynamics.ActiveReports.Label();
			this.label13 = new DataDynamics.ActiveReports.Label();
			this.label14 = new DataDynamics.ActiveReports.Label();
			this.PrintedBy = new DataDynamics.ActiveReports.TextBox();
			this.detail = new DataDynamics.ActiveReports.Detail();
			this.textBox1 = new DataDynamics.ActiveReports.TextBox();
			this.textbox2 = new DataDynamics.ActiveReports.TextBox();
			this.textBox3 = new DataDynamics.ActiveReports.TextBox();
			this.ComplianceIndicator = new DataDynamics.ActiveReports.TextBox();
			this.InProgressReviewCount = new DataDynamics.ActiveReports.TextBox();
			this.CompletedReviewCount = new DataDynamics.ActiveReports.TextBox();
			this.ValidCount = new DataDynamics.ActiveReports.TextBox();
			this.ValidAdminCount = new DataDynamics.ActiveReports.TextBox();
			this.InvalidAdminCount = new DataDynamics.ActiveReports.TextBox();
			this.InvalidRecovery = new DataDynamics.ActiveReports.TextBox();
			this.InvalidNoRecovery = new DataDynamics.ActiveReports.TextBox();
			this.TotalRecoveryAmount = new DataDynamics.ActiveReports.TextBox();
			this.TotalReviewCount = new DataDynamics.ActiveReports.TextBox();
			this.pageFooter = new DataDynamics.ActiveReports.PageFooter();
			this.reportInfo1 = new DataDynamics.ActiveReports.ReportInfo();
			this.line2 = new DataDynamics.ActiveReports.Line();
			this.label15 = new DataDynamics.ActiveReports.Label();
			this.textBox4 = new DataDynamics.ActiveReports.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.label1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label5)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label6)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label7)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label8)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label9)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label10)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label11)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label12)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label13)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label14)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.PrintedBy)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.textBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.textbox2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.textBox3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ComplianceIndicator)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.InProgressReviewCount)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.CompletedReviewCount)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ValidCount)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ValidAdminCount)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.InvalidAdminCount)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.InvalidRecovery)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.InvalidNoRecovery)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.TotalRecoveryAmount)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.TotalReviewCount)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label15)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.textBox4)).BeginInit();
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
            this.label8,
            this.label9,
            this.label10,
            this.label11,
            this.label12,
            this.label13,
            this.label14,
            this.PrintedBy,
            this.label15});
			this.pageHeader.Height = 1.8125F;
			this.pageHeader.Name = "pageHeader";
			// 
			// label1
			// 
			this.label1.Height = 0.312F;
			this.label1.HyperLink = null;
			this.label1.Left = 3.625F;
			this.label1.Name = "label1";
			this.label1.Style = "font-size: 18pt; font-weight: bold; text-align: center; ddo-char-set: 1";
			this.label1.Text = "Compliance Risk Indicator Report\r\n";
			this.label1.Top = 0F;
			this.label1.Width = 4.375F;
			// 
			// label2
			// 
			this.label2.Height = 0.8750001F;
			this.label2.HyperLink = null;
			this.label2.Left = 0.06200006F;
			this.label2.Name = "label2";
			this.label2.Style = "font-weight: bold; text-align: left; vertical-align: bottom";
			this.label2.Text = "Org Code";
			this.label2.Top = 0.8750001F;
			this.label2.Width = 0.6880001F;
			// 
			// label3
			// 
			this.label3.Height = 0.8750001F;
			this.label3.HyperLink = null;
			this.label3.Left = 0.812F;
			this.label3.Name = "label3";
			this.label3.Style = "font-weight: bold; text-align: left; vertical-align: bottom";
			this.label3.Text = "ESA Code";
			this.label3.Top = 0.8750001F;
			this.label3.Width = 0.6880001F;
			// 
			// label4
			// 
			this.label4.Height = 0.8750001F;
			this.label4.HyperLink = null;
			this.label4.Left = 1.562F;
			this.label4.Name = "label4";
			this.label4.Style = "font-weight: bold; text-align: left; vertical-align: bottom";
			this.label4.Text = "Site Code";
			this.label4.Top = 0.8750001F;
			this.label4.Width = 0.688F;
			// 
			// label5
			// 
			this.label5.Height = 0.8750001F;
			this.label5.HyperLink = null;
			this.label5.Left = 3.063F;
			this.label5.Name = "label5";
			this.label5.Style = "font-weight: bold; text-align: left; vertical-align: bottom";
			this.label5.Text = "Compliance Indicator";
			this.label5.Top = 0.8750001F;
			this.label5.Width = 0.8750001F;
			// 
			// line1
			// 
			this.line1.Height = 0F;
			this.line1.Left = 0.062F;
			this.line1.LineWeight = 1F;
			this.line1.Name = "line1";
			this.line1.Top = 1.77F;
			this.line1.Width = 11.814F;
			this.line1.X1 = 0.062F;
			this.line1.X2 = 11.876F;
			this.line1.Y1 = 1.77F;
			this.line1.Y2 = 1.77F;
			// 
			// label6
			// 
			this.label6.Height = 0.8750001F;
			this.label6.HyperLink = null;
			this.label6.Left = 4.001F;
			this.label6.Name = "label6";
			this.label6.Style = "font-weight: bold; text-align: left; vertical-align: bottom";
			this.label6.Text = "In Progress Reviews";
			this.label6.Top = 0.8750002F;
			this.label6.Width = 0.813F;
			// 
			// label7
			// 
			this.label7.Height = 0.8750001F;
			this.label7.HyperLink = null;
			this.label7.Left = 4.876F;
			this.label7.Name = "label7";
			this.label7.Style = "font-weight: bold; text-align: left; vertical-align: bottom";
			this.label7.Text = "Completed  Reviews";
			this.label7.Top = 0.8750002F;
			this.label7.Width = 0.813F;
			// 
			// label8
			// 
			this.label8.Height = 0.8750001F;
			this.label8.HyperLink = null;
			this.label8.Left = 7.501F;
			this.label8.Name = "label8";
			this.label8.Style = "font-weight: bold; text-align: left; vertical-align: bottom";
			this.label8.Text = "Valid (NFA)";
			this.label8.Top = 0.8750002F;
			this.label8.Width = 0.813F;
			// 
			// label9
			// 
			this.label9.Height = 0.8750001F;
			this.label9.HyperLink = null;
			this.label9.Left = 8.375999F;
			this.label9.Name = "label9";
			this.label9.Style = "font-weight: bold; text-align: left; vertical-align: bottom";
			this.label9.Text = "Valid (Admin Deficiency - Provider Education)";
			this.label9.Top = 0.8750002F;
			this.label9.Width = 0.813F;
			// 
			// label10
			// 
			this.label10.Height = 0.8750001F;
			this.label10.HyperLink = null;
			this.label10.Left = 9.250999F;
			this.label10.Name = "label10";
			this.label10.Style = "font-weight: bold; text-align: left; vertical-align: bottom";
			this.label10.Text = "Invalid (Admin Deficiency - Provider Education)";
			this.label10.Top = 0.8750002F;
			this.label10.Width = 0.813F;
			// 
			// label11
			// 
			this.label11.Height = 0.8750001F;
			this.label11.HyperLink = null;
			this.label11.Left = 10.126F;
			this.label11.Name = "label11";
			this.label11.Style = "font-weight: bold; text-align: left; vertical-align: bottom";
			this.label11.Text = "Invalid  Recovery)";
			this.label11.Top = 0.8750002F;
			this.label11.Width = 0.813F;
			// 
			// label12
			// 
			this.label12.Height = 0.8750001F;
			this.label12.HyperLink = null;
			this.label12.Left = 11.001F;
			this.label12.Name = "label12";
			this.label12.Style = "font-weight: bold; text-align: left; vertical-align: bottom";
			this.label12.Text = "Invalid (No Recovery)";
			this.label12.Top = 0.8750002F;
			this.label12.Width = 0.813F;
			// 
			// label13
			// 
			this.label13.Height = 0.8750001F;
			this.label13.HyperLink = null;
			this.label13.Left = 6.626F;
			this.label13.Name = "label13";
			this.label13.Style = "font-weight: bold; text-align: left; vertical-align: bottom";
			this.label13.Text = "Total Recovery Amount ($)";
			this.label13.Top = 0.8750002F;
			this.label13.Width = 0.813F;
			// 
			// label14
			// 
			this.label14.Height = 0.8750001F;
			this.label14.HyperLink = null;
			this.label14.Left = 5.751F;
			this.label14.Name = "label14";
			this.label14.Style = "font-weight: bold; text-align: left; vertical-align: bottom";
			this.label14.Text = "Total Number of Reviews";
			this.label14.Top = 0.8750002F;
			this.label14.Width = 0.813F;
			// 
			// PrintedBy
			// 
			this.PrintedBy.Height = 0.2F;
			this.PrintedBy.Left = 8.813F;
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
            this.textBox1,
            this.textbox2,
            this.textBox3,
            this.ComplianceIndicator,
            this.InProgressReviewCount,
            this.CompletedReviewCount,
            this.ValidCount,
            this.ValidAdminCount,
            this.InvalidAdminCount,
            this.InvalidRecovery,
            this.InvalidNoRecovery,
            this.TotalRecoveryAmount,
            this.TotalReviewCount,
            this.textBox4});
			this.detail.Height = 0.24375F;
			this.detail.Name = "detail";
			this.detail.Format += new System.EventHandler(this.detail_Format);
			// 
			// textBox1
			// 
			this.textBox1.DataField = "OrgCode";
			this.textBox1.Height = 0.2F;
			this.textBox1.Left = 0.062F;
			this.textBox1.Name = "textBox1";
			this.textBox1.Style = "text-align: left";
			this.textBox1.Text = "OrgCode";
			this.textBox1.Top = 0F;
			this.textBox1.Width = 0.6880001F;
			// 
			// textbox2
			// 
			this.textbox2.DataField = "ESACode";
			this.textbox2.Height = 0.2F;
			this.textbox2.Left = 0.812F;
			this.textbox2.Name = "textbox2";
			this.textbox2.Style = "text-align: left";
			this.textbox2.Text = "ESACode";
			this.textbox2.Top = 0F;
			this.textbox2.Width = 0.6880001F;
			// 
			// textBox3
			// 
			this.textBox3.DataField = "SiteCode";
			this.textBox3.Height = 0.2F;
			this.textBox3.Left = 1.562F;
			this.textBox3.Name = "textBox3";
			this.textBox3.Style = "text-align: left";
			this.textBox3.Text = "SiteCode";
			this.textBox3.Top = 0F;
			this.textBox3.Width = 0.6880001F;
			// 
			// ComplianceIndicator
			// 
			this.ComplianceIndicator.DataField = "ComplianceIndicator";
			this.ComplianceIndicator.Height = 0.2F;
			this.ComplianceIndicator.Left = 3.063F;
			this.ComplianceIndicator.Name = "ComplianceIndicator";
			this.ComplianceIndicator.OutputFormat = resources.GetString("ComplianceIndicator.OutputFormat");
			this.ComplianceIndicator.Style = "text-align: right";
			this.ComplianceIndicator.Text = "Comp Ind";
			this.ComplianceIndicator.Top = 0F;
			this.ComplianceIndicator.Width = 0.8130002F;
			// 
			// InProgressReviewCount
			// 
			this.InProgressReviewCount.DataField = "InProgressReviewCount";
			this.InProgressReviewCount.Height = 0.2F;
			this.InProgressReviewCount.Left = 4.001F;
			this.InProgressReviewCount.Name = "InProgressReviewCount";
			this.InProgressReviewCount.OutputFormat = resources.GetString("InProgressReviewCount.OutputFormat");
			this.InProgressReviewCount.Style = "text-align: right";
			this.InProgressReviewCount.Text = "In Progress";
			this.InProgressReviewCount.Top = 0F;
			this.InProgressReviewCount.Width = 0.7499998F;
			// 
			// CompletedReviewCount
			// 
			this.CompletedReviewCount.DataField = "CompletedReviewCount";
			this.CompletedReviewCount.Height = 0.2F;
			this.CompletedReviewCount.Left = 4.876F;
			this.CompletedReviewCount.Name = "CompletedReviewCount";
			this.CompletedReviewCount.OutputFormat = resources.GetString("CompletedReviewCount.OutputFormat");
			this.CompletedReviewCount.Style = "text-align: right";
			this.CompletedReviewCount.Text = "Completed";
			this.CompletedReviewCount.Top = 0F;
			this.CompletedReviewCount.Width = 0.7499998F;
			// 
			// ValidCount
			// 
			this.ValidCount.DataField = "ValidCount";
			this.ValidCount.Height = 0.2F;
			this.ValidCount.Left = 7.501F;
			this.ValidCount.Name = "ValidCount";
			this.ValidCount.OutputFormat = resources.GetString("ValidCount.OutputFormat");
			this.ValidCount.Style = "text-align: right";
			this.ValidCount.Text = "Valid (NFA)";
			this.ValidCount.Top = 0F;
			this.ValidCount.Width = 0.7500003F;
			// 
			// ValidAdminCount
			// 
			this.ValidAdminCount.DataField = "ValidAdminCount";
			this.ValidAdminCount.Height = 0.2F;
			this.ValidAdminCount.Left = 8.375999F;
			this.ValidAdminCount.Name = "ValidAdminCount";
			this.ValidAdminCount.OutputFormat = resources.GetString("ValidAdminCount.OutputFormat");
			this.ValidAdminCount.Style = "text-align: right";
			this.ValidAdminCount.Text = "Valid Adm";
			this.ValidAdminCount.Top = 0F;
			this.ValidAdminCount.Width = 0.6870006F;
			// 
			// InvalidAdminCount
			// 
			this.InvalidAdminCount.DataField = "InvalidAdminCount";
			this.InvalidAdminCount.Height = 0.2F;
			this.InvalidAdminCount.Left = 9.250999F;
			this.InvalidAdminCount.Name = "InvalidAdminCount";
			this.InvalidAdminCount.OutputFormat = resources.GetString("InvalidAdminCount.OutputFormat");
			this.InvalidAdminCount.Style = "text-align: right";
			this.InvalidAdminCount.Text = "Invalid Adm";
			this.InvalidAdminCount.Top = 0F;
			this.InvalidAdminCount.Width = 0.6870006F;
			// 
			// InvalidRecovery
			// 
			this.InvalidRecovery.DataField = "InvalidRecovery";
			this.InvalidRecovery.Height = 0.2F;
			this.InvalidRecovery.Left = 10.126F;
			this.InvalidRecovery.Name = "InvalidRecovery";
			this.InvalidRecovery.OutputFormat = resources.GetString("InvalidRecovery.OutputFormat");
			this.InvalidRecovery.Style = "text-align: right";
			this.InvalidRecovery.Text = "Invalid Rec";
			this.InvalidRecovery.Top = 0F;
			this.InvalidRecovery.Width = 0.6869996F;
			// 
			// InvalidNoRecovery
			// 
			this.InvalidNoRecovery.DataField = "InvalidNoRecovery";
			this.InvalidNoRecovery.Height = 0.2F;
			this.InvalidNoRecovery.Left = 11.001F;
			this.InvalidNoRecovery.Name = "InvalidNoRecovery";
			this.InvalidNoRecovery.OutputFormat = resources.GetString("InvalidNoRecovery.OutputFormat");
			this.InvalidNoRecovery.Style = "text-align: right";
			this.InvalidNoRecovery.Text = "Invalid Non";
			this.InvalidNoRecovery.Top = 0F;
			this.InvalidNoRecovery.Width = 0.6870006F;
			// 
			// TotalRecoveryAmount
			// 
			this.TotalRecoveryAmount.DataField = "TotalRecoveryAmount";
			this.TotalRecoveryAmount.Height = 0.2F;
			this.TotalRecoveryAmount.Left = 6.626F;
			this.TotalRecoveryAmount.Name = "TotalRecoveryAmount";
			this.TotalRecoveryAmount.OutputFormat = resources.GetString("TotalRecoveryAmount.OutputFormat");
			this.TotalRecoveryAmount.Style = "text-align: right";
			this.TotalRecoveryAmount.Text = "Rec Amt";
			this.TotalRecoveryAmount.Top = 0F;
			this.TotalRecoveryAmount.Width = 0.6870001F;
			// 
			// TotalReviewCount
			// 
			this.TotalReviewCount.DataField = "TotalReviewCount";
			this.TotalReviewCount.Height = 0.2F;
			this.TotalReviewCount.Left = 5.751F;
			this.TotalReviewCount.Name = "TotalReviewCount";
			this.TotalReviewCount.OutputFormat = resources.GetString("TotalReviewCount.OutputFormat");
			this.TotalReviewCount.Style = "text-align: right";
			this.TotalReviewCount.Text = "Ttl Review";
			this.TotalReviewCount.Top = 0F;
			this.TotalReviewCount.Width = 0.6870001F;
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
			this.reportInfo1.Left = 8.625999F;
			this.reportInfo1.Name = "reportInfo1";
			this.reportInfo1.Style = "text-align: right";
			this.reportInfo1.Top = 0.062F;
			this.reportInfo1.Width = 3.187F;
			// 
			// line2
			// 
			this.line2.Height = 0F;
			this.line2.Left = 0.062F;
			this.line2.LineWeight = 1F;
			this.line2.Name = "line2";
			this.line2.Top = 0F;
			this.line2.Width = 11.838F;
			this.line2.X1 = 0.062F;
			this.line2.X2 = 11.9F;
			this.line2.Y1 = 0F;
			this.line2.Y2 = 0F;
			// 
			// label15
			// 
			this.label15.Height = 0.875F;
			this.label15.HyperLink = null;
			this.label15.Left = 2.312F;
			this.label15.Name = "label15";
			this.label15.Style = "font-weight: bold; text-align: left; vertical-align: bottom";
			this.label15.Text = "Project Type";
			this.label15.Top = 0.875F;
			this.label15.Width = 0.688F;
			// 
			// textBox4
			// 
			this.textBox4.DataField = "ProjectType";
			this.textBox4.Height = 0.2F;
			this.textBox4.Left = 2.312F;
			this.textBox4.Name = "textBox4";
			this.textBox4.Style = "text-align: left";
			this.textBox4.Text = "ProjectType";
			this.textBox4.Top = 0F;
			this.textBox4.Width = 0.6880001F;
			// 
			// ComplianceReport
			// 
			this.MasterReport = false;
			xmlDataSource1.FileURL = "C:\\Userdata\\Source\\ESC\\Release\\WebSites\\ProgramAssuranceTool\\ProgramAssuranceTool" +
    "\\Content\\Xml\\ComplianceRiskIndicatorReport.xml";
			xmlDataSource1.RecordsetPattern = "ArrayOfComplianceRiskIndicator/ComplianceRiskIndicator";
			this.DataSource = xmlDataSource1;
			this.PageSettings.PaperHeight = 11F;
			this.PageSettings.PaperWidth = 8.5F;
			this.PrintWidth = 12F;
			this.Sections.Add(this.pageHeader);
			this.Sections.Add(this.detail);
			this.Sections.Add(this.pageFooter);
			this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" +
            "l; font-size: 10pt; color: Black; ddo-char-set: 186", "Normal"));
			this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"));
			this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" +
            "lic", "Heading2", "Normal"));
			this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"));
			this.ReportStart += new System.EventHandler(this.ComplianceReport_ReportStart);
			((System.ComponentModel.ISupportInitialize)(this.label1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label5)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label6)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label7)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label8)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label9)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label10)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label11)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label12)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label13)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label14)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.PrintedBy)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.textBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.textbox2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.textBox3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ComplianceIndicator)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.InProgressReviewCount)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.CompletedReviewCount)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ValidCount)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ValidAdminCount)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.InvalidAdminCount)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.InvalidRecovery)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.InvalidNoRecovery)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.TotalRecoveryAmount)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.TotalReviewCount)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label15)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.textBox4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private DataDynamics.ActiveReports.Label label1;
        private DataDynamics.ActiveReports.ReportInfo reportInfo1;
        private DataDynamics.ActiveReports.Label label2;
        private DataDynamics.ActiveReports.Label label3;
        private DataDynamics.ActiveReports.Label label4;
        private DataDynamics.ActiveReports.Label label5;
        private DataDynamics.ActiveReports.Line line1;
        private DataDynamics.ActiveReports.Label label6;
        private DataDynamics.ActiveReports.Label label7;
        private DataDynamics.ActiveReports.TextBox textBox1;
        private DataDynamics.ActiveReports.TextBox textbox2;
        private DataDynamics.ActiveReports.TextBox textBox3;
        private DataDynamics.ActiveReports.TextBox ComplianceIndicator;
        private DataDynamics.ActiveReports.TextBox InProgressReviewCount;
        private DataDynamics.ActiveReports.TextBox CompletedReviewCount;
        private DataDynamics.ActiveReports.Label label8;
        private DataDynamics.ActiveReports.Label label9;
        private DataDynamics.ActiveReports.Label label10;
        private DataDynamics.ActiveReports.Label label11;
        private DataDynamics.ActiveReports.Label label12;
        private DataDynamics.ActiveReports.Label label13;
        private DataDynamics.ActiveReports.TextBox ValidCount;
        private DataDynamics.ActiveReports.TextBox ValidAdminCount;
        private DataDynamics.ActiveReports.TextBox InvalidAdminCount;
        private DataDynamics.ActiveReports.TextBox InvalidRecovery;
        private DataDynamics.ActiveReports.TextBox InvalidNoRecovery;
        private DataDynamics.ActiveReports.TextBox TotalRecoveryAmount;
        private DataDynamics.ActiveReports.Label label14;
        private DataDynamics.ActiveReports.TextBox TotalReviewCount;
        private DataDynamics.ActiveReports.Line line2;
        private DataDynamics.ActiveReports.TextBox PrintedBy;
		  private DataDynamics.ActiveReports.Label label15;
		  private DataDynamics.ActiveReports.TextBox textBox4;
    }
}
