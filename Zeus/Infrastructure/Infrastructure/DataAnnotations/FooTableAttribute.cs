using System; 

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{

    /// <summary>
    /// Represents an attribute that is used to indicate whether to render a Table as a FooTable
    /// Example: 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FooTableAttribute : Attribute
    {
       
    }

   

    



}
