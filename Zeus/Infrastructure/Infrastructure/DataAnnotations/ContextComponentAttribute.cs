using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ContextComponentAttribute : Attribute
    {
        public string KeyString { get; private set; }
        public ContextComponentAttribute(string keyString)
        {
            KeyString = keyString;
        }

        // This is needed to make the framework recognise the difference between multiple attributes. Otherwise they are all treated as the same thing.
        public override object TypeId
        {
            get
            {
                return this;
            }
        }

        public static Dictionary<string, Func<HtmlHelper, object, MvcHtmlString>> RegisteredRenderers = new Dictionary<string,Func<HtmlHelper, object, MvcHtmlString>>();
    }
}
