namespace Employment.Web.Mvc.Infrastructure.Types
{
    /// <summary>
    /// Represents an enum which defines the action to take based on a dependency.
    /// </summary>
    public enum ActionForDependencyType
    {
        /// <summary>
        /// None.
        /// </summary>
        None,

        /// <summary>
        /// Enabled.
        /// </summary>
        Enabled,

        /// <summary>
        /// Disabled.
        /// </summary>
        Disabled,

        /// <summary>
        /// Visible.
        /// </summary>
        Visible,

        /// <summary>
        /// Hidden.
        /// </summary>
        Hidden,

        /// <summary>
        /// Bindable.
        /// </summary>
        Bindable
    }
}
