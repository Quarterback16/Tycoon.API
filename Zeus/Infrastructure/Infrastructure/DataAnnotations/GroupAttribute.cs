using System;
using System.ComponentModel;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to indicate the details of a group for display in a view.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class GroupAttribute : ContingentAttribute
    {
        /// <summary>
        /// The type ID of this attribute.
        /// </summary>
        /// <remarks>
        /// When <see cref="AttributeUsageAttribute.AllowMultiple" /> is true, <see cref="TypeId" /> must be overriden to return the current instance so <see cref="TypeDescriptor" /> can properly return multiple attributes of the same type.
        /// </remarks>
        public override object TypeId { get { return this; } }

        /// <summary>
        /// Name of group.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of group.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Order in which the button should appear.
        /// </summary>
        /// <remarks>
        /// Defaults to <see cref="int.MaxValue" /> if not set.
        /// </remarks>
        public int Order
        {
            get
            {
                return order.HasValue ? order.Value : int.MaxValue;
            }
            set
            {
                order = value;
            }
        }

        private int? order;

        /// <summary>
        /// The type of the group.
        /// </summary>
        public GroupType GroupType { get; set; }

        /// <summary>
        /// The row-type for the group.
        /// </summary>
        public GroupRowType RowType { get; set; }

        /// <summary>
        /// Name for rows for groups.
        /// </summary>
        public string RowName { get; set; }

        /// <summary>
        /// If set, will override the group name with the value in the specified property.
        /// </summary>
        public string OverrideNameWithPropertyValue { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Employment.Web.Mvc.Infrastructure.DataAnnotations.GroupAttribute" /> class.
        /// </summary>
        /// <param name="name">Name of group.</param> 
        public GroupAttribute(string name)
        {
            Name = name;
            GroupType = GroupType.Default; 
            RowType = GroupRowType.Default;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Employment.Web.Mvc.Infrastructure.DataAnnotations.GroupAttribute" /> class.
        /// </summary>
        /// <param name="name">Name of group.</param>
        /// <param name="rowName">Name of the row.</param>
        /// <param name="groupRowType">Type of the Row.</param>
        public GroupAttribute(string name, string rowName, GroupRowType groupRowType)
        {
            Name = name;
            GroupType = GroupType.Default;
            RowName = rowName;
            RowType = groupRowType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Employment.Web.Mvc.Infrastructure.DataAnnotations.GroupAttribute" /> class.
        /// </summary>
        /// <param name="name">Name of group.</param>
        /// <param name="rowName">Name of the row.</param>
        /// <param name="groupRowType">Type of the Row.</param>
        /// <param name="groupType">Type for the group.</param>
        public GroupAttribute(string name, string rowName, GroupRowType groupRowType, GroupType groupType) : this(name, rowName, groupRowType)
        { 
            GroupType = groupType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Employment.Web.Mvc.Infrastructure.DataAnnotations.GroupAttribute" /> class.
        /// </summary>
        /// <param name="name">Name of group.</param>
        /// <param name="dependencyType">The type of action to take if the dependency condition is met.</param>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        /// <param name="comparisonType">The comparison type to use.</param>
        /// <param name="dependentValue">The value to compare against.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is <c>null</c> or empty.</exception>
        public GroupAttribute(string name, ActionForDependencyType dependencyType, string dependentProperty, ComparisonType comparisonType, object dependentValue) : base(dependencyType, dependentProperty, comparisonType, dependentValue)
        {
            Name = name; 
            GroupType = GroupType.Default;
            RowType = GroupRowType.Default;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Employment.Web.Mvc.Infrastructure.DataAnnotations.GroupAttribute" /> class.
        /// </summary>
        /// <param name="name">Name of group.</param>
        /// <param name="rowName"></param>
        /// <param name="groupRowType">Group Row type.</param>
        /// <param name="dependencyType">The type of action to take if the dependency condition is met.</param>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        /// <param name="comparisonType">The comparison type to use.</param>
        /// <param name="dependentValue">The value to compare against.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is <c>null</c> or empty.</exception>
        public GroupAttribute(string name, string rowName, GroupRowType groupRowType, ActionForDependencyType dependencyType, string dependentProperty, ComparisonType comparisonType, object dependentValue)
            : this(name, dependencyType, dependentProperty, comparisonType, dependentValue)
        {
            Name = name;
            RowName = rowName;
            GroupType = GroupType.Default;
            RowType = groupRowType;
        }
    }
}
