using System.ComponentModel.DataAnnotations;

namespace Employment.Web.Mvc.Infrastructure.Types
{
    /// <summary>
    /// Represents a custom data type for use with the <see cref="DataTypeAttribute" />.
    /// </summary>
    public class CustomDataType
    {
        /// <summary>
        /// Represents a custom data type for a Grid.
        /// </summary>
        public const string Grid = "Grid";

        /// <summary>
        /// Represents a custom data type for a vertical Radio Button Group.
        /// </summary>
        public const string RadioButtonGroupVertical = "RadioButtonGroupVertical";

        /// <summary>
        /// Represents a custom data type for a horizontal Radio Button Group.
        /// </summary>
        public const string RadioButtonGroupHorizontal = "RadioButtonGroupHorizontal";

        /// <summary>
        /// Represents a custom data type for a check box list (lo.
        /// </summary>
        public const string CheckBoxList = "CheckBoxList";

        /// <summary>
        /// Represents a custom data type for an Editable Grid
        /// </summary>
        public const string GridEditable = "GridEditable";

    }
}
