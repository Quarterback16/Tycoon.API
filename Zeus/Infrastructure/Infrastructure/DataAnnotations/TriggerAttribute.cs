using System;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to indicate the property should trigger a postback on value change.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class TriggerAttribute : Attribute
    {
        /// <summary>
        /// The submit type to use when triggering the postback.
        /// </summary>
        public string SubmitType { get; set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="TriggerAttribute" /> class.
        /// </summary>
        /// <param name="submitType">Submit type to use on postback.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="submitType"/> is <c>null</c> or empty.</exception>
        public TriggerAttribute(string submitType)
        {
            if (string.IsNullOrEmpty(submitType))
            {
                throw new ArgumentNullException("submitType");
            }

            SubmitType = submitType;
        }
    }
}
