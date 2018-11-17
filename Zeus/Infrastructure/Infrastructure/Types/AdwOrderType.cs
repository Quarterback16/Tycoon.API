namespace Employment.Web.Mvc.Infrastructure.Types
{
    /// <summary>
    /// Represents an enum which defines the adw order type.
    /// </summary>
    public enum AdwOrderType
    {
        /// <summary>
        /// Order by description text.
        /// </summary>
        ByDescription,

        /// <summary>
        /// Order by code.
        /// </summary>
        ByCode,

        /// <summary>
        /// Order by dominant code.
        /// </summary>
        ByDominantCode,

        /// <summary>
        /// Order by subordinate code.
        /// </summary>
        BySubordinateCode,

        /// <summary>
        /// Default order type is by description.
        /// </summary>
        Default = ByDescription
    }
}
