namespace Employment.Web.Mvc.Infrastructure.Types
{
    /// <summary>
    /// Represents an enum which defines the selection type.
    /// </summary>
    public enum SelectionType
    {
        /// <summary>
        /// No items are selectable.
        /// </summary>
        None,

        /// <summary>
        /// A single item is selectable.
        /// </summary>
        Single,

        /// <summary>
        /// Multiple items are selectable.
        /// </summary>
        Multiple,

        /// <summary>
        /// The default selection type is none.
        /// </summary>
        Default = None
    }
}
