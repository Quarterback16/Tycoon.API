namespace ProgramAssuranceTool.Reports
{
    /// <summary>
    /// Summary description for FindingSummarySubReport.
    /// </summary>
    partial class FindingSummarySubReport
    {
        private DataDynamics.ActiveReports.Detail detail;

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FindingSummarySubReport));
			this.detail = new DataDynamics.ActiveReports.Detail();
			this.Description = new DataDynamics.ActiveReports.TextBox();
			this.ReviewCount = new DataDynamics.ActiveReports.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.Description)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ReviewCount)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			// 
			// detail
			// 
			this.detail.ColumnSpacing = 0F;
			this.detail.Controls.AddRange(new DataDynamics.ActiveReports.ARControl[] {
            this.Description,
            this.ReviewCount});
			this.detail.Height = 0.2708333F;
			this.detail.Name = "detail";
			this.detail.Format += new System.EventHandler(this.detail_Format);
			// 
			// Description
			// 
			this.Description.DataField = "Description";
			this.Description.Height = 0.2F;
			this.Description.Left = 0F;
			this.Description.Name = "Description";
			this.Description.Text = "Description";
			this.Description.Top = 3.72529E-09F;
			this.Description.Width = 4.937F;
			// 
			// ReviewCount
			// 
			this.ReviewCount.DataField = "ReviewCount";
			this.ReviewCount.Height = 0.2F;
			this.ReviewCount.Left = 5F;
			this.ReviewCount.Name = "ReviewCount";
			this.ReviewCount.OutputFormat = resources.GetString("ReviewCount.OutputFormat");
			this.ReviewCount.Style = "text-align: right";
			this.ReviewCount.Text = "ReviewCount";
			this.ReviewCount.Top = 0F;
			this.ReviewCount.Width = 1F;
			// 
			// FindingSummarySubReport
			// 
			this.MasterReport = false;
			this.PageSettings.PaperHeight = 11F;
			this.PageSettings.PaperWidth = 8.5F;
			this.PrintWidth = 6.063F;
			this.Sections.Add(this.detail);
			this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" +
            "l; font-size: 10pt; color: Black", "Normal"));
			this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"));
			this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" +
            "lic", "Heading2", "Normal"));
			this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"));
			((System.ComponentModel.ISupportInitialize)(this.Description)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ReviewCount)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private DataDynamics.ActiveReports.TextBox Description;
        private DataDynamics.ActiveReports.TextBox ReviewCount;

    }
}
