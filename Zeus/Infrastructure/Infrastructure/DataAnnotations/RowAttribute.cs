using System;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to indicate the row a property belongs to.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class RowAttribute : Attribute
    {
        /// <summary>
        /// Name of row.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// The type of the row.
        /// </summary>
        public RowType RowType { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Employment.Web.Mvc.Infrastructure.DataAnnotations.RowAttribute" /> class.
        /// </summary>
        /// <param name="name">Name of group.</param>
        public RowAttribute(string name)
        {
            Name = name;
            RowType = RowType.Default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Employment.Web.Mvc.Infrastructure.DataAnnotations.RowAttribute" /> class.
        /// </summary>
        /// <param name="name">Name of row.</param>
        /// <param name="rowType">The type of the row.</param>
        public RowAttribute(string name, RowType rowType)
        {
            Name = name;
            RowType = rowType;
        }
    }
}
