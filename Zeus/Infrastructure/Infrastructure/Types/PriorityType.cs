namespace Employment.Web.Mvc.Infrastructure.Types
{
    /// <summary>
    /// Represents an enum which defines the priority type.
    /// </summary>
    public enum PriorityType
    {
        /// <summary>
        /// The priority is low.
        /// </summary>
        Low,

        /// <summary>
        /// The priority is normal.
        /// </summary>
        Normal,

        /// <summary>
        /// The priority is high.
        /// </summary>
        /// <remarks>
        /// This priority is intended for use by Infrastructure only.
        /// </remarks>
        High,

        /// <summary>
        /// The default priority.
        /// </summary>
        Default = Normal
    }
}
