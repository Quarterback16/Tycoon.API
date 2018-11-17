using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Employment.Web.Mvc.Infrastructure.Interfaces
{
    /// <summary>
    /// Interface required for an item to be able to be added as a child element for a Split Button
    /// </summary>
    public interface ISplitButtonChild
    {
        /// <summary>
        /// Used to order elements within the split button group
        /// </summary>
        int Order { get; }

        /// <summary>
        /// Used to define the name of the button element that is the split button default
        /// </summary>
        string SplitButtonParent { get; }

        /// <summary>
        /// Returns an HTML string that represents this particular element.
        /// The return value of this method will be wrapped in an HTML list by the framework.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        MvcHtmlString Render(HtmlHelper html);
    }
}
