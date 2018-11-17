using System;

namespace Employment.Web.Mvc.Infrastructure.Models
{
    /// <summary>
    /// Chart axis model.
    /// </summary>
    [Serializable]
    public class ChartAxisModel
    {
        /// <summary>
        /// Label for axis.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Whether axis values are categories (string values).
        /// </summary>
        public bool Categories { get; set; }

        /// <summary>
        /// Whether axis values are times.
        /// </summary>
        public bool Times { get; set; }

        /// <summary>
        /// The format string for displaying the time.
        /// </summary>
        public string TimeFormat { get; set; }

        /// <summary>
        /// Whether axis can be panned.
        /// </summary>
        public bool Pan { get; set; }

        /// <summary>
        /// Whether axis can be zoomed.
        /// </summary>
        public bool Zoom { get; set; }
    }
}
