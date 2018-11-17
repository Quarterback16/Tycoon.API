using System;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to indicate the HTML access key of a property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AccessKeyAttribute : Attribute
    {
        /// <summary>
        /// Access key.
        /// </summary>
        public char Key { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessKeyAttribute" /> class.
        /// </summary>
        /// <param name="key">The access key.</param>
        public AccessKeyAttribute(char key)
        {
            Key = key;
        }
    }
}
