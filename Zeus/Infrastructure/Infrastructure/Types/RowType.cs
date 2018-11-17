namespace Employment.Web.Mvc.Infrastructure.Types
{
    /// <summary>
    /// Represents an enum which defines the row type.
    /// </summary>
    public enum RowType
    {
        /// <summary>
        /// Row items are added as they are and sized evenly based on their content.
        /// </summary>
        Flow,

        /// <summary>
        /// Row items are set at half width (should have two items).
        /// </summary>
        Half,

        /// <summary>
        /// Row items are set at third width (should have three items).
        /// </summary>
        Third,

        /// <summary>
        /// Row items are set at quarter width (should have four items).
        /// </summary>
        Quarter,

        /// <summary>
        /// Row items are set at fifth width (should have five items).
        /// </summary>
        //Fifth,

        /// <summary>
        /// Row items are added as they are and sized dynamically based on their content.
        /// </summary>
        Dynamic,

        /// <summary>
        /// Default row type is flow.
        /// </summary>
        Default = Flow
    }
}
