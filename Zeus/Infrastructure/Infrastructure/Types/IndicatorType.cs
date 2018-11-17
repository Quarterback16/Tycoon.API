namespace Employment.Web.Mvc.Infrastructure.Types
{
    /// <summary>
    /// Represents an enum which defines the indicator type.
    /// </summary>
    public enum IndicatorType
    {
        /// <summary>
        /// No indicator.
        /// </summary>
        None,

        /// <summary>
        /// Indicate incomplete.
        /// </summary>
        Incomplete,

        /// <summary>
        /// Indicate complete.
        /// </summary>
        Complete,

        /// <summary>
        /// Default indicatation is no indicator.
        /// </summary>
        Default = None
    }
}
