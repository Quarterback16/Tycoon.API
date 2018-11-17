using System;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to provide an alias of the property name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AliasAttribute : Attribute
    {
        /// <summary>
        /// Alias name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AliasAttribute" /> class.
        /// </summary>
        /// <param name="name">The alias name.</param>
        public AliasAttribute(string name)
        {
            Name = name;
        }
    }
}
