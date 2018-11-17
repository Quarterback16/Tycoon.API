using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to indicate whether a boolean property should be rendered as a swticher (i.e. on/off slider button)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class SwitcherAttribute : Attribute
    {
        /// <summary>
        /// The text to display in the switcher UI element when it is equivalent to the checkbox being 'Checked'
        /// </summary>
        public string CheckedText { get; set; }

        /// <summary>
        /// The text to display in the switcher UI element when it is equivalent to the checkbox being 'Unchecked'
        /// </summary>
        public string UncheckedText { get; set; }

        /// <summary>
        /// Creates a new instance of a SwitcherAttribute and sets the checked and unchecked properties to default values of "Yes" and "No"
        /// </summary>
        public SwitcherAttribute()
        {
            CheckedText = "Yes";
            UncheckedText = "No";
        }

        /// <summary>
        /// Creates a new instance of a SwitcherAttribute and sets the checked and unchecked properties to the values provided
        /// </summary>
        /// <param name="checkedText"></param>
        /// <param name="uncheckedText"></param>
        public SwitcherAttribute(string checkedText, string uncheckedText)
        {
            CheckedText = checkedText;
            UncheckedText = uncheckedText;
        }
    }
}
