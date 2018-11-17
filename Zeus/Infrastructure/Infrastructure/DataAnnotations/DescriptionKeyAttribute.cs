using System;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to indicate the property in a Grid View Model that will be used as the key property for the accessibility description when it allows single or multiple selection. This property must be visible by the user.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DescriptionKeyAttribute : Attribute
    {

    }
}
