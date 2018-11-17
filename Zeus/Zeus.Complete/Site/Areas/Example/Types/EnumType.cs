using System.ComponentModel;

namespace Employment.Web.Mvc.Area.Example.Types
{
    /// <summary>
    /// Represents an enum which defines the options for an enum example.
    /// </summary>
    /// <remarks>
    /// The default value will only be used in View Models where the enum property is defined as nullable.
    /// If you wish to set a default on a non nullable property, you must explicitly set the value of the first option to 1, so that an unselected value can be identified
    /// </remarks>
    [DefaultValue(Option2)]
    public enum EnumType
    {
        /// <summary>
        /// Option one.
        /// </summary>
        [Description("Option one")]
        Option1 = 1,

        /// <summary>
        /// Option two.
        /// </summary>
        [Description("Option two")]
        Option2,

        /// <summary>
        /// Option three.
        /// </summary>
        [Description("Option three")]
        Option3,

        /// <summary>
        /// Option four.
        /// </summary>
        [Description("Option four")]
        Option4,

        /// <summary>
        /// Option five.
        /// </summary>
        [Description("Option five")]
        Option5
    }
}