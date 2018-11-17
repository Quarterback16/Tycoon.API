namespace ProgramAssuranceTool.Reports
{
    /// <summary>
    /// Summary description for DashboardReport.
    /// </summary>
    partial class DashboardReport
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DashboardReport));
			DataDynamics.ActiveReports.Chart.ChartArea chartArea1 = new DataDynamics.ActiveReports.Chart.ChartArea();
			DataDynamics.ActiveReports.Chart.Axis axis1 = new DataDynamics.ActiveReports.Chart.Axis();
			DataDynamics.ActiveReports.Chart.Axis axis2 = new DataDynamics.ActiveReports.Chart.Axis();
			DataDynamics.ActiveReports.Chart.Axis axis3 = new DataDynamics.ActiveReports.Chart.Axis();
			DataDynamics.ActiveReports.Chart.Axis axis4 = new DataDynamics.ActiveReports.Chart.Axis();
			DataDynamics.ActiveReports.Chart.Axis axis5 = new DataDynamics.ActiveReports.Chart.Axis();
			DataDynamics.ActiveReports.DataSources.XMLDataSource xmlDataSource1 = new DataDynamics.ActiveReports.DataSources.XMLDataSource();
			DataDynamics.ActiveReports.Chart.Legend legend1 = new DataDynamics.ActiveReports.Chart.Legend();
			DataDynamics.ActiveReports.Chart.Title title1 = new DataDynamics.ActiveReports.Chart.Title();
			DataDynamics.ActiveReports.Chart.Title title2 = new DataDynamics.ActiveReports.Chart.Title();
			DataDynamics.ActiveReports.Chart.Series series1 = new DataDynamics.ActiveReports.Chart.Series();
			DataDynamics.ActiveReports.Chart.Series series2 = new DataDynamics.ActiveReports.Chart.Series();
			DataDynamics.ActiveReports.Chart.Series series3 = new DataDynamics.ActiveReports.Chart.Series();
			DataDynamics.ActiveReports.Chart.Title title3 = new DataDynamics.ActiveReports.Chart.Title();
			DataDynamics.ActiveReports.Chart.Title title4 = new DataDynamics.ActiveReports.Chart.Title();
			DataDynamics.ActiveReports.DataSources.XMLDataSource xmlDataSource2 = new DataDynamics.ActiveReports.DataSources.XMLDataSource();
			this.pageHeader = new DataDynamics.ActiveReports.PageHeader();
			this.label1 = new DataDynamics.ActiveReports.Label();
			this.PrintedBy = new DataDynamics.ActiveReports.TextBox();
			this.label6 = new DataDynamics.ActiveReports.Label();
			this.label14 = new DataDynamics.ActiveReports.Label();
			this.detail = new DataDynamics.ActiveReports.Detail();
			this.OutcomeDescription = new DataDynamics.ActiveReports.TextBox();
			this.ReviewCount = new DataDynamics.ActiveReports.TextBox();
			this.pageFooter = new DataDynamics.ActiveReports.PageFooter();
			this.reportInfo1 = new DataDynamics.ActiveReports.ReportInfo();
			this.line2 = new DataDynamics.ActiveReports.Line();
			this.groupHeader1 = new DataDynamics.ActiveReports.GroupHeader();
			this.chart1 = new DataDynamics.ActiveReports.ChartControl();
			this.line3 = new DataDynamics.ActiveReports.Line();
			this.groupFooter1 = new DataDynamics.ActiveReports.GroupFooter();
			((System.ComponentModel.ISupportInitialize)(this.label1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.PrintedBy)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label6)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label14)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.OutcomeDescription)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ReviewCount)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			// 
			// pageHeader
			// 
			this.pageHeader.Controls.AddRange(new DataDynamics.ActiveReports.ARControl[] {
            this.label1,
            this.PrintedBy});
			this.pageHeader.Height = 0.7911667F;
			this.pageHeader.Name = "pageHeader";
			// 
			// label1
			// 
			this.label1.Height = 0.312F;
			this.label1.HyperLink = null;
			this.label1.Left = 1.375F;
			this.label1.Name = "label1";
			this.label1.Style = "font-size: 18pt; font-weight: bold; text-align: center; ddo-char-set: 1";
			this.label1.Text = "Dashboard Report\r\n";
			this.label1.Top = 0F;
			this.label1.Width = 4.375F;
			// 
			// PrintedBy
			// 
			this.PrintedBy.Height = 0.2F;
			this.PrintedBy.Left = 4.062F;
			this.PrintedBy.Name = "PrintedBy";
			this.PrintedBy.OutputFormat = resources.GetString("PrintedBy.OutputFormat");
			this.PrintedBy.Style = "text-align: right";
			this.PrintedBy.Text = "Printed By: XXXXXX";
			this.PrintedBy.Top = 0.375F;
			this.PrintedBy.Width = 3F;
			// 
			// label6
			// 
			this.label6.Height = 0.2500004F;
			this.label6.HyperLink = null;
			this.label6.Left = 1.25F;
			this.label6.Name = "label6";
			this.label6.RightToLeft = true;
			this.label6.Style = "font-weight: bold; text-align: right; vertical-align: bottom";
			this.label6.Text = "Outcome Description";
			this.label6.Top = 4.437F;
			this.label6.Width = 3F;
			// 
			// label14
			// 
			this.label14.Height = 0.2500004F;
			this.label14.HyperLink = null;
			this.label14.Left = 4.313F;
			this.label14.Name = "label14";
			this.label14.Style = "font-weight: bold; text-align: right; vertical-align: bottom";
			this.label14.Text = "Number of Reviews";
			this.label14.Top = 4.437F;
			this.label14.Width = 1.437F;
			// 
			// detail
			// 
			this.detail.ColumnSpacing = 0F;
			this.detail.Controls.AddRange(new DataDynamics.ActiveReports.ARControl[] {
            this.OutcomeDescription,
            this.ReviewCount});
			this.detail.Height = 0.3375013F;
			this.detail.Name = "detail";
			this.detail.Format += new System.EventHandler(this.detail_Format);
			// 
			// OutcomeDescription
			// 
			this.OutcomeDescription.DataField = "OutcomeDescription";
			this.OutcomeDescription.Height = 0.2F;
			this.OutcomeDescription.Left = 1.25F;
			this.OutcomeDescription.Name = "OutcomeDescription";
			this.OutcomeDescription.OutputFormat = resources.GetString("OutcomeDescription.OutputFormat");
			this.OutcomeDescription.RightToLeft = true;
			this.OutcomeDescription.Style = "text-align: right";
			this.OutcomeDescription.Text = "OutcomeDescription";
			this.OutcomeDescription.Top = 0.062F;
			this.OutcomeDescription.Width = 3.001F;
			// 
			// ReviewCount
			// 
			this.ReviewCount.DataField = "ReviewCount";
			this.ReviewCount.Height = 0.2F;
			this.ReviewCount.Left = 4.313F;
			this.ReviewCount.Name = "ReviewCount";
			this.ReviewCount.OutputFormat = resources.GetString("ReviewCount.OutputFormat");
			this.ReviewCount.Style = "text-align: right";
			this.ReviewCount.Text = "ReviewCount";
			this.ReviewCount.Top = 0.062F;
			this.ReviewCount.Width = 1.437F;
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
			this.reportInfo1.Left = 3.937F;
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
			this.line2.Width = 7.038F;
			this.line2.X1 = 0.062F;
			this.line2.X2 = 7.1F;
			this.line2.Y1 = 0F;
			this.line2.Y2 = 0F;
			// 
			// groupHeader1
			// 
			this.groupHeader1.Controls.AddRange(new DataDynamics.ActiveReports.ARControl[] {
            this.chart1,
            this.label6,
            this.label14,
            this.line3});
			this.groupHeader1.Height = 4.823417F;
			this.groupHeader1.Name = "groupHeader1";
			// 
			// chart1
			// 
			this.chart1.AutoRefresh = true;
			this.chart1.Backdrop = new DataDynamics.ActiveReports.Chart.BackdropItem(DataDynamics.ActiveReports.Chart.Graphics.BackdropStyle.Transparent, System.Drawing.Color.White, System.Drawing.Color.SteelBlue, DataDynamics.ActiveReports.Chart.Graphics.GradientType.Vertical, System.Drawing.Drawing2D.HatchStyle.DottedGrid, null, DataDynamics.ActiveReports.Chart.Graphics.PicturePutStyle.Stretched);
			axis1.AxisType = DataDynamics.ActiveReports.Chart.AxisType.Categorical;
			axis1.LabelFont = new DataDynamics.ActiveReports.Chart.FontInfo(System.Drawing.Color.Black, new System.Drawing.Font("Microsoft Sans Serif", 8F));
			axis1.Labels.AddRange(new string[] {
            ""});
			axis1.MajorTick = new DataDynamics.ActiveReports.Chart.Tick(new DataDynamics.ActiveReports.Chart.Graphics.Line(), new DataDynamics.ActiveReports.Chart.Graphics.Line(System.Drawing.Color.Black, DataDynamics.ActiveReports.Chart.Graphics.LineStyle.Dot), 1D, 5F, true);
			axis1.MinorTick = new DataDynamics.ActiveReports.Chart.Tick(new DataDynamics.ActiveReports.Chart.Graphics.Line(System.Drawing.Color.Transparent, 0, DataDynamics.ActiveReports.Chart.Graphics.LineStyle.None), new DataDynamics.ActiveReports.Chart.Graphics.Line(System.Drawing.Color.Transparent, 0, DataDynamics.ActiveReports.Chart.Graphics.LineStyle.None), 0D, 0F, false);
			axis1.Title = "Axis X";
			axis1.TitleFont = new DataDynamics.ActiveReports.Chart.FontInfo(System.Drawing.Color.Black, new System.Drawing.Font("Microsoft Sans Serif", 8F));
			axis2.LabelFont = new DataDynamics.ActiveReports.Chart.FontInfo(System.Drawing.Color.Black, new System.Drawing.Font("Microsoft Sans Serif", 8F));
			axis2.LabelsGap = 0;
			axis2.LabelsVisible = false;
			axis2.Line = new DataDynamics.ActiveReports.Chart.Graphics.Line(System.Drawing.Color.Transparent, 0, DataDynamics.ActiveReports.Chart.Graphics.LineStyle.None);
			axis2.MajorTick = new DataDynamics.ActiveReports.Chart.Tick(new DataDynamics.ActiveReports.Chart.Graphics.Line(System.Drawing.Color.Transparent, 0, DataDynamics.ActiveReports.Chart.Graphics.LineStyle.None), new DataDynamics.ActiveReports.Chart.Graphics.Line(System.Drawing.Color.Transparent, 0, DataDynamics.ActiveReports.Chart.Graphics.LineStyle.None), 0D, 0F, false);
			axis2.MinorTick = new DataDynamics.ActiveReports.Chart.Tick(new DataDynamics.ActiveReports.Chart.Graphics.Line(System.Drawing.Color.Transparent, 0, DataDynamics.ActiveReports.Chart.Graphics.LineStyle.None), new DataDynamics.ActiveReports.Chart.Graphics.Line(System.Drawing.Color.Transparent, 0, DataDynamics.ActiveReports.Chart.Graphics.LineStyle.None), 0D, 0F, false);
			axis2.Position = 0D;
			axis2.TickOffset = 0D;
			axis2.TitleFont = new DataDynamics.ActiveReports.Chart.FontInfo(System.Drawing.Color.Black, new System.Drawing.Font("Microsoft Sans Serif", 8F));
			axis2.Visible = false;
			axis3.LabelFont = new DataDynamics.ActiveReports.Chart.FontInfo(System.Drawing.Color.Black, new System.Drawing.Font("Microsoft Sans Serif", 8F));
			axis3.MajorTick = new DataDynamics.ActiveReports.Chart.Tick(new DataDynamics.ActiveReports.Chart.Graphics.Line(), new DataDynamics.ActiveReports.Chart.Graphics.Line(System.Drawing.Color.Black, DataDynamics.ActiveReports.Chart.Graphics.LineStyle.Dot), 1D, 5F, true);
			axis3.MinorTick = new DataDynamics.ActiveReports.Chart.Tick(new DataDynamics.ActiveReports.Chart.Graphics.Line(System.Drawing.Color.Transparent, 0, DataDynamics.ActiveReports.Chart.Graphics.LineStyle.None), new DataDynamics.ActiveReports.Chart.Graphics.Line(System.Drawing.Color.Transparent, 0, DataDynamics.ActiveReports.Chart.Graphics.LineStyle.None), 0D, 0F, false);
			axis3.Position = 0D;
			axis3.Title = "Axis Y";
			axis3.TitleFont = new DataDynamics.ActiveReports.Chart.FontInfo(System.Drawing.Color.Black, new System.Drawing.Font("Microsoft Sans Serif", 8F), -90F);
			axis4.LabelFont = new DataDynamics.ActiveReports.Chart.FontInfo(System.Drawing.Color.Black, new System.Drawing.Font("Microsoft Sans Serif", 8F));
			axis4.LabelsVisible = false;
			axis4.Line = new DataDynamics.ActiveReports.Chart.Graphics.Line(System.Drawing.Color.Transparent, 0, DataDynamics.ActiveReports.Chart.Graphics.LineStyle.None);
			axis4.MajorTick = new DataDynamics.ActiveReports.Chart.Tick(new DataDynamics.ActiveReports.Chart.Graphics.Line(System.Drawing.Color.Transparent, 0, DataDynamics.ActiveReports.Chart.Graphics.LineStyle.None), new DataDynamics.ActiveReports.Chart.Graphics.Line(System.Drawing.Color.Transparent, 0, DataDynamics.ActiveReports.Chart.Graphics.LineStyle.None), 0D, 0F, false);
			axis4.MinorTick = new DataDynamics.ActiveReports.Chart.Tick(new DataDynamics.ActiveReports.Chart.Graphics.Line(System.Drawing.Color.Transparent, 0, DataDynamics.ActiveReports.Chart.Graphics.LineStyle.None), new DataDynamics.ActiveReports.Chart.Graphics.Line(System.Drawing.Color.Transparent, 0, DataDynamics.ActiveReports.Chart.Graphics.LineStyle.None), 0D, 0F, false);
			axis4.TitleFont = new DataDynamics.ActiveReports.Chart.FontInfo(System.Drawing.Color.Black, new System.Drawing.Font("Microsoft Sans Serif", 8F));
			axis4.Visible = false;
			axis5.LabelFont = new DataDynamics.ActiveReports.Chart.FontInfo(System.Drawing.Color.Black, new System.Drawing.Font("Microsoft Sans Serif", 8F));
			axis5.LabelsGap = 0;
			axis5.LabelsVisible = false;
			axis5.Line = new DataDynamics.ActiveReports.Chart.Graphics.Line(System.Drawing.Color.Transparent, 0, DataDynamics.ActiveReports.Chart.Graphics.LineStyle.None);
			axis5.MajorTick = new DataDynamics.ActiveReports.Chart.Tick(new DataDynamics.ActiveReports.Chart.Graphics.Line(System.Drawing.Color.Transparent, 0, DataDynamics.ActiveReports.Chart.Graphics.LineStyle.None), new DataDynamics.ActiveReports.Chart.Graphics.Line(System.Drawing.Color.Transparent, 0, DataDynamics.ActiveReports.Chart.Graphics.LineStyle.None), 0D, 0F, false);
			axis5.MinorTick = new DataDynamics.ActiveReports.Chart.Tick(new DataDynamics.ActiveReports.Chart.Graphics.Line(System.Drawing.Color.Transparent, 0, DataDynamics.ActiveReports.Chart.Graphics.LineStyle.None), new DataDynamics.ActiveReports.Chart.Graphics.Line(System.Drawing.Color.Transparent, 0, DataDynamics.ActiveReports.Chart.Graphics.LineStyle.None), 0D, 0F, false);
			axis5.Position = 0D;
			axis5.TickOffset = 0D;
			axis5.TitleFont = new DataDynamics.ActiveReports.Chart.FontInfo(System.Drawing.Color.Black, new System.Drawing.Font("Microsoft Sans Serif", 8F));
			axis5.Visible = false;
			chartArea1.Axes.AddRange(new DataDynamics.ActiveReports.Chart.AxisBase[] {
            axis1,
            axis2,
            axis3,
            axis4,
            axis5});
			chartArea1.Backdrop = new DataDynamics.ActiveReports.Chart.BackdropItem(DataDynamics.ActiveReports.Chart.Graphics.BackdropStyle.Transparent, System.Drawing.Color.White, System.Drawing.Color.White, DataDynamics.ActiveReports.Chart.Graphics.GradientType.Vertical, System.Drawing.Drawing2D.HatchStyle.DottedGrid, null, DataDynamics.ActiveReports.Chart.Graphics.PicturePutStyle.Stretched);
			chartArea1.Border = new DataDynamics.ActiveReports.Chart.Border(new DataDynamics.ActiveReports.Chart.Graphics.Line(System.Drawing.Color.Transparent, 0, DataDynamics.ActiveReports.Chart.Graphics.LineStyle.None), 0, System.Drawing.Color.Black);
			chartArea1.Light = new DataDynamics.ActiveReports.Chart.Light(new DataDynamics.ActiveReports.Chart.Graphics.Point3d(10F, 40F, 20F), DataDynamics.ActiveReports.Chart.LightType.InfiniteDirectional, 0.3F);
			chartArea1.Name = "defaultArea";
			chartArea1.SwapAxesDirection = true;
			this.chart1.ChartAreas.AddRange(new DataDynamics.ActiveReports.Chart.ChartArea[] {
            chartArea1});
			this.chart1.ChartBorder = new DataDynamics.ActiveReports.Chart.Border(new DataDynamics.ActiveReports.Chart.Graphics.Line(System.Drawing.Color.Transparent, 0, DataDynamics.ActiveReports.Chart.Graphics.LineStyle.None), 0, System.Drawing.Color.Black);
			xmlDataSource1.FileURL = "C:\\Userdata\\Source\\ESC\\Release\\WebSites\\ProgramAssuranceTool\\ProgramAssuranceTool" +
    "\\Content\\Xml\\DashboardReport.xml";
			xmlDataSource1.RecordsetPattern = "/ArrayOfDashboard/Dashboard";
			this.chart1.DataSource = xmlDataSource1;
			this.chart1.Height = 4.25F;
			this.chart1.Left = 0.062F;
			legend1.Alignment = DataDynamics.ActiveReports.Chart.Alignment.Right;
			legend1.Backdrop = new DataDynamics.ActiveReports.Chart.BackdropItem(System.Drawing.Color.White, ((byte)(128)));
			legend1.Border = new DataDynamics.ActiveReports.Chart.Border(new DataDynamics.ActiveReports.Chart.Graphics.Line(), 0, System.Drawing.Color.Black);
			legend1.DockArea = chartArea1;
			title1.Backdrop = new DataDynamics.ActiveReports.Chart.Graphics.Backdrop(DataDynamics.ActiveReports.Chart.Graphics.BackdropStyle.Transparent, System.Drawing.Color.White, System.Drawing.Color.White, DataDynamics.ActiveReports.Chart.Graphics.GradientType.Vertical, System.Drawing.Drawing2D.HatchStyle.DottedGrid, null, DataDynamics.ActiveReports.Chart.Graphics.PicturePutStyle.Stretched);
			title1.Border = new DataDynamics.ActiveReports.Chart.Border(new DataDynamics.ActiveReports.Chart.Graphics.Line(System.Drawing.Color.Transparent, 0, DataDynamics.ActiveReports.Chart.Graphics.LineStyle.None), 0, System.Drawing.Color.Black);
			title1.DockArea = null;
			title1.Font = new DataDynamics.ActiveReports.Chart.FontInfo(System.Drawing.Color.Black, new System.Drawing.Font("Microsoft Sans Serif", 8F));
			title1.Name = "";
			title1.Text = "";
			title1.Visible = false;
			legend1.Footer = title1;
			legend1.GridLayout = new DataDynamics.ActiveReports.Chart.GridLayout(0, 1);
			title2.Border = new DataDynamics.ActiveReports.Chart.Border(new DataDynamics.ActiveReports.Chart.Graphics.Line(System.Drawing.Color.White, 2), 0, System.Drawing.Color.Black);
			title2.DockArea = null;
			title2.Font = new DataDynamics.ActiveReports.Chart.FontInfo(System.Drawing.Color.Black, new System.Drawing.Font("Microsoft Sans Serif", 8F));
			title2.Name = "";
			title2.Text = "Legend";
			legend1.Header = title2;
			legend1.LabelsFont = new DataDynamics.ActiveReports.Chart.FontInfo(System.Drawing.Color.Black, new System.Drawing.Font("Microsoft Sans Serif", 8F));
			legend1.MarginX = 20;
			legend1.MarginY = 20;
			legend1.Name = "defaultLegend";
			this.chart1.Legends.AddRange(new DataDynamics.ActiveReports.Chart.Legend[] {
            legend1});
			this.chart1.Name = "chart1";
			series1.AxisX = axis1;
			series1.AxisY = axis3;
			series1.ChartArea = chartArea1;
			series1.ColorPalette = DataDynamics.ActiveReports.Chart.ColorPalette.Default;
			series1.Legend = legend1;
			series1.LegendText = "";
			series1.Name = "Series1";
			series1.Properties = new DataDynamics.ActiveReports.Chart.CustomProperties(new DataDynamics.ActiveReports.Chart.KeyValuePair[] {
            new DataDynamics.ActiveReports.Chart.KeyValuePair("Backdrop", new DataDynamics.ActiveReports.Chart.Graphics.Backdrop()),
            new DataDynamics.ActiveReports.Chart.KeyValuePair("BorderLine", new DataDynamics.ActiveReports.Chart.Graphics.Line(System.Drawing.Color.White)),
            new DataDynamics.ActiveReports.Chart.KeyValuePair("HoleSize", 0.3F),
            new DataDynamics.ActiveReports.Chart.KeyValuePair("Marker", new DataDynamics.ActiveReports.Chart.Marker(10, DataDynamics.ActiveReports.Chart.MarkerStyle.None, new DataDynamics.ActiveReports.Chart.Graphics.Backdrop(), new DataDynamics.ActiveReports.Chart.Graphics.Line(), new DataDynamics.ActiveReports.Chart.LabelInfo(new DataDynamics.ActiveReports.Chart.Graphics.Line(), new DataDynamics.ActiveReports.Chart.Graphics.Backdrop(), new DataDynamics.ActiveReports.Chart.FontInfo(), "{Pct:#.##}%", DataDynamics.ActiveReports.Chart.Alignment.Top))),
            new DataDynamics.ActiveReports.Chart.KeyValuePair("Radius", 1F),
            new DataDynamics.ActiveReports.Chart.KeyValuePair("BarType", DataDynamics.ActiveReports.Chart.BarType.Bar)});
			series1.Type = DataDynamics.ActiveReports.Chart.ChartType.Doughnut;
			series1.ValueMembersY = "xpath:ReviewCount";
			series1.ValueMemberX = "xpath:OutcomeDescription";
			series2.AxisX = axis1;
			series2.AxisY = axis3;
			series2.ChartArea = chartArea1;
			series2.ColorPalette = DataDynamics.ActiveReports.Chart.ColorPalette.Default;
			series2.Legend = legend1;
			series2.LegendText = "";
			series2.Name = "Series2";
			series2.Properties = new DataDynamics.ActiveReports.Chart.CustomProperties(new DataDynamics.ActiveReports.Chart.KeyValuePair[] {
            new DataDynamics.ActiveReports.Chart.KeyValuePair("HoleSize", 0.3F),
            new DataDynamics.ActiveReports.Chart.KeyValuePair("Radius", 1F),
            new DataDynamics.ActiveReports.Chart.KeyValuePair("BarType", DataDynamics.ActiveReports.Chart.BarType.Bar)});
			series2.Type = DataDynamics.ActiveReports.Chart.ChartType.Doughnut;
			series3.AxisX = axis1;
			series3.AxisY = axis3;
			series3.ChartArea = chartArea1;
			series3.ColorPalette = DataDynamics.ActiveReports.Chart.ColorPalette.Default;
			series3.Legend = legend1;
			series3.LegendText = "";
			series3.Name = "Series3";
			series3.Properties = new DataDynamics.ActiveReports.Chart.CustomProperties(new DataDynamics.ActiveReports.Chart.KeyValuePair[] {
            new DataDynamics.ActiveReports.Chart.KeyValuePair("HoleSize", 0.3F),
            new DataDynamics.ActiveReports.Chart.KeyValuePair("Radius", 1F),
            new DataDynamics.ActiveReports.Chart.KeyValuePair("BarType", DataDynamics.ActiveReports.Chart.BarType.Bar)});
			series3.Type = DataDynamics.ActiveReports.Chart.ChartType.Doughnut;
			this.chart1.Series.AddRange(new DataDynamics.ActiveReports.Chart.Series[] {
            series1,
            series2,
            series3});
			title3.Border = new DataDynamics.ActiveReports.Chart.Border(new DataDynamics.ActiveReports.Chart.Graphics.Line(System.Drawing.Color.Transparent, 0, DataDynamics.ActiveReports.Chart.Graphics.LineStyle.None), 0, System.Drawing.Color.Black);
			title3.DockArea = null;
			title3.Font = new DataDynamics.ActiveReports.Chart.FontInfo(System.Drawing.Color.Black, new System.Drawing.Font("Microsoft Sans Serif", 11F));
			title3.Name = "header";
			title3.Text = "Outcome and Number of Reviews";
			title4.Border = new DataDynamics.ActiveReports.Chart.Border(new DataDynamics.ActiveReports.Chart.Graphics.Line(System.Drawing.Color.Transparent, 0, DataDynamics.ActiveReports.Chart.Graphics.LineStyle.None), 0, System.Drawing.Color.Black);
			title4.DockArea = null;
			title4.Docking = DataDynamics.ActiveReports.Chart.DockType.Bottom;
			title4.Font = new DataDynamics.ActiveReports.Chart.FontInfo(System.Drawing.Color.Black, new System.Drawing.Font("Microsoft Sans Serif", 8F));
			title4.Name = "footer";
			title4.Text = "Chart Footer";
			title4.Visible = false;
			this.chart1.Titles.AddRange(new DataDynamics.ActiveReports.Chart.Title[] {
            title3,
            title4});
			this.chart1.Top = 0.05F;
			this.chart1.UIOptions = DataDynamics.ActiveReports.Chart.UIOptions.ForceHitTesting;
			this.chart1.Width = 7F;
			// 
			// line3
			// 
			this.line3.Height = 0F;
			this.line3.Left = 0.06200001F;
			this.line3.LineWeight = 1F;
			this.line3.Name = "line3";
			this.line3.Top = 4.75F;
			this.line3.Width = 7.038F;
			this.line3.X1 = 0.06200001F;
			this.line3.X2 = 7.1F;
			this.line3.Y1 = 4.75F;
			this.line3.Y2 = 4.75F;
			// 
			// groupFooter1
			// 
			this.groupFooter1.Height = 0.07291722F;
			this.groupFooter1.Name = "groupFooter1";
			this.groupFooter1.Visible = false;
			// 
			// DashboardReport
			// 
			this.MasterReport = false;
			xmlDataSource2.FileURL = "C:\\Userdata\\Source\\ESC\\Release\\WebSites\\ProgramAssuranceTool\\ProgramAssuranceTool" +
    "\\Content\\Xml\\DashboardReport.xml";
			xmlDataSource2.RecordsetPattern = "/ArrayOfDashboard/Dashboard";
			this.DataSource = xmlDataSource2;
			this.PageSettings.PaperHeight = 11F;
			this.PageSettings.PaperWidth = 8.5F;
			this.PrintWidth = 7.2F;
			this.Sections.Add(this.pageHeader);
			this.Sections.Add(this.groupHeader1);
			this.Sections.Add(this.detail);
			this.Sections.Add(this.groupFooter1);
			this.Sections.Add(this.pageFooter);
			this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" +
            "l; font-size: 10pt; color: Black; ddo-char-set: 186", "Normal"));
			this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"));
			this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" +
            "lic", "Heading2", "Normal"));
			this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"));
			this.ReportStart += new System.EventHandler(this.DashboardReport_ReportStart);
			((System.ComponentModel.ISupportInitialize)(this.label1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.PrintedBy)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label6)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label14)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.OutcomeDescription)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ReviewCount)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private DataDynamics.ActiveReports.Label label1;
        private DataDynamics.ActiveReports.ReportInfo reportInfo1;
        private DataDynamics.ActiveReports.Label label6;
        private DataDynamics.ActiveReports.TextBox OutcomeDescription;
        private DataDynamics.ActiveReports.Label label14;
        private DataDynamics.ActiveReports.TextBox ReviewCount;
        private DataDynamics.ActiveReports.Line line2;
        private DataDynamics.ActiveReports.TextBox PrintedBy;
        private DataDynamics.ActiveReports.GroupHeader groupHeader1;
        private DataDynamics.ActiveReports.GroupFooter groupFooter1;
        private DataDynamics.ActiveReports.ChartControl chart1;
        private DataDynamics.ActiveReports.Line line3;
    }
}
