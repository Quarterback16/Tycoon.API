using System;

namespace Employment.Web.Mvc.Infrastructure.Models
{
    /// <summary>
    /// Chart options model for configuring how the chart data will be displayed.
    /// </summary>
    [Serializable]
    public class ChartOptionsModel
    {
        /// <summary>
        /// Format string for label.
        /// </summary>
        public const string LabelFormat = "{L}";

        /// <summary>
        /// Format string for value of X axis.
        /// </summary>
        public const string ValueXFormat = "{X}";

        /// <summary>
        /// Format string for value of Y axis.
        /// </summary>
        public const string ValueYFormat = "{Y}";

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartOptionsModel" /> class.
        /// </summary>
        public ChartOptionsModel()
        {
            AxisX = new ChartAxisModel();
            AxisY = new ChartAxisModel();
        }

        private string tooltipFirstFormat;

        /// <summary>
        /// Tooltip format string to be used for first item.
        /// </summary>
        public string TooltipFirstFormat { get { return tooltipFirstFormat ?? TooltipFormat; } set { tooltipFirstFormat = value; } }

        /// <summary>
        /// Tooltip format string to be used for middle items.
        /// </summary>
        public string TooltipFormat { get; set; }

        private string tooltipLastFormat;

        /// <summary>
        /// Tooltip format string to be used for last item.
        /// </summary>
        public string TooltipLastFormat { get { return tooltipLastFormat ?? TooltipFormat; } set { tooltipLastFormat = value; } }

        /// <summary>
        /// Expected last value for <see cref="TooltipLastFormat"/> to be used.
        /// </summary>
        public double? ExpectedLast { get; set; }

        /// <summary>
        /// Whether the chart can be zoomed.
        /// </summary>
        public bool Zoom { get { return AxisX.Zoom || AxisY.Zoom; } }

        /// <summary>
        /// Whether the chart can be panned.
        /// </summary>
        public bool Pan { get { return AxisX.Pan || AxisY.Pan; } }

        /// <summary>
        /// Whether to show data with points.
        /// </summary>
        public bool Points { get; set; }

        /// <summary>
        /// Whether to show data with lines.
        /// </summary>
        public bool Lines { get; set; }

        /// <summary>
        /// Whether to show data with bars.
        /// </summary>
        public bool Bars { get; set; }

        /// <summary>
        /// Whether to show data stacked.
        /// </summary>
        public bool Stacked { get; set; }

        /// <summary>
        /// Whether to allow toggling datasets on and off.
        /// </summary>
        public bool Toggle { get; set; }

        /// <summary>
        /// The settings for the X axis.
        /// </summary>
        public ChartAxisModel AxisX { get; set; }

        /// <summary>
        /// The settings for the Y axis.
        /// </summary>
        public ChartAxisModel AxisY { get; set; }
    }
}
