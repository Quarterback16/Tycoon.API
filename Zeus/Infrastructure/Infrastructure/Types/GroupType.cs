namespace Employment.Web.Mvc.Infrastructure.Types
{
    /// <summary>
    /// Represents an enum which defines the group type.
    /// </summary>
    public enum GroupType
    {
        /// <summary>
        /// Group type is a fieldset.
        /// </summary>
        FieldSet,

        /// <summary>
        /// Group type is logical.
        /// </summary>
        /// <remarks>
        /// Properties are grouped like they're in a fieldset but without the actual fieldset.
        /// </remarks>
        Logical,

        /// <summary>
        /// Group type is a workflow.
        /// </summary>
        Workflow,

        /// <summary>
        /// Group type is of search criteria.
        /// </summary>
        //Search,

        /// <summary>
        /// Default group type is fieldset.
        /// </summary>
        Default = FieldSet
    }
}
