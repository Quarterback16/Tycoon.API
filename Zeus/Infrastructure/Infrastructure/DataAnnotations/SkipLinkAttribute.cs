using System; 

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{

    /// <summary>
    /// Represents an attribute that is used to indicate whether to render Skip-link for the property which has been decorated with this attribute.
    /// Example: 'Skip to Search results' link will be rendered on top of the page clicking on which will take the user to the property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SkipLinkAttribute: Attribute
    {
        
    }
}
