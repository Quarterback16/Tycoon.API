using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Employment.Web.Mvc.Infrastructure.Extensions
{
    /// <summary>
    /// Extensions for <see cref="MvcHtmlString"/>.
    /// </summary>
    public static class MvcHtmlStringExtension
    {
        /// <summary>
        /// Concatenates one or more <see cref="MvcHtmlString"/> objects.
        /// </summary>
        /// <param name="mvcHtmlString">A <see cref="MvcHtmlString"/> object.</param>
        /// <param name="mvcHtmlStrings">The <see cref="MvcHtmlString"/> objects to concatenate with.</param>
        /// <returns>The concatenated <see cref="MvcHtmlString"/>.</returns>
        public static MvcHtmlString Concat(this MvcHtmlString mvcHtmlString, params MvcHtmlString[] mvcHtmlStrings)
        {
            return MvcHtmlString.Create(string.Concat(mvcHtmlString.ToString(), string.Concat(mvcHtmlStrings.Select(s => s.ToString()))));
        }
    }
}
