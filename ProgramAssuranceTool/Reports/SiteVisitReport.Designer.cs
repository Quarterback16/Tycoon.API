namespace ProgramAssuranceTool.Reports
{
    /// <summary>
    /// Summary description for ComplianceReport.
    /// </summary>
    partial class SiteVisitReport
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
			DataDynamics.ActiveReports.DataSources.XMLDataSource xmlDataSource1 = new DataDynamics.ActiveReports.DataSources.XMLDataSource();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SiteVisitReport));
			this.pageHeader = new DataDynamics.ActiveReports.PageHeader();
			this.label1 = new DataDynamics.ActiveReports.Label();
			this.line1 = new DataDynamics.ActiveReports.Line();
			this.PrintedBy = new DataDynamics.ActiveReports.TextBox();
			this.label2 = new DataDynamics.ActiveReports.Label();
			this.detail = new DataDynamics.ActiveReports.Detail();
			this.ClaimTypeDescription = new DataDynamics.ActiveReports.TextBox();
			this.JobSeekerName = new DataDynamics.ActiveReports.TextBox();
			this.ClaimID = new DataDynamics.ActiveReports.TextBox();
			this.JobSeekerID = new DataDynamics.ActiveReports.TextBox();
			this.textBox13 = new DataDynamics.ActiveReports.TextBox();
			this.ClaimAmount = new DataDynamics.ActiveReports.TextBox();
			this.ClaimCreationDate = new DataDynamics.ActiveReports.TextBox();
			this.textBox3 = new DataDynamics.ActiveReports.TextBox();
			this.DaysOverdue = new DataDynamics.ActiveReports.TextBox();
			this.textBox7 = new DataDynamics.ActiveReports.TextBox();
			this.textBox8 = new DataDynamics.ActiveReports.TextBox();
			this.LastUpdateDate = new DataDynamics.ActiveReports.TextBox();
			this.textBox10 = new DataDynamics.ActiveReports.TextBox();
			this.textBox11 = new DataDynamics.ActiveReports.TextBox();
			this.UploadDate = new DataDynamics.ActiveReports.TextBox();
			this.OrgCode = new DataDynamics.ActiveReports.TextBox();
			this.pageFooter = new DataDynamics.ActiveReports.PageFooter();
			this.reportInfo1 = new DataDynamics.ActiveReports.ReportInfo();
			this.line2 = new DataDynamics.ActiveReports.Line();
			this.groupHeader1 = new DataDynamics.ActiveReports.GroupHeader();
			this.OrgName = new DataDynamics.ActiveReports.TextBox();
			this.ESACode = new DataDynamics.ActiveReports.TextBox();
			this.label4 = new DataDynamics.ActiveReports.Label();
			this.ESAName = new DataDynamics.ActiveReports.TextBox();
			this.SiteCode = new DataDynamics.ActiveReports.TextBox();
			this.label16 = new DataDynamics.ActiveReports.Label();
			this.SiteName = new DataDynamics.ActiveReports.TextBox();
			this.lblOrg = new DataDynamics.ActiveReports.Label();
			this.lblESA = new DataDynamics.ActiveReports.Label();
			this.lblSite = new DataDynamics.ActiveReports.Label();
			this.line3 = new DataDynamics.ActiveReports.Line();
			this.groupFooter1 = new DataDynamics.ActiveReports.GroupFooter();
			this.label18 = new DataDynamics.ActiveReports.Label();
			this.groupHeader2 = new DataDynamics.ActiveReports.GroupHeader();
			this.ProjectID = new DataDynamics.ActiveReports.TextBox();
			this.ProjectName = new DataDynamics.ActiveReports.TextBox();
			this.label20 = new DataDynamics.ActiveReports.Label();
			this.label22 = new DataDynamics.ActiveReports.Label();
			this.label27 = new DataDynamics.ActiveReports.Label();
			this.label25 = new DataDynamics.ActiveReports.Label();
			this.label28 = new DataDynamics.ActiveReports.Label();
			this.label29 = new DataDynamics.ActiveReports.Label();
			this.label30 = new DataDynamics.ActiveReports.Label();
			this.label31 = new DataDynamics.ActiveReports.Label();
			this.label32 = new DataDynamics.ActiveReports.Label();
			this.label23 = new DataDynamics.ActiveReports.Label();
			this.label5 = new DataDynamics.ActiveReports.Label();
			this.label6 = new DataDynamics.ActiveReports.Label();
			this.lblProject = new DataDynamics.ActiveReports.Label();
			this.line4 = new DataDynamics.ActiveReports.Line();
			this.line5 = new DataDynamics.ActiveReports.Line();
			this.groupFooter2 = new DataDynamics.ActiveReports.GroupFooter();
			((System.ComponentModel.ISupportInitialize)(this.label1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.PrintedBy)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ClaimTypeDescription)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.JobSeekerName)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ClaimID)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.JobSeekerID)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.textBox13)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ClaimAmount)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ClaimCreationDate)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.textBox3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.DaysOverdue)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.textBox7)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.textBox8)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.LastUpdateDate)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.textBox10)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.textBox11)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.UploadDate)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.OrgCode)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.OrgName)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ESACode)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ESAName)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.SiteCode)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label16)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.SiteName)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.lblOrg)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.lblESA)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.lblSite)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label18)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ProjectID)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ProjectName)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label20)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label22)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label27)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label25)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label28)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label29)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label30)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label31)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label32)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label23)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label5)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label6)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.lblProject)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			// 
			// pageHeader
			// 
			this.pageHeader.Controls.AddRange(new DataDynamics.ActiveReports.ARControl[] {
            this.label1,
            this.line1,
            this.PrintedBy});
			this.pageHeader.Height = 0.7911667F;
			this.pageHeader.Name = "pageHeader";
			// 
			// label1
			// 
			this.label1.Height = 0.312F;
			this.label1.HyperLink = null;
			this.label1.Left = 3.625F;
			this.label1.Name = "label1";
			this.label1.Style = "font-size: 18pt; font-weight: bold; text-align: center; ddo-char-set: 1";
			this.label1.Text = "Site Visit Report\r\n";
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
			this.line1.Width = 11.538F;
			this.line1.X1 = 0.062F;
			this.line1.X2 = 11.6F;
			this.line1.Y1 = 0.687F;
			this.line1.Y2 = 0.687F;
			// 
			// PrintedBy
			// 
			this.PrintedBy.Height = 0.2F;
			this.PrintedBy.Left = 8.562F;
			this.PrintedBy.Name = "PrintedBy";
			this.PrintedBy.OutputFormat = resources.GetString("PrintedBy.OutputFormat");
			this.PrintedBy.Style = "text-align: right";
			this.PrintedBy.Text = "Printed By: XXXXXX";
			this.PrintedBy.Top = 0.375F;
			this.PrintedBy.Width = 3F;
			// 
			// label2
			// 
			this.label2.Height = 0.25F;
			this.label2.HyperLink = null;
			this.label2.Left = 0.062F;
			this.label2.Name = "label2";
			this.label2.Style = "font-weight: bold; vertical-align: top; ddo-char-set: 1";
			this.label2.Text = "Org Code:";
			this.label2.Top = 0.062F;
			this.label2.Width = 0.7500001F;
			// 
			// detail
			// 
			this.detail.ColumnSpacing = 0F;
			this.detail.Controls.AddRange(new DataDynamics.ActiveReports.ARControl[] {
            this.ClaimTypeDescription,
            this.JobSeekerName,
            this.ClaimID,
            this.JobSeekerID,
            this.textBox13,
            this.ClaimAmount,
            this.ClaimCreationDate,
            this.textBox3,
            this.DaysOverdue,
            this.textBox7,
            this.textBox8,
            this.LastUpdateDate,
            this.textBox10,
            this.textBox11,
            this.UploadDate});
			this.detail.Height = 0.8338336F;
			this.detail.Name = "detail";
			this.detail.Format += new System.EventHandler(this.detail_Format);
			// 
			// ClaimTypeDescription
			// 
			this.ClaimTypeDescription.DataField = "ClaimTypeDescription";
			this.ClaimTypeDescription.Height = 0.4380001F;
			this.ClaimTypeDescription.Left = 3.187F;
			this.ClaimTypeDescription.Name = "ClaimTypeDescription";
			this.ClaimTypeDescription.OutputFormat = resources.GetString("ClaimTypeDescription.OutputFormat");
			this.ClaimTypeDescription.Style = "text-align: left";
			this.ClaimTypeDescription.Text = "ClaimTypeDescription";
			this.ClaimTypeDescription.Top = 0.312F;
			this.ClaimTypeDescription.Width = 2.313F;
			// 
			// JobSeekerName
			// 
			this.JobSeekerName.DataField = "JobSeekerName";
			this.JobSeekerName.Height = 0.438F;
			this.JobSeekerName.Left = 0.25F;
			this.JobSeekerName.Name = "JobSeekerName";
			this.JobSeekerName.OutputFormat = resources.GetString("JobSeekerName.OutputFormat");
			this.JobSeekerName.Style = "text-align: left";
			this.JobSeekerName.Text = "JobSeekerName";
			this.JobSeekerName.Top = 0.312F;
			this.JobSeekerName.Width = 2.25F;
			// 
			// ClaimID
			// 
			this.ClaimID.DataField = "ClaimID";
			this.ClaimID.Height = 0.2F;
			this.ClaimID.Left = 2.562F;
			this.ClaimID.Name = "ClaimID";
			this.ClaimID.OutputFormat = resources.GetString("ClaimID.OutputFormat");
			this.ClaimID.Style = "text-align: left";
			this.ClaimID.Text = "ClaimID";
			this.ClaimID.Top = 0.06200001F;
			this.ClaimID.Width = 0.563F;
			// 
			// JobSeekerID
			// 
			this.JobSeekerID.DataField = "JobSeekerID";
			this.JobSeekerID.Height = 0.2F;
			this.JobSeekerID.Left = 0.25F;
			this.JobSeekerID.Name = "JobSeekerID";
			this.JobSeekerID.OutputFormat = resources.GetString("JobSeekerID.OutputFormat");
			this.JobSeekerID.Style = "text-align: left";
			this.JobSeekerID.Text = "JobSeekerID";
			this.JobSeekerID.Top = 0.06200001F;
			this.JobSeekerID.Width = 2.25F;
			// 
			// textBox13
			// 
			this.textBox13.DataField = "ClaimType";
			this.textBox13.Height = 0.438F;
			this.textBox13.Left = 2.562F;
			this.textBox13.Name = "textBox13";
			this.textBox13.OutputFormat = resources.GetString("textBox13.OutputFormat");
			this.textBox13.Style = "text-align: left";
			this.textBox13.Text = "ClaimType";
			this.textBox13.Top = 0.312F;
			this.textBox13.Width = 0.563F;
			// 
			// ClaimAmount
			// 
			this.ClaimAmount.DataField = "ClaimAmount";
			this.ClaimAmount.Height = 0.2F;
			this.ClaimAmount.Left = 3.187F;
			this.ClaimAmount.Name = "ClaimAmount";
			this.ClaimAmount.OutputFormat = resources.GetString("ClaimAmount.OutputFormat");
			this.ClaimAmount.Style = "text-align: left";
			this.ClaimAmount.Text = "ClaimAmount";
			this.ClaimAmount.Top = 0.062F;
			this.ClaimAmount.Width = 0.6880002F;
			// 
			// ClaimCreationDate
			// 
			this.ClaimCreationDate.DataField = "ClaimCreationDate";
			this.ClaimCreationDate.Height = 0.2F;
			this.ClaimCreationDate.Left = 3.937F;
			this.ClaimCreationDate.Name = "ClaimCreationDate";
			this.ClaimCreationDate.OutputFormat = resources.GetString("ClaimCreationDate.OutputFormat");
			this.ClaimCreationDate.Style = "text-align: left";
			this.ClaimCreationDate.Text = "ClaimCreationDate";
			this.ClaimCreationDate.Top = 0.062F;
			this.ClaimCreationDate.Width = 0.813F;
			// 
			// textBox3
			// 
			this.textBox3.DataField = "ContractType";
			this.textBox3.Height = 0.2F;
			this.textBox3.Left = 4.812F;
			this.textBox3.Name = "textBox3";
			this.textBox3.OutputFormat = resources.GetString("textBox3.OutputFormat");
			this.textBox3.Style = "text-align: left";
			this.textBox3.Text = "ContractType";
			this.textBox3.Top = 0.062F;
			this.textBox3.Width = 0.813F;
			// 
			// DaysOverdue
			// 
			this.DaysOverdue.DataField = "DaysOverdue";
			this.DaysOverdue.Height = 0.2F;
			this.DaysOverdue.Left = 5.687F;
			this.DaysOverdue.Name = "DaysOverdue";
			this.DaysOverdue.OutputFormat = resources.GetString("DaysOverdue.OutputFormat");
			this.DaysOverdue.Style = "text-align: left";
			this.DaysOverdue.Text = "DaysOverdue";
			this.DaysOverdue.Top = 0.062F;
			this.DaysOverdue.Width = 0.6260004F;
			// 
			// textBox7
			// 
			this.textBox7.DataField = "AssessmentOutcome";
			this.textBox7.Height = 0.2F;
			this.textBox7.Left = 6.375F;
			this.textBox7.Name = "textBox7";
			this.textBox7.OutputFormat = resources.GetString("textBox7.OutputFormat");
			this.textBox7.Style = "text-align: left";
			this.textBox7.Text = "AssessmentOutcome";
			this.textBox7.Top = 0.062F;
			this.textBox7.Width = 0.8750003F;
			// 
			// textBox8
			// 
			this.textBox8.DataField = "FinalOutcome";
			this.textBox8.Height = 0.2F;
			this.textBox8.Left = 7.312F;
			this.textBox8.Name = "textBox8";
			this.textBox8.OutputFormat = resources.GetString("textBox8.OutputFormat");
			this.textBox8.Style = "text-align: left";
			this.textBox8.Text = "FinalOutcome";
			this.textBox8.Top = 0.062F;
			this.textBox8.Width = 0.813F;
			// 
			// LastUpdateDate
			// 
			this.LastUpdateDate.DataField = "LastUpdateDate";
			this.LastUpdateDate.Height = 0.2F;
			this.LastUpdateDate.Left = 8.125F;
			this.LastUpdateDate.Name = "LastUpdateDate";
			this.LastUpdateDate.OutputFormat = resources.GetString("LastUpdateDate.OutputFormat");
			this.LastUpdateDate.Style = "text-align: left";
			this.LastUpdateDate.Text = "LastUpdateDate";
			this.LastUpdateDate.Top = 0.062F;
			this.LastUpdateDate.Width = 0.813F;
			// 
			// textBox10
			// 
			this.textBox10.DataField = "RecoveryReason";
			this.textBox10.Height = 0.2F;
			this.textBox10.Left = 9F;
			this.textBox10.Name = "textBox10";
			this.textBox10.OutputFormat = resources.GetString("textBox10.OutputFormat");
			this.textBox10.Style = "text-align: left";
			this.textBox10.Text = "RecoveryReason";
			this.textBox10.Top = 0.062F;
			this.textBox10.Width = 0.813F;
			// 
			// textBox11
			// 
			this.textBox11.DataField = "ReviewStatus";
			this.textBox11.Height = 0.2F;
			this.textBox11.Left = 9.875F;
			this.textBox11.Name = "textBox11";
			this.textBox11.OutputFormat = resources.GetString("textBox11.OutputFormat");
			this.textBox11.Style = "text-align: left";
			this.textBox11.Text = "ReviewStatus";
			this.textBox11.Top = 0.062F;
			this.textBox11.Width = 0.813F;
			// 
			// UploadDate
			// 
			this.UploadDate.DataField = "UploadDate";
			this.UploadDate.Height = 0.2F;
			this.UploadDate.Left = 10.75F;
			this.UploadDate.Name = "UploadDate";
			this.UploadDate.OutputFormat = resources.GetString("UploadDate.OutputFormat");
			this.UploadDate.Style = "text-align: left";
			this.UploadDate.Text = "UploadDate";
			this.UploadDate.Top = 0.062F;
			this.UploadDate.Width = 0.813F;
			// 
			// OrgCode
			// 
			this.OrgCode.DataField = "OrgCode";
			this.OrgCode.Height = 0.25F;
			this.OrgCode.Left = 5.375F;
			this.OrgCode.Name = "OrgCode";
			this.OrgCode.Text = "OrgCode";
			this.OrgCode.Top = 0.062F;
			this.OrgCode.Visible = false;
			this.OrgCode.Width = 0.5619999F;
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
			this.reportInfo1.Left = 8.375F;
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
			this.line2.Width = 11.538F;
			this.line2.X1 = 0.062F;
			this.line2.X2 = 11.6F;
			this.line2.Y1 = 0F;
			this.line2.Y2 = 0F;
			// 
			// groupHeader1
			// 
			this.groupHeader1.Controls.AddRange(new DataDynamics.ActiveReports.ARControl[] {
            this.OrgCode,
            this.label2,
            this.OrgName,
            this.ESACode,
            this.label4,
            this.ESAName,
            this.SiteCode,
            this.label16,
            this.SiteName,
            this.lblOrg,
            this.lblESA,
            this.lblSite,
            this.line3});
			this.groupHeader1.DataField = "SiteCode";
			this.groupHeader1.Height = 1.020833F;
			this.groupHeader1.Name = "groupHeader1";
			// 
			// OrgName
			// 
			this.OrgName.DataField = "OrgName";
			this.OrgName.Height = 0.25F;
			this.OrgName.Left = 6.125F;
			this.OrgName.Name = "OrgName";
			this.OrgName.Text = "OrgName";
			this.OrgName.Top = 0.062F;
			this.OrgName.Visible = false;
			this.OrgName.Width = 4.188001F;
			// 
			// ESACode
			// 
			this.ESACode.DataField = "ESACode";
			this.ESACode.Height = 0.25F;
			this.ESACode.Left = 5.375F;
			this.ESACode.Name = "ESACode";
			this.ESACode.Text = "ESACode";
			this.ESACode.Top = 0.3745F;
			this.ESACode.Visible = false;
			this.ESACode.Width = 0.5619999F;
			// 
			// label4
			// 
			this.label4.Height = 0.25F;
			this.label4.HyperLink = null;
			this.label4.Left = 0.062F;
			this.label4.Name = "label4";
			this.label4.Style = "font-weight: bold; vertical-align: top; ddo-char-set: 1";
			this.label4.Text = "ESA Code:";
			this.label4.Top = 0.3745F;
			this.label4.Width = 0.7500001F;
			// 
			// ESAName
			// 
			this.ESAName.DataField = "ESAName";
			this.ESAName.Height = 0.25F;
			this.ESAName.Left = 6.125F;
			this.ESAName.Name = "ESAName";
			this.ESAName.Text = "ESAName";
			this.ESAName.Top = 0.3745F;
			this.ESAName.Visible = false;
			this.ESAName.Width = 4.188001F;
			// 
			// SiteCode
			// 
			this.SiteCode.DataField = "SiteCode";
			this.SiteCode.Height = 0.25F;
			this.SiteCode.Left = 5.375F;
			this.SiteCode.Name = "SiteCode";
			this.SiteCode.Text = "SiteCode";
			this.SiteCode.Top = 0.687F;
			this.SiteCode.Visible = false;
			this.SiteCode.Width = 0.5619999F;
			// 
			// label16
			// 
			this.label16.Height = 0.25F;
			this.label16.HyperLink = null;
			this.label16.Left = 0.062F;
			this.label16.Name = "label16";
			this.label16.Style = "font-weight: bold; vertical-align: top; ddo-char-set: 1";
			this.label16.Text = "Site Code:";
			this.label16.Top = 0.687F;
			this.label16.Width = 0.7500001F;
			// 
			// SiteName
			// 
			this.SiteName.DataField = "SiteName";
			this.SiteName.Height = 0.25F;
			this.SiteName.Left = 6.125F;
			this.SiteName.Name = "SiteName";
			this.SiteName.Text = "SiteName";
			this.SiteName.Top = 0.687F;
			this.SiteName.Visible = false;
			this.SiteName.Width = 4.188001F;
			// 
			// lblOrg
			// 
			this.lblOrg.Height = 0.25F;
			this.lblOrg.HyperLink = null;
			this.lblOrg.Left = 0.812F;
			this.lblOrg.Name = "lblOrg";
			this.lblOrg.Style = "font-weight: bold; ddo-char-set: 1";
			this.lblOrg.Text = "label3";
			this.lblOrg.Top = 0.062F;
			this.lblOrg.Width = 4.187F;
			// 
			// lblESA
			// 
			this.lblESA.Height = 0.25F;
			this.lblESA.HyperLink = null;
			this.lblESA.Left = 0.812F;
			this.lblESA.Name = "lblESA";
			this.lblESA.Style = "font-weight: bold; ddo-char-set: 1";
			this.lblESA.Text = "label3";
			this.lblESA.Top = 0.374F;
			this.lblESA.Width = 4.187F;
			// 
			// lblSite
			// 
			this.lblSite.Height = 0.25F;
			this.lblSite.HyperLink = null;
			this.lblSite.Left = 0.812F;
			this.lblSite.Name = "lblSite";
			this.lblSite.Style = "font-weight: bold; ddo-char-set: 1";
			this.lblSite.Text = "label3";
			this.lblSite.Top = 0.687F;
			this.lblSite.Width = 4.187F;
			// 
			// line3
			// 
			this.line3.Height = 0F;
			this.line3.Left = 0.06200027F;
			this.line3.LineWeight = 1F;
			this.line3.Name = "line3";
			this.line3.Top = 1F;
			this.line3.Width = 5F;
			this.line3.X1 = 5.062F;
			this.line3.X2 = 0.06200027F;
			this.line3.Y1 = 1F;
			this.line3.Y2 = 1F;
			// 
			// groupFooter1
			// 
			this.groupFooter1.Height = 0.1041667F;
			this.groupFooter1.Name = "groupFooter1";
			this.groupFooter1.Visible = false;
			// 
			// label18
			// 
			this.label18.Height = 0.25F;
			this.label18.HyperLink = null;
			this.label18.Left = 0.25F;
			this.label18.Name = "label18";
			this.label18.Style = "font-weight: bold; vertical-align: top; ddo-char-set: 1";
			this.label18.Text = "Project ID:";
			this.label18.Top = 0.062F;
			this.label18.Width = 0.75F;
			// 
			// groupHeader2
			// 
			this.groupHeader2.Controls.AddRange(new DataDynamics.ActiveReports.ARControl[] {
            this.label18,
            this.ProjectID,
            this.ProjectName,
            this.label20,
            this.label22,
            this.label27,
            this.label25,
            this.label28,
            this.label29,
            this.label30,
            this.label31,
            this.label32,
            this.label23,
            this.label5,
            this.label6,
            this.lblProject,
            this.line4,
            this.line5});
			this.groupHeader2.DataField = "ProjectID";
			this.groupHeader2.Height = 1.104167F;
			this.groupHeader2.Name = "groupHeader2";
			// 
			// ProjectID
			// 
			this.ProjectID.DataField = "ProjectID";
			this.ProjectID.Height = 0.25F;
			this.ProjectID.Left = 1.062F;
			this.ProjectID.Name = "ProjectID";
			this.ProjectID.Text = "ProjectID";
			this.ProjectID.Top = 0.312F;
			this.ProjectID.Visible = false;
			this.ProjectID.Width = 0.5619999F;
			// 
			// ProjectName
			// 
			this.ProjectName.DataField = "ProjectName";
			this.ProjectName.Height = 0.25F;
			this.ProjectName.Left = 1.812F;
			this.ProjectName.Name = "ProjectName";
			this.ProjectName.Text = "ProjectName";
			this.ProjectName.Top = 0.312F;
			this.ProjectName.Visible = false;
			this.ProjectName.Width = 4.188001F;
			// 
			// label20
			// 
			this.label20.Height = 0.5F;
			this.label20.HyperLink = null;
			this.label20.Left = 0.251F;
			this.label20.Name = "label20";
			this.label20.Style = "font-weight: bold; text-align: left; vertical-align: bottom; ddo-char-set: 1";
			this.label20.Text = "Job Seeker ";
			this.label20.Top = 0.5F;
			this.label20.Width = 1F;
			// 
			// label22
			// 
			this.label22.Height = 0.5F;
			this.label22.HyperLink = null;
			this.label22.Left = 2.562F;
			this.label22.Name = "label22";
			this.label22.Style = "font-weight: bold; text-align: left; vertical-align: bottom; ddo-char-set: 1";
			this.label22.Text = "Claim ID";
			this.label22.Top = 0.5F;
			this.label22.Width = 0.563F;
			// 
			// label27
			// 
			this.label27.Height = 0.5F;
			this.label27.HyperLink = null;
			this.label27.Left = 4.812F;
			this.label27.Name = "label27";
			this.label27.Style = "font-weight: bold; text-align: left; vertical-align: bottom; ddo-char-set: 1";
			this.label27.Text = "Contract Type";
			this.label27.Top = 0.5F;
			this.label27.Width = 0.813F;
			// 
			// label25
			// 
			this.label25.Height = 0.5F;
			this.label25.HyperLink = null;
			this.label25.Left = 5.687F;
			this.label25.Name = "label25";
			this.label25.Style = "font-weight: bold; text-align: left; vertical-align: bottom; ddo-char-set: 1";
			this.label25.Text = "Days Overdue";
			this.label25.Top = 0.5F;
			this.label25.Width = 0.6260004F;
			// 
			// label28
			// 
			this.label28.Height = 0.5F;
			this.label28.HyperLink = null;
			this.label28.Left = 6.375F;
			this.label28.Name = "label28";
			this.label28.Style = "font-weight: bold; text-align: left; vertical-align: bottom; ddo-char-set: 1";
			this.label28.Text = "Assessment Outcome";
			this.label28.Top = 0.5F;
			this.label28.Width = 0.8750003F;
			// 
			// label29
			// 
			this.label29.Height = 0.5F;
			this.label29.HyperLink = null;
			this.label29.Left = 7.312F;
			this.label29.Name = "label29";
			this.label29.Style = "font-weight: bold; text-align: left; vertical-align: bottom; ddo-char-set: 1";
			this.label29.Text = "Final Outcome";
			this.label29.Top = 0.5F;
			this.label29.Width = 0.813F;
			// 
			// label30
			// 
			this.label30.Height = 0.5F;
			this.label30.HyperLink = null;
			this.label30.Left = 8.125F;
			this.label30.Name = "label30";
			this.label30.Style = "font-weight: bold; text-align: left; vertical-align: bottom; ddo-char-set: 1";
			this.label30.Text = "Last Updated Date";
			this.label30.Top = 0.5F;
			this.label30.Width = 0.813F;
			// 
			// label31
			// 
			this.label31.Height = 0.5F;
			this.label31.HyperLink = null;
			this.label31.Left = 9F;
			this.label31.Name = "label31";
			this.label31.Style = "font-weight: bold; text-align: left; vertical-align: bottom; ddo-char-set: 1";
			this.label31.Text = "Recovery Reason";
			this.label31.Top = 0.5F;
			this.label31.Width = 0.813F;
			// 
			// label32
			// 
			this.label32.Height = 0.5F;
			this.label32.HyperLink = null;
			this.label32.Left = 9.875F;
			this.label32.Name = "label32";
			this.label32.Style = "font-weight: bold; text-align: left; vertical-align: bottom; ddo-char-set: 1";
			this.label32.Text = "Review Status";
			this.label32.Top = 0.5F;
			this.label32.Width = 0.813F;
			// 
			// label23
			// 
			this.label23.Height = 0.5F;
			this.label23.HyperLink = null;
			this.label23.Left = 10.75F;
			this.label23.Name = "label23";
			this.label23.Style = "font-weight: bold; text-align: left; vertical-align: bottom; ddo-char-set: 1";
			this.label23.Text = "Upload Date";
			this.label23.Top = 0.5F;
			this.label23.Width = 0.813F;
			// 
			// label5
			// 
			this.label5.Height = 0.5F;
			this.label5.HyperLink = null;
			this.label5.Left = 3.187F;
			this.label5.Name = "label5";
			this.label5.Style = "font-weight: bold; text-align: left; vertical-align: bottom; ddo-char-set: 1";
			this.label5.Text = "Claim Amount ($)";
			this.label5.Top = 0.5F;
			this.label5.Width = 0.6880002F;
			// 
			// label6
			// 
			this.label6.Height = 0.5F;
			this.label6.HyperLink = null;
			this.label6.Left = 3.937F;
			this.label6.Name = "label6";
			this.label6.Style = "font-weight: bold; text-align: left; vertical-align: bottom; ddo-char-set: 1";
			this.label6.Text = "Claim Creation Date";
			this.label6.Top = 0.5F;
			this.label6.Width = 0.8129997F;
			// 
			// lblProject
			// 
			this.lblProject.Height = 0.25F;
			this.lblProject.HyperLink = null;
			this.lblProject.Left = 1F;
			this.lblProject.Name = "lblProject";
			this.lblProject.Style = "font-weight: bold; ddo-char-set: 1";
			this.lblProject.Text = "label3";
			this.lblProject.Top = 0.062F;
			this.lblProject.Width = 9.25F;
			// 
			// line4
			// 
			this.line4.Height = 0F;
			this.line4.Left = 0.25F;
			this.line4.LineWeight = 1F;
			this.line4.Name = "line4";
			this.line4.Top = 0.375F;
			this.line4.Width = 11.35F;
			this.line4.X1 = 0.25F;
			this.line4.X2 = 11.6F;
			this.line4.Y1 = 0.375F;
			this.line4.Y2 = 0.375F;
			// 
			// line5
			// 
			this.line5.Height = 0F;
			this.line5.Left = 0.25F;
			this.line5.LineWeight = 1F;
			this.line5.Name = "line5";
			this.line5.Top = 1.062F;
			this.line5.Width = 11.35F;
			this.line5.X1 = 0.25F;
			this.line5.X2 = 11.6F;
			this.line5.Y1 = 1.062F;
			this.line5.Y2 = 1.062F;
			// 
			// groupFooter2
			// 
			this.groupFooter2.Height = 0.08333333F;
			this.groupFooter2.Name = "groupFooter2";
			this.groupFooter2.Visible = false;
			// 
			// SiteVisitReport
			// 
			this.MasterReport = false;
			xmlDataSource1.FileURL = "C:\\Userdata\\Source\\ESC\\Release\\WebSites\\ProgramAssuranceTool\\ProgramAssuranceTool" +
    "\\Content\\Xml\\ComplianceRiskIndicatorReport.xml";
			xmlDataSource1.RecordsetPattern = "ArrayOfComplianceRiskIndicator/ComplianceRiskIndicator";
			this.DataSource = xmlDataSource1;
			this.PageSettings.PaperHeight = 11F;
			this.PageSettings.PaperWidth = 8.5F;
			this.PrintWidth = 11.69F;
			this.Script = resources.GetString("$this.Script");
			this.Sections.Add(this.pageHeader);
			this.Sections.Add(this.groupHeader1);
			this.Sections.Add(this.groupHeader2);
			this.Sections.Add(this.detail);
			this.Sections.Add(this.groupFooter2);
			this.Sections.Add(this.groupFooter1);
			this.Sections.Add(this.pageFooter);
			this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" +
            "l; font-size: 10pt; color: Black; ddo-char-set: 186", "Normal"));
			this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"));
			this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" +
            "lic", "Heading2", "Normal"));
			this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"));
			this.ReportStart += new System.EventHandler(this.SiteVisitReport_ReportStart);
			((System.ComponentModel.ISupportInitialize)(this.label1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.PrintedBy)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ClaimTypeDescription)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.JobSeekerName)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ClaimID)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.JobSeekerID)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.textBox13)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ClaimAmount)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ClaimCreationDate)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.textBox3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.DaysOverdue)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.textBox7)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.textBox8)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.LastUpdateDate)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.textBox10)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.textBox11)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.UploadDate)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.OrgCode)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.OrgName)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ESACode)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ESAName)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.SiteCode)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label16)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.SiteName)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.lblOrg)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.lblESA)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.lblSite)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label18)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ProjectID)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ProjectName)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label20)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label22)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label27)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label25)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label28)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label29)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label30)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label31)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label32)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label23)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label5)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label6)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.lblProject)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private DataDynamics.ActiveReports.Label label1;
        private DataDynamics.ActiveReports.ReportInfo reportInfo1;
        private DataDynamics.ActiveReports.Label label2;
        private DataDynamics.ActiveReports.Line line1;
        private DataDynamics.ActiveReports.TextBox OrgCode;
        private DataDynamics.ActiveReports.Line line2;
        private DataDynamics.ActiveReports.TextBox PrintedBy;
        private DataDynamics.ActiveReports.GroupHeader groupHeader1;
        private DataDynamics.ActiveReports.TextBox OrgName;
        private DataDynamics.ActiveReports.TextBox ESACode;
        private DataDynamics.ActiveReports.Label label4;
        private DataDynamics.ActiveReports.TextBox ESAName;
        private DataDynamics.ActiveReports.TextBox SiteCode;
        private DataDynamics.ActiveReports.Label label16;
        private DataDynamics.ActiveReports.TextBox SiteName;
        private DataDynamics.ActiveReports.GroupFooter groupFooter1;
        private DataDynamics.ActiveReports.Label label18;
        private DataDynamics.ActiveReports.GroupHeader groupHeader2;
        private DataDynamics.ActiveReports.TextBox ProjectID;
        private DataDynamics.ActiveReports.TextBox ProjectName;
        private DataDynamics.ActiveReports.GroupFooter groupFooter2;
        private DataDynamics.ActiveReports.Label label20;
        private DataDynamics.ActiveReports.TextBox ClaimTypeDescription;
        private DataDynamics.ActiveReports.TextBox JobSeekerName;
        private DataDynamics.ActiveReports.TextBox ClaimID;
        private DataDynamics.ActiveReports.TextBox JobSeekerID;
        private DataDynamics.ActiveReports.TextBox textBox13;
        private DataDynamics.ActiveReports.TextBox ClaimAmount;
        private DataDynamics.ActiveReports.TextBox ClaimCreationDate;
        private DataDynamics.ActiveReports.TextBox textBox3;
        private DataDynamics.ActiveReports.Label label22;
        private DataDynamics.ActiveReports.Label label27;
        private DataDynamics.ActiveReports.Label label25;
        private DataDynamics.ActiveReports.Label label28;
        private DataDynamics.ActiveReports.Label label29;
        private DataDynamics.ActiveReports.Label label30;
        private DataDynamics.ActiveReports.Label label31;
        private DataDynamics.ActiveReports.Label label32;
        private DataDynamics.ActiveReports.Label label23;
        private DataDynamics.ActiveReports.TextBox DaysOverdue;
        private DataDynamics.ActiveReports.TextBox textBox7;
        private DataDynamics.ActiveReports.TextBox textBox8;
        private DataDynamics.ActiveReports.TextBox LastUpdateDate;
        private DataDynamics.ActiveReports.TextBox textBox10;
        private DataDynamics.ActiveReports.TextBox textBox11;
        private DataDynamics.ActiveReports.TextBox UploadDate;
        private DataDynamics.ActiveReports.Label label5;
        private DataDynamics.ActiveReports.Label label6;
        private DataDynamics.ActiveReports.Label lblOrg;
        private DataDynamics.ActiveReports.Label lblESA;
        private DataDynamics.ActiveReports.Label lblSite;
        private DataDynamics.ActiveReports.Label lblProject;
        private DataDynamics.ActiveReports.Line line3;
        private DataDynamics.ActiveReports.Line line4;
		  private DataDynamics.ActiveReports.Line line5;
    }
}
