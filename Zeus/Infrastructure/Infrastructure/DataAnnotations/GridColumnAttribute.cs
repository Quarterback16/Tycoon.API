using System;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to indicate the details of a grid column property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class GridColumnAttribute : Attribute
    {
        /// <summary>
        /// Width of the column as a percentage (1 to 100).
        /// </summary>
        public int Width { get; protected set; }

        /// <summary>
        /// Whether the column value should be wrapped if too long.
        /// </summary>
        public bool Wrap { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GridColumnAttribute" /> class.
        /// </summary>
        /// <param name="width">Width of the column as a percentage (1 to 100).</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="width" /> is not within range (1 to 100).</exception>
        public GridColumnAttribute(int width)
        {
            if (width < 1 || width > 100)
            {
                throw new ArgumentOutOfRangeException("width", "Width must be a percentage value from 1 to 100.");
            }

            Width = width;
        }
    }
}
