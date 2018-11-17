namespace Employment.Web.Mvc.Infrastructure.Types
{
    /// <summary>
    /// Represents an enum which defines the row type for Group.
    /// </summary>
    public enum GroupRowType
    {
        /// <summary>
        /// Group will occupy entire row.
        /// </summary>
        Full = 12,

        /// <summary>
        /// Two Groups will sit side-by-side.
        /// </summary>
        Half = 6,

        ///// <summary>
        ///// 
        ///// </summary>
        //Third = 4,

        /// <summary>
        /// One Third.
        /// </summary>
        OneThird = 4,

        /// <summary>
        /// Two Third.
        /// </summary>
        TwoThird = 8,


        /// <summary>
        /// Default is 0.
        /// </summary>
        Default = 0
    }
}
