using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// 
    /// </summary>
    public class ViewFormatAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string FormatString
        {
            get
            {
                return this.formatString;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected string formatString = string.Empty;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="allowEdit"></param>
        /// <param name="dataType"></param>
        public ViewFormatAttribute(string formatString)
        {
            this.formatString = formatString;

        }

    }
}
