using System;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to indicate the property is an ID, such as Job Seeker ID.
    /// This is used to render ID properties appropriately. That is, normally numeric properties are
    /// rendered as left-aligned, while numeric ID properties should be rendered as right-aligned.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IdAttribute : Attribute
    {

    }
}
