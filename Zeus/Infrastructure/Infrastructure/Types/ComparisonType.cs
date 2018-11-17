using System.ComponentModel;

namespace Employment.Web.Mvc.Infrastructure.Types
{
    /// <summary>
    /// Comparison types.
    /// </summary>
    public enum ComparisonType
    {
        /// <summary>
        /// Equal to
        /// </summary>
        [Description("equal to")]
        EqualTo,

        /// <summary>
        /// Not equal to
        /// </summary>
        [Description("not equal to")]
        NotEqualTo,

        /// <summary>
        /// Greater than
        /// </summary>
        [Description("greater than")]
        GreaterThan,

        /// <summary>
        /// Less than
        /// </summary>
        [Description("less than")]
        LessThan,

        /// <summary>
        /// Greater than or equal
        /// </summary>
        [Description("greater than or equal to")]
        GreaterThanOrEqualTo,

        /// <summary>
        /// Less than or equal to
        /// </summary>
        [Description("less than or equal to")]
        LessThanOrEqualTo,

        /// <summary>
        /// Regular expression match
        /// </summary>
        [Description("a match to")]
        RegExMatch,

        /// <summary>
        /// Negated regular expression match
        /// </summary>
        [Description("not a match to")]
        NotRegExMatch
    }
}
